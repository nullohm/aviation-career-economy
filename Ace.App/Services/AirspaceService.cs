using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Utilities;

namespace Ace.App.Services
{
    public class AirspaceService : IAirspaceService
    {
        private readonly ILoggingService _logger;
        private readonly ISettingsService _settingsService;
        private List<Airspace> _airspaces = new();
        private bool _isLoaded;
        private string? _dataSource;

        public bool HasAirspaceData => _airspaces.Count > 0;
        public string DataSource => _dataSource ?? "None";

        public AirspaceService(ILoggingService logger, ISettingsService settingsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        public List<Airspace> GetAllAirspaces()
        {
            if (!_isLoaded)
                LoadAirspaces();

            return _airspaces;
        }

        public void LoadAirspaces()
        {
            _airspaces = new List<Airspace>();
            _dataSource = null;

            if (TryLoadFromLittleNavmap())
            {
                _isLoaded = true;
                return;
            }

            LoadFromOpenAirFiles();
            _isLoaded = true;
        }

        private bool TryLoadFromLittleNavmap()
        {
            try
            {
                var dbPath = FindNavigraphDatabase();
                if (dbPath == null)
                {
                    _logger.Debug("AirspaceService: No Little Navmap Navigraph database found");
                    return false;
                }

                _airspaces = LoadAirspacesFromLnmDatabase(dbPath);
                if (_airspaces.Count > 0)
                {
                    _dataSource = $"Little Navmap ({Path.GetFileName(dbPath)})";
                    _logger.Info($"AirspaceService: Loaded {_airspaces.Count} airspaces from {dbPath}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"AirspaceService: Failed to load from Little Navmap: {ex.Message}");
            }

            return false;
        }

        private string? FindNavigraphDatabase()
        {
            return LittleNavmapPathResolver.FindNavigraphDatabase(_settingsService);
        }

        private List<Airspace> LoadAirspacesFromLnmDatabase(string dbPath)
        {
            var airspaces = new List<Airspace>();

            using var connection = new SqliteConnection($"Data Source={dbPath};Mode=ReadOnly");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT boundary_id, type, name, restrictive_designation,
                       min_altitude_type, min_altitude,
                       max_altitude_type, max_altitude,
                       geometry
                FROM boundary
                WHERE geometry IS NOT NULL
                  AND type NOT IN ('FIR', 'UIR', 'OCA', 'OCEANIC', 'CENTER')
                  AND (max_lonx - min_lonx) < 10
                  AND (max_laty - min_laty) < 10";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    var type = reader.GetString(1);
                    var name = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    var designation = reader.IsDBNull(3) ? "" : reader.GetString(3);

                    var minAltType = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    var minAlt = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                    var maxAltType = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    var maxAlt = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);

                    var geometryBlob = (byte[])reader.GetValue(8);

                    var airspace = new Airspace
                    {
                        Name = string.IsNullOrEmpty(name) ? designation : name,
                        Class = MapLnmTypeToAirspaceClass(type),
                        LowerAltitude = FormatAltitude(minAltType, minAlt),
                        UpperAltitude = FormatAltitude(maxAltType, maxAlt)
                    };

                    ParseGeometryBlob(geometryBlob, airspace);

                    if (airspace.Coordinates.Count >= 3 || airspace.IsCircle)
                    {
                        if (!IsProblematicAirspace(airspace))
                            airspaces.Add(airspace);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Debug($"AirspaceService: Skipping invalid airspace record: {ex.Message}");
                }
            }

            return airspaces;
        }

        private static AirspaceClass MapLnmTypeToAirspaceClass(string type)
        {
            return type.ToUpperInvariant() switch
            {
                "A" or "CA" => AirspaceClass.A,
                "B" or "CB" => AirspaceClass.B,
                "C" or "CC" => AirspaceClass.C,
                "CD" => AirspaceClass.D,
                "E" or "CE" => AirspaceClass.E,
                "F" or "CF" => AirspaceClass.F,
                "G" or "CG" => AirspaceClass.G,
                "CTR" or "T" => AirspaceClass.CTR,
                "TMA" => AirspaceClass.TMA,
                "ATZ" => AirspaceClass.ATZ,
                "RMZ" or "R" => AirspaceClass.RMZ,
                "TMZ" => AirspaceClass.TMZ,
                "P" or "PR" => AirspaceClass.Prohibited,
                "RE" or "MOA" => AirspaceClass.Restricted,
                "DA" => AirspaceClass.Danger,
                "D" => AirspaceClass.D,
                "W" or "GP" or "GS" => AirspaceClass.Glider,
                "WW" => AirspaceClass.Wave,
                _ => AirspaceClass.Other
            };
        }

        private static string FormatAltitude(string type, int altitude)
        {
            if (string.IsNullOrEmpty(type))
                return altitude == 0 ? "GND" : $"{altitude} ft";

            return type.ToUpperInvariant() switch
            {
                "G" or "GND" or "SFC" => "GND",
                "AGL" => altitude == 0 ? "GND" : $"{altitude} ft AGL",
                "MSL" => $"{altitude} ft",
                "FL" => $"FL{altitude / 100}",
                "U" or "UNL" or "UNLIMITED" => "UNL",
                _ => altitude == 0 ? type : $"{altitude} ft"
            };
        }

        private void ParseGeometryBlob(byte[] blob, Airspace airspace)
        {
            if (blob.Length < 4)
                return;

            int numPoints = (blob[0] << 24) | (blob[1] << 16) | (blob[2] << 8) | blob[3];

            if (numPoints <= 0 || blob.Length < 4 + numPoints * 8)
                return;

            int offset = 4;
            for (int i = 0; i < numPoints; i++)
            {
                float lon = ReadBigEndianFloat(blob, offset);
                float lat = ReadBigEndianFloat(blob, offset + 4);

                if (lon >= -180 && lon <= 180 && lat >= -90 && lat <= 90)
                {
                    airspace.Coordinates.Add(new AirspaceCoordinate(lat, lon));
                }

                offset += 8;
            }
        }

        private static float ReadBigEndianFloat(byte[] data, int offset)
        {
            if (BitConverter.IsLittleEndian)
            {
                var bytes = new byte[] { data[offset + 3], data[offset + 2], data[offset + 1], data[offset] };
                return BitConverter.ToSingle(bytes, 0);
            }
            return BitConverter.ToSingle(data, offset);
        }

        private static bool IsProblematicAirspace(Airspace airspace)
        {
            if (airspace.Coordinates.Count < 3)
                return true;

            var lons = airspace.Coordinates.Select(c => c.Longitude).ToList();
            var lats = airspace.Coordinates.Select(c => c.Latitude).ToList();

            var lonSpan = lons.Max() - lons.Min();
            var latSpan = lats.Max() - lats.Min();

            if (lonSpan > 5 || latSpan > 5)
                return true;

            if (lons.Any(l => l > 170) && lons.Any(l => l < -170))
                return true;

            for (int i = 1; i < airspace.Coordinates.Count; i++)
            {
                var dlon = Math.Abs(airspace.Coordinates[i].Longitude - airspace.Coordinates[i - 1].Longitude);
                var dlat = Math.Abs(airspace.Coordinates[i].Latitude - airspace.Coordinates[i - 1].Latitude);
                if (dlon > 2 || dlat > 2)
                    return true;
            }

            return false;
        }

        private void LoadFromOpenAirFiles()
        {
            try
            {
                var directory = PathUtilities.GetAirspacesDirectory();

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    _logger.Info($"AirspaceService: Created airspaces directory at {directory}");
                    return;
                }

                _airspaces = OpenAirParser.ParseDirectory(directory);
                if (_airspaces.Count > 0)
                {
                    _dataSource = "OpenAir files";
                    _logger.Info($"AirspaceService: Loaded {_airspaces.Count} airspaces from OpenAir files in {directory}");
                }
                else
                {
                    _logger.Info($"AirspaceService: No airspaces found in {directory}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"AirspaceService: Failed to load OpenAir files: {ex.Message}");
            }
        }

        public void RefreshAirspaces()
        {
            _isLoaded = false;
            LoadAirspaces();
        }
    }
}
