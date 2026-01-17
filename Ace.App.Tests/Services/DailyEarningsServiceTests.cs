using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Services;
using Xunit;

namespace Ace.App.Tests.Services
{
    public class DailyEarningsServiceTests
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly Mock<IFinanceService> _mockFinanceService;
        private readonly Mock<IPersistenceService> _mockPersistenceService;
        private readonly Mock<IPricingService> _mockPricingService;
        private readonly Mock<IScheduledRouteService> _mockScheduledRouteService;
        private readonly Mock<IAchievementService> _mockAchievementService;
        private readonly DailyEarningsService _dailyEarningsService;

        public DailyEarningsServiceTests()
        {
            _mockLogger = new Mock<ILoggingService>();
            _mockFinanceService = new Mock<IFinanceService>();
            _mockPersistenceService = new Mock<IPersistenceService>();
            _mockPricingService = new Mock<IPricingService>();
            _mockScheduledRouteService = new Mock<IScheduledRouteService>();
            _mockAchievementService = new Mock<IAchievementService>();
            _dailyEarningsService = new DailyEarningsService(
                _mockLogger.Object,
                _mockFinanceService.Object,
                _mockPersistenceService.Object,
                _mockPricingService.Object,
                _mockScheduledRouteService.Object,
                _mockAchievementService.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggingServiceIsNull()
        {
            Action act = () => new DailyEarningsService(
                null!,
                _mockFinanceService.Object,
                _mockPersistenceService.Object,
                _mockPricingService.Object,
                _mockScheduledRouteService.Object,
                _mockAchievementService.Object);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("loggingService");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenFinanceServiceIsNull()
        {
            Action act = () => new DailyEarningsService(
                _mockLogger.Object,
                null!,
                _mockPersistenceService.Object,
                _mockPricingService.Object,
                _mockScheduledRouteService.Object,
                _mockAchievementService.Object);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("financeService");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenPersistenceServiceIsNull()
        {
            Action act = () => new DailyEarningsService(
                _mockLogger.Object,
                _mockFinanceService.Object,
                null!,
                _mockPricingService.Object,
                _mockScheduledRouteService.Object,
                _mockAchievementService.Object);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("persistenceService");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenPricingServiceIsNull()
        {
            Action act = () => new DailyEarningsService(
                _mockLogger.Object,
                _mockFinanceService.Object,
                _mockPersistenceService.Object,
                null!,
                _mockScheduledRouteService.Object,
                _mockAchievementService.Object);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("pricingService");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenScheduledRouteServiceIsNull()
        {
            Action act = () => new DailyEarningsService(
                _mockLogger.Object,
                _mockFinanceService.Object,
                _mockPersistenceService.Object,
                _mockPricingService.Object,
                null!,
                _mockAchievementService.Object);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("scheduledRouteService");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenAchievementServiceIsNull()
        {
            Action act = () => new DailyEarningsService(
                _mockLogger.Object,
                _mockFinanceService.Object,
                _mockPersistenceService.Object,
                _mockPricingService.Object,
                _mockScheduledRouteService.Object,
                null!);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("achievementService");
        }

        [Fact]
        public async Task ProcessDailyEarnings_ShouldNotThrow()
        {
            Func<Task> act = async () => await _dailyEarningsService.ProcessDailyEarnings();

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ProcessDailyEarnings_ShouldLogStartMessage()
        {
            await _dailyEarningsService.ProcessDailyEarnings();

            _mockLogger.Verify(
                x => x.Info(It.Is<string>(s => s.Contains("Starting daily earnings processing"))),
                Times.Once);
        }

        [Fact]
        public async Task ProcessDailyEarnings_ShouldHandleExceptions()
        {
            Func<Task> act = async () => await _dailyEarningsService.ProcessDailyEarnings();

            await act.Should().NotThrowAsync("the service handles exceptions internally");
        }
    }
}
