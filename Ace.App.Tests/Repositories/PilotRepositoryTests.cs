using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Repositories;
using Xunit;

namespace Ace.App.Tests.Repositories
{
    public class PilotRepositoryTests
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly PilotRepository _repository;

        public PilotRepositoryTests()
        {
            _mockLogger = new Mock<ILoggingService>();
            _repository = new PilotRepository(_mockLogger.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            Action act = () => new PilotRepository(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetAvailablePilots_ShouldNotThrow()
        {
            Action act = () => _repository.GetAvailablePilots();
            act.Should().NotThrow();
        }

        [Fact]
        public void GetAvailablePilots_ShouldReturnList()
        {
            var pilots = _repository.GetAvailablePilots();
            pilots.Should().NotBeNull();
        }

        [Fact]
        public void GetPilotById_ShouldNotThrow()
        {
            Action act = () => _repository.GetPilotById(1);
            act.Should().NotThrow();
        }
    }
}
