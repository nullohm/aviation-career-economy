using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Repositories
{
    public class AircraftPilotAssignmentRepository : IAircraftPilotAssignmentRepository
    {
        private readonly ILoggingService _logger;

        public AircraftPilotAssignmentRepository(ILoggingService logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<AircraftPilotAssignment> GetAssignmentsByAircraftId(int aircraftId)
        {
            try
            {
                using var db = new AceDbContext();
                return db.AircraftPilotAssignments
                    .Where(a => a.AircraftId == aircraftId)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"GetAssignmentsByAircraftId({aircraftId}): {ex.Message}");
                return new List<AircraftPilotAssignment>();
            }
        }

        public List<AircraftPilotAssignment> GetAllAssignments()
        {
            try
            {
                using var db = new AceDbContext();
                return db.AircraftPilotAssignments.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"GetAllAssignments: {ex.Message}");
                return new List<AircraftPilotAssignment>();
            }
        }

        public AircraftPilotAssignment? GetAssignmentByPilotId(int pilotId)
        {
            try
            {
                using var db = new AceDbContext();
                return db.AircraftPilotAssignments
                    .FirstOrDefault(a => a.PilotId == pilotId);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetAssignmentByPilotId({pilotId}): {ex.Message}");
                return null;
            }
        }

        public void AssignPilot(int aircraftId, int pilotId)
        {
            try
            {
                using var db = new AceDbContext();
                var existing = db.AircraftPilotAssignments
                    .FirstOrDefault(a => a.PilotId == pilotId);

                if (existing != null)
                {
                    _logger.Warn($"Pilot {pilotId} already assigned to aircraft {existing.AircraftId}, reassigning to {aircraftId}");
                    existing.AircraftId = aircraftId;
                    existing.AssignedDate = DateTime.Today;
                }
                else
                {
                    db.AircraftPilotAssignments.Add(new AircraftPilotAssignment
                    {
                        AircraftId = aircraftId,
                        PilotId = pilotId,
                        AssignedDate = DateTime.Today
                    });
                }

                db.SaveChanges();
                _logger.Info($"Assigned pilot {pilotId} to aircraft {aircraftId}");
            }
            catch (Exception ex)
            {
                _logger.Error($"AssignPilot({aircraftId}, {pilotId}): {ex.Message}");
                throw;
            }
        }

        public void UnassignPilot(int pilotId)
        {
            try
            {
                using var db = new AceDbContext();
                var assignment = db.AircraftPilotAssignments
                    .FirstOrDefault(a => a.PilotId == pilotId);

                if (assignment != null)
                {
                    db.AircraftPilotAssignments.Remove(assignment);
                    db.SaveChanges();
                    _logger.Info($"Unassigned pilot {pilotId} from aircraft {assignment.AircraftId}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"UnassignPilot({pilotId}): {ex.Message}");
                throw;
            }
        }

        public void UnassignAllFromAircraft(int aircraftId)
        {
            try
            {
                using var db = new AceDbContext();
                var assignments = db.AircraftPilotAssignments
                    .Where(a => a.AircraftId == aircraftId)
                    .ToList();

                if (assignments.Any())
                {
                    db.AircraftPilotAssignments.RemoveRange(assignments);
                    db.SaveChanges();
                    _logger.Info($"Unassigned {assignments.Count} pilots from aircraft {aircraftId}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"UnassignAllFromAircraft({aircraftId}): {ex.Message}");
                throw;
            }
        }

        public int GetAssignedPilotCount(int aircraftId)
        {
            try
            {
                using var db = new AceDbContext();
                return db.AircraftPilotAssignments
                    .Count(a => a.AircraftId == aircraftId);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetAssignedPilotCount({aircraftId}): {ex.Message}");
                return 0;
            }
        }

        public List<int> GetAssignedPilotIds(int aircraftId)
        {
            try
            {
                using var db = new AceDbContext();
                return db.AircraftPilotAssignments
                    .Where(a => a.AircraftId == aircraftId)
                    .Select(a => a.PilotId)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"GetAssignedPilotIds({aircraftId}): {ex.Message}");
                return new List<int>();
            }
        }

        public bool IsPilotAssigned(int pilotId)
        {
            try
            {
                using var db = new AceDbContext();
                return db.AircraftPilotAssignments
                    .Any(a => a.PilotId == pilotId);
            }
            catch (Exception ex)
            {
                _logger.Error($"IsPilotAssigned({pilotId}): {ex.Message}");
                return false;
            }
        }
    }
}
