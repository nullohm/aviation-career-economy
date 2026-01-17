using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;
using Xunit;

namespace Ace.App.Tests.Services
{
    public class RouteSuggestionServiceTests
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly Mock<IAirportDatabase> _mockAirportDb;
        private readonly Mock<ISettingsService> _mockSettings;
        private readonly Mock<IPricingService> _mockPricingService;
        private readonly RouteSuggestionService _routeService;

        public RouteSuggestionServiceTests()
        {
            _mockLogger = new Mock<ILoggingService>();
            _mockAirportDb = new Mock<IAirportDatabase>();
            _mockSettings = new Mock<ISettingsService>();
            _mockPricingService = new Mock<IPricingService>();

            var appSettings = new AppSettings { RatePerPaxPerNMSmall = 0.20m };
            _mockSettings.Setup(x => x.CurrentSettings).Returns(appSettings);

            _routeService = new RouteSuggestionService(
                _mockLogger.Object,
                _mockAirportDb.Object,
                _mockSettings.Object,
                _mockPricingService.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggingServiceIsNull()
        {
            Action act = () => new RouteSuggestionService(null!, _mockAirportDb.Object, _mockSettings.Object, _mockPricingService.Object);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("loggingService");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenAirportDatabaseIsNull()
        {
            Action act = () => new RouteSuggestionService(_mockLogger.Object, null!, _mockSettings.Object, _mockPricingService.Object);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("airportDatabase");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenSettingsServiceIsNull()
        {
            Action act = () => new RouteSuggestionService(_mockLogger.Object, _mockAirportDb.Object, null!, _mockPricingService.Object);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("settingsService");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenPricingServiceIsNull()
        {
            Action act = () => new RouteSuggestionService(_mockLogger.Object, _mockAirportDb.Object, _mockSettings.Object, null!);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("pricingService");
        }

        [Fact]
        public void SuggestedRoute_DefaultValues_ShouldBeInitialized()
        {
            var route = new SuggestedRoute();

            route.DepartureIcao.Should().BeEmpty();
            route.ArrivalIcao.Should().BeEmpty();
            route.DepartureName.Should().BeEmpty();
            route.ArrivalName.Should().BeEmpty();
            route.DistanceNM.Should().Be(0);
            route.Bearing.Should().Be(0);
            route.Passengers.Should().Be(0);
            route.CruiseSpeedKts.Should().Be(0);
        }

        [Fact]
        public void SuggestedRoute_EstimatedFlightHours_ShouldCalculateCorrectly()
        {
            var route = new SuggestedRoute
            {
                DistanceNM = 300,
                CruiseSpeedKts = 150
            };

            var hours = route.EstimatedFlightHours;

            hours.Should().Be(2.0);
        }

        [Fact]
        public void SuggestedRoute_EstimatedFlightHours_ShouldReturnZero_WhenSpeedIsZero()
        {
            var route = new SuggestedRoute
            {
                DistanceNM = 300,
                CruiseSpeedKts = 0
            };

            var hours = route.EstimatedFlightHours;

            hours.Should().Be(0);
        }

        [Fact]
        public void SuggestedRoute_EstimatedRevenue_ShouldReturnZero_WhenPricingServiceNotSet()
        {
            var route = new SuggestedRoute
            {
                DistanceNM = 300,
                Passengers = 3
            };

            var revenue = route.EstimatedRevenue;

            revenue.Should().Be(0);
        }

        [Fact]
        public void SuggestedRoute_EstimatedRevenue_ShouldUsesPricingService()
        {
            var route = new SuggestedRoute
            {
                DistanceNM = 300,
                Passengers = 3,
                CruiseSpeedKts = 150
            };

            var aircraft = new Aircraft { Id = 1, Registration = "D-TEST", MaxPassengers = 4 };
            var priceBreakdown = new FlightPriceBreakdown { PassengerRevenue = 400m, CargoRevenue = 100m };

            _mockPricingService.Setup(p => p.CalculateFlightPrice(
                aircraft,
                300,
                3,
                It.IsAny<double>()
            )).Returns(priceBreakdown);

            route.SetPricingService(_mockPricingService.Object, aircraft);

            var revenue = route.EstimatedRevenue;

            revenue.Should().Be(500m);
        }

        [Fact]
        public void SuggestedRoute_DisplayText_ShouldFormatCorrectly()
        {
            var route = new SuggestedRoute
            {
                DepartureIcao = "EDDF",
                ArrivalIcao = "EDDM"
            };

            var text = route.DisplayText;

            text.Should().Be("EDDF → EDDM");
        }

        [Fact]
        public void SuggestedRoute_MissionDescription_ShouldReturnCorrectText()
        {
            new SuggestedRoute { MissionType = MissionType.ShortHop }.MissionDescription.Should().Be("Short Hop");
            new SuggestedRoute { MissionType = MissionType.LocalFlight }.MissionDescription.Should().Be("Local Flight");
            new SuggestedRoute { MissionType = MissionType.RegionalFlight }.MissionDescription.Should().Be("Regional Flight");
            new SuggestedRoute { MissionType = MissionType.CrossCountry }.MissionDescription.Should().Be("Cross Country");
            new SuggestedRoute { MissionType = MissionType.LongHaul }.MissionDescription.Should().Be("Long Haul");
        }

        [Fact]
        public void SuggestedRoute_CompassDirection_ShouldReturnCorrectDirection()
        {
            new SuggestedRoute { Bearing = 0 }.CompassDirection.Should().Be("N");
            new SuggestedRoute { Bearing = 45 }.CompassDirection.Should().Be("NE");
            new SuggestedRoute { Bearing = 90 }.CompassDirection.Should().Be("E");
            new SuggestedRoute { Bearing = 135 }.CompassDirection.Should().Be("SE");
            new SuggestedRoute { Bearing = 180 }.CompassDirection.Should().Be("S");
            new SuggestedRoute { Bearing = 225 }.CompassDirection.Should().Be("SW");
            new SuggestedRoute { Bearing = 270 }.CompassDirection.Should().Be("W");
            new SuggestedRoute { Bearing = 315 }.CompassDirection.Should().Be("NW");
        }

        [Fact]
        public void SuggestedRoute_RouteDescription_ShouldIncludeAllInfo()
        {
            var route = new SuggestedRoute
            {
                MissionType = MissionType.RegionalFlight,
                DistanceNM = 250,
                Bearing = 90,
                Passengers = 4,
                CargoKg = 200,
                CruiseSpeedKts = 150,
                DepartureName = "Frankfurt",
                ArrivalName = "Munich"
            };

            var aircraft = new Aircraft { Id = 1, Registration = "D-TEST", MaxPassengers = 4 };
            var priceBreakdown = new FlightPriceBreakdown { PassengerRevenue = 400m, CargoRevenue = 100m };
            _mockPricingService.Setup(p => p.CalculateFlightPrice(
                aircraft,
                250,
                4,
                It.IsAny<double>()
            )).Returns(priceBreakdown);

            route.SetPricingService(_mockPricingService.Object, aircraft);

            var description = route.RouteDescription;

            description.Should().Contain("Regional Flight");
            description.Should().Contain("250 NM");
            description.Should().Contain("E");
            description.Should().Contain("4 PAX");
            description.Should().Contain("200 kg");
            description.Should().Contain("Frankfurt");
            description.Should().Contain("Munich");
        }

        [Fact]
        public void MissionType_Enum_ShouldHaveAllValues()
        {
            Enum.IsDefined(typeof(MissionType), MissionType.ShortHop).Should().BeTrue();
            Enum.IsDefined(typeof(MissionType), MissionType.LocalFlight).Should().BeTrue();
            Enum.IsDefined(typeof(MissionType), MissionType.RegionalFlight).Should().BeTrue();
            Enum.IsDefined(typeof(MissionType), MissionType.CrossCountry).Should().BeTrue();
            Enum.IsDefined(typeof(MissionType), MissionType.LongHaul).Should().BeTrue();
        }
    }
}
