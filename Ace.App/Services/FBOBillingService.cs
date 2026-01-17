using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Services
{
    public class FBOBillingService : IFBOBillingService
    {
        private readonly ILoggingService _loggingService;
        private readonly IFinanceService _financeService;
        private readonly ISettingsService _settingsService;

        public FBOBillingService(
            ILoggingService loggingService,
            IFinanceService financeService,
            ISettingsService settingsService)
        {
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        public async Task ProcessMonthlyFBOBilling()
        {
            _loggingService.Info("FBOBillingService: Starting monthly billing check");

            try
            {
                await Task.Run(() =>
                {
                    using var db = new AceDbContext();

                    var settings = db.Settings.FirstOrDefault();
                    if (settings == null)
                    {
                        _loggingService.Info("FBOBillingService: No settings found, skipping");
                        return;
                    }

                    var today = DateTime.Today;
                    var lastBillingDate = settings.LastFBOBillingDate?.Date;

                    _loggingService.Info($"FBOBillingService: Today={today:yyyy-MM-dd}, LastBillingDate={lastBillingDate?.ToString("yyyy-MM-dd") ?? "NULL"}");

                    if (lastBillingDate == null)
                    {
                        _loggingService.Info("FBOBillingService: First run, setting initial date to today (no billing)");
                        settings.LastFBOBillingDate = today;
                        db.SaveChanges();
                        return;
                    }

                    var monthsToProcess = GetMonthsToProcess(lastBillingDate.Value, today);

                    if (monthsToProcess.Count == 0)
                    {
                        _loggingService.Info("FBOBillingService: No new months to process");
                        return;
                    }

                    var fbos = db.FBOs.ToList();
                    var employedPilots = db.Pilots.Where(p => p.IsEmployed && !p.IsPlayer).ToList();
                    var appSettings = _settingsService.CurrentSettings;

                    _loggingService.Info($"FBOBillingService: Processing {monthsToProcess.Count} month(s) for {fbos.Count} FBO(s) and {employedPilots.Count} pilot(s)");

                    foreach (var billingMonth in monthsToProcess)
                    {
                        var billingDate = new DateTime(billingMonth.Year, billingMonth.Month, 1);
                        var billingDetails = new List<MonthlyBillingDetail>();
                        decimal totalMonthlyExpense = 0m;

                        // Process FBOs
                        foreach (var fbo in fbos)
                        {
                            var (totalCost, rentCost, terminalCost, servicesCost, servicesDesc) = CalculateFBOCosts(fbo, appSettings);

                            if (totalCost > 0)
                            {
                                billingDetails.Add(new MonthlyBillingDetail
                                {
                                    BillingDate = billingDate,
                                    Category = "FBO",
                                    Name = fbo.ICAO,
                                    Description = servicesDesc,
                                    Amount = totalCost,
                                    RentCost = rentCost,
                                    TerminalCost = terminalCost,
                                    ServicesCost = servicesCost
                                });

                                totalMonthlyExpense += totalCost;
                                _loggingService.Debug($"FBOBillingService: {billingDate:yyyy-MM-dd} - FBO {fbo.ICAO}: €{totalCost:N2}");
                            }
                        }

                        // Process Pilot Salaries
                        foreach (var pilot in employedPilots)
                        {
                            if (pilot.SalaryPerMonth > 0)
                            {
                                billingDetails.Add(new MonthlyBillingDetail
                                {
                                    BillingDate = billingDate,
                                    Category = "Pilot",
                                    Name = pilot.Name,
                                    Description = $"Monthly salary",
                                    Amount = pilot.SalaryPerMonth,
                                    BaseSalary = pilot.SalaryPerMonth,
                                    BonusPay = 0
                                });

                                totalMonthlyExpense += pilot.SalaryPerMonth;
                                _loggingService.Debug($"FBOBillingService: {billingDate:yyyy-MM-dd} - Pilot {pilot.Name}: €{pilot.SalaryPerMonth:N2}");
                            }
                        }

                        // Save billing details
                        if (billingDetails.Any())
                        {
                            db.MonthlyBillingDetails.AddRange(billingDetails);
                        }

                        // Create single summary transaction
                        if (totalMonthlyExpense > 0)
                        {
                            db.Transactions.Add(new Transaction
                            {
                                Date = billingDate,
                                Amount = -totalMonthlyExpense,
                                Type = "Expense",
                                Description = $"Monthly costs: {billingDate:MM.yyyy}"
                            });

                            _loggingService.Info($"FBOBillingService: {billingDate:yyyy-MM-dd} - Total monthly expense: €{totalMonthlyExpense:N2}");
                        }
                    }

                    settings.LastFBOBillingDate = today;
                    db.SaveChanges();

                    _financeService.LoadTransactions();
                    _loggingService.Info($"FBOBillingService: Completed billing for {monthsToProcess.Count} month(s)");
                });
            }
            catch (Exception ex)
            {
                _loggingService.Error("FBOBillingService: Failed to process monthly billing", ex);
            }
        }

        private List<(int Year, int Month)> GetMonthsToProcess(DateTime lastBillingDate, DateTime today)
        {
            var months = new List<(int Year, int Month)>();

            var startMonth = new DateTime(lastBillingDate.Year, lastBillingDate.Month, 1).AddMonths(1);
            var endMonth = new DateTime(today.Year, today.Month, 1);

            var currentMonth = startMonth;
            while (currentMonth <= endMonth)
            {
                months.Add((currentMonth.Year, currentMonth.Month));
                currentMonth = currentMonth.AddMonths(1);
            }

            return months;
        }

        private (decimal total, decimal rent, decimal terminal, decimal services, string servicesDesc) CalculateFBOCosts(FBO fbo, AppSettings settings)
        {
            decimal rentCost = fbo.MonthlyRent;
            decimal terminalCost = fbo.TerminalMonthlyCost;
            decimal servicesCost = 0m;
            var servicesList = new List<string>();

            if (fbo.HasRefuelingService)
            {
                servicesCost += GetServiceCostByTerminalSize(fbo.TerminalSize, settings.ServiceCostRefueling,
                    settings.ServiceCostRefuelingMedium, settings.ServiceCostRefuelingMediumLarge,
                    settings.ServiceCostRefuelingLarge, settings.ServiceCostRefuelingVeryLarge);
                servicesList.Add("Fuel");
            }

            if (fbo.HasHangarService)
            {
                servicesCost += GetServiceCostByTerminalSize(fbo.TerminalSize, settings.ServiceCostHangar,
                    settings.ServiceCostHangarMedium, settings.ServiceCostHangarMediumLarge,
                    settings.ServiceCostHangarLarge, settings.ServiceCostHangarVeryLarge);
                servicesList.Add("Hangar");
            }

            if (fbo.HasCateringService)
            {
                servicesCost += GetServiceCostByTerminalSize(fbo.TerminalSize, settings.ServiceCostCatering,
                    settings.ServiceCostCateringMedium, settings.ServiceCostCateringMediumLarge,
                    settings.ServiceCostCateringLarge, settings.ServiceCostCateringVeryLarge);
                servicesList.Add("Catering");
            }

            if (fbo.HasGroundHandling)
            {
                servicesCost += GetServiceCostByTerminalSize(fbo.TerminalSize, settings.ServiceCostGroundHandling,
                    settings.ServiceCostGroundHandlingMedium, settings.ServiceCostGroundHandlingMediumLarge,
                    settings.ServiceCostGroundHandlingLarge, settings.ServiceCostGroundHandlingVeryLarge);
                servicesList.Add("Ground");
            }

            if (fbo.HasDeIcingService)
            {
                servicesCost += GetServiceCostByTerminalSize(fbo.TerminalSize, settings.ServiceCostDeIcing,
                    settings.ServiceCostDeIcingMedium, settings.ServiceCostDeIcingMediumLarge,
                    settings.ServiceCostDeIcingLarge, settings.ServiceCostDeIcingVeryLarge);
                servicesList.Add("De-Ice");
            }

            var total = rentCost + terminalCost + servicesCost;
            var desc = servicesList.Any()
                ? $"Rent + Terminal + {string.Join(", ", servicesList)}"
                : "Rent + Terminal";

            return (total, rentCost, terminalCost, servicesCost, desc);
        }

        private static decimal GetServiceCostByTerminalSize(TerminalSize terminalSize, decimal small, decimal medium, decimal mediumLarge, decimal large, decimal veryLarge)
        {
            return terminalSize switch
            {
                TerminalSize.Small => small,
                TerminalSize.Medium => medium,
                TerminalSize.MediumLarge => mediumLarge,
                TerminalSize.Large => large,
                TerminalSize.VeryLarge => veryLarge,
                _ => small
            };
        }
    }
}
