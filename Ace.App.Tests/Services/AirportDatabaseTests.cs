using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Services;
using Xunit;

namespace Ace.App.Tests.Services
{
    /// <summary>
    /// Unit tests for AirportDatabase and Airport
    /// </summary>
    public class AirportDatabaseTests
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly Mock<ISettingsService> _mockSettings;
        private readonly AirportDatabase _airportDb;

        public AirportDatabaseTests()
        {
            _mockLogger = new Mock<ILoggingService>();
            _mockSettings = new Mock<ISettingsService>();
            _mockSettings.Setup(s => s.CurrentSettings).Returns(new AppSettings());
            _airportDb = new AirportDatabase(_mockLogger.Object, _mockSettings.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggingServiceIsNull()
        {
            // Act & Assert
            Action act = () => new AirportDatabase(null!, _mockSettings.Object);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("loggingService");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenSettingsServiceIsNull()
        {
            // Act & Assert
            Action act = () => new AirportDatabase(_mockLogger.Object, null!);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("settingsService");
        }

        [Fact]
        public void Initialize_ShouldNotThrow()
        {
            // Act
            Action act = () => _airportDb.Initialize();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Initialize_WithPath_ShouldNotThrow()
        {
            // Act
            Action act = () => _airportDb.Initialize("C:\\SomePath");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Initialize_MultipleCalls_ShouldNotThrow()
        {
            // Act
            Action act = () =>
            {
                _airportDb.Initialize();
                _airportDb.Initialize();
                _airportDb.Initialize();
            };

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Airport_DefaultConstructor_ShouldInitializeWithEmptyStrings()
        {
            // Act
            var airport = new Airport();

            // Assert
            airport.Icao.Should().BeEmpty();
            airport.Name.Should().BeEmpty();
            airport.Latitude.Should().Be(0);
            airport.Longitude.Should().Be(0);
        }

        [Fact]
        public void Airport_ICAO_PropertyShouldReturnIcao()
        {
            // Arrange
            var airport = new Airport { Icao = "EDDF" };

            // Act & Assert
            airport.ICAO.Should().Be("EDDF");
            airport.ICAO.Should().Be(airport.Icao);
        }

        [Fact]
        public void Airport_ShouldAllowPropertyAssignment()
        {
            // Arrange
            var airport = new Airport();

            // Act
            airport.Icao = "EDDM";
            airport.Name = "Munich Airport";
            airport.Latitude = 48.3538;
            airport.Longitude = 11.7861;

            // Assert
            airport.Icao.Should().Be("EDDM");
            airport.Name.Should().Be("Munich Airport");
            airport.Latitude.Should().Be(48.3538);
            airport.Longitude.Should().Be(11.7861);
        }

        [Fact]
        public void Airport_ShouldHandleNegativeCoordinates()
        {
            // Arrange
            var airport = new Airport
            {
                Latitude = -33.9461,
                Longitude = -18.6017
            };

            // Act & Assert
            airport.Latitude.Should().Be(-33.9461);
            airport.Longitude.Should().Be(-18.6017);
        }
    }
}
