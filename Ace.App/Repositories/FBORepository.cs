using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Repositories
{
    public class FBORepository : BaseRepository, IFBORepository
    {
        public FBORepository(ILoggingService logger) : base(logger) { }

        public List<FBO> GetAllFBOs()
        {
            try
            {
                using var db = new AceDbContext();
                return db.FBOs.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"FBORepository: Failed to get all FBOs: {ex.Message}");
                return new List<FBO>();
            }
        }

        public FBO? GetFBOById(int id)
        {
            try
            {
                using var db = new AceDbContext();
                return db.FBOs.FirstOrDefault(f => f.Id == id);
            }
            catch (Exception ex)
            {
                _logger.Error($"FBORepository: Failed to get FBO {id}: {ex.Message}");
                return null;
            }
        }

        public FBO? GetFBOByICAO(string icao)
        {
            try
            {
                using var db = new AceDbContext();
                return db.FBOs.FirstOrDefault(f => f.ICAO == icao);
            }
            catch (Exception ex)
            {
                _logger.Error($"FBORepository: Failed to get FBO by ICAO {icao}: {ex.Message}");
                return null;
            }
        }

        public void AddFBO(FBO fbo)
        {
            try
            {
                using var db = new AceDbContext();
                db.FBOs.Add(fbo);
                db.SaveChanges();
                _logger.Info($"FBORepository: Added FBO at {fbo.ICAO}");
            }
            catch (Exception ex)
            {
                _logger.Error($"FBORepository: Failed to add FBO: {ex.Message}");
                throw;
            }
        }

        public void UpdateFBO(FBO fbo)
        {
            try
            {
                using var db = new AceDbContext();
                db.FBOs.Update(fbo);
                db.SaveChanges();
                _logger.Info($"FBORepository: Updated FBO at {fbo.ICAO}");
            }
            catch (Exception ex)
            {
                _logger.Error($"FBORepository: Failed to update FBO: {ex.Message}");
                throw;
            }
        }

        public void DeleteFBO(int id)
        {
            try
            {
                using var db = new AceDbContext();
                var fbo = db.FBOs.Find(id);
                if (fbo != null)
                {
                    var stationedAircraft = db.Aircraft.Where(a => a.AssignedFBOId == id).ToList();
                    foreach (var aircraft in stationedAircraft)
                    {
                        aircraft.AssignedFBOId = null;
                        _logger.Info($"FBORepository: Removed stationing of aircraft {aircraft.Registration} from FBO {fbo.ICAO}");
                    }

                    db.FBOs.Remove(fbo);
                    db.SaveChanges();
                    _logger.Info($"FBORepository: Deleted FBO at {fbo.ICAO}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"FBORepository: Failed to delete FBO {id}: {ex.Message}");
                throw;
            }
        }
    }
}
