using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Repositories
{
    public class MonthlyBillingRepository : BaseRepository, IMonthlyBillingRepository
    {
        public MonthlyBillingRepository(ILoggingService logger) : base(logger) { }

        public void AddBillingDetails(List<MonthlyBillingDetail> details)
        {
            try
            {
                using var db = new AceDbContext();
                db.MonthlyBillingDetails.AddRange(details);
                db.SaveChanges();
                _logger.Info($"MonthlyBillingRepository: Added {details.Count} billing details");
            }
            catch (Exception ex)
            {
                _logger.Error($"MonthlyBillingRepository: Failed to add details: {ex.Message}");
                throw;
            }
        }

        public List<MonthlyBillingDetail> GetDetailsByMonth(int year, int month)
        {
            try
            {
                using var db = new AceDbContext();
                return db.MonthlyBillingDetails
                    .Where(d => d.BillingDate.Year == year && d.BillingDate.Month == month)
                    .OrderBy(d => d.Category)
                    .ThenBy(d => d.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"MonthlyBillingRepository: Failed to get details for {year}/{month}: {ex.Message}");
                return new List<MonthlyBillingDetail>();
            }
        }

        public List<MonthlyBillingDetail> GetAllDetails()
        {
            try
            {
                using var db = new AceDbContext();
                return db.MonthlyBillingDetails.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"MonthlyBillingRepository: Failed to get all details: {ex.Message}");
                return new List<MonthlyBillingDetail>();
            }
        }
    }
}
