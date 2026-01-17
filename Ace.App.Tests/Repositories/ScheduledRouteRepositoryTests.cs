using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Repositories;
using Xunit;

namespace Ace.App.Tests.Repositories
{
    public class ScheduledRouteRepositoryTests
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly ScheduledRouteRepository _repository;

        public ScheduledRouteRepositoryTests()
        {
            _mockLogger = new Mock<ILoggingService>();
            _repository = new ScheduledRouteRepository(_mockLogger.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            Action act = () => new ScheduledRouteRepository(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetAllRoutes_ShouldNotThrow()
        {
            Action act = () => _repository.GetAllRoutes();
            act.Should().NotThrow();
        }

        [Fact]
        public void GetAllRoutes_ShouldReturnList()
        {
            var routes = _repository.GetAllRoutes();
            routes.Should().NotBeNull();
        }

        [Fact]
        public void GetActiveRoutes_ShouldNotThrow()
        {
            Action act = () => _repository.GetActiveRoutes();
            act.Should().NotThrow();
        }

        [Fact]
        public void GetActiveRoutes_ShouldReturnList()
        {
            var routes = _repository.GetActiveRoutes();
            routes.Should().NotBeNull();
        }

        [Fact]
        public void GetRouteById_ShouldNotThrow()
        {
            Action act = () => _repository.GetRouteById(1);
            act.Should().NotThrow();
        }

        [Fact]
        public void GetRouteById_ShouldReturnNull_ForNonExistentId()
        {
            var route = _repository.GetRouteById(999999);
            route.Should().BeNull();
        }

        [Fact]
        public void GetRouteByAircraft_ShouldNotThrow()
        {
            Action act = () => _repository.GetRouteByAircraft(1);
            act.Should().NotThrow();
        }

        [Fact]
        public void GetRouteByAircraft_ShouldReturnNull_ForNonExistentAircraft()
        {
            var route = _repository.GetRouteByAircraft(999999);
            route.Should().BeNull();
        }

        [Fact]
        public void GetRoutesByFBO_ShouldNotThrow()
        {
            Action act = () => _repository.GetRoutesByFBO(1);
            act.Should().NotThrow();
        }

        [Fact]
        public void GetRoutesByFBO_ShouldReturnList()
        {
            var routes = _repository.GetRoutesByFBO(1);
            routes.Should().NotBeNull();
        }

        [Fact]
        public void GetRouteCountForFBO_ShouldNotThrow()
        {
            Action act = () => _repository.GetRouteCountForFBO(1);
            act.Should().NotThrow();
        }

        [Fact]
        public void GetRouteCountForFBO_ShouldReturnZero_ForNonExistentFBO()
        {
            var count = _repository.GetRouteCountForFBO(999999);
            count.Should().Be(0);
        }
    }
}
