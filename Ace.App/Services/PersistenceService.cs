using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;
using System.IO;

namespace Ace.App.Services
{
    public class PersistenceService : IPersistenceService
    {
        private readonly ILoggingService _logger;
        private readonly Func<AceDbContext> _dbContextFactory;
        public event Action? FlightRecordsChanged;
        public event Action? PilotProfileChanged;

        public PersistenceService(ILoggingService logger, Func<AceDbContext>? dbContextFactory = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContextFactory = dbContextFactory ?? (() => new AceDbContext());
            EnsureDatabaseReady();
            CleanupInvalidRecords();
        }

        private void EnsureDatabaseReady()
        {
            try
            {
                using var db = _dbContextFactory();
                db.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                _logger.Error("Critical Database Initialization Error", ex);
            }
        }

        private void CleanupInvalidRecords()
        {
            try
            {
                using var db = _dbContextFactory();

                var allFlights = db.Flights.ToList();
                var invalid = allFlights.Where(f => f.Duration.TotalSeconds < 5).ToList();
                if (invalid.Any())
                {
                    db.Flights.RemoveRange(invalid);
                    db.SaveChanges();
                    _logger.Info($"Database: Cleaned up {invalid.Count} invalid flight records.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"PersistenceService: Failed to clean up invalid records: {ex.Message}");
            }
        }

        public List<FlightRecord> LoadFlightRecords()
        {
            try
            {
                using var db = _dbContextFactory();
                return db.Flights.OrderByDescending(f => f.Date).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Database: Failed to load flights", ex);
                if (ex.Message.Contains("no such column") || ex.Message.Contains("Status") || ex.Message.Contains("DistanceNM") || ex.Message.Contains("Earnings") || ex.Message.Contains("LegType") || ex.Message.Contains("ParentFlightId") || ex.Message.Contains("PlannedDestination"))
                {
                    HandleSchemaMismatch();
                    // Retry loading after migration
                    try
                    {
                        using var db2 = _dbContextFactory();
                        return db2.Flights.OrderByDescending(f => f.Date).ToList();
                    }
                    catch (Exception retryEx)
                    {
                        _logger.Error("Database: Failed to load flights after migration", retryEx);
                    }
                }
                return new List<FlightRecord>();
            }
        }

        public void SaveFlightRecord(FlightRecord record)
        {
            try
            {
                using var db = _dbContextFactory();
                db.Flights.Add(record);

                var aircraft = db.Aircraft.FirstOrDefault(a => a.Registration == record.Aircraft);

                if (aircraft != null)
                {
                    var flightHours = record.Duration.TotalHours;
                    aircraft.TotalFlightHours += flightHours;
                    aircraft.HoursSinceLastMaintenance += flightHours;
                    _logger.Info($"Database: Updated aircraft {aircraft.Registration} flight hours: +{flightHours:F2}h (total: {aircraft.TotalFlightHours:F2}h)");

                    // Update player pilot flight hours and distance (player flights are always flown by the player)
                    var playerPilot = db.Pilots.FirstOrDefault(p => p.IsPlayer);
                    if (playerPilot != null)
                    {
                        playerPilot.TotalFlightHours += flightHours;
                        playerPilot.TotalDistanceNM += record.DistanceNM;
                        _logger.Info($"Database: Updated player pilot {playerPilot.Name} - hours: +{flightHours:F2}h (total: {playerPilot.TotalFlightHours:F2}h), distance: +{record.DistanceNM:F0} NM (total: {playerPilot.TotalDistanceNM:F0} NM)");
                    }
                }
                else
                {
                    _logger.Warn($"Database: Could not find aircraft for registration '{record.Aircraft}' - flight hours not tracked");
                }

                db.SaveChanges();

                _logger.Info($"Database: Saved flight {record.Departure}->{record.Arrival}");
                FlightRecordsChanged?.Invoke();
                PilotProfileChanged?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.Error("Database: Failed to save flight", ex);
                if (ex.Message.Contains("no such column") || ex.Message.Contains("Status") || ex.Message.Contains("DistanceNM") || ex.Message.Contains("Earnings") || ex.Message.Contains("LegType") || ex.Message.Contains("ParentFlightId") || ex.Message.Contains("PlannedDestination"))
                {
                    HandleSchemaMismatch();
                }
            }
        }

        private void HandleSchemaMismatch()
        {
            _logger.Warn("Database schema mismatch detected. Attempting to migrate schema.");
            try
            {
                using var db = _dbContextFactory();
                var connection = db.Database.GetDbConnection();
                connection.Open();

                using var command = connection.CreateCommand();


                try
                {
                    command.CommandText = "ALTER TABLE Flights ADD COLUMN Earnings REAL DEFAULT 0";
                    command.ExecuteNonQuery();
                    _logger.Info("Added Earnings column to Flights table");
                }
                catch (Exception ex)
                {

                    _logger.Debug($"PersistenceService: Earnings column may already exist: {ex.Message}");
                }

                try
                {
                    command.CommandText = "ALTER TABLE Flights ADD COLUMN Status TEXT DEFAULT ''";
                    command.ExecuteNonQuery();
                    _logger.Info("Added Status column to Flights table");
                }
                catch (Exception ex)
                {

                    _logger.Debug($"PersistenceService: Status column may already exist: {ex.Message}");
                }

                try
                {
                    command.CommandText = "ALTER TABLE Flights ADD COLUMN DistanceNM REAL DEFAULT 0";
                    command.ExecuteNonQuery();
                    _logger.Info("Added DistanceNM column to Flights table");
                }
                catch (Exception ex)
                {

                    _logger.Debug($"PersistenceService: DistanceNM column may already exist: {ex.Message}");
                }

                try
                {
                    command.CommandText = "ALTER TABLE Flights ADD COLUMN LegType INTEGER DEFAULT 0";
                    command.ExecuteNonQuery();
                    _logger.Info("Added LegType column to Flights table");
                }
                catch (Exception ex)
                {
                    _logger.Debug($"PersistenceService: LegType column may already exist: {ex.Message}");
                }

                try
                {
                    command.CommandText = "ALTER TABLE Flights ADD COLUMN ParentFlightId INTEGER";
                    command.ExecuteNonQuery();
                    _logger.Info("Added ParentFlightId column to Flights table");
                }
                catch (Exception ex)
                {
                    _logger.Debug($"PersistenceService: ParentFlightId column may already exist: {ex.Message}");
                }

                try
                {
                    command.CommandText = "ALTER TABLE Flights ADD COLUMN PlannedDestination TEXT DEFAULT ''";
                    command.ExecuteNonQuery();
                    _logger.Info("Added PlannedDestination column to Flights table");
                }
                catch (Exception ex)
                {
                    _logger.Debug($"PersistenceService: PlannedDestination column may already exist: {ex.Message}");
                }

                connection.Close();
                _logger.Info("Schema migration completed successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to migrate database schema, resetting database", ex);
                try
                {
                    using var db = _dbContextFactory();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                    EnsureDatabaseReady();
                }
                catch (Exception resetEx)
                {
                    _logger.Error("Failed to reset database", resetEx);
                }
            }
        }

        public Pilot GetActivePilot()
        {
            try
            {
                using var db = _dbContextFactory();
                return db.Pilots
                    .Include(p => p.Licenses)
                    .Include(p => p.TypeRatings)
                    .FirstOrDefault() ?? new Pilot();
            }
            catch (Exception ex)
            {
                _logger.Error("Database: Failed to load pilot", ex);
                return new Pilot();
            }
        }

        public void SavePilot(Pilot pilot)
        {
            try
            {
                using var db = _dbContextFactory();
                db.Pilots.Update(pilot);
                db.SaveChanges();
                PilotProfileChanged?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.Error("Database: Failed to save pilot", ex);
            }
        }

        public TimeSpan GetTotalFlightTime()
        {
            try
            {
                using var db = _dbContextFactory();
                if (!db.Flights.Any()) return TimeSpan.Zero;

                var durations = db.Flights.Select(f => f.Duration).ToList();
                var totalTicks = durations.Sum(d => d.Ticks);
                return TimeSpan.FromTicks(totalTicks);
            }
            catch (Exception ex)
            {
                _logger.Error("Database: Failed to calculate flight time", ex);
                return TimeSpan.Zero;
            }
        }
    }
}

