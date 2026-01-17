using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Repositories;
using Xunit;

namespace Ace.App.Tests.Repositories
{
    public class FBORepositoryTests
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly FBORepository _repository;

        public FBORepositoryTests()
        {
            _mockLogger = new Mock<ILoggingService>();
            _repository = new FBORepository(_mockLogger.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            Action act = () => new FBORepository(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetAllFBOs_ShouldNotThrow()
        {
            Action act = () => _repository.GetAllFBOs();
            act.Should().NotThrow();
        }

        [Fact]
        public void GetAllFBOs_ShouldReturnList()
        {
            var fbos = _repository.GetAllFBOs();
            fbos.Should().NotBeNull();
        }
    }
}
