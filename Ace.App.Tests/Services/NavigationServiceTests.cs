using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Services;
using Xunit;

namespace Ace.App.Tests.Services
{
    /// <summary>
    /// Unit tests for NavigationService
    /// Note: Full WPF navigation tests are limited due to STA thread requirements
    /// These tests focus on business logic and error handling
    /// </summary>
    public class NavigationServiceTests
    {
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly NavigationService _navigationService;

        public NavigationServiceTests()
        {
            _mockServiceProvider = new Mock<IServiceProvider>();
            _mockLogger = new Mock<ILoggingService>();
            _navigationService = new NavigationService(_mockServiceProvider.Object, _mockLogger.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenServiceProviderIsNull()
        {
            // Act & Assert
            Action act = () => new NavigationService(null!, _mockLogger.Object);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("serviceProvider");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            // Act & Assert
            Action act = () => new NavigationService(_mockServiceProvider.Object, null!);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("logger");
        }

        [Fact]
        public void Constructor_ShouldNotThrow_WithValidParameters()
        {
            // Act
            Action act = () => new NavigationService(_mockServiceProvider.Object, _mockLogger.Object);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void NavigateTo_Type_ShouldThrowInvalidOperationException_WhenNotInitialized()
        {
            // Arrange
            var viewType = typeof(System.Windows.Controls.UserControl);

            // Act & Assert
            Action act = () => _navigationService.NavigateTo(viewType);
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*must be initialized*");
        }

        [Fact]
        public void NavigateTo_Type_ShouldLogError_WhenNotInitialized()
        {
            // Arrange
            var viewType = typeof(System.Windows.Controls.UserControl);

            // Act
            try
            {
                _navigationService.NavigateTo(viewType);
            }
            catch
            {
                // Expected
            }

            // Assert
            _mockLogger.Verify(
                x => x.Error(It.Is<string>(s => s.Contains("Not initialized")), It.IsAny<Exception>()),
                Times.Once);
        }
    }
}
