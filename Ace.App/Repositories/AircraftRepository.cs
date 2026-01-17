using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Repositories
{
    public class AircraftRepository : BaseRepository, IAircraftRepository
    {
        public AircraftRepository(ILoggingService logger) : base(logger) { }

        public List<Aircraft> GetAllAircraft()
        {
            try
            {
                using var db = new AceDbContext();
                return db.Aircraft.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftRepository: Failed to get all aircraft: {ex.Message}");
                return new List<Aircraft>();
            }
        }

        public Aircraft? GetAircraftById(int id)
        {
            try
            {
                using var db = new AceDbContext();
                return db.Aircraft.FirstOrDefault(a => a.Id == id);
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftRepository: Failed to get aircraft {id}: {ex.Message}");
                return null;
            }
        }

        public Aircraft? GetAircraftByRegistration(string registration)
        {
            try
            {
                using var db = new AceDbContext();
                return db.Aircraft.FirstOrDefault(a => a.Registration == registration);
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftRepository: Failed to get aircraft by registration {registration}: {ex.Message}");
                return null;
            }
        }

        public void AddAircraft(Aircraft aircraft)
        {
            try
            {
                using var db = new AceDbContext();
                db.Aircraft.Add(aircraft);
                db.SaveChanges();
                _logger.Info($"AircraftRepository: Added aircraft {aircraft.Registration}");
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftRepository: Failed to add aircraft: {ex.Message}");
                throw;
            }
        }

        public void UpdateAircraft(Aircraft aircraft)
        {
            try
            {
                using var db = new AceDbContext();
                db.Aircraft.Update(aircraft);
                db.SaveChanges();
                _logger.Info($"AircraftRepository: Updated aircraft {aircraft.Registration}");
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftRepository: Failed to update aircraft: {ex.Message}");
                throw;
            }
        }

        public void DeleteAircraft(int id)
        {
            try
            {
                using var db = new AceDbContext();
                var aircraft = db.Aircraft.Find(id);
                if (aircraft != null)
                {
                    db.Aircraft.Remove(aircraft);
                    db.SaveChanges();
                    _logger.Info($"AircraftRepository: Deleted aircraft {aircraft.Registration}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftRepository: Failed to delete aircraft {id}: {ex.Message}");
                throw;
            }
        }

        public List<Aircraft> GetAircraftByStatus(AircraftStatus status)
        {
            try
            {
                using var db = new AceDbContext();
                return db.Aircraft.Where(a => a.Status == status).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftRepository: Failed to get aircraft by status: {ex.Message}");
                return new List<Aircraft>();
            }
        }

        public List<Aircraft> GetAircraftByFBOId(int fboId)
        {
            try
            {
                using var db = new AceDbContext();
                return db.Aircraft.Where(a => a.AssignedFBOId == fboId).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftRepository: Failed to get aircraft by FBO {fboId}: {ex.Message}");
                return new List<Aircraft>();
            }
        }

        public List<Aircraft> GetAircraftWithPilotByFBOIds(params int[] fboIds)
        {
            try
            {
                using var db = new AceDbContext();
                return db.Aircraft
                    .Where(a => fboIds.Contains(a.AssignedFBOId ?? 0) && a.AssignedPilotId != null)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftRepository: Failed to get aircraft with pilots: {ex.Message}");
                return new List<Aircraft>();
            }
        }

        public List<string> GetDistinctAircraftTypes()
        {
            try
            {
                using var db = new AceDbContext();
                return db.Aircraft.Select(a => a.Type).Distinct().ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftRepository: Failed to get distinct aircraft types: {ex.Message}");
                return new List<string>();
            }
        }

        public Aircraft? GetAircraftByType(string aircraftType)
        {
            try
            {
                using var db = new AceDbContext();
                return db.Aircraft.FirstOrDefault(a => a.Type == aircraftType);
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftRepository: Failed to get aircraft by type {aircraftType}: {ex.Message}");
                return null;
            }
        }

        public List<Aircraft> GetUnassignedStationedAircraft()
        {
            try
            {
                using var db = new AceDbContext();
                return db.Aircraft
                    .Where(a => a.AssignedPilotId == null && a.Status == AircraftStatus.Stationed)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftRepository: Failed to get unassigned stationed aircraft: {ex.Message}");
                return new List<Aircraft>();
            }
        }

        public Aircraft? GetAircraftByPilotId(int pilotId)
        {
            try
            {
                using var db = new AceDbContext();
                return db.Aircraft.FirstOrDefault(a => a.AssignedPilotId == pilotId);
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftRepository: Failed to get aircraft by pilot {pilotId}: {ex.Message}");
                return null;
            }
        }

        public decimal GetTotalFleetValue()
        {
            try
            {
                using var db = new AceDbContext();
                return db.Aircraft.Sum(a => a.PurchasePrice);
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftRepository: Failed to get total fleet value: {ex.Message}");
                return 0;
            }
        }

        public int GetAircraftCount()
        {
            try
            {
                using var db = new AceDbContext();
                return db.Aircraft.Count();
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftRepository: Failed to get aircraft count: {ex.Message}");
                return 0;
            }
        }
    }
}
