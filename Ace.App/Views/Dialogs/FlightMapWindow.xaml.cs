using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Manipulations;
using Mapsui.Nts;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling.Layers;
using NetTopologySuite.Geometries;
using Ace.App.Controls;
using Ace.App.Helpers;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Views.FBO;
using WpfColor = System.Windows.Media.Color;
using WpfBrush = System.Windows.Media.SolidColorBrush;

namespace Ace.App.Views.Dialogs
{
    public partial class FlightMapWindow : Window
    {
        private readonly IAirportDatabase _airportDatabase;
        private readonly IActiveFlightPlanService _activeFlightPlanService;
        private readonly ILoggingService _logger;
        private readonly ISettingsService _settingsService;

        private MemoryLayer? _routeLayer;
        private MemoryLayer? _aircraftLayer;

        private readonly string _departureIcao;
        private readonly string _arrivalIcao;

        public FlightMapWindow(
            string departureIcao,
            string arrivalIcao,
            IAirportDatabase airportDatabase,
            IActiveFlightPlanService activeFlightPlanService,
            ILoggingService logger,
            ISettingsService settingsService)
        {
            _airportDatabase = airportDatabase ?? throw new ArgumentNullException(nameof(airportDatabase));
            _activeFlightPlanService = activeFlightPlanService ?? throw new ArgumentNullException(nameof(activeFlightPlanService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

            _departureIcao = departureIcao;
            _arrivalIcao = arrivalIcao;

            InitializeComponent();
            InitializeMap();
            UpdateFlightInfo();

            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.SimConnectService.FlightDataReceived += OnFlightDataReceived;
            }

            TxtRoute.Text = $"{departureIcao} -> {arrivalIcao}";
            TxtDeparture.Text = departureIcao;
            TxtArrival.Text = arrivalIcao;

            Closed += OnWindowClosed;
        }

        private void InitializeMap()
        {
            var savedLayer = Enum.TryParse<MapLayerType>(_settingsService.CurrentSettings.MapLayer, out var layer)
                ? layer
                : MapLayerType.Street;
            MapControl.Map.Layers.Add(MapTileHelper.CreateTileLayer(savedLayer));

            _routeLayer = new MemoryLayer { Name = "Route", Style = null };
            _aircraftLayer = new MemoryLayer { Name = "Aircraft", Style = null };

            MapControl.Map.Layers.Add(_routeLayer);
            MapControl.Map.Layers.Add(_aircraftLayer);

            MapControl.MouseRightButtonUp += OnMapRightClick;

            ScaleControl.AttachToMap(MapControl.Map);
            InitializeRunwayLegend();
            UpdateMapLayerButtons(savedLayer);
            UpdateLegendVisibility(savedLayer);

            // Initialize legend expanded state from settings
            LegendPanel.IsExpanded = _settingsService.CurrentSettings.MapLegendExpanded;
            LegendPanel.ExpandedStateChanged += OnLegendExpandedStateChanged;

            UpdateRoute();
        }

        private void OnLegendExpandedStateChanged(bool isExpanded)
        {
            _settingsService.CurrentSettings.MapLegendExpanded = isExpanded;
            _settingsService.Save();
        }

        private void InitializeRunwayLegend()
        {
            var runwayEntries = new List<MapLegendEntry>
            {
                new("< 2,000 ft", new WpfBrush(WpfColor.FromRgb(144, 202, 249))),
                new("< 4,000 ft", new WpfBrush(WpfColor.FromRgb(100, 181, 246))),
                new("< 6,000 ft", new WpfBrush(WpfColor.FromRgb(66, 165, 245))),
                new("< 8,000 ft", new WpfBrush(WpfColor.FromRgb(33, 150, 243))),
                new("8,000+ ft", new WpfBrush(WpfColor.FromRgb(156, 39, 176)))
            };
            LegendPanel.SetRunwayLegend(runwayEntries);
        }

        private void UpdateRoute()
        {
            if (_routeLayer == null) return;

            var departure = _airportDatabase.GetAirport(_departureIcao);
            var arrival = _airportDatabase.GetAirport(_arrivalIcao);

            if (departure == null || arrival == null) return;

            var features = new List<IFeature>();

            var depCoords = SphericalMercator.FromLonLat(departure.Longitude, departure.Latitude);
            var arrCoords = SphericalMercator.FromLonLat(arrival.Longitude, arrival.Latitude);
            var depPoint = new MPoint(depCoords.x, depCoords.y);
            var arrPoint = new MPoint(arrCoords.x, arrCoords.y);

            var lineGeometry = new LineString(new[]
            {
                new Coordinate(depPoint.X, depPoint.Y),
                new Coordinate(arrPoint.X, arrPoint.Y)
            });

            var routeFeature = new GeometryFeature(lineGeometry);
            routeFeature.Styles.Add(new VectorStyle
            {
                Line = new Pen(Color.FromString("#FF9800"), 3) { PenStyle = PenStyle.Dash }
            });
            features.Add(routeFeature);

            var depFeature = new PointFeature(depPoint);
            depFeature["icao"] = _departureIcao;
            depFeature["type"] = "departure";
            depFeature.Styles.Add(new SymbolStyle
            {
                Fill = new Brush(Color.FromString("#4CAF50")),
                Outline = new Pen(Color.White, 2),
                SymbolScale = 0.6,
                SymbolType = SymbolType.Ellipse
            });
            depFeature.Styles.Add(new LabelStyle
            {
                Text = _departureIcao,
                ForeColor = Color.White,
                BackColor = new Brush(Color.FromArgb(180, 0, 0, 0)),
                Offset = new Offset(0, -25),
                Font = new Font { Size = 12, Bold = true }
            });
            features.Add(depFeature);

            var arrFeature = new PointFeature(arrPoint);
            arrFeature["icao"] = _arrivalIcao;
            arrFeature["type"] = "arrival";
            arrFeature.Styles.Add(new SymbolStyle
            {
                Fill = new Brush(Color.FromString("#2196F3")),
                Outline = new Pen(Color.White, 2),
                SymbolScale = 0.6,
                SymbolType = SymbolType.Ellipse
            });
            arrFeature.Styles.Add(new LabelStyle
            {
                Text = _arrivalIcao,
                ForeColor = Color.White,
                BackColor = new Brush(Color.FromArgb(180, 0, 0, 0)),
                Offset = new Offset(0, -25),
                Font = new Font { Size = 12, Bold = true }
            });
            features.Add(arrFeature);

            _routeLayer.Features = features;
            _routeLayer.DataHasChanged();

            ZoomToRoute(depPoint, arrPoint);
        }

        private void ZoomToRoute(MPoint departure, MPoint arrival)
        {
            double minX = Math.Min(departure.X, arrival.X);
            double maxX = Math.Max(departure.X, arrival.X);
            double minY = Math.Min(departure.Y, arrival.Y);
            double maxY = Math.Max(departure.Y, arrival.Y);

            double paddingX = (maxX - minX) * 0.3;
            double paddingY = (maxY - minY) * 0.3;

            if (paddingX < 100000) paddingX = 100000;
            if (paddingY < 100000) paddingY = 100000;

            var extent = new MRect(minX - paddingX, minY - paddingY, maxX + paddingX, maxY + paddingY);
            MapControl.Map.Navigator.ZoomToBox(extent, MBoxFit.Fit);
        }

        private void UpdateAircraftPosition(double latitude, double longitude, double heading, double altitude, double groundSpeed)
        {
            if (_aircraftLayer == null) return;

            if (latitude == 0 && longitude == 0)
            {
                _aircraftLayer.Features = new List<IFeature>();
                _aircraftLayer.DataHasChanged();
                return;
            }

            var coords = SphericalMercator.FromLonLat(longitude, latitude);
            var point = new MPoint(coords.x, coords.y);
            var feature = new PointFeature(point);
            foreach (var style in AircraftMapSymbolHelper.CreateAircraftStyle(heading))
            {
                feature.Styles.Add(style);
            }

            _aircraftLayer.Features = new List<IFeature> { feature };
            _aircraftLayer.DataHasChanged();

            TxtAltitude.Text = $"{altitude:N0} ft";
            TxtSpeed.Text = $"{groundSpeed:N0} kts";
            TxtHeading.Text = $"{heading:N0}°";
        }

        private void UpdateFlightInfo()
        {
            var plan = _activeFlightPlanService.GetActivePlan();
            if (plan != null)
            {
                TxtProgress.Text = "---";
                TxtRemaining.Text = $"{plan.DistanceNM:N0} NM";
            }
        }

        private void OnFlightDataReceived(FlightData data)
        {
            Dispatcher.Invoke(() =>
            {
                UpdateAircraftPosition(data.Latitude, data.Longitude, data.Heading, data.Altitude, data.GroundSpeed);

                var plan = _activeFlightPlanService.GetActivePlan();
                if (plan != null)
                {
                    var arrival = _airportDatabase.GetAirport(_arrivalIcao);
                    if (arrival != null)
                    {
                        double distToArrival = CalculateDistanceNM(data.Latitude, data.Longitude, arrival.Latitude, arrival.Longitude);
                        double progress = ((plan.DistanceNM - distToArrival) / plan.DistanceNM) * 100;
                        progress = Math.Max(0, Math.Min(100, progress));

                        TxtProgress.Text = $"{progress:F0}%";
                        TxtRemaining.Text = $"{distToArrival:N0} NM";
                    }
                }
            });
        }

        private void OnMapRightClick(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(MapControl);
            var screenPosition = new ScreenPosition(pos.X, pos.Y);
            var layers = MapControl.Map?.Layers;
            if (layers == null) return;

            var mapInfo = MapControl.GetMapInfo(screenPosition, layers);
            if (mapInfo?.Feature == null) return;

            if (mapInfo.Feature["icao"] is not string icao || string.IsNullOrEmpty(icao)) return;

            var airport = _airportDatabase.GetAirport(icao);
            if (airport == null) return;

            var contextMenu = new ContextMenu();

            var setDepartureItem = new MenuItem { Header = $"Set {icao} as Departure" };
            setDepartureItem.Click += (s, args) =>
            {
                _logger.Info($"FlightMapWindow: Setting {icao} as departure via context menu");
            };
            contextMenu.Items.Add(setDepartureItem);

            var setArrivalItem = new MenuItem { Header = $"Set {icao} as Arrival" };
            setArrivalItem.Click += (s, args) =>
            {
                _logger.Info($"FlightMapWindow: Setting {icao} as arrival via context menu");
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
            try
            {
                var detailView = new AirportDetailView(_airportDatabase, _logger, icao);
                var window = new Window
                {
                    Title = $"Airport Info - {icao}",
                    Content = detailView,
                    Width = 700,
                    Height = 600,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = this
                };

                detailView.BackRequested += (s, args) => window.Close();
                window.ShowDialog();
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to show airport info for {icao}: {ex.Message}");
                TxtStatus.Text = $"Error loading airport info: {ex.Message}";
            }
        }

        private static double CalculateDistanceNM(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 3440.065;
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private void OnWindowClosed(object? sender, EventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.SimConnectService.FlightDataReceived -= OnFlightDataReceived;
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

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
            var tileLayer = MapControl.Map.Layers.FirstOrDefault(l => l is TileLayer);
            if (tileLayer != null)
            {
                MapControl.Map.Layers.Remove(tileLayer);
            }

            var newTileLayer = MapTileHelper.CreateTileLayer(layerType);
            MapControl.Map.Layers.Insert(0, newTileLayer);

            _settingsService.CurrentSettings.MapLayer = layerType.ToString();
            _settingsService.Save();

            UpdateMapLayerButtons(layerType);
            UpdateLegendVisibility(layerType);

            _logger.Info($"FlightMapWindow: Switched to {layerType} map layer");
        }

        private void UpdateMapLayerButtons(MapLayerType layerType = MapLayerType.Street)
        {
            StreetLayerButton.Opacity = layerType == MapLayerType.Street ? 1.0 : 0.6;
            TerrainLayerButton.Opacity = layerType == MapLayerType.Terrain ? 1.0 : 0.6;
            SatelliteLayerButton.Opacity = layerType == MapLayerType.Satellite ? 1.0 : 0.6;
        }

        private void UpdateLegendVisibility(MapLayerType layerType)
        {
            LegendPanel.ShowTerrainLegend(layerType == MapLayerType.Terrain);
        }
    }
}
