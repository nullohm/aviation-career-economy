using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Repositories;
using Xunit;

namespace Ace.App.Tests.Repositories
{
    /// <summary>
    /// Unit tests for AircraftRepository
    /// Note: These are integration tests that use the real database
    /// </summary>
    public class AircraftRepositoryTests
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly AircraftRepository _repository;

        public AircraftRepositoryTests()
        {
            _mockLogger = new Mock<ILoggingService>();
            _repository = new AircraftRepository(_mockLogger.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            // Act & Assert
            Action act = () => new AircraftRepository(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetAllAircraft_ShouldNotThrow()
        {
            // Act
            Action act = () => _repository.GetAllAircraft();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void GetAllAircraft_ShouldReturnList()
        {
            // Act
            var aircraft = _repository.GetAllAircraft();

            // Assert
            aircraft.Should().NotBeNull();
            aircraft.Should().BeAssignableTo<System.Collections.Generic.List<Ace.App.Models.Aircraft>>();
        }

        [Fact]
        public void GetAircraftById_ShouldNotThrow()
        {
            // Act
            Action act = () => _repository.GetAircraftById(1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void GetAircraftById_ShouldReturnNull_ForNonExistentId()
        {
            // Act
            var aircraft = _repository.GetAircraftById(999999);

            // Assert
            aircraft.Should().BeNull();
        }

        [Fact]
        public void GetAircraftByRegistration_ShouldNotThrow()
        {
            // Act
            Action act = () => _repository.GetAircraftByRegistration("D-TEST");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void GetAircraftByRegistration_ShouldReturnNull_ForNonExistentRegistration()
        {
            // Act
            var aircraft = _repository.GetAircraftByRegistration("NONEXISTENT");

            // Assert
            aircraft.Should().BeNull();
        }

        [Fact]
        public void GetAircraftByStatus_ShouldNotThrow()
        {
            // Act
            Action act = () => _repository.GetAircraftByStatus(Ace.App.Models.AircraftStatus.Available);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void GetAircraftByStatus_ShouldReturnList()
        {
            // Act
            var aircraft = _repository.GetAircraftByStatus(Ace.App.Models.AircraftStatus.Available);

            // Assert
            aircraft.Should().NotBeNull();
            aircraft.Should().BeAssignableTo<System.Collections.Generic.List<Ace.App.Models.Aircraft>>();
        }
    }
}
