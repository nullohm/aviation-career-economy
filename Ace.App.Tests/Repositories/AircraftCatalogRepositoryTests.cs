using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Repositories;
using Xunit;

namespace Ace.App.Tests.Repositories
{
    public class AircraftCatalogRepositoryTests
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly AircraftCatalogRepository _repository;

        public AircraftCatalogRepositoryTests()
        {
            _mockLogger = new Mock<ILoggingService>();
            _repository = new AircraftCatalogRepository(_mockLogger.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            Action act = () => new AircraftCatalogRepository(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetAllAircraft_ShouldNotThrow()
        {
            Action act = () => _repository.GetAllAircraft();
            act.Should().NotThrow();
        }

        [Fact]
        public void GetAllAircraft_ShouldReturnList()
        {
            var entries = _repository.GetAllAircraft();
            entries.Should().NotBeNull();
        }
    }
}
