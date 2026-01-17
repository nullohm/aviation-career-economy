using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Repositories
{
    public class ScheduledRouteRepository : BaseRepository, IScheduledRouteRepository
    {
        public ScheduledRouteRepository(ILoggingService logger) : base(logger) { }

        public List<ScheduledRoute> GetAllRoutes()
        {
            try
            {
                using var db = new AceDbContext();
                return db.ScheduledRoutes.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"ScheduledRouteRepository: Failed to get all routes: {ex.Message}");
                return new List<ScheduledRoute>();
            }
        }

        public List<ScheduledRoute> GetActiveRoutes()
        {
            try
            {
                using var db = new AceDbContext();
                return db.ScheduledRoutes.Where(r => r.IsActive).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"ScheduledRouteRepository: Failed to get active routes: {ex.Message}");
                return new List<ScheduledRoute>();
            }
        }

        public List<ScheduledRoute> GetRoutesByFBO(int fboId)
        {
            try
            {
                using var db = new AceDbContext();
                return db.ScheduledRoutes
                    .Where(r => r.OriginFBOId == fboId || r.DestinationFBOId == fboId)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"ScheduledRouteRepository: Failed to get routes for FBO {fboId}: {ex.Message}");
                return new List<ScheduledRoute>();
            }
        }

        public ScheduledRoute? GetRouteById(int id)
        {
            try
            {
                using var db = new AceDbContext();
                return db.ScheduledRoutes.FirstOrDefault(r => r.Id == id);
            }
            catch (Exception ex)
            {
                _logger.Error($"ScheduledRouteRepository: Failed to get route {id}: {ex.Message}");
                return null;
            }
        }

        public ScheduledRoute? GetRouteByAircraft(int aircraftId)
        {
            try
            {
                using var db = new AceDbContext();
                return db.ScheduledRoutes.FirstOrDefault(r => r.AssignedAircraftId == aircraftId && r.IsActive);
            }
            catch (Exception ex)
            {
                _logger.Error($"ScheduledRouteRepository: Failed to get route for aircraft {aircraftId}: {ex.Message}");
                return null;
            }
        }

        public HashSet<int> GetAircraftIdsWithActiveRoutes()
        {
            try
            {
                using var db = new AceDbContext();
                return db.ScheduledRoutes
                    .Where(r => r.IsActive && r.AssignedAircraftId.HasValue)
                    .Select(r => r.AssignedAircraftId!.Value)
                    .ToHashSet();
            }
            catch (Exception ex)
            {
                _logger.Error($"ScheduledRouteRepository: Failed to get aircraft IDs with routes: {ex.Message}");
                return new HashSet<int>();
            }
        }

        public void AddRoute(ScheduledRoute route)
        {
            try
            {
                using var db = new AceDbContext();
                db.ScheduledRoutes.Add(route);
                db.SaveChanges();
                _logger.Info($"ScheduledRouteRepository: Added route {route.OriginFBOId} -> {route.DestinationFBOId}");
            }
            catch (Exception ex)
            {
                _logger.Error($"ScheduledRouteRepository: Failed to add route: {ex.Message}");
                throw;
            }
        }

        public void UpdateRoute(ScheduledRoute route)
        {
            try
            {
                using var db = new AceDbContext();
                var existing = db.ScheduledRoutes.Find(route.Id);
                if (existing != null)
                {
                    existing.OriginFBOId = route.OriginFBOId;
                    existing.DestinationFBOId = route.DestinationFBOId;
                    existing.AssignedAircraftId = route.AssignedAircraftId;
                    existing.IsActive = route.IsActive;
                    db.SaveChanges();
                    _logger.Info($"ScheduledRouteRepository: Updated route {route.Id}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"ScheduledRouteRepository: Failed to update route: {ex.Message}");
                throw;
            }
        }

        public void DeleteRoute(int id)
        {
            try
            {
                using var db = new AceDbContext();
                var route = db.ScheduledRoutes.Find(id);
                if (route != null)
                {
                    db.ScheduledRoutes.Remove(route);
                    db.SaveChanges();
                    _logger.Info($"ScheduledRouteRepository: Deleted route {id}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"ScheduledRouteRepository: Failed to delete route {id}: {ex.Message}");
                throw;
            }
        }

        public int GetRouteCountForFBO(int fboId)
        {
            try
            {
                using var db = new AceDbContext();
                return db.ScheduledRoutes.Count(r =>
                    r.IsActive && (r.OriginFBOId == fboId || r.DestinationFBOId == fboId));
            }
            catch (Exception ex)
            {
                _logger.Error($"ScheduledRouteRepository: Failed to count routes for FBO {fboId}: {ex.Message}");
                return 0;
            }
        }

        public int GetOutgoingRouteCountForFBO(int fboId)
        {
            try
            {
                using var db = new AceDbContext();
                return db.ScheduledRoutes.Count(r => r.IsActive && r.OriginFBOId == fboId);
            }
            catch (Exception ex)
            {
                _logger.Error($"ScheduledRouteRepository: Failed to count outgoing routes for FBO {fboId}: {ex.Message}");
                return 0;
            }
        }
    }
}
