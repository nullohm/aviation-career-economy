using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;
using Xunit;

namespace Ace.App.Tests.Services
{
    public class PricingServiceTests
    {
        private readonly Mock<ISettingsService> _mockSettingsService;
        private readonly Mock<ILoggingService> _mockLoggingService;
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly PricingService _pricingService;
        private readonly AppSettings _testSettings;

        public PricingServiceTests()
        {
            _mockSettingsService = new Mock<ISettingsService>();
            _mockLoggingService = new Mock<ILoggingService>();
            _mockServiceProvider = new Mock<IServiceProvider>();

            _testSettings = new AppSettings
            {
                FuelPricePerGallon = 6.0m,
                MaintenanceCostPerHourSmall = 50m,
                MaintenanceCostPerHourMedium = 50m,
                MaintenanceCostPerHourMediumLarge = 50m,
                MaintenanceCostPerHourLarge = 50m,
                MaintenanceCostPerHourVeryLarge = 50m,
                AircraftDepreciationRate = 0.05m,
                InsuranceRatePercentage = 0.0075m,
                PilotBaseSalary = 5000m,
                PilotFlightHoursPerDay = 8m,
                CrewCostMultiplier = 1.3m,
                CateringCostPerPassenger = 8m,
                ServiceCostGroundHandling = 600m,
                LandingFeeSmall = 50m,
                LandingFeeMedium = 150m,
                LandingFeeMediumLarge = 350m,
                LandingFeeLarge = 750m,
                LandingFeeVeryLarge = 1500m,
                RatePerPaxPerNMSmall = 0.20m,
                RatePerPaxPerNMMedium = 0.15m,
                RatePerPaxPerNMMediumLarge = 0.12m,
                RatePerPaxPerNMLarge = 0.10m,
                RatePerPaxPerNMVeryLarge = 0.08m,
                FBOCostFactor = 0.05m,
                ROIPercentSmall = 15m,
                ROIPercentMedium = 15m,
                ROIPercentMediumLarge = 15m,
                ROIPercentLarge = 15m,
                ROIPercentVeryLarge = 15m,
                OldtimerROIMalusPercent = 50m
            };

            _mockSettingsService.Setup(s => s.CurrentSettings).Returns(_testSettings);
            _pricingService = new PricingService(_mockSettingsService.Object, _mockLoggingService.Object, _mockServiceProvider.Object);
        }

        private Aircraft CreateTestAircraft(int maxPassengers = 4, double fuelBurn = 10.0, decimal purchasePrice = 100000m)
        {
            return new Aircraft
            {
                Id = 1,
                Registration = "D-TEST",
                Type = "Cessna 172",
                MaxPassengers = maxPassengers,
                FuelBurnGalPerHour = fuelBurn,
                PurchasePrice = purchasePrice,
                CurrentValue = purchasePrice,
                CruiseSpeedKts = 120
            };
        }

        [Fact]
        public void Constructor_WithNullSettingsService_ShouldThrowArgumentNullException()
        {
            Action act = () => new PricingService(null!, _mockLoggingService.Object, _mockServiceProvider.Object);
            act.Should().Throw<ArgumentNullException>().WithParameterName("settingsService");
        }

        [Fact]
        public void Constructor_WithNullLoggingService_ShouldThrowArgumentNullException()
        {
            Action act = () => new PricingService(_mockSettingsService.Object, null!, _mockServiceProvider.Object);
            act.Should().Throw<ArgumentNullException>().WithParameterName("loggingService");
        }

        [Fact]
        public void Constructor_WithNullServiceProvider_ShouldThrowArgumentNullException()
        {
            Action act = () => new PricingService(_mockSettingsService.Object, _mockLoggingService.Object, null!);
            act.Should().Throw<ArgumentNullException>().WithParameterName("serviceProvider");
        }

        [Fact]
        public void CalculateFlightPrice_ShouldCalculateFuelCost()
        {
            var aircraft = CreateTestAircraft(fuelBurn: 10.0);
            double flightHours = 2.0;

            var result = _pricingService.CalculateFlightPrice(aircraft, 200, 4, flightHours);

            decimal expectedFuelCost = 10.0m * 2.0m * 6.0m;
            result.FuelCost.Should().Be(expectedFuelCost);
        }

        [Fact]
        public void CalculateFlightPrice_ShouldCalculateMaintenanceCost()
        {
            var aircraft = CreateTestAircraft();
            double flightHours = 2.0;

            var result = _pricingService.CalculateFlightPrice(aircraft, 200, 4, flightHours);

            decimal expectedMaintenanceCost = 2.0m * 50m;
            result.MaintenanceCost.Should().Be(expectedMaintenanceCost);
        }

        [Theory]
        [InlineData(4, 50)]
        [InlineData(10, 150)]
        [InlineData(50, 350)]
        [InlineData(200, 750)]
        [InlineData(400, 1500)]
        public void CalculateFlightPrice_ShouldCalculateLandingFeesBasedOnSize(int maxPassengers, decimal expectedFee)
        {
            var aircraft = CreateTestAircraft(maxPassengers: maxPassengers);

            var result = _pricingService.CalculateFlightPrice(aircraft, 200, maxPassengers, 2.0);

            result.LandingFees.Should().Be(expectedFee);
        }

        [Fact]
        public void CalculateFlightPrice_ShouldCalculateCateringCost()
        {
            var aircraft = CreateTestAircraft();
            int passengers = 4;

            var result = _pricingService.CalculateFlightPrice(aircraft, 200, passengers, 2.0);

            decimal expectedCatering = passengers * 8m;
            result.CateringCost.Should().Be(expectedCatering);
        }

        [Fact]
        public void CalculateFlightPrice_ShouldCalculateGroundHandlingCost()
        {
            var aircraft = CreateTestAircraft();

            var result = _pricingService.CalculateFlightPrice(aircraft, 200, 4, 2.0);

            // Small aircraft (4 PAX) get 5% of base ground handling cost (600 * 0.05 = 30)
            result.GroundHandlingCost.Should().Be(30m);
        }

        [Fact]
        public void CalculateFlightPrice_ShouldCalculateDepreciationCost()
        {
            var aircraft = CreateTestAircraft(purchasePrice: 100000m);
            double flightHours = 2.0;

            var result = _pricingService.CalculateFlightPrice(aircraft, 200, 4, flightHours);

            result.DepreciationCost.Should().BeGreaterThan(0);
        }

        [Fact]
        public void CalculateFlightPrice_ShouldCalculateInsuranceCost()
        {
            var aircraft = CreateTestAircraft(purchasePrice: 100000m);
            double flightHours = 2.0;

            var result = _pricingService.CalculateFlightPrice(aircraft, 200, 4, flightHours);

            result.InsuranceCost.Should().BeGreaterThan(0);
        }

        [Fact]
        public void CalculateFlightPrice_ShouldCalculateCrewCost()
        {
            var aircraft = CreateTestAircraft();
            double flightHours = 2.0;

            var result = _pricingService.CalculateFlightPrice(aircraft, 200, 4, flightHours);

            result.CrewCost.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData(4, 0.20)]
        [InlineData(10, 0.15)]
        [InlineData(50, 0.12)]
        [InlineData(200, 0.10)]
        [InlineData(400, 0.08)]
        public void GetRateForAircraftSize_ShouldReturnCorrectRate(int maxPassengers, decimal expectedRate)
        {
            var result = _pricingService.GetRateForAircraftSize(maxPassengers);

            result.Should().Be(expectedRate);
        }

        [Fact]
        public void CalculateFlightPrice_ShouldCalculateTotalOperatingCost()
        {
            var aircraft = CreateTestAircraft();

            var result = _pricingService.CalculateFlightPrice(aircraft, 200, 4, 2.0);

            var expectedTotal = result.FuelCost + result.MaintenanceCost + result.DepreciationCost +
                               result.InsuranceCost + result.CrewCost + result.LandingFees +
                               result.GroundHandlingCost + result.CateringCost + result.FBOCost;

            result.TotalOperatingCost.Should().Be(expectedTotal);
        }

        [Fact]
        public void CalculateFlightPrice_ShouldCalculateHybridRevenue()
        {
            var aircraft = CreateTestAircraft(maxPassengers: 4, purchasePrice: 100000m);
            double flightHours = 2.0;
            double distanceNM = 200;
            int passengers = 4;

            var result = _pricingService.CalculateFlightPrice(aircraft, distanceNM, passengers, flightHours);

            result.PassengerRevenue.Should().BeGreaterThan(0);
            var expectedROIPerHour = _pricingService.CalculateROIPerFlightHour(100000m, 4, false);
            var roiRevenue = expectedROIPerHour * (decimal)flightHours;
            var paxRate = _pricingService.GetRateForAircraftSize(4);
            var paxNmRevenue = passengers * (decimal)distanceNM * paxRate;
            var expectedRevenue = roiRevenue + paxNmRevenue;
            result.PassengerRevenue.Should().Be(expectedRevenue);
        }

        [Fact]
        public void CalculateFlightPrice_ShouldCalculateTotalPrice()
        {
            var aircraft = CreateTestAircraft();

            var result = _pricingService.CalculateFlightPrice(aircraft, 200, 4, 2.0);

            result.TotalPrice.Should().Be(result.TotalOperatingCost + result.PassengerRevenue + result.CargoRevenue);
        }

        [Fact]
        public void CalculateFlightPrice_ShouldCalculateProfit()
        {
            var aircraft = CreateTestAircraft();

            var result = _pricingService.CalculateFlightPrice(aircraft, 200, 4, 2.0);

            result.Profit.Should().Be(result.TotalPrice - result.TotalOperatingCost);
        }

        [Fact]
        public void CalculateFlightPrice_ShouldSetMetadata()
        {
            var aircraft = CreateTestAircraft();
            int passengers = 4;
            double distanceNM = 200;
            double flightHours = 2.0;

            var result = _pricingService.CalculateFlightPrice(aircraft, distanceNM, passengers, flightHours);

            result.Passengers.Should().Be(passengers);
            result.DistanceNM.Should().Be(distanceNM);
            result.FlightHours.Should().Be(flightHours);
        }

        [Fact]
        public void CalculateDailyOperatingCosts_ShouldCalculateAllCosts()
        {
            var aircraft = CreateTestAircraft();
            double flightHours = 5.5;

            var result = _pricingService.CalculateDailyOperatingCosts(aircraft, flightHours);

            result.FuelCost.Should().BeGreaterThan(0);
            result.MaintenanceCost.Should().BeGreaterThan(0);
            result.DepreciationCost.Should().BeGreaterThan(0);
            result.InsuranceCost.Should().BeGreaterThan(0);
            result.CrewCost.Should().BeGreaterThan(0);
        }

        [Fact]
        public void CalculateDailyOperatingCosts_TotalDailyCost_ShouldSumAllCosts()
        {
            var aircraft = CreateTestAircraft();
            double flightHours = 5.5;

            var result = _pricingService.CalculateDailyOperatingCosts(aircraft, flightHours);

            var expectedTotal = result.FuelCost + result.MaintenanceCost +
                               result.DepreciationCost + result.InsuranceCost + result.CrewCost + result.FBOCost;

            result.TotalDailyCost.Should().Be(expectedTotal);
        }

        [Fact]
        public void CalculateFlightPrice_WithZeroFlightHours_ShouldNotThrow()
        {
            var aircraft = CreateTestAircraft();

            Action act = () => _pricingService.CalculateFlightPrice(aircraft, 100, 4, 0);

            act.Should().NotThrow();
        }

        [Fact]
        public void CalculateFlightPrice_WithZeroPassengers_ShouldCalculateWithZeroCatering()
        {
            var aircraft = CreateTestAircraft();

            var result = _pricingService.CalculateFlightPrice(aircraft, 100, 0, 1.0);

            result.CateringCost.Should().Be(0);
            result.PassengerRevenue.Should().BeGreaterThanOrEqualTo(0);
        }

        [Fact]
        public void CalculateFlightPrice_WithZeroDistance_ShouldCalculateROIBasedRevenue()
        {
            var aircraft = CreateTestAircraft();

            var result = _pricingService.CalculateFlightPrice(aircraft, 0, 4, 1.0);

            result.PassengerRevenue.Should().BeGreaterThanOrEqualTo(0);
        }

        [Fact]
        public void CalculateFlightPrice_LargeAircraft_ShouldUseLargeROIAndPaxRate()
        {
            var aircraft = CreateTestAircraft(maxPassengers: 300, purchasePrice: 50000000m);
            double flightHours = 3.0;
            double distanceNM = 500;
            int passengers = 300;

            var result = _pricingService.CalculateFlightPrice(aircraft, distanceNM, passengers, flightHours);

            var expectedROIPerHour = _pricingService.CalculateROIPerFlightHour(50000000m, 300, false);
            var roiRevenue = expectedROIPerHour * (decimal)flightHours;
            var paxRate = _pricingService.GetRateForAircraftSize(300);
            var paxNmRevenue = passengers * (decimal)distanceNM * paxRate;
            var expectedRevenue = roiRevenue + paxNmRevenue;
            result.PassengerRevenue.Should().Be(expectedRevenue);
            result.LandingFees.Should().Be(750m);
        }

        [Fact]
        public void CalculateFlightPrice_VeryLargeAircraft_ShouldUseVeryLargeROIAndPaxRate()
        {
            var aircraft = CreateTestAircraft(maxPassengers: 400, purchasePrice: 100000000m);
            double flightHours = 5.0;
            double distanceNM = 1000;
            int passengers = 400;

            var result = _pricingService.CalculateFlightPrice(aircraft, distanceNM, passengers, flightHours);

            var expectedROIPerHour = _pricingService.CalculateROIPerFlightHour(100000000m, 400, false);
            var roiRevenue = expectedROIPerHour * (decimal)flightHours;
            var paxRate = _pricingService.GetRateForAircraftSize(400);
            var paxNmRevenue = passengers * (decimal)distanceNM * paxRate;
            var expectedRevenue = roiRevenue + paxNmRevenue;
            result.PassengerRevenue.Should().Be(expectedRevenue);
            result.LandingFees.Should().Be(1500m);
        }

        [Fact]
        public void CalculateFlightPrice_WithAircraftValueZero_ShouldUsePurchasePrice()
        {
            var aircraft = CreateTestAircraft(purchasePrice: 100000m);
            aircraft.CurrentValue = 0;

            var result = _pricingService.CalculateFlightPrice(aircraft, 200, 4, 2.0);

            result.DepreciationCost.Should().BeGreaterThan(0);
            result.InsuranceCost.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData(4, 15)]
        [InlineData(10, 15)]
        [InlineData(50, 15)]
        [InlineData(200, 15)]
        [InlineData(400, 15)]
        public void GetROIPercentForSize_ShouldReturnConfiguredROI(int maxPassengers, decimal expectedROI)
        {
            var result = _pricingService.GetROIPercentForSize(maxPassengers);
            result.Should().Be(expectedROI);
        }

        [Fact]
        public void CalculateROIPerFlightHour_ShouldCalculateCorrectly()
        {
            decimal purchasePrice = 100000m;
            int maxPassengers = 4;
            bool isOldtimer = false;

            var result = _pricingService.CalculateROIPerFlightHour(purchasePrice, maxPassengers, isOldtimer);

            decimal roiPercent = 15m;
            decimal annualROI = purchasePrice * (roiPercent / 100m);
            decimal hoursPerYear = 8m * 365m;
            decimal expectedROIPerHour = annualROI / hoursPerYear;
            result.Should().Be(expectedROIPerHour);
        }

        [Fact]
        public void CalculateROIPerFlightHour_OldtimerAircraft_ShouldApplyMalus()
        {
            decimal purchasePrice = 100000m;
            int maxPassengers = 4;

            var normalResult = _pricingService.CalculateROIPerFlightHour(purchasePrice, maxPassengers, false);
            var oldtimerResult = _pricingService.CalculateROIPerFlightHour(purchasePrice, maxPassengers, true);

            oldtimerResult.Should().BeLessThan(normalResult);
            oldtimerResult.Should().Be(normalResult * 0.5m);
        }

        [Fact]
        public void CalculateFlightPrice_OldtimerAircraft_ShouldHaveReducedROIPortion()
        {
            var normalAircraft = CreateTestAircraft(maxPassengers: 4, purchasePrice: 100000m);
            normalAircraft.IsOldtimer = false;

            var oldtimerAircraft = CreateTestAircraft(maxPassengers: 4, purchasePrice: 100000m);
            oldtimerAircraft.IsOldtimer = true;

            double flightHours = 2.0;
            double distanceNM = 200;
            int passengers = 4;

            var normalResult = _pricingService.CalculateFlightPrice(normalAircraft, distanceNM, passengers, flightHours);
            var oldtimerResult = _pricingService.CalculateFlightPrice(oldtimerAircraft, distanceNM, passengers, flightHours);

            var normalROI = _pricingService.CalculateROIPerFlightHour(100000m, 4, false) * (decimal)flightHours;
            var oldtimerROI = _pricingService.CalculateROIPerFlightHour(100000m, 4, true) * (decimal)flightHours;
            var paxNmRevenue = passengers * (decimal)distanceNM * _pricingService.GetRateForAircraftSize(4);

            oldtimerROI.Should().Be(normalROI * 0.5m);
            oldtimerResult.PassengerRevenue.Should().BeLessThan(normalResult.PassengerRevenue);
            normalResult.PassengerRevenue.Should().Be(normalROI + paxNmRevenue);
            oldtimerResult.PassengerRevenue.Should().Be(oldtimerROI + paxNmRevenue);
        }
    }
}
