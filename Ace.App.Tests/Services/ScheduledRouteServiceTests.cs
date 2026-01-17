using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;
using Xunit;

namespace Ace.App.Tests.Services
{
    public class ScheduledRouteServiceTests
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly Mock<ISettingsService> _mockSettingsService;
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly Mock<IAirportDatabase> _mockAirportDatabase;
        private readonly ScheduledRouteService _service;

        public ScheduledRouteServiceTests()
        {
            _mockLogger = new Mock<ILoggingService>();
            _mockSettingsService = new Mock<ISettingsService>();
            _mockServiceProvider = new Mock<IServiceProvider>();
            _mockAirportDatabase = new Mock<IAirportDatabase>();

            var settings = new AppSettings
            {
                RouteSlotLimitLocal = 2,
                RouteSlotLimitRegional = 5,
                RouteSlotLimitInternational = 10
            };
            _mockSettingsService.Setup(s => s.CurrentSettings).Returns(settings);

            _service = new ScheduledRouteService(
                _mockLogger.Object,
                _mockSettingsService.Object,
                _mockServiceProvider.Object,
                _mockAirportDatabase.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            Action act = () => new ScheduledRouteService(
                null!,
                _mockSettingsService.Object,
                _mockServiceProvider.Object,
                _mockAirportDatabase.Object);
            act.Should().Throw<ArgumentNullException>().WithParameterName("logger");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenSettingsServiceIsNull()
        {
            Action act = () => new ScheduledRouteService(
                _mockLogger.Object,
                null!,
                _mockServiceProvider.Object,
                _mockAirportDatabase.Object);
            act.Should().Throw<ArgumentNullException>().WithParameterName("settingsService");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenServiceProviderIsNull()
        {
            Action act = () => new ScheduledRouteService(
                _mockLogger.Object,
                _mockSettingsService.Object,
                null!,
                _mockAirportDatabase.Object);
            act.Should().Throw<ArgumentNullException>().WithParameterName("serviceProvider");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenAirportDatabaseIsNull()
        {
            Action act = () => new ScheduledRouteService(
                _mockLogger.Object,
                _mockSettingsService.Object,
                _mockServiceProvider.Object,
                null!);
            act.Should().Throw<ArgumentNullException>().WithParameterName("airportDatabase");
        }

        [Fact]
        public void GetMaxSlots_ShouldReturn2_ForLocalFBO()
        {
            var slots = _service.GetMaxSlots(FBOType.Local);
            slots.Should().Be(2);
        }

        [Fact]
        public void GetMaxSlots_ShouldReturn5_ForRegionalFBO()
        {
            var slots = _service.GetMaxSlots(FBOType.Regional);
            slots.Should().Be(5);
        }

        [Fact]
        public void GetMaxSlots_ShouldReturn10_ForInternationalFBO()
        {
            var slots = _service.GetMaxSlots(FBOType.International);
            slots.Should().Be(10);
        }

        [Fact]
        public void GetMaxSlots_ShouldReturn2_ForUnknownFBOType()
        {
            var slots = _service.GetMaxSlots((FBOType)999);
            slots.Should().Be(2);
        }
    }
}
