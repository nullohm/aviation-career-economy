using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Utilities;

namespace Ace.App.Services
{
    public sealed class PilotCatalogService : IPilotCatalogService
    {
        private readonly ILoggingService _loggingService;

        public PilotCatalogService(ILoggingService loggingService)
        {
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
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
                _loggingService.Error($"PilotCatalogService: Failed to get available pilots: {ex.Message}");
                return new List<Pilot>();
            }
        }

        public List<Pilot> GetEmployedPilots()
        {
            try
            {
                using var db = new AceDbContext();
                return db.Pilots
                    .Where(p => p.IsEmployed)
                    .OrderBy(p => p.IsPlayer ? 0 : 1)
                    .ThenBy(p => p.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                _loggingService.Error($"PilotCatalogService: Failed to get employed pilots: {ex.Message}");
                return new List<Pilot>();
            }
        }

        public string GetPilotImagePath(string imageFileName)
        {
            return PathUtilities.GetPilotImagePath(imageFileName);
        }
    }
}
