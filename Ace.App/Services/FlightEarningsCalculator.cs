using System;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Services
{
    public class FlightEarningsCalculator : IFlightEarningsCalculator
    {
        private readonly ILoggingService _logger;
        private readonly ISettingsService _settingsService;
        private readonly IPricingService _pricingService;

        public FlightEarningsCalculator(
            ILoggingService logger,
            ISettingsService settingsService,
            IPricingService pricingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
        }

        public FlightEarningsResult CalculateEarnings(FlightEarningsRequest request)
        {
            var settings = _settingsService.CurrentSettings;
            var result = new FlightEarningsResult
            {
                PlayerBonusPercent = settings.PlayerFlightBonusPercent,
                IsNetworkFlight = _pricingService.IsNetworkFlight(request.DepartureIcao, request.ArrivalIcao),
                ServiceBonusPercent = settings.ServiceBonusFactorPercent,
            };

            result.NetworkBonusPercent = result.IsNetworkFlight ? _pricingService.GetNetworkBonusPercent() : 0m;

            var departureFBO = LoadDepartureFBO(request.DepartureIcao);
            if (departureFBO != null)
            {
                result.HasDepartureFBOServices = HasAnyServices(departureFBO);
                if (result.HasDepartureFBOServices)
                {
                    var totalServiceCost = CalculateTotalServiceCost(departureFBO, settings);
                    result.ServiceBonusAmount = totalServiceCost * (result.ServiceBonusPercent / 100m);
                }
            }

            Aircraft? aircraft = LoadAircraft(request.AircraftRegistration);
            result.AircraftFound = aircraft != null;

            if (aircraft != null)
            {
                CalculateWithAircraft(request, result, aircraft);
            }
            else
            {
                CalculateWithoutAircraft(request, result);
            }

            LogEarningsBreakdown(request, result);

            return result;
        }

        private Aircraft? LoadAircraft(string registration)
        {
            try
            {
                using var db = new AceDbContext();
                return db.Aircraft.FirstOrDefault(a => a.Registration == registration);
            }
            catch (Exception ex)
            {
                _logger.Error($"FlightEarningsCalculator: Failed to load aircraft from database: {ex.Message}");
                return null;
            }
        }

        private void CalculateWithAircraft(FlightEarningsRequest request, FlightEarningsResult result, Aircraft aircraft)
        {
            var priceBreakdown = _pricingService.CalculateFlightPrice(
                aircraft,
                request.DistanceNM,
                request.Passengers,
                request.FlightHours);

            result.PriceBreakdown = priceBreakdown;
            result.BaseRevenue = priceBreakdown.TotalPrice;
            result.TotalOperatingCost = priceBreakdown.TotalOperatingCost;

            result.PlayerBonusAmount = result.BaseRevenue * (result.PlayerBonusPercent / 100m);
            result.NetworkBonusAmount = result.BaseRevenue * (result.NetworkBonusPercent / 100m);

            decimal totalBonusRevenue = result.BaseRevenue + result.PlayerBonusAmount + result.NetworkBonusAmount + result.ServiceBonusAmount;
            result.TotalEarnings = totalBonusRevenue - result.TotalOperatingCost;
        }

        private void CalculateWithoutAircraft(FlightEarningsRequest request, FlightEarningsResult result)
        {
            int passengers = request.Passengers > 0 ? request.Passengers : 4;
            decimal ratePerPaxPerNM = _pricingService.GetRateForAircraftSize(passengers);

            result.BaseRevenue = passengers * (decimal)request.DistanceNM * ratePerPaxPerNM;
            result.TotalOperatingCost = 0m;

            result.PlayerBonusAmount = result.BaseRevenue * (result.PlayerBonusPercent / 100m);
            result.NetworkBonusAmount = result.BaseRevenue * (result.NetworkBonusPercent / 100m);

            result.TotalEarnings = result.BaseRevenue + result.PlayerBonusAmount + result.NetworkBonusAmount + result.ServiceBonusAmount;
        }

        private void LogEarningsBreakdown(FlightEarningsRequest request, FlightEarningsResult result)
        {
            string manualSuffix = request.IsManualCompletion ? " [Manual]" : "";

            if (result.AircraftFound && result.PriceBreakdown != null)
            {
                var pb = result.PriceBreakdown;
                _logger.Info($"Flight earnings{manualSuffix}: Distance={request.DistanceNM:F0}NM, {request.Passengers}PAX, {request.FlightHours:F2}h");
                _logger.Info($"  Fuel: €{pb.FuelCost:N2}, Maint: €{pb.MaintenanceCost:N2}, Depr: €{pb.DepreciationCost:N2}, Ins: €{pb.InsuranceCost:N2}");
                _logger.Info($"  Crew: €{pb.CrewCost:N2}, Landing: €{pb.LandingFees:N2}, Ground: €{pb.GroundHandlingCost:N2}, Catering: €{pb.CateringCost:N2}");
                _logger.Info($"  OpCost: €{result.TotalOperatingCost:N2}, Revenue: €{result.BaseRevenue:N2}, Base Profit: €{pb.Profit:N2}");

                if (result.PlayerBonusPercent > 0)
                {
                    _logger.Info($"  Player bonus: {result.PlayerBonusPercent}% on gross = +€{result.PlayerBonusAmount:N2}");
                }
                if (result.IsNetworkFlight && result.NetworkBonusPercent > 0)
                {
                    _logger.Info($"  Network bonus: {result.NetworkBonusPercent}% on gross = +€{result.NetworkBonusAmount:N2} (FBO-to-FBO flight)");
                }
                if (result.HasDepartureFBOServices && result.ServiceBonusAmount > 0)
                {
                    _logger.Info($"  Service bonus: {result.ServiceBonusPercent}% of monthly cost = +€{result.ServiceBonusAmount:N2} (departure FBO services)");
                }
                _logger.Info($"  Final Profit: €{result.TotalEarnings:N2}");
            }
            else
            {
                _logger.Warn($"Flight earnings{manualSuffix}: Aircraft not found, using simple calculation: {request.Passengers} PAX × {request.DistanceNM:F0} NM = €{result.BaseRevenue:N2}");

                if (result.PlayerBonusPercent > 0)
                {
                    _logger.Info($"  Player bonus: {result.PlayerBonusPercent}% = +€{result.PlayerBonusAmount:N2}");
                }
                if (result.IsNetworkFlight && result.NetworkBonusPercent > 0)
                {
                    _logger.Info($"  Network bonus: {result.NetworkBonusPercent}% = +€{result.NetworkBonusAmount:N2} (FBO-to-FBO flight)");
                }
                if (result.HasDepartureFBOServices && result.ServiceBonusAmount > 0)
                {
                    _logger.Info($"  Service bonus: {result.ServiceBonusPercent}% = +€{result.ServiceBonusAmount:N2} (departure FBO services)");
                }
                _logger.Info($"  Total: €{result.TotalEarnings:N2}");
            }
        }

        private FBO? LoadDepartureFBO(string departureIcao)
        {
            try
            {
                using var db = new AceDbContext();
                return db.FBOs.FirstOrDefault(f => f.ICAO == departureIcao);
            }
            catch (Exception ex)
            {
                _logger.Error($"FlightEarningsCalculator: Failed to load departure FBO: {ex.Message}");
                return null;
            }
        }

        private static bool HasAnyServices(FBO fbo)
        {
            return fbo.HasRefuelingService ||
                   fbo.HasHangarService ||
                   fbo.HasCateringService ||
                   fbo.HasGroundHandling ||
                   fbo.HasDeIcingService;
        }

        private static decimal CalculateTotalServiceCost(FBO fbo, AppSettings settings)
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
