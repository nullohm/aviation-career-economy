using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Infrastructure;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly ILoggingService _logger;
        private readonly IFinanceService _financeService;
        private readonly ISettingsService _settingsService;

        public MaintenanceService(ILoggingService logger, IFinanceService financeService, ISettingsService settingsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        public List<MaintenanceCheckStatus> GetMaintenanceStatus(Aircraft aircraft)
        {
            var statuses = new List<MaintenanceCheckStatus>();
            var applicableChecks = MaintenanceCheckDefinitions.GetApplicableChecks(aircraft.Category);

            foreach (var checkType in applicableChecks)
            {
                var info = MaintenanceCheckDefinitions.GetCheckInfo(checkType, aircraft.Category, aircraft.CurrentValue, _settingsService.CurrentSettings);
                var status = CalculateCheckStatus(aircraft, checkType, info);
                statuses.Add(status);
            }

            return statuses.OrderBy(s => s.UrgencyScore).ToList();
        }

        public List<MaintenanceCheckStatus> GetSchedulableChecks(Aircraft aircraft)
        {
            var allStatuses = GetMaintenanceStatus(aircraft);
            var overdueOrDue = allStatuses.Where(s => s.IsOverdue || s.UrgencyScore <= 1).ToList();

            if (!overdueOrDue.Any())
            {
                return allStatuses;
            }

            var highestPriorityCheck = GetHighestPriorityCheck(overdueOrDue, aircraft.Category);

            var result = new List<MaintenanceCheckStatus> { highestPriorityCheck };

            var okChecks = allStatuses.Where(s => !s.IsOverdue && s.UrgencyScore > 1).ToList();
            result.AddRange(okChecks);

            return result;
        }

        private MaintenanceCheckStatus GetHighestPriorityCheck(List<MaintenanceCheckStatus> checks, AircraftCategory category)
        {
            var isGA = category == AircraftCategory.SingleEnginePiston ||
                       category == AircraftCategory.MultiEnginePiston ||
                       category == AircraftCategory.Turboprop;

            if (isGA)
            {
                if (checks.Any(c => c.CheckType == MaintenanceCheckType.TBO))
                    return checks.First(c => c.CheckType == MaintenanceCheckType.TBO);
                if (checks.Any(c => c.CheckType == MaintenanceCheckType.AnnualInspection))
                    return checks.First(c => c.CheckType == MaintenanceCheckType.AnnualInspection);
                if (checks.Any(c => c.CheckType == MaintenanceCheckType.Check100Hour))
                    return checks.First(c => c.CheckType == MaintenanceCheckType.Check100Hour);
                if (checks.Any(c => c.CheckType == MaintenanceCheckType.Check50Hour))
                    return checks.First(c => c.CheckType == MaintenanceCheckType.Check50Hour);
            }
            else
            {
                if (checks.Any(c => c.CheckType == MaintenanceCheckType.DCheck))
                    return checks.First(c => c.CheckType == MaintenanceCheckType.DCheck);
                if (checks.Any(c => c.CheckType == MaintenanceCheckType.CCheck))
                    return checks.First(c => c.CheckType == MaintenanceCheckType.CCheck);
                if (checks.Any(c => c.CheckType == MaintenanceCheckType.BCheck))
                    return checks.First(c => c.CheckType == MaintenanceCheckType.BCheck);
                if (checks.Any(c => c.CheckType == MaintenanceCheckType.ACheck))
                    return checks.First(c => c.CheckType == MaintenanceCheckType.ACheck);
            }

            return checks.OrderBy(c => c.UrgencyScore).First();
        }

        private MaintenanceCheckStatus CalculateCheckStatus(Aircraft aircraft, MaintenanceCheckType checkType, MaintenanceCheckInfo info)
        {
            var status = new MaintenanceCheckStatus
            {
                CheckType = checkType,
                Name = info.Name,
                Description = info.Description,
                Cost = info.BaseCost,
                DurationDays = info.DurationDays
            };

            double? hoursRemaining = null;
            int? daysRemaining = null;
            DateTime? lastPerformed = null;

            switch (checkType)
            {
                case MaintenanceCheckType.Check50Hour:
                    if (info.IntervalHours.HasValue)
                    {
                        hoursRemaining = info.IntervalHours.Value - aircraft.HoursSinceLastMaintenance;
                    }
                    lastPerformed = aircraft.LastMaintenanceDate;
                    break;

                case MaintenanceCheckType.Check100Hour:
                    if (info.IntervalHours.HasValue)
                    {
                        hoursRemaining = info.IntervalHours.Value - aircraft.HoursSinceLastMaintenance;
                    }
                    lastPerformed = aircraft.LastMaintenanceDate;
                    break;

                case MaintenanceCheckType.AnnualInspection:
                    if (info.IntervalMonths.HasValue)
                    {
                        var nextDue = (aircraft.LastAnnualInspection ?? aircraft.PurchaseDate).AddMonths(info.IntervalMonths.Value);
                        daysRemaining = (nextDue - DateTime.Today).Days;
                    }
                    lastPerformed = aircraft.LastAnnualInspection ?? aircraft.PurchaseDate;
                    break;

                case MaintenanceCheckType.TBO:
                    if (info.IntervalHours.HasValue)
                    {
                        hoursRemaining = info.IntervalHours.Value - aircraft.HoursSinceTBO;
                    }
                    break;

                case MaintenanceCheckType.ACheck:
                    if (info.IntervalHours.HasValue)
                    {
                        hoursRemaining = info.IntervalHours.Value - aircraft.HoursSinceACheck;
                    }
                    if (info.IntervalMonths.HasValue)
                    {
                        var nextDue = (aircraft.LastACheck ?? aircraft.PurchaseDate).AddMonths(info.IntervalMonths.Value);
                        var monthDaysRemaining = (nextDue - DateTime.Today).Days;
                        if (!daysRemaining.HasValue || monthDaysRemaining < daysRemaining)
                            daysRemaining = monthDaysRemaining;
                    }
                    lastPerformed = aircraft.LastACheck;
                    break;

                case MaintenanceCheckType.BCheck:
                    if (info.IntervalMonths.HasValue)
                    {
                        var nextDue = (aircraft.LastBCheck ?? aircraft.PurchaseDate).AddMonths(info.IntervalMonths.Value);
                        daysRemaining = (nextDue - DateTime.Today).Days;
                    }
                    lastPerformed = aircraft.LastBCheck;
                    break;

                case MaintenanceCheckType.CCheck:
                    if (info.IntervalMonths.HasValue)
                    {
                        var nextDue = (aircraft.LastCCheck ?? aircraft.PurchaseDate).AddMonths(info.IntervalMonths.Value);
                        daysRemaining = (nextDue - DateTime.Today).Days;
                    }
                    lastPerformed = aircraft.LastCCheck;
                    break;

                case MaintenanceCheckType.DCheck:
                    if (info.IntervalMonths.HasValue)
                    {
                        var nextDue = (aircraft.LastDCheck ?? aircraft.PurchaseDate).AddMonths(info.IntervalMonths.Value);
                        daysRemaining = (nextDue - DateTime.Today).Days;
                    }
                    lastPerformed = aircraft.LastDCheck;
                    break;
            }

            status.HoursRemaining = hoursRemaining;
            status.DaysRemaining = daysRemaining;
            status.LastPerformed = lastPerformed;

            if (hoursRemaining.HasValue && hoursRemaining <= 0)
                status.IsOverdue = true;
            if (daysRemaining.HasValue && daysRemaining <= 0)
                status.IsOverdue = true;

            if (hoursRemaining.HasValue)
            {
                var interval = info.IntervalHours ?? 100;
                var percentRemaining = (hoursRemaining.Value / interval) * 100;
                if (percentRemaining <= 10)
                    status.UrgencyScore = 1;
                else if (percentRemaining <= 25)
                    status.UrgencyScore = 2;
                else
                    status.UrgencyScore = 3;
            }
            else if (daysRemaining.HasValue)
            {
                if (daysRemaining <= 7)
                    status.UrgencyScore = 1;
                else if (daysRemaining <= 30)
                    status.UrgencyScore = 2;
                else
                    status.UrgencyScore = 3;
            }
            else
            {
                status.UrgencyScore = 3;
            }

            if (status.IsOverdue)
                status.UrgencyScore = 0;

            return status;
        }

        public MaintenanceResult ScheduleMaintenance(int aircraftId, MaintenanceCheckType checkType)
        {
            try
            {
                using var db = new AceDbContext();
                var aircraft = db.Aircraft.Find(aircraftId);

                if (aircraft == null)
                    return new MaintenanceResult { Success = false, Message = "Aircraft not found" };

                if (aircraft.Status == AircraftStatus.InFlight)
                    return new MaintenanceResult { Success = false, Message = "Aircraft is currently in flight" };

                if (aircraft.Status == AircraftStatus.Maintenance)
                    return new MaintenanceResult { Success = false, Message = "Aircraft is already in maintenance" };

                var info = MaintenanceCheckDefinitions.GetCheckInfo(checkType, aircraft.Category, aircraft.CurrentValue, _settingsService.CurrentSettings);

                if (_financeService.Balance < info.BaseCost)
                    return new MaintenanceResult { Success = false, Message = $"Insufficient funds. Required: {info.BaseCost:N0} €" };

                _financeService.AddExpense(info.BaseCost, $"{info.Name}: {aircraft.Registration}");

                var completionDate = DateTime.Today.AddDays(info.DurationDays);

                aircraft.Status = AircraftStatus.Maintenance;
                aircraft.CurrentMaintenanceType = checkType;
                aircraft.MaintenanceCompletionDate = completionDate;

                var maintenanceRecord = new MaintenanceCheck
                {
                    AircraftId = aircraftId,
                    CheckType = checkType,
                    ScheduledDate = DateTime.Today,
                    FlightHoursAtCheck = aircraft.TotalFlightHours,
                    Cost = info.BaseCost,
                    DurationDays = info.DurationDays
                };

                db.MaintenanceChecks.Add(maintenanceRecord);
                db.SaveChanges();

                _logger.Info($"MaintenanceService: Scheduled {info.Name} for {aircraft.Registration}, completion: {completionDate:dd.MM.yyyy}");

                return new MaintenanceResult
                {
                    Success = true,
                    Message = $"{info.Name} scheduled for {aircraft.Registration}.\n\nCost: {info.BaseCost:N0} €\nDuration: {info.DurationDays} days\nCompletion: {completionDate:dd.MM.yyyy}",
                    CompletionDate = completionDate
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"MaintenanceService: Error scheduling maintenance: {ex.Message}");
                return new MaintenanceResult { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public MaintenanceResult CompleteMaintenance(int aircraftId)
        {
            try
            {
                using var db = new AceDbContext();
                var aircraft = db.Aircraft.Find(aircraftId);

                if (aircraft == null)
                    return new MaintenanceResult { Success = false, Message = "Aircraft not found" };

                if (aircraft.Status != AircraftStatus.Maintenance)
                    return new MaintenanceResult { Success = false, Message = "Aircraft is not in maintenance" };

                if (!aircraft.MaintenanceCompletionDate.HasValue || aircraft.MaintenanceCompletionDate.Value > DateTime.Today)
                    return new MaintenanceResult { Success = false, Message = "Maintenance is not yet complete" };

                var checkType = aircraft.CurrentMaintenanceType ?? MaintenanceCheckType.Check100Hour;

                switch (checkType)
                {
                    case MaintenanceCheckType.Check50Hour:
                    case MaintenanceCheckType.Check100Hour:
                        aircraft.HoursSinceLastMaintenance = 0;
                        aircraft.LastMaintenanceDate = DateTime.Today;
                        break;

                    case MaintenanceCheckType.AnnualInspection:
                        aircraft.LastAnnualInspection = DateTime.Today;
                        aircraft.HoursSinceLastMaintenance = 0;
                        aircraft.LastMaintenanceDate = DateTime.Today;
                        break;

                    case MaintenanceCheckType.TBO:
                        aircraft.HoursSinceTBO = 0;
                        aircraft.HoursSinceLastMaintenance = 0;
                        aircraft.LastMaintenanceDate = DateTime.Today;
                        break;

                    case MaintenanceCheckType.ACheck:
                        aircraft.HoursSinceACheck = 0;
                        aircraft.LastACheck = DateTime.Today;
                        aircraft.HoursSinceLastMaintenance = 0;
                        aircraft.LastMaintenanceDate = DateTime.Today;
                        break;

                    case MaintenanceCheckType.BCheck:
                        aircraft.LastBCheck = DateTime.Today;
                        aircraft.HoursSinceACheck = 0;
                        aircraft.LastACheck = DateTime.Today;
                        aircraft.HoursSinceLastMaintenance = 0;
                        aircraft.LastMaintenanceDate = DateTime.Today;
                        break;

                    case MaintenanceCheckType.CCheck:
                        aircraft.LastCCheck = DateTime.Today;
                        aircraft.LastBCheck = DateTime.Today;
                        aircraft.HoursSinceACheck = 0;
                        aircraft.LastACheck = DateTime.Today;
                        aircraft.HoursSinceLastMaintenance = 0;
                        aircraft.LastMaintenanceDate = DateTime.Today;
                        break;

                    case MaintenanceCheckType.DCheck:
                        aircraft.LastDCheck = DateTime.Today;
                        aircraft.LastCCheck = DateTime.Today;
                        aircraft.LastBCheck = DateTime.Today;
                        aircraft.HoursSinceACheck = 0;
                        aircraft.LastACheck = DateTime.Today;
                        aircraft.HoursSinceLastMaintenance = 0;
                        aircraft.HoursSinceTBO = 0;
                        aircraft.LastMaintenanceDate = DateTime.Today;
                        break;
                }

                aircraft.Status = AircraftStatus.Available;
                aircraft.CurrentMaintenanceType = null;
                aircraft.MaintenanceCompletionDate = null;

                var lastRecord = db.MaintenanceChecks
                    .Where(m => m.AircraftId == aircraftId && m.CompletedDate == null)
                    .OrderByDescending(m => m.ScheduledDate)
                    .FirstOrDefault();

                if (lastRecord != null)
                {
                    lastRecord.CompletedDate = DateTime.Today;
                }

                db.SaveChanges();

                var info = MaintenanceCheckDefinitions.GetCheckInfo(checkType, aircraft.Category, aircraft.CurrentValue, _settingsService.CurrentSettings);
                _logger.Info($"MaintenanceService: Completed {info.Name} for {aircraft.Registration}");

                return new MaintenanceResult
                {
                    Success = true,
                    Message = $"{info.Name} completed for {aircraft.Registration}.\n\nAircraft is now available for service."
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"MaintenanceService: Error completing maintenance: {ex.Message}");
                return new MaintenanceResult { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public void UpdateFlightHours(int aircraftId, double hoursFlown)
        {
            try
            {
                using var db = new AceDbContext();
                var aircraft = db.Aircraft.Find(aircraftId);

                if (aircraft != null)
                {
                    aircraft.TotalFlightHours += hoursFlown;
                    aircraft.HoursSinceLastMaintenance += hoursFlown;
                    aircraft.HoursSinceTBO += hoursFlown;
                    aircraft.HoursSinceACheck += hoursFlown;

                    db.SaveChanges();

                    _logger.Debug($"MaintenanceService: Updated flight hours for {aircraft.Registration}: +{hoursFlown:F1}h");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"MaintenanceService: Error updating flight hours: {ex.Message}");
            }
        }

        public void CompleteOverdueMaintenances()
        {
            try
            {
                using var db = new AceDbContext();
                var aircraftInMaintenance = db.Aircraft
                    .Where(a => a.Status == AircraftStatus.Maintenance &&
                                a.MaintenanceCompletionDate.HasValue &&
                                a.MaintenanceCompletionDate.Value <= DateTime.Today)
                    .ToList();

                foreach (var aircraft in aircraftInMaintenance)
                {
                    _logger.Info($"MaintenanceService: Auto-completing maintenance for {aircraft.Registration}");

                    var checkType = aircraft.CurrentMaintenanceType ?? MaintenanceCheckType.Check100Hour;
                    ApplyMaintenanceCompletion(aircraft, checkType);

                    aircraft.Status = AircraftStatus.Available;
                    aircraft.CurrentMaintenanceType = null;
                    aircraft.MaintenanceCompletionDate = null;

                    var lastRecord = db.MaintenanceChecks
                        .Where(m => m.AircraftId == aircraft.Id && m.CompletedDate == null)
                        .OrderByDescending(m => m.ScheduledDate)
                        .FirstOrDefault();

                    if (lastRecord != null)
                    {
                        lastRecord.CompletedDate = DateTime.Today;
                    }
                }

                if (aircraftInMaintenance.Any())
                {
                    db.SaveChanges();
                    _logger.Info($"MaintenanceService: Auto-completed maintenance for {aircraftInMaintenance.Count} aircraft");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"MaintenanceService: Error auto-completing maintenances: {ex.Message}");
            }
        }

        public void GroundAircraftWithOverdueChecks()
        {
            try
            {
                using var db = new AceDbContext();
                var availableAircraft = db.Aircraft
                    .Where(a => a.Status == AircraftStatus.Available || a.Status == AircraftStatus.Stationed)
                    .ToList();

                int groundedCount = 0;
                foreach (var aircraft in availableAircraft)
                {
                    if (IsMaintenanceOverdue(aircraft))
                    {
                        _logger.Info($"MaintenanceService: Grounding {aircraft.Registration} due to overdue maintenance");
                        aircraft.Status = AircraftStatus.Grounded;
                        groundedCount++;
                    }
                }

                if (groundedCount > 0)
                {
                    db.SaveChanges();
                    _logger.Info($"MaintenanceService: Grounded {groundedCount} aircraft due to overdue maintenance");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"MaintenanceService: Error grounding aircraft: {ex.Message}");
            }
        }

        private bool IsMaintenanceOverdue(Aircraft aircraft)
        {
            var statuses = GetMaintenanceStatus(aircraft);
            return statuses.Any(s => s.IsOverdue);
        }

        private void ApplyMaintenanceCompletion(Aircraft aircraft, MaintenanceCheckType checkType)
        {
            switch (checkType)
            {
                case MaintenanceCheckType.Check50Hour:
                case MaintenanceCheckType.Check100Hour:
                    aircraft.HoursSinceLastMaintenance = 0;
                    aircraft.LastMaintenanceDate = DateTime.Today;
                    break;

                case MaintenanceCheckType.AnnualInspection:
                    aircraft.LastAnnualInspection = DateTime.Today;
                    aircraft.HoursSinceLastMaintenance = 0;
                    aircraft.LastMaintenanceDate = DateTime.Today;
                    break;

                case MaintenanceCheckType.TBO:
                    aircraft.HoursSinceTBO = 0;
                    aircraft.HoursSinceLastMaintenance = 0;
                    aircraft.LastMaintenanceDate = DateTime.Today;
                    break;

                case MaintenanceCheckType.ACheck:
                    aircraft.HoursSinceACheck = 0;
                    aircraft.LastACheck = DateTime.Today;
                    aircraft.HoursSinceLastMaintenance = 0;
                    aircraft.LastMaintenanceDate = DateTime.Today;
                    break;

                case MaintenanceCheckType.BCheck:
                    aircraft.LastBCheck = DateTime.Today;
                    aircraft.HoursSinceACheck = 0;
                    aircraft.LastACheck = DateTime.Today;
                    aircraft.HoursSinceLastMaintenance = 0;
                    aircraft.LastMaintenanceDate = DateTime.Today;
                    break;

                case MaintenanceCheckType.CCheck:
                    aircraft.LastCCheck = DateTime.Today;
                    aircraft.LastBCheck = DateTime.Today;
                    aircraft.HoursSinceACheck = 0;
                    aircraft.LastACheck = DateTime.Today;
                    aircraft.HoursSinceLastMaintenance = 0;
                    aircraft.LastMaintenanceDate = DateTime.Today;
                    break;

                case MaintenanceCheckType.DCheck:
                    aircraft.LastDCheck = DateTime.Today;
                    aircraft.LastCCheck = DateTime.Today;
                    aircraft.LastBCheck = DateTime.Today;
                    aircraft.HoursSinceACheck = 0;
                    aircraft.LastACheck = DateTime.Today;
                    aircraft.HoursSinceLastMaintenance = 0;
                    aircraft.HoursSinceTBO = 0;
                    aircraft.LastMaintenanceDate = DateTime.Today;
                    break;
            }
        }

        public bool IsMaintenanceRequired(Aircraft aircraft)
        {
            var statuses = GetMaintenanceStatus(aircraft);
            return statuses.Any(s => s.IsOverdue || s.UrgencyScore <= 1);
        }

        public MaintenanceCheckStatus? GetMostUrgentCheck(Aircraft aircraft)
        {
            var statuses = GetMaintenanceStatus(aircraft);
            var overdueOrDue = statuses.Where(s => s.IsOverdue || s.UrgencyScore <= 1).ToList();

            if (overdueOrDue.Any())
            {
                return GetHighestPriorityCheck(overdueOrDue, aircraft.Category);
            }

            return statuses.FirstOrDefault();
        }
    }

    public class MaintenanceCheckStatus
    {
        public MaintenanceCheckType CheckType { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double? HoursRemaining { get; set; }
        public int? DaysRemaining { get; set; }
        public DateTime? LastPerformed { get; set; }
        public bool IsOverdue { get; set; }
        public int UrgencyScore { get; set; }
        public decimal Cost { get; set; }
        public int DurationDays { get; set; }

        public string StatusText
        {
            get
            {
                if (IsOverdue)
                    return "OVERDUE";

                var parts = new List<string>();

                if (HoursRemaining.HasValue)
                    parts.Add($"{HoursRemaining.Value:F0}h remaining");

                if (DaysRemaining.HasValue)
                    parts.Add($"{DaysRemaining.Value} days remaining");

                return parts.Count > 0 ? string.Join(" / ", parts) : "OK";
            }
        }

        public string UrgencyText
        {
            get
            {
                return UrgencyScore switch
                {
                    0 => "OVERDUE",
                    1 => "Due Soon",
                    2 => "Upcoming",
                    _ => "OK"
                };
            }
        }
    }

    public class MaintenanceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime? CompletionDate { get; set; }
    }
}
