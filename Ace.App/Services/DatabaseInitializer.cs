using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ace.App.Data;
using Ace.App.Models;
using Ace.App.Utilities;
using Ace.App.Interfaces;

namespace Ace.App.Services
{
    public class DatabaseInitializer
    {
        private readonly ILoggingService _logger;

        public DatabaseInitializer(ILoggingService logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Initialize(AceDbContext context)
        {
            _logger.Database("Starting initialization");

            try
            {
                var dbPath = context.Database.GetDbConnection().DataSource;
                var templatePath = PathUtilities.GetTemplateDbPath();

                if (!File.Exists(dbPath))
                {
                    if (File.Exists(templatePath))
                    {
                        var dbDir = Path.GetDirectoryName(dbPath);
                        if (!string.IsNullOrEmpty(dbDir) && !Directory.Exists(dbDir))
                        {
                            Directory.CreateDirectory(dbDir);
                        }

                        File.Copy(templatePath, dbPath);
                        _logger.Database($"Copied template.db to savegame: {dbPath}");
                        EnsureSchemaUpToDate(context);
                    }
                    else
                    {
                        _logger.Warn($"Template database not found at {templatePath}, creating new database...");
                        context.Database.EnsureCreated();
                    }
                }
                else
                {
                    _logger.Database("Existing database found, ensuring schema is up-to-date...");
                    EnsureSchemaUpToDate(context);
                }
                _logger.Database("Database ready");
            }
            catch (Exception ex)
            {
                _logger.Error($"Database creation error: {ex.Message}");
                _logger.Debug($"Database creation stack trace: {ex.StackTrace}");
                throw;
            }

            UpdateExistingAircraftCategories(context);
            SyncServiceCeilingFromCatalog(context);

            var count = context.Aircraft.Count();
            _logger.Database("Aircraft count query", count);
        }

        private void EnsureSchemaUpToDate(AceDbContext context)
        {
            var connection = context.Database.GetDbConnection();
            connection.Open();

            _logger.Database("Checking for missing columns and tables...");

            AddColumnIfNotExists(connection, "Aircraft", "CustomImagePath", "TEXT");
            AddColumnIfNotExists(connection, "AircraftCatalog", "CustomImagePath", "TEXT");
            AddColumnIfNotExists(connection, "Aircraft", "ServiceCeilingFt", "INTEGER DEFAULT 0");
            AddColumnIfNotExists(connection, "AircraftCatalog", "ServiceCeilingFt", "INTEGER DEFAULT 0");

            CreateTableIfNotExists(connection, "ScheduledRoutes", @"
                CREATE TABLE ScheduledRoutes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    OriginFBOId INTEGER NOT NULL,
                    DestinationFBOId INTEGER NOT NULL,
                    AssignedAircraftId INTEGER,
                    CreatedDate TEXT NOT NULL,
                    IsActive INTEGER NOT NULL DEFAULT 1
                )");

            AddColumnIfNotExists(connection, "Settings", "ScheduledRouteBonusPercent", "REAL DEFAULT 30");
            AddColumnIfNotExists(connection, "Settings", "RouteSlotLimitLocal", "INTEGER DEFAULT 2");
            AddColumnIfNotExists(connection, "Settings", "RouteSlotLimitRegional", "INTEGER DEFAULT 5");
            AddColumnIfNotExists(connection, "Settings", "RouteSlotLimitInternational", "INTEGER DEFAULT 10");
            AddColumnIfNotExists(connection, "Settings", "RoutesPerFBOPairLimit", "INTEGER DEFAULT 2");

            AddColumnIfNotExists(connection, "DailyEarningsDetails", "DistanceNM", "REAL DEFAULT 0");
            AddColumnIfNotExists(connection, "DailyEarningsDetails", "ServiceBonusAmount", "REAL DEFAULT 0");

            AddColumnIfNotExists(connection, "Pilots", "TotalDistanceNM", "REAL DEFAULT 0");

            CreateTableIfNotExists(connection, "Achievements", @"
                CREATE TABLE Achievements (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Key TEXT NOT NULL UNIQUE,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    Category INTEGER NOT NULL,
                    Tier INTEGER NOT NULL,
                    Icon TEXT DEFAULT '🏆',
                    IsUnlocked INTEGER NOT NULL DEFAULT 0,
                    UnlockedDate TEXT,
                    Progress INTEGER NOT NULL DEFAULT 0,
                    Target INTEGER NOT NULL DEFAULT 1,
                    Reward REAL
                )");

            AddColumnIfNotExists(connection, "Settings", "AllowAllAircraftForFlightPlan", "INTEGER DEFAULT 0");
            AddColumnIfNotExists(connection, "Settings", "LastSelectedAircraftRegistration", "TEXT DEFAULT ''");

            AddColumnIfNotExists(connection, "Settings", "SoundFlightCompletedEnabled", "INTEGER DEFAULT 1");
            AddColumnIfNotExists(connection, "Settings", "SoundAchievementEnabled", "INTEGER DEFAULT 1");
            AddColumnIfNotExists(connection, "Settings", "SoundTopOfDescentEnabled", "INTEGER DEFAULT 1");
            AddColumnIfNotExists(connection, "Settings", "SoundWarningEnabled", "INTEGER DEFAULT 1");
            AddColumnIfNotExists(connection, "Settings", "SoundNotificationEnabled", "INTEGER DEFAULT 1");
            AddColumnIfNotExists(connection, "Settings", "SoundButtonClickEnabled", "INTEGER DEFAULT 0");

            AddColumnIfNotExists(connection, "Settings", "ShowAirspaceOverlay", "INTEGER DEFAULT 0");
            AddColumnIfNotExists(connection, "Settings", "ShowAirports", "INTEGER DEFAULT 1");

            AddColumnIfNotExists(connection, "Aircraft", "IsOldtimer", "INTEGER DEFAULT 0");
            AddColumnIfNotExists(connection, "AircraftCatalog", "IsOldtimer", "INTEGER DEFAULT 0");

            AddColumnIfNotExists(connection, "Flights", "AircraftTitle", "TEXT DEFAULT ''");

            AddColumnIfNotExists(connection, "AircraftCatalog", "IsFavorite", "INTEGER DEFAULT 0");

            MigrateBalanceToAssetsAchievements(connection);

            _logger.Database("Schema update complete");
        }

        private void MigrateBalanceToAssetsAchievements(System.Data.Common.DbConnection connection)
        {
            try
            {
                using var checkCommand = connection.CreateCommand();
                checkCommand.CommandText = "SELECT COUNT(*) FROM Achievements WHERE Key LIKE 'balance_%'";
                var count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    using var updateCommand = connection.CreateCommand();
                    updateCommand.CommandText = @"
                        UPDATE Achievements
                        SET Key = REPLACE(Key, 'balance_', 'assets_'),
                            Description = REPLACE(Description, 'balance', 'total assets')
                        WHERE Key LIKE 'balance_%'";
                    var updated = updateCommand.ExecuteNonQuery();
                    _logger.Database($"Migrated {updated} balance achievements to assets achievements");
                }
            }
            catch (Exception ex)
            {
                _logger.Debug($"MigrateBalanceToAssetsAchievements: {ex.Message}");
            }
        }

        private void CreateTableIfNotExists(System.Data.Common.DbConnection connection, string tableName, string createSql)
        {
            using var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";
            var result = checkCommand.ExecuteScalar();

            if (result == null)
            {
                using var createCommand = connection.CreateCommand();
                createCommand.CommandText = createSql;
                createCommand.ExecuteNonQuery();
                _logger.Database($"Created table {tableName}");
            }
            else
            {
                _logger.Database($"Table {tableName} already exists");
            }
        }

        private void AddColumnIfNotExists(System.Data.Common.DbConnection connection, string tableName, string columnName, string columnType)
        {
            using var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = $"PRAGMA table_info({tableName})";

            bool columnExists = false;
            using (var reader = checkCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.GetString(1).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        columnExists = true;
                        break;
                    }
                }
            }

            if (!columnExists)
            {
                using var alterCommand = connection.CreateCommand();
                alterCommand.CommandText = $"ALTER TABLE {tableName} ADD COLUMN {columnName} {columnType}";
                alterCommand.ExecuteNonQuery();
                _logger.Database($"Added column {columnName} to {tableName}");
            }
            else
            {
                _logger.Database($"Column {columnName} already exists in {tableName}");
            }
        }

        private void UpdateExistingAircraftCategories(AceDbContext context)
        {

            var aircraftWithoutCategory = context.Aircraft
                .Where(a => a.Category == AircraftCategory.SingleEnginePiston && string.IsNullOrEmpty(a.CategoryString))
                .ToList();

            foreach (var aircraft in aircraftWithoutCategory)
            {
                aircraft.Category = MaintenanceCheckDefinitions.DetermineCategory(
                    aircraft.CategoryString,
                    aircraft.MaxPassengers,
                    aircraft.CruiseSpeedKts);

                _logger.Database($"Set category for {aircraft.Registration} to {aircraft.Category}");
            }

            if (aircraftWithoutCategory.Any())
            {
                context.SaveChanges();
                _logger.Database($"Updated categories for {aircraftWithoutCategory.Count} aircraft");
            }
        }

        private void SyncServiceCeilingFromCatalog(AceDbContext context)
        {
            var aircraftWithoutCeiling = context.Aircraft
                .Where(a => a.ServiceCeilingFt == 0)
                .ToList();

            if (aircraftWithoutCeiling.Count == 0) return;

            var catalog = context.AircraftCatalog.ToList();
            var updated = 0;

            foreach (var aircraft in aircraftWithoutCeiling)
            {
                var fullType = $"{aircraft.Type} {aircraft.Variant}".Trim();
                var catalogEntry = catalog.FirstOrDefault(c =>
                    c.Type == fullType ||
                    c.Type == aircraft.Variant ||
                    c.Title.Contains(aircraft.Type, StringComparison.OrdinalIgnoreCase));

                if (catalogEntry != null && catalogEntry.ServiceCeilingFt > 0)
                {
                    aircraft.ServiceCeilingFt = catalogEntry.ServiceCeilingFt;
                    updated++;
                    _logger.Database($"Synced ServiceCeilingFt for {aircraft.Registration}: {catalogEntry.ServiceCeilingFt} ft");
                }
            }

            if (updated > 0)
            {
                context.SaveChanges();
                _logger.Database($"Updated ServiceCeilingFt for {updated} aircraft from catalog");
            }
        }
    }
}
