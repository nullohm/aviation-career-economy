using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Ace.App.Infrastructure;
using Ace.App.Interfaces;
using Ace.App.Services;

namespace Ace.App.Tests.Helpers
{
    public static class TestServiceLocatorInitializer
    {
        private static bool _isInitialized;
        private static readonly object _lock = new();

        public static void Initialize()
        {
            lock (_lock)
            {
                if (_isInitialized)
                    return;

                var services = new ServiceCollection();

                var mockSettingsService = new Mock<ISettingsService>();
                mockSettingsService.Setup(s => s.CurrentSettings).Returns(CreateDefaultSettings());
                services.AddSingleton(mockSettingsService.Object);

                var mockLoggingService = new Mock<ILoggingService>();
                services.AddSingleton(mockLoggingService.Object);

                var serviceProvider = services.BuildServiceProvider();
                ServiceLocator.Initialize(serviceProvider);

                _isInitialized = true;
            }
        }

        private static AppSettings CreateDefaultSettings()
        {
            return new AppSettings
            {
                MaintenanceCheck50Hour = 450m,
                MaintenanceCheck100Hour = 2500m,
                MaintenanceCheckAnnual = 3500m,
                MaintenanceCheckACheck = 20000m,
                MaintenanceCheckBCheck = 50000m,
                MaintenanceCheckCCheck = 300000m,
                MaintenanceCheckDCheck = 3500000m,
                FuelPricePerGallon = 6.0m,
                MaintenanceCostPerHourSmall = 40m,
                MaintenanceCostPerHourMedium = 200m,
                MaintenanceCostPerHourMediumLarge = 1000m,
                MaintenanceCostPerHourLarge = 2500m,
                MaintenanceCostPerHourVeryLarge = 5000m,
                AircraftDepreciationRate = 0.05m,
                InsuranceRatePercentage = 0.0075m,
                PilotBaseSalary = 5000m,
                PilotFlightHoursPerDay = 8m,
                CrewCostMultiplier = 1.3m,
                CateringCostPerPassenger = 8m,
                ServiceCostGroundHandling = 600m,
                LandingFeeSmall = 50m,
                LandingFeeMedium = 150m,
                LandingFeeMediumLarge = 350m,
                LandingFeeLarge = 750m,
                LandingFeeVeryLarge = 1500m,
                RatePerPaxPerNMSmall = 0.20m,
                RatePerPaxPerNMMedium = 0.15m,
                RatePerPaxPerNMMediumLarge = 0.12m,
                RatePerPaxPerNMLarge = 0.10m,
                RatePerPaxPerNMVeryLarge = 0.08m,
                FBOCostFactor = 0.05m,
                RoutesPerFBOPairLimit = 2
            };
        }
    }
}
