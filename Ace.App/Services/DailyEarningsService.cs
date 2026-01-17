using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Services
{
    public class DailyEarningsService : IDailyEarningsService
    {
        private readonly ILoggingService _loggingService;
        private readonly IFinanceService _financeService;
        private readonly IPersistenceService _persistenceService;
        private readonly IPricingService _pricingService;
        private readonly IScheduledRouteService _scheduledRouteService;
        private readonly IAchievementService _achievementService;

        public DailyEarningsService(
            ILoggingService loggingService,
            IFinanceService financeService,
            IPersistenceService persistenceService,
            IPricingService pricingService,
            IScheduledRouteService scheduledRouteService,
            IAchievementService achievementService)
        {
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _persistenceService = persistenceService ?? throw new ArgumentNullException(nameof(persistenceService));
            _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
            _scheduledRouteService = scheduledRouteService ?? throw new ArgumentNullException(nameof(scheduledRouteService));
            _achievementService = achievementService ?? throw new ArgumentNullException(nameof(achievementService));
        }

        public async Task ProcessDailyEarnings()
        {
            _loggingService.Info("DailyEarningsService: Starting daily earnings processing");

            try
            {
                await Task.Run(() =>
                {
                    using var db = new AceDbContext();

                    var settings = db.Settings.FirstOrDefault();
                    if (settings == null)
                    {
                        _loggingService.Info("DailyEarningsService: No settings found, skipping");
                        return;
                    }

                    var dailyFlightHours = (double)settings.PilotFlightHoursPerDay;
                    _loggingService.Info($"DailyEarningsService: Using {dailyFlightHours}h flight hours per pilot per day");

                    var today = DateTime.Today;
                    var lastEarningsDate = settings.LastDailyEarningsDate?.Date;

                    _loggingService.Info($"DailyEarningsService: Today={today:yyyy-MM-dd}, LastEarningsDate={lastEarningsDate?.ToString("yyyy-MM-dd") ?? "NULL"}");

                    if (lastEarningsDate == null)
                    {
                        _loggingService.Info("DailyEarningsService: First run, setting initial date to today (no earnings)");
                        settings.LastDailyEarningsDate = today;
                        db.SaveChanges();
                        return;
                    }

                    if (lastEarningsDate >= today)
                    {
                        _loggingService.Info($"DailyEarningsService: Already processed today ({lastEarningsDate:yyyy-MM-dd} >= {today:yyyy-MM-dd}), skipping");
                        return;
                    }

                    var assignedAircraft = db.Aircraft
                        .Where(a => a.AssignedPilotId != null && a.Status != AircraftStatus.Maintenance)
                        .ToList();

                    if (!assignedAircraft.Any())
                    {
                        _loggingService.Info("DailyEarningsService: No assigned aircraft found, updating date only");
                        settings.LastDailyEarningsDate = today;
                        db.SaveChanges();
                        return;
                    }

                    var pilots = db.Pilots.ToList();
                    var daysToProcess = (int)(today - lastEarningsDate.Value).TotalDays;
                    _loggingService.Info($"DailyEarningsService: Processing {daysToProcess} day(s) for {assignedAircraft.Count} aircraft");

                    decimal totalEarnings = 0m;

                    for (int dayOffset = 1; dayOffset <= daysToProcess; dayOffset++)
                    {
                        var earningsDate = lastEarningsDate.Value.AddDays(dayOffset);

                        decimal dailyTotalRevenue = 0m;
                        decimal dailyTotalCosts = 0m;
                        var dailyDetails = new List<DailyEarningsDetail>();

                        foreach (var aircraft in assignedAircraft)
                        {
                            var pilot = pilots.FirstOrDefault(p => p.Id == aircraft.AssignedPilotId);
                            var passiveEarnings = _pricingService.CalculateDailyPassiveEarnings(aircraft, dailyFlightHours, pilot);

                            var pilotName = pilot?.Name ?? "Unknown";

                            // Create detail entry for this aircraft
                            var detail = new DailyEarningsDetail
                            {
                                Date = earningsDate,
                                AircraftId = aircraft.Id,
                                AircraftRegistration = aircraft.Registration,
                                AircraftType = aircraft.Type,
                                PilotName = pilotName,
                                FlightHours = dailyFlightHours,
                                DistanceNM = passiveEarnings.DailyDistanceNM,
                                Revenue = passiveEarnings.Revenue,
                                FuelCost = passiveEarnings.FuelCost,
                                MaintenanceCost = passiveEarnings.MaintenanceCost,
                                InsuranceCost = passiveEarnings.InsuranceCost,
                                DepreciationCost = passiveEarnings.DepreciationCost,
                                CrewCost = passiveEarnings.CrewCost,
                                FBOCost = passiveEarnings.FBOCost,
                                ServiceBonusAmount = passiveEarnings.ServiceBonusAmount
                            };
                            dailyDetails.Add(detail);

                            dailyTotalRevenue += passiveEarnings.Revenue + passiveEarnings.ServiceBonusAmount;
                            dailyTotalCosts += passiveEarnings.TotalCost;
                            totalEarnings += passiveEarnings.Profit;

                            // Update aircraft state
                            aircraft.TotalFlightHours += dailyFlightHours;
                            aircraft.HoursSinceLastMaintenance += dailyFlightHours;
                            ApplyDailyDepreciation(aircraft, settings);

                            // Update pilot flight hours
                            if (pilot != null)
                            {
                                pilot.TotalFlightHours += dailyFlightHours;
                                pilot.TotalDistanceNM += passiveEarnings.DailyDistanceNM;
                            }

                            _loggingService.Debug($"DailyEarningsService: {earningsDate:yyyy-MM-dd} - {aircraft.Registration}: " +
                                $"Revenue €{passiveEarnings.Revenue:N2}, Costs €{passiveEarnings.TotalCost:N2}, Profit €{passiveEarnings.Profit:N2}");
                        }

                        // Create single summary transaction for the day (net profit)
                        var dailyProfit = dailyTotalRevenue - dailyTotalCosts;
                        if (dailyTotalRevenue > 0 || dailyTotalCosts > 0)
                        {
                            db.Transactions.Add(new Transaction
                            {
                                Date = earningsDate,
                                Amount = dailyProfit,
                                Type = dailyProfit >= 0 ? "Income" : "Expense",
                                Description = $"Daily earnings: {earningsDate:dd.MM.yyyy}"
                            });
                        }

                        // Save detail entries
                        db.DailyEarningsDetails.AddRange(dailyDetails);

                        _loggingService.Info($"DailyEarningsService: {earningsDate:yyyy-MM-dd} - " +
                            $"Total Revenue €{dailyTotalRevenue:N2}, Total Costs €{dailyTotalCosts:N2}, " +
                            $"Profit €{(dailyTotalRevenue - dailyTotalCosts):N2} ({assignedAircraft.Count} aircraft)");

                        // Check for month/year end and create summary transactions
                        var nextDay = earningsDate.AddDays(1);

                        // Monthly summary at end of month
                        if (nextDay.Month != earningsDate.Month)
                        {
                            CreateMonthlySummaryTransaction(db, earningsDate.Year, earningsDate.Month);
                        }

                        // Yearly summary at end of year
                        if (nextDay.Year != earningsDate.Year)
                        {
                            CreateYearlySummaryTransaction(db, earningsDate.Year);
                        }
                    }

                    settings.LastDailyEarningsDate = today;
                    db.SaveChanges();

                    _financeService.LoadTransactions();
                    _loggingService.Info($"DailyEarningsService: Completed, total earnings: €{totalEarnings:N2}");

                    CheckFinanceAchievements(db);
                });
            }
            catch (Exception ex)
            {
                _loggingService.Error("DailyEarningsService: Failed to process daily earnings", ex);
            }
        }

        private void CheckFinanceAchievements(AceDbContext db)
        {
            var balance = _financeService.Balance;
            var fleetValue = db.Aircraft.Sum(a => a.CurrentValue);
            var totalAssets = balance + fleetValue;

            var transactions = db.Transactions.ToList();
            var totalRevenue = transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
            var totalExpenses = transactions.Where(t => t.Type == "Expense").Sum(t => Math.Abs(t.Amount));
            var totalProfit = totalRevenue - totalExpenses;

            _achievementService.CheckFinanceAchievements(totalAssets, totalRevenue, totalProfit);

            var aircraftCount = db.Aircraft.Count();
            _achievementService.CheckFleetAchievements(aircraftCount, fleetValue);
        }

        private void ApplyDailyDepreciation(Aircraft aircraft, AppSettingsEntity settings)
        {
            if (aircraft.CurrentValue == 0)
            {
                aircraft.CurrentValue = aircraft.PurchasePrice;
            }

            decimal dailyDepreciationRate = settings.AircraftDepreciationRate / 365m;
            decimal depreciation = aircraft.CurrentValue * dailyDepreciationRate;
            decimal newValue = aircraft.CurrentValue - depreciation;

            if (newValue < 0)
                newValue = 0;

            aircraft.CurrentValue = newValue;
        }

        private void CreateMonthlySummaryTransaction(AceDbContext db, int year, int month)
        {
            var monthlyDetails = db.DailyEarningsDetails
                .Where(d => d.Date.Year == year && d.Date.Month == month)
                .ToList();

            if (!monthlyDetails.Any())
                return;

            var totalRevenue = monthlyDetails.Sum(d => d.Revenue);
            var totalCosts = monthlyDetails.Sum(d => d.TotalCost);
            var totalProfit = totalRevenue - totalCosts;

            db.Transactions.Add(new Transaction
            {
                Date = new DateTime(year, month, DateTime.DaysInMonth(year, month)),
                Amount = 0,
                Type = "Summary",
                Description = $"Monthly summary: {month:D2}.{year}"
            });

            _loggingService.Info($"DailyEarningsService: Created monthly summary for {month:D2}/{year} - " +
                $"Revenue €{totalRevenue:N2}, Costs €{totalCosts:N2}, Profit €{totalProfit:N2}");
        }

        private void CreateYearlySummaryTransaction(AceDbContext db, int year)
        {
            var yearlyDetails = db.DailyEarningsDetails
                .Where(d => d.Date.Year == year)
                .ToList();

            if (!yearlyDetails.Any())
                return;

            var totalRevenue = yearlyDetails.Sum(d => d.Revenue);
            var totalCosts = yearlyDetails.Sum(d => d.TotalCost);
            var totalProfit = totalRevenue - totalCosts;

            db.Transactions.Add(new Transaction
            {
                Date = new DateTime(year, 12, 31),
                Amount = 0,
                Type = "Summary",
                Description = $"Yearly summary: {year}"
            });

            _loggingService.Info($"DailyEarningsService: Created yearly summary for {year} - " +
                $"Revenue €{totalRevenue:N2}, Costs €{totalCosts:N2}, Profit €{totalProfit:N2}");
        }
    }
}
