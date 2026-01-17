using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;

namespace Ace.App.Repositories
{
    public class PilotRepository : IPilotRepository
    {
        private readonly ILoggingService _log;

        public PilotRepository(ILoggingService loggingService)
        {
            _log = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        }

        public List<Pilot> GetAvailablePilots()
        {
            try
            {
                using var db = new AceDbContext();
                return db.Pilots
                    .Where(p => !p.IsEmployed && !p.IsPlayer)
                    .OrderBy(p => p.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                _log.Error($"PilotRepository: Failed to get available pilots: {ex.Message}");
                return new List<Pilot>();
            }
        }

        public List<Pilot> GetEmployedPilots()
        {
            try
            {
                using var db = new AceDbContext();
                var pilots = db.Pilots
                    .Where(p => p.IsEmployed)
                    .OrderBy(p => p.IsPlayer ? 0 : 1)
                    .ThenBy(p => p.Name)
                    .ToList();

                foreach (var pilot in pilots)
                {
                    _log.Debug($"PilotRepository: Employed pilot {pilot.Name}, ImagePath='{pilot.ImagePath}'");
                }

                return pilots;
            }
            catch (Exception ex)
            {
                _log.Error($"PilotRepository: Failed to get employed pilots: {ex.Message}");
                return new List<Pilot>();
            }
        }

        public List<Pilot> GetEmployedNonPlayerPilots()
        {
            try
            {
                using var db = new AceDbContext();
                return db.Pilots
                    .Where(p => p.IsEmployed && !p.IsPlayer)
                    .OrderBy(p => p.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                _log.Error($"PilotRepository: Failed to get employed non-player pilots: {ex.Message}");
                return new List<Pilot>();
            }
        }

        public Pilot? GetPilotById(int id)
        {
            try
            {
                using var db = new AceDbContext();
                return db.Pilots.FirstOrDefault(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _log.Error($"PilotRepository: Failed to get pilot by ID {id}: {ex.Message}");
                return null;
            }
        }

        public void HirePilot(int pilotId)
        {
            try
            {
                using var db = new AceDbContext();
                var pilot = db.Pilots.FirstOrDefault(p => p.Id == pilotId);

                if (pilot == null)
                {
                    _log.Error($"PilotRepository: Pilot {pilotId} not found");
                    throw new InvalidOperationException($"Pilot with ID {pilotId} not found");
                }

                pilot.IsEmployed = true;
                db.SaveChanges();
                _log.Info($"PilotRepository: Successfully hired pilot {pilot.Name}");
            }
            catch (Exception ex)
            {
                _log.Error($"PilotRepository: Failed to hire pilot {pilotId}: {ex.Message}");
                throw;
            }
        }

        public void FirePilot(int pilotId)
        {
            try
            {
                using var db = new AceDbContext();
                var pilot = db.Pilots.FirstOrDefault(p => p.Id == pilotId);

                if (pilot == null)
                {
                    _log.Error($"PilotRepository: Pilot {pilotId} not found");
                    throw new InvalidOperationException($"Pilot with ID {pilotId} not found");
                }

                if (pilot.IsPlayer)
                {
                    _log.Warn($"PilotRepository: Attempt to fire player pilot blocked");
                    throw new InvalidOperationException("Cannot fire the player pilot");
                }

                pilot.IsEmployed = false;
                db.SaveChanges();
                _log.Info($"PilotRepository: Successfully fired pilot {pilot.Name}");
            }
            catch (Exception ex)
            {
                _log.Error($"PilotRepository: Failed to fire pilot {pilotId}: {ex.Message}");
                throw;
            }
        }

        public void UpdatePilot(Pilot pilot)
        {
            try
            {
                using var db = new AceDbContext();
                db.Pilots.Update(pilot);
                db.SaveChanges();
                _log.Info($"PilotRepository: Successfully updated pilot {pilot.Name}");
            }
            catch (Exception ex)
            {
                _log.Error($"PilotRepository: Failed to update pilot: {ex.Message}");
                throw;
            }
        }

        public void CreatePilot(Pilot pilot)
        {
            try
            {
                using var db = new AceDbContext();
                db.Pilots.Add(pilot);
                db.SaveChanges();
                _log.Info($"PilotRepository: Successfully created pilot {pilot.Name}");
            }
            catch (Exception ex)
            {
                _log.Error($"PilotRepository: Failed to create pilot: {ex.Message}");
                throw;
            }
        }

        public void DeletePilot(int pilotId)
        {
            try
            {
                using var db = new AceDbContext();
                var pilot = db.Pilots.FirstOrDefault(p => p.Id == pilotId);

                if (pilot == null)
                {
                    _log.Error($"PilotRepository: Pilot {pilotId} not found");
                    throw new InvalidOperationException($"Pilot with ID {pilotId} not found");
                }

                if (pilot.IsPlayer)
                {
                    _log.Warn($"PilotRepository: Attempt to delete player pilot blocked");
                    throw new InvalidOperationException("Cannot delete the player pilot");
                }

                var assignedAircraft = db.Aircraft.Where(a => a.AssignedPilotId == pilotId).ToList();
                if (assignedAircraft.Any())
                {
                    _log.Warn($"PilotRepository: Pilot {pilot.Name} has {assignedAircraft.Count} assigned aircraft, unassigning them");
                    foreach (var aircraft in assignedAircraft)
                    {
                        aircraft.AssignedPilotId = null;
                    }
                }

                db.Pilots.Remove(pilot);
                db.SaveChanges();
                _log.Info($"PilotRepository: Successfully deleted pilot {pilot.Name}");
            }
            catch (Exception ex)
            {
                _log.Error($"PilotRepository: Failed to delete pilot {pilotId}: {ex.Message}");
                throw;
            }
        }
    }
}
