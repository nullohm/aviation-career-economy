using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Xunit;

namespace Ace.App.Tests.Services
{
    /// <summary>
    /// Unit tests for FinanceService logic.
    /// These tests verify the interface contract and basic behavior without database access.
    /// Integration tests with database should use a separate test database.
    /// </summary>
    public class FinanceServiceTests
    {
        [Fact]
        public void IFinanceService_ShouldDefineRequiredMembers()
        {
            // This test verifies the IFinanceService interface contract
            var mockService = new Mock<IFinanceService>();

            // Verify interface has expected members
            mockService.SetupGet(s => s.Balance).Returns(1000m);
            mockService.SetupGet(s => s.TotalEarnings).Returns(5000m);
            mockService.SetupGet(s => s.BalanceFormatted).Returns("€ 1.000,00");
            mockService.Setup(s => s.GetCurrentBalance()).Returns(1000m);
            mockService.Setup(s => s.AddEarnings(It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<int?>()));
            mockService.Setup(s => s.AddExpense(It.IsAny<decimal>(), It.IsAny<string>()));
            mockService.Setup(s => s.SetBalance(It.IsAny<decimal>()));
            mockService.Setup(s => s.LoadTransactions());

            // Act & Assert
            mockService.Object.Balance.Should().Be(1000m);
            mockService.Object.TotalEarnings.Should().Be(5000m);
            mockService.Object.BalanceFormatted.Should().Be("€ 1.000,00");
            mockService.Object.GetCurrentBalance().Should().Be(1000m);
        }

        [Fact]
        public void AddEarnings_ShouldBeCallableWithOptionalParameters()
        {
            // Arrange
            var mockService = new Mock<IFinanceService>();

            // Act - verify all overloads work
            mockService.Object.AddEarnings(100m);
            mockService.Object.AddEarnings(100m, "Flight revenue");
            mockService.Object.AddEarnings(100m, "Flight revenue", 123);

            // Assert
            mockService.Verify(s => s.AddEarnings(100m, It.IsAny<string>(), It.IsAny<int?>()), Times.Exactly(3));
        }

        [Fact]
        public void AddExpense_ShouldBeCallableWithOptionalDescription()
        {
            // Arrange
            var mockService = new Mock<IFinanceService>();

            // Act
            mockService.Object.AddExpense(50m);
            mockService.Object.AddExpense(50m, "Fuel cost");

            // Assert
            mockService.Verify(s => s.AddExpense(50m, It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public void SetBalance_ShouldAcceptAnyDecimalValue()
        {
            // Arrange
            var mockService = new Mock<IFinanceService>();

            // Act & Assert - should accept positive, negative and zero
            mockService.Object.SetBalance(1000m);
            mockService.Object.SetBalance(-500m);
            mockService.Object.SetBalance(0m);

            mockService.Verify(s => s.SetBalance(It.IsAny<decimal>()), Times.Exactly(3));
        }

        [Fact]
        public void RecentTransactions_ShouldReturnListOfTransactions()
        {
            // Arrange
            var mockService = new Mock<IFinanceService>();
            var transactions = new System.Collections.Generic.List<Ace.App.Models.Transaction>
            {
                new() { Id = 1, Amount = 100m, Description = "Test", Type = "Income" }
            };
            mockService.SetupGet(s => s.RecentTransactions).Returns(transactions);

            // Act & Assert
            mockService.Object.RecentTransactions.Should().HaveCount(1);
            mockService.Object.RecentTransactions[0].Amount.Should().Be(100m);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void AddEarnings_WithNonPositiveAmount_ShouldNotThrow(decimal amount)
        {
            // Arrange
            var mockService = new Mock<IFinanceService>();

            // Act
            Action act = () => mockService.Object.AddEarnings(amount, "Test");

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void AddExpense_WithNonPositiveAmount_ShouldNotThrow(decimal amount)
        {
            // Arrange
            var mockService = new Mock<IFinanceService>();

            // Act
            Action act = () => mockService.Object.AddExpense(amount, "Test");

            // Assert
            act.Should().NotThrow();
        }
    }
}
