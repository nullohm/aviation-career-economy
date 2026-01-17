using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ace.App.Data;
using Ace.App.Infrastructure;
using Ace.App.Interfaces;
using Ace.App.Services;

namespace Ace.App.DevTools
{
    public static class TestDailyEarnings
    {
        public static void SetDateDaysAgo(int daysAgo)
        {
            var logger = ServiceLocator.GetService<ILoggingService>();
            using var db = new AceDbContext();
            var settings = db.Settings.FirstOrDefault();
            if (settings != null)
            {
                var newDate = DateTime.Today.AddDays(-daysAgo);
                settings.LastDailyEarningsDate = newDate;
                db.SaveChanges();
                logger.Info($"TestDailyEarnings: Set LastDailyEarningsDate to {newDate:yyyy-MM-dd} ({daysAgo} days ago)");
            }
        }

        public static void ClearDate()
        {
            var logger = ServiceLocator.GetService<ILoggingService>();
            using var db = new AceDbContext();
            var settings = db.Settings.FirstOrDefault();
            if (settings != null)
            {
                settings.LastDailyEarningsDate = null;
                db.SaveChanges();
                logger.Info("TestDailyEarnings: Cleared LastDailyEarningsDate (will initialize on next startup)");
            }
        }

        public static string GetStatus()
        {
            using var db = new AceDbContext();
            var settings = db.Settings.AsNoTracking().FirstOrDefault();
            var date = settings?.LastDailyEarningsDate;
            var assignedCount = db.Aircraft.AsNoTracking().Count(a => a.AssignedPilotId != null);
            var totalAircraft = db.Aircraft.AsNoTracking().Count();

            var assignedList = db.Aircraft.AsNoTracking()
                .Where(a => a.AssignedPilotId != null)
                .Select(a => new { a.Registration, a.AssignedPilotId, a.MaxPassengers, a.CruiseSpeedKts })
                .ToList();

            var assignedInfo = assignedList.Any()
                ? string.Join(", ", assignedList.Select(a => $"{a.Registration}(PAX:{a.MaxPassengers},Kts:{a.CruiseSpeedKts})"))
                : "none";

            return $"Date: {(date.HasValue ? date.Value.ToString("yyyy-MM-dd") : "NULL")}, Assigned: {assignedCount}/{totalAircraft}\nAircraft: {assignedInfo}";
        }
    }
}
