using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Services;
using Ace.App.Tests.Helpers;
using Xunit;

namespace Ace.App.Tests.Services
{
    /// <summary>
    /// Unit tests for SettingsService
    /// Note: SettingsService works with the real database, so these tests verify behavior
    /// </summary>
    public class SettingsServiceTests
    {
        private readonly Mock<ILoggingService> _mockLogger;

        public SettingsServiceTests()
        {
            TestServiceLocatorInitializer.Initialize();
            _mockLogger = new Mock<ILoggingService>();
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            // Act & Assert
            Action act = () => new SettingsService(null!);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("loggingService");
        }

        [Fact]
        public void Constructor_ShouldInitializeCurrentSettings()
        {
            // Act
            var service = new SettingsService(_mockLogger.Object);

            // Assert
            service.CurrentSettings.Should().NotBeNull();
        }

        [Fact]
        public void CurrentSettings_ShouldHaveDefaultValues_WhenFirstCreated()
        {
            // Act
            var service = new SettingsService(_mockLogger.Object);

            // Assert
            var settings = service.CurrentSettings;
            settings.Should().NotBeNull();
            settings.WindowWidth.Should().BeGreaterThan(0);
            settings.WindowHeight.Should().BeGreaterThan(0);
            settings.RatePerPaxPerNMSmall.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Load_ShouldNotThrow()
        {
            // Arrange
            var service = new SettingsService(_mockLogger.Object);

            // Act
            Action act = () => service.Load();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Save_ShouldNotThrow()
        {
            // Arrange
            var service = new SettingsService(_mockLogger.Object);

            // Act
            Action act = () => service.Save();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Save_ShouldPersistChanges()
        {
            // Arrange
            var service = new SettingsService(_mockLogger.Object);
            var originalRate = service.CurrentSettings.RatePerPaxPerNMSmall;
            var newRate = 0.25m;

            // Act
            service.CurrentSettings.RatePerPaxPerNMSmall = newRate;
            service.Save();

            // Create new instance to load from database
            var newService = new SettingsService(_mockLogger.Object);

            // Assert
            newService.CurrentSettings.RatePerPaxPerNMSmall.Should().Be(newRate);

            // Cleanup - restore original value
            service.CurrentSettings.RatePerPaxPerNMSmall = originalRate;
            service.Save();
        }

        [Fact]
        public void AppSettings_DefaultValues_ShouldBeValid()
        {
            // Arrange & Act
            var settings = new AppSettings();

            // Assert
            settings.IsSimConnectEnabled.Should().BeTrue();
            settings.AutoStartTracking.Should().BeFalse();
            settings.WindowTop.Should().Be(100);
            settings.WindowLeft.Should().Be(100);
            settings.WindowWidth.Should().Be(1400);
            settings.WindowHeight.Should().Be(1000);
            settings.IsMaximized.Should().BeFalse();
            settings.RatePerPaxPerNMSmall.Should().Be(2.00m);
            settings.LastDepartureIcao.Should().BeEmpty();
            settings.LastArrivalIcao.Should().BeEmpty();
            settings.FBORentLocal.Should().Be(500m);
            settings.FBORentRegional.Should().Be(1500m);
            settings.FBORentInternational.Should().Be(5000m);
            settings.TerminalCostSmall.Should().Be(1000m);
            settings.TerminalCostMedium.Should().Be(3000m);
            settings.TerminalCostLarge.Should().Be(8000m);
            settings.ServiceCostRefueling.Should().Be(500m);
            settings.ServiceCostHangar.Should().Be(800m);
            settings.ServiceCostCatering.Should().Be(400m);
            settings.ServiceCostGroundHandling.Should().Be(600m);
            settings.ServiceCostDeIcing.Should().Be(300m);
        }

        [Fact]
        public void CurrentSettings_CanModifyIsSimConnectEnabled()
        {
            // Arrange
            var service = new SettingsService(_mockLogger.Object);
            var originalValue = service.CurrentSettings.IsSimConnectEnabled;

            // Act
            service.CurrentSettings.IsSimConnectEnabled = !originalValue;

            // Assert
            service.CurrentSettings.IsSimConnectEnabled.Should().Be(!originalValue);

            // Cleanup
            service.CurrentSettings.IsSimConnectEnabled = originalValue;
        }

        [Fact]
        public void CurrentSettings_CanModifyWindowPosition()
        {
            // Arrange
            var service = new SettingsService(_mockLogger.Object);

            // Act
            service.CurrentSettings.WindowTop = 200;
            service.CurrentSettings.WindowLeft = 300;
            service.CurrentSettings.WindowWidth = 1600;
            service.CurrentSettings.WindowHeight = 900;
            service.CurrentSettings.IsMaximized = true;

            // Assert
            service.CurrentSettings.WindowTop.Should().Be(200);
            service.CurrentSettings.WindowLeft.Should().Be(300);
            service.CurrentSettings.WindowWidth.Should().Be(1600);
            service.CurrentSettings.WindowHeight.Should().Be(900);
            service.CurrentSettings.IsMaximized.Should().BeTrue();
        }

        [Fact]
        public void CurrentSettings_CanModifyRates()
        {
            // Arrange
            var service = new SettingsService(_mockLogger.Object);

            // Act
            service.CurrentSettings.RatePerPaxPerNMSmall = 0.20m;

            // Assert
            service.CurrentSettings.RatePerPaxPerNMSmall.Should().Be(0.20m);
        }

        [Fact]
        public void CurrentSettings_CanModifyLastAirports()
        {
            // Arrange
            var service = new SettingsService(_mockLogger.Object);

            // Act
            service.CurrentSettings.LastDepartureIcao = "EDDF";
            service.CurrentSettings.LastArrivalIcao = "EDDM";

            // Assert
            service.CurrentSettings.LastDepartureIcao.Should().Be("EDDF");
            service.CurrentSettings.LastArrivalIcao.Should().Be("EDDM");
        }

        [Fact]
        public void CurrentSettings_CanModifyFBORents()
        {
            // Arrange
            var service = new SettingsService(_mockLogger.Object);

            // Act
            service.CurrentSettings.FBORentLocal = 600m;
            service.CurrentSettings.FBORentRegional = 1800m;
            service.CurrentSettings.FBORentInternational = 6000m;

            // Assert
            service.CurrentSettings.FBORentLocal.Should().Be(600m);
            service.CurrentSettings.FBORentRegional.Should().Be(1800m);
            service.CurrentSettings.FBORentInternational.Should().Be(6000m);
        }

        [Fact]
        public void CurrentSettings_CanModifyTerminalCosts()
        {
            // Arrange
            var service = new SettingsService(_mockLogger.Object);

            // Act
            service.CurrentSettings.TerminalCostSmall = 1200m;
            service.CurrentSettings.TerminalCostMedium = 3500m;
            service.CurrentSettings.TerminalCostLarge = 9000m;

            // Assert
            service.CurrentSettings.TerminalCostSmall.Should().Be(1200m);
            service.CurrentSettings.TerminalCostMedium.Should().Be(3500m);
            service.CurrentSettings.TerminalCostLarge.Should().Be(9000m);
        }

        [Fact]
        public void CurrentSettings_CanModifyServiceCosts()
        {
            // Arrange
            var service = new SettingsService(_mockLogger.Object);

            // Act
            service.CurrentSettings.ServiceCostRefueling = 600m;
            service.CurrentSettings.ServiceCostHangar = 900m;
            service.CurrentSettings.ServiceCostCatering = 500m;
            service.CurrentSettings.ServiceCostGroundHandling = 700m;
            service.CurrentSettings.ServiceCostDeIcing = 400m;

            // Assert
            service.CurrentSettings.ServiceCostRefueling.Should().Be(600m);
            service.CurrentSettings.ServiceCostHangar.Should().Be(900m);
            service.CurrentSettings.ServiceCostCatering.Should().Be(500m);
            service.CurrentSettings.ServiceCostGroundHandling.Should().Be(700m);
            service.CurrentSettings.ServiceCostDeIcing.Should().Be(400m);
        }

        [Fact]
        public void Save_ShouldLogSuccess()
        {
            // Arrange
            var service = new SettingsService(_mockLogger.Object);

            // Act
            service.Save();

            // Assert
            _mockLogger.Verify(
                x => x.Info(It.Is<string>(s => s.Contains("Settings saved"))),
                Times.AtLeastOnce);
        }

        [Fact]
        public void Load_ShouldLogSuccess()
        {
            // Arrange
            var service = new SettingsService(_mockLogger.Object);

            // Assert - Constructor calls Load(), so it should have logged
            _mockLogger.Verify(
                x => x.Info(It.IsAny<string>()),
                Times.AtLeastOnce);
        }

        [Fact]
        public void MultipleInstances_ShouldShareSameSettings()
        {
            // Arrange
            var service1 = new SettingsService(_mockLogger.Object);
            var testValue = 0.99m;

            // Act
            service1.CurrentSettings.RatePerPaxPerNMSmall = testValue;
            service1.Save();

            var service2 = new SettingsService(_mockLogger.Object);

            // Assert
            service2.CurrentSettings.RatePerPaxPerNMSmall.Should().Be(testValue);

            // Cleanup
            service1.CurrentSettings.RatePerPaxPerNMSmall = 0.15m;
            service1.Save();
        }

        [Fact]
        public void AppSettings_AllPropertiesArePubliclyAccessible()
        {
            // Arrange
            var settings = new AppSettings();

            // Act & Assert - verify all properties can be read and written
            settings.IsSimConnectEnabled = true;
            settings.IsSimConnectEnabled.Should().BeTrue();

            settings.AutoStartTracking = true;
            settings.AutoStartTracking.Should().BeTrue();

            settings.WindowTop = 50;
            settings.WindowTop.Should().Be(50);

            settings.WindowLeft = 60;
            settings.WindowLeft.Should().Be(60);

            settings.WindowWidth = 1920;
            settings.WindowWidth.Should().Be(1920);

            settings.WindowHeight = 1080;
            settings.WindowHeight.Should().Be(1080);

            settings.IsMaximized = true;
            settings.IsMaximized.Should().BeTrue();
        }
    }
}
