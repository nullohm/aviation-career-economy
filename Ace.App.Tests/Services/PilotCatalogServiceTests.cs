using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Services;
using Xunit;

namespace Ace.App.Tests.Services
{
    public class PilotCatalogServiceTests
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly PilotCatalogService _catalogService;

        public PilotCatalogServiceTests()
        {
            _mockLogger = new Mock<ILoggingService>();
            _catalogService = new PilotCatalogService(_mockLogger.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggingServiceIsNull()
        {
            Action act = () => new PilotCatalogService(null!);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("loggingService");
        }

        [Fact]
        public void GetAvailablePilots_ShouldNotThrow()
        {
            Action act = () => _catalogService.GetAvailablePilots();
            act.Should().NotThrow();
        }

        [Fact]
        public void GetEmployedPilots_ShouldNotThrow()
        {
            Action act = () => _catalogService.GetEmployedPilots();
            act.Should().NotThrow();
        }

        [Fact]
        public void GetPilotImagePath_ShouldReturnPath_ForValidFileName()
        {
            var result = _catalogService.GetPilotImagePath("test.jpg");
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("test.jpg");
        }

        [Fact]
        public void GetPilotImagePath_ShouldReturnDefaultPath_ForEmptyFileName()
        {
            var result = _catalogService.GetPilotImagePath(string.Empty);
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("myPilot.bmp");
        }
    }
}
