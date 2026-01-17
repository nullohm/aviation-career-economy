using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Repositories;
using Xunit;

namespace Ace.App.Tests.Repositories
{
    public class FlightRepositoryTests
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly FlightRepository _repository;

        public FlightRepositoryTests()
        {
            _mockLogger = new Mock<ILoggingService>();
            _repository = new FlightRepository(_mockLogger.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            Action act = () => new FlightRepository(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetAllFlights_ShouldNotThrow()
        {
            Action act = () => _repository.GetAllFlights();
            act.Should().NotThrow();
        }

        [Fact]
        public void GetAllFlights_ShouldReturnList()
        {
            var flights = _repository.GetAllFlights();
            flights.Should().NotBeNull();
        }
    }
}
