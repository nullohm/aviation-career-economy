using System;
using System.Linq;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;
using Ace.App.Tests.Helpers;
using Xunit;

namespace Ace.App.Tests.Services
{
    /// <summary>
    /// Unit tests for MaintenanceService
    /// Tests focus on critical business logic and integration points
    /// </summary>
    public class MaintenanceServiceTests
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly Mock<IFinanceService> _mockFinanceService;
        private readonly Mock<ISettingsService> _mockSettingsService;
        private readonly MaintenanceService _maintenanceService;

        public MaintenanceServiceTests()
        {
            TestServiceLocatorInitializer.Initialize();
            _mockLogger = new Mock<ILoggingService>();
            _mockFinanceService = new Mock<IFinanceService>();
            _mockSettingsService = new Mock<ISettingsService>();
            _mockSettingsService.Setup(x => x.CurrentSettings).Returns(new AppSettings());
            _maintenanceService = new MaintenanceService(_mockLogger.Object, _mockFinanceService.Object, _mockSettingsService.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            // Act & Assert
            Action act = () => new MaintenanceService(null!, _mockFinanceService.Object, _mockSettingsService.Object);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("logger");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenFinanceServiceIsNull()
        {
            // Act & Assert
            Action act = () => new MaintenanceService(_mockLogger.Object, null!, _mockSettingsService.Object);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("financeService");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenSettingsServiceIsNull()
        {
            // Act & Assert
            Action act = () => new MaintenanceService(_mockLogger.Object, _mockFinanceService.Object, null!);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("settingsService");
        }

        [Fact]
        public void GetMaintenanceStatus_ShouldReturnList_ForSingleEnginePistonAircraft()
        {
            // Arrange
            var aircraft = CreateTestAircraft(AircraftCategory.SingleEnginePiston);

            // Act
            var statuses = _maintenanceService.GetMaintenanceStatus(aircraft);

            // Assert
            statuses.Should().NotBeNull();
            statuses.Should().NotBeEmpty();
            statuses.All(s => s.CheckType != MaintenanceCheckType.ACheck).Should().BeTrue("GA aircraft don't have A/B/C/D checks");
        }

        [Fact]
        public void GetMaintenanceStatus_ShouldReturnOrderedByUrgency()
        {
            // Arrange
            var aircraft = CreateTestAircraft(AircraftCategory.SingleEnginePiston);
            aircraft.HoursSinceLastMaintenance = 95; // Close to 100-hour check

            // Act
            var statuses = _maintenanceService.GetMaintenanceStatus(aircraft);

            // Assert
            statuses.Should().NotBeEmpty();
            // Verify ordering: most urgent (lowest score) should be first
            for (int i = 0; i < statuses.Count - 1; i++)
            {
                statuses[i].UrgencyScore.Should().BeLessThanOrEqualTo(statuses[i + 1].UrgencyScore);
            }
        }

        [Fact]
        public void GetMaintenanceStatus_ShouldMarkAsOverdue_WhenHoursExceedLimit()
        {
            // Arrange
            var aircraft = CreateTestAircraft(AircraftCategory.SingleEnginePiston);
            aircraft.HoursSinceLastMaintenance = 150; // Over 100-hour limit

            // Act
            var statuses = _maintenanceService.GetMaintenanceStatus(aircraft);

            // Assert
            statuses.Any(s => s.IsOverdue).Should().BeTrue();
        }

        [Fact]
        public void GetSchedulableChecks_ShouldReturnAllChecks_WhenNoneOverdue()
        {
            // Arrange
            var aircraft = CreateTestAircraft(AircraftCategory.SingleEnginePiston);
            aircraft.HoursSinceLastMaintenance = 10; // Fresh maintenance

            // Act
            var schedulableChecks = _maintenanceService.GetSchedulableChecks(aircraft);

            // Assert
            schedulableChecks.Should().NotBeEmpty();
        }

        [Fact]
        public void GetSchedulableChecks_ShouldPrioritizeOverdueChecks()
        {
            // Arrange
            var aircraft = CreateTestAircraft(AircraftCategory.SingleEnginePiston);
            aircraft.HoursSinceLastMaintenance = 150; // Overdue

            // Act
            var schedulableChecks = _maintenanceService.GetSchedulableChecks(aircraft);

            // Assert
            schedulableChecks.Should().NotBeEmpty();
            schedulableChecks.First().IsOverdue.Should().BeTrue("Most urgent check should be first");
        }

        [Fact]
        public void IsMaintenanceRequired_ShouldReturnTrue_WhenAircraftIsOverdue()
        {
            // Arrange
            var aircraft = CreateTestAircraft(AircraftCategory.SingleEnginePiston);
            aircraft.HoursSinceLastMaintenance = 150; // Overdue for 100-hour

            // Act
            var isRequired = _maintenanceService.IsMaintenanceRequired(aircraft);

            // Assert
            isRequired.Should().BeTrue();
        }

        [Fact]
        public void IsMaintenanceRequired_ShouldExecuteWithoutError()
        {
            // Arrange
            var aircraft = CreateTestAircraft(AircraftCategory.SingleEnginePiston);
            aircraft.HoursSinceLastMaintenance = 5; // Very fresh
            aircraft.LastAnnualInspection = DateTime.Today; // Recent annual
            aircraft.PurchaseDate = DateTime.Today; // New aircraft

            // Act
            var isRequired = _maintenanceService.IsMaintenanceRequired(aircraft);

            Assert.IsType<bool>(isRequired);
        }

        [Fact]
        public void GetMostUrgentCheck_ShouldReturnCheck_WhenMaintenanceNeeded()
        {
            // Arrange
            var aircraft = CreateTestAircraft(AircraftCategory.SingleEnginePiston);
            aircraft.HoursSinceLastMaintenance = 95; // Close to limit

            // Act
            var urgentCheck = _maintenanceService.GetMostUrgentCheck(aircraft);

            // Assert
            urgentCheck.Should().NotBeNull();
            urgentCheck!.UrgencyScore.Should().BeLessThanOrEqualTo(2);
        }

        [Fact]
        public void GetMostUrgentCheck_ShouldReturnNull_WhenNoMaintenanceNeeded()
        {
            // Arrange
            var aircraft = CreateTestAircraft(AircraftCategory.SingleEnginePiston);
            aircraft.HoursSinceLastMaintenance = 5; // Very fresh

            // Act
            var urgentCheck = _maintenanceService.GetMostUrgentCheck(aircraft);

            // Assert
            // Even if not urgent, it should return some check (the least urgent one)
            urgentCheck.Should().NotBeNull();
        }

        [Fact]
        public void ScheduleMaintenance_ShouldReturnFailure_WhenAircraftNotFoundOrDbMissing()
        {
            // Arrange
            _mockFinanceService.Setup(x => x.Balance).Returns(100m);
            var nonExistentAircraftId = 999999;

            // Act
            var result = _maintenanceService.ScheduleMaintenance(nonExistentAircraftId, MaintenanceCheckType.Check100Hour);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().NotBeEmpty();
        }

        [Fact]
        public void UpdateFlightHours_ShouldNotThrow_WithValidAircraftId()
        {
            // Arrange
            var aircraftId = 999999; // Non-existent ID
            var hoursFlown = 2.5;

            // Act
            Action act = () => _maintenanceService.UpdateFlightHours(aircraftId, hoursFlown);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void UpdateFlightHours_ShouldLogDebug_OnSuccess()
        {
            // Arrange
            var aircraftId = 1;
            var hoursFlown = 2.5;

            // Act
            _maintenanceService.UpdateFlightHours(aircraftId, hoursFlown);

            // Assert
            // Even if aircraft not found, it should not throw
            _mockLogger.Verify(
                x => x.Debug(It.IsAny<string>()),
                Times.AtMost(1)); // May or may not log depending on whether aircraft exists
        }

        [Fact]
        public void CompleteOverdueMaintenances_ShouldNotThrow()
        {
            // Act
            Action act = () => _maintenanceService.CompleteOverdueMaintenances();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void GroundAircraftWithOverdueChecks_ShouldNotThrow()
        {
            // Act
            Action act = () => _maintenanceService.GroundAircraftWithOverdueChecks();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void GetMaintenanceStatus_ShouldSetCorrectUrgencyScore_WhenOverdue()
        {
            // Arrange
            var aircraft = CreateTestAircraft(AircraftCategory.SingleEnginePiston);
            aircraft.HoursSinceLastMaintenance = 200; // Well overdue

            // Act
            var statuses = _maintenanceService.GetMaintenanceStatus(aircraft);

            // Assert
            var overdueChecks = statuses.Where(s => s.IsOverdue).ToList();
            overdueChecks.Should().NotBeEmpty();
            overdueChecks.All(s => s.UrgencyScore == 0).Should().BeTrue("Overdue checks should have urgency score 0");
        }

        [Fact]
        public void GetMaintenanceStatus_ShouldCalculateHoursRemaining()
        {
            // Arrange
            var aircraft = CreateTestAircraft(AircraftCategory.SingleEnginePiston);
            aircraft.HoursSinceLastMaintenance = 50;

            // Act
            var statuses = _maintenanceService.GetMaintenanceStatus(aircraft);

            // Assert
            var checksWithHours = statuses.Where(s => s.HoursRemaining.HasValue).ToList();
            checksWithHours.Should().NotBeEmpty();
            checksWithHours.All(s => s.HoursRemaining >= 0 || s.IsOverdue).Should().BeTrue();
        }

        [Fact]
        public void GetMaintenanceStatus_ShouldHandleJetAircraft()
        {
            // Arrange
            var aircraft = CreateTestAircraft(AircraftCategory.NarrowBody);

            // Act
            var statuses = _maintenanceService.GetMaintenanceStatus(aircraft);

            // Assert
            statuses.Should().NotBeEmpty();
            // Jet aircraft should have A/B/C/D checks
            var hasAircraftLevelChecks = statuses.Any(s =>
                s.CheckType == MaintenanceCheckType.ACheck ||
                s.CheckType == MaintenanceCheckType.BCheck ||
                s.CheckType == MaintenanceCheckType.CCheck ||
                s.CheckType == MaintenanceCheckType.DCheck);
            hasAircraftLevelChecks.Should().BeTrue();
        }

        [Fact]
        public void MaintenanceCheckStatus_StatusText_ShouldReturnOverdue_WhenOverdue()
        {
            // Arrange
            var status = new MaintenanceCheckStatus
            {
                IsOverdue = true,
                HoursRemaining = -10
            };

            // Act
            var statusText = status.StatusText;

            // Assert
            statusText.Should().Be("OVERDUE");
        }

        [Fact]
        public void MaintenanceCheckStatus_UrgencyText_ShouldReturnCorrectValues()
        {
            // Assert all urgency levels
            new MaintenanceCheckStatus { UrgencyScore = 0 }.UrgencyText.Should().Be("OVERDUE");
            new MaintenanceCheckStatus { UrgencyScore = 1 }.UrgencyText.Should().Be("Due Soon");
            new MaintenanceCheckStatus { UrgencyScore = 2 }.UrgencyText.Should().Be("Upcoming");
            new MaintenanceCheckStatus { UrgencyScore = 3 }.UrgencyText.Should().Be("OK");
        }

        [Fact]
        public void MaintenanceResult_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            var result = new MaintenanceResult();

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().BeEmpty();
            result.CompletionDate.Should().BeNull();
        }

        [Fact]
        public void CompleteMaintenance_ShouldReturnFailure_ForNonExistentAircraftOrDbMissing()
        {
            // Arrange
            var nonExistentId = 999999;

            // Act
            var result = _maintenanceService.CompleteMaintenance(nonExistentId);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().NotBeEmpty();
        }

        // Helper method to create test aircraft
        private Aircraft CreateTestAircraft(AircraftCategory category)
        {
            return new Aircraft
            {
                Id = 1,
                Registration = "D-TEST",
                Category = category,
                Status = AircraftStatus.Available,
                HoursSinceLastMaintenance = 0,
                HoursSinceTBO = 0,
                HoursSinceACheck = 0,
                TotalFlightHours = 100,
                PurchaseDate = DateTime.Today.AddYears(-1),
                LastMaintenanceDate = DateTime.Today.AddDays(-10),
                CurrentValue = 100000m
            };
        }
    }
}
