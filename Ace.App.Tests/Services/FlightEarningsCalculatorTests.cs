using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;
using Ace.App.Tests.Helpers;
using Xunit;

namespace Ace.App.Tests.Services
{
    public class FlightEarningsCalculatorTests
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly Mock<ISettingsService> _mockSettingsService;
        private readonly Mock<IPricingService> _mockPricingService;
        private readonly FlightEarningsCalculator _calculator;

        public FlightEarningsCalculatorTests()
        {
            TestServiceLocatorInitializer.Initialize();
            _mockLogger = new Mock<ILoggingService>();
            _mockSettingsService = new Mock<ISettingsService>();
            _mockPricingService = new Mock<IPricingService>();

            var settings = new AppSettings { PlayerFlightBonusPercent = 10m };
            _mockSettingsService.Setup(x => x.CurrentSettings).Returns(settings);

            _calculator = new FlightEarningsCalculator(
                _mockLogger.Object,
                _mockSettingsService.Object,
                _mockPricingService.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            Action act = () => new FlightEarningsCalculator(null!, _mockSettingsService.Object, _mockPricingService.Object);
            act.Should().Throw<ArgumentNullException>().WithParameterName("logger");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenSettingsServiceIsNull()
        {
            Action act = () => new FlightEarningsCalculator(_mockLogger.Object, null!, _mockPricingService.Object);
            act.Should().Throw<ArgumentNullException>().WithParameterName("settingsService");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenPricingServiceIsNull()
        {
            Action act = () => new FlightEarningsCalculator(_mockLogger.Object, _mockSettingsService.Object, null!);
            act.Should().Throw<ArgumentNullException>().WithParameterName("pricingService");
        }

        [Fact]
        public void CalculateEarnings_ShouldReturnCorrectResult_WhenAircraftNotFound()
        {
            _mockPricingService.Setup(x => x.IsNetworkFlight(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _mockPricingService.Setup(x => x.GetRateForAircraftSize(It.IsAny<int>())).Returns(0.15m);

            var request = new FlightEarningsRequest
            {
                AircraftRegistration = "UNKNOWN",
                DepartureIcao = "EDDF",
                ArrivalIcao = "EDDM",
                DistanceNM = 100,
                Passengers = 4,
                FlightHours = 1.0,
                IsManualCompletion = false
            };

            var result = _calculator.CalculateEarnings(request);

            result.Should().NotBeNull();
            result.AircraftFound.Should().BeFalse();
            result.BaseRevenue.Should().Be(60m);
            result.PlayerBonusPercent.Should().Be(10m);
            result.PlayerBonusAmount.Should().Be(6m);
            result.TotalEarnings.Should().Be(66m);
        }

        [Fact]
        public void CalculateEarnings_ShouldApplyNetworkBonus_WhenNetworkFlight()
        {
            _mockPricingService.Setup(x => x.IsNetworkFlight("EDDF", "EDDM")).Returns(true);
            _mockPricingService.Setup(x => x.GetNetworkBonusPercent()).Returns(5m);
            _mockPricingService.Setup(x => x.GetRateForAircraftSize(It.IsAny<int>())).Returns(0.15m);

            var request = new FlightEarningsRequest
            {
                AircraftRegistration = "UNKNOWN",
                DepartureIcao = "EDDF",
                ArrivalIcao = "EDDM",
                DistanceNM = 100,
                Passengers = 4,
                FlightHours = 1.0,
                IsManualCompletion = false
            };

            var result = _calculator.CalculateEarnings(request);

            result.IsNetworkFlight.Should().BeTrue();
            result.NetworkBonusPercent.Should().Be(5m);
            result.NetworkBonusAmount.Should().Be(3m);
            result.TotalEarnings.Should().Be(69m);
        }

        [Fact]
        public void CalculateEarnings_ShouldUseFallbackPassengers_WhenPassengersIsZero()
        {
            _mockPricingService.Setup(x => x.IsNetworkFlight(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _mockPricingService.Setup(x => x.GetRateForAircraftSize(4)).Returns(0.15m);

            var request = new FlightEarningsRequest
            {
                AircraftRegistration = "UNKNOWN",
                DepartureIcao = "EDDF",
                ArrivalIcao = "EDDM",
                DistanceNM = 100,
                Passengers = 0,
                FlightHours = 1.0,
                IsManualCompletion = false
            };

            var result = _calculator.CalculateEarnings(request);

            _mockPricingService.Verify(x => x.GetRateForAircraftSize(4), Times.Once);
        }

        [Fact]
        public void CalculateEarnings_ShouldReturnZeroNetworkBonus_WhenNotNetworkFlight()
        {
            _mockPricingService.Setup(x => x.IsNetworkFlight(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _mockPricingService.Setup(x => x.GetRateForAircraftSize(It.IsAny<int>())).Returns(0.15m);

            var request = new FlightEarningsRequest
            {
                AircraftRegistration = "UNKNOWN",
                DepartureIcao = "EDDF",
                ArrivalIcao = "EDDM",
                DistanceNM = 100,
                Passengers = 4,
                FlightHours = 1.0,
                IsManualCompletion = false
            };

            var result = _calculator.CalculateEarnings(request);

            result.IsNetworkFlight.Should().BeFalse();
            result.NetworkBonusPercent.Should().Be(0m);
            result.NetworkBonusAmount.Should().Be(0m);
        }

        [Fact]
        public void CalculateEarnings_ShouldLogManualCompletion_WhenIsManualCompletionTrue()
        {
            _mockPricingService.Setup(x => x.IsNetworkFlight(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _mockPricingService.Setup(x => x.GetRateForAircraftSize(It.IsAny<int>())).Returns(0.15m);

            var request = new FlightEarningsRequest
            {
                AircraftRegistration = "UNKNOWN",
                DepartureIcao = "EDDF",
                ArrivalIcao = "EDDM",
                DistanceNM = 100,
                Passengers = 4,
                FlightHours = 1.0,
                IsManualCompletion = true
            };

            _calculator.CalculateEarnings(request);

            _mockLogger.Verify(
                x => x.Warn(It.Is<string>(s => s.Contains("[Manual]"))),
                Times.Once);
        }

        [Fact]
        public void CalculateEarnings_ShouldReturnCorrectTotalOperatingCost_WhenAircraftNotFound()
        {
            _mockPricingService.Setup(x => x.IsNetworkFlight(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _mockPricingService.Setup(x => x.GetRateForAircraftSize(It.IsAny<int>())).Returns(0.15m);

            var request = new FlightEarningsRequest
            {
                AircraftRegistration = "UNKNOWN",
                DepartureIcao = "EDDF",
                ArrivalIcao = "EDDM",
                DistanceNM = 100,
                Passengers = 4,
                FlightHours = 1.0,
                IsManualCompletion = false
            };

            var result = _calculator.CalculateEarnings(request);

            result.TotalOperatingCost.Should().Be(0m);
        }

        [Fact]
        public void CalculateEarnings_ShouldSetPriceBreakdownToNull_WhenAircraftNotFound()
        {
            _mockPricingService.Setup(x => x.IsNetworkFlight(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _mockPricingService.Setup(x => x.GetRateForAircraftSize(It.IsAny<int>())).Returns(0.15m);

            var request = new FlightEarningsRequest
            {
                AircraftRegistration = "UNKNOWN",
                DepartureIcao = "EDDF",
                ArrivalIcao = "EDDM",
                DistanceNM = 100,
                Passengers = 4,
                FlightHours = 1.0,
                IsManualCompletion = false
            };

            var result = _calculator.CalculateEarnings(request);

            result.PriceBreakdown.Should().BeNull();
        }
    }
}
