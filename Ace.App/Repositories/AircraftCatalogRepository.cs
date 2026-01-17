using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Repositories
{
    public class AircraftCatalogRepository : BaseRepository, IAircraftCatalogRepository
    {
        public AircraftCatalogRepository(ILoggingService logger) : base(logger) { }

        public List<AircraftCatalogEntry> GetAllAircraft()
        {
            try
            {
                using var db = new AceDbContext();
                return db.AircraftCatalog.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftCatalogRepository: Failed to get all aircraft: {ex.Message}");
                return new List<AircraftCatalogEntry>();
            }
        }

        public AircraftCatalogEntry? GetAircraftByTitle(string title)
        {
            try
            {
                using var db = new AceDbContext();
                return db.AircraftCatalog.FirstOrDefault(a => a.Title == title);
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftCatalogRepository: Failed to get aircraft by title {title}: {ex.Message}");
                return null;
            }
        }

        public void UpdateMarketPrice(int id, decimal newPrice)
        {
            try
            {
                using var db = new AceDbContext();
                var aircraft = db.AircraftCatalog.Find(id);
                if (aircraft != null)
                {
                    aircraft.MarketPrice = newPrice;
                    db.SaveChanges();
                    _logger.Info($"AircraftCatalogRepository: Updated market price for {aircraft.Title} to {newPrice:C}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftCatalogRepository: Failed to update market price: {ex.Message}");
                throw;
            }
        }

        public void UpdateAircraftCatalogEntry(AircraftCatalogEntry entry)
        {
            try
            {
                using var db = new AceDbContext();
                var existing = db.AircraftCatalog.FirstOrDefault(a => a.Title == entry.Title);
                if (existing != null)
                {
                    existing.PassengerCapacity = entry.PassengerCapacity;
                    existing.MaxCargoKg = entry.MaxCargoKg;
                    existing.CruiseSpeedKts = entry.CruiseSpeedKts;
                    existing.MaxRangeNM = entry.MaxRangeNM;
                    existing.FuelCapacityGal = entry.FuelCapacityGal;
                    existing.FuelBurnGalPerHour = entry.FuelBurnGalPerHour;
                    existing.MarketPrice = entry.MarketPrice;
                    existing.HourlyOperatingCost = entry.HourlyOperatingCost;
                    db.SaveChanges();
                    _logger.Info($"AircraftCatalogRepository: Updated {entry.Title}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftCatalogRepository: Failed to update catalog entry: {ex.Message}");
                throw;
            }
        }

        public void AddAircraftCatalogEntry(AircraftCatalogEntry entry)
        {
            try
            {
                using var db = new AceDbContext();
                db.AircraftCatalog.Add(entry);
                db.SaveChanges();
                _logger.Info($"AircraftCatalogRepository: Added {entry.Title}");
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftCatalogRepository: Failed to add catalog entry: {ex.Message}");
                throw;
            }
        }

        public void UpdateCustomImagePath(string title, string? imagePath)
        {
            try
            {
                using var db = new AceDbContext();
                var aircraft = db.AircraftCatalog.FirstOrDefault(a => a.Title == title);
                if (aircraft != null)
                {
                    aircraft.CustomImagePath = imagePath;
                    db.SaveChanges();
                    _logger.Info($"AircraftCatalogRepository: Updated custom image for {title}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftCatalogRepository: Failed to update custom image: {ex.Message}");
                throw;
            }
        }

        public void DeleteByTitle(string title)
        {
            try
            {
                using var db = new AceDbContext();
                var aircraft = db.AircraftCatalog.FirstOrDefault(a => a.Title == title);
                if (aircraft != null)
                {
                    db.AircraftCatalog.Remove(aircraft);
                    db.SaveChanges();
                    _logger.Info($"AircraftCatalogRepository: Deleted {title}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"AircraftCatalogRepository: Failed to delete catalog entry: {ex.Message}");
                throw;
            }
        }
    }
}
