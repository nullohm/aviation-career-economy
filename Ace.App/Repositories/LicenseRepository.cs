using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Repositories
{
    public class LicenseRepository : BaseRepository, ILicenseRepository
    {
        public LicenseRepository(ILoggingService logger) : base(logger) { }

        public List<PilotLicense> GetLicensesByPilotId(int pilotId)
        {
            try
            {
                using var db = new AceDbContext();
                return db.Licenses.Where(l => l.PilotId == pilotId).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"LicenseRepository: Failed to get licenses for pilot {pilotId}: {ex.Message}");
                return new List<PilotLicense>();
            }
        }

        public void AddLicense(PilotLicense license)
        {
            try
            {
                using var db = new AceDbContext();
                db.Licenses.Add(license);
                db.SaveChanges();
                _logger.Info($"LicenseRepository: Added license {license.Name} for pilot {license.PilotId}");
            }
            catch (Exception ex)
            {
                _logger.Error($"LicenseRepository: Failed to add license: {ex.Message}");
                throw;
            }
        }

        public void DeleteLicense(int id)
        {
            try
            {
                using var db = new AceDbContext();
                var license = db.Licenses.Find(id);
                if (license != null)
                {
                    db.Licenses.Remove(license);
                    db.SaveChanges();
                    _logger.Info($"LicenseRepository: Deleted license {id}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"LicenseRepository: Failed to delete license {id}: {ex.Message}");
                throw;
            }
        }
    }
}
