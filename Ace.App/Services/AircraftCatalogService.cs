using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Utilities;

namespace Ace.App.Services
{
    public class AircraftInfo
    {
        public string Title { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int CrewCount { get; set; }
        public int PassengerCapacity { get; set; }
        public double MaxCargoKg { get; set; }
        public string Description { get; set; } = string.Empty;
        public double CruiseSpeedKts { get; set; }
        public double MaxRangeNM { get; set; }
        public double FuelCapacityGal { get; set; }
        public double FuelBurnGalPerHour { get; set; }
        public decimal HourlyOperatingCost { get; set; }
        public string? ThumbnailSourcePath { get; set; }
    }

    public class AircraftCatalogService : IAircraftCatalogService
    {
        private readonly ILoggingService _loggingService;
        private List<AircraftInfo> _availableAircraft = new();
        private readonly object _lock = new();

        public IReadOnlyList<AircraftInfo> AvailableAircraft
        {
            get { lock (_lock) return _availableAircraft.AsReadOnly(); }
        }

        public AircraftCatalogService(ILoggingService loggingService)
        {
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        }

        public void LoadAvailableAircraft()
        {
            _loggingService.Info("AircraftCatalog: Starting scan for available aircraft");

            var aircraft = new List<AircraftInfo>();
            var searchPaths = GetMsfsPackagePaths();

            foreach (var basePath in searchPaths)
            {
                if (!Directory.Exists(basePath))
                {
                    _loggingService.Debug($"AircraftCatalog: Path not found: {basePath}");
                    continue;
                }

                _loggingService.Info($"AircraftCatalog: Scanning {basePath}");
                ScanDirectory(basePath, aircraft);
            }

            lock (_lock)
            {
                _availableAircraft = aircraft.OrderBy(a => a.Manufacturer).ThenBy(a => a.Title).ToList();
            }

            _loggingService.Info($"AircraftCatalog: Found {_availableAircraft.Count} aircraft");

            UpdateCatalogDatabase();
        }

        private void UpdateCatalogDatabase()
        {
            _loggingService.Info("AircraftCatalog: Updating database catalog");

            try
            {
                using var db = new AceDbContext();
                db.Database.EnsureCreated();

                var now = DateTime.Now;
                int catalogAddedCount = 0;
                int catalogUpdatedCount = 0;
                int thumbnailsCopied = 0;

                var existingCatalogEntries = db.AircraftCatalog.ToList();

                foreach (var aircraft in _availableAircraft)
                {
                    var existingCatalog = existingCatalogEntries.FirstOrDefault(a => a.Title == aircraft.Title);

                    if (existingCatalog == null)
                    {
                        var copiedImagePath = CopyThumbnailToTemplate(aircraft);

                        var newEntry = new AircraftCatalogEntry
                        {
                            Title = aircraft.Title,
                            Manufacturer = aircraft.Manufacturer,
                            Type = aircraft.Type,
                            Category = aircraft.Category,
                            CrewCount = aircraft.CrewCount,
                            PassengerCapacity = aircraft.PassengerCapacity,
                            MaxCargoKg = aircraft.MaxCargoKg,
                            MarketPrice = 100000m,
                            CruiseSpeedKts = aircraft.CruiseSpeedKts,
                            MaxRangeNM = aircraft.MaxRangeNM,
                            FuelCapacityGal = aircraft.FuelCapacityGal,
                            FuelBurnGalPerHour = aircraft.FuelBurnGalPerHour,
                            HourlyOperatingCost = aircraft.HourlyOperatingCost,
                            FirstSeen = now,
                            LastSeen = now,
                            CustomImagePath = copiedImagePath
                        };

                        if (copiedImagePath != null) thumbnailsCopied++;

                        db.AircraftCatalog.Add(newEntry);
                        existingCatalogEntries.Add(newEntry);
                        catalogAddedCount++;
                        _loggingService.Info($"AircraftCatalog: Added new entry: {aircraft.Title}");
                    }
                    else
                    {
                        existingCatalog.LastSeen = now;
                        existingCatalog.Manufacturer = aircraft.Manufacturer;
                        existingCatalog.Type = aircraft.Type;
                        existingCatalog.Category = aircraft.Category;
                        existingCatalog.CrewCount = aircraft.CrewCount;

                        if (string.IsNullOrEmpty(existingCatalog.CustomImagePath) && !string.IsNullOrEmpty(aircraft.ThumbnailSourcePath))
                        {
                            var copiedImagePath = CopyThumbnailToTemplate(aircraft);
                            if (copiedImagePath != null)
                            {
                                existingCatalog.CustomImagePath = copiedImagePath;
                                thumbnailsCopied++;
                            }
                        }

                        catalogUpdatedCount++;
                    }
                }

                db.SaveChanges();
                _loggingService.Info($"AircraftCatalog: Sync complete - {catalogAddedCount} new, {catalogUpdatedCount} updated, {thumbnailsCopied} thumbnails copied");

                var similarImagesCopied = FillMissingImagesFromSimilarAircraft(db);
                if (similarImagesCopied > 0)
                {
                    _loggingService.Info($"AircraftCatalog: Filled {similarImagesCopied} missing images from similar aircraft");
                }

                ApplyAircraftSpecsFromScript(db);
            }
            catch (Exception ex)
            {
                _loggingService.Error($"AircraftCatalog: Failed to update database: {ex.Message}");
            }
        }

        private int FillMissingImagesFromSimilarAircraft(AceDbContext db)
        {
            var allEntries = db.AircraftCatalog.ToList();
            var entriesWithImages = allEntries.Where(e => !string.IsNullOrEmpty(e.CustomImagePath)).ToList();
            var entriesWithoutImages = allEntries.Where(e => string.IsNullOrEmpty(e.CustomImagePath)).ToList();

            int filled = 0;

            foreach (var entry in entriesWithoutImages)
            {
                var normalizedTitle = NormalizeAircraftTitle(entry.Title);

                var match = entriesWithImages
                    .Where(e => NormalizeAircraftTitle(e.Title) == normalizedTitle)
                    .FirstOrDefault();

                if (match != null)
                {
                    entry.CustomImagePath = match.CustomImagePath;
                    filled++;
                    _loggingService.Debug($"AircraftCatalog: '{entry.Title}' using image from '{match.Title}'");
                    continue;
                }

                match = entriesWithImages
                    .Where(e => e.Manufacturer == entry.Manufacturer &&
                                NormalizeAircraftTitle(e.Title).StartsWith(normalizedTitle.Substring(0, Math.Min(normalizedTitle.Length, 4))))
                    .OrderBy(e => e.Title.Length)
                    .FirstOrDefault();

                if (match != null)
                {
                    entry.CustomImagePath = match.CustomImagePath;
                    filled++;
                    _loggingService.Debug($"AircraftCatalog: '{entry.Title}' using similar image from '{match.Title}'");
                }
            }

            if (filled > 0)
            {
                db.SaveChanges();
            }

            return filled;
        }

        private static string NormalizeAircraftTitle(string title)
        {
            var normalized = title
                .Replace("Cessna ", "")
                .Replace("Beechcraft ", "")
                .Replace("Piper ", "")
                .Replace("Boeing ", "")
                .Replace("Airbus ", "")
                .Replace(" - ", " ")
                .Replace("-", "")
                .Trim();

            if (normalized.Length > 1 && char.IsLetter(normalized[0]) && char.IsDigit(normalized[1]))
            {
                normalized = normalized.Substring(1);
            }

            var spaceIndex = normalized.IndexOf(' ');
            if (spaceIndex > 0 && spaceIndex < normalized.Length - 1)
            {
                var afterSpace = normalized.Substring(spaceIndex + 1);
                if (afterSpace.StartsWith("Aerobat") || afterSpace.StartsWith("Turbo") ||
                    afterSpace.StartsWith("Float") || afterSpace.StartsWith("Ski") ||
                    afterSpace.StartsWith("HP") || afterSpace.StartsWith("II") ||
                    afterSpace.StartsWith("III") || afterSpace.StartsWith("Modular"))
                {
                    normalized = normalized.Substring(0, spaceIndex);
                }
            }

            return normalized.ToLowerInvariant();
        }

        private void ApplyAircraftSpecsFromScript(AceDbContext db)
        {
            try
            {
                var dataDir = PathUtilities.GetDataDirectory();
                var scriptPath = Path.Combine(dataDir, "update_aircraft_specs.sql");
                if (!File.Exists(scriptPath))
                {
                    _loggingService.Debug($"AircraftCatalog: Specs script not found: {scriptPath}");
                    return;
                }

                var sql = File.ReadAllText(scriptPath).Replace("\r\n", "\n").Replace("\r", "\n");

                db.ChangeTracker.Clear();

                var connection = db.Database.GetDbConnection();
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                var statements = sql.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                int updatedCount = 0;

                using var transaction = connection.BeginTransaction();

                foreach (var statement in statements)
                {
                    var lines = statement.Split('\n')
                        .Select(l => l.Trim())
                        .Where(l => !string.IsNullOrEmpty(l) && !l.StartsWith("--"))
                        .ToList();

                    if (lines.Count == 0) continue;
                    var trimmed = string.Join(" ", lines);

                    try
                    {
                        using var cmd = connection.CreateCommand();
                        cmd.CommandText = trimmed;
                        cmd.Transaction = transaction;
                        var affected = cmd.ExecuteNonQuery();
                        if (affected > 0) updatedCount += affected;
                    }
                    catch (Exception ex)
                    {
                        _loggingService.Debug($"AircraftCatalog: SQL error: {ex.Message}");
                    }
                }

                transaction.Commit();

                _loggingService.Info($"AircraftCatalog: Applied specs script - {updatedCount} aircraft updated");
            }
            catch (Exception ex)
            {
                _loggingService.Error($"AircraftCatalog: Failed to apply specs script: {ex.Message}");
            }
        }

        private string? CopyThumbnailToTemplate(AircraftInfo aircraft)
        {
            if (string.IsNullOrEmpty(aircraft.ThumbnailSourcePath) || !File.Exists(aircraft.ThumbnailSourcePath))
            {
                return null;
            }

            try
            {
                var templateDir = PathUtilities.GetTemplateAircraftImagesDirectory();
                Directory.CreateDirectory(templateDir);

                var displayName = string.IsNullOrEmpty(aircraft.Manufacturer)
                    ? aircraft.Title
                    : $"{aircraft.Manufacturer}_{aircraft.Title}";

                var safeFileName = MakeSafeFileName(displayName);
                var extension = Path.GetExtension(aircraft.ThumbnailSourcePath);
                var targetFileName = $"{safeFileName}{extension}";
                var targetPath = Path.Combine(templateDir, targetFileName);

                if (!File.Exists(targetPath))
                {
                    File.Copy(aircraft.ThumbnailSourcePath, targetPath, overwrite: false);
                    _loggingService.Debug($"AircraftCatalog: Copied thumbnail for {aircraft.Title}: {targetFileName}");
                }

                return targetFileName;
            }
            catch (Exception ex)
            {
                _loggingService.Debug($"AircraftCatalog: Failed to copy thumbnail for {aircraft.Title}: {ex.Message}");
                return null;
            }
        }

        private static string MakeSafeFileName(string input)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var safe = input;
            foreach (var c in invalid)
            {
                safe = safe.Replace(c, '_');
            }
            return safe.Replace(' ', '_');
        }

        private List<string> GetMsfsPackagePaths()
        {
            var paths = new List<string>();

            _loggingService.Info("AircraftCatalog: Searching for MSFS installation paths...");

            var possiblePaths = new List<string>
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages", "Microsoft.FlightSimulator_8wekyb3d8bbwe", "LocalCache", "Packages"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages", "Microsoft.Limitless_8wekyb3d8bbwe", "LocalCache", "Packages"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft Flight Simulator", "Packages"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MSFSPackages"),
                "C:\\Program Files\\WindowsApps\\Microsoft.FlightSimulator_1.0.0.0_x64__8wekyb3d8bbwe\\Packages",
                "C:\\Program Files (x86)\\Steam\\steamapps\\common\\MicrosoftFlightSimulator\\Packages",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents", "Microsoft Flight Simulator 2024", "Packages"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft Flight Simulator 2024", "Packages"),
            };

            foreach (var basePath in possiblePaths)
            {
                if (Directory.Exists(basePath))
                {
                    _loggingService.Info($"AircraftCatalog: Found: {basePath}");
                    bool foundSubdirs = false;

                    var officialDirNames = new[] { "Official", "Official2024", "Official2020" };
                    foreach (var officialName in officialDirNames)
                    {
                        var officialDir = Path.Combine(basePath, officialName);
                        if (Directory.Exists(officialDir))
                        {
                            foreach (var subDir in Directory.GetDirectories(officialDir))
                            {
                                paths.Add(subDir);
                                foundSubdirs = true;
                            }
                        }
                    }

                    var communityDirNames = new[] { "Community", "Community2024" };
                    foreach (var communityName in communityDirNames)
                    {
                        var communityDir = Path.Combine(basePath, communityName);
                        if (Directory.Exists(communityDir))
                        {
                            paths.Add(communityDir);
                            foundSubdirs = true;
                        }
                    }

                    if (!foundSubdirs)
                    {
                        paths.Add(basePath);
                    }
                }
            }

            _loggingService.Info($"AircraftCatalog: Total paths to scan: {paths.Count}");
            return paths;
        }

        private void ScanDirectory(string path, List<AircraftInfo> aircraft)
        {
            try
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    ScanAircraftPackage(dir, aircraft);
                }
            }
            catch (Exception ex)
            {
                _loggingService.Warn($"AircraftCatalog: Error scanning {path}: {ex.Message}");
            }
        }

        private void ScanAircraftPackage(string packagePath, List<AircraftInfo> aircraft)
        {
            try
            {
                var manifestPath = Path.Combine(packagePath, "manifest.json");
                if (File.Exists(manifestPath))
                {
                    var info = ParseManifestJson(manifestPath);
                    if (info != null && !string.IsNullOrWhiteSpace(info.Title))
                    {
                        var simObjectsPath = Path.Combine(packagePath, "SimObjects", "Airplanes");
                        if (Directory.Exists(simObjectsPath))
                        {
                            var aircraftDirs = Directory.GetDirectories(simObjectsPath);
                            if (aircraftDirs.Length > 0)
                            {
                                ParsePerformanceData(aircraftDirs[0], info);
                            }
                        }
                        aircraft.Add(info);
                        return;
                    }
                }

                var simObjectsPathAlt = Path.Combine(packagePath, "SimObjects", "Airplanes");
                if (!Directory.Exists(simObjectsPathAlt)) return;

                foreach (var aircraftDir in Directory.GetDirectories(simObjectsPathAlt))
                {
                    var aircraftCfg = Path.Combine(aircraftDir, "aircraft.cfg");
                    if (File.Exists(aircraftCfg))
                    {
                        var info = ParseAircraftCfg(aircraftCfg);
                        if (info != null && !string.IsNullOrWhiteSpace(info.Title))
                        {
                            ParsePerformanceData(aircraftDir, info);
                            aircraft.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _loggingService.Debug($"AircraftCatalog: Error scanning package {packagePath}: {ex.Message}");
            }
        }

        private void ParsePerformanceData(string aircraftDir, AircraftInfo info)
        {
            try
            {
                var flightModelCfg = Path.Combine(aircraftDir, "flight_model.cfg");
                if (File.Exists(flightModelCfg))
                {
                    ParseFlightModelCfg(flightModelCfg, info);
                }

                var enginesCfg = Path.Combine(aircraftDir, "engines.cfg");
                if (File.Exists(enginesCfg))
                {
                    ParseEnginesCfg(enginesCfg, info);
                }

                var systemsCfg = Path.Combine(aircraftDir, "systems.cfg");
                if (File.Exists(systemsCfg))
                {
                    ParseSystemsCfg(systemsCfg, info);
                }

                info.ThumbnailSourcePath = FindThumbnail(aircraftDir);

                if (info.CruiseSpeedKts > 0 && info.FuelBurnGalPerHour > 0 && info.FuelCapacityGal > 0)
                {
                    var enduranceHours = info.FuelCapacityGal / info.FuelBurnGalPerHour;
                    var calculatedRange = info.CruiseSpeedKts * enduranceHours * 0.85;
                    if (info.MaxRangeNM == 0)
                    {
                        info.MaxRangeNM = calculatedRange;
                    }
                }

                if (info.FuelBurnGalPerHour > 0)
                {
                    var fuelCostPerGallon = 6.0m;
                    var baseCost = (decimal)info.FuelBurnGalPerHour * fuelCostPerGallon;
                    var maintenanceFactor = 1.5m;
                    info.HourlyOperatingCost = Math.Round(baseCost * maintenanceFactor, 0);
                }
            }
            catch (Exception ex)
            {
                _loggingService.Debug($"AircraftCatalog: Error parsing performance data for {info.Title}: {ex.Message}");
            }
        }

        private string? FindThumbnail(string aircraftDir)
        {
            var thumbnailPaths = new[]
            {
                Path.Combine(aircraftDir, "thumbnail", "thumbnail.png"),
                Path.Combine(aircraftDir, "thumbnail", "thumbnail.jpg"),
                Path.Combine(aircraftDir, "texture", "thumbnail.png"),
                Path.Combine(aircraftDir, "texture", "thumbnail.jpg"),
                Path.Combine(aircraftDir, "common", "thumbnail", "thumbnail.png"),
                Path.Combine(aircraftDir, "common", "thumbnail", "thumbnail.jpg"),
                Path.Combine(aircraftDir, "common", "texture", "thumbnail.png"),
                Path.Combine(aircraftDir, "common", "texture", "thumbnail.jpg"),
            };

            foreach (var path in thumbnailPaths)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }

            var liveriesDir = Path.Combine(aircraftDir, "liveries");
            if (Directory.Exists(liveriesDir))
            {
                foreach (var liveryDir in Directory.GetDirectories(liveriesDir, "*", SearchOption.AllDirectories))
                {
                    var thumbPath = Path.Combine(liveryDir, "thumbnail", "thumbnail.png");
                    if (File.Exists(thumbPath)) return thumbPath;
                }
            }

            var presetsDir = Path.Combine(aircraftDir, "presets");
            if (Directory.Exists(presetsDir))
            {
                foreach (var presetDir in Directory.GetDirectories(presetsDir, "*", SearchOption.AllDirectories))
                {
                    var thumbPath = Path.Combine(presetDir, "thumbnail", "thumbnail.png");
                    if (File.Exists(thumbPath)) return thumbPath;
                }
            }

            var textureDir = Path.Combine(aircraftDir, "texture");
            if (Directory.Exists(textureDir))
            {
                foreach (var subDir in Directory.GetDirectories(textureDir))
                {
                    var thumbPath = Path.Combine(subDir, "thumbnail.png");
                    if (File.Exists(thumbPath)) return thumbPath;

                    thumbPath = Path.Combine(subDir, "thumbnail.jpg");
                    if (File.Exists(thumbPath)) return thumbPath;
                }
            }

            try
            {
                var parentDir = Directory.GetParent(aircraftDir)?.FullName;
                if (parentDir != null)
                {
                    foreach (var siblingDir in Directory.GetDirectories(parentDir, "texture.*"))
                    {
                        var thumbPath = Path.Combine(siblingDir, "thumbnail.png");
                        if (File.Exists(thumbPath)) return thumbPath;

                        thumbPath = Path.Combine(siblingDir, "thumbnail.jpg");
                        if (File.Exists(thumbPath)) return thumbPath;
                    }
                }
            }
            catch
            {
            }

            return null;
        }

        private void ParseFlightModelCfg(string filePath, AircraftInfo info)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                string currentSection = "";

                foreach (var line in lines)
                {
                    var trimmed = line.Trim();
                    if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith(";")) continue;

                    if (trimmed.StartsWith("["))
                    {
                        currentSection = trimmed.ToLowerInvariant();
                        continue;
                    }

                    if (!trimmed.Contains("=")) continue;

                    var parts = trimmed.Split(new[] { '=' }, 2);
                    if (parts.Length != 2) continue;

                    var key = parts[0].Trim().ToLowerInvariant();
                    var valueStr = parts[1].Trim().Split(';')[0].Trim();

                    if (currentSection.Contains("reference_speeds"))
                    {
                        if (key == "cruise_speed" && double.TryParse(valueStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double cruiseSpeed))
                        {
                            info.CruiseSpeedKts = cruiseSpeed;
                        }
                    }

                    if (currentSection.Contains("flight_tuning"))
                    {
                        if (key == "cruise_speed" && info.CruiseSpeedKts == 0 && double.TryParse(valueStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double cruiseSpeedTuning))
                        {
                            info.CruiseSpeedKts = cruiseSpeedTuning;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _loggingService.Debug($"AircraftCatalog: Error parsing flight_model.cfg: {ex.Message}");
            }
        }

        private void ParseEnginesCfg(string filePath, AircraftInfo info)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                string currentSection = "";
                double totalFuelFlow = 0;
                int engineCount = 0;

                foreach (var line in lines)
                {
                    var trimmed = line.Trim();
                    if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith(";")) continue;

                    if (trimmed.StartsWith("["))
                    {
                        currentSection = trimmed.ToLowerInvariant();
                        continue;
                    }

                    if (!trimmed.Contains("=")) continue;

                    var parts = trimmed.Split(new[] { '=' }, 2);
                    if (parts.Length != 2) continue;

                    var key = parts[0].Trim().ToLowerInvariant();
                    var valueStr = parts[1].Trim().Split(';')[0].Trim();

                    if (key == "fuel_flow_scalar" && double.TryParse(valueStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double fuelFlow))
                    {
                        totalFuelFlow += fuelFlow;
                        engineCount++;
                    }

                    if (key == "fuel_consumption" && double.TryParse(valueStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double fuelConsumption))
                    {
                        if (info.FuelBurnGalPerHour == 0)
                        {
                            info.FuelBurnGalPerHour = fuelConsumption;
                        }
                    }
                }

                if (engineCount > 0 && totalFuelFlow > 0 && info.FuelBurnGalPerHour == 0)
                {
                    info.FuelBurnGalPerHour = totalFuelFlow * 10;
                }
            }
            catch (Exception ex)
            {
                _loggingService.Debug($"AircraftCatalog: Error parsing engines.cfg: {ex.Message}");
            }
        }

        private void ParseSystemsCfg(string filePath, AircraftInfo info)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                string currentSection = "";
                double totalFuelCapacity = 0;

                foreach (var line in lines)
                {
                    var trimmed = line.Trim();
                    if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith(";")) continue;

                    if (trimmed.StartsWith("["))
                    {
                        currentSection = trimmed.ToLowerInvariant();
                        continue;
                    }

                    if (!trimmed.Contains("=")) continue;

                    var parts = trimmed.Split(new[] { '=' }, 2);
                    if (parts.Length != 2) continue;

                    var key = parts[0].Trim().ToLowerInvariant();
                    var valueStr = parts[1].Trim().Split(';')[0].Trim();

                    if (currentSection.Contains("fuel_system") || currentSection.Contains("fuel"))
                    {
                        if ((key == "capacity" || key == "fuel_quantity") && double.TryParse(valueStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double capacity))
                        {
                            totalFuelCapacity += capacity;
                        }
                    }
                }

                if (totalFuelCapacity > 0)
                {
                    info.FuelCapacityGal = totalFuelCapacity;
                }
            }
            catch (Exception ex)
            {
                _loggingService.Debug($"AircraftCatalog: Error parsing systems.cfg: {ex.Message}");
            }
        }

        private AircraftInfo? ParseManifestJson(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (!root.TryGetProperty("content_type", out var contentType) ||
                    contentType.GetString() != "AIRCRAFT")
                {
                    return null;
                }

                var info = new AircraftInfo();

                if (root.TryGetProperty("title", out var title))
                    info.Title = title.GetString() ?? string.Empty;

                if (root.TryGetProperty("manufacturer", out var manufacturer))
                    info.Manufacturer = manufacturer.GetString() ?? string.Empty;

                if (root.TryGetProperty("creator", out var creator) && string.IsNullOrEmpty(info.Manufacturer))
                    info.Manufacturer = creator.GetString() ?? string.Empty;

                info.Category = "Aircraft";

                return string.IsNullOrWhiteSpace(info.Title) ? null : info;
            }
            catch (Exception ex)
            {
                _loggingService.Debug($"AircraftCatalog: Error parsing manifest {filePath}: {ex.Message}");
                return null;
            }
        }

        private AircraftInfo? ParseAircraftCfg(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                var info = new AircraftInfo();
                bool inGeneralSection = false;

                foreach (var line in lines)
                {
                    var trimmed = line.Trim();

                    if (trimmed.StartsWith("[FLTSIM.", StringComparison.OrdinalIgnoreCase))
                    {
                        inGeneralSection = true;
                        continue;
                    }

                    if (trimmed.StartsWith("[") && !trimmed.StartsWith("[FLTSIM.", StringComparison.OrdinalIgnoreCase))
                    {
                        inGeneralSection = false;
                    }

                    if (!inGeneralSection || !trimmed.Contains("=")) continue;

                    var parts = trimmed.Split(new[] { '=' }, 2);
                    if (parts.Length != 2) continue;

                    var key = parts[0].Trim().ToLowerInvariant();
                    var value = parts[1].Trim().Trim('"');

                    switch (key)
                    {
                        case "title":
                            info.Title = value;
                            break;
                        case "atc_type":
                            info.Type = value;
                            break;
                        case "atc_model":
                            info.Manufacturer = value;
                            break;
                        case "category":
                            info.Category = value;
                            break;
                        case "crew":
                            if (int.TryParse(value, out int crew)) info.CrewCount = crew;
                            break;
                        case "passenger":
                            if (int.TryParse(value, out int passengers)) info.PassengerCapacity = passengers;
                            break;
                        case "ui_manufacturer":
                            if (string.IsNullOrEmpty(info.Manufacturer)) info.Manufacturer = value;
                            break;
                        case "ui_type":
                            if (string.IsNullOrEmpty(info.Type)) info.Type = value;
                            break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(info.Title))
                {
                    return info;
                }
            }
            catch (Exception ex)
            {
                _loggingService.Debug($"AircraftCatalog: Error parsing {filePath}: {ex.Message}");
            }

            return null;
        }
    }
}
