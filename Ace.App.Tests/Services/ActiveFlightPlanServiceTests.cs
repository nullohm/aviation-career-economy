using System;
using FluentAssertions;
using Moq;
using Ace.App.Interfaces;
using Ace.App.Services;
using Xunit;

namespace Ace.App.Tests.Services
{
    /// <summary>
    /// Unit tests for ActiveFlightPlanService
    /// </summary>
    public class ActiveFlightPlanServiceTests
    {
        private readonly Mock<ILoggingService> _mockLogger;
        private readonly ActiveFlightPlanService _service;

        public ActiveFlightPlanServiceTests()
        {
            _mockLogger = new Mock<ILoggingService>();
            _service = new ActiveFlightPlanService(_mockLogger.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggingServiceIsNull()
        {
            // Act & Assert
            Action act = () => new ActiveFlightPlanService(null!);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("loggingService");
        }

        [Fact]
        public void GetActivePlan_ShouldReturnNull_WhenNoPlanActivated()
        {
            // Act
            var plan = _service.GetActivePlan();

            // Assert
            plan.Should().BeNull();
        }

        [Fact]
        public void HasValidFlightPlan_ShouldReturnFalse_WhenNoPlanActivated()
        {
            // Act
            var hasValid = _service.HasValidFlightPlan();

            // Assert
            hasValid.Should().BeFalse();
        }

        [Fact]
        public void ActivateFlightPlan_ShouldCreateActivePlan()
        {
            // Act
            _service.ActivateFlightPlan("EDDF", "EDDM", 1, "D-TEST", "Cessna 172", 150.5, 3);

            // Assert
            var plan = _service.GetActivePlan();
            plan.Should().NotBeNull();
        }

        [Fact]
        public void ActivateFlightPlan_ShouldSetCorrectProperties()
        {
            // Arrange
            var departure = "EDDF";
            var arrival = "EDDM";
            var registration = "D-TEST";
            var aircraftType = "Cessna 172";
            var distance = 150.5;
            var passengers = 3;

            // Act
            _service.ActivateFlightPlan(departure, arrival, 1, registration, aircraftType, distance, passengers);

            // Assert
            var plan = _service.GetActivePlan();
            plan!.DepartureIcao.Should().Be(departure);
            plan.ArrivalIcao.Should().Be(arrival);
            plan.AircraftRegistration.Should().Be(registration);
            plan.AircraftType.Should().Be(aircraftType);
            plan.DistanceNM.Should().Be(distance);
            plan.Passengers.Should().Be(passengers);
        }

        [Fact]
        public void ActivateFlightPlan_ShouldSetActivatedAtToNow()
        {
            // Arrange
            var before = DateTime.Now;

            // Act
            _service.ActivateFlightPlan("EDDF", "EDDM", 1, "D-TEST", "Cessna 172", 150.5);

            // Assert
            var plan = _service.GetActivePlan();
            plan!.ActivatedAt.Should().BeOnOrAfter(before);
            plan.ActivatedAt.Should().BeOnOrBefore(DateTime.Now);
        }

        [Fact]
        public void ActivateFlightPlan_ShouldDefaultPassengersToZero()
        {
            // Act
            _service.ActivateFlightPlan("EDDF", "EDDM", 1, "D-TEST", "Cessna 172", 150.5);

            // Assert
            var plan = _service.GetActivePlan();
            plan!.Passengers.Should().Be(0);
        }

        [Fact]
        public void ActivateFlightPlan_ShouldLogInfo()
        {
            // Act
            _service.ActivateFlightPlan("EDDF", "EDDM", 1, "D-TEST", "Cessna 172", 150.5, 3);

            // Assert
            _mockLogger.Verify(
                x => x.Info(It.Is<string>(s =>
                    s.Contains("Flight plan activated") &&
                    s.Contains("EDDF") &&
                    s.Contains("EDDM"))),
                Times.Once);
        }

        [Fact]
        public void ActivateFlightPlan_ShouldFireFlightPlanChangedEvent()
        {
            // Arrange
            ActiveFlightPlan? capturedPlan = null;
            _service.FlightPlanChanged += (plan) => capturedPlan = plan;

            // Act
            _service.ActivateFlightPlan("EDDF", "EDDM", 1, "D-TEST", "Cessna 172", 150.5, 3);

            // Assert
            capturedPlan.Should().NotBeNull();
            capturedPlan!.DepartureIcao.Should().Be("EDDF");
        }

        [Fact]
        public void HasValidFlightPlan_ShouldReturnTrue_WhenPlanActivated()
        {
            // Arrange
            _service.ActivateFlightPlan("EDDF", "EDDM", 1, "D-TEST", "Cessna 172", 150.5);

            // Act
            var hasValid = _service.HasValidFlightPlan();

            // Assert
            hasValid.Should().BeTrue();
        }

        [Fact]
        public void ClearFlightPlan_ShouldSetPlanToNull()
        {
            // Arrange
            _service.ActivateFlightPlan("EDDF", "EDDM", 1, "D-TEST", "Cessna 172", 150.5);

            // Act
            _service.ClearFlightPlan();

            // Assert
            _service.GetActivePlan().Should().BeNull();
        }

        [Fact]
        public void ClearFlightPlan_ShouldMakeHasValidFlightPlanReturnFalse()
        {
            // Arrange
            _service.ActivateFlightPlan("EDDF", "EDDM", 1, "D-TEST", "Cessna 172", 150.5);

            // Act
            _service.ClearFlightPlan();

            // Assert
            _service.HasValidFlightPlan().Should().BeFalse();
        }

        [Fact]
        public void ClearFlightPlan_ShouldLogInfo_WhenPlanExists()
        {
            // Arrange
            _service.ActivateFlightPlan("EDDF", "EDDM", 1, "D-TEST", "Cessna 172", 150.5);

            // Act
            _service.ClearFlightPlan();

            // Assert
            _mockLogger.Verify(
                x => x.Info(It.Is<string>(s =>
                    s.Contains("Flight plan cleared") &&
                    s.Contains("EDDF") &&
                    s.Contains("EDDM"))),
                Times.Once);
        }

        [Fact]
        public void ClearFlightPlan_ShouldNotLog_WhenNoPlanExists()
        {
            // Act
            _service.ClearFlightPlan();

            // Assert
            _mockLogger.Verify(
                x => x.Info(It.Is<string>(s => s.Contains("Flight plan cleared"))),
                Times.Never);
        }

        [Fact]
        public void ClearFlightPlan_ShouldFireFlightPlanChangedEvent_WithNull()
        {
            // Arrange
            _service.ActivateFlightPlan("EDDF", "EDDM", 1, "D-TEST", "Cessna 172", 150.5);
            ActiveFlightPlan? capturedPlan = new ActiveFlightPlan(); // Non-null to start
            _service.FlightPlanChanged += (plan) => capturedPlan = plan;

            // Act
            _service.ClearFlightPlan();

            // Assert
            capturedPlan.Should().BeNull();
        }

        [Fact]
        public void ActivateFlightPlan_ShouldReplaceExistingPlan()
        {
            // Arrange
            _service.ActivateFlightPlan("EDDF", "EDDM", 1, "D-TEST1", "Cessna 172", 150.5, 3);

            // Act
            _service.ActivateFlightPlan("EDDH", "EDDK", 2, "D-TEST2", "Airbus A320", 250.0, 5);

            // Assert
            var plan = _service.GetActivePlan();
            plan!.DepartureIcao.Should().Be("EDDH");
            plan.ArrivalIcao.Should().Be("EDDK");
            plan.AircraftRegistration.Should().Be("D-TEST2");
            plan.DistanceNM.Should().Be(250.0);
            plan.Passengers.Should().Be(5);
        }

        [Fact]
        public void ActivateFlightPlan_ShouldHandleEmptyStrings()
        {
            // Act
            _service.ActivateFlightPlan("", "", 0, "", "", 0, 0);

            // Assert
            var plan = _service.GetActivePlan();
            plan.Should().NotBeNull();
            plan!.DepartureIcao.Should().BeEmpty();
            plan.ArrivalIcao.Should().BeEmpty();
        }

        [Fact]
        public void ActivateFlightPlan_ShouldHandleNegativeDistance()
        {
            // Act
            _service.ActivateFlightPlan("EDDF", "EDDM", 1, "D-TEST", "Test", -100, 0);

            // Assert
            var plan = _service.GetActivePlan();
            plan!.DistanceNM.Should().Be(-100);
        }

        [Fact]
        public void ActivateFlightPlan_ShouldHandleNegativePassengers()
        {
            // Act
            _service.ActivateFlightPlan("EDDF", "EDDM", 1, "D-TEST", "Test", 150, -5);

            // Assert
            var plan = _service.GetActivePlan();
            plan!.Passengers.Should().Be(-5);
        }

        [Fact]
        public void ActiveFlightPlan_DefaultConstructor_ShouldInitializeWithEmptyStrings()
        {
            // Act
            var plan = new ActiveFlightPlan();

            // Assert
            plan.DepartureIcao.Should().BeEmpty();
            plan.ArrivalIcao.Should().BeEmpty();
            plan.AircraftRegistration.Should().BeEmpty();
            plan.AircraftType.Should().BeEmpty();
            plan.DistanceNM.Should().Be(0);
            plan.Passengers.Should().Be(0);
        }
    }
}
