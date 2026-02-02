using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ace.App.Converters;
using Ace.App.Helpers;
using Ace.App.Infrastructure;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;
using Ace.App.ViewModels;
using Ace.App.Views.Dialogs;

namespace Ace.App.Views.Aircraft
{
    public partial class HangarView : UserControl
    {
        private readonly HangarViewModel _viewModel;
        private readonly ILoggingService _logger;
        private readonly IFinanceService _financeService;
        private readonly IAircraftImageService _imageService;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IFBORepository _fboRepository;
        private readonly IAircraftCatalogRepository _catalogRepository;
        private readonly IPilotRepository _pilotRepository;
        private readonly ITypeRatingRepository _typeRatingRepository;
        private readonly IAircraftPilotAssignmentRepository _assignmentRepository;
        private readonly ISettingsService _settingsService;
        private AircraftViewModel? _selectedAircraft;
        private Models.Aircraft? _selectedAircraftModel;
        private ObservableCollection<FBOSelectionItem> _availableFBOs = new();
        private bool _listExpanded = true;

        public HangarView(
            HangarViewModel viewModel,
            ILoggingService logger,
            IFinanceService financeService,
            IAircraftImageService imageService,
            IAircraftRepository aircraftRepository,
            IFBORepository fboRepository,
            IAircraftCatalogRepository catalogRepository,
            IPilotRepository pilotRepository,
            ITypeRatingRepository typeRatingRepository,
            IAircraftPilotAssignmentRepository assignmentRepository,
            ISettingsService settingsService)
        {
            InitializeComponent();
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));
            _fboRepository = fboRepository ?? throw new ArgumentNullException(nameof(fboRepository));
            _catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
            _pilotRepository = pilotRepository ?? throw new ArgumentNullException(nameof(pilotRepository));
            _typeRatingRepository = typeRatingRepository ?? throw new ArgumentNullException(nameof(typeRatingRepository));
            _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            DataContext = _viewModel;

            Loaded += HangarView_Loaded;
        }

        private void HangarView_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadAircraft();
            UpdateStats();
        }

        private void UpdateStats()
        {
            var aircraft = _viewModel.FilteredAircraft;
            var allAircraft = _viewModel.Aircraft;

            TxtAircraftCount.Text = $"{allAircraft.Count} Aircraft";
            TxtAircraftListHeader.Text = $"Your Aircraft ({aircraft.Count})";

            var totalValue = allAircraft.Sum(a =>
            {
                var valueStr = a.ValueText.Replace("€", "").Replace(".", "").Replace(",", "").Trim();
                return decimal.TryParse(valueStr, out var v) ? v : 0;
            });
            TxtFleetValue.Text = $"€{totalValue:N0}";
        }

        private void SelectAircraft(AircraftViewModel aircraft)
        {
            _selectedAircraft = aircraft;
            UpdateDetailsPanel();
            ShowDetails();
        }

        private void ShowDetails()
        {
            TxtSelectHint.Visibility = Visibility.Collapsed;
            HeaderContent.Visibility = Visibility.Visible;
            DetailsContent.Visibility = Visibility.Visible;
        }

        private void HideDetails()
        {
            TxtSelectHint.Visibility = Visibility.Visible;
            HeaderContent.Visibility = Visibility.Collapsed;
            DetailsContent.Visibility = Visibility.Collapsed;
        }

        private void UpdateDetailsPanel()
        {
            if (_selectedAircraft == null) return;

            TxtDetailRegistration.Text = _selectedAircraft.Registration;
            TxtDetailType.Text = _selectedAircraft.TypeVariant;
            TxtDetailStatus.Text = _selectedAircraft.Status;
            DetailStatusBadge.Background = _selectedAircraft.StatusColor;

            if (_selectedAircraft.ShowMaintenanceWarning)
            {
                DetailMaintenanceBadge.Visibility = Visibility.Visible;
                DetailMaintenanceBadge.Background = _selectedAircraft.MaintenanceWarningColor;
                TxtDetailMaintenance.Text = _selectedAircraft.MaintenanceWarningText;
            }
            else
            {
                DetailMaintenanceBadge.Visibility = Visibility.Collapsed;
            }

            TxtDetailSize.Text = _selectedAircraft.SizeCategory;
            TxtDetailLocation.Text = _selectedAircraft.HomeBase;
            TxtDetailFlightHours.Text = _selectedAircraft.FlightHoursText;
            TxtDetailRange.Text = _selectedAircraft.RangeText;
            TxtDetailValue.Text = _selectedAircraft.ValueText;
            TxtDetailPassengers.Text = _selectedAircraft.PassengersText;

            LoadAircraftDetails();
            UpdateCrewSection();
            UpdateAircraftIcon();
        }

        private void LoadAircraftDetails()
        {
            if (_selectedAircraft == null) return;

            try
            {
                _selectedAircraftModel = _aircraftRepository.GetAircraftById(_selectedAircraft.Id);

                if (_selectedAircraftModel == null) return;

                var catalogEntry = _catalogRepository.GetAircraftByTitle(_selectedAircraftModel.Variant);
                TxtDetailCrew.Text = catalogEntry != null && catalogEntry.CrewCount > 0 ? $"{catalogEntry.CrewCount}" : "—";

                TxtDetailCargo.Text = $"{_selectedAircraftModel.MaxCargoKg:F0} kg";
                TxtDetailSpeed.Text = $"{_selectedAircraftModel.CruiseSpeedKts:F0} kts";
                TxtDetailFuel.Text = $"{_selectedAircraftModel.FuelCapacityGal:F0} gal";
                TxtDetailFuelBurn.Text = $"{_selectedAircraftModel.FuelBurnGalPerHour:F1} gal/h";
                TxtDetailServiceCeiling.Text = _selectedAircraftModel.ServiceCeilingFt > 0 ? $"{_selectedAircraftModel.ServiceCeilingFt:N0} ft" : "—";
                TxtDetailOperatingCost.Text = $"{_selectedAircraftModel.HourlyOperatingCost:N0} €/h";

                TxtDetailHoursSinceMaint.Text = $"{_selectedAircraftModel.HoursSinceLastMaintenance:F1} h";
                TxtDetailHoursSinceMaint.Foreground = GetMaintenanceWarningBrush(_selectedAircraftModel.HoursSinceLastMaintenance);
                TxtDetailLastMaint.Text = _selectedAircraftModel.LastMaintenanceDate.ToString("dd.MM.yyyy");

                if (_selectedAircraftModel.Status == AircraftStatus.Maintenance && _selectedAircraftModel.MaintenanceCompletionDate.HasValue)
                {
                    MaintenanceCompletionRow.Visibility = Visibility.Visible;
                    TxtDetailMaintComplete.Text = _selectedAircraftModel.MaintenanceCompletionDate.Value.ToString("dd.MM.yyyy");
                    BtnMaintenance.Content = "In Progress";
                    BtnMaintenance.IsEnabled = _selectedAircraftModel.MaintenanceCompletionDate.Value <= DateTime.Today;
                }
                else
                {
                    MaintenanceCompletionRow.Visibility = Visibility.Collapsed;
                    BtnMaintenance.Content = "Schedule";
                    BtnMaintenance.IsEnabled = _selectedAircraftModel.Status != AircraftStatus.InFlight;
                }

                if (_selectedAircraftModel.AssignedFBOId.HasValue)
                {
                    var fbo = _fboRepository.GetFBOById(_selectedAircraftModel.AssignedFBOId.Value);
                    TxtDetailFBOAssignment.Text = fbo != null ? $"{fbo.ICAO} - {fbo.AirportName}" : "Unknown";
                    BtnRemoveFBO.IsEnabled = _selectedAircraftModel.Status.ToString().StartsWith("Stationed");
                }
                else
                {
                    TxtDetailFBOAssignment.Text = "Not assigned";
                    BtnRemoveFBO.IsEnabled = false;
                }

                LoadAvailableFBOs();
            }
            catch (Exception ex)
            {
                _logger.Error($"HangarView: Error loading aircraft details: {ex.Message}");
            }
        }

        private Brush GetMaintenanceWarningBrush(double hoursSinceMaintenance)
        {
            if (hoursSinceMaintenance >= 100)
                return new SolidColorBrush(Color.FromRgb(239, 68, 68));
            if (hoursSinceMaintenance >= 75)
                return new SolidColorBrush(Color.FromRgb(251, 146, 60));
            return new SolidColorBrush(Color.FromRgb(34, 197, 94));
        }

        private void LoadAvailableFBOs()
        {
            if (_selectedAircraftModel == null) return;

            try
            {
                var fbos = _fboRepository.GetAllFBOs().OrderBy(f => f.ICAO).ToList();

                var aircraftSize = AircraftSizeExtensions.GetAircraftSize(_selectedAircraftModel.MaxPassengers);
                var requiredRunway = aircraftSize.GetRequiredRunwayLengthFt();
                var requiredTerminal = aircraftSize.GetRequiredTerminalSize();

                _availableFBOs.Clear();
                foreach (var fbo in fbos)
                {
                    bool runwayOk = fbo.RunwayLengthFt == 0 || fbo.RunwayLengthFt >= requiredRunway;
                    bool terminalOk = fbo.TerminalSize >= requiredTerminal;
                    bool isCompatible = runwayOk && terminalOk;

                    if (isCompatible)
                    {
                        _availableFBOs.Add(new FBOSelectionItem(
                            fbo.Id, fbo.ICAO, fbo.AirportName,
                            fbo.RunwayLengthFt, fbo.TerminalSize, true));
                    }
                }

                CmbFBOSelection.ItemsSource = _availableFBOs;
                BtnAssignFBO.IsEnabled = false;
            }
            catch (Exception ex)
            {
                _logger.Error($"HangarView: Error loading FBOs: {ex.Message}");
            }
        }

        private void UpdateAircraftIcon()
        {
            if (_selectedAircraft == null || _selectedAircraftModel == null) return;

            var imagePath = _imageService.GetImagePath(_selectedAircraftModel.CustomImagePath, _selectedAircraft.SizeCategory);

            if (File.Exists(imagePath))
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(imagePath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    DetailAircraftImage.Source = bitmap;
                    DetailAircraftImage.Visibility = Visibility.Visible;
                    DetailAircraftIcon.Visibility = Visibility.Collapsed;
                    return;
                }
                catch (Exception ex)
                {
                    _logger.Debug($"HangarView: Failed to load aircraft image: {ex.Message}");
                }
            }

            DetailAircraftImage.Source = null;
            DetailAircraftImage.Visibility = Visibility.Collapsed;

            var iconConverter = new SizeToAircraftIconConverter();
            var brushConverter = new SizeToBrushConverter();

            var geometry = iconConverter.Convert(_selectedAircraft.SizeCategory, typeof(Geometry), string.Empty, CultureInfo.CurrentCulture) as Geometry;
            var brush = brushConverter.Convert(_selectedAircraft.SizeCategory, typeof(Brush), string.Empty, CultureInfo.CurrentCulture) as Brush;

            DetailAircraftIcon.Data = geometry;
            DetailAircraftIcon.Fill = brush;
            DetailAircraftIcon.Visibility = Visibility.Visible;
        }

        private void EditImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAircraftModel == null) return;

            var aircraftName = $"{_selectedAircraftModel.Registration} - {_selectedAircraftModel.Type} {_selectedAircraftModel.Variant}";
            var dialog = new SelectAircraftImageDialog(
                aircraftName,
                _selectedAircraftModel.Registration,
                _selectedAircraft?.SizeCategory ?? "Medium",
                _selectedAircraftModel.CustomImagePath,
                _imageService,
                _logger);
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true && dialog.ImageChanged)
            {
                try
                {
                    _selectedAircraftModel.CustomImagePath = dialog.NewImagePath;
                    _aircraftRepository.UpdateAircraft(_selectedAircraftModel);
                    _viewModel.LoadAircraft();
                    RefreshSelectedAircraft();
                }
                catch (Exception ex)
                {
                    _logger.Error($"HangarView: Error saving custom image: {ex.Message}");
                    InfoDialog.Show("Error", $"Failed to save image: {ex.Message}", Window.GetWindow(this));
                }
            }
        }

        private void OnAircraftListItemClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is AircraftViewModel aircraft)
            {
                SelectAircraft(aircraft);
            }
        }

        private void AircraftListHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _listExpanded = !_listExpanded;
            AircraftListScrollViewer.Visibility = _listExpanded ? Visibility.Visible : Visibility.Collapsed;
            TxtExpandIcon.Text = _listExpanded ? "▼" : "▶";
        }

        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            TxtFilterHint.Visibility = string.IsNullOrEmpty(TxtFilter.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;

            UpdateStats();
        }

        private void CmbStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel == null)
                return;

            if (CmbStatusFilter?.SelectedItem is ComboBoxItem item)
            {
                _viewModel.SelectedStatus = item.Content?.ToString() ?? "All";
            }

            UpdateStats();
        }

        private void EditSpecsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAircraftModel == null) return;

            var aircraftName = $"{_selectedAircraftModel.Registration} - {_selectedAircraftModel.Type} {_selectedAircraftModel.Variant}";
            var dialog = new EditAircraftSpecsDialog(
                _selectedAircraftModel.Id,
                aircraftName,
                _selectedAircraftModel.MaxPassengers,
                _selectedAircraftModel.MaxCargoKg,
                _selectedAircraftModel.CruiseSpeedKts,
                _selectedAircraftModel.MaxRangeNM,
                _selectedAircraftModel.FuelCapacityGal,
                _selectedAircraftModel.FuelBurnGalPerHour,
                _selectedAircraftModel.ServiceCeilingFt,
                _selectedAircraftModel.HourlyOperatingCost,
                _selectedAircraftModel.TotalFlightHours,
                _logger,
                _aircraftRepository);
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true && dialog.Saved)
            {
                _viewModel.LoadAircraft();
                UpdateStats();
                RefreshSelectedAircraft();
            }
        }

        private void MaintenanceButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAircraftModel == null) return;

            var owner = Window.GetWindow(this);

            if (_selectedAircraftModel.Status == AircraftStatus.Maintenance)
            {
                if (_selectedAircraftModel.MaintenanceCompletionDate.HasValue &&
                    _selectedAircraftModel.MaintenanceCompletionDate.Value <= DateTime.Today)
                {
                    CompleteMaintenance(owner);
                }
                return;
            }

            ScheduleMaintenance(owner);
        }

        private void ScheduleMaintenance(Window owner)
        {
            if (_selectedAircraftModel == null) return;

            var maintenanceCost = _selectedAircraftModel.CurrentValue * 0.02m;

            if (_financeService.Balance < maintenanceCost)
            {
                InfoDialog.Show("Insufficient Funds", $"Maintenance cost: {maintenanceCost:N0} €\nYour balance: {_financeService.Balance:N0} €", owner);
                return;
            }

            var confirmed = ConfirmDialog.Show(
                "Schedule Maintenance",
                $"Schedule maintenance for {_selectedAircraftModel.Registration}?\n\nCost: {maintenanceCost:N0} €\nDuration: 7 days",
                owner);

            if (!confirmed) return;

            try
            {
                _financeService.AddExpense(maintenanceCost, $"Maintenance: {_selectedAircraftModel.Registration}");

                _selectedAircraftModel.LastMaintenanceDate = DateTime.Today;
                _selectedAircraftModel.HoursSinceLastMaintenance = 0;
                _selectedAircraftModel.Status = AircraftStatus.Maintenance;
                _selectedAircraftModel.MaintenanceCompletionDate = DateTime.Today.AddDays(7);
                _aircraftRepository.UpdateAircraft(_selectedAircraftModel);

                _logger.Info($"HangarView: Scheduled maintenance for {_selectedAircraftModel.Registration}");

                _viewModel.LoadAircraft();
                UpdateStats();
                RefreshSelectedAircraft();
            }
            catch (Exception ex)
            {
                _logger.Error($"HangarView: Error scheduling maintenance: {ex.Message}");
                InfoDialog.Show("Error", $"Failed to schedule maintenance: {ex.Message}", owner);
            }
        }

        private void CompleteMaintenance(Window owner)
        {
            if (_selectedAircraftModel == null) return;

            try
            {
                _selectedAircraftModel.Status = AircraftStatus.Available;
                _selectedAircraftModel.MaintenanceCompletionDate = null;
                _aircraftRepository.UpdateAircraft(_selectedAircraftModel);

                _logger.Info($"HangarView: Completed maintenance for {_selectedAircraftModel.Registration}");

                _viewModel.LoadAircraft();
                UpdateStats();
                RefreshSelectedAircraft();
            }
            catch (Exception ex)
            {
                _logger.Error($"HangarView: Error completing maintenance: {ex.Message}");
                InfoDialog.Show("Error", $"Failed to complete maintenance: {ex.Message}", owner);
            }
        }

        private void CmbFBOSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnAssignFBO.IsEnabled = CmbFBOSelection.SelectedItem != null &&
                                     _selectedAircraftModel?.Status == AircraftStatus.Available;
        }

        private void AssignFBOButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAircraftModel == null || CmbFBOSelection.SelectedItem is not FBOSelectionItem selectedFBO) return;

            var owner = Window.GetWindow(this);

            try
            {
                _selectedAircraftModel.AssignedFBOId = selectedFBO.Id;
                _selectedAircraftModel.Status = AircraftStatus.Stationed;
                _selectedAircraftModel.HomeBase = selectedFBO.ICAO;
                _aircraftRepository.UpdateAircraft(_selectedAircraftModel);

                _logger.Info($"HangarView: Assigned {_selectedAircraftModel.Registration} to FBO {selectedFBO.ICAO}");

                _viewModel.LoadAircraft();
                UpdateStats();
                RefreshSelectedAircraft();
            }
            catch (Exception ex)
            {
                _logger.Error($"HangarView: Error assigning FBO: {ex.Message}");
                InfoDialog.Show("Error", $"Failed to assign FBO: {ex.Message}", owner);
            }
        }

        private void RemoveFBOButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAircraftModel == null) return;

            var owner = Window.GetWindow(this);

            try
            {
                _selectedAircraftModel.AssignedFBOId = null;
                _selectedAircraftModel.Status = AircraftStatus.Available;
                _aircraftRepository.UpdateAircraft(_selectedAircraftModel);

                _logger.Info($"HangarView: Removed {_selectedAircraftModel.Registration} from FBO assignment");

                _viewModel.LoadAircraft();
                UpdateStats();
                RefreshSelectedAircraft();
            }
            catch (Exception ex)
            {
                _logger.Error($"HangarView: Error removing FBO: {ex.Message}");
                InfoDialog.Show("Error", $"Failed to remove FBO assignment: {ex.Message}", owner);
            }
        }

        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAircraft == null) return;

            var owner = Window.GetWindow(this);

            var confirmed = ConfirmDialog.Show(
                "Sell Aircraft",
                $"Do you want to sell {_selectedAircraft.Registration} ({_selectedAircraft.TypeVariant}) for {_selectedAircraft.ValueText}?",
                owner);

            if (!confirmed)
                return;

            var (success, message) = _viewModel.SellAircraft(_selectedAircraft);

            if (success)
            {
                HideDetails();
                _selectedAircraft = null;
                UpdateStats();
            }
            else
            {
                InfoDialog.Show("Error", $"Failed to sell aircraft: {message}", owner);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAircraft == null) return;

            var owner = Window.GetWindow(this);

            var confirmed = ConfirmDialog.Show(
                "Delete Aircraft",
                $"Do you want to permanently delete {_selectedAircraft.Registration} ({_selectedAircraft.TypeVariant})?\n\nThis will remove it without any refund.",
                owner);

            if (!confirmed)
                return;

            var (success, message) = _viewModel.DeleteAircraft(_selectedAircraft);

            if (success)
            {
                HideDetails();
                _selectedAircraft = null;
                UpdateStats();
            }
            else
            {
                InfoDialog.Show("Error", $"Failed to delete aircraft: {message}", owner);
            }
        }

        private void UpdateCrewSection()
        {
            if (_selectedAircraft == null) return;

            TxtCrewStatus.Text = _selectedAircraft.CrewStatusText;
            CrewStatusBadge.Background = _selectedAircraft.CrewStatusColor;

            CrewPilotList.ItemsSource = _selectedAircraft.AssignedPilots
                .Select(p => new { p.Id, p.Name })
                .ToList();

            PilotSelectionGrid.Visibility = Visibility.Collapsed;
        }

        private void AssignPilotButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAircraft == null || _selectedAircraftModel == null) return;

            try
            {
                var employedPilots = _pilotRepository.GetEmployedPilots();

                var availablePilots = new List<PilotSelectionItem>();
                foreach (var pilot in employedPilots)
                {
                    if (_assignmentRepository.IsPilotAssigned(pilot.Id))
                        continue;

                    var typeRatings = _typeRatingRepository.GetTypeRatingsByPilotId(pilot.Id);
                    var ratingStrings = typeRatings.Select(tr => tr.AircraftType).ToList();
                    if (!TypeRatingMatchHelper.HasMatchingTypeRating(_selectedAircraftModel.Type, ratingStrings))
                        continue;

                    availablePilots.Add(new PilotSelectionItem(pilot.Id, pilot.Name));
                }

                if (availablePilots.Count == 0)
                {
                    InfoDialog.Show("No Pilots Available",
                        "No available pilots with matching type rating found.",
                        Window.GetWindow(this));
                    return;
                }

                CmbPilotSelection.ItemsSource = availablePilots;
                CmbPilotSelection.SelectedIndex = -1;
                PilotSelectionGrid.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                _logger.Error($"HangarView: Error loading available pilots: {ex.Message}");
                InfoDialog.Show("Error", $"Failed to load available pilots: {ex.Message}", Window.GetWindow(this));
            }
        }

        private void ConfirmAssignPilotButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAircraft == null || CmbPilotSelection.SelectedItem is not PilotSelectionItem selectedPilot) return;

            try
            {
                _assignmentRepository.AssignPilot(_selectedAircraft.Id, selectedPilot.Id);
                _logger.Info($"HangarView: Assigned pilot {selectedPilot.DisplayName} to aircraft {_selectedAircraft.Registration}");

                _viewModel.LoadAircraft();
                UpdateStats();
                RefreshSelectedAircraft();
            }
            catch (Exception ex)
            {
                _logger.Error($"HangarView: Error assigning pilot: {ex.Message}");
                InfoDialog.Show("Error", $"Failed to assign pilot: {ex.Message}", Window.GetWindow(this));
            }
        }

        private void RemovePilotButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.Tag is not int pilotId) return;

            try
            {
                _assignmentRepository.UnassignPilot(pilotId);
                _logger.Info($"HangarView: Unassigned pilot {pilotId} from aircraft {_selectedAircraft?.Registration}");

                _viewModel.LoadAircraft();
                UpdateStats();
                RefreshSelectedAircraft();
            }
            catch (Exception ex)
            {
                _logger.Error($"HangarView: Error removing pilot: {ex.Message}");
                InfoDialog.Show("Error", $"Failed to remove pilot: {ex.Message}", Window.GetWindow(this));
            }
        }

        private void RefreshSelectedAircraft()
        {
            if (_selectedAircraft == null) return;

            var refreshed = _viewModel.FilteredAircraft.FirstOrDefault(a => a.Id == _selectedAircraft.Id);
            if (refreshed != null)
            {
                _selectedAircraft = refreshed;
                UpdateDetailsPanel();
            }
            else
            {
                HideDetails();
                _selectedAircraft = null;
            }
        }
    }

    public class PilotSelectionItem
    {
        public int Id { get; }
        public string DisplayName { get; }

        public PilotSelectionItem(int id, string name)
        {
            Id = id;
            DisplayName = name;
        }
    }
}
