using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Services
{
    public class AppSettings
    {
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

        // Maintenance Check Costs
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

        public decimal CargoRatePerKgPerNMSmall { get; set; } = 0.015m;
        public decimal CargoRatePerKgPerNMMedium { get; set; } = 0.012m;
        public decimal CargoRatePerKgPerNMMediumLarge { get; set; } = 0.010m;
        public decimal CargoRatePerKgPerNMLarge { get; set; } = 0.008m;
        public decimal CargoRatePerKgPerNMVeryLarge { get; set; } = 0.006m;

        public string Theme { get; set; } = "Dark";
        public string MapStyle { get; set; } = "Auto";
        public string MapLayer { get; set; } = "Street";

        public int RouteSlotLimitLocal { get; set; } = 2;
        public int RouteSlotLimitRegional { get; set; } = 5;
        public int RouteSlotLimitInternational { get; set; } = 10;
        public int RoutesPerFBOPairLimit { get; set; } = 2;

        public decimal AchievementRewardMultiplier { get; set; } = 1.0m;

        public bool AllowAllAircraftForFlightPlan { get; set; } = false;

        public bool EnforceCrewRequirement { get; set; } = false;
        public bool EnableMultiCrewShifts { get; set; } = false;

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

    public class SettingsService : ISettingsService
    {
        private readonly ILoggingService _loggingService;
        private readonly string _jsonFilePath;

        public AppSettings CurrentSettings { get; private set; } = new AppSettings();

        public SettingsService(ILoggingService loggingService)
        {
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));

            var folder = AppDomain.CurrentDomain.BaseDirectory;
            _jsonFilePath = Path.Combine(folder, "settings.json");
            Load();
        }

        public void Load()
        {
            try
            {
                using var db = new AceDbContext();


                try
                {
                    db.Database.EnsureCreated();
                }
                catch
                {
                    // Ignore - table might already exist
                }

                // Always run migrations to ensure schema is up to date
                EnsureSettingsTableExists(db);

                AppSettingsEntity? settingsEntity = null;
                try
                {
                    settingsEntity = db.Settings.FirstOrDefault();
                }
                catch
                {
                    settingsEntity = db.Settings.FirstOrDefault();
                }

                if (settingsEntity != null)
                {

                    CurrentSettings = new AppSettings
                    {
                        IsSimConnectEnabled = settingsEntity.IsSimConnectEnabled,
                        AutoStartTracking = settingsEntity.AutoStartTracking,
                        WindowTop = settingsEntity.WindowTop,
                        WindowLeft = settingsEntity.WindowLeft,
                        WindowWidth = settingsEntity.WindowWidth,
                        WindowHeight = settingsEntity.WindowHeight,
                        IsMaximized = settingsEntity.IsMaximized,
                        RatePerPaxPerNMSmall = settingsEntity.RatePerPaxPerNMSmall,
                        RatePerPaxPerNMMedium = settingsEntity.RatePerPaxPerNMMedium,
                        RatePerPaxPerNMMediumLarge = settingsEntity.RatePerPaxPerNMMediumLarge,
                        RatePerPaxPerNMLarge = settingsEntity.RatePerPaxPerNMLarge,
                        RatePerPaxPerNMVeryLarge = settingsEntity.RatePerPaxPerNMVeryLarge,
                        LastDepartureIcao = settingsEntity.LastDepartureIcao,
                        LastArrivalIcao = settingsEntity.LastArrivalIcao,
                        LastSelectedAircraftRegistration = settingsEntity.LastSelectedAircraftRegistration,
                        FBORentLocal = settingsEntity.FBORentLocal,
                        FBORentRegional = settingsEntity.FBORentRegional,
                        FBORentInternational = settingsEntity.FBORentInternational,
                        TerminalCostSmall = settingsEntity.TerminalCostSmall,
                        TerminalCostMedium = settingsEntity.TerminalCostMedium,
                        TerminalCostMediumLarge = settingsEntity.TerminalCostMediumLarge,
                        TerminalCostLarge = settingsEntity.TerminalCostLarge,
                        TerminalCostVeryLarge = settingsEntity.TerminalCostVeryLarge,
                        ServiceCostRefueling = settingsEntity.ServiceCostRefueling,
                        ServiceCostHangar = settingsEntity.ServiceCostHangar,
                        ServiceCostCatering = settingsEntity.ServiceCostCatering,
                        ServiceCostGroundHandling = settingsEntity.ServiceCostGroundHandling,
                        ServiceCostDeIcing = settingsEntity.ServiceCostDeIcing,
                        ServiceCostRefuelingMedium = settingsEntity.ServiceCostRefuelingMedium,
                        ServiceCostHangarMedium = settingsEntity.ServiceCostHangarMedium,
                        ServiceCostCateringMedium = settingsEntity.ServiceCostCateringMedium,
                        ServiceCostGroundHandlingMedium = settingsEntity.ServiceCostGroundHandlingMedium,
                        ServiceCostDeIcingMedium = settingsEntity.ServiceCostDeIcingMedium,
                        ServiceCostRefuelingMediumLarge = settingsEntity.ServiceCostRefuelingMediumLarge,
                        ServiceCostHangarMediumLarge = settingsEntity.ServiceCostHangarMediumLarge,
                        ServiceCostCateringMediumLarge = settingsEntity.ServiceCostCateringMediumLarge,
                        ServiceCostGroundHandlingMediumLarge = settingsEntity.ServiceCostGroundHandlingMediumLarge,
                        ServiceCostDeIcingMediumLarge = settingsEntity.ServiceCostDeIcingMediumLarge,
                        ServiceCostRefuelingLarge = settingsEntity.ServiceCostRefuelingLarge,
                        ServiceCostHangarLarge = settingsEntity.ServiceCostHangarLarge,
                        ServiceCostCateringLarge = settingsEntity.ServiceCostCateringLarge,
                        ServiceCostGroundHandlingLarge = settingsEntity.ServiceCostGroundHandlingLarge,
                        ServiceCostDeIcingLarge = settingsEntity.ServiceCostDeIcingLarge,
                        ServiceCostRefuelingVeryLarge = settingsEntity.ServiceCostRefuelingVeryLarge,
                        ServiceCostHangarVeryLarge = settingsEntity.ServiceCostHangarVeryLarge,
                        ServiceCostCateringVeryLarge = settingsEntity.ServiceCostCateringVeryLarge,
                        ServiceCostGroundHandlingVeryLarge = settingsEntity.ServiceCostGroundHandlingVeryLarge,
                        ServiceCostDeIcingVeryLarge = settingsEntity.ServiceCostDeIcingVeryLarge,
                        FuelPricePerGallon = settingsEntity.FuelPricePerGallon,
                        MaintenanceCostPerHourSmall = settingsEntity.MaintenanceCostPerHourSmall,
                        MaintenanceCostPerHourMedium = settingsEntity.MaintenanceCostPerHourMedium,
                        MaintenanceCostPerHourMediumLarge = settingsEntity.MaintenanceCostPerHourMediumLarge,
                        MaintenanceCostPerHourLarge = settingsEntity.MaintenanceCostPerHourLarge,
                        MaintenanceCostPerHourVeryLarge = settingsEntity.MaintenanceCostPerHourVeryLarge,
                        MaintenanceCheck50Hour = settingsEntity.MaintenanceCheck50Hour,
                        MaintenanceCheck100Hour = settingsEntity.MaintenanceCheck100Hour,
                        MaintenanceCheckAnnual = settingsEntity.MaintenanceCheckAnnual,
                        MaintenanceCheckACheck = settingsEntity.MaintenanceCheckACheck,
                        MaintenanceCheckBCheck = settingsEntity.MaintenanceCheckBCheck,
                        MaintenanceCheckCCheck = settingsEntity.MaintenanceCheckCCheck,
                        MaintenanceCheckDCheck = settingsEntity.MaintenanceCheckDCheck,
                        AircraftDepreciationRate = settingsEntity.AircraftDepreciationRate,
                        InsuranceRatePercentage = settingsEntity.InsuranceRatePercentage,
                        PilotBaseSalary = settingsEntity.PilotBaseSalary,
                        PilotFlightHoursPerDay = settingsEntity.PilotFlightHoursPerDay,
                        PilotRankJuniorHours = settingsEntity.PilotRankJuniorHours,
                        PilotRankSeniorHours = settingsEntity.PilotRankSeniorHours,
                        PilotRankCaptainHours = settingsEntity.PilotRankCaptainHours,
                        PilotRankSeniorCaptainHours = settingsEntity.PilotRankSeniorCaptainHours,
                        PilotRankChiefPilotHours = settingsEntity.PilotRankChiefPilotHours,
                        PilotRankJuniorBonus = settingsEntity.PilotRankJuniorBonus,
                        PilotRankSeniorBonus = settingsEntity.PilotRankSeniorBonus,
                        PilotRankCaptainBonus = settingsEntity.PilotRankCaptainBonus,
                        PilotRankSeniorCaptainBonus = settingsEntity.PilotRankSeniorCaptainBonus,
                        PilotRankChiefPilotBonus = settingsEntity.PilotRankChiefPilotBonus,
                        LandingFeeSmall = settingsEntity.LandingFeeSmall,
                        LandingFeeMedium = settingsEntity.LandingFeeMedium,
                        LandingFeeMediumLarge = settingsEntity.LandingFeeMediumLarge,
                        LandingFeeLarge = settingsEntity.LandingFeeLarge,
                        LandingFeeVeryLarge = settingsEntity.LandingFeeVeryLarge,
                        CrewCostMultiplier = settingsEntity.CrewCostMultiplier,
                        CateringCostPerPassenger = settingsEntity.CateringCostPerPassenger,
                        FBOCostFactor = settingsEntity.FBOCostFactor,
                        PlayerFlightBonusPercent = settingsEntity.PlayerFlightBonusPercent,
                        NetworkBonusPercent = settingsEntity.NetworkBonusPercent,
                        ServiceBonusFactorPercent = settingsEntity.ServiceBonusFactorPercent,
                        CargoRatePerKgPerNMSmall = settingsEntity.CargoRatePerKgPerNMSmall,
                        CargoRatePerKgPerNMMedium = settingsEntity.CargoRatePerKgPerNMMedium,
                        CargoRatePerKgPerNMMediumLarge = settingsEntity.CargoRatePerKgPerNMMediumLarge,
                        CargoRatePerKgPerNMLarge = settingsEntity.CargoRatePerKgPerNMLarge,
                        CargoRatePerKgPerNMVeryLarge = settingsEntity.CargoRatePerKgPerNMVeryLarge,
                        Theme = settingsEntity.Theme,
                        MapStyle = settingsEntity.MapStyle,
                        RouteSlotLimitLocal = settingsEntity.RouteSlotLimitLocal,
                        RouteSlotLimitRegional = settingsEntity.RouteSlotLimitRegional,
                        RouteSlotLimitInternational = settingsEntity.RouteSlotLimitInternational,
                        RoutesPerFBOPairLimit = settingsEntity.RoutesPerFBOPairLimit,
                        AchievementRewardMultiplier = settingsEntity.AchievementRewardMultiplier,
                        AllowAllAircraftForFlightPlan = settingsEntity.AllowAllAircraftForFlightPlan,
                        EnforceCrewRequirement = settingsEntity.EnforceCrewRequirement,
                        EnableMultiCrewShifts = settingsEntity.EnableMultiCrewShifts,
                        ROIPercentSmall = settingsEntity.ROIPercentSmall,
                        ROIPercentMedium = settingsEntity.ROIPercentMedium,
                        ROIPercentMediumLarge = settingsEntity.ROIPercentMediumLarge,
                        ROIPercentLarge = settingsEntity.ROIPercentLarge,
                        ROIPercentVeryLarge = settingsEntity.ROIPercentVeryLarge,
                        OldtimerROIMalusPercent = settingsEntity.OldtimerROIMalusPercent,
                        TypeRatingCostSmall = settingsEntity.TypeRatingCostSmall,
                        TypeRatingCostMedium = settingsEntity.TypeRatingCostMedium,
                        TypeRatingCostMediumLarge = settingsEntity.TypeRatingCostMediumLarge,
                        TypeRatingCostLarge = settingsEntity.TypeRatingCostLarge,
                        TypeRatingCostVeryLarge = settingsEntity.TypeRatingCostVeryLarge,
                        SoundEnabled = settingsEntity.SoundEnabled,
                        SoundVolume = settingsEntity.SoundVolume,
                        ShowAirspaceOverlay = settingsEntity.ShowAirspaceOverlay,
                        ShowAirports = settingsEntity.ShowAirports,
                        LittleNavmapDatabasePath = settingsEntity.LittleNavmapDatabasePath,
                        ShowAirspaceClassA = settingsEntity.ShowAirspaceClassA,
                        ShowAirspaceClassB = settingsEntity.ShowAirspaceClassB,
                        ShowAirspaceClassC = settingsEntity.ShowAirspaceClassC,
                        ShowAirspaceClassD = settingsEntity.ShowAirspaceClassD,
                        ShowAirspaceClassE = settingsEntity.ShowAirspaceClassE,
                        ShowAirspaceCTR = settingsEntity.ShowAirspaceCTR,
                        ShowAirspaceRestricted = settingsEntity.ShowAirspaceRestricted,
                        ShowAirspaceProhibited = settingsEntity.ShowAirspaceProhibited,
                        ShowAirspaceDanger = settingsEntity.ShowAirspaceDanger,
                        ShowAirspaceGlider = settingsEntity.ShowAirspaceGlider,
                        ShowAirspaceOther = settingsEntity.ShowAirspaceOther,
                        MapCenterLatitude = settingsEntity.MapCenterLatitude,
                        MapCenterLongitude = settingsEntity.MapCenterLongitude,
                        MapZoomLevel = settingsEntity.MapZoomLevel,
                        MapLegendExpanded = settingsEntity.MapLegendExpanded,
                        PassengerLoadFactorPercent = settingsEntity.PassengerLoadFactorPercent,
                        CargoLoadFactorPercent = settingsEntity.CargoLoadFactorPercent
                    };

                    _loggingService.Info("Settings loaded from database");
                }
                else
                {

                    if (File.Exists(_jsonFilePath))
                    {
                        _loggingService.Info("Migrating settings from JSON to database");
                        var json = File.ReadAllText(_jsonFilePath);
                        CurrentSettings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                        Save();


                        try
                        {
                            File.Delete(_jsonFilePath);
                            _loggingService.Info("Deleted old settings.json file after migration");
                        }
                        catch (Exception ex)
                        {
                            _loggingService.Warn($"SettingsService: Could not delete old settings.json file: {ex.Message}");
                        }
                    }
                    else
                    {

                        CurrentSettings = new AppSettings();
                        Save();
                        _loggingService.Info("Created default settings in database");
                    }
                }
            }
            catch (Exception ex)
            {
                _loggingService.Error("Failed to load settings", ex);
                CurrentSettings = new AppSettings();
            }
        }

        public void Save()
        {
            try
            {
                using var db = new AceDbContext();
                db.Database.EnsureCreated();

                var settingsEntity = db.Settings.FirstOrDefault();

                if (settingsEntity == null)
                {

                    settingsEntity = new AppSettingsEntity();
                    db.Settings.Add(settingsEntity);
                }


                settingsEntity.IsSimConnectEnabled = CurrentSettings.IsSimConnectEnabled;
                settingsEntity.AutoStartTracking = CurrentSettings.AutoStartTracking;
                settingsEntity.WindowTop = CurrentSettings.WindowTop;
                settingsEntity.WindowLeft = CurrentSettings.WindowLeft;
                settingsEntity.WindowWidth = CurrentSettings.WindowWidth;
                settingsEntity.WindowHeight = CurrentSettings.WindowHeight;
                settingsEntity.IsMaximized = CurrentSettings.IsMaximized;
                settingsEntity.RatePerPaxPerNMSmall = CurrentSettings.RatePerPaxPerNMSmall;
                settingsEntity.RatePerPaxPerNMMedium = CurrentSettings.RatePerPaxPerNMMedium;
                settingsEntity.RatePerPaxPerNMMediumLarge = CurrentSettings.RatePerPaxPerNMMediumLarge;
                settingsEntity.RatePerPaxPerNMLarge = CurrentSettings.RatePerPaxPerNMLarge;
                settingsEntity.RatePerPaxPerNMVeryLarge = CurrentSettings.RatePerPaxPerNMVeryLarge;
                settingsEntity.LastDepartureIcao = CurrentSettings.LastDepartureIcao;
                settingsEntity.LastArrivalIcao = CurrentSettings.LastArrivalIcao;
                settingsEntity.LastSelectedAircraftRegistration = CurrentSettings.LastSelectedAircraftRegistration;
                settingsEntity.FBORentLocal = CurrentSettings.FBORentLocal;
                settingsEntity.FBORentRegional = CurrentSettings.FBORentRegional;
                settingsEntity.FBORentInternational = CurrentSettings.FBORentInternational;
                settingsEntity.TerminalCostSmall = CurrentSettings.TerminalCostSmall;
                settingsEntity.TerminalCostMedium = CurrentSettings.TerminalCostMedium;
                settingsEntity.TerminalCostMediumLarge = CurrentSettings.TerminalCostMediumLarge;
                settingsEntity.TerminalCostLarge = CurrentSettings.TerminalCostLarge;
                settingsEntity.TerminalCostVeryLarge = CurrentSettings.TerminalCostVeryLarge;
                settingsEntity.ServiceCostRefueling = CurrentSettings.ServiceCostRefueling;
                settingsEntity.ServiceCostHangar = CurrentSettings.ServiceCostHangar;
                settingsEntity.ServiceCostCatering = CurrentSettings.ServiceCostCatering;
                settingsEntity.ServiceCostGroundHandling = CurrentSettings.ServiceCostGroundHandling;
                settingsEntity.ServiceCostDeIcing = CurrentSettings.ServiceCostDeIcing;
                settingsEntity.ServiceCostRefuelingMedium = CurrentSettings.ServiceCostRefuelingMedium;
                settingsEntity.ServiceCostHangarMedium = CurrentSettings.ServiceCostHangarMedium;
                settingsEntity.ServiceCostCateringMedium = CurrentSettings.ServiceCostCateringMedium;
                settingsEntity.ServiceCostGroundHandlingMedium = CurrentSettings.ServiceCostGroundHandlingMedium;
                settingsEntity.ServiceCostDeIcingMedium = CurrentSettings.ServiceCostDeIcingMedium;
                settingsEntity.ServiceCostRefuelingMediumLarge = CurrentSettings.ServiceCostRefuelingMediumLarge;
                settingsEntity.ServiceCostHangarMediumLarge = CurrentSettings.ServiceCostHangarMediumLarge;
                settingsEntity.ServiceCostCateringMediumLarge = CurrentSettings.ServiceCostCateringMediumLarge;
                settingsEntity.ServiceCostGroundHandlingMediumLarge = CurrentSettings.ServiceCostGroundHandlingMediumLarge;
                settingsEntity.ServiceCostDeIcingMediumLarge = CurrentSettings.ServiceCostDeIcingMediumLarge;
                settingsEntity.ServiceCostRefuelingLarge = CurrentSettings.ServiceCostRefuelingLarge;
                settingsEntity.ServiceCostHangarLarge = CurrentSettings.ServiceCostHangarLarge;
                settingsEntity.ServiceCostCateringLarge = CurrentSettings.ServiceCostCateringLarge;
                settingsEntity.ServiceCostGroundHandlingLarge = CurrentSettings.ServiceCostGroundHandlingLarge;
                settingsEntity.ServiceCostDeIcingLarge = CurrentSettings.ServiceCostDeIcingLarge;
                settingsEntity.ServiceCostRefuelingVeryLarge = CurrentSettings.ServiceCostRefuelingVeryLarge;
                settingsEntity.ServiceCostHangarVeryLarge = CurrentSettings.ServiceCostHangarVeryLarge;
                settingsEntity.ServiceCostCateringVeryLarge = CurrentSettings.ServiceCostCateringVeryLarge;
                settingsEntity.ServiceCostGroundHandlingVeryLarge = CurrentSettings.ServiceCostGroundHandlingVeryLarge;
                settingsEntity.ServiceCostDeIcingVeryLarge = CurrentSettings.ServiceCostDeIcingVeryLarge;
                settingsEntity.FuelPricePerGallon = CurrentSettings.FuelPricePerGallon;
                settingsEntity.MaintenanceCostPerHourSmall = CurrentSettings.MaintenanceCostPerHourSmall;
                settingsEntity.MaintenanceCostPerHourMedium = CurrentSettings.MaintenanceCostPerHourMedium;
                settingsEntity.MaintenanceCostPerHourMediumLarge = CurrentSettings.MaintenanceCostPerHourMediumLarge;
                settingsEntity.MaintenanceCostPerHourLarge = CurrentSettings.MaintenanceCostPerHourLarge;
                settingsEntity.MaintenanceCostPerHourVeryLarge = CurrentSettings.MaintenanceCostPerHourVeryLarge;
                settingsEntity.MaintenanceCheck50Hour = CurrentSettings.MaintenanceCheck50Hour;
                settingsEntity.MaintenanceCheck100Hour = CurrentSettings.MaintenanceCheck100Hour;
                settingsEntity.MaintenanceCheckAnnual = CurrentSettings.MaintenanceCheckAnnual;
                settingsEntity.MaintenanceCheckACheck = CurrentSettings.MaintenanceCheckACheck;
                settingsEntity.MaintenanceCheckBCheck = CurrentSettings.MaintenanceCheckBCheck;
                settingsEntity.MaintenanceCheckCCheck = CurrentSettings.MaintenanceCheckCCheck;
                settingsEntity.MaintenanceCheckDCheck = CurrentSettings.MaintenanceCheckDCheck;
                settingsEntity.AircraftDepreciationRate = CurrentSettings.AircraftDepreciationRate;
                settingsEntity.InsuranceRatePercentage = CurrentSettings.InsuranceRatePercentage;
                settingsEntity.PilotBaseSalary = CurrentSettings.PilotBaseSalary;
                settingsEntity.PilotFlightHoursPerDay = CurrentSettings.PilotFlightHoursPerDay;
                settingsEntity.PilotRankJuniorHours = CurrentSettings.PilotRankJuniorHours;
                settingsEntity.PilotRankSeniorHours = CurrentSettings.PilotRankSeniorHours;
                settingsEntity.PilotRankCaptainHours = CurrentSettings.PilotRankCaptainHours;
                settingsEntity.PilotRankSeniorCaptainHours = CurrentSettings.PilotRankSeniorCaptainHours;
                settingsEntity.PilotRankChiefPilotHours = CurrentSettings.PilotRankChiefPilotHours;
                settingsEntity.PilotRankJuniorBonus = CurrentSettings.PilotRankJuniorBonus;
                settingsEntity.PilotRankSeniorBonus = CurrentSettings.PilotRankSeniorBonus;
                settingsEntity.PilotRankCaptainBonus = CurrentSettings.PilotRankCaptainBonus;
                settingsEntity.PilotRankSeniorCaptainBonus = CurrentSettings.PilotRankSeniorCaptainBonus;
                settingsEntity.PilotRankChiefPilotBonus = CurrentSettings.PilotRankChiefPilotBonus;
                settingsEntity.LandingFeeSmall = CurrentSettings.LandingFeeSmall;
                settingsEntity.LandingFeeMedium = CurrentSettings.LandingFeeMedium;
                settingsEntity.LandingFeeMediumLarge = CurrentSettings.LandingFeeMediumLarge;
                settingsEntity.LandingFeeLarge = CurrentSettings.LandingFeeLarge;
                settingsEntity.LandingFeeVeryLarge = CurrentSettings.LandingFeeVeryLarge;
                settingsEntity.CrewCostMultiplier = CurrentSettings.CrewCostMultiplier;
                settingsEntity.CateringCostPerPassenger = CurrentSettings.CateringCostPerPassenger;
                settingsEntity.FBOCostFactor = CurrentSettings.FBOCostFactor;
                settingsEntity.PlayerFlightBonusPercent = CurrentSettings.PlayerFlightBonusPercent;
                settingsEntity.NetworkBonusPercent = CurrentSettings.NetworkBonusPercent;
                settingsEntity.ServiceBonusFactorPercent = CurrentSettings.ServiceBonusFactorPercent;
                settingsEntity.CargoRatePerKgPerNMSmall = CurrentSettings.CargoRatePerKgPerNMSmall;
                settingsEntity.CargoRatePerKgPerNMMedium = CurrentSettings.CargoRatePerKgPerNMMedium;
                settingsEntity.CargoRatePerKgPerNMMediumLarge = CurrentSettings.CargoRatePerKgPerNMMediumLarge;
                settingsEntity.CargoRatePerKgPerNMLarge = CurrentSettings.CargoRatePerKgPerNMLarge;
                settingsEntity.CargoRatePerKgPerNMVeryLarge = CurrentSettings.CargoRatePerKgPerNMVeryLarge;
                settingsEntity.Theme = CurrentSettings.Theme;
                settingsEntity.MapStyle = CurrentSettings.MapStyle;
                settingsEntity.RouteSlotLimitLocal = CurrentSettings.RouteSlotLimitLocal;
                settingsEntity.RouteSlotLimitRegional = CurrentSettings.RouteSlotLimitRegional;
                settingsEntity.RouteSlotLimitInternational = CurrentSettings.RouteSlotLimitInternational;
                settingsEntity.RoutesPerFBOPairLimit = CurrentSettings.RoutesPerFBOPairLimit;
                settingsEntity.AchievementRewardMultiplier = CurrentSettings.AchievementRewardMultiplier;
                settingsEntity.AllowAllAircraftForFlightPlan = CurrentSettings.AllowAllAircraftForFlightPlan;
                settingsEntity.EnforceCrewRequirement = CurrentSettings.EnforceCrewRequirement;
                settingsEntity.EnableMultiCrewShifts = CurrentSettings.EnableMultiCrewShifts;
                settingsEntity.ROIPercentSmall = CurrentSettings.ROIPercentSmall;
                settingsEntity.ROIPercentMedium = CurrentSettings.ROIPercentMedium;
                settingsEntity.ROIPercentMediumLarge = CurrentSettings.ROIPercentMediumLarge;
                settingsEntity.ROIPercentLarge = CurrentSettings.ROIPercentLarge;
                settingsEntity.ROIPercentVeryLarge = CurrentSettings.ROIPercentVeryLarge;
                settingsEntity.OldtimerROIMalusPercent = CurrentSettings.OldtimerROIMalusPercent;
                settingsEntity.TypeRatingCostSmall = CurrentSettings.TypeRatingCostSmall;
                settingsEntity.TypeRatingCostMedium = CurrentSettings.TypeRatingCostMedium;
                settingsEntity.TypeRatingCostMediumLarge = CurrentSettings.TypeRatingCostMediumLarge;
                settingsEntity.TypeRatingCostLarge = CurrentSettings.TypeRatingCostLarge;
                settingsEntity.TypeRatingCostVeryLarge = CurrentSettings.TypeRatingCostVeryLarge;
                settingsEntity.SoundEnabled = CurrentSettings.SoundEnabled;
                settingsEntity.SoundVolume = CurrentSettings.SoundVolume;
                settingsEntity.ShowAirspaceOverlay = CurrentSettings.ShowAirspaceOverlay;
                settingsEntity.ShowAirports = CurrentSettings.ShowAirports;
                settingsEntity.LittleNavmapDatabasePath = CurrentSettings.LittleNavmapDatabasePath;
                settingsEntity.ShowAirspaceClassA = CurrentSettings.ShowAirspaceClassA;
                settingsEntity.ShowAirspaceClassB = CurrentSettings.ShowAirspaceClassB;
                settingsEntity.ShowAirspaceClassC = CurrentSettings.ShowAirspaceClassC;
                settingsEntity.ShowAirspaceClassD = CurrentSettings.ShowAirspaceClassD;
                settingsEntity.ShowAirspaceClassE = CurrentSettings.ShowAirspaceClassE;
                settingsEntity.ShowAirspaceCTR = CurrentSettings.ShowAirspaceCTR;
                settingsEntity.ShowAirspaceRestricted = CurrentSettings.ShowAirspaceRestricted;
                settingsEntity.ShowAirspaceProhibited = CurrentSettings.ShowAirspaceProhibited;
                settingsEntity.ShowAirspaceDanger = CurrentSettings.ShowAirspaceDanger;
                settingsEntity.ShowAirspaceGlider = CurrentSettings.ShowAirspaceGlider;
                settingsEntity.ShowAirspaceOther = CurrentSettings.ShowAirspaceOther;
                settingsEntity.MapCenterLatitude = CurrentSettings.MapCenterLatitude;
                settingsEntity.MapCenterLongitude = CurrentSettings.MapCenterLongitude;
                settingsEntity.MapZoomLevel = CurrentSettings.MapZoomLevel;
                settingsEntity.MapLegendExpanded = CurrentSettings.MapLegendExpanded;
                settingsEntity.PassengerLoadFactorPercent = CurrentSettings.PassengerLoadFactorPercent;
                settingsEntity.CargoLoadFactorPercent = CurrentSettings.CargoLoadFactorPercent;
                settingsEntity.LastModified = DateTime.Now;

                db.SaveChanges();
                _loggingService.Info("Settings saved to database");
            }
            catch (Exception ex)
            {
                _loggingService.Error("Failed to save settings to database", ex);
            }
        }

        private void EnsureSettingsTableExists(AceDbContext db)
        {
            try
            {
                var connection = db.Database.GetDbConnection();
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Settings (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        IsSimConnectEnabled INTEGER NOT NULL DEFAULT 1,
                        AutoStartTracking INTEGER NOT NULL DEFAULT 0,
                        WindowTop REAL NOT NULL DEFAULT 100,
                        WindowLeft REAL NOT NULL DEFAULT 100,
                        WindowWidth REAL NOT NULL DEFAULT 1000,
                        WindowHeight REAL NOT NULL DEFAULT 600,
                        IsMaximized INTEGER NOT NULL DEFAULT 0,
                        RatePerPaxPerNM REAL NOT NULL DEFAULT 0.15,
                        LastDepartureIcao TEXT NOT NULL DEFAULT '',
                        LastArrivalIcao TEXT NOT NULL DEFAULT '',
                        LastModified TEXT NOT NULL
                    )";
                command.ExecuteNonQuery();

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='RatePerPaxPerNM'";
                var hasRateColumn = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasRateColumn)
                {
                    _loggingService.Info("Adding RatePerPaxPerNM column to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN RatePerPaxPerNM REAL NOT NULL DEFAULT 0.15";
                    command.ExecuteNonQuery();
                    _loggingService.Info("RatePerPaxPerNM column added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='LastDepartureIcao'";
                var hasDepartureColumn = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasDepartureColumn)
                {
                    _loggingService.Info("Adding LastDepartureIcao and LastArrivalIcao columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN LastDepartureIcao TEXT NOT NULL DEFAULT ''";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN LastArrivalIcao TEXT NOT NULL DEFAULT ''";
                    command.ExecuteNonQuery();
                    _loggingService.Info("Settings table columns added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='FBORentLocal'";
                var hasFBORentColumn = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasFBORentColumn)
                {
                    _loggingService.Info("Adding FBO rent and service cost columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN FBORentLocal REAL NOT NULL DEFAULT 500";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN FBORentRegional REAL NOT NULL DEFAULT 1500";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN FBORentInternational REAL NOT NULL DEFAULT 5000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN TerminalCostSmall REAL NOT NULL DEFAULT 1000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN TerminalCostMedium REAL NOT NULL DEFAULT 3000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN TerminalCostLarge REAL NOT NULL DEFAULT 8000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostPerService REAL NOT NULL DEFAULT 500";
                    command.ExecuteNonQuery();
                    _loggingService.Info("FBO rent columns added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='FuelPricePerGallon'";
                var hasGameplayColumns = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasGameplayColumns)
                {
                    _loggingService.Info("Adding gameplay customization columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN FuelPricePerGallon REAL NOT NULL DEFAULT 6.0";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN MaintenanceIntervalHours REAL NOT NULL DEFAULT 100";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN MaintenanceCostPerHour REAL NOT NULL DEFAULT 50";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN PilotTrainingCost REAL NOT NULL DEFAULT 5000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN AircraftDepreciationRate REAL NOT NULL DEFAULT 0.05";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN InsuranceCostPerAircraftMonth REAL NOT NULL DEFAULT 200";
                    command.ExecuteNonQuery();
                    _loggingService.Info("Gameplay customization columns added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='PilotBaseSalary'";
                var hasPilotBaseSalary = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasPilotBaseSalary)
                {
                    _loggingService.Info("Adding pilot base salary column to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN PilotBaseSalary REAL NOT NULL DEFAULT 5000";
                    command.ExecuteNonQuery();
                    _loggingService.Info("Pilot base salary column added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='InsuranceRatePercentage'";
                var hasInsuranceRatePercentage = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasInsuranceRatePercentage)
                {
                    _loggingService.Info("Migrating insurance from fixed cost to percentage");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN InsuranceRatePercentage REAL NOT NULL DEFAULT 0.0075";
                    command.ExecuteNonQuery();
                    _loggingService.Info("Insurance rate percentage column added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='PilotRankJuniorHours'";
                var hasPilotRankColumns = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasPilotRankColumns)
                {
                    _loggingService.Info("Adding pilot rank columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN PilotRankJuniorHours REAL NOT NULL DEFAULT 0";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN PilotRankSeniorHours REAL NOT NULL DEFAULT 500";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN PilotRankCaptainHours REAL NOT NULL DEFAULT 1500";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN PilotRankSeniorCaptainHours REAL NOT NULL DEFAULT 3000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN PilotRankChiefPilotHours REAL NOT NULL DEFAULT 5000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN PilotRankJuniorBonus REAL NOT NULL DEFAULT 0";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN PilotRankSeniorBonus REAL NOT NULL DEFAULT 0.15";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN PilotRankCaptainBonus REAL NOT NULL DEFAULT 0.30";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN PilotRankSeniorCaptainBonus REAL NOT NULL DEFAULT 0.50";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN PilotRankChiefPilotBonus REAL NOT NULL DEFAULT 0.75";
                    command.ExecuteNonQuery();
                    _loggingService.Info("Pilot rank columns added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='RatePerPaxPerNMSmall'";
                var hasSizeBasedRates = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasSizeBasedRates)
                {
                    _loggingService.Info("Adding size-based revenue rate columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN RatePerPaxPerNMSmall REAL NOT NULL DEFAULT 0.20";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN RatePerPaxPerNMMedium REAL NOT NULL DEFAULT 0.15";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN RatePerPaxPerNMMediumLarge REAL NOT NULL DEFAULT 0.12";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN RatePerPaxPerNMLarge REAL NOT NULL DEFAULT 0.10";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN RatePerPaxPerNMVeryLarge REAL NOT NULL DEFAULT 0.08";
                    command.ExecuteNonQuery();
                    _loggingService.Info("Size-based revenue rate columns added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='TerminalCostMediumLarge'";
                var hasTerminalSizes = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasTerminalSizes)
                {
                    _loggingService.Info("Adding terminal size columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN TerminalCostMediumLarge REAL NOT NULL DEFAULT 5000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN TerminalCostVeryLarge REAL NOT NULL DEFAULT 12000";
                    command.ExecuteNonQuery();
                    _loggingService.Info("Terminal size columns added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='ServiceCostRefueling'";
                var hasIndividualServiceCosts = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasIndividualServiceCosts)
                {
                    _loggingService.Info("Adding individual service cost columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostRefueling REAL NOT NULL DEFAULT 500";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostHangar REAL NOT NULL DEFAULT 800";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostCatering REAL NOT NULL DEFAULT 400";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostGroundHandling REAL NOT NULL DEFAULT 600";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostDeIcing REAL NOT NULL DEFAULT 300";
                    command.ExecuteNonQuery();
                    _loggingService.Info("Individual service cost columns added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='ServiceCostRefuelingMedium'";
                var hasTerminalSizeServiceCosts = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasTerminalSizeServiceCosts)
                {
                    _loggingService.Info("Adding terminal-size-based service cost columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostRefuelingMedium REAL NOT NULL DEFAULT 800";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostHangarMedium REAL NOT NULL DEFAULT 1200";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostCateringMedium REAL NOT NULL DEFAULT 600";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostGroundHandlingMedium REAL NOT NULL DEFAULT 900";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostDeIcingMedium REAL NOT NULL DEFAULT 500";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostRefuelingMediumLarge REAL NOT NULL DEFAULT 1200";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostHangarMediumLarge REAL NOT NULL DEFAULT 2000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostCateringMediumLarge REAL NOT NULL DEFAULT 1000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostGroundHandlingMediumLarge REAL NOT NULL DEFAULT 1500";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostDeIcingMediumLarge REAL NOT NULL DEFAULT 800";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostRefuelingLarge REAL NOT NULL DEFAULT 2000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostHangarLarge REAL NOT NULL DEFAULT 3500";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostCateringLarge REAL NOT NULL DEFAULT 1800";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostGroundHandlingLarge REAL NOT NULL DEFAULT 2500";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostDeIcingLarge REAL NOT NULL DEFAULT 1200";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostRefuelingVeryLarge REAL NOT NULL DEFAULT 3500";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostHangarVeryLarge REAL NOT NULL DEFAULT 6000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostCateringVeryLarge REAL NOT NULL DEFAULT 3000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostGroundHandlingVeryLarge REAL NOT NULL DEFAULT 4000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceCostDeIcingVeryLarge REAL NOT NULL DEFAULT 2000";
                    command.ExecuteNonQuery();
                    _loggingService.Info("Terminal-size-based service cost columns added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='MaintenanceCheck50Hour'";
                var hasMaintenanceChecks = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasMaintenanceChecks)
                {
                    _loggingService.Info("Adding maintenance check cost columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN MaintenanceCheck50Hour REAL NOT NULL DEFAULT 450";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN MaintenanceCheck100Hour REAL NOT NULL DEFAULT 2500";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN MaintenanceCheckAnnual REAL NOT NULL DEFAULT 3500";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN MaintenanceCheckACheck REAL NOT NULL DEFAULT 20000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN MaintenanceCheckBCheck REAL NOT NULL DEFAULT 50000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN MaintenanceCheckCCheck REAL NOT NULL DEFAULT 300000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN MaintenanceCheckDCheck REAL NOT NULL DEFAULT 3500000";
                    command.ExecuteNonQuery();
                    _loggingService.Info("Maintenance check cost columns added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='PilotFlightHoursPerDay'";
                var hasPilotFlightHoursPerDay = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasPilotFlightHoursPerDay)
                {
                    _loggingService.Info("Adding PilotFlightHoursPerDay column to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN PilotFlightHoursPerDay REAL NOT NULL DEFAULT 8";
                    command.ExecuteNonQuery();
                    _loggingService.Info("PilotFlightHoursPerDay column added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('FBOs') WHERE name='RunwayLengthFt'";
                var hasRunwayLengthFt = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasRunwayLengthFt)
                {
                    _loggingService.Info("Adding RunwayLengthFt column to FBOs table");
                    command.CommandText = "ALTER TABLE FBOs ADD COLUMN RunwayLengthFt INTEGER NOT NULL DEFAULT 0";
                    command.ExecuteNonQuery();
                    _loggingService.Info("RunwayLengthFt column added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='LandingFeeSmall'";
                var hasLandingFees = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasLandingFees)
                {
                    _loggingService.Info("Adding pricing columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN LandingFeeSmall REAL NOT NULL DEFAULT 50";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN LandingFeeMedium REAL NOT NULL DEFAULT 150";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN LandingFeeMediumLarge REAL NOT NULL DEFAULT 350";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN LandingFeeLarge REAL NOT NULL DEFAULT 750";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN LandingFeeVeryLarge REAL NOT NULL DEFAULT 1500";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN CrewCostMultiplier REAL NOT NULL DEFAULT 1.3";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN CateringCostPerPassenger REAL NOT NULL DEFAULT 8";
                    command.ExecuteNonQuery();
                    _loggingService.Info("Pricing columns added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='LastFBOBillingDate'";
                var hasLastFBOBillingDate = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasLastFBOBillingDate)
                {
                    _loggingService.Info("Adding LastFBOBillingDate column to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN LastFBOBillingDate TEXT";
                    command.ExecuteNonQuery();
                    _loggingService.Info("LastFBOBillingDate column added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='FBOCostFactor'";
                var hasFBOCostFactor = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasFBOCostFactor)
                {
                    _loggingService.Info("Adding FBOCostFactor column to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN FBOCostFactor REAL NOT NULL DEFAULT 0.05";
                    command.ExecuteNonQuery();
                    _loggingService.Info("FBOCostFactor column added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='MaintenanceCostPerHourSmall'";
                var hasMaintenanceCostPerHourSmall = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasMaintenanceCostPerHourSmall)
                {
                    _loggingService.Info("Adding size-based maintenance cost columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN MaintenanceCostPerHourSmall REAL NOT NULL DEFAULT 40";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN MaintenanceCostPerHourMedium REAL NOT NULL DEFAULT 200";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN MaintenanceCostPerHourMediumLarge REAL NOT NULL DEFAULT 1000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN MaintenanceCostPerHourLarge REAL NOT NULL DEFAULT 2500";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN MaintenanceCostPerHourVeryLarge REAL NOT NULL DEFAULT 5000";
                    command.ExecuteNonQuery();
                    _loggingService.Info("Size-based maintenance cost columns added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='PlayerFlightBonusPercent'";
                var hasPlayerFlightBonus = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasPlayerFlightBonus)
                {
                    _loggingService.Info("Adding PlayerFlightBonusPercent column to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN PlayerFlightBonusPercent REAL NOT NULL DEFAULT 100";
                    command.ExecuteNonQuery();
                    _loggingService.Info("PlayerFlightBonusPercent column added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='CargoRatePerKgPerNMSmall'";
                var hasCargoRates = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasCargoRates)
                {
                    _loggingService.Info("Adding cargo rate columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN CargoRatePerKgPerNMSmall REAL NOT NULL DEFAULT 0.015";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN CargoRatePerKgPerNMMedium REAL NOT NULL DEFAULT 0.012";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN CargoRatePerKgPerNMMediumLarge REAL NOT NULL DEFAULT 0.010";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN CargoRatePerKgPerNMLarge REAL NOT NULL DEFAULT 0.008";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN CargoRatePerKgPerNMVeryLarge REAL NOT NULL DEFAULT 0.006";
                    command.ExecuteNonQuery();
                    _loggingService.Info("Cargo rate columns added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='NetworkBonusPercent'";
                var hasNetworkBonus = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasNetworkBonus)
                {
                    _loggingService.Info("Adding NetworkBonusPercent column to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN NetworkBonusPercent REAL NOT NULL DEFAULT 20";
                    command.ExecuteNonQuery();
                    _loggingService.Info("NetworkBonusPercent column added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='ServiceBonusFactorPercent'";
                var hasServiceBonus = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasServiceBonus)
                {
                    _loggingService.Info("Adding ServiceBonusFactorPercent column to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ServiceBonusFactorPercent REAL NOT NULL DEFAULT 5";
                    command.ExecuteNonQuery();
                    _loggingService.Info("ServiceBonusFactorPercent column added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='Theme'";
                var hasTheme = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasTheme)
                {
                    _loggingService.Info("Adding Theme column to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN Theme TEXT NOT NULL DEFAULT 'Dark'";
                    command.ExecuteNonQuery();
                    _loggingService.Info("Theme column added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='MapStyle'";
                var hasMapStyle = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasMapStyle)
                {
                    _loggingService.Info("Adding MapStyle column to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN MapStyle TEXT NOT NULL DEFAULT 'Auto'";
                    command.ExecuteNonQuery();
                    _loggingService.Info("MapStyle column added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='AchievementRewardMultiplier'";
                var hasAchievementRewardMultiplier = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasAchievementRewardMultiplier)
                {
                    _loggingService.Info("Adding AchievementRewardMultiplier column to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN AchievementRewardMultiplier REAL NOT NULL DEFAULT 1.0";
                    command.ExecuteNonQuery();
                    _loggingService.Info("AchievementRewardMultiplier column added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='AllowAllAircraftForFlightPlan'";
                var hasAllowAllAircraftForFlightPlan = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasAllowAllAircraftForFlightPlan)
                {
                    _loggingService.Info("Adding AllowAllAircraftForFlightPlan column to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN AllowAllAircraftForFlightPlan INTEGER NOT NULL DEFAULT 0";
                    command.ExecuteNonQuery();
                    _loggingService.Info("AllowAllAircraftForFlightPlan column added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='ROIPercentSmall'";
                var hasROISettings = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasROISettings)
                {
                    _loggingService.Info("Adding ROI columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ROIPercentSmall REAL NOT NULL DEFAULT 15";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ROIPercentMedium REAL NOT NULL DEFAULT 15";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ROIPercentMediumLarge REAL NOT NULL DEFAULT 15";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ROIPercentLarge REAL NOT NULL DEFAULT 15";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ROIPercentVeryLarge REAL NOT NULL DEFAULT 15";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN OldtimerROIMalusPercent REAL NOT NULL DEFAULT 50";
                    command.ExecuteNonQuery();
                    _loggingService.Info("ROI columns added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='TypeRatingCostSmall'";
                var hasTypeRatingCosts = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasTypeRatingCosts)
                {
                    _loggingService.Info("Adding TypeRating cost columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN TypeRatingCostSmall REAL NOT NULL DEFAULT 2000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN TypeRatingCostMedium REAL NOT NULL DEFAULT 5000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN TypeRatingCostMediumLarge REAL NOT NULL DEFAULT 15000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN TypeRatingCostLarge REAL NOT NULL DEFAULT 35000";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN TypeRatingCostVeryLarge REAL NOT NULL DEFAULT 50000";
                    command.ExecuteNonQuery();
                    _loggingService.Info("TypeRating cost columns added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='SoundEnabled'";
                var hasSoundSettings = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasSoundSettings)
                {
                    _loggingService.Info("Adding sound settings columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN SoundEnabled INTEGER NOT NULL DEFAULT 1";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN SoundVolume REAL NOT NULL DEFAULT 0.5";
                    command.ExecuteNonQuery();
                    _loggingService.Info("Sound settings columns added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Aircraft') WHERE name='IsOldtimer'";
                var hasAircraftOldtimer = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasAircraftOldtimer)
                {
                    _loggingService.Info("Adding IsOldtimer column to Aircraft table");
                    command.CommandText = "ALTER TABLE Aircraft ADD COLUMN IsOldtimer INTEGER NOT NULL DEFAULT 0";
                    command.ExecuteNonQuery();
                    _loggingService.Info("IsOldtimer column added to Aircraft table");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('AircraftCatalog') WHERE name='IsOldtimer'";
                var hasCatalogOldtimer = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasCatalogOldtimer)
                {
                    _loggingService.Info("Adding IsOldtimer column to AircraftCatalog table");
                    command.CommandText = "ALTER TABLE AircraftCatalog ADD COLUMN IsOldtimer INTEGER NOT NULL DEFAULT 0";
                    command.ExecuteNonQuery();
                    _loggingService.Info("IsOldtimer column added to AircraftCatalog table");
                }

                SetOldtimerAircraft(command);

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='ShowAirspaceOverlay'";
                var hasShowAirspaceOverlay = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasShowAirspaceOverlay)
                {
                    _loggingService.Info("Adding ShowAirspaceOverlay column to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ShowAirspaceOverlay INTEGER NOT NULL DEFAULT 0";
                    command.ExecuteNonQuery();
                    _loggingService.Info("ShowAirspaceOverlay column added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='ShowAirports'";
                var hasShowAirports = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasShowAirports)
                {
                    _loggingService.Info("Adding ShowAirports column to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN ShowAirports INTEGER NOT NULL DEFAULT 1";
                    command.ExecuteNonQuery();
                    _loggingService.Info("ShowAirports column added successfully");
                }

                command.CommandText = @"
                    SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='LittleNavmapDatabasePath'";
                var hasLittleNavmapDatabasePath = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasLittleNavmapDatabasePath)
                {
                    _loggingService.Info("Adding LittleNavmapDatabasePath column to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN LittleNavmapDatabasePath TEXT NOT NULL DEFAULT ''";
                    command.ExecuteNonQuery();
                    _loggingService.Info("LittleNavmapDatabasePath column added successfully");
                }

                var airspaceColumns = new[]
                {
                    ("ShowAirspaceClassA", 1),
                    ("ShowAirspaceClassB", 1),
                    ("ShowAirspaceClassC", 1),
                    ("ShowAirspaceClassD", 1),
                    ("ShowAirspaceClassE", 1),
                    ("ShowAirspaceCTR", 1),
                    ("ShowAirspaceRestricted", 1),
                    ("ShowAirspaceProhibited", 1),
                    ("ShowAirspaceDanger", 1),
                    ("ShowAirspaceGlider", 0),
                    ("ShowAirspaceOther", 0)
                };

                foreach (var (columnName, defaultValue) in airspaceColumns)
                {
                    command.CommandText = $"SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='{columnName}'";
                    var hasColumn = Convert.ToInt32(command.ExecuteScalar()) > 0;

                    if (!hasColumn)
                    {
                        _loggingService.Info($"Adding {columnName} column to Settings table");
                        command.CommandText = $"ALTER TABLE Settings ADD COLUMN {columnName} INTEGER NOT NULL DEFAULT {defaultValue}";
                        command.ExecuteNonQuery();
                    }
                }

                var mapPositionColumns = new[]
                {
                    ("MapCenterLatitude", "50.0"),
                    ("MapCenterLongitude", "10.0"),
                    ("MapZoomLevel", "6.0")
                };

                foreach (var (columnName, defaultValue) in mapPositionColumns)
                {
                    command.CommandText = $"SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='{columnName}'";
                    var hasColumn = Convert.ToInt32(command.ExecuteScalar()) > 0;

                    if (!hasColumn)
                    {
                        _loggingService.Info($"Adding {columnName} column to Settings table");
                        command.CommandText = $"ALTER TABLE Settings ADD COLUMN {columnName} REAL NOT NULL DEFAULT {defaultValue}";
                        command.ExecuteNonQuery();
                    }
                }

                command.CommandText = "SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='MapLegendExpanded'";
                if (Convert.ToInt32(command.ExecuteScalar()) == 0)
                {
                    _loggingService.Info("Adding MapLegendExpanded column to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN MapLegendExpanded INTEGER NOT NULL DEFAULT 1";
                    command.ExecuteNonQuery();
                }

                command.CommandText = "SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='PassengerLoadFactorPercent'";
                if (Convert.ToInt32(command.ExecuteScalar()) == 0)
                {
                    _loggingService.Info("Adding load factor columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN PassengerLoadFactorPercent REAL NOT NULL DEFAULT 100";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN CargoLoadFactorPercent REAL NOT NULL DEFAULT 100";
                    command.ExecuteNonQuery();
                }

                command.CommandText = "SELECT COUNT(*) FROM pragma_table_info('Settings') WHERE name='EnforceCrewRequirement'";
                if (Convert.ToInt32(command.ExecuteScalar()) == 0)
                {
                    _loggingService.Info("Adding multi-crew settings columns to Settings table");
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN EnforceCrewRequirement INTEGER NOT NULL DEFAULT 0";
                    command.ExecuteNonQuery();
                    command.CommandText = "ALTER TABLE Settings ADD COLUMN EnableMultiCrewShifts INTEGER NOT NULL DEFAULT 0";
                    command.ExecuteNonQuery();
                    _loggingService.Info("Multi-crew settings columns added successfully");
                }

                connection.Close();
                _loggingService.Info("Settings table created or verified");
            }
            catch (Exception ex)
            {
                _loggingService.Error("Failed to create Settings table", ex);
            }
        }

        private void SetOldtimerAircraft(System.Data.Common.DbCommand command)
        {
            var oldtimerPatterns = new[]
            {
                "Ae-45", "Aero 45",
                "L39",
                "Optica",
                "AN-225", "An-2", "Antonov",
                "A310", "A340", "A380",
                "BelugaXL", "Beluga XL",
                "A-400M", "A400M",
                "Beech 18", "Beechcraft 18", "Beech Model 18", "Model 18",
                "Beech 17", "Beechcraft 17", "Staggerwing", "D17",
                "D18",
                "Model 50", "Model 60", "Model 76", "Model 77", "Model 95",
                "Boeing 247",
                "707", "717", "747", "757", "767",
                "Boeing 307", "Stratoliner",
                "C-17A", "Globemaster",
                "FA18", "F/A-18",
                "Cessna 140", "C140",
                "Cessna 152", "C152", "152 Aerobat", "152 -",
                "C162",
                "Cessna 170", "C170",
                "Cessna 195", "C195",
                "C303", "C320", "C337", "C340",
                "C402", "C404", "404 Titan", "c411", "C421", "C441",
                "T50",
                "CO1",
                "Curtiss", "C-46", "C-47",
                "DC-3", "Douglas DC-3",
                "DC-6", "Douglas DC-6",
                "DHC-2", "Beaver",
                "DHC-3", "Otter",
                "DHC-4", "Caribou",
                "Dornier Do-31", "Do-31",
                "Dornier Do X", "Do X", "Do-X",
                "Dornier Do-J", "Do-J",
                "Seastar",
                "ERJFamily",
                "Focke-Wulf", "Focke Wulf", "Fw 200",
                "Fokker F",
                "Ford Trimotor", "Ford 4-AT", "Ford 5-AT",
                "Gee Bee",
                "Grumman Goose", "G-21", "Grumman Albatross",
                "Hughes H-4", "Spruce Goose",
                "Junkers", "Ju 52",
                "Latecoere",
                "Lockheed 10", "Model 10 Electra",
                "Lockheed Constellation", "L-049", "L-1049",
                "MD10", "MD80", "DC-10",
                "T-6 Texan", "AT-6", "SNJ", "T6",
                "P-51", "P51", "p51d",
                "E2 ", "Hawkeye",
                "T38", "T-38",
                "J-3 Cub", "Piper Cub",
                "PA-18", "Super Cub",
                "PC-6", "Porter",
                "Pilatus P-2",
                "de Havilland Chipmunk",
                "Ryan PT-22",
                "Spirit of St Louis", "Spirit of St. Louis",
                "Stearman", "PT-17",
                "Saab 17", "Saab 340", "S340",
                "Savoia-Marchetti", "S.55",
                "SIAI", "Marchetti",
                "Waco", "YMF", "CG4A", "CG-4A",
                "Wasp and Scout",
                "Wright Flyer", "Wright Brothers",
                "Howard DGA",
                "Spartan Executive",
                "Noorduyn Norseman",
                "Fairchild 24",
                "Travel Air"
            };

            _loggingService.Info("Setting IsOldtimer flag for classic aircraft...");

            foreach (var pattern in oldtimerPatterns)
            {
                command.CommandText = $"UPDATE AircraftCatalog SET IsOldtimer = 1 WHERE Title LIKE '%{pattern.Replace("'", "''")}%'";
                var affected = command.ExecuteNonQuery();
                if (affected > 0)
                {
                    _loggingService.Debug($"Marked {affected} aircraft as oldtimer (pattern: {pattern})");
                }
            }

            command.CommandText = "SELECT COUNT(*) FROM AircraftCatalog WHERE IsOldtimer = 1";
            var totalOldtimers = Convert.ToInt32(command.ExecuteScalar());
            _loggingService.Info($"Total oldtimer aircraft marked: {totalOldtimers}");
        }
    }
}
