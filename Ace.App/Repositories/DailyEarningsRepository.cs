using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Repositories
{
    public class DailyEarningsRepository : BaseRepository, IDailyEarningsRepository
    {
        public DailyEarningsRepository(ILoggingService logger) : base(logger) { }

        public void AddDailyEarningsDetails(List<DailyEarningsDetail> details)
        {
            try
            {
                using var db = new AceDbContext();
                db.DailyEarningsDetails.AddRange(details);
                db.SaveChanges();
                _logger.Info($"DailyEarningsRepository: Added {details.Count} daily earnings details");
            }
            catch (Exception ex)
            {
                _logger.Error($"DailyEarningsRepository: Failed to add details: {ex.Message}");
                throw;
            }
        }

        public List<DailyEarningsDetail> GetDetailsByDate(DateTime date)
        {
            try
            {
                using var db = new AceDbContext();
                return db.DailyEarningsDetails
                    .Where(d => d.Date.Date == date.Date)
                    .OrderBy(d => d.AircraftRegistration)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"DailyEarningsRepository: Failed to get details for date {date:d}: {ex.Message}");
                return new List<DailyEarningsDetail>();
            }
        }

        public List<DailyEarningsDetail> GetDetailsByYearMonth(int year, int month)
        {
            try
            {
                using var db = new AceDbContext();
                return db.DailyEarningsDetails
                    .Where(d => d.Date.Year == year && d.Date.Month == month)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"DailyEarningsRepository: Failed to get details for {year}/{month}: {ex.Message}");
                return new List<DailyEarningsDetail>();
            }
        }

        public List<DailyEarningsDetail> GetDetailsByYear(int year)
        {
            try
            {
                using var db = new AceDbContext();
                return db.DailyEarningsDetails
                    .Where(d => d.Date.Year == year)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"DailyEarningsRepository: Failed to get details for year {year}: {ex.Message}");
                return new List<DailyEarningsDetail>();
            }
        }

        public List<DailyEarningsDetail> GetAllDetails()
        {
            try
            {
                using var db = new AceDbContext();
                return db.DailyEarningsDetails.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"DailyEarningsRepository: Failed to get all details: {ex.Message}");
                return new List<DailyEarningsDetail>();
            }
        }
    }
}
