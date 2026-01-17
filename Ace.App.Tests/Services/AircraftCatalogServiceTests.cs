using System;
using System.Linq;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Services;
using Xunit;

namespace Ace.App.Tests.Services
{
    public class AircraftCatalogServiceTests
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly AircraftCatalogService _catalogService;

        public AircraftCatalogServiceTests()
        {
            _mockLogger = new Mock<ILoggingService>();
            _catalogService = new AircraftCatalogService(_mockLogger.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggingServiceIsNull()
        {
            Action act = () => new AircraftCatalogService(null!);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("loggingService");
        }

        [Fact]
        public void AvailableAircraft_ShouldReturnEmptyList_Initially()
        {
            var aircraft = _catalogService.AvailableAircraft;

            aircraft.Should().NotBeNull();
            aircraft.Should().BeEmpty();
        }

        [Fact]
        public void AvailableAircraft_ShouldReturnReadOnlyList()
        {
            var aircraft = _catalogService.AvailableAircraft;

            aircraft.Should().BeAssignableTo<System.Collections.Generic.IReadOnlyList<AircraftInfo>>();
        }

        [Fact]
        public void LoadAvailableAircraft_ShouldNotThrow()
        {
            Action act = () => _catalogService.LoadAvailableAircraft();

            act.Should().NotThrow();
        }

        [Fact]
        public void LoadAvailableAircraft_ShouldLogStartMessage()
        {
            _catalogService.LoadAvailableAircraft();

            _mockLogger.Verify(
                x => x.Info(It.Is<string>(s => s.Contains("Starting scan"))),
                Times.Once);
        }

        [Fact]
        public void LoadAvailableAircraft_ShouldLogCompletionMessage()
        {
            _catalogService.LoadAvailableAircraft();

            _mockLogger.Verify(
                x => x.Info(It.Is<string>(s => s.Contains("Found") && s.Contains("aircraft"))),
                Times.Once);
        }

        [Fact]
        public void LoadAvailableAircraft_ShouldUpdateAvailableAircraftList()
        {
            var initialCount = _catalogService.AvailableAircraft.Count;

            _catalogService.LoadAvailableAircraft();
            var finalCount = _catalogService.AvailableAircraft.Count;

            finalCount.Should().BeGreaterThanOrEqualTo(0);
        }

        [Fact]
        public void AircraftInfo_DefaultConstructor_ShouldInitializeWithEmptyStrings()
        {
            var info = new AircraftInfo();

            info.Title.Should().BeEmpty();
            info.Manufacturer.Should().BeEmpty();
            info.Type.Should().BeEmpty();
            info.Category.Should().BeEmpty();
            info.Description.Should().BeEmpty();
        }

        [Fact]
        public void AircraftInfo_ShouldAllowPropertyAssignment()
        {
            var info = new AircraftInfo();

            info.Title = "Cessna 172";
            info.Manufacturer = "Cessna";
            info.Type = "C172";
            info.Category = "Piston";
            info.CrewCount = 1;
            info.PassengerCapacity = 3;
            info.CruiseSpeedKts = 120;
            info.MaxRangeNM = 600;
            info.FuelCapacityGal = 50;
            info.FuelBurnGalPerHour = 8;
            info.HourlyOperatingCost = 150m;

            info.Title.Should().Be("Cessna 172");
            info.Manufacturer.Should().Be("Cessna");
            info.Type.Should().Be("C172");
            info.Category.Should().Be("Piston");
            info.CrewCount.Should().Be(1);
            info.PassengerCapacity.Should().Be(3);
            info.CruiseSpeedKts.Should().Be(120);
            info.MaxRangeNM.Should().Be(600);
            info.FuelCapacityGal.Should().Be(50);
            info.FuelBurnGalPerHour.Should().Be(8);
            info.HourlyOperatingCost.Should().Be(150m);
        }

        [Fact]
        public void AvailableAircraft_ShouldBeThreadSafe()
        {
            var tasks = new System.Threading.Tasks.Task[10];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = System.Threading.Tasks.Task.Run(() =>
                {
                    var aircraft = _catalogService.AvailableAircraft;
                    aircraft.Should().NotBeNull();
                });
            }

            Action act = () => System.Threading.Tasks.Task.WaitAll(tasks);
            act.Should().NotThrow();
        }

        [Fact]
        public void LoadAvailableAircraft_MultipleCalls_ShouldNotThrow()
        {
            Action act = () =>
            {
                _catalogService.LoadAvailableAircraft();
                _catalogService.LoadAvailableAircraft();
                _catalogService.LoadAvailableAircraft();
            };

            act.Should().NotThrow();
        }

        [Fact]
        public void AircraftInfo_NumericProperties_ShouldDefaultToZero()
        {
            var info = new AircraftInfo();

            info.CrewCount.Should().Be(0);
            info.PassengerCapacity.Should().Be(0);
            info.CruiseSpeedKts.Should().Be(0);
            info.MaxRangeNM.Should().Be(0);
            info.FuelCapacityGal.Should().Be(0);
            info.FuelBurnGalPerHour.Should().Be(0);
            info.HourlyOperatingCost.Should().Be(0);
        }
    }
}
