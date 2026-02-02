using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Services
{
    public class PricingService : IPricingService
    {
        private readonly ISettingsService _settingsService;
        private readonly ILoggingService _loggingService;
        private readonly IServiceProvider _serviceProvider;

        public PricingService(ISettingsService settingsService, ILoggingService loggingService, IServiceProvider serviceProvider)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public FlightPriceBreakdown CalculateFlightPrice(Aircraft aircraft, double distanceNM, int passengers, double flightHours)
        {
            return CalculateCombinedFlightPrice(aircraft, distanceNM, passengers, aircraft.MaxCargoKg, flightHours);
        }

        public FlightPriceBreakdown CalculateCombinedFlightPrice(Aircraft aircraft, double distanceNM, int passengers, double cargoKg, double flightHours)
        {
            var settings = _settingsService.CurrentSettings;

            int effectivePassengers = (int)(passengers * (settings.PassengerLoadFactorPercent / 100m));
            double effectiveCargoKg = cargoKg * (double)(settings.CargoLoadFactorPercent / 100m);

            var breakdown = new FlightPriceBreakdown
            {
                Passengers = effectivePassengers,
                CargoKg = effectiveCargoKg,
                DistanceNM = distanceNM,
                FlightHours = flightHours
            };

            breakdown.FuelCost = CalculateFuelCost(aircraft, flightHours, settings);
            breakdown.MaintenanceCost = CalculateMaintenanceCost(aircraft, flightHours, settings);
            breakdown.DepreciationCost = CalculateDepreciationCostPerFlight(aircraft, flightHours, settings);
            breakdown.InsuranceCost = CalculateInsuranceCostPerFlight(aircraft, flightHours, settings);
            breakdown.CrewCost = CalculateCrewCostPerFlight(flightHours, settings);
            breakdown.LandingFees = CalculateLandingFees(aircraft, settings);
            breakdown.GroundHandlingCost = CalculateGroundHandlingCost(aircraft, settings);
            breakdown.CateringCost = CalculateCateringCost(effectivePassengers, settings);

            decimal baseCosts = breakdown.FuelCost + breakdown.MaintenanceCost + breakdown.DepreciationCost +
                breakdown.InsuranceCost + breakdown.CrewCost + breakdown.LandingFees +
                breakdown.GroundHandlingCost + breakdown.CateringCost;
            breakdown.FBOCost = baseCosts * settings.FBOCostFactor;

            decimal roiPerHour = CalculateROIPerFlightHour(aircraft.PurchasePrice, aircraft.MaxPassengers, aircraft.IsOldtimer);
            decimal roiRevenue = roiPerHour * (decimal)flightHours;

            decimal paxRate = GetRateForAircraftSize(aircraft.MaxPassengers);
            decimal paxNmRevenue = effectivePassengers * (decimal)distanceNM * paxRate;

            decimal cargoRate = GetCargoRateForAircraftSize(aircraft.MaxPassengers);
            decimal cargoNmRevenue = (decimal)effectiveCargoKg * (decimal)distanceNM * cargoRate;

            breakdown.PassengerRevenue = roiRevenue + paxNmRevenue;
            breakdown.CargoRevenue = cargoNmRevenue;

            breakdown.TotalPrice = breakdown.TotalOperatingCost + breakdown.PassengerRevenue + breakdown.CargoRevenue;

            _loggingService.Debug($"PricingService: Flight {distanceNM:F0}NM, {passengers}PAX, {cargoKg:F0}kg, {flightHours:F1}h - " +
                $"Fuel: €{breakdown.FuelCost:N2}, Maint: €{breakdown.MaintenanceCost:N2}, " +
                $"Depr: €{breakdown.DepreciationCost:N2}, Ins: €{breakdown.InsuranceCost:N2}, " +
                $"Crew: €{breakdown.CrewCost:N2}, Landing: €{breakdown.LandingFees:N2}, " +
                $"Ground: €{breakdown.GroundHandlingCost:N2}, Catering: €{breakdown.CateringCost:N2}, " +
                $"FBO: €{breakdown.FBOCost:N2}, OpCost: €{breakdown.TotalOperatingCost:N2}, " +
                $"PaxRev: €{breakdown.PassengerRevenue:N2}, CargoRev: €{breakdown.CargoRevenue:N2}, " +
                $"Total: €{breakdown.TotalPrice:N2}, Profit: €{breakdown.Profit:N2}");

            return breakdown;
        }

        public DailyPassiveEarnings CalculateDailyPassiveEarnings(Aircraft aircraft, double dailyFlightHours)
        {
            return CalculateDailyPassiveEarnings(aircraft, dailyFlightHours, (Pilot?)null);
        }

        public DailyPassiveEarnings CalculateDailyPassiveEarnings(Aircraft aircraft, double dailyFlightHours, Pilot? pilot)
        {
            var settings = _settingsService.CurrentSettings;
            double dailyDistance = aircraft.CruiseSpeedKts * dailyFlightHours;

            int effectivePassengers = (int)(aircraft.MaxPassengers * (settings.PassengerLoadFactorPercent / 100m));
            double effectiveCargoKg = aircraft.MaxCargoKg * (double)(settings.CargoLoadFactorPercent / 100m);

            var earnings = new DailyPassiveEarnings
            {
                DailyFlightHours = dailyFlightHours,
                DailyDistanceNM = dailyDistance,
                Passengers = effectivePassengers
            };

            earnings.FuelCost = CalculateFuelCost(aircraft, dailyFlightHours, settings);
            earnings.MaintenanceCost = CalculateMaintenanceCost(aircraft, dailyFlightHours, settings);
            earnings.DepreciationCost = CalculateDailyDepreciation(aircraft, settings);
            earnings.InsuranceCost = CalculateDailyInsurance(aircraft, settings);

            if (pilot != null && !pilot.IsPlayer && pilot.SalaryPerMonth > 0)
            {
                earnings.CrewCost = pilot.SalaryPerMonth / 30m;
            }
            else
            {
                earnings.CrewCost = 0m;
            }

            decimal baseCosts = earnings.FuelCost + earnings.MaintenanceCost + earnings.DepreciationCost +
                earnings.InsuranceCost + earnings.CrewCost;
            earnings.FBOCost = baseCosts * settings.FBOCostFactor;

            decimal roiPerHour = CalculateROIPerFlightHour(aircraft.PurchasePrice, aircraft.MaxPassengers, aircraft.IsOldtimer);
            decimal dailyROI = roiPerHour * (decimal)dailyFlightHours;

            decimal paxRate = GetRateForAircraftSize(aircraft.MaxPassengers);
            decimal paxNmRevenue = effectivePassengers * (decimal)dailyDistance * paxRate;

            decimal cargoRate = GetCargoRateForAircraftSize(aircraft.MaxPassengers);
            decimal cargoNmRevenue = (decimal)effectiveCargoKg * (decimal)dailyDistance * cargoRate;

            earnings.Revenue = earnings.TotalCost + dailyROI + paxNmRevenue + cargoNmRevenue;

            earnings.ServiceBonusAmount = CalculateRouteServiceBonus(aircraft, dailyFlightHours, settings);

            var pilotInfo = pilot?.IsPlayer == true ? "Player" : (pilot?.Name ?? "Unknown");
            var serviceBonusInfo = earnings.ServiceBonusAmount > 0 ? $", ServiceBonus: €{earnings.ServiceBonusAmount:N2}" : "";
            _loggingService.Debug($"PricingService: Daily passive earnings for {aircraft.Registration} (Pilot: {pilotInfo}): " +
                $"{dailyFlightHours:F1}h, {dailyDistance:F0}NM, {effectivePassengers}PAX (load: {settings.PassengerLoadFactorPercent}%) - " +
                $"Revenue: €{earnings.Revenue:N2}, Costs: €{earnings.TotalCost:N2} (Crew: €{earnings.CrewCost:N2}, FBO: €{earnings.FBOCost:N2}){serviceBonusInfo}, Profit: €{earnings.Profit:N2}");

            return earnings;
        }

        public DailyPassiveEarnings CalculateDailyPassiveEarnings(Aircraft aircraft, double dailyFlightHours, List<Pilot> assignedPilots)
        {
            var settings = _settingsService.CurrentSettings;
            double dailyDistance = aircraft.CruiseSpeedKts * dailyFlightHours;

            int effectivePassengers = (int)(aircraft.MaxPassengers * (settings.PassengerLoadFactorPercent / 100m));
            double effectiveCargoKg = aircraft.MaxCargoKg * (double)(settings.CargoLoadFactorPercent / 100m);

            var earnings = new DailyPassiveEarnings
            {
                DailyFlightHours = dailyFlightHours,
                DailyDistanceNM = dailyDistance,
                Passengers = effectivePassengers
            };

            earnings.FuelCost = CalculateFuelCost(aircraft, dailyFlightHours, settings);
            earnings.MaintenanceCost = CalculateMaintenanceCost(aircraft, dailyFlightHours, settings);
            earnings.DepreciationCost = CalculateDailyDepreciation(aircraft, settings);
            earnings.InsuranceCost = CalculateDailyInsurance(aircraft, settings);

            earnings.CrewCost = assignedPilots
                .Where(p => !p.IsPlayer && p.SalaryPerMonth > 0)
                .Sum(p => p.SalaryPerMonth / 30m);

            decimal baseCosts = earnings.FuelCost + earnings.MaintenanceCost + earnings.DepreciationCost +
                earnings.InsuranceCost + earnings.CrewCost;
            earnings.FBOCost = baseCosts * settings.FBOCostFactor;

            decimal roiPerHour = CalculateROIPerFlightHour(aircraft.PurchasePrice, aircraft.MaxPassengers, aircraft.IsOldtimer);
            decimal dailyROI = roiPerHour * (decimal)dailyFlightHours;

            decimal paxRate = GetRateForAircraftSize(aircraft.MaxPassengers);
            decimal paxNmRevenue = effectivePassengers * (decimal)dailyDistance * paxRate;

            decimal cargoRate = GetCargoRateForAircraftSize(aircraft.MaxPassengers);
            decimal cargoNmRevenue = (decimal)effectiveCargoKg * (decimal)dailyDistance * cargoRate;

            earnings.Revenue = earnings.TotalCost + dailyROI + paxNmRevenue + cargoNmRevenue;

            earnings.ServiceBonusAmount = CalculateRouteServiceBonus(aircraft, dailyFlightHours, settings);

            var pilotNames = string.Join(", ", assignedPilots.Select(p => p.IsPlayer ? "Player" : p.Name));
            var serviceBonusInfo = earnings.ServiceBonusAmount > 0 ? $", ServiceBonus: €{earnings.ServiceBonusAmount:N2}" : "";
            _loggingService.Debug($"PricingService: Daily passive earnings for {aircraft.Registration} (Pilots: {pilotNames}): " +
                $"{dailyFlightHours:F1}h, {dailyDistance:F0}NM, {effectivePassengers}PAX - " +
                $"Revenue: €{earnings.Revenue:N2}, Costs: €{earnings.TotalCost:N2} (Crew: €{earnings.CrewCost:N2}){serviceBonusInfo}, Profit: €{earnings.Profit:N2}");

            return earnings;
        }

        public decimal CalculateProfitPerHour(AircraftCatalogEntry catalogEntry)
        {
            var settings = _settingsService.CurrentSettings;
            decimal dailyFlightHours = settings.PilotFlightHoursPerDay;
            if (dailyFlightHours <= 0) dailyFlightHours = 8m;

            var tempAircraft = new Aircraft
            {
                PurchasePrice = catalogEntry.MarketPrice,
                CurrentValue = catalogEntry.MarketPrice,
                MaxPassengers = catalogEntry.PassengerCapacity,
                MaxCargoKg = catalogEntry.MaxCargoKg,
                CruiseSpeedKts = catalogEntry.CruiseSpeedKts,
                FuelBurnGalPerHour = catalogEntry.FuelBurnGalPerHour,
                IsOldtimer = catalogEntry.IsOldtimer
            };

            decimal rookieSalary = PilotRank.CalculateAdjustedSalary(PilotRankType.Junior, settings);
            var tempPilot = new Pilot { IsPlayer = false, SalaryPerMonth = rookieSalary };

            var earnings = CalculateDailyPassiveEarnings(tempAircraft, (double)dailyFlightHours, tempPilot);

            return earnings.Profit / dailyFlightHours;
        }

        public DailyOperatingCosts CalculateDailyOperatingCosts(Aircraft aircraft, double flightHours)
        {
            var settings = _settingsService.CurrentSettings;

            var costs = new DailyOperatingCosts
            {
                FuelCost = CalculateFuelCost(aircraft, flightHours, settings),
                MaintenanceCost = CalculateMaintenanceCost(aircraft, flightHours, settings),
                DepreciationCost = CalculateDailyDepreciation(aircraft, settings),
                InsuranceCost = CalculateDailyInsurance(aircraft, settings),
                CrewCost = CalculateDailyCrewCost(settings)
            };

            decimal baseCosts = costs.FuelCost + costs.MaintenanceCost + costs.DepreciationCost +
                costs.InsuranceCost + costs.CrewCost;
            costs.FBOCost = baseCosts * settings.FBOCostFactor;

            return costs;
        }

        public decimal GetRateForAircraftSize(int maxPassengers)
        {
            var settings = _settingsService.CurrentSettings;
            var size = AircraftSizeExtensions.GetAircraftSize(maxPassengers);

            return size switch
            {
                AircraftSize.Small => settings.RatePerPaxPerNMSmall,
                AircraftSize.Medium => settings.RatePerPaxPerNMMedium,
                AircraftSize.MediumLarge => settings.RatePerPaxPerNMMediumLarge,
                AircraftSize.Large => settings.RatePerPaxPerNMLarge,
                AircraftSize.VeryLarge => settings.RatePerPaxPerNMVeryLarge,
                _ => settings.RatePerPaxPerNMMedium
            };
        }

        public decimal GetCargoRateForAircraftSize(int maxPassengers)
        {
            var settings = _settingsService.CurrentSettings;
            var size = AircraftSizeExtensions.GetAircraftSize(maxPassengers);

            return size switch
            {
                AircraftSize.Small => settings.CargoRatePerKgPerNMSmall,
                AircraftSize.Medium => settings.CargoRatePerKgPerNMMedium,
                AircraftSize.MediumLarge => settings.CargoRatePerKgPerNMMediumLarge,
                AircraftSize.Large => settings.CargoRatePerKgPerNMLarge,
                AircraftSize.VeryLarge => settings.CargoRatePerKgPerNMVeryLarge,
                _ => settings.CargoRatePerKgPerNMMedium
            };
        }

        public decimal GetROIPercentForSize(int maxPassengers)
        {
            var settings = _settingsService.CurrentSettings;
            var size = AircraftSizeExtensions.GetAircraftSize(maxPassengers);

            return size switch
            {
                AircraftSize.Small => settings.ROIPercentSmall,
                AircraftSize.Medium => settings.ROIPercentMedium,
                AircraftSize.MediumLarge => settings.ROIPercentMediumLarge,
                AircraftSize.Large => settings.ROIPercentLarge,
                AircraftSize.VeryLarge => settings.ROIPercentVeryLarge,
                _ => settings.ROIPercentMedium
            };
        }

        public decimal CalculateROIPerFlightHour(decimal purchasePrice, int maxPassengers, bool isOldtimer)
        {
            var settings = _settingsService.CurrentSettings;
            decimal roiPercent = GetROIPercentForSize(maxPassengers);

            if (isOldtimer)
            {
                decimal malusMultiplier = 1m - (settings.OldtimerROIMalusPercent / 100m);
                roiPercent *= malusMultiplier;
            }

            decimal annualROI = purchasePrice * (roiPercent / 100m);
            decimal hoursPerYear = settings.PilotFlightHoursPerDay * 365m;
            if (hoursPerYear <= 0) hoursPerYear = 2920m;

            return annualROI / hoursPerYear;
        }

        public decimal CalculateCargoFlightPrice(Aircraft aircraft, double distanceNM, double cargoKg, double flightHours)
        {
            var settings = _settingsService.CurrentSettings;

            double effectiveCargoKg = cargoKg * (double)(settings.CargoLoadFactorPercent / 100m);

            decimal fuelCost = CalculateFuelCost(aircraft, flightHours, settings);
            decimal maintenanceCost = CalculateMaintenanceCost(aircraft, flightHours, settings);
            decimal depreciationCost = CalculateDepreciationCostPerFlight(aircraft, flightHours, settings);
            decimal insuranceCost = CalculateInsuranceCostPerFlight(aircraft, flightHours, settings);
            decimal crewCost = CalculateCrewCostPerFlight(flightHours, settings);
            decimal landingFees = CalculateLandingFees(aircraft, settings);
            decimal groundHandlingCost = CalculateGroundHandlingCost(aircraft, settings);

            decimal baseCosts = fuelCost + maintenanceCost + depreciationCost + insuranceCost + crewCost + landingFees + groundHandlingCost;
            decimal fboCost = baseCosts * settings.FBOCostFactor;
            decimal totalOperatingCost = baseCosts + fboCost;

            decimal roiPerHour = CalculateROIPerFlightHour(aircraft.PurchasePrice, aircraft.MaxPassengers, aircraft.IsOldtimer);
            decimal roiRevenue = roiPerHour * (decimal)flightHours;

            decimal cargoRate = GetCargoRateForAircraftSize(aircraft.MaxPassengers);
            decimal cargoNmRevenue = (decimal)effectiveCargoKg * (decimal)distanceNM * cargoRate;

            decimal totalPrice = totalOperatingCost + roiRevenue + cargoNmRevenue;

            _loggingService.Debug($"PricingService: Cargo flight {distanceNM:F0}NM, {effectiveCargoKg:F0}kg (load: {settings.CargoLoadFactorPercent}%) - " +
                $"OpCost: €{totalOperatingCost:N2}, ROI: €{roiRevenue:N2}, CargoRev: €{cargoNmRevenue:N2}, Total: €{totalPrice:N2}");

            return totalPrice;
        }

        private decimal CalculateFuelCost(Aircraft aircraft, double flightHours, AppSettings settings)
        {
            decimal fuelConsumed = (decimal)(aircraft.FuelBurnGalPerHour * flightHours);
            return fuelConsumed * settings.FuelPricePerGallon;
        }

        private decimal CalculateMaintenanceCost(Aircraft aircraft, double flightHours, AppSettings settings)
        {
            var size = AircraftSizeExtensions.GetAircraftSize(aircraft.MaxPassengers);
            decimal costPerHour = size switch
            {
                AircraftSize.Small => settings.MaintenanceCostPerHourSmall,
                AircraftSize.Medium => settings.MaintenanceCostPerHourMedium,
                AircraftSize.MediumLarge => settings.MaintenanceCostPerHourMediumLarge,
                AircraftSize.Large => settings.MaintenanceCostPerHourLarge,
                AircraftSize.VeryLarge => settings.MaintenanceCostPerHourVeryLarge,
                _ => settings.MaintenanceCostPerHourMedium
            };
            return (decimal)flightHours * costPerHour;
        }

        private decimal CalculateDepreciationCostPerFlight(Aircraft aircraft, double flightHours, AppSettings settings)
        {
            decimal currentValue = aircraft.CurrentValue > 0 ? aircraft.CurrentValue : aircraft.PurchasePrice;
            decimal annualDepreciation = currentValue * settings.AircraftDepreciationRate;
            decimal hoursPerYear = settings.PilotFlightHoursPerDay * 365m;
            if (hoursPerYear <= 0) hoursPerYear = 2000m;
            decimal depreciationPerHour = annualDepreciation / hoursPerYear;
            return depreciationPerHour * (decimal)flightHours;
        }

        private decimal CalculateInsuranceCostPerFlight(Aircraft aircraft, double flightHours, AppSettings settings)
        {
            decimal annualInsurance = aircraft.PurchasePrice * settings.InsuranceRatePercentage;
            decimal hoursPerYear = settings.PilotFlightHoursPerDay * 365m;
            if (hoursPerYear <= 0) hoursPerYear = 2000m;
            decimal insurancePerHour = annualInsurance / hoursPerYear;
            return insurancePerHour * (decimal)flightHours;
        }

        private decimal CalculateCrewCostPerFlight(double flightHours, AppSettings settings)
        {
            decimal monthlyCrewCost = settings.PilotBaseSalary * settings.CrewCostMultiplier;
            decimal hoursPerMonth = settings.PilotFlightHoursPerDay * 30m;
            if (hoursPerMonth <= 0) hoursPerMonth = 165m;
            decimal crewCostPerHour = monthlyCrewCost / hoursPerMonth;
            return crewCostPerHour * (decimal)flightHours;
        }

        private decimal CalculateLandingFees(Aircraft aircraft, AppSettings settings)
        {
            var size = AircraftSizeExtensions.GetAircraftSize(aircraft.MaxPassengers);
            return size switch
            {
                AircraftSize.Small => settings.LandingFeeSmall,
                AircraftSize.Medium => settings.LandingFeeMedium,
                AircraftSize.MediumLarge => settings.LandingFeeMediumLarge,
                AircraftSize.Large => settings.LandingFeeLarge,
                AircraftSize.VeryLarge => settings.LandingFeeVeryLarge,
                _ => settings.LandingFeeMedium
            };
        }

        private decimal CalculateGroundHandlingCost(Aircraft aircraft, AppSettings settings)
        {
            // Scale ground handling cost based on aircraft size
            // Small GA aircraft don't need expensive ground handling
            var size = AircraftSizeExtensions.GetAircraftSize(aircraft.MaxPassengers);
            decimal baseCost = settings.ServiceCostGroundHandling;

            return size switch
            {
                AircraftSize.Small => baseCost * 0.05m,        // 5% - minimal handling for GA
                AircraftSize.Medium => baseCost * 0.15m,       // 15% - small turboprops
                AircraftSize.MediumLarge => baseCost * 0.40m,  // 40% - regional jets
                AircraftSize.Large => baseCost * 0.70m,        // 70% - narrow-body
                AircraftSize.VeryLarge => baseCost,            // 100% - wide-body
                _ => baseCost * 0.25m
            };
        }

        private decimal CalculateCateringCost(int passengers, AppSettings settings)
        {
            return passengers * settings.CateringCostPerPassenger;
        }

        private decimal CalculateDailyDepreciation(Aircraft aircraft, AppSettings settings)
        {
            decimal currentValue = aircraft.CurrentValue > 0 ? aircraft.CurrentValue : aircraft.PurchasePrice;
            decimal dailyDepreciationRate = settings.AircraftDepreciationRate / 365m;
            return currentValue * dailyDepreciationRate;
        }

        private decimal CalculateDailyInsurance(Aircraft aircraft, AppSettings settings)
        {
            decimal annualInsurance = aircraft.PurchasePrice * settings.InsuranceRatePercentage;
            return annualInsurance / 365m;
        }

        private decimal CalculateDailyCrewCost(AppSettings settings)
        {
            decimal monthlyCrewCost = settings.PilotBaseSalary * settings.CrewCostMultiplier;
            return monthlyCrewCost / 30m;
        }

        public bool IsNetworkFlight(string departureIcao, string arrivalIcao)
        {
            if (string.IsNullOrEmpty(departureIcao) || string.IsNullOrEmpty(arrivalIcao))
                return false;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var fboRepository = scope.ServiceProvider.GetRequiredService<IFBORepository>();

                var departureFbo = fboRepository.GetFBOByICAO(departureIcao);
                var arrivalFbo = fboRepository.GetFBOByICAO(arrivalIcao);

                bool isNetwork = departureFbo != null && arrivalFbo != null;
                if (isNetwork)
                {
                    _loggingService.Debug($"PricingService: Network flight detected: {departureIcao} → {arrivalIcao} (both are owned FBOs)");
                }

                return isNetwork;
            }
            catch (Exception ex)
            {
                _loggingService.Error($"PricingService: Error checking network flight: {ex.Message}");
                return false;
            }
        }

        public decimal GetNetworkBonusPercent()
        {
            return _settingsService.CurrentSettings.NetworkBonusPercent;
        }

        private decimal CalculateRouteServiceBonus(Aircraft aircraft, double dailyFlightHours, AppSettings settings)
        {
            if (settings.ServiceBonusFactorPercent <= 0)
                return 0m;

            try
            {
                using var db = new AceDbContext();

                var routes = db.ScheduledRoutes
                    .Where(r => r.AssignedAircraftId == aircraft.Id && r.IsActive)
                    .ToList();

                if (!routes.Any())
                    return 0m;

                var fboIds = routes.SelectMany(r => new[] { r.OriginFBOId, r.DestinationFBOId }).Distinct().ToList();
                var fbos = db.FBOs.Where(f => fboIds.Contains(f.Id)).ToList();

                decimal totalServiceBonus = 0m;

                foreach (var route in routes)
                {
                    var originFBO = fbos.FirstOrDefault(f => f.Id == route.OriginFBOId);
                    var destinationFBO = fbos.FirstOrDefault(f => f.Id == route.DestinationFBOId);

                    if (originFBO == null || destinationFBO == null)
                        continue;

                    decimal originServiceCost = CalculateFBOServiceCost(originFBO, settings);
                    decimal destinationServiceCost = CalculateFBOServiceCost(destinationFBO, settings);
                    decimal totalRouteMonthlyCost = originServiceCost + destinationServiceCost;

                    if (totalRouteMonthlyCost <= 0)
                        continue;

                    decimal dailyServiceCost = totalRouteMonthlyCost / 30m;
                    decimal routeBonus = dailyServiceCost * (settings.ServiceBonusFactorPercent / 100m);

                    totalServiceBonus += routeBonus;
                }

                return totalServiceBonus;
            }
            catch (Exception ex)
            {
                _loggingService.Error($"PricingService: Error calculating route service bonus: {ex.Message}");
                return 0m;
            }
        }

        private static decimal CalculateFBOServiceCost(FBO fbo, AppSettings settings)
        {
            decimal totalCost = 0m;

            if (fbo.HasRefuelingService)
            {
                totalCost += GetServiceCostByTerminalSize(fbo.TerminalSize,
                    settings.ServiceCostRefueling, settings.ServiceCostRefuelingMedium,
                    settings.ServiceCostRefuelingMediumLarge, settings.ServiceCostRefuelingLarge,
                    settings.ServiceCostRefuelingVeryLarge);
            }

            if (fbo.HasHangarService)
            {
                totalCost += GetServiceCostByTerminalSize(fbo.TerminalSize,
                    settings.ServiceCostHangar, settings.ServiceCostHangarMedium,
                    settings.ServiceCostHangarMediumLarge, settings.ServiceCostHangarLarge,
                    settings.ServiceCostHangarVeryLarge);
            }

            if (fbo.HasCateringService)
            {
                totalCost += GetServiceCostByTerminalSize(fbo.TerminalSize,
                    settings.ServiceCostCatering, settings.ServiceCostCateringMedium,
                    settings.ServiceCostCateringMediumLarge, settings.ServiceCostCateringLarge,
                    settings.ServiceCostCateringVeryLarge);
            }

            if (fbo.HasGroundHandling)
            {
                totalCost += GetServiceCostByTerminalSize(fbo.TerminalSize,
                    settings.ServiceCostGroundHandling, settings.ServiceCostGroundHandlingMedium,
                    settings.ServiceCostGroundHandlingMediumLarge, settings.ServiceCostGroundHandlingLarge,
                    settings.ServiceCostGroundHandlingVeryLarge);
            }

            if (fbo.HasDeIcingService)
            {
                totalCost += GetServiceCostByTerminalSize(fbo.TerminalSize,
                    settings.ServiceCostDeIcing, settings.ServiceCostDeIcingMedium,
                    settings.ServiceCostDeIcingMediumLarge, settings.ServiceCostDeIcingLarge,
                    settings.ServiceCostDeIcingVeryLarge);
            }

            return totalCost;
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
