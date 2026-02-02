using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Ace.App.Controls;
using Ace.App.Models;
using AircraftModel = Ace.App.Models.Aircraft;
using Ace.App.Services;
using Ace.App.Interfaces;
using Ace.App.Views.Dialogs;
using Ace.App.Views.FBO;
using Ace.App.ViewModels;
using Ace.App.Infrastructure;
using Ace.App.Helpers;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Projections;

namespace Ace.App.Views.Core
{
    public partial class FlightplanView : UserControl
    {
        private ObservableCollection<AircraftSelectionViewModel> _aircraft = new ObservableCollection<AircraftSelectionViewModel>();
        private ObservableCollection<SuggestedRoute> _suggestedRoutes = new ObservableCollection<SuggestedRoute>();
        private ObservableCollection<IcaoSelectionItem> _departureIcaoItems = new ObservableCollection<IcaoSelectionItem>();
        private ObservableCollection<IcaoSelectionItem> _arrivalIcaoItems = new ObservableCollection<IcaoSelectionItem>();
        private bool _isSettingDepartureFromSelection;
        private bool _isSettingArrivalFromSelection;
        private bool _isSettingFromMapSelection;
        private bool _suggestedRoutesExpanded = true;
        private bool _isMeasuringDistance;
        private MPoint? _measureStartPoint;

        private readonly ILoggingService _logger;
        private readonly IAirportDatabase _airportDatabase;
        private readonly IActiveFlightPlanService _activeFlightPlanService;
        private readonly ISettingsService _settingsService;
        private readonly IRouteSuggestionService _routeSuggestionService;
        private readonly IPricingService _pricingService;
        private readonly SimConnectService _simConnectService;
        private readonly FlightPlanMapViewModel _mapViewModel;
        private readonly IFBORepository _fboRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly ITypeRatingRepository _typeRatingRepository;
        private readonly IAircraftPilotAssignmentRepository _assignmentRepository;

        public FlightplanView(
            ILoggingService logger,
            IAirportDatabase airportDatabase,
            IActiveFlightPlanService activeFlightPlanService,
            ISettingsService settingsService,
            IRouteSuggestionService routeSuggestionService,
            IPricingService pricingService,
            SimConnectService simConnectService,
            FlightPlanMapViewModel mapViewModel,
            IFBORepository fboRepository,
            IAircraftRepository aircraftRepository,
            ITypeRatingRepository typeRatingRepository,
            IAircraftPilotAssignmentRepository assignmentRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _airportDatabase = airportDatabase ?? throw new ArgumentNullException(nameof(airportDatabase));
            _activeFlightPlanService = activeFlightPlanService ?? throw new ArgumentNullException(nameof(activeFlightPlanService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _routeSuggestionService = routeSuggestionService ?? throw new ArgumentNullException(nameof(routeSuggestionService));
            _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
            _simConnectService = simConnectService ?? throw new ArgumentNullException(nameof(simConnectService));
            _mapViewModel = mapViewModel ?? throw new ArgumentNullException(nameof(mapViewModel));
            _fboRepository = fboRepository ?? throw new ArgumentNullException(nameof(fboRepository));
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));
            _typeRatingRepository = typeRatingRepository ?? throw new ArgumentNullException(nameof(typeRatingRepository));
            _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));

            InitializeComponent();
            Loaded += FlightplanView_Loaded;
            Unloaded += FlightplanView_Unloaded;
            SuggestedRoutesListBox.ItemsSource = _suggestedRoutes;
            DepartureIcaoComboBox.ItemsSource = _departureIcaoItems;
            ArrivalIcaoComboBox.ItemsSource = _arrivalIcaoItems;

            // Setup map
            if (_mapViewModel.Map != null)
            {
                FlightPlanMapControl.Map = _mapViewModel.Map;
            }
            FlightPlanMapControl.MouseRightButtonUp += OnMapRightClick;
            FlightPlanMapControl.MouseMove += OnMapMouseMove;
            FlightPlanMapControl.MouseLeave += OnMapMouseLeave;
        }

        private void FlightplanView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFBOs();
            LoadAircraft();
            RestoreActiveFlightPlan();
            SetupIcaoComboBoxEvents();

            // Sync current ICAO values to map after everything is loaded
            SyncCurrentRouteToMap();
            UpdateRangeCircle();
            ZoomToRangeCircleIfAvailable();

            // Initialize map controls
            ScaleControl.AttachToMap(FlightPlanMapControl.Map);
            InitializeMapLegend();
            UpdateMapLayerButtons();

            var currentLayer = Enum.TryParse<MapLayerType>(_settingsService.CurrentSettings.MapLayer, out var layer)
                ? layer
                : MapLayerType.Street;
            UpdateLegendVisibility(currentLayer);
            MapAttribution.SetMapLayer(currentLayer);

            // Initialize layer visibility from settings
            _mapViewModel?.SetAirspaceVisibility(_settingsService.CurrentSettings.ShowAirspaceOverlay);
            LegendPanel.ShowAirspaceLegend(_settingsService.CurrentSettings.ShowAirspaceOverlay);
            if (!_settingsService.CurrentSettings.ShowAirports)
            {
                _mapViewModel?.SetAirportVisibility(false);
            }

            // Initialize legend expanded state from settings
            LegendPanel.IsExpanded = _settingsService.CurrentSettings.MapLegendExpanded;
            LegendPanel.ExpandedStateChanged += OnLegendExpandedStateChanged;

            InitializeAirspaceFilterCheckboxes();
        }

        private void FlightplanView_Unloaded(object sender, RoutedEventArgs e)
        {
            _mapViewModel?.SaveMapPosition();
            LegendPanel.ExpandedStateChanged -= OnLegendExpandedStateChanged;
        }

        private void OnLegendExpandedStateChanged(bool isExpanded)
        {
            _settingsService.CurrentSettings.MapLegendExpanded = isExpanded;
            _settingsService.Save();
        }

        private void InitializeMapLegend()
        {
            var routeEntries = new List<MapLegendEntry>
            {
                new("Departure", new SolidColorBrush(System.Windows.Media.Color.FromRgb(76, 175, 80))),
                new("Arrival", new SolidColorBrush(System.Windows.Media.Color.FromRgb(233, 30, 99))),
                new("Route", new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 152, 0)), true, true)
            };
            LegendPanel.SetRouteLegend(routeEntries);

            var fboEntries = new List<MapLegendEntry>
            {
                new("Your FBOs", new SolidColorBrush(System.Windows.Media.Color.FromRgb(88, 166, 255)))
            };
            LegendPanel.SetFBOLegend(fboEntries);

            var runwayEntries = new List<MapLegendEntry>
            {
                new("< 2,000 ft", new SolidColorBrush(System.Windows.Media.Color.FromRgb(144, 202, 249))),
                new("< 4,000 ft", new SolidColorBrush(System.Windows.Media.Color.FromRgb(100, 181, 246))),
                new("< 6,000 ft", new SolidColorBrush(System.Windows.Media.Color.FromRgb(66, 165, 245))),
                new("< 8,000 ft", new SolidColorBrush(System.Windows.Media.Color.FromRgb(33, 150, 243))),
                new("8,000+ ft", new SolidColorBrush(System.Windows.Media.Color.FromRgb(156, 39, 176)))
            };
            LegendPanel.SetRunwayLegend(runwayEntries);
        }

        private void SyncCurrentRouteToMap()
        {
            var departureIcao = GetDepartureIcao();
            var arrivalIcao = GetArrivalIcao();

            if (departureIcao.Length == 4)
            {
                _mapViewModel?.SetDepartureIcao(departureIcao);
            }
            if (arrivalIcao.Length == 4)
            {
                _mapViewModel?.SetArrivalIcao(arrivalIcao);
            }
        }

        private void LoadFBOs()
        {
            try
            {
                var fbos = _fboRepository.GetAllFBOs().OrderBy(f => f.ICAO).ToList();

                _departureIcaoItems.Clear();
                _arrivalIcaoItems.Clear();

                foreach (var fbo in fbos)
                {
                    var item = new IcaoSelectionItem(fbo.ICAO, fbo.AirportName, true);
                    _departureIcaoItems.Add(item);
                    _arrivalIcaoItems.Add(new IcaoSelectionItem(fbo.ICAO, fbo.AirportName, true));
                }

                _logger.Info($"FlightplanView: Loaded {fbos.Count} FBOs for ICAO selection");
            }
            catch (Exception ex)
            {
                _logger.Error("FlightplanView: Error loading FBOs", ex);
            }
        }

        private void SetupIcaoComboBoxEvents()
        {
            DepartureIcaoTextBox.TextChanged += DepartureIcaoTextBox_TextChanged;
            ArrivalIcaoTextBox.TextChanged += ArrivalIcaoTextBox_TextChanged;
            DepartureIcaoTextBox.MouseDoubleClick += IcaoTextBox_MouseDoubleClick;
            ArrivalIcaoTextBox.MouseDoubleClick += IcaoTextBox_MouseDoubleClick;
            DepartureIcaoComboBox.SelectionChanged += DepartureIcaoComboBox_SelectionChanged;
            ArrivalIcaoComboBox.SelectionChanged += ArrivalIcaoComboBox_SelectionChanged;

            UpdateRoutePreview();
        }

        private void IcaoTextBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is not TextBox textBox) return;
            var icao = textBox.Text?.Trim().ToUpperInvariant();
            if (string.IsNullOrEmpty(icao) || icao.Length != 4) return;

            var airport = _airportDatabase.GetAirport(icao);
            if (airport == null) return;

            _mapViewModel?.ZoomToAirport(airport.Latitude, airport.Longitude);
            _logger.Info($"FlightplanView: Zoomed to airport {icao} on double-click");
        }

        private void DepartureIcaoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isSettingDepartureFromSelection) return;
            if (DepartureIcaoComboBox.SelectedItem is not IcaoSelectionItem selectedItem) return;

            var icao = selectedItem.Icao;
            _logger.Info($"FlightplanView: FBO selected as departure: {icao}");

            _isSettingDepartureFromSelection = true;
            DepartureIcaoTextBox.Text = icao;
            DepartureIcaoComboBox.SelectedItem = null;
            _isSettingDepartureFromSelection = false;

            UpdateSuggestedRoutesWithDeparture(icao);
        }

        private void ArrivalIcaoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isSettingArrivalFromSelection) return;
            if (ArrivalIcaoComboBox.SelectedItem is not IcaoSelectionItem selectedItem) return;

            var icao = selectedItem.Icao;
            _logger.Info($"FlightplanView: FBO selected as arrival: {icao}");

            _isSettingArrivalFromSelection = true;
            ArrivalIcaoTextBox.Text = icao;
            ArrivalIcaoComboBox.SelectedItem = null;
            _isSettingArrivalFromSelection = false;
        }

        private string GetDepartureIcao()
        {
            return DepartureIcaoTextBox.Text?.Trim().ToUpper() ?? string.Empty;
        }

        private string GetArrivalIcao()
        {
            return ArrivalIcaoTextBox.Text?.Trim().ToUpper() ?? string.Empty;
        }

        private void SetDepartureIcao(string icao)
        {
            DepartureIcaoTextBox.Text = icao;
        }

        private void SetArrivalIcao(string icao)
        {
            ArrivalIcaoTextBox.Text = icao;
        }

        private void RestoreActiveFlightPlan()
        {
            var activePlan = _activeFlightPlanService.GetActivePlan();

            if (activePlan == null)
            {
                _logger.Debug("FlightplanView: No active flight plan to restore");
                return;
            }

            if (activePlan.IsCompleted)
            {
                _logger.Info($"FlightplanView: Active flight plan is completed, clearing for new route");
                _activeFlightPlanService.ClearFlightPlan();
                return;
            }

            _logger.Info($"FlightplanView: Restoring active flight plan {activePlan.DepartureIcao} → {activePlan.ArrivalIcao}");

            var aircraft = _aircraft.FirstOrDefault(a => a.Registration == activePlan.AircraftRegistration);
            if (aircraft != null)
            {
                AircraftComboBox.SelectedItem = aircraft;
            }

            SetDepartureIcao(activePlan.DepartureIcao);
            SetArrivalIcao(activePlan.ArrivalIcao);

            _mapViewModel?.SetDepartureIcao(activePlan.DepartureIcao);
            _mapViewModel?.SetArrivalIcao(activePlan.ArrivalIcao);

            var departureAirport = _airportDatabase.GetAirport(activePlan.DepartureIcao);
            var arrivalAirport = _airportDatabase.GetAirport(activePlan.ArrivalIcao);

            if (departureAirport != null && arrivalAirport != null && aircraft != null)
            {
                var greenBrush = new SolidColorBrush(Color.FromRgb(34, 197, 94));
                var cruiseSpeed = aircraft.CruiseSpeedKts > 0 ? aircraft.CruiseSpeedKts : 120.0;
                var flightTimeHours = activePlan.DistanceNM / cruiseSpeed;
                var hours = (int)flightTimeHours;
                var minutes = (int)((flightTimeHours - hours) * 60);
                var timeDisplay = $"{hours}h {minutes}m";

                var aircraftEntity = _aircraftRepository.GetAircraftByRegistration(activePlan.AircraftRegistration);
                decimal totalRevenue = 0m;
                if (aircraftEntity != null)
                {
                    var priceBreakdown = _pricingService.CalculateFlightPrice(aircraftEntity, activePlan.DistanceNM, activePlan.Passengers, flightTimeHours);
                    totalRevenue = priceBreakdown.TotalRevenue;
                }

                var bearing = _airportDatabase.CalculateBearing(departureAirport, arrivalAirport);
                var courseDisplay = $"{bearing:F0}°";

                DistanceText.Text = $"{activePlan.DistanceNM:F0} NM";
                DistanceText.Foreground = greenBrush;
                CourseText.Text = courseDisplay;
                CourseText.Foreground = greenBrush;
                EstimatedTimeText.Text = timeDisplay;
                EstimatedTimeText.Foreground = greenBrush;
                RouteRevenueText.Text = $"${totalRevenue:N0}";
                RouteInfoBadge.Visibility = Visibility.Visible;

                RouteText.Text = $"{activePlan.DepartureIcao} → {activePlan.ArrivalIcao}";
                RouteDistanceText.Text = $"{activePlan.DistanceNM:F0} NM";
                RouteCourseText.Text = courseDisplay;
                RouteTimeText.Text = timeDisplay;
                SelectedAircraftText.Text = $"{aircraft.Registration} ({aircraft.TypeVariant})";

                RouteDetailsPanel.Visibility = Visibility.Visible;
                FlightPlanDetailsPanel.Visibility = Visibility.Visible;

                UpdateSuggestedRoutesWithDeparture(activePlan.DepartureIcao);
                _logger.Info("FlightplanView: Active flight plan restored successfully");
            }
        }

        private void LoadSavedIcaos()
        {
            var settings = _settingsService.CurrentSettings;

            if (!string.IsNullOrEmpty(settings.LastDepartureIcao))
            {
                SetDepartureIcao(settings.LastDepartureIcao);
                _logger.Info($"FlightplanView: Loaded saved departure ICAO: {settings.LastDepartureIcao}");
            }

            if (!string.IsNullOrEmpty(settings.LastArrivalIcao))
            {
                SetArrivalIcao(settings.LastArrivalIcao);
                _logger.Info($"FlightplanView: Loaded saved arrival ICAO: {settings.LastArrivalIcao}");
            }
        }

        private void LoadAircraft()
        {
            try
            {
                _logger.Info("FlightplanView: Loading aircraft from hangar");

                var allowAll = _settingsService.CurrentSettings.AllowAllAircraftForFlightPlan;
                var aircraft = allowAll
                    ? _aircraftRepository.GetAllAircraft()
                    : _aircraftRepository.GetAircraftByStatus(AircraftStatus.Available);

                _logger.Database($"Aircraft query completed (AllowAll={allowAll})", aircraft.Count);

                _aircraft.Clear();

                foreach (var ac in aircraft)
                {
                    var assignment = _assignmentRepository.GetAssignmentsByAircraftId(ac.Id).FirstOrDefault();
                    if (assignment != null)
                    {
                        var pilotRatings = _typeRatingRepository.GetTypeRatingsByPilotId(assignment.PilotId)
                            .Select(tr => tr.AircraftType)
                            .ToList();

                        if (!TypeRatingMatchHelper.HasMatchingTypeRating(ac.Type, pilotRatings))
                        {
                            _logger.Debug($"FlightplanView: Skipping aircraft {ac.Registration} ({ac.Type}) - pilot {assignment.PilotId} has no matching type rating");
                            continue;
                        }
                    }

                    _logger.Debug($"Adding aircraft to selection: {ac.Registration}");
                    _aircraft.Add(new AircraftSelectionViewModel(ac));
                }

                AircraftComboBox.ItemsSource = _aircraft;

                if (_aircraft.Any())
                {
                    var savedRegistration = _settingsService.CurrentSettings.LastSelectedAircraftRegistration;
                    var savedAircraft = !string.IsNullOrEmpty(savedRegistration)
                        ? _aircraft.FirstOrDefault(a => a.Registration == savedRegistration)
                        : null;

                    if (savedAircraft != null)
                    {
                        AircraftComboBox.SelectedItem = savedAircraft;
                        _logger.Info($"FlightplanView: Restored saved aircraft selection: {savedRegistration}");
                    }
                    else
                    {
                        AircraftComboBox.SelectedIndex = 0;
                    }
                    _logger.Info($"FlightplanView: Loaded {_aircraft.Count} available aircraft");

                    LoadSavedIcaos();

                    AircraftComboBox.SelectionChanged += AircraftComboBox_SelectionChanged;

                    var departureIcao = GetDepartureIcao();
                    if (!string.IsNullOrEmpty(departureIcao) && departureIcao.Length == 4)
                    {
                        var airport = _airportDatabase.GetAirport(departureIcao);
                        if (airport != null)
                        {
                            UpdateSuggestedRoutesWithDeparture(departureIcao);
                        }
                        else
                        {
                            UpdateSuggestedRoutes();
                        }
                    }
                    else
                    {
                        UpdateSuggestedRoutes();
                    }
                }
                else
                {
                    _logger.Warn("FlightplanView: No available aircraft found");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("FlightplanView: Error loading aircraft", ex);
            }
        }

        private void CreateFlightPlanButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAircraft = AircraftComboBox.SelectedItem as AircraftSelectionViewModel;
            var departureIcao = GetDepartureIcao();
            var arrivalIcao = GetArrivalIcao();

            if (selectedAircraft == null)
            {
                _logger.Warn("FlightplanView: No aircraft selected");
                InfoDialog.Show("Flight Plan", "Please select an aircraft.", Application.Current.MainWindow);
                return;
            }

            if (string.IsNullOrEmpty(departureIcao) || departureIcao.Length != 4)
            {
                _logger.Warn($"FlightplanView: Invalid departure ICAO: {departureIcao}");
                InfoDialog.Show("Flight Plan", "Please enter a valid 4-letter departure ICAO code.", Application.Current.MainWindow);
                return;
            }

            if (string.IsNullOrEmpty(arrivalIcao) || arrivalIcao.Length != 4)
            {
                _logger.Warn($"FlightplanView: Invalid arrival ICAO: {arrivalIcao}");
                InfoDialog.Show("Flight Plan", "Please enter a valid 4-letter arrival ICAO code.", Application.Current.MainWindow);
                return;
            }

            _logger.Info($"FlightplanView: Creating flight plan - Aircraft: {selectedAircraft.Registration}, Route: {departureIcao} -> {arrivalIcao}");

            var departureAirport = _airportDatabase.GetAirport(departureIcao);
            var arrivalAirport = _airportDatabase.GetAirport(arrivalIcao);

            bool isValid = true;
            var greenBrush = new SolidColorBrush(Color.FromRgb(34, 197, 94));
            var redBrush = new SolidColorBrush(Color.FromRgb(239, 68, 68));

            if (departureAirport == null)
            {
                _logger.Warn($"FlightplanView: Departure airport not found: {departureIcao}");
                DistanceText.Text = "Invalid Departure";
                DistanceText.Foreground = redBrush;
                EstimatedTimeText.Text = "N/A";
                EstimatedTimeText.Foreground = redBrush;
                isValid = false;
            }

            if (arrivalAirport == null)
            {
                _logger.Warn($"FlightplanView: Arrival airport not found: {arrivalIcao}");
                if (isValid)
                {
                    DistanceText.Text = "Invalid Arrival";
                    EstimatedTimeText.Text = "N/A";
                }
                DistanceText.Foreground = redBrush;
                EstimatedTimeText.Foreground = redBrush;
                isValid = false;
            }

            if (isValid && departureAirport != null && arrivalAirport != null)
            {
                var distanceNM = _airportDatabase.CalculateDistanceBetweenAirports(departureAirport, arrivalAirport);
                var cruiseSpeed = selectedAircraft.CruiseSpeedKts > 0 ? selectedAircraft.CruiseSpeedKts : 120.0;
                var flightTimeHours = distanceNM / cruiseSpeed;
                var hours = (int)flightTimeHours;
                var minutes = (int)((flightTimeHours - hours) * 60);

                _logger.Info($"FlightplanView: Route calculation - Distance: {distanceNM:F1} NM, Cruise Speed: {cruiseSpeed:F0} kts, Flight Time: {hours}h {minutes}m");

                var aircraft = _aircraftRepository.GetAircraftById(selectedAircraft.Id);
                double maxCargoKg = aircraft?.MaxCargoKg ?? 0;
                int passengers = selectedAircraft.MaxPassengers;

                var priceBreakdown = aircraft != null
                    ? _pricingService.CalculateFlightPrice(aircraft, distanceNM, passengers, flightTimeHours)
                    : null;

                var bearing = _airportDatabase.CalculateBearing(departureAirport, arrivalAirport);
                var courseDisplay = $"{bearing:F0}°";

                DistanceText.Text = $"{distanceNM:F1} NM";
                DistanceText.Foreground = greenBrush;
                CourseText.Text = courseDisplay;
                CourseText.Foreground = greenBrush;
                EstimatedTimeText.Text = $"{hours}h {minutes}m";
                EstimatedTimeText.Foreground = greenBrush;
                RouteRevenueText.Text = priceBreakdown != null ? $"${priceBreakdown.TotalRevenue:N0}" : "$0";
                RouteInfoBadge.Visibility = Visibility.Visible;

                RouteDistanceText.Text = $"{distanceNM:F1} NM";
                RouteCourseText.Text = courseDisplay;
                RouteTimeText.Text = $"{hours}h {minutes}m";

                _activeFlightPlanService.ActivateFlightPlan(departureIcao, arrivalIcao, selectedAircraft.Id, selectedAircraft.Registration, selectedAircraft.TypeVariant, distanceNM, passengers, maxCargoKg, false);
                _logger.Info($"FlightplanView: Created combined flight plan - {passengers} PAX, {maxCargoKg:F0} kg cargo");

                var settings = _settingsService.CurrentSettings;
                settings.LastDepartureIcao = departureIcao;
                settings.LastArrivalIcao = arrivalIcao;
                settings.LastSelectedAircraftRegistration = selectedAircraft.Registration;
                _settingsService.Save();
                _logger.Info($"FlightplanView: Saved ICAOs and aircraft to settings - {departureIcao} → {arrivalIcao}, Aircraft: {selectedAircraft.Registration}");
            }

            SelectedAircraftText.Text = $"{selectedAircraft.Registration} ({selectedAircraft.TypeVariant})";
            RouteText.Text = $"{departureIcao} → {arrivalIcao}";

            RouteDetailsPanel.Visibility = Visibility.Visible;
            FlightPlanDetailsPanel.Visibility = Visibility.Visible;

            _logger.Info($"FlightplanView: Flight plan {(isValid ? "created and activated successfully" : "created with errors - not activated")}");
        }

        private void CompleteFlightPlanButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_activeFlightPlanService.HasValidFlightPlan())
            {
                _logger.Warn("FlightplanView: No active flight plan to complete");
                InfoDialog.Show("Flight Plan", "No active flight plan to complete.", Application.Current.MainWindow);
                return;
            }

            var activePlan = _activeFlightPlanService.GetActivePlan();
            var confirmed = ConfirmDialog.Show(
                "Complete Flight Plan",
                $"Complete flight plan {activePlan?.DepartureIcao} → {activePlan?.ArrivalIcao}?\n\n" +
                "This will use estimated values for distance, time, and earnings.",
                Application.Current.MainWindow,
                "Yes", "No");

            if (confirmed)
            {
                _logger.Info("FlightplanView: User confirmed manual flight completion");
                _simConnectService.ManuallyCompleteFlightPlan();

                FlightPlanDetailsPanel.Visibility = Visibility.Collapsed;
                RouteDetailsPanel.Visibility = Visibility.Collapsed;
                RouteInfoBadge.Visibility = Visibility.Collapsed;
            }
        }

        private void AircraftComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRangeCircle();

            var departureIcao = GetDepartureIcao();
            if (!string.IsNullOrEmpty(departureIcao) && departureIcao.Length == 4)
            {
                var airport = _airportDatabase.GetAirport(departureIcao);
                if (airport != null)
                {
                    UpdateSuggestedRoutesWithDeparture(departureIcao);
                    UpdateRoutePreview();
                    return;
                }
            }
            UpdateSuggestedRoutes();
            UpdateRoutePreview();
        }

        private void DepartureIcaoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var departureIcao = GetDepartureIcao();

            // Sync to map if not coming from map selection
            if (!_isSettingFromMapSelection && departureIcao.Length == 4)
            {
                _mapViewModel?.SetDepartureIcao(departureIcao);
                UpdateRangeCircle();
            }

            if (departureIcao.Length != 4)
            {
                UpdateSuggestedRoutes();
                UpdateRoutePreview();
                UpdateRangeCircle();
                return;
            }

            var airport = _airportDatabase.GetAirport(departureIcao);
            if (airport == null)
            {
                _logger.Debug($"FlightplanView: Invalid departure ICAO entered: {departureIcao}");
                UpdateSuggestedRoutes();
                UpdateRoutePreview();
                UpdateRangeCircle();
                return;
            }

            _logger.Info($"FlightplanView: Valid departure ICAO entered: {departureIcao}, updating route suggestions");
            UpdateSuggestedRoutesWithDeparture(departureIcao);
            UpdateRoutePreview();
            UpdateRangeCircle();
        }

        private void ArrivalIcaoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var arrivalIcao = GetArrivalIcao();

            // Sync to map if not coming from map selection
            if (!_isSettingFromMapSelection && arrivalIcao.Length == 4)
            {
                _mapViewModel?.SetArrivalIcao(arrivalIcao);
            }

            UpdateRoutePreview();
        }

        private void UpdateRoutePreview()
        {
            var departureIcao = GetDepartureIcao();
            var arrivalIcao = GetArrivalIcao();
            var selectedAircraft = AircraftComboBox.SelectedItem as AircraftSelectionViewModel;

            if (string.IsNullOrEmpty(departureIcao) || departureIcao.Length != 4 ||
                string.IsNullOrEmpty(arrivalIcao) || arrivalIcao.Length != 4 ||
                selectedAircraft == null)
            {
                RouteDetailsPanel.Visibility = Visibility.Collapsed;
                RouteInfoBadge.Visibility = Visibility.Collapsed;
                RangeWarningPanel.Visibility = Visibility.Collapsed;
                return;
            }

            var departureAirport = _airportDatabase.GetAirport(departureIcao);
            var arrivalAirport = _airportDatabase.GetAirport(arrivalIcao);

            if (departureAirport == null || arrivalAirport == null)
            {
                RouteDetailsPanel.Visibility = Visibility.Collapsed;
                RouteInfoBadge.Visibility = Visibility.Collapsed;
                RangeWarningPanel.Visibility = Visibility.Collapsed;
                return;
            }

            var distanceNM = _airportDatabase.CalculateDistanceBetweenAirports(departureAirport, arrivalAirport);

            var aircraft = _aircraftRepository.GetAircraftById(selectedAircraft.Id);
            decimal passengerRevenue = 0m;
            decimal cargoRevenue = 0m;
            decimal totalRevenue = 0m;
            double maxCargoKg = 0;
            var cruiseSpeed = 120.0;
            var flightHours = 0.0;

            if (aircraft != null)
            {
                cruiseSpeed = aircraft.CruiseSpeedKts > 0 ? aircraft.CruiseSpeedKts : 120.0;
                flightHours = distanceNM / cruiseSpeed;
                maxCargoKg = aircraft.MaxCargoKg;
                var priceBreakdown = _pricingService.CalculateFlightPrice(aircraft, distanceNM, selectedAircraft.MaxPassengers, flightHours);
                passengerRevenue = priceBreakdown.PassengerRevenue;
                cargoRevenue = priceBreakdown.CargoRevenue;
                totalRevenue = priceBreakdown.TotalRevenue;
            }

            var hours = (int)flightHours;
            var minutes = (int)((flightHours - hours) * 60);
            var timeDisplay = $"{hours}h {minutes}m";

            var bearing = _airportDatabase.CalculateBearing(departureAirport, arrivalAirport);
            var courseDisplay = $"{bearing:F0}°";

            var greenBrush = new SolidColorBrush(Color.FromRgb(34, 197, 94));
            DistanceText.Text = $"{distanceNM:F0} NM";
            DistanceText.Foreground = greenBrush;
            CourseText.Text = courseDisplay;
            CourseText.Foreground = greenBrush;
            EstimatedTimeText.Text = timeDisplay;
            EstimatedTimeText.Foreground = greenBrush;
            RouteRevenueText.Text = $"${totalRevenue:N0}";
            RouteInfoBadge.Visibility = Visibility.Visible;

            RouteText.Text = $"{departureIcao} → {arrivalIcao}";
            RouteDistanceText.Text = $"{distanceNM:F0} NM";
            RouteCourseText.Text = courseDisplay;
            RouteTimeText.Text = timeDisplay;
            SelectedAircraftText.Text = $"{selectedAircraft.Registration} ({selectedAircraft.TypeVariant})";

            PassengerLoadText.Text = $"{selectedAircraft.MaxPassengers} PAX";
            PassengerRevenueText.Text = $"${passengerRevenue:N0}";

            CargoLoadText.Text = $"{maxCargoKg:F0} kg";
            CargoRevenueText.Text = $"${cargoRevenue:N0}";

            TotalRevenueText.Text = $"${totalRevenue:N0}";

            if (selectedAircraft.MaxRangeNM > 0 && distanceNM > selectedAircraft.MaxRangeNM)
            {
                RangeWarningText.Text = $"Route exceeds aircraft range ({distanceNM:F0} NM > {selectedAircraft.MaxRangeNM:F0} NM)";
                RangeWarningPanel.Visibility = Visibility.Visible;
            }
            else
            {
                RangeWarningPanel.Visibility = Visibility.Collapsed;
            }

            RouteDetailsPanel.Visibility = Visibility.Visible;
        }

        private void UpdateSuggestedRoutes()
        {
            _suggestedRoutes.Clear();
            ShowRoutesHint();
            _logger.Debug("FlightplanView: Routes cleared, showing hint - no valid departure ICAO");
        }

        private void ShowRoutesHint()
        {
            NoRoutesHint.Visibility = Visibility.Visible;
            SuggestedRoutesListBox.Visibility = Visibility.Collapsed;
            SuggestedRoutesCount.Text = "(0)";
        }

        private void ShowRoutesList()
        {
            NoRoutesHint.Visibility = Visibility.Collapsed;
            SuggestedRoutesListBox.Visibility = Visibility.Visible;
            SuggestedRoutesCount.Text = $"({_suggestedRoutes.Count})";
        }

        private void UpdateSuggestedRoutesWithDeparture(string departureIcao)
        {
            _suggestedRoutes.Clear();

            var selectedAircraft = AircraftComboBox.SelectedItem as AircraftSelectionViewModel;
            if (selectedAircraft == null)
            {
                _logger.Debug("FlightplanView: No aircraft selected for route suggestions");
                ShowRoutesHint();
                return;
            }

            var aircraft = _aircraftRepository.GetAircraftById(selectedAircraft.Id);
            if (aircraft == null)
            {
                _logger.Warn($"FlightplanView: Aircraft {selectedAircraft.Id} not found for route suggestions");
                ShowRoutesHint();
                return;
            }

            _logger.Debug($"FlightplanView: Updating route suggestions with departure ICAO: {departureIcao}");

            var suggestions = _routeSuggestionService.GetSuggestedRoutes(aircraft, departureIcao);

            foreach (var suggestion in suggestions)
            {
                _suggestedRoutes.Add(suggestion);
            }

            if (_suggestedRoutes.Count > 0)
            {
                ShowRoutesList();
            }
            else
            {
                ShowRoutesHint();
            }

            _logger.Info($"FlightplanView: Loaded {_suggestedRoutes.Count} route suggestions from {departureIcao}");
        }

        private void SuggestedRoutesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void RouteItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement element) return;
            var selectedRoute = element.DataContext as SuggestedRoute;

            if (selectedRoute == null) return;

            _logger.Info($"FlightplanView: Route selected - {selectedRoute.DepartureIcao} → {selectedRoute.ArrivalIcao}");

            SetDepartureIcao(selectedRoute.DepartureIcao);
            SetArrivalIcao(selectedRoute.ArrivalIcao);
        }

        private void SwapIcaoButton_Click(object sender, RoutedEventArgs e)
        {
            var departureIcao = GetDepartureIcao();
            var arrivalIcao = GetArrivalIcao();

            _logger.Info($"FlightplanView: Swapping ICAOs - {departureIcao} <-> {arrivalIcao}");

            _isSettingDepartureFromSelection = true;
            _isSettingArrivalFromSelection = true;
            try
            {
                SetDepartureIcao(arrivalIcao);
                SetArrivalIcao(departureIcao);

                if (arrivalIcao.Length == 4)
                {
                    _mapViewModel?.SetDepartureIcao(arrivalIcao);
                }
                if (departureIcao.Length == 4)
                {
                    _mapViewModel?.SetArrivalIcao(departureIcao);
                }
            }
            finally
            {
                _isSettingDepartureFromSelection = false;
                _isSettingArrivalFromSelection = false;
            }

            if (arrivalIcao.Length == 4)
            {
                var airport = _airportDatabase.GetAirport(arrivalIcao);
                if (airport != null)
                {
                    UpdateSuggestedRoutesWithDeparture(arrivalIcao);
                }
            }

            UpdateRoutePreview();
        }

        private void SuggestedRoutesHeader_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _suggestedRoutesExpanded = !_suggestedRoutesExpanded;
            SuggestedRoutesContent.Visibility = _suggestedRoutesExpanded ? Visibility.Visible : Visibility.Collapsed;
            RoutesToggleIcon.Text = _suggestedRoutesExpanded ? "▼" : "▶";
            _logger.Debug($"FlightplanView: Suggested routes panel {(_suggestedRoutesExpanded ? "expanded" : "collapsed")}");
        }

        private void DepartureInfoButton_Click(object sender, RoutedEventArgs e)
        {
            var icao = GetDepartureIcao();
            if (string.IsNullOrEmpty(icao) || icao.Length != 4)
            {
                InfoDialog.Show("Airport Info", "Please enter a valid 4-letter departure ICAO first.", Application.Current.MainWindow);
                return;
            }

            _logger.Info($"FlightplanView: Opening airport info for departure {icao}");
            var dialog = new AirportInfoDialog(icao, _airportDatabase, _logger)
            {
                Owner = Window.GetWindow(this)
            };
            dialog.ShowDialog();
        }

        private void ArrivalInfoButton_Click(object sender, RoutedEventArgs e)
        {
            var icao = GetArrivalIcao();
            if (string.IsNullOrEmpty(icao) || icao.Length != 4)
            {
                InfoDialog.Show("Airport Info", "Please enter a valid 4-letter arrival ICAO first.", Application.Current.MainWindow);
                return;
            }

            _logger.Info($"FlightplanView: Opening airport info for arrival {icao}");
            var dialog = new AirportInfoDialog(icao, _airportDatabase, _logger)
            {
                Owner = Window.GetWindow(this)
            };
            dialog.ShowDialog();
        }

        // Map Layer Switching
        private void StreetLayerButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchMapLayer(MapLayerType.Street);
        }

        private void TerrainLayerButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchMapLayer(MapLayerType.Terrain);
        }

        private void SatelliteLayerButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchMapLayer(MapLayerType.Satellite);
        }

        private void SwitchMapLayer(MapLayerType layerType)
        {
            _settingsService.CurrentSettings.MapLayer = layerType.ToString();
            _settingsService.Save();

            _mapViewModel?.RefreshMapLayer(layerType);

            UpdateMapLayerButtons();
            UpdateLegendVisibility(layerType);
            MapAttribution.SetMapLayer(layerType);

            _logger.Info($"FlightplanView: Switched to {layerType} map layer");
        }

        private void UpdateMapLayerButtons()
        {
            var currentLayer = Enum.TryParse<MapLayerType>(_settingsService.CurrentSettings.MapLayer, out var layer)
                ? layer
                : MapLayerType.Street;

            StreetLayerButton.Opacity = currentLayer == MapLayerType.Street ? 1.0 : 0.6;
            TerrainLayerButton.Opacity = currentLayer == MapLayerType.Terrain ? 1.0 : 0.6;
            SatelliteLayerButton.Opacity = currentLayer == MapLayerType.Satellite ? 1.0 : 0.6;

            var airspaceEnabled = _settingsService.CurrentSettings.ShowAirspaceOverlay;
            AirspaceLayerButton.Opacity = airspaceEnabled ? 1.0 : 0.6;

            var airportsEnabled = _settingsService.CurrentSettings.ShowAirports;
            AirportsLayerButton.Opacity = airportsEnabled ? 1.0 : 0.6;
        }

        private void AirportsLayerButton_Click(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;
            settings.ShowAirports = !settings.ShowAirports;
            _settingsService.Save();

            _mapViewModel?.SetAirportVisibility(settings.ShowAirports);
            AirportsLayerButton.Opacity = settings.ShowAirports ? 1.0 : 0.6;

            _logger.Info($"FlightplanView: Airport markers toggled to {settings.ShowAirports}");
        }

        private void AirspaceLayerButton_Click(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;
            settings.ShowAirspaceOverlay = !settings.ShowAirspaceOverlay;
            _settingsService.Save();

            _mapViewModel?.SetAirspaceVisibility(settings.ShowAirspaceOverlay);
            AirspaceLayerButton.Opacity = settings.ShowAirspaceOverlay ? 1.0 : 0.6;
            LegendPanel.ShowAirspaceLegend(settings.ShowAirspaceOverlay);

            _logger.Info($"FlightplanView: Airspace overlay toggled to {settings.ShowAirspaceOverlay}");
        }

        private void AirspaceFilterButton_Click(object sender, RoutedEventArgs e)
        {
            AirspaceFilterPopup.IsOpen = !AirspaceFilterPopup.IsOpen;
        }

        private void InitializeAirspaceFilterCheckboxes()
        {
            var settings = _settingsService.CurrentSettings;
            ChkAirspaceA.IsChecked = settings.ShowAirspaceClassA;
            ChkAirspaceB.IsChecked = settings.ShowAirspaceClassB;
            ChkAirspaceC.IsChecked = settings.ShowAirspaceClassC;
            ChkAirspaceD.IsChecked = settings.ShowAirspaceClassD;
            ChkAirspaceE.IsChecked = settings.ShowAirspaceClassE;
            ChkAirspaceCTR.IsChecked = settings.ShowAirspaceCTR;
            ChkAirspaceRestricted.IsChecked = settings.ShowAirspaceRestricted;
            ChkAirspaceProhibited.IsChecked = settings.ShowAirspaceProhibited;
            ChkAirspaceDanger.IsChecked = settings.ShowAirspaceDanger;
            ChkAirspaceGlider.IsChecked = settings.ShowAirspaceGlider;
            ChkAirspaceOther.IsChecked = settings.ShowAirspaceOther;
        }

        private void AirspaceFilter_Changed(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.CurrentSettings;
            settings.ShowAirspaceClassA = ChkAirspaceA.IsChecked == true;
            settings.ShowAirspaceClassB = ChkAirspaceB.IsChecked == true;
            settings.ShowAirspaceClassC = ChkAirspaceC.IsChecked == true;
            settings.ShowAirspaceClassD = ChkAirspaceD.IsChecked == true;
            settings.ShowAirspaceClassE = ChkAirspaceE.IsChecked == true;
            settings.ShowAirspaceCTR = ChkAirspaceCTR.IsChecked == true;
            settings.ShowAirspaceRestricted = ChkAirspaceRestricted.IsChecked == true;
            settings.ShowAirspaceProhibited = ChkAirspaceProhibited.IsChecked == true;
            settings.ShowAirspaceDanger = ChkAirspaceDanger.IsChecked == true;
            settings.ShowAirspaceGlider = ChkAirspaceGlider.IsChecked == true;
            settings.ShowAirspaceOther = ChkAirspaceOther.IsChecked == true;
            _settingsService.Save();

            _mapViewModel?.UpdateAirspaceFilter();
        }

        private void UpdateLegendVisibility(MapLayerType layerType)
        {
            LegendPanel.ShowTerrainLegend(layerType == MapLayerType.Terrain);
        }

        private void UpdateRangeCircle()
        {
            if (_mapViewModel == null) return;

            var selectedAircraft = AircraftComboBox.SelectedItem as AircraftSelectionViewModel;
            if (selectedAircraft != null)
            {
                _mapViewModel.UpdateRangeCircle((int)selectedAircraft.MaxRangeNM);
            }
            else
            {
                _mapViewModel.UpdateRangeCircle(null);
            }
        }

        private void ZoomToRangeCircleIfAvailable()
        {
            if (_mapViewModel == null) return;

            var selectedAircraft = AircraftComboBox.SelectedItem as AircraftSelectionViewModel;
            if (selectedAircraft != null && selectedAircraft.MaxRangeNM > 0)
            {
                _mapViewModel.ZoomToRangeCircle((int)selectedAircraft.MaxRangeNM);
            }
        }

        // Map integration methods
        private void OnMapRightClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(FlightPlanMapControl);
            var screenPosition = new Mapsui.Manipulations.ScreenPosition(pos.X, pos.Y);
            var layers = FlightPlanMapControl.Map?.Layers.Where(l => l.Name != "RangeCircle").ToList();
            if (layers == null || layers.Count == 0) return;

            var mapInfo = FlightPlanMapControl.GetMapInfo(screenPosition, layers);
            if (mapInfo?.Feature == null) return;

            var icao = mapInfo.Feature["icao"]?.ToString();
            if (string.IsNullOrEmpty(icao)) return;

            var airport = _airportDatabase.GetAirport(icao);
            if (airport == null) return;

            // Create and show context menu
            var contextMenu = new ContextMenu();

            var setDepartureItem = new MenuItem { Header = $"Set {icao} as Departure" };
            setDepartureItem.Click += (s, args) =>
            {
                _isSettingFromMapSelection = true;
                try
                {
                    SetDepartureIcao(icao);
                    _mapViewModel?.SetDepartureIcao(icao);
                    UpdateRangeCircle();
                    _logger.Info($"FlightplanView: Departure set from map context menu: {icao}");
                }
                finally
                {
                    _isSettingFromMapSelection = false;
                }
            };
            contextMenu.Items.Add(setDepartureItem);

            var setArrivalItem = new MenuItem { Header = $"Set {icao} as Arrival" };
            setArrivalItem.Click += (s, args) =>
            {
                _isSettingFromMapSelection = true;
                try
                {
                    SetArrivalIcao(icao);
                    _mapViewModel?.SetArrivalIcao(icao);
                    _logger.Info($"FlightplanView: Arrival set from map context menu: {icao}");
                }
                finally
                {
                    _isSettingFromMapSelection = false;
                }
            };
            contextMenu.Items.Add(setArrivalItem);

            contextMenu.Items.Add(new Separator());

            var infoItem = new MenuItem { Header = $"Airport Info: {icao}" };
            infoItem.Click += (s, args) => ShowAirportInfo(icao);
            contextMenu.Items.Add(infoItem);

            contextMenu.IsOpen = true;
        }

        private void ShowAirportInfo(string icao)
        {
            var detailView = new AirportDetailView(_airportDatabase, _logger, icao);

            var window = new Window
            {
                Title = $"Airport Info - {icao}",
                Content = detailView,
                Width = 700,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Window.GetWindow(this)
            };
            window.ShowDialog();
        }

        private void OnMapMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!_settingsService.CurrentSettings.ShowAirspaceOverlay)
            {
                AirspaceTooltip.Visibility = Visibility.Collapsed;
                return;
            }

            var pos = e.GetPosition(FlightPlanMapControl);
            var screenPosition = new Mapsui.Manipulations.ScreenPosition(pos.X, pos.Y);
            var airspaceInfo = _mapViewModel?.GetAirspaceAtPosition(screenPosition);

            if (airspaceInfo != null)
            {
                AirspaceTooltipName.Text = airspaceInfo.Value.Name;
                AirspaceTooltipClass.Text = $"Class: {airspaceInfo.Value.Class}";
                AirspaceTooltipAltitude.Text = $"{airspaceInfo.Value.LowerAltitude} - {airspaceInfo.Value.UpperAltitude}";

                var containerPos = e.GetPosition(this);
                AirspaceTooltip.Margin = new Thickness(containerPos.X + 15, containerPos.Y + 15, 0, 0);
                AirspaceTooltip.Visibility = Visibility.Visible;
            }
            else
            {
                AirspaceTooltip.Visibility = Visibility.Collapsed;
            }
        }

        private void OnMapMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            AirspaceTooltip.Visibility = Visibility.Collapsed;
        }

        private void MeasureDistanceButton_Click(object sender, RoutedEventArgs e)
        {
            _isMeasuringDistance = !_isMeasuringDistance;
            _measureStartPoint = null;

            if (_isMeasuringDistance)
            {
                MeasureDistanceButton.Opacity = 1.0;
                MapHintText.Text = "Click on map to set first measurement point";
                FlightPlanMapControl.MouseLeftButtonUp += OnMapLeftClickForMeasure;
                _mapViewModel?.ClearMeasurementLine();
                MeasureDistancePanel.Visibility = Visibility.Collapsed;
                _logger.Info("FlightplanView: Distance measurement mode enabled");
            }
            else
            {
                MeasureDistanceButton.Opacity = 0.6;
                MapHintText.Text = "Right-click on airport to set as Departure or Arrival";
                FlightPlanMapControl.MouseLeftButtonUp -= OnMapLeftClickForMeasure;
                _mapViewModel?.ClearMeasurementLine();
                MeasureDistancePanel.Visibility = Visibility.Collapsed;
                _logger.Info("FlightplanView: Distance measurement mode disabled");
            }
        }

        private void OnMapLeftClickForMeasure(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!_isMeasuringDistance) return;

            var pos = e.GetPosition(FlightPlanMapControl);
            var worldPosition = FlightPlanMapControl.Map?.Navigator.Viewport.ScreenToWorld(pos.X, pos.Y);

            if (worldPosition == null) return;

            if (_measureStartPoint == null)
            {
                _measureStartPoint = worldPosition;
                MapHintText.Text = "Click on map to set second measurement point";
                _logger.Debug($"FlightplanView: Measurement start point set at {worldPosition.X:F0}, {worldPosition.Y:F0}");
            }
            else
            {
                var startLonLat = SphericalMercator.ToLonLat(_measureStartPoint.X, _measureStartPoint.Y);
                var endLonLat = SphericalMercator.ToLonLat(worldPosition.X, worldPosition.Y);

                var distanceNM = CalculateDistanceNM(startLonLat.lat, startLonLat.lon, endLonLat.lat, endLonLat.lon);
                var distanceKm = distanceNM * 1.852;

                _mapViewModel?.DrawMeasurementLine(_measureStartPoint, worldPosition);

                MeasureDistanceText.Text = $"Distance: {distanceNM:F1} NM ({distanceKm:F1} km)";
                MeasureDistancePanel.Visibility = Visibility.Visible;

                MapHintText.Text = "Click on map to start new measurement";
                _measureStartPoint = null;

                _logger.Info($"FlightplanView: Measured distance: {distanceNM:F1} NM");
            }
        }

        private void ClearMeasurement_Click(object sender, RoutedEventArgs e)
        {
            _measureStartPoint = null;
            _mapViewModel?.ClearMeasurementLine();
            MeasureDistancePanel.Visibility = Visibility.Collapsed;
            MapHintText.Text = _isMeasuringDistance
                ? "Click on map to set first measurement point"
                : "Right-click on airport to set as Departure or Arrival";
        }

        private static double CalculateDistanceNM(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadiusNM = 3440.065;
            var dLat = (lat2 - lat1) * Math.PI / 180.0;
            var dLon = (lon2 - lon1) * Math.PI / 180.0;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EarthRadiusNM * c;
        }
    }

    public class IcaoSelectionItem
    {
        public string Icao { get; }
        public string AirportName { get; }
        public bool IsFBO { get; }
        public string DisplayText => IsFBO ? $"★ {Icao} - {AirportName}" : Icao;

        public IcaoSelectionItem(string icao, string airportName, bool isFBO)
        {
            Icao = icao;
            AirportName = airportName;
            IsFBO = isFBO;
        }

        public override string ToString() => DisplayText;
    }

    public class AircraftSelectionViewModel
    {
        private readonly AircraftModel _aircraft;

        public AircraftSelectionViewModel(AircraftModel aircraft)
        {
            _aircraft = aircraft;
        }

        public int Id => _aircraft.Id;
        public string Registration => _aircraft.Registration;
        public string TypeVariant => $"{_aircraft.Type} - {_aircraft.Variant}";
        public string DisplayName => $"{_aircraft.Registration} - {_aircraft.Type} {_aircraft.Variant}";
        public double CruiseSpeedKts => _aircraft.CruiseSpeedKts;
        public int MaxPassengers => _aircraft.MaxPassengers;
        public double MaxRangeNM => _aircraft.MaxRangeNM;
    }
}
