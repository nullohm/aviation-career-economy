using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;

namespace Ace.App.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly ILoggingService _log;

        public FlightRepository(ILoggingService loggingService)
        {
            _log = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        }

        public List<FlightRecord> GetAllFlights()
        {
            try
            {
                using var db = new AceDbContext();
                return db.Flights.OrderByDescending(f => f.Date).ToList();
            }
            catch (Exception ex)
            {
                _log.Error($"FlightRepository: Failed to get all flights: {ex.Message}");
                return new List<FlightRecord>();
            }
        }

        public FlightRecord? GetFlightById(int id)
        {
            try
            {
                using var db = new AceDbContext();
                return db.Flights.FirstOrDefault(f => f.Id == id);
            }
            catch (Exception ex)
            {
                _log.Error($"FlightRepository: Failed to get flight by ID {id}: {ex.Message}");
                return null;
            }
        }

        public void SaveFlight(FlightRecord flight)
        {
            try
            {
                using var db = new AceDbContext();
                db.Database.EnsureCreated();

                if (flight.Id == 0)
                {
                    db.Flights.Add(flight);
                    _log.Info($"FlightRepository: Adding new flight {flight.Departure} → {flight.Arrival}");
                }
                else
                {
                    db.Flights.Update(flight);
                    _log.Info($"FlightRepository: Updating flight {flight.Id}");
                }

                db.SaveChanges();
                _log.Database("SaveFlight", 1);
            }
            catch (Exception ex)
            {
                _log.Error($"FlightRepository: Failed to save flight: {ex.Message}");
                throw;
            }
        }

        public void DeleteFlight(int id)
        {
            try
            {
                using var db = new AceDbContext();
                var flight = db.Flights.FirstOrDefault(f => f.Id == id);

                if (flight == null)
                {
                    _log.Warn($"FlightRepository: Flight {id} not found for deletion");
                    return;
                }

                db.Flights.Remove(flight);
                db.SaveChanges();
                _log.Info($"FlightRepository: Deleted flight {id}");
            }
            catch (Exception ex)
            {
                _log.Error($"FlightRepository: Failed to delete flight {id}: {ex.Message}");
                throw;
            }
        }

        public TimeSpan GetTotalFlightTime()
        {
            try
            {
                using var db = new AceDbContext();
                if (!db.Flights.Any()) return TimeSpan.Zero;

                var totalSeconds = db.Flights.Sum(f => f.Duration.TotalSeconds);
                return TimeSpan.FromSeconds(totalSeconds);
            }
            catch (Exception ex)
            {
                _log.Error($"FlightRepository: Failed to calculate total flight time: {ex.Message}");
                return TimeSpan.Zero;
            }
        }
    }
}
