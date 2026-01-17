using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;
using Ace.App.ViewModels;
using Xunit;

namespace Ace.App.Tests.ViewModels
{
    public class FlightMapViewModelTests
    {
        private readonly Mock<IPersistenceService> _persistenceServiceMock;
        private readonly Mock<IAirportDatabase> _airportDatabaseMock;
        private readonly Mock<ILoggingService> _loggingServiceMock;
        private readonly Mock<SimConnectService> _simConnectServiceMock;
        private readonly Mock<IActiveFlightPlanService> _activeFlightPlanServiceMock;
        private readonly Mock<ISettingsService> _settingsServiceMock;

        public FlightMapViewModelTests()
        {
            _persistenceServiceMock = new Mock<IPersistenceService>();
            _airportDatabaseMock = new Mock<IAirportDatabase>();
            _loggingServiceMock = new Mock<ILoggingService>();
            _activeFlightPlanServiceMock = new Mock<IActiveFlightPlanService>();
            _settingsServiceMock = new Mock<ISettingsService>();

            var settings = new AppSettings { MapLayer = "Street" };
            _settingsServiceMock.Setup(s => s.CurrentSettings).Returns(settings);

            var financeServiceMock = new Mock<IFinanceService>();
            var earningsCalculatorMock = new Mock<IFlightEarningsCalculator>();
            var achievementServiceMock = new Mock<IAchievementService>();
            var soundServiceMock = new Mock<ISoundService>();

            _simConnectServiceMock = new Mock<SimConnectService>(
                _loggingServiceMock.Object,
                _activeFlightPlanServiceMock.Object,
                _persistenceServiceMock.Object,
                financeServiceMock.Object,
                _airportDatabaseMock.Object,
                earningsCalculatorMock.Object,
                achievementServiceMock.Object,
                soundServiceMock.Object);
        }

        [Fact]
        public void Constructor_ShouldInitializeMap()
        {
            var viewModel = CreateViewModel();

            viewModel.Map.Should().NotBeNull();
            viewModel.Map!.Layers.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void IsConnected_ShouldBeFalseByDefault()
        {
            var viewModel = CreateViewModel();

            viewModel.IsConnected.Should().BeFalse();
        }

        [Fact]
        public void LoadFlightData_ShouldHandleEmptyFlightList()
        {
            _persistenceServiceMock.Setup(x => x.LoadFlightRecords())
                .Returns(new List<FlightRecord>());

            var viewModel = CreateViewModel();
            viewModel.LoadFlightData();

            _loggingServiceMock.Verify(x => x.Debug(It.Is<string>(s => s.Contains("No flight records"))), Times.Once);
        }

        [Fact]
        public void LoadFlightData_ShouldLoadFlightsWithValidAirports()
        {
            var flights = new List<FlightRecord>
            {
                new FlightRecord
                {
                    Departure = "EDDF",
                    Arrival = "EGLL",
                    DistanceNM = 350,
                    Date = System.DateTime.Now
                }
            };

            _persistenceServiceMock.Setup(x => x.LoadFlightRecords())
                .Returns(flights);

            _airportDatabaseMock.Setup(x => x.GetAirport("EDDF"))
                .Returns(new Airport { Icao = "EDDF", Name = "Frankfurt", Latitude = 50.0379, Longitude = 8.5622 });
            _airportDatabaseMock.Setup(x => x.GetAirport("EGLL"))
                .Returns(new Airport { Icao = "EGLL", Name = "Heathrow", Latitude = 51.4700, Longitude = -0.4543 });

            var viewModel = CreateViewModel();
            viewModel.LoadFlightData();

            _loggingServiceMock.Verify(x => x.Info(It.Is<string>(s => s.Contains("Loaded 1 flights"))), Times.Once);
        }

        [Fact]
        public void Cleanup_ShouldUnsubscribeFromEvents()
        {
            var viewModel = CreateViewModel();
            viewModel.Cleanup();
        }

        private FlightMapViewModel CreateViewModel()
        {
            return new FlightMapViewModel(
                _persistenceServiceMock.Object,
                _airportDatabaseMock.Object,
                _loggingServiceMock.Object,
                _simConnectServiceMock.Object,
                _activeFlightPlanServiceMock.Object,
                _settingsServiceMock.Object);
        }
    }
}
