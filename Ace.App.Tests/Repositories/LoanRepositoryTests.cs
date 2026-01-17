using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Repositories;
using Xunit;

namespace Ace.App.Tests.Repositories
{
    public class LoanRepositoryTests
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly LoanRepository _repository;

        public LoanRepositoryTests()
        {
            _mockLogger = new Mock<ILoggingService>();
            _repository = new LoanRepository(_mockLogger.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            Action act = () => new LoanRepository(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetActiveLoans_ShouldNotThrow()
        {
            Action act = () => _repository.GetActiveLoans();
            act.Should().NotThrow();
        }

        [Fact]
        public void GetActiveLoans_ShouldReturnList()
        {
            var loans = _repository.GetActiveLoans();
            loans.Should().NotBeNull();
        }
    }
}
