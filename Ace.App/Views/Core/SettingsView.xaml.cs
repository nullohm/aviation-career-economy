using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Ace.App.Interfaces;
using Ace.App.Services;
using Ace.App.Utilities;
using Ace.App.Views.Dialogs;

namespace Ace.App.Views.Core
{
    public partial class SettingsView : UserControl
    {
        private readonly ILoggingService _logger;
        private readonly ISettingsService _settingsService;
        private readonly IFinanceService _financeService;
        private readonly IDailyEarningsService _dailyEarningsService;
        private readonly IThemeService _themeService;
        private readonly ISoundService _soundService;
        private readonly IAirspaceService _airspaceService;
        private bool _isInitializing = true;

        public SettingsView(
            ILoggingService logger,
            ISettingsService settingsService,
            IFinanceService financeService,
            IDailyEarningsService dailyEarningsService,
            IThemeService themeService,
            ISoundService soundService,
            IAirspaceService airspaceService)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new System.ArgumentNullException(nameof(settingsService));
            _financeService = financeService ?? throw new System.ArgumentNullException(nameof(financeService));
            _dailyEarningsService = dailyEarningsService ?? throw new System.ArgumentNullException(nameof(dailyEarningsService));
            _themeService = themeService ?? throw new System.ArgumentNullException(nameof(themeService));
            _soundService = soundService ?? throw new System.ArgumentNullException(nameof(soundService));
            _airspaceService = airspaceService ?? throw new System.ArgumentNullException(nameof(airspaceService));

            InitializeComponent();
            LoadSettings();
            _isInitializing = false;
        }

        private void LoadSettings()
        {
            var settings = _settingsService.CurrentSettings;

            LoadThemeDropdown();

            for (int i = 0; i < CmbMapStyle.Items.Count; i++)
            {
                if (CmbMapStyle.Items[i] is ComboBoxItem item && item.Content?.ToString() == settings.MapStyle)
                {
                    CmbMapStyle.SelectedIndex = i;
                    break;
                }
            }

            TxtRateSmall.Text = settings.RatePerPaxPerNMSmall.ToString("0.00");
            TxtRateMedium.Text = settings.RatePerPaxPerNMMedium.ToString("0.00");
            TxtRateMediumLarge.Text = settings.RatePerPaxPerNMMediumLarge.ToString("0.00");
            TxtRateLarge.Text = settings.RatePerPaxPerNMLarge.ToString("0.00");
            TxtRateVeryLarge.Text = settings.RatePerPaxPerNMVeryLarge.ToString("0.00");

            TxtFBORentLocal.Text = settings.FBORentLocal.ToString("0");
            TxtFBORentRegional.Text = settings.FBORentRegional.ToString("0");
            TxtFBORentInternational.Text = settings.FBORentInternational.ToString("0");

            TxtTerminalCostSmall.Text = settings.TerminalCostSmall.ToString("0");
            TxtTerminalCostMedium.Text = settings.TerminalCostMedium.ToString("0");
            TxtTerminalCostMediumLarge.Text = settings.TerminalCostMediumLarge.ToString("0");
            TxtTerminalCostLarge.Text = settings.TerminalCostLarge.ToString("0");
            TxtTerminalCostVeryLarge.Text = settings.TerminalCostVeryLarge.ToString("0");

            TxtServiceCostRefueling.Text = settings.ServiceCostRefueling.ToString("0");
            TxtServiceCostHangar.Text = settings.ServiceCostHangar.ToString("0");
            TxtServiceCostCatering.Text = settings.ServiceCostCatering.ToString("0");
            TxtServiceCostGroundHandling.Text = settings.ServiceCostGroundHandling.ToString("0");
            TxtServiceCostDeIcing.Text = settings.ServiceCostDeIcing.ToString("0");

            TxtServiceCostRefuelingMedium.Text = settings.ServiceCostRefuelingMedium.ToString("0");
            TxtServiceCostHangarMedium.Text = settings.ServiceCostHangarMedium.ToString("0");
            TxtServiceCostCateringMedium.Text = settings.ServiceCostCateringMedium.ToString("0");
            TxtServiceCostGroundHandlingMedium.Text = settings.ServiceCostGroundHandlingMedium.ToString("0");
            TxtServiceCostDeIcingMedium.Text = settings.ServiceCostDeIcingMedium.ToString("0");

            TxtServiceCostRefuelingMediumLarge.Text = settings.ServiceCostRefuelingMediumLarge.ToString("0");
            TxtServiceCostHangarMediumLarge.Text = settings.ServiceCostHangarMediumLarge.ToString("0");
            TxtServiceCostCateringMediumLarge.Text = settings.ServiceCostCateringMediumLarge.ToString("0");
            TxtServiceCostGroundHandlingMediumLarge.Text = settings.ServiceCostGroundHandlingMediumLarge.ToString("0");
            TxtServiceCostDeIcingMediumLarge.Text = settings.ServiceCostDeIcingMediumLarge.ToString("0");

            TxtServiceCostRefuelingLarge.Text = settings.ServiceCostRefuelingLarge.ToString("0");
            TxtServiceCostHangarLarge.Text = settings.ServiceCostHangarLarge.ToString("0");
            TxtServiceCostCateringLarge.Text = settings.ServiceCostCateringLarge.ToString("0");
            TxtServiceCostGroundHandlingLarge.Text = settings.ServiceCostGroundHandlingLarge.ToString("0");
            TxtServiceCostDeIcingLarge.Text = settings.ServiceCostDeIcingLarge.ToString("0");

            TxtServiceCostRefuelingVeryLarge.Text = settings.ServiceCostRefuelingVeryLarge.ToString("0");
            TxtServiceCostHangarVeryLarge.Text = settings.ServiceCostHangarVeryLarge.ToString("0");
            TxtServiceCostCateringVeryLarge.Text = settings.ServiceCostCateringVeryLarge.ToString("0");
            TxtServiceCostGroundHandlingVeryLarge.Text = settings.ServiceCostGroundHandlingVeryLarge.ToString("0");
            TxtServiceCostDeIcingVeryLarge.Text = settings.ServiceCostDeIcingVeryLarge.ToString("0");

            TxtRouteSlotLimitLocal.Text = settings.RouteSlotLimitLocal.ToString();
            TxtRouteSlotLimitRegional.Text = settings.RouteSlotLimitRegional.ToString();
            TxtRouteSlotLimitInternational.Text = settings.RouteSlotLimitInternational.ToString();
            TxtRoutesPerFBOPairLimit.Text = settings.RoutesPerFBOPairLimit.ToString();

            TxtFuelPrice.Text = settings.FuelPricePerGallon.ToString("0.00");
            TxtFBOCostFactor.Text = (settings.FBOCostFactor * 100).ToString("0");

            TxtMaintenanceCostSmall.Text = settings.MaintenanceCostPerHourSmall.ToString("0");
            TxtMaintenanceCostMedium.Text = settings.MaintenanceCostPerHourMedium.ToString("0");
            TxtMaintenanceCostMediumLarge.Text = settings.MaintenanceCostPerHourMediumLarge.ToString("0");
            TxtMaintenanceCostLarge.Text = settings.MaintenanceCostPerHourLarge.ToString("0");
            TxtMaintenanceCostVeryLarge.Text = settings.MaintenanceCostPerHourVeryLarge.ToString("0");

            TxtMaintCheck50Hour.Text = settings.MaintenanceCheck50Hour.ToString("0");
            TxtMaintCheck100Hour.Text = settings.MaintenanceCheck100Hour.ToString("0");
            TxtMaintCheckAnnual.Text = settings.MaintenanceCheckAnnual.ToString("0");
            TxtMaintCheckACheck.Text = settings.MaintenanceCheckACheck.ToString("0");
            TxtMaintCheckBCheck.Text = settings.MaintenanceCheckBCheck.ToString("0");
            TxtMaintCheckCCheck.Text = settings.MaintenanceCheckCCheck.ToString("0");
            TxtMaintCheckDCheck.Text = settings.MaintenanceCheckDCheck.ToString("0");

            TxtDepreciation.Text = (settings.AircraftDepreciationRate * 100).ToString("0");
            TxtInsurance.Text = (settings.InsuranceRatePercentage * 100).ToString("0.00");

            TxtPlayerFlightBonus.Text = settings.PlayerFlightBonusPercent.ToString("0");
            TxtNetworkBonus.Text = settings.NetworkBonusPercent.ToString("0");
            TxtServiceBonusFactor.Text = settings.ServiceBonusFactorPercent.ToString("0");

            TxtPassengerLoadFactor.Text = settings.PassengerLoadFactorPercent.ToString("0");
            TxtCargoLoadFactor.Text = settings.CargoLoadFactorPercent.ToString("0");

            TxtPilotBaseSalary.Text = settings.PilotBaseSalary.ToString("0");
            TxtPilotFlightHoursPerDay.Text = settings.PilotFlightHoursPerDay.ToString("0.0");

            TxtRankJuniorHours.Text = settings.PilotRankJuniorHours.ToString("0");
            TxtRankSeniorHours.Text = settings.PilotRankSeniorHours.ToString("0");
            TxtRankCaptainHours.Text = settings.PilotRankCaptainHours.ToString("0");
            TxtRankSeniorCaptainHours.Text = settings.PilotRankSeniorCaptainHours.ToString("0");
            TxtRankChiefPilotHours.Text = settings.PilotRankChiefPilotHours.ToString("0");

            TxtTypeRatingCostSmall.Text = settings.TypeRatingCostSmall.ToString("0");
            TxtTypeRatingCostMedium.Text = settings.TypeRatingCostMedium.ToString("0");
            TxtTypeRatingCostMediumLarge.Text = settings.TypeRatingCostMediumLarge.ToString("0");
            TxtTypeRatingCostLarge.Text = settings.TypeRatingCostLarge.ToString("0");
            TxtTypeRatingCostVeryLarge.Text = settings.TypeRatingCostVeryLarge.ToString("0");

            TxtRankJuniorBonus.Text = (settings.PilotRankJuniorBonus * 100).ToString("0");
            TxtRankSeniorBonus.Text = (settings.PilotRankSeniorBonus * 100).ToString("0");
            TxtRankCaptainBonus.Text = (settings.PilotRankCaptainBonus * 100).ToString("0");
            TxtRankSeniorCaptainBonus.Text = (settings.PilotRankSeniorCaptainBonus * 100).ToString("0");
            TxtRankChiefPilotBonus.Text = (settings.PilotRankChiefPilotBonus * 100).ToString("0");

            TxtAchievementRewardMultiplier.Text = (settings.AchievementRewardMultiplier * 100).ToString("0");

            ChkAllowAllAircraftForFlightPlan.IsChecked = settings.AllowAllAircraftForFlightPlan;

            ChkSoundEnabled.IsChecked = _soundService.IsSoundEnabled;
            SliderVolume.Value = _soundService.Volume * 100;
            TxtVolume.Text = $"{(int)SliderVolume.Value}%";

            ChkSoundFlightCompleted.IsChecked = settings.SoundFlightCompletedEnabled;
            ChkSoundAchievement.IsChecked = settings.SoundAchievementEnabled;
            ChkSoundTopOfDescent.IsChecked = settings.SoundTopOfDescentEnabled;
            ChkSoundWarning.IsChecked = settings.SoundWarningEnabled;
            ChkSoundNotification.IsChecked = settings.SoundNotificationEnabled;
            ChkSoundButtonClick.IsChecked = settings.SoundButtonClickEnabled;

            ChkShowAirspaceOverlay.IsChecked = settings.ShowAirspaceOverlay;
            UpdateAirspaceStatus();

            TxtLittleNavmapPath.Text = settings.LittleNavmapDatabasePath;
            UpdateLnmDbStatus();

            _logger.Info($"SettingsView: Loaded settings - Size-based rates loaded");

            TxtRateSmall.LostFocus += OnRateChanged;
            TxtRateMedium.LostFocus += OnRateChanged;
            TxtRateMediumLarge.LostFocus += OnRateChanged;
            TxtRateLarge.LostFocus += OnRateChanged;
            TxtRateVeryLarge.LostFocus += OnRateChanged;
            TxtFBORentLocal.LostFocus += OnFBORentChanged;
            TxtFBORentRegional.LostFocus += OnFBORentChanged;
            TxtFBORentInternational.LostFocus += OnFBORentChanged;
            TxtTerminalCostSmall.LostFocus += OnTerminalCostChanged;
            TxtTerminalCostMedium.LostFocus += OnTerminalCostChanged;
            TxtTerminalCostMediumLarge.LostFocus += OnTerminalCostChanged;
            TxtTerminalCostLarge.LostFocus += OnTerminalCostChanged;
            TxtTerminalCostVeryLarge.LostFocus += OnTerminalCostChanged;
            TxtServiceCostRefueling.LostFocus += OnServiceCostsChanged;
            TxtServiceCostHangar.LostFocus += OnServiceCostsChanged;
            TxtServiceCostCatering.LostFocus += OnServiceCostsChanged;
            TxtServiceCostGroundHandling.LostFocus += OnServiceCostsChanged;
            TxtServiceCostDeIcing.LostFocus += OnServiceCostsChanged;
            TxtServiceCostRefuelingMedium.LostFocus += OnServiceCostsChanged;
            TxtServiceCostHangarMedium.LostFocus += OnServiceCostsChanged;
            TxtServiceCostCateringMedium.LostFocus += OnServiceCostsChanged;
            TxtServiceCostGroundHandlingMedium.LostFocus += OnServiceCostsChanged;
            TxtServiceCostDeIcingMedium.LostFocus += OnServiceCostsChanged;
            TxtServiceCostRefuelingMediumLarge.LostFocus += OnServiceCostsChanged;
            TxtServiceCostHangarMediumLarge.LostFocus += OnServiceCostsChanged;
            TxtServiceCostCateringMediumLarge.LostFocus += OnServiceCostsChanged;
            TxtServiceCostGroundHandlingMediumLarge.LostFocus += OnServiceCostsChanged;
            TxtServiceCostDeIcingMediumLarge.LostFocus += OnServiceCostsChanged;
            TxtServiceCostRefuelingLarge.LostFocus += OnServiceCostsChanged;
            TxtServiceCostHangarLarge.LostFocus += OnServiceCostsChanged;
            TxtServiceCostCateringLarge.LostFocus += OnServiceCostsChanged;
            TxtServiceCostGroundHandlingLarge.LostFocus += OnServiceCostsChanged;
            TxtServiceCostDeIcingLarge.LostFocus += OnServiceCostsChanged;
            TxtServiceCostRefuelingVeryLarge.LostFocus += OnServiceCostsChanged;
            TxtServiceCostHangarVeryLarge.LostFocus += OnServiceCostsChanged;
            TxtServiceCostCateringVeryLarge.LostFocus += OnServiceCostsChanged;
            TxtServiceCostGroundHandlingVeryLarge.LostFocus += OnServiceCostsChanged;
            TxtServiceCostDeIcingVeryLarge.LostFocus += OnServiceCostsChanged;
            TxtRouteSlotLimitLocal.LostFocus += OnRouteSettingsChanged;
            TxtRouteSlotLimitRegional.LostFocus += OnRouteSettingsChanged;
            TxtRouteSlotLimitInternational.LostFocus += OnRouteSettingsChanged;
            TxtRoutesPerFBOPairLimit.LostFocus += OnRouteSettingsChanged;
            TxtFuelPrice.LostFocus += OnGameplaySettingsChanged;
            TxtFBOCostFactor.LostFocus += OnGameplaySettingsChanged;
            TxtMaintenanceCostSmall.LostFocus += OnMaintenanceCostPerHourChanged;
            TxtMaintenanceCostMedium.LostFocus += OnMaintenanceCostPerHourChanged;
            TxtMaintenanceCostMediumLarge.LostFocus += OnMaintenanceCostPerHourChanged;
            TxtMaintenanceCostLarge.LostFocus += OnMaintenanceCostPerHourChanged;
            TxtMaintenanceCostVeryLarge.LostFocus += OnMaintenanceCostPerHourChanged;
            TxtMaintCheck50Hour.LostFocus += OnMaintenanceCheckCostsChanged;
            TxtMaintCheck100Hour.LostFocus += OnMaintenanceCheckCostsChanged;
            TxtMaintCheckAnnual.LostFocus += OnMaintenanceCheckCostsChanged;
            TxtMaintCheckACheck.LostFocus += OnMaintenanceCheckCostsChanged;
            TxtMaintCheckBCheck.LostFocus += OnMaintenanceCheckCostsChanged;
            TxtMaintCheckCCheck.LostFocus += OnMaintenanceCheckCostsChanged;
            TxtMaintCheckDCheck.LostFocus += OnMaintenanceCheckCostsChanged;
            TxtDepreciation.LostFocus += OnGameplaySettingsChanged;
            TxtInsurance.LostFocus += OnGameplaySettingsChanged;
            TxtPlayerFlightBonus.LostFocus += OnPlayerFlightBonusChanged;
            TxtNetworkBonus.LostFocus += OnNetworkBonusChanged;
            TxtServiceBonusFactor.LostFocus += OnServiceBonusFactorChanged;
            TxtPassengerLoadFactor.LostFocus += OnLoadFactorChanged;
            TxtCargoLoadFactor.LostFocus += OnLoadFactorChanged;

            TxtPilotBaseSalary.LostFocus += OnPilotRankSettingsChanged;
            TxtPilotFlightHoursPerDay.LostFocus += OnPilotRankSettingsChanged;

            TxtRankJuniorHours.LostFocus += OnPilotRankSettingsChanged;
            TxtRankSeniorHours.LostFocus += OnPilotRankSettingsChanged;
            TxtRankCaptainHours.LostFocus += OnPilotRankSettingsChanged;
            TxtRankSeniorCaptainHours.LostFocus += OnPilotRankSettingsChanged;
            TxtRankChiefPilotHours.LostFocus += OnPilotRankSettingsChanged;
            TxtRankJuniorBonus.LostFocus += OnPilotRankSettingsChanged;
            TxtRankSeniorBonus.LostFocus += OnPilotRankSettingsChanged;
            TxtRankCaptainBonus.LostFocus += OnPilotRankSettingsChanged;
            TxtRankSeniorCaptainBonus.LostFocus += OnPilotRankSettingsChanged;
            TxtRankChiefPilotBonus.LostFocus += OnPilotRankSettingsChanged;

            TxtTypeRatingCostSmall.LostFocus += OnTypeRatingCostsChanged;
            TxtTypeRatingCostMedium.LostFocus += OnTypeRatingCostsChanged;
            TxtTypeRatingCostMediumLarge.LostFocus += OnTypeRatingCostsChanged;
            TxtTypeRatingCostLarge.LostFocus += OnTypeRatingCostsChanged;
            TxtTypeRatingCostVeryLarge.LostFocus += OnTypeRatingCostsChanged;

            TxtAchievementRewardMultiplier.LostFocus += OnAchievementSettingsChanged;
        }

        private void OnRateChanged(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;
            bool changed = false;

            if (decimal.TryParse(TxtRateSmall.Text, out decimal rateSmall) && rateSmall >= 0)
            {
                settings.RatePerPaxPerNMSmall = rateSmall;
                changed = true;
            }
            else
            {
                TxtRateSmall.Text = settings.RatePerPaxPerNMSmall.ToString("0.00");
            }

            if (decimal.TryParse(TxtRateMedium.Text, out decimal rateMedium) && rateMedium >= 0)
            {
                settings.RatePerPaxPerNMMedium = rateMedium;
                changed = true;
            }
            else
            {
                TxtRateMedium.Text = settings.RatePerPaxPerNMMedium.ToString("0.00");
            }

            if (decimal.TryParse(TxtRateMediumLarge.Text, out decimal rateMediumLarge) && rateMediumLarge >= 0)
            {
                settings.RatePerPaxPerNMMediumLarge = rateMediumLarge;
                changed = true;
            }
            else
            {
                TxtRateMediumLarge.Text = settings.RatePerPaxPerNMMediumLarge.ToString("0.00");
            }

            if (decimal.TryParse(TxtRateLarge.Text, out decimal rateLarge) && rateLarge >= 0)
            {
                settings.RatePerPaxPerNMLarge = rateLarge;
                changed = true;
            }
            else
            {
                TxtRateLarge.Text = settings.RatePerPaxPerNMLarge.ToString("0.00");
            }

            if (decimal.TryParse(TxtRateVeryLarge.Text, out decimal rateVeryLarge) && rateVeryLarge >= 0)
            {
                settings.RatePerPaxPerNMVeryLarge = rateVeryLarge;
                changed = true;
            }
            else
            {
                TxtRateVeryLarge.Text = settings.RatePerPaxPerNMVeryLarge.ToString("0.00");
            }

            if (changed)
            {
                _settingsService.Save();
                _logger.Info($"SettingsView: Size-based rates updated - Small: {rateSmall}, Medium: {rateMedium}, ML: {rateMediumLarge}, Large: {rateLarge}, VL: {rateVeryLarge}");
            }
        }

        private void OnFBORentChanged(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;
            bool changed = false;

            if (decimal.TryParse(TxtFBORentLocal.Text, out decimal local) && local >= 0)
            {
                settings.FBORentLocal = local;
                changed = true;
            }
            else
            {
                TxtFBORentLocal.Text = settings.FBORentLocal.ToString("0");
            }

            if (decimal.TryParse(TxtFBORentRegional.Text, out decimal regional) && regional >= 0)
            {
                settings.FBORentRegional = regional;
                changed = true;
            }
            else
            {
                TxtFBORentRegional.Text = settings.FBORentRegional.ToString("0");
            }

            if (decimal.TryParse(TxtFBORentInternational.Text, out decimal international) && international >= 0)
            {
                settings.FBORentInternational = international;
                changed = true;
            }
            else
            {
                TxtFBORentInternational.Text = settings.FBORentInternational.ToString("0");
            }

            if (changed)
            {
                _settingsService.Save();
                _logger.Info($"SettingsView: FBO rent settings updated - Local: {settings.FBORentLocal}, Regional: {settings.FBORentRegional}, International: {settings.FBORentInternational}");
            }
        }

        private void OnTerminalCostChanged(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;
            bool changed = false;

            if (decimal.TryParse(TxtTerminalCostSmall.Text, out decimal small) && small >= 0)
            {
                settings.TerminalCostSmall = small;
                changed = true;
            }
            else
            {
                TxtTerminalCostSmall.Text = settings.TerminalCostSmall.ToString("0");
            }

            if (decimal.TryParse(TxtTerminalCostMedium.Text, out decimal medium) && medium >= 0)
            {
                settings.TerminalCostMedium = medium;
                changed = true;
            }
            else
            {
                TxtTerminalCostMedium.Text = settings.TerminalCostMedium.ToString("0");
            }

            if (decimal.TryParse(TxtTerminalCostMediumLarge.Text, out decimal mediumLarge) && mediumLarge >= 0)
            {
                settings.TerminalCostMediumLarge = mediumLarge;
                changed = true;
            }
            else
            {
                TxtTerminalCostMediumLarge.Text = settings.TerminalCostMediumLarge.ToString("0");
            }

            if (decimal.TryParse(TxtTerminalCostLarge.Text, out decimal large) && large >= 0)
            {
                settings.TerminalCostLarge = large;
                changed = true;
            }
            else
            {
                TxtTerminalCostLarge.Text = settings.TerminalCostLarge.ToString("0");
            }

            if (decimal.TryParse(TxtTerminalCostVeryLarge.Text, out decimal veryLarge) && veryLarge >= 0)
            {
                settings.TerminalCostVeryLarge = veryLarge;
                changed = true;
            }
            else
            {
                TxtTerminalCostVeryLarge.Text = settings.TerminalCostVeryLarge.ToString("0");
            }

            if (changed)
            {
                _settingsService.Save();
                _logger.Info($"SettingsView: Terminal cost settings updated - S: {settings.TerminalCostSmall}, M: {settings.TerminalCostMedium}, ML: {settings.TerminalCostMediumLarge}, L: {settings.TerminalCostLarge}, VL: {settings.TerminalCostVeryLarge}");
            }
        }

        private void OnServiceCostsChanged(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;
            bool changed = false;

            changed |= TryUpdateServiceCost(TxtServiceCostRefueling, v => settings.ServiceCostRefueling = v, settings.ServiceCostRefueling);
            changed |= TryUpdateServiceCost(TxtServiceCostHangar, v => settings.ServiceCostHangar = v, settings.ServiceCostHangar);
            changed |= TryUpdateServiceCost(TxtServiceCostCatering, v => settings.ServiceCostCatering = v, settings.ServiceCostCatering);
            changed |= TryUpdateServiceCost(TxtServiceCostGroundHandling, v => settings.ServiceCostGroundHandling = v, settings.ServiceCostGroundHandling);
            changed |= TryUpdateServiceCost(TxtServiceCostDeIcing, v => settings.ServiceCostDeIcing = v, settings.ServiceCostDeIcing);

            changed |= TryUpdateServiceCost(TxtServiceCostRefuelingMedium, v => settings.ServiceCostRefuelingMedium = v, settings.ServiceCostRefuelingMedium);
            changed |= TryUpdateServiceCost(TxtServiceCostHangarMedium, v => settings.ServiceCostHangarMedium = v, settings.ServiceCostHangarMedium);
            changed |= TryUpdateServiceCost(TxtServiceCostCateringMedium, v => settings.ServiceCostCateringMedium = v, settings.ServiceCostCateringMedium);
            changed |= TryUpdateServiceCost(TxtServiceCostGroundHandlingMedium, v => settings.ServiceCostGroundHandlingMedium = v, settings.ServiceCostGroundHandlingMedium);
            changed |= TryUpdateServiceCost(TxtServiceCostDeIcingMedium, v => settings.ServiceCostDeIcingMedium = v, settings.ServiceCostDeIcingMedium);

            changed |= TryUpdateServiceCost(TxtServiceCostRefuelingMediumLarge, v => settings.ServiceCostRefuelingMediumLarge = v, settings.ServiceCostRefuelingMediumLarge);
            changed |= TryUpdateServiceCost(TxtServiceCostHangarMediumLarge, v => settings.ServiceCostHangarMediumLarge = v, settings.ServiceCostHangarMediumLarge);
            changed |= TryUpdateServiceCost(TxtServiceCostCateringMediumLarge, v => settings.ServiceCostCateringMediumLarge = v, settings.ServiceCostCateringMediumLarge);
            changed |= TryUpdateServiceCost(TxtServiceCostGroundHandlingMediumLarge, v => settings.ServiceCostGroundHandlingMediumLarge = v, settings.ServiceCostGroundHandlingMediumLarge);
            changed |= TryUpdateServiceCost(TxtServiceCostDeIcingMediumLarge, v => settings.ServiceCostDeIcingMediumLarge = v, settings.ServiceCostDeIcingMediumLarge);

            changed |= TryUpdateServiceCost(TxtServiceCostRefuelingLarge, v => settings.ServiceCostRefuelingLarge = v, settings.ServiceCostRefuelingLarge);
            changed |= TryUpdateServiceCost(TxtServiceCostHangarLarge, v => settings.ServiceCostHangarLarge = v, settings.ServiceCostHangarLarge);
            changed |= TryUpdateServiceCost(TxtServiceCostCateringLarge, v => settings.ServiceCostCateringLarge = v, settings.ServiceCostCateringLarge);
            changed |= TryUpdateServiceCost(TxtServiceCostGroundHandlingLarge, v => settings.ServiceCostGroundHandlingLarge = v, settings.ServiceCostGroundHandlingLarge);
            changed |= TryUpdateServiceCost(TxtServiceCostDeIcingLarge, v => settings.ServiceCostDeIcingLarge = v, settings.ServiceCostDeIcingLarge);

            changed |= TryUpdateServiceCost(TxtServiceCostRefuelingVeryLarge, v => settings.ServiceCostRefuelingVeryLarge = v, settings.ServiceCostRefuelingVeryLarge);
            changed |= TryUpdateServiceCost(TxtServiceCostHangarVeryLarge, v => settings.ServiceCostHangarVeryLarge = v, settings.ServiceCostHangarVeryLarge);
            changed |= TryUpdateServiceCost(TxtServiceCostCateringVeryLarge, v => settings.ServiceCostCateringVeryLarge = v, settings.ServiceCostCateringVeryLarge);
            changed |= TryUpdateServiceCost(TxtServiceCostGroundHandlingVeryLarge, v => settings.ServiceCostGroundHandlingVeryLarge = v, settings.ServiceCostGroundHandlingVeryLarge);
            changed |= TryUpdateServiceCost(TxtServiceCostDeIcingVeryLarge, v => settings.ServiceCostDeIcingVeryLarge = v, settings.ServiceCostDeIcingVeryLarge);

            if (changed)
            {
                _settingsService.Save();
                _logger.Info("SettingsView: Service costs updated");
            }
        }

        private bool TryUpdateServiceCost(TextBox textBox, Action<decimal> setter, decimal currentValue)
        {
            if (decimal.TryParse(textBox.Text, out decimal value) && value >= 0)
            {
                setter(value);
                return true;
            }
            textBox.Text = currentValue.ToString("0");
            return false;
        }

        private void OnRouteSettingsChanged(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;
            bool changed = false;

            if (int.TryParse(TxtRouteSlotLimitLocal.Text, out int local) && local >= 0)
            {
                settings.RouteSlotLimitLocal = local;
                changed = true;
            }
            else
            {
                TxtRouteSlotLimitLocal.Text = settings.RouteSlotLimitLocal.ToString();
            }

            if (int.TryParse(TxtRouteSlotLimitRegional.Text, out int regional) && regional >= 0)
            {
                settings.RouteSlotLimitRegional = regional;
                changed = true;
            }
            else
            {
                TxtRouteSlotLimitRegional.Text = settings.RouteSlotLimitRegional.ToString();
            }

            if (int.TryParse(TxtRouteSlotLimitInternational.Text, out int international) && international >= 0)
            {
                settings.RouteSlotLimitInternational = international;
                changed = true;
            }
            else
            {
                TxtRouteSlotLimitInternational.Text = settings.RouteSlotLimitInternational.ToString();
            }

            if (int.TryParse(TxtRoutesPerFBOPairLimit.Text, out int pairLimit) && pairLimit >= 1)
            {
                settings.RoutesPerFBOPairLimit = pairLimit;
                changed = true;
            }
            else
            {
                TxtRoutesPerFBOPairLimit.Text = settings.RoutesPerFBOPairLimit.ToString();
            }

            if (changed)
            {
                _settingsService.Save();
                _logger.Info($"SettingsView: Route settings updated - Local: {settings.RouteSlotLimitLocal}, Regional: {settings.RouteSlotLimitRegional}, Int'l: {settings.RouteSlotLimitInternational}, PairLimit: {settings.RoutesPerFBOPairLimit}");
            }
        }

        private void OnMaintenanceCostPerHourChanged(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;
            bool changed = false;

            if (decimal.TryParse(TxtMaintenanceCostSmall.Text, out decimal small) && small >= 0)
            {
                settings.MaintenanceCostPerHourSmall = small;
                changed = true;
            }
            else
            {
                TxtMaintenanceCostSmall.Text = settings.MaintenanceCostPerHourSmall.ToString("0");
            }

            if (decimal.TryParse(TxtMaintenanceCostMedium.Text, out decimal medium) && medium >= 0)
            {
                settings.MaintenanceCostPerHourMedium = medium;
                changed = true;
            }
            else
            {
                TxtMaintenanceCostMedium.Text = settings.MaintenanceCostPerHourMedium.ToString("0");
            }

            if (decimal.TryParse(TxtMaintenanceCostMediumLarge.Text, out decimal mediumLarge) && mediumLarge >= 0)
            {
                settings.MaintenanceCostPerHourMediumLarge = mediumLarge;
                changed = true;
            }
            else
            {
                TxtMaintenanceCostMediumLarge.Text = settings.MaintenanceCostPerHourMediumLarge.ToString("0");
            }

            if (decimal.TryParse(TxtMaintenanceCostLarge.Text, out decimal large) && large >= 0)
            {
                settings.MaintenanceCostPerHourLarge = large;
                changed = true;
            }
            else
            {
                TxtMaintenanceCostLarge.Text = settings.MaintenanceCostPerHourLarge.ToString("0");
            }

            if (decimal.TryParse(TxtMaintenanceCostVeryLarge.Text, out decimal veryLarge) && veryLarge >= 0)
            {
                settings.MaintenanceCostPerHourVeryLarge = veryLarge;
                changed = true;
            }
            else
            {
                TxtMaintenanceCostVeryLarge.Text = settings.MaintenanceCostPerHourVeryLarge.ToString("0");
            }

            if (changed)
            {
                _settingsService.Save();
                _logger.Info($"SettingsView: Maintenance cost per hour updated - S: {settings.MaintenanceCostPerHourSmall}, M: {settings.MaintenanceCostPerHourMedium}, ML: {settings.MaintenanceCostPerHourMediumLarge}, L: {settings.MaintenanceCostPerHourLarge}, VL: {settings.MaintenanceCostPerHourVeryLarge}");
            }
        }

        private void OnMaintenanceCheckCostsChanged(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;
            bool changed = false;

            if (decimal.TryParse(TxtMaintCheck50Hour.Text, out decimal check50) && check50 >= 0)
            {
                settings.MaintenanceCheck50Hour = check50;
                changed = true;
            }
            else
            {
                TxtMaintCheck50Hour.Text = settings.MaintenanceCheck50Hour.ToString("0");
            }

            if (decimal.TryParse(TxtMaintCheck100Hour.Text, out decimal check100) && check100 >= 0)
            {
                settings.MaintenanceCheck100Hour = check100;
                changed = true;
            }
            else
            {
                TxtMaintCheck100Hour.Text = settings.MaintenanceCheck100Hour.ToString("0");
            }

            if (decimal.TryParse(TxtMaintCheckAnnual.Text, out decimal checkAnnual) && checkAnnual >= 0)
            {
                settings.MaintenanceCheckAnnual = checkAnnual;
                changed = true;
            }
            else
            {
                TxtMaintCheckAnnual.Text = settings.MaintenanceCheckAnnual.ToString("0");
            }

            if (decimal.TryParse(TxtMaintCheckACheck.Text, out decimal checkA) && checkA >= 0)
            {
                settings.MaintenanceCheckACheck = checkA;
                changed = true;
            }
            else
            {
                TxtMaintCheckACheck.Text = settings.MaintenanceCheckACheck.ToString("0");
            }

            if (decimal.TryParse(TxtMaintCheckBCheck.Text, out decimal checkB) && checkB >= 0)
            {
                settings.MaintenanceCheckBCheck = checkB;
                changed = true;
            }
            else
            {
                TxtMaintCheckBCheck.Text = settings.MaintenanceCheckBCheck.ToString("0");
            }

            if (decimal.TryParse(TxtMaintCheckCCheck.Text, out decimal checkC) && checkC >= 0)
            {
                settings.MaintenanceCheckCCheck = checkC;
                changed = true;
            }
            else
            {
                TxtMaintCheckCCheck.Text = settings.MaintenanceCheckCCheck.ToString("0");
            }

            if (decimal.TryParse(TxtMaintCheckDCheck.Text, out decimal checkD) && checkD >= 0)
            {
                settings.MaintenanceCheckDCheck = checkD;
                changed = true;
            }
            else
            {
                TxtMaintCheckDCheck.Text = settings.MaintenanceCheckDCheck.ToString("0");
            }

            if (changed)
            {
                _settingsService.Save();
                _logger.Info($"SettingsView: Maintenance check costs updated");
            }
        }

        private void OnGameplaySettingsChanged(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;
            bool changed = false;

            if (decimal.TryParse(TxtFuelPrice.Text, out decimal fuelPrice) && fuelPrice >= 0)
            {
                settings.FuelPricePerGallon = fuelPrice;
                changed = true;
            }
            else
            {
                TxtFuelPrice.Text = settings.FuelPricePerGallon.ToString("0.00");
            }

            if (decimal.TryParse(TxtFBOCostFactor.Text, out decimal fboCostFactor) && fboCostFactor >= 0 && fboCostFactor <= 100)
            {
                settings.FBOCostFactor = fboCostFactor / 100m;
                changed = true;
            }
            else
            {
                TxtFBOCostFactor.Text = (settings.FBOCostFactor * 100).ToString("0");
            }

            if (decimal.TryParse(TxtDepreciation.Text, out decimal depreciation) && depreciation >= 0 && depreciation <= 100)
            {
                settings.AircraftDepreciationRate = depreciation / 100m;
                changed = true;
            }
            else
            {
                TxtDepreciation.Text = (settings.AircraftDepreciationRate * 100).ToString("0");
            }

            if (decimal.TryParse(TxtInsurance.Text, out decimal insurance) && insurance >= 0 && insurance <= 100)
            {
                settings.InsuranceRatePercentage = insurance / 100m;
                changed = true;
            }
            else
            {
                TxtInsurance.Text = (settings.InsuranceRatePercentage * 100).ToString("0.00");
            }

            if (changed)
            {
                _settingsService.Save();
                _logger.Info($"SettingsView: Gameplay settings updated - Fuel: {settings.FuelPricePerGallon}, Depreciation: {settings.AircraftDepreciationRate * 100}%, Insurance: {settings.InsuranceRatePercentage * 100}%/year");
            }
        }

        private void BtnAddFunds_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(TxtAddFunds.Text, out decimal amount) && amount > 0)
            {
                _financeService.AddEarnings(amount, "Capital injection");
                _logger.Info($"SettingsView: Added capital injection of €{amount:N2}");
            }
            else
            {
                _logger.Warn($"SettingsView: Invalid amount for capital injection: {TxtAddFunds.Text}");
                InfoDialog.Show("Invalid Amount", "Please enter a valid positive amount.", Window.GetWindow(this));
            }
        }

        private void BtnRemoveFunds_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(TxtAddFunds.Text, out decimal amount) && amount > 0)
            {
                _financeService.AddExpense(amount, "Capital withdrawal");
                _logger.Info($"SettingsView: Removed capital withdrawal of €{amount:N2}");
            }
            else
            {
                _logger.Warn($"SettingsView: Invalid amount for capital withdrawal: {TxtAddFunds.Text}");
                InfoDialog.Show("Invalid Amount", "Please enter a valid positive amount.", Window.GetWindow(this));
            }
        }

        private async void BtnTestEarnings_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag?.ToString(), out int days))
            {
                DevTools.TestDailyEarnings.SetDateDaysAgo(days);
                await _dailyEarningsService.ProcessDailyEarnings();
                _logger.Info($"SettingsView: Simulated {days} day(s) of daily earnings");
            }
        }

        private void OnPlayerFlightBonusChanged(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;

            if (decimal.TryParse(TxtPlayerFlightBonus.Text, out decimal bonus) && bonus >= 0 && bonus <= 1000)
            {
                settings.PlayerFlightBonusPercent = bonus;
                _settingsService.Save();
                _logger.Info($"SettingsView: Player flight bonus updated to {bonus}%");
            }
            else
            {
                TxtPlayerFlightBonus.Text = settings.PlayerFlightBonusPercent.ToString("0");
            }
        }

        private void OnNetworkBonusChanged(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;

            if (decimal.TryParse(TxtNetworkBonus.Text, out decimal bonus) && bonus >= 0 && bonus <= 500)
            {
                settings.NetworkBonusPercent = bonus;
                _settingsService.Save();
                _logger.Info($"SettingsView: Network bonus updated to {bonus}%");
            }
            else
            {
                TxtNetworkBonus.Text = settings.NetworkBonusPercent.ToString("0");
            }
        }

        private void OnServiceBonusFactorChanged(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;

            if (decimal.TryParse(TxtServiceBonusFactor.Text, out decimal factor) && factor >= 0 && factor <= 100)
            {
                settings.ServiceBonusFactorPercent = factor;
                _settingsService.Save();
                _logger.Info($"SettingsView: Service bonus factor updated to {factor}%");
            }
            else
            {
                TxtServiceBonusFactor.Text = settings.ServiceBonusFactorPercent.ToString("0");
            }
        }

        private void OnLoadFactorChanged(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;
            bool changed = false;

            if (decimal.TryParse(TxtPassengerLoadFactor.Text, out decimal paxFactor) && paxFactor >= 1 && paxFactor <= 100)
            {
                settings.PassengerLoadFactorPercent = paxFactor;
                changed = true;
            }
            else
            {
                TxtPassengerLoadFactor.Text = settings.PassengerLoadFactorPercent.ToString("0");
            }

            if (decimal.TryParse(TxtCargoLoadFactor.Text, out decimal cargoFactor) && cargoFactor >= 1 && cargoFactor <= 100)
            {
                settings.CargoLoadFactorPercent = cargoFactor;
                changed = true;
            }
            else
            {
                TxtCargoLoadFactor.Text = settings.CargoLoadFactorPercent.ToString("0");
            }

            if (changed)
            {
                _settingsService.Save();
                _logger.Info($"SettingsView: Load factors updated - Passenger: {settings.PassengerLoadFactorPercent}%, Cargo: {settings.CargoLoadFactorPercent}%");
            }
        }

        private void LoadThemeDropdown()
        {
            CmbTheme.Items.Clear();
            var themes = _themeService.AvailableThemes;
            var currentTheme = _themeService.CurrentTheme;
            int selectedIndex = 0;

            for (int i = 0; i < themes.Count; i++)
            {
                CmbTheme.Items.Add(themes[i]);
                if (themes[i].Name.Equals(currentTheme, System.StringComparison.OrdinalIgnoreCase))
                {
                    selectedIndex = i;
                }
            }

            CmbTheme.SelectedIndex = selectedIndex;
        }

        private void CmbTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isInitializing) return;

            if (CmbTheme.SelectedItem is Models.ThemeInfo themeInfo)
            {
                _themeService.SetTheme(themeInfo.Name);
                _logger.Info($"SettingsView: Theme changed to '{themeInfo.DisplayName}'");
            }
        }

        private void BtnEditTheme_Click(object sender, RoutedEventArgs e)
        {
            _themeService.OpenCurrentThemeInEditor();
        }

        private void BtnCreateTheme_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog("Create New Theme", "Enter a name for the new theme:");
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.InputText))
            {
                _themeService.CreateThemeFromCurrent(dialog.InputText);
                LoadThemeDropdown();
                _logger.Info($"SettingsView: Created new theme '{dialog.InputText}'");
            }
        }

        private void BtnReloadTheme_Click(object sender, RoutedEventArgs e)
        {
            _themeService.ReloadCurrentTheme();
            _logger.Info("SettingsView: Theme reloaded");
        }

        private void CmbMapStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isInitializing) return;

            if (CmbMapStyle.SelectedItem is ComboBoxItem item && item.Content?.ToString() is string mapStyle)
            {
                var settings = _settingsService.CurrentSettings;
                settings.MapStyle = mapStyle;
                _settingsService.Save();
                _logger.Info($"SettingsView: Map style changed to '{mapStyle}' - Restart required for maps to update");
            }
        }

        private void OnPilotRankSettingsChanged(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;
            bool changed = false;

            if (decimal.TryParse(TxtPilotBaseSalary.Text, out decimal baseSalary) && baseSalary >= 0)
            {
                settings.PilotBaseSalary = baseSalary;
                changed = true;
            }
            else
            {
                TxtPilotBaseSalary.Text = settings.PilotBaseSalary.ToString("0");
            }

            if (decimal.TryParse(TxtPilotFlightHoursPerDay.Text, out decimal flightHoursPerDay) && flightHoursPerDay > 0 && flightHoursPerDay <= 24)
            {
                settings.PilotFlightHoursPerDay = flightHoursPerDay;
                changed = true;
            }
            else
            {
                TxtPilotFlightHoursPerDay.Text = settings.PilotFlightHoursPerDay.ToString("0.0");
            }

            if (decimal.TryParse(TxtRankJuniorHours.Text, out decimal juniorHours) && juniorHours >= 0)
            {
                settings.PilotRankJuniorHours = juniorHours;
                changed = true;
            }
            else
            {
                TxtRankJuniorHours.Text = settings.PilotRankJuniorHours.ToString("0");
            }

            if (decimal.TryParse(TxtRankSeniorHours.Text, out decimal seniorHours) && seniorHours >= 0)
            {
                settings.PilotRankSeniorHours = seniorHours;
                changed = true;
            }
            else
            {
                TxtRankSeniorHours.Text = settings.PilotRankSeniorHours.ToString("0");
            }

            if (decimal.TryParse(TxtRankCaptainHours.Text, out decimal captainHours) && captainHours >= 0)
            {
                settings.PilotRankCaptainHours = captainHours;
                changed = true;
            }
            else
            {
                TxtRankCaptainHours.Text = settings.PilotRankCaptainHours.ToString("0");
            }

            if (decimal.TryParse(TxtRankSeniorCaptainHours.Text, out decimal seniorCaptainHours) && seniorCaptainHours >= 0)
            {
                settings.PilotRankSeniorCaptainHours = seniorCaptainHours;
                changed = true;
            }
            else
            {
                TxtRankSeniorCaptainHours.Text = settings.PilotRankSeniorCaptainHours.ToString("0");
            }

            if (decimal.TryParse(TxtRankChiefPilotHours.Text, out decimal chiefPilotHours) && chiefPilotHours >= 0)
            {
                settings.PilotRankChiefPilotHours = chiefPilotHours;
                changed = true;
            }
            else
            {
                TxtRankChiefPilotHours.Text = settings.PilotRankChiefPilotHours.ToString("0");
            }

            if (decimal.TryParse(TxtRankJuniorBonus.Text, out decimal juniorBonus) && juniorBonus >= 0 && juniorBonus <= 500)
            {
                settings.PilotRankJuniorBonus = juniorBonus / 100m;
                changed = true;
            }
            else
            {
                TxtRankJuniorBonus.Text = (settings.PilotRankJuniorBonus * 100).ToString("0");
            }

            if (decimal.TryParse(TxtRankSeniorBonus.Text, out decimal seniorBonus) && seniorBonus >= 0 && seniorBonus <= 500)
            {
                settings.PilotRankSeniorBonus = seniorBonus / 100m;
                changed = true;
            }
            else
            {
                TxtRankSeniorBonus.Text = (settings.PilotRankSeniorBonus * 100).ToString("0");
            }

            if (decimal.TryParse(TxtRankCaptainBonus.Text, out decimal captainBonus) && captainBonus >= 0 && captainBonus <= 500)
            {
                settings.PilotRankCaptainBonus = captainBonus / 100m;
                changed = true;
            }
            else
            {
                TxtRankCaptainBonus.Text = (settings.PilotRankCaptainBonus * 100).ToString("0");
            }

            if (decimal.TryParse(TxtRankSeniorCaptainBonus.Text, out decimal seniorCaptainBonus) && seniorCaptainBonus >= 0 && seniorCaptainBonus <= 500)
            {
                settings.PilotRankSeniorCaptainBonus = seniorCaptainBonus / 100m;
                changed = true;
            }
            else
            {
                TxtRankSeniorCaptainBonus.Text = (settings.PilotRankSeniorCaptainBonus * 100).ToString("0");
            }

            if (decimal.TryParse(TxtRankChiefPilotBonus.Text, out decimal chiefPilotBonus) && chiefPilotBonus >= 0 && chiefPilotBonus <= 500)
            {
                settings.PilotRankChiefPilotBonus = chiefPilotBonus / 100m;
                changed = true;
            }
            else
            {
                TxtRankChiefPilotBonus.Text = (settings.PilotRankChiefPilotBonus * 100).ToString("0");
            }

            if (changed)
            {
                _settingsService.Save();
                _logger.Info($"SettingsView: Pilot rank settings updated");
            }
        }

        private void OnAchievementSettingsChanged(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;

            if (decimal.TryParse(TxtAchievementRewardMultiplier.Text, out decimal percent) && percent >= 0 && percent <= 1000)
            {
                settings.AchievementRewardMultiplier = percent / 100m;
                _settingsService.Save();
                _logger.Info($"SettingsView: Achievement reward updated to {percent}%");
            }
            else
            {
                TxtAchievementRewardMultiplier.Text = (settings.AchievementRewardMultiplier * 100).ToString("0");
            }
        }

        private void OnTypeRatingCostsChanged(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;
            bool changed = false;

            if (decimal.TryParse(TxtTypeRatingCostSmall.Text, out decimal small) && small >= 0)
            {
                settings.TypeRatingCostSmall = small;
                changed = true;
            }
            else
            {
                TxtTypeRatingCostSmall.Text = settings.TypeRatingCostSmall.ToString("0");
            }

            if (decimal.TryParse(TxtTypeRatingCostMedium.Text, out decimal medium) && medium >= 0)
            {
                settings.TypeRatingCostMedium = medium;
                changed = true;
            }
            else
            {
                TxtTypeRatingCostMedium.Text = settings.TypeRatingCostMedium.ToString("0");
            }

            if (decimal.TryParse(TxtTypeRatingCostMediumLarge.Text, out decimal mediumLarge) && mediumLarge >= 0)
            {
                settings.TypeRatingCostMediumLarge = mediumLarge;
                changed = true;
            }
            else
            {
                TxtTypeRatingCostMediumLarge.Text = settings.TypeRatingCostMediumLarge.ToString("0");
            }

            if (decimal.TryParse(TxtTypeRatingCostLarge.Text, out decimal large) && large >= 0)
            {
                settings.TypeRatingCostLarge = large;
                changed = true;
            }
            else
            {
                TxtTypeRatingCostLarge.Text = settings.TypeRatingCostLarge.ToString("0");
            }

            if (decimal.TryParse(TxtTypeRatingCostVeryLarge.Text, out decimal veryLarge) && veryLarge >= 0)
            {
                settings.TypeRatingCostVeryLarge = veryLarge;
                changed = true;
            }
            else
            {
                TxtTypeRatingCostVeryLarge.Text = settings.TypeRatingCostVeryLarge.ToString("0");
            }

            if (changed)
            {
                _settingsService.Save();
                _logger.Info($"SettingsView: Type rating costs updated - S: {settings.TypeRatingCostSmall}, M: {settings.TypeRatingCostMedium}, ML: {settings.TypeRatingCostMediumLarge}, L: {settings.TypeRatingCostLarge}, VL: {settings.TypeRatingCostVeryLarge}");
            }
        }

        private void ChkAllowAllAircraftForFlightPlan_Changed(object sender, RoutedEventArgs e)
        {
            if (_isInitializing) return;

            var settings = _settingsService.CurrentSettings;
            settings.AllowAllAircraftForFlightPlan = ChkAllowAllAircraftForFlightPlan.IsChecked ?? false;
            _settingsService.Save();
            _logger.Info($"SettingsView: AllowAllAircraftForFlightPlan set to {settings.AllowAllAircraftForFlightPlan}");
        }

        private void ChkSoundEnabled_Changed(object sender, RoutedEventArgs e)
        {
            if (_isInitializing) return;

            _soundService.IsSoundEnabled = ChkSoundEnabled.IsChecked ?? true;
            _logger.Info($"SettingsView: Sound enabled set to {_soundService.IsSoundEnabled}");
        }

        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isInitializing) return;

            _soundService.Volume = SliderVolume.Value / 100.0;
            TxtVolume.Text = $"{(int)SliderVolume.Value}%";
        }

        private void ChkSoundFlightCompleted_Changed(object sender, RoutedEventArgs e)
        {
            if (_isInitializing) return;

            var settings = _settingsService.CurrentSettings;
            settings.SoundFlightCompletedEnabled = ChkSoundFlightCompleted.IsChecked ?? true;
            _settingsService.Save();
            _logger.Info($"SettingsView: SoundFlightCompletedEnabled set to {settings.SoundFlightCompletedEnabled}");
        }

        private void ChkSoundAchievement_Changed(object sender, RoutedEventArgs e)
        {
            if (_isInitializing) return;

            var settings = _settingsService.CurrentSettings;
            settings.SoundAchievementEnabled = ChkSoundAchievement.IsChecked ?? true;
            _settingsService.Save();
            _logger.Info($"SettingsView: SoundAchievementEnabled set to {settings.SoundAchievementEnabled}");
        }

        private void ChkSoundTopOfDescent_Changed(object sender, RoutedEventArgs e)
        {
            if (_isInitializing) return;

            var settings = _settingsService.CurrentSettings;
            settings.SoundTopOfDescentEnabled = ChkSoundTopOfDescent.IsChecked ?? true;
            _settingsService.Save();
            _logger.Info($"SettingsView: SoundTopOfDescentEnabled set to {settings.SoundTopOfDescentEnabled}");
        }

        private void ChkSoundWarning_Changed(object sender, RoutedEventArgs e)
        {
            if (_isInitializing) return;

            var settings = _settingsService.CurrentSettings;
            settings.SoundWarningEnabled = ChkSoundWarning.IsChecked ?? true;
            _settingsService.Save();
            _logger.Info($"SettingsView: SoundWarningEnabled set to {settings.SoundWarningEnabled}");
        }

        private void ChkSoundNotification_Changed(object sender, RoutedEventArgs e)
        {
            if (_isInitializing) return;

            var settings = _settingsService.CurrentSettings;
            settings.SoundNotificationEnabled = ChkSoundNotification.IsChecked ?? true;
            _settingsService.Save();
            _logger.Info($"SettingsView: SoundNotificationEnabled set to {settings.SoundNotificationEnabled}");
        }

        private void ChkSoundButtonClick_Changed(object sender, RoutedEventArgs e)
        {
            if (_isInitializing) return;

            var settings = _settingsService.CurrentSettings;
            settings.SoundButtonClickEnabled = ChkSoundButtonClick.IsChecked ?? false;
            _settingsService.Save();
            _logger.Info($"SettingsView: SoundButtonClickEnabled set to {settings.SoundButtonClickEnabled}");
        }

        private void ChkShowAirspaceOverlay_Changed(object sender, RoutedEventArgs e)
        {
            if (_isInitializing) return;

            var settings = _settingsService.CurrentSettings;
            settings.ShowAirspaceOverlay = ChkShowAirspaceOverlay.IsChecked ?? false;
            _settingsService.Save();
            _logger.Info($"SettingsView: ShowAirspaceOverlay set to {settings.ShowAirspaceOverlay}");
        }

        private void BtnRefreshAirspaces_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _airspaceService.RefreshAirspaces();
                UpdateAirspaceStatus();
                _logger.Info("SettingsView: Airspace data refreshed");
            }
            catch (System.Exception ex)
            {
                _logger.Error("SettingsView: Failed to refresh airspace data", ex);
                TxtAirspaceStatus.Text = "Error refreshing airspace data";
            }
        }

        private void UpdateAirspaceStatus()
        {
            var airspaces = _airspaceService.GetAllAirspaces();
            if (airspaces.Count > 0)
            {
                TxtAirspaceStatus.Text = $"{airspaces.Count} airspaces loaded";
                TxtAirspaceStatus.Foreground = (System.Windows.Media.Brush)FindResource("SuccessBrush");
            }
            else
            {
                TxtAirspaceStatus.Text = "No airspace data found";
                TxtAirspaceStatus.Foreground = (System.Windows.Media.Brush)FindResource("SubtleForegroundBrush");
            }
        }

        private void BtnBrowseLnmPath_Click(object sender, RoutedEventArgs e)
        {
            var defaultPath = LittleNavmapPathResolver.GetDefaultPath();
            var initialDirectory = Directory.Exists(defaultPath)
                ? defaultPath
                : Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var dialog = new OpenFileDialog
            {
                Title = "Select Little Navmap Database Folder",
                Filter = "SQLite Database|*.sqlite|All Files|*.*",
                InitialDirectory = initialDirectory,
                FileName = "little_navmap_navigraph.sqlite"
            };

            if (dialog.ShowDialog() == true)
            {
                var selectedPath = Path.GetDirectoryName(dialog.FileName) ?? string.Empty;
                var settings = _settingsService.CurrentSettings;
                settings.LittleNavmapDatabasePath = selectedPath;
                _settingsService.Save();

                TxtLittleNavmapPath.Text = selectedPath;
                UpdateLnmDbStatus();
                _logger.Info($"SettingsView: Little Navmap database path set to '{selectedPath}'");
            }
        }

        private void BtnClearLnmPath_Click(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;
            settings.LittleNavmapDatabasePath = string.Empty;
            _settingsService.Save();

            TxtLittleNavmapPath.Text = string.Empty;
            UpdateLnmDbStatus();
            _logger.Info("SettingsView: Little Navmap database path cleared (using auto-detect)");
        }

        private void UpdateLnmDbStatus()
        {
            var path = _settingsService.CurrentSettings.LittleNavmapDatabasePath;
            var checkPath = string.IsNullOrEmpty(path) ? LittleNavmapPathResolver.GetDefaultPath() : path;
            var isAutoDetect = string.IsNullOrEmpty(path);

            if (!Directory.Exists(checkPath))
            {
                TxtLnmDbStatus.Text = isAutoDetect
                    ? "No database found (install Little Navmap or set path manually)"
                    : "Path does not exist";
                TxtLnmDbStatus.Foreground = (System.Windows.Media.Brush)FindResource(isAutoDetect ? "WarningBrush" : "ErrorBrush");
                return;
            }

            var navigraphDb = Path.Combine(checkPath, "little_navmap_navigraph.sqlite");
            var msfsDb = Path.Combine(checkPath, "little_navmap_msfs24.sqlite");
            var dbCount = (File.Exists(navigraphDb) ? 1 : 0) + (File.Exists(msfsDb) ? 1 : 0);

            if (dbCount > 0)
            {
                TxtLnmDbStatus.Text = isAutoDetect
                    ? $"Auto-detected: {dbCount} database(s) found"
                    : $"{dbCount} database(s) found";
                TxtLnmDbStatus.Foreground = (System.Windows.Media.Brush)FindResource("SuccessBrush");
            }
            else
            {
                TxtLnmDbStatus.Text = "No valid databases in this folder";
                TxtLnmDbStatus.Foreground = (System.Windows.Media.Brush)FindResource("WarningBrush");
            }
        }
    }
}
