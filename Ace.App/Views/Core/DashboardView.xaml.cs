using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Ace.App.Models;
using Ace.App.Services;
using Ace.App.Interfaces;
using Ace.App.Infrastructure;
using Ace.App.Views.Dialogs;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace Ace.App.Views.Core
{
    public partial class DashboardView : UserControl
    {
        private readonly FlightData _viewData = new FlightData();
        private readonly IActiveFlightPlanService _activeFlightPlanService;
        private readonly IFinanceService _financeService;
        private readonly ILoggingService _logger;
        private readonly IAirportDatabase _airportDatabase;
        private readonly IFBORepository _fboRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IPilotRepository _pilotRepository;
        private readonly ISoundService _soundService;
        private readonly ISettingsService _settingsService;
        private string? _lastKnownFlightPlanId;
        private bool _isLoaded;
        private bool _todSoundPlayed;
        private bool _fuelLowSoundPlayed;
        private bool _stallWarningSoundPlaying;

        public DashboardView(
            IActiveFlightPlanService activeFlightPlanService,
            IFinanceService financeService,
            ILoggingService logger,
            IAirportDatabase airportDatabase,
            IFBORepository fboRepository,
            IAircraftRepository aircraftRepository,
            IPilotRepository pilotRepository,
            ISoundService soundService,
            ISettingsService settingsService)
        {
            _activeFlightPlanService = activeFlightPlanService ?? throw new ArgumentNullException(nameof(activeFlightPlanService));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _airportDatabase = airportDatabase ?? throw new ArgumentNullException(nameof(airportDatabase));
            _fboRepository = fboRepository ?? throw new ArgumentNullException(nameof(fboRepository));
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));
            _pilotRepository = pilotRepository ?? throw new ArgumentNullException(nameof(pilotRepository));
            _soundService = soundService ?? throw new ArgumentNullException(nameof(soundService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

            InitializeComponent();
            this.DataContext = _viewData;

            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.SimConnectService.ConnectionChanged += OnConnectionChanged;
                mw.SimConnectService.FlightDataReceived += OnFlightDataReceived;
            }

            _activeFlightPlanService.FlightPlanChanged += OnFlightPlanChanged;
            _financeService.PropertyChanged += OnFinancePropertyChanged;
            _viewData.Balance = _financeService.Balance;

            Unloaded += DashboardView_Unloaded;

            var activePlan = _activeFlightPlanService.GetActivePlan();
            OnFlightPlanChanged(activePlan);

            LoadStatistics();
        }

        private void DashboardView_Unloaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.SimConnectService.ConnectionChanged -= OnConnectionChanged;
                mw.SimConnectService.FlightDataReceived -= OnFlightDataReceived;
            }

            _activeFlightPlanService.FlightPlanChanged -= OnFlightPlanChanged;
            _financeService.PropertyChanged -= OnFinancePropertyChanged;
        }

        private void DashboardView_Loaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;

            if (Application.Current.MainWindow is MainWindow mw)
            {
                var isConnected = mw.SimConnectService.IsConnected;
                _logger.Debug($"DashboardView loaded, SimConnect status: {isConnected}");
                _viewData.IsConnected = isConnected;
            }

            var activePlan = _activeFlightPlanService.GetActivePlan();
            if (activePlan != null)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    InitializeAltitudeProfileChart();
                }), System.Windows.Threading.DispatcherPriority.Render);
            }
        }

        private void OnConnectionChanged(bool isConnected)
        {
            Dispatcher.Invoke(() =>
            {
                _viewData.IsConnected = isConnected;
                if (!isConnected)
                {
                    _viewData.Aircraft = "---";
                    _viewData.GroundSpeed = 0;
                    _viewData.Altitude = 0;
                }
            });
        }

        private void OnFlightDataReceived(FlightData newData)
        {
            Dispatcher.Invoke(() =>
            {
                _viewData.IsConnected = true;
                _viewData.Aircraft = newData.Aircraft;
                _viewData.Latitude = newData.Latitude;
                _viewData.Longitude = newData.Longitude;
                _viewData.Altitude = newData.Altitude;
                _viewData.GroundSpeed = newData.GroundSpeed;
                _viewData.Heading = newData.Heading;
                _viewData.VerticalSpeed = newData.VerticalSpeed;
                _viewData.SimulationRate = newData.SimulationRate;
                _viewData.FuelQuantityGallons = newData.FuelQuantityGallons;
                _viewData.FuelCapacityGallons = newData.FuelCapacityGallons;
                _viewData.FuelFlowGallonsPerHour = newData.FuelFlowGallonsPerHour;
                _viewData.AngleOfAttack = newData.AngleOfAttack;
                _viewData.StallWarning = newData.StallWarning;
                _viewData.OnGround = newData.OnGround;

                HandleStallWarning(newData.StallWarning);
                _viewData.FlightPhase = newData.FlightPhase;
                _viewData.CurrentAirport = newData.CurrentAirport;
                _viewData.DestinationAirport = newData.DestinationAirport;

                UpdateFlightProgress(newData.Latitude, newData.Longitude);
            });
        }

        private void UpdateFlightProgress(double currentLat, double currentLon)
        {
            var flightPlan = _activeFlightPlanService.GetActivePlan();
            if (flightPlan == null) return;

            var arrivalAirport = _airportDatabase.GetAirport(flightPlan.ArrivalIcao);
            if (arrivalAirport == null) return;

            // Calculate distance from current position to arrival
            double distanceToArrival = CalculateDistanceNM(currentLat, currentLon, arrivalAirport.Latitude, arrivalAirport.Longitude);

            // Calculate current bearing to destination
            _viewData.CourseToDest = CalculateBearing(currentLat, currentLon, arrivalAirport.Latitude, arrivalAirport.Longitude);

            // Calculate progress percentage
            double totalDistance = flightPlan.DistanceNM;
            if (totalDistance > 0)
            {
                double distanceCovered = totalDistance - distanceToArrival;
                double progress = Math.Max(0, Math.Min(100, (distanceCovered / totalDistance) * 100));

                _viewData.FlightProgress = progress;
                _viewData.DistanceRemaining = Math.Max(0, distanceToArrival);

                if (_viewData.ShouldStartDescent && !_todSoundPlayed)
                {
                    _soundService.PlayTopOfDescent();
                    _todSoundPlayed = true;
                    _logger.Debug("Top of Descent reached - sound played");
                }

                if (_viewData.FuelLow && !_fuelLowSoundPlayed)
                {
                    _soundService.PlayWarning();
                    _fuelLowSoundPlayed = true;
                    _logger.Debug("Low fuel warning - sound played");
                }
                else if (!_viewData.FuelLow && _fuelLowSoundPlayed)
                {
                    _fuelLowSoundPlayed = false;
                }

                // Track altitude for profile chart
                TrackAltitudePoint(distanceCovered, _viewData.Altitude);
            }
        }

        private void HandleStallWarning(bool stallWarning)
        {
            if (stallWarning && !_stallWarningSoundPlaying)
            {
                _soundService.PlayStallWarning();
                _stallWarningSoundPlaying = true;
                _logger.Debug("Stall warning - sound played");
            }
            else if (!stallWarning && _stallWarningSoundPlaying)
            {
                _stallWarningSoundPlaying = false;
            }
        }

        private void TrackAltitudePoint(double distanceCovered, double altitude)
        {
            _activeFlightPlanService.AddAltitudePoint(distanceCovered, altitude, _viewData.GroundSpeed, _viewData.VerticalSpeed);
            UpdateAltitudeProfileChart();
        }

        private void UpdateAltitudeProfileChart()
        {
            var flightPlan = _activeFlightPlanService.GetActivePlan();
            if (flightPlan == null) return;

            var altitudeTrack = _activeFlightPlanService.GetAltitudeTrack();
            double maxAltitude = _activeFlightPlanService.GetMaxAltitudeRecorded();
            double totalDistance = flightPlan.DistanceNM;

            double plannedCruise = _activeFlightPlanService.GetPlannedCruiseAltitude();
            if (plannedCruise == 0 || maxAltitude > plannedCruise)
            {
                plannedCruise = Math.Max(maxAltitude, 3000);
                _activeFlightPlanService.SetPlannedCruiseAltitude(plannedCruise);
            }
            double cruiseAltitude = plannedCruise;

            var model = new PlotModel
            {
                Background = OxyColors.Transparent,
                PlotAreaBorderColor = OxyColors.Transparent,
                PlotMargins = new OxyThickness(40, 4, 8, 4)
            };

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = totalDistance,
                IsAxisVisible = false,
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = cruiseAltitude * 1.2,
                IsAxisVisible = true,
                AxislineColor = OxyColor.FromRgb(139, 148, 158),
                AxislineStyle = LineStyle.Solid,
                AxislineThickness = 1,
                TextColor = OxyColor.FromRgb(139, 148, 158),
                TicklineColor = OxyColor.FromRgb(139, 148, 158),
                MajorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColor.FromArgb(40, 139, 148, 158),
                MinorGridlineStyle = LineStyle.None,
                FontSize = 10,
                StringFormat = "N0",
                Unit = "ft",
                AxisTitleDistance = 8
            });

            var idealLine = new LineSeries
            {
                Color = OxyColor.FromArgb(180, 150, 150, 150),
                StrokeThickness = 1.5,
                LineStyle = LineStyle.Dash,
                TrackerFormatString = ""
            };

            double climbEnd = totalDistance * 0.1;
            double descentStart = totalDistance * 0.9;

            idealLine.Points.Add(new DataPoint(0, 0));
            idealLine.Points.Add(new DataPoint(climbEnd, cruiseAltitude));
            idealLine.Points.Add(new DataPoint(descentStart, cruiseAltitude));
            idealLine.Points.Add(new DataPoint(totalDistance, 0));
            model.Series.Add(idealLine);

            if (altitudeTrack.Count > 0)
            {
                var actualLine = new LineSeries
                {
                    Color = OxyColor.FromRgb(0, 200, 255),
                    StrokeThickness = 2,
                    TrackerFormatString = "",
                    Tag = altitudeTrack
                };

                foreach (var point in altitudeTrack)
                {
                    actualLine.Points.Add(new DataPoint(point.DistanceNM, point.AltitudeFt));
                }
                model.Series.Add(actualLine);
            }

            _viewData.AltitudeProfileModel = model;
            model.InvalidatePlot(true);
        }

        private void InitializeAltitudeProfileChart()
        {
            var flightPlan = _activeFlightPlanService.GetActivePlan();
            if (flightPlan == null) return;

            var altitudeTrack = _activeFlightPlanService.GetAltitudeTrack();
            double maxAltitude = _activeFlightPlanService.GetMaxAltitudeRecorded();
            double totalDistance = flightPlan.DistanceNM;

            double plannedCruise = _activeFlightPlanService.GetPlannedCruiseAltitude();
            if (plannedCruise == 0)
            {
                plannedCruise = maxAltitude > 0 ? Math.Max(maxAltitude, 3000) : 8000;
                _activeFlightPlanService.SetPlannedCruiseAltitude(plannedCruise);
            }
            double cruiseAltitude = plannedCruise;

            var model = new PlotModel
            {
                Background = OxyColors.Transparent,
                PlotAreaBorderColor = OxyColors.Transparent,
                PlotMargins = new OxyThickness(40, 4, 8, 4)
            };

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = totalDistance,
                IsAxisVisible = false,
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = cruiseAltitude * 1.2,
                IsAxisVisible = true,
                AxislineColor = OxyColor.FromRgb(139, 148, 158),
                AxislineStyle = LineStyle.Solid,
                AxislineThickness = 1,
                TextColor = OxyColor.FromRgb(139, 148, 158),
                TicklineColor = OxyColor.FromRgb(139, 148, 158),
                MajorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColor.FromArgb(40, 139, 148, 158),
                MinorGridlineStyle = LineStyle.None,
                FontSize = 10,
                StringFormat = "N0",
                Unit = "ft",
                AxisTitleDistance = 8
            });

            var idealLine = new LineSeries
            {
                Color = OxyColor.FromArgb(180, 150, 150, 150),
                StrokeThickness = 1.5,
                LineStyle = LineStyle.Dash,
                TrackerFormatString = ""
            };
            idealLine.Points.Add(new DataPoint(0, 0));
            idealLine.Points.Add(new DataPoint(totalDistance * 0.1, cruiseAltitude));
            idealLine.Points.Add(new DataPoint(totalDistance * 0.9, cruiseAltitude));
            idealLine.Points.Add(new DataPoint(totalDistance, 0));
            model.Series.Add(idealLine);

            if (altitudeTrack.Count > 0)
            {
                var actualLine = new LineSeries
                {
                    Color = OxyColor.FromRgb(0, 200, 255),
                    StrokeThickness = 2,
                    TrackerFormatString = "",
                    Tag = altitudeTrack
                };

                foreach (var point in altitudeTrack)
                {
                    actualLine.Points.Add(new DataPoint(point.DistanceNM, point.AltitudeFt));
                }
                model.Series.Add(actualLine);
            }

            _viewData.AltitudeProfileModel = model;
            model.InvalidatePlot(true);
        }

        private static double CalculateDistanceNM(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 3440.065; // Earth radius in nautical miles
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            return 2 * R * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }

        private static double CalculateBearing(double lat1, double lon1, double lat2, double lon2)
        {
            double lat1Rad = lat1 * Math.PI / 180;
            double lat2Rad = lat2 * Math.PI / 180;
            double dLonRad = (lon2 - lon1) * Math.PI / 180;

            double y = Math.Sin(dLonRad) * Math.Cos(lat2Rad);
            double x = Math.Cos(lat1Rad) * Math.Sin(lat2Rad) - Math.Sin(lat1Rad) * Math.Cos(lat2Rad) * Math.Cos(dLonRad);

            double bearing = Math.Atan2(y, x) * 180 / Math.PI;
            return (bearing + 360) % 360;
        }

        private void OnFlightPlanChanged(ActiveFlightPlan? flightPlan)
        {
            Dispatcher.Invoke(() =>
            {
                _viewData.HasActiveFlightPlan = flightPlan != null;
                if (flightPlan != null)
                {
                    string currentFlightPlanId = $"{flightPlan.DepartureIcao}-{flightPlan.ArrivalIcao}-{flightPlan.ActivatedAt:O}";
                    bool isNewFlightPlan = _lastKnownFlightPlanId != currentFlightPlanId;

                    _viewData.DepartureIcao = flightPlan.DepartureIcao;
                    _viewData.ArrivalIcao = flightPlan.ArrivalIcao;
                    _viewData.DistanceRemaining = flightPlan.DistanceNM;
                    _viewData.TotalDistanceNM = flightPlan.DistanceNM;

                    _viewData.FlightPlanAircraftRegistration = flightPlan.AircraftRegistration;
                    _viewData.FlightPlanAircraftType = flightPlan.AircraftType;
                    _viewData.FlightPlanPassengers = flightPlan.Passengers;
                    _viewData.FlightPlanCargoKg = flightPlan.CargoKg;

                    if (flightPlan.AircraftId > 0)
                    {
                        var aircraft = _aircraftRepository.GetAircraftById(flightPlan.AircraftId);
                        if (aircraft != null)
                        {
                            _viewData.AircraftCruiseSpeedKts = aircraft.CruiseSpeedKts;
                            _viewData.AircraftServiceCeilingFt = aircraft.ServiceCeilingFt;
                        }
                    }

                    var departureAirport = _airportDatabase.GetAirport(flightPlan.DepartureIcao);
                    var arrivalAirport = _airportDatabase.GetAirport(flightPlan.ArrivalIcao);

                    var arrivalDetail = _airportDatabase.GetAirportDetail(flightPlan.ArrivalIcao);
                    if (arrivalDetail != null)
                    {
                        _viewData.DestinationElevationFt = arrivalDetail.Elevation;
                    }
                    if (departureAirport != null && arrivalAirport != null)
                    {
                        _viewData.CourseToDest = CalculateBearing(
                            departureAirport.Latitude, departureAirport.Longitude,
                            arrivalAirport.Latitude, arrivalAirport.Longitude);
                    }

                    var existingTrack = _activeFlightPlanService.GetAltitudeTrack();
                    bool hasExistingTrackData = existingTrack.Count > 0;

                    if (isNewFlightPlan && !hasExistingTrackData)
                    {
                        _viewData.FlightProgress = 0;
                        _activeFlightPlanService.ResetAltitudeTracking();
                        _activeFlightPlanService.AddAltitudePoint(0, 0);
                        _todSoundPlayed = false;
                        _fuelLowSoundPlayed = false;
                    }
                    _lastKnownFlightPlanId = currentFlightPlanId;

                    if (_isLoaded)
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            InitializeAltitudeProfileChart();
                        }), System.Windows.Threading.DispatcherPriority.Render);
                    }
                }
                else
                {
                    _viewData.DepartureIcao = string.Empty;
                    _viewData.ArrivalIcao = string.Empty;
                    _viewData.DistanceRemaining = 0;
                    _viewData.FlightProgress = 0;
                    _viewData.TotalDistanceNM = 0;
                    _viewData.AltitudeProfileModel = null;
                    _viewData.FlightPlanAircraftRegistration = string.Empty;
                    _viewData.FlightPlanAircraftType = string.Empty;
                    _viewData.FlightPlanPassengers = 0;
                    _viewData.FlightPlanCargoKg = 0;
                    _viewData.CourseToDest = 0;
                    _viewData.AircraftCruiseSpeedKts = 0;
                    _viewData.AircraftServiceCeilingFt = 0;
                    _viewData.DestinationElevationFt = 0;
                    _lastKnownFlightPlanId = null;
                }
                _logger.Info($"DashboardView: Flight plan status changed - {(flightPlan != null ? "Active" : "Inactive")}");
            });
        }

        private void OnFinancePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IFinanceService.Balance))
            {
                Dispatcher.Invoke(() =>
                {
                    _viewData.Balance = _financeService.Balance;
                });
            }
        }

        private void LoadStatistics()
        {
            try
            {
                var allFBOs = _fboRepository.GetAllFBOs();
                var allAircraft = _aircraftRepository.GetAllAircraft();
                var employedPilots = _pilotRepository.GetEmployedPilots();

                var aircraftWithFBO = allAircraft.Where(a => a.AssignedFBOId != null).Select(a => a.AssignedFBOId).Distinct().ToList();
                var fbosWithoutAircraft = allFBOs.Count(f => !aircraftWithFBO.Contains(f.Id));

                var aircraftWithoutFBO = allAircraft.Count(a => a.AssignedFBOId == null);

                var aircraftWithPilot = allAircraft.Where(a => a.AssignedPilotId != null).Select(a => a.AssignedPilotId).Distinct().ToList();
                var pilotsWithoutAircraft = employedPilots.Count(p => !aircraftWithPilot.Contains(p.Id));

                _viewData.FBOsWithoutAircraft = fbosWithoutAircraft;
                _viewData.AircraftWithoutFBO = aircraftWithoutFBO;
                _viewData.PilotsWithoutAircraft = pilotsWithoutAircraft;

                _logger.Debug($"Dashboard statistics loaded: FBOs without aircraft={fbosWithoutAircraft}, Aircraft without FBO={aircraftWithoutFBO}, Pilots without aircraft={pilotsWithoutAircraft}");
            }
            catch (System.Exception ex)
            {
                _logger.Error($"Failed to load dashboard statistics: {ex.Message}");
            }
        }

        private void MiniMapControl_MapClicked(object? sender, EventArgs e)
        {
            if (!_viewData.HasActiveFlightPlan) return;

            var window = new FlightMapWindow(
                _viewData.DepartureIcao,
                _viewData.ArrivalIcao,
                _airportDatabase,
                _activeFlightPlanService,
                _logger,
                _settingsService)
            {
                Owner = Window.GetWindow(this)
            };
            window.ShowDialog();
        }

        private void AltitudeChartView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var plotView = sender as OxyPlot.Wpf.PlotView;
            if (plotView?.Model == null) return;

            var altitudeTrack = _activeFlightPlanService.GetAltitudeTrack();
            if (altitudeTrack.Count == 0) return;

            var position = e.GetPosition(plotView);
            var plotArea = plotView.Model.PlotArea;

            if (!plotArea.Contains(position.X, position.Y)) return;

            var xAxis = plotView.Model.Axes.FirstOrDefault(a => a.Position == AxisPosition.Bottom);
            if (xAxis == null) return;

            double xValue = xAxis.InverseTransform(position.X);

            var closestPoint = altitudeTrack
                .OrderBy(p => Math.Abs(p.DistanceNM - xValue))
                .FirstOrDefault();

            if (closestPoint == null) return;

            ShowAltitudeTooltip(closestPoint, position, plotView);
        }

        private Popup? _altitudeTooltipPopup;

        private void ShowAltitudeTooltip(AltitudePoint point, Point position, OxyPlot.Wpf.PlotView plotView)
        {
            if (_altitudeTooltipPopup != null)
            {
                _altitudeTooltipPopup.IsOpen = false;
            }

            var tooltipBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(33, 38, 45)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(48, 54, 61)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(12, 8, 12, 8),
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color = Colors.Black,
                    BlurRadius = 10,
                    ShadowDepth = 2,
                    Opacity = 0.5
                }
            };

            var stack = new StackPanel { Orientation = Orientation.Vertical };

            var altitudePanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 4) };
            altitudePanel.Children.Add(new TextBlock
            {
                Text = "ALT",
                Foreground = new SolidColorBrush(Color.FromRgb(139, 148, 158)),
                FontSize = 10,
                FontWeight = System.Windows.FontWeights.SemiBold,
                Width = 35,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            });
            altitudePanel.Children.Add(new TextBlock
            {
                Text = $"{point.AltitudeFt:N0} ft",
                Foreground = new SolidColorBrush(Color.FromRgb(240, 246, 252)),
                FontSize = 13,
                FontWeight = System.Windows.FontWeights.Bold
            });
            stack.Children.Add(altitudePanel);

            var vsPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 4) };
            vsPanel.Children.Add(new TextBlock
            {
                Text = "VS",
                Foreground = new SolidColorBrush(Color.FromRgb(139, 148, 158)),
                FontSize = 10,
                FontWeight = System.Windows.FontWeights.SemiBold,
                Width = 35,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            });
            var vsColor = point.VerticalSpeedFpm >= 0
                ? Color.FromRgb(35, 134, 54)
                : Color.FromRgb(248, 81, 73);
            vsPanel.Children.Add(new TextBlock
            {
                Text = $"{point.VerticalSpeedFpm:+#,0;-#,0;0} fpm",
                Foreground = new SolidColorBrush(vsColor),
                FontSize = 13,
                FontWeight = System.Windows.FontWeights.Bold
            });
            stack.Children.Add(vsPanel);

            var gsPanel = new StackPanel { Orientation = Orientation.Horizontal };
            gsPanel.Children.Add(new TextBlock
            {
                Text = "GS",
                Foreground = new SolidColorBrush(Color.FromRgb(139, 148, 158)),
                FontSize = 10,
                FontWeight = System.Windows.FontWeights.SemiBold,
                Width = 35,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            });
            gsPanel.Children.Add(new TextBlock
            {
                Text = $"{point.GroundSpeedKts:N0} kts",
                Foreground = new SolidColorBrush(Color.FromRgb(88, 166, 255)),
                FontSize = 13,
                FontWeight = System.Windows.FontWeights.Bold
            });
            stack.Children.Add(gsPanel);

            tooltipBorder.Child = stack;

            _altitudeTooltipPopup = new Popup
            {
                Child = tooltipBorder,
                PlacementTarget = plotView,
                Placement = PlacementMode.Relative,
                HorizontalOffset = position.X + 10,
                VerticalOffset = position.Y - 60,
                IsOpen = true,
                AllowsTransparency = true,
                StaysOpen = false
            };

            _altitudeTooltipPopup.Closed += (s, args) => _altitudeTooltipPopup = null;
        }
    }
}

