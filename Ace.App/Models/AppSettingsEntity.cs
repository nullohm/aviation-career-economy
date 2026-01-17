using System;

namespace Ace.App.Models
{
    public class AppSettingsEntity
    {
        public int Id { get; set; }
        public bool IsSimConnectEnabled { get; set; } = true;
        public bool AutoStartTracking { get; set; } = false;
        public double WindowTop { get; set; } = 100;
        public double WindowLeft { get; set; } = 100;
        public double WindowWidth { get; set; } = 1400;
        public double WindowHeight { get; set; } = 1000;
        public bool IsMaximized { get; set; } = false;
        public decimal RatePerPaxPerNMSmall { get; set; } = 2.00m;
        public decimal RatePerPaxPerNMMedium { get; set; } = 1.00m;
        public decimal RatePerPaxPerNMMediumLarge { get; set; } = 0.60m;
        public decimal RatePerPaxPerNMLarge { get; set; } = 0.40m;
        public decimal RatePerPaxPerNMVeryLarge { get; set; } = 0.30m;
        public string LastDepartureIcao { get; set; } = string.Empty;
        public string LastArrivalIcao { get; set; } = string.Empty;
        public string LastSelectedAircraftRegistration { get; set; } = string.Empty;
        public DateTime LastModified { get; set; } = DateTime.Now;
        public DateTime? LastDailyEarningsDate { get; set; }

        public decimal FBORentLocal { get; set; } = 500m;
        public decimal FBORentRegional { get; set; } = 1500m;
        public decimal FBORentInternational { get; set; } = 5000m;
        public decimal TerminalCostSmall { get; set; } = 1000m;
        public decimal TerminalCostMedium { get; set; } = 3000m;
        public decimal TerminalCostMediumLarge { get; set; } = 5000m;
        public decimal TerminalCostLarge { get; set; } = 8000m;
        public decimal TerminalCostVeryLarge { get; set; } = 12000m;

        public decimal ServiceCostRefueling { get; set; } = 500m;
        public decimal ServiceCostHangar { get; set; } = 800m;
        public decimal ServiceCostCatering { get; set; } = 400m;
        public decimal ServiceCostGroundHandling { get; set; } = 600m;
        public decimal ServiceCostDeIcing { get; set; } = 300m;

        public decimal ServiceCostRefuelingMedium { get; set; } = 800m;
        public decimal ServiceCostHangarMedium { get; set; } = 1200m;
        public decimal ServiceCostCateringMedium { get; set; } = 600m;
        public decimal ServiceCostGroundHandlingMedium { get; set; } = 900m;
        public decimal ServiceCostDeIcingMedium { get; set; } = 500m;

        public decimal ServiceCostRefuelingMediumLarge { get; set; } = 1200m;
        public decimal ServiceCostHangarMediumLarge { get; set; } = 2000m;
        public decimal ServiceCostCateringMediumLarge { get; set; } = 1000m;
        public decimal ServiceCostGroundHandlingMediumLarge { get; set; } = 1500m;
        public decimal ServiceCostDeIcingMediumLarge { get; set; } = 800m;

        public decimal ServiceCostRefuelingLarge { get; set; } = 2000m;
        public decimal ServiceCostHangarLarge { get; set; } = 3500m;
        public decimal ServiceCostCateringLarge { get; set; } = 1800m;
        public decimal ServiceCostGroundHandlingLarge { get; set; } = 2500m;
        public decimal ServiceCostDeIcingLarge { get; set; } = 1200m;

        public decimal ServiceCostRefuelingVeryLarge { get; set; } = 3500m;
        public decimal ServiceCostHangarVeryLarge { get; set; } = 6000m;
        public decimal ServiceCostCateringVeryLarge { get; set; } = 3000m;
        public decimal ServiceCostGroundHandlingVeryLarge { get; set; } = 4000m;
        public decimal ServiceCostDeIcingVeryLarge { get; set; } = 2000m;

        public decimal FuelPricePerGallon { get; set; } = 6.0m;
        public decimal MaintenanceCostPerHourSmall { get; set; } = 40m;
        public decimal MaintenanceCostPerHourMedium { get; set; } = 200m;
        public decimal MaintenanceCostPerHourMediumLarge { get; set; } = 1000m;
        public decimal MaintenanceCostPerHourLarge { get; set; } = 2500m;
        public decimal MaintenanceCostPerHourVeryLarge { get; set; } = 5000m;

        public decimal MaintenanceCheck50Hour { get; set; } = 450m;
        public decimal MaintenanceCheck100Hour { get; set; } = 2500m;
        public decimal MaintenanceCheckAnnual { get; set; } = 3500m;
        public decimal MaintenanceCheckACheck { get; set; } = 20000m;
        public decimal MaintenanceCheckBCheck { get; set; } = 50000m;
        public decimal MaintenanceCheckCCheck { get; set; } = 300000m;
        public decimal MaintenanceCheckDCheck { get; set; } = 3500000m;

        public decimal AircraftDepreciationRate { get; set; } = 0.05m;
        public decimal InsuranceRatePercentage { get; set; } = 0.0075m;

        public decimal PilotBaseSalary { get; set; } = 5000m;
        public decimal PilotFlightHoursPerDay { get; set; } = 8m;

        public decimal PilotRankJuniorHours { get; set; } = 0m;
        public decimal PilotRankSeniorHours { get; set; } = 500m;
        public decimal PilotRankCaptainHours { get; set; } = 1500m;
        public decimal PilotRankSeniorCaptainHours { get; set; } = 3000m;
        public decimal PilotRankChiefPilotHours { get; set; } = 5000m;

        public decimal PilotRankJuniorBonus { get; set; } = 0m;
        public decimal PilotRankSeniorBonus { get; set; } = 0.15m;
        public decimal PilotRankCaptainBonus { get; set; } = 0.30m;
        public decimal PilotRankSeniorCaptainBonus { get; set; } = 0.50m;
        public decimal PilotRankChiefPilotBonus { get; set; } = 0.75m;

        public decimal LandingFeeSmall { get; set; } = 50m;
        public decimal LandingFeeMedium { get; set; } = 150m;
        public decimal LandingFeeMediumLarge { get; set; } = 350m;
        public decimal LandingFeeLarge { get; set; } = 750m;
        public decimal LandingFeeVeryLarge { get; set; } = 1500m;

        public decimal CrewCostMultiplier { get; set; } = 1.3m;
        public decimal CateringCostPerPassenger { get; set; } = 8m;

        public decimal FBOCostFactor { get; set; } = 0.05m;

        public decimal PlayerFlightBonusPercent { get; set; } = 100m;
        public decimal NetworkBonusPercent { get; set; } = 20m;
        public decimal ServiceBonusFactorPercent { get; set; } = 5m;

        public DateTime? LastFBOBillingDate { get; set; }

        public decimal CargoRatePerKgPerNMSmall { get; set; } = 0.015m;
        public decimal CargoRatePerKgPerNMMedium { get; set; } = 0.012m;
        public decimal CargoRatePerKgPerNMMediumLarge { get; set; } = 0.010m;
        public decimal CargoRatePerKgPerNMLarge { get; set; } = 0.008m;
        public decimal CargoRatePerKgPerNMVeryLarge { get; set; } = 0.006m;

        public string Theme { get; set; } = "Dark";
        public string MapStyle { get; set; } = "Auto";

        public int RouteSlotLimitLocal { get; set; } = 2;
        public int RouteSlotLimitRegional { get; set; } = 5;
        public int RouteSlotLimitInternational { get; set; } = 10;
        public int RoutesPerFBOPairLimit { get; set; } = 1;

        public decimal AchievementRewardMultiplier { get; set; } = 1.0m;

        public bool AllowAllAircraftForFlightPlan { get; set; } = false;

        public decimal ROIPercentSmall { get; set; } = 15m;
        public decimal ROIPercentMedium { get; set; } = 15m;
        public decimal ROIPercentMediumLarge { get; set; } = 15m;
        public decimal ROIPercentLarge { get; set; } = 15m;
        public decimal ROIPercentVeryLarge { get; set; } = 15m;
        public decimal OldtimerROIMalusPercent { get; set; } = 50m;

        public decimal TypeRatingCostSmall { get; set; } = 2000m;
        public decimal TypeRatingCostMedium { get; set; } = 5000m;
        public decimal TypeRatingCostMediumLarge { get; set; } = 15000m;
        public decimal TypeRatingCostLarge { get; set; } = 35000m;
        public decimal TypeRatingCostVeryLarge { get; set; } = 50000m;

        public bool SoundEnabled { get; set; } = true;
        public double SoundVolume { get; set; } = 0.5;
        public bool SoundFlightCompletedEnabled { get; set; } = true;
        public bool SoundAchievementEnabled { get; set; } = true;
        public bool SoundTopOfDescentEnabled { get; set; } = true;
        public bool SoundWarningEnabled { get; set; } = true;
        public bool SoundNotificationEnabled { get; set; } = true;
        public bool SoundButtonClickEnabled { get; set; } = false;

        public bool ShowAirspaceOverlay { get; set; } = false;
        public bool ShowAirports { get; set; } = true;

        public string LittleNavmapDatabasePath { get; set; } = string.Empty;

        public bool ShowAirspaceClassA { get; set; } = true;
        public bool ShowAirspaceClassB { get; set; } = true;
        public bool ShowAirspaceClassC { get; set; } = true;
        public bool ShowAirspaceClassD { get; set; } = true;
        public bool ShowAirspaceClassE { get; set; } = true;
        public bool ShowAirspaceCTR { get; set; } = true;
        public bool ShowAirspaceRestricted { get; set; } = true;
        public bool ShowAirspaceProhibited { get; set; } = true;
        public bool ShowAirspaceDanger { get; set; } = true;
        public bool ShowAirspaceGlider { get; set; } = false;
        public bool ShowAirspaceOther { get; set; } = false;

        public double MapCenterLatitude { get; set; } = 50.0;
        public double MapCenterLongitude { get; set; } = 10.0;
        public double MapZoomLevel { get; set; } = 6.0;
        public bool MapLegendExpanded { get; set; } = true;

        public decimal PassengerLoadFactorPercent { get; set; } = 100m;
        public decimal CargoLoadFactorPercent { get; set; } = 100m;
    }
}
