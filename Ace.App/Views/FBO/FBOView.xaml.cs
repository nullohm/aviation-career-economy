using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Projections;
using Mapsui.Tiling;
using Mapsui.Tiling.Layers;
using NetTopologySuite.Geometries;
using MapsuiStyles = Mapsui.Styles;
using Ace.App.Controls;
using Ace.App.Helpers;
using Ace.App.Infrastructure;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;
using Ace.App.ViewModels;
using Ace.App.Views.Dialogs;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Ace.App.Views.FBO
{
    public partial class FBOView : UserControl
    {
        private readonly FBOListViewModel _viewModel;
        private readonly IAirportDatabase _airportDatabase;
        private readonly ILoggingService _logger;
        private readonly ISettingsService _settingsService;
        private readonly IScheduledRouteService _scheduledRouteService;
        private readonly IFBORepository _fboRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IPilotRepository _pilotRepository;
        private readonly IScheduledRouteRepository _routeRepository;

        private bool _fboListExpanded = true;
        private FBOViewModel? _selectedFBO;
        private int _selectedFBOId;
        private bool _isMeasuringDistance;
        private MPoint? _measureStartPoint;

        private MemoryLayer? _airportLayer;
        private MemoryLayer? _fboLayer;
        private MemoryLayer? _routeLayer;
        private MemoryLayer? _measurementLayer;
        private Map? _map;
        private List<Airport> _allAirports = new();
        private HashSet<string> _fboIcaos = new();

        public FBOView(
            FBOListViewModel viewModel,
            IAirportDatabase airportDatabase,
            ILoggingService logger,
            ISettingsService settingsService,
            IScheduledRouteService scheduledRouteService,
            IFBORepository fboRepository,
            IAircraftRepository aircraftRepository,
            IPilotRepository pilotRepository,
            IScheduledRouteRepository routeRepository)
        {
            InitializeComponent();
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _airportDatabase = airportDatabase ?? throw new ArgumentNullException(nameof(airportDatabase));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _scheduledRouteService = scheduledRouteService ?? throw new ArgumentNullException(nameof(scheduledRouteService));
            _fboRepository = fboRepository ?? throw new ArgumentNullException(nameof(fboRepository));
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));
            _pilotRepository = pilotRepository ?? throw new ArgumentNullException(nameof(pilotRepository));
            _routeRepository = routeRepository ?? throw new ArgumentNullException(nameof(routeRepository));
            DataContext = _viewModel;

            InitializeMap();
            InitializeTerminalSizes();
            InitializeServiceLabels();

            Loaded += FBOView_Loaded;
            TxtICAO.TextChanged += TxtICAO_TextChanged;
        }

        private void InitializeMap()
        {
            _map = new Map();
            _map.Widgets.Clear();

            var savedLayer = Enum.TryParse<MapLayerType>(_settingsService.CurrentSettings.MapLayer, out var layer)
                ? layer
                : MapLayerType.Street;
            _map.Layers.Add(MapTileHelper.CreateTileLayer(savedLayer));

            _airportLayer = new MemoryLayer
            {
                Name = "Airports",
                Style = null
            };
            _map.Layers.Add(_airportLayer);

            _routeLayer = new MemoryLayer
            {
                Name = "Routes",
                Style = null
            };
            _map.Layers.Add(_routeLayer);

            _fboLayer = new MemoryLayer
            {
                Name = "FBOs",
                Style = null
            };
            _map.Layers.Add(_fboLayer);

            _measurementLayer = new MemoryLayer
            {
                Name = "Measurement",
                Style = null
            };
            _map.Layers.Add(_measurementLayer);

            MapControl.Map = _map;
            MapControl.MouseLeftButtonUp += MapControl_MouseLeftButtonUp;
            MapControl.MouseRightButtonUp += MapControl_MouseRightButtonUp;

            _map.Navigator.ViewportChanged += (s, e) => UpdateAirportMarkers();

            UpdateMapLayerButtons(savedLayer);
            UpdateLegendVisibility(savedLayer);
        }

        private void InitializeTerminalSizes()
        {
            CmbTerminalSize.ItemsSource = Enum.GetNames(typeof(TerminalSize)).ToList();
            CmbTerminalSize.SelectedIndex = 0;
        }

        private void InitializeServiceLabels()
        {
            var settings = _settingsService.CurrentSettings;
            ChkRefueling.Content = $"Refueling Service ({settings.ServiceCostRefueling:N0} €/mo)";
            ChkHangar.Content = $"Hangar Service ({settings.ServiceCostHangar:N0} €/mo)";
            ChkCatering.Content = $"Catering Service ({settings.ServiceCostCatering:N0} €/mo)";
            ChkGroundHandling.Content = $"Ground Handling ({settings.ServiceCostGroundHandling:N0} €/mo)";
            ChkDeIcing.Content = $"De-Icing Service ({settings.ServiceCostDeIcing:N0} €/mo)";
        }

        private void FBOView_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadFBOs();
            LoadAirportsAndFBOs();
            UpdateStats();

            ScaleControl.AttachToMap(MapControl.Map);
            InitializeMapLegend();

            // Initialize map attribution
            var currentLayer = Enum.TryParse<MapLayerType>(_settingsService.CurrentSettings.MapLayer, out var layer)
                ? layer
                : MapLayerType.Street;
            MapAttribution.SetMapLayer(currentLayer);

            // Initialize legend expanded state from settings
            LegendPanel.IsExpanded = _settingsService.CurrentSettings.MapLegendExpanded;
            LegendPanel.ExpandedStateChanged += OnLegendExpandedStateChanged;
        }

        private void OnLegendExpandedStateChanged(bool isExpanded)
        {
            _settingsService.CurrentSettings.MapLegendExpanded = isExpanded;
            _settingsService.Save();
        }

        private void InitializeMapLegend()
        {
            var fboEntries = new List<MapLegendEntry>
            {
                new("▲ Local FBO", new SolidColorBrush(System.Windows.Media.Color.FromRgb(156, 163, 175))),
                new("■ Regional FBO", new SolidColorBrush(System.Windows.Media.Color.FromRgb(156, 163, 175))),
                new("● Int'l FBO", new SolidColorBrush(System.Windows.Media.Color.FromRgb(156, 163, 175))),
                new("Red = No Terminal", new SolidColorBrush(System.Windows.Media.Color.FromRgb(239, 68, 68))),
                new("Yellow = Free Slots", new SolidColorBrush(System.Windows.Media.Color.FromRgb(234, 179, 8))),
                new("Green = Full", new SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 197, 94)))
            };
            LegendPanel.SetFBOLegend(fboEntries);

            var routeEntries = new List<MapLegendEntry>
            {
                new("No Route", new SolidColorBrush(System.Windows.Media.Color.FromRgb(239, 68, 68)), true),
                new("Route Available", new SolidColorBrush(System.Windows.Media.Color.FromRgb(234, 179, 8)), true),
                new("Route Full", new SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 197, 94)), true)
            };
            LegendPanel.SetRouteLegend(routeEntries);

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

        private void LoadAirportsAndFBOs()
        {
            try
            {
                _fboIcaos = _fboRepository.GetAllFBOs().Select(f => f.ICAO).ToHashSet();

                _allAirports = _airportDatabase.GetAllAirports().ToList();

                UpdateAirportMarkers();
                LoadFBOMapData();

                _logger.Info($"FBOView: Loaded {_allAirports.Count} airports, {_fboIcaos.Count} FBOs");
            }
            catch (Exception ex)
            {
                _logger.Error("FBOView: Error loading airports and FBOs", ex);
            }
        }

        private void UpdateAirportMarkers()
        {
            if (_airportLayer == null || _map == null) return;

            var resolution = _map.Navigator.Viewport.Resolution;
            var minRunwayLength = MapZoomFilterHelper.GetMinRunwayLengthForZoom(resolution);

            _logger.Debug($"FBOView: UpdateAirportMarkers - Resolution: {resolution:F0}, MinRunwayLength: {minRunwayLength}");

            var airportFeatures = new List<IFeature>();

            foreach (var airport in _allAirports)
            {
                if (_fboIcaos.Contains(airport.Icao)) continue;

                if (MapZoomFilterHelper.ShouldShowNormalAirport(airport.LongestRunwayFt, minRunwayLength))
                {
                    var feature = CreateAirportFeature(airport);
                    airportFeatures.Add(feature);
                }
            }

            _airportLayer.Features = airportFeatures;
            _airportLayer.DataHasChanged();
        }

        private IFeature CreateAirportFeature(Airport airport)
        {
            var point = SphericalMercator.FromLonLat(airport.Longitude, airport.Latitude).ToMPoint();
            var feature = new PointFeature(point);

            feature["icao"] = airport.Icao;
            feature["name"] = airport.Name;
            feature["isFbo"] = false;

            var (scale, color, showLabel) = AirportSymbolHelper.GetAirportStyle(airport.LongestRunwayFt);

            feature.Styles.Add(new MapsuiStyles.SymbolStyle
            {
                SymbolScale = scale,
                Fill = new MapsuiStyles.Brush(color),
                Outline = new MapsuiStyles.Pen(MapsuiStyles.Color.White, 1),
                SymbolType = MapsuiStyles.SymbolType.Ellipse
            });

            if (showLabel)
            {
                feature.Styles.Add(new MapsuiStyles.LabelStyle
                {
                    Text = airport.Icao,
                    ForeColor = MapsuiStyles.Color.White,
                    BackColor = new MapsuiStyles.Brush(MapsuiStyles.Color.FromArgb(180, 0, 0, 0)),
                    Offset = new MapsuiStyles.Offset(0, -20),
                    Font = new MapsuiStyles.Font { Size = 9, Bold = true },
                    Halo = new MapsuiStyles.Pen(MapsuiStyles.Color.Black, 1)
                });
            }

            return feature;
        }

        private void LoadFBOMapData()
        {
            try
            {
                var fbos = _fboRepository.GetAllFBOs();

                if (fbos.Count == 0)
                {
                    _fboLayer!.Features = new List<IFeature>();
                    _fboLayer.DataHasChanged();
                    return;
                }

                var aircraftByFbo = new Dictionary<int, int>();
                foreach (var fbo in fbos)
                {
                    var count = _aircraftRepository.GetAircraftByFBOId(fbo.Id).Count;
                    if (count > 0) aircraftByFbo[fbo.Id] = count;
                }

                var settings = _settingsService.CurrentSettings;
                var fboFeatures = new List<IFeature>();

                foreach (var fbo in fbos)
                {
                    var airport = _airportDatabase.GetAirport(fbo.ICAO);
                    if (airport != null)
                    {
                        var aircraftCount = aircraftByFbo.GetValueOrDefault(fbo.Id, 0);
                        var outgoingRouteCount = _scheduledRouteService.GetOutgoingRouteCountForFBO(fbo.Id);
                        var maxRouteSlots = fbo.Type switch
                        {
                            FBOType.Local => settings.RouteSlotLimitLocal,
                            FBOType.Regional => settings.RouteSlotLimitRegional,
                            FBOType.International => settings.RouteSlotLimitInternational,
                            _ => settings.RouteSlotLimitLocal
                        };
                        fboFeatures.Add(CreateFBOFeature(airport, fbo, aircraftCount, outgoingRouteCount, maxRouteSlots));
                    }
                }

                _fboLayer!.Features = fboFeatures;
                _fboLayer.DataHasChanged();

                LoadRouteLines(fbos);

                if (fboFeatures.Count > 0)
                {
                    ZoomToFBOs(fbos);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("FBOView: Error loading FBO map data", ex);
            }
        }

        private void LoadRouteLines(List<Models.FBO> fbos)
        {
            if (_routeLayer == null) return;

            try
            {
                var features = new List<IFeature>();
                var settings = _settingsService.CurrentSettings;
                var pairLimit = settings.RoutesPerFBOPairLimit;

                for (int i = 0; i < fbos.Count; i++)
                {
                    for (int j = i + 1; j < fbos.Count; j++)
                    {
                        var fbo1 = fbos[i];
                        var fbo2 = fbos[j];

                        var airport1 = _airportDatabase.GetAirport(fbo1.ICAO);
                        var airport2 = _airportDatabase.GetAirport(fbo2.ICAO);
                        if (airport1 == null || airport2 == null) continue;

                        var routeCount = _scheduledRouteService.GetRouteCountBetweenFBOs(fbo1.Id, fbo2.Id);

                        MapsuiStyles.Color color;
                        if (routeCount == 0)
                        {
                            color = MapsuiStyles.Color.FromString("#EF4444");
                        }
                        else if (routeCount >= pairLimit)
                        {
                            color = MapsuiStyles.Color.FromString("#22C55E");
                        }
                        else
                        {
                            color = MapsuiStyles.Color.FromString("#EAB308");
                        }

                        features.Add(CreateRouteLine(airport1, airport2, color));
                    }
                }

                _routeLayer.Features = features;
                _routeLayer.DataHasChanged();

                _logger.Debug($"FBOView: Loaded {features.Count} connection lines");
            }
            catch (Exception ex)
            {
                _logger.Error("FBOView: Error loading route lines", ex);
            }
        }

        private IFeature CreateRouteLine(Airport origin, Airport dest, MapsuiStyles.Color color)
        {
            var p1 = SphericalMercator.FromLonLat(origin.Longitude, origin.Latitude);
            var p2 = SphericalMercator.FromLonLat(dest.Longitude, dest.Latitude);

            var line = new LineString(new[]
            {
                new Coordinate(p1.x, p1.y),
                new Coordinate(p2.x, p2.y)
            });

            var feature = new GeometryFeature(line);
            feature.Styles.Add(new MapsuiStyles.VectorStyle
            {
                Line = new MapsuiStyles.Pen(color, 2)
                {
                    PenStyle = MapsuiStyles.PenStyle.Solid
                }
            });

            return feature;
        }

        private IFeature CreateFBOFeature(Airport airport, Models.FBO fbo, int aircraftCount, int outgoingRouteCount, int maxRouteSlots)
        {
            var point = SphericalMercator.FromLonLat(airport.Longitude, airport.Latitude).ToMPoint();
            var feature = new PointFeature(point);

            feature["fboId"] = fbo.Id;
            feature["icao"] = airport.Icao;
            feature["name"] = fbo.AirportName;
            feature["type"] = fbo.Type.ToString();

            MapsuiStyles.Color color;
            if (fbo.TerminalSize == TerminalSize.None)
            {
                color = MapsuiStyles.Color.FromString("#EF4444");
            }
            else if (outgoingRouteCount >= maxRouteSlots)
            {
                color = MapsuiStyles.Color.FromString("#22C55E");
            }
            else
            {
                color = MapsuiStyles.Color.FromString("#EAB308");
            }

            var symbolType = fbo.Type switch
            {
                FBOType.Local => MapsuiStyles.SymbolType.Triangle,
                FBOType.Regional => MapsuiStyles.SymbolType.Rectangle,
                FBOType.International => MapsuiStyles.SymbolType.Ellipse,
                _ => MapsuiStyles.SymbolType.Rectangle
            };

            feature.Styles.Add(new MapsuiStyles.SymbolStyle
            {
                SymbolScale = 0.8,
                Fill = new MapsuiStyles.Brush(color),
                Outline = new MapsuiStyles.Pen(MapsuiStyles.Color.Black, 2),
                SymbolType = symbolType
            });

            feature.Styles.Add(new MapsuiStyles.LabelStyle
            {
                Text = airport.Icao,
                ForeColor = MapsuiStyles.Color.White,
                BackColor = new MapsuiStyles.Brush(MapsuiStyles.Color.FromArgb(200, 0, 0, 0)),
                Offset = new MapsuiStyles.Offset(0, -30),
                Font = new MapsuiStyles.Font { Size = 10, Bold = true },
                Halo = new MapsuiStyles.Pen(MapsuiStyles.Color.Black, 1)
            });

            return feature;
        }

        private void ZoomToFBOs(List<Models.FBO> fbos)
        {
            if (_map == null || fbos.Count == 0) return;

            double minLat = double.MaxValue, maxLat = double.MinValue;
            double minLon = double.MaxValue, maxLon = double.MinValue;

            foreach (var fbo in fbos)
            {
                var airport = _airportDatabase.GetAirport(fbo.ICAO);
                if (airport != null)
                {
                    minLat = Math.Min(minLat, airport.Latitude);
                    maxLat = Math.Max(maxLat, airport.Latitude);
                    minLon = Math.Min(minLon, airport.Longitude);
                    maxLon = Math.Max(maxLon, airport.Longitude);
                }
            }

            if (minLat < double.MaxValue)
            {
                var padding = 1.0;
                minLat -= padding;
                maxLat += padding;
                minLon -= padding;
                maxLon += padding;

                var min = SphericalMercator.FromLonLat(minLon, minLat);
                var max = SphericalMercator.FromLonLat(maxLon, maxLat);

                var extent = new MRect(min.x, min.y, max.x, max.y);
                _map.Navigator.ZoomToBox(extent, MBoxFit.Fit);
            }
        }

        private void MapControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(MapControl);
            var screenPosition = new Mapsui.Manipulations.ScreenPosition(pos.X, pos.Y);
            var layers = MapControl.Map?.Layers;
            if (layers == null) return;

            var mapInfo = MapControl.GetMapInfo(screenPosition, layers);
            if (mapInfo?.Feature == null) return;

            if (mapInfo.Feature["fboId"] is int fboId)
            {
                var fboViewModel = _viewModel.FBOs.FirstOrDefault(f => f.Id == fboId);
                if (fboViewModel != null)
                {
                    SelectFBO(fboViewModel);
                }
            }
        }

        private void MapControl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(MapControl);
            var screenPosition = new Mapsui.Manipulations.ScreenPosition(pos.X, pos.Y);
            var layers = MapControl.Map?.Layers;
            if (layers == null) return;

            var mapInfo = MapControl.GetMapInfo(screenPosition, layers);
            if (mapInfo?.Feature == null) return;

            var icao = mapInfo.Feature["icao"]?.ToString();
            if (string.IsNullOrEmpty(icao)) return;

            var airport = _airportDatabase.GetAirport(icao);
            if (airport == null) return;

            var isFbo = mapInfo.Feature["fboId"] != null || _fboIcaos.Contains(icao);

            var contextMenu = new ContextMenu();

            if (isFbo)
            {
                var fboViewModel = _viewModel.FBOs.FirstOrDefault(f => f.ICAO == icao);
                var fboId = fboViewModel?.Id ?? 0;

                var editItem = new MenuItem { Header = $"Edit FBO at {icao}" };
                editItem.Click += (s, args) =>
                {
                    if (fboViewModel != null)
                    {
                        SelectFBO(fboViewModel);
                    }
                };
                contextMenu.Items.Add(editItem);

                var terminateItem = new MenuItem { Header = $"Terminate FBO at {icao}" };
                terminateItem.Click += (s, args) => TerminateFBOFromContext(icao);
                contextMenu.Items.Add(terminateItem);

                contextMenu.Items.Add(new Separator());

                var remainingSlots = _scheduledRouteService.GetRemainingSlots(fboId);
                var createRouteItem = new MenuItem
                {
                    Header = $"Create Route from {icao}...",
                    IsEnabled = remainingSlots > 0
                };
                createRouteItem.Click += (s, args) => OpenCreateRouteDialog(fboId, icao);
                contextMenu.Items.Add(createRouteItem);

                var routes = _scheduledRouteService.GetRoutesByFBO(fboId);
                if (routes.Any())
                {
                    var routesMenu = new MenuItem { Header = $"Routes ({routes.Count})" };
                    foreach (var route in routes)
                    {
                        var otherFboId = route.OriginFBOId == fboId ? route.DestinationFBOId : route.OriginFBOId;
                        var otherFbo = _fboRepository.GetFBOById(otherFboId);

                        var aircraftBadge = "";
                        if (route.AssignedAircraftId.HasValue)
                        {
                            var aircraft = _aircraftRepository.GetAircraftById(route.AssignedAircraftId.Value);
                            aircraftBadge = aircraft != null ? $" [{aircraft.Registration}]" : " ✈";
                        }

                        var routeItem = new MenuItem
                        {
                            Header = $"→ {otherFbo?.ICAO ?? "?"}{aircraftBadge}"
                        };
                        var routeId = route.Id;
                        routeItem.Click += (s, args) => OpenRouteDetailsDialog(routeId);
                        routesMenu.Items.Add(routeItem);
                    }
                    contextMenu.Items.Add(routesMenu);
                }
            }
            else
            {
                var rentItem = new MenuItem { Header = $"Rent FBO at {icao}" };
                rentItem.Click += (s, args) => RentFBOFromContext(icao, airport.Name);
                contextMenu.Items.Add(rentItem);
            }

            contextMenu.Items.Add(new Separator());

            var infoItem = new MenuItem { Header = $"Airport Info: {icao}" };
            infoItem.Click += (s, args) => ShowAirportInfo(icao);
            contextMenu.Items.Add(infoItem);

            contextMenu.IsOpen = true;
        }

        private void RentFBOFromContext(string icao, string airportName)
        {
            var typeDialog = new SelectFBOTypeDialog(icao, airportName, _settingsService);
            typeDialog.Owner = Window.GetWindow(this);

            if (typeDialog.ShowDialog() != true || !typeDialog.Confirmed)
                return;

            _viewModel.ICAOInput = icao;
            _viewModel.SelectedFBOType = typeDialog.SelectedType.ToString();

            if (_viewModel.RentFBO(airportName))
            {
                _fboIcaos.Add(icao);
                UpdateAirportMarkers();
                LoadFBOMapData();
                UpdateStats();
            }
            else
            {
                var errorDialog = new MessageDialog("Error", "Failed to rent FBO");
                errorDialog.Owner = Window.GetWindow(this);
                errorDialog.ShowDialog();
            }
        }

        private void TerminateFBOFromContext(string icao)
        {
            var fboViewModel = _viewModel.FBOs.FirstOrDefault(f => f.ICAO == icao);
            if (fboViewModel == null) return;

            var confirmDialog = new ConfirmDialog(
                "Terminate FBO",
                $"Terminate FBO at {icao} ({fboViewModel.AirportName})?");
            confirmDialog.Owner = Window.GetWindow(this);
            confirmDialog.ShowDialog();

            if (!confirmDialog.Result)
                return;

            if (_viewModel.TerminateFBO(fboViewModel.Id))
            {
                _fboIcaos.Remove(icao);
                UpdateAirportMarkers();
                LoadFBOMapData();
                UpdateStats();
                FBODetailsPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                var errorDialog = new MessageDialog("Error", "Failed to terminate FBO");
                errorDialog.Owner = Window.GetWindow(this);
                errorDialog.ShowDialog();
            }
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

            detailView.BackRequested += (s, args) => window.Close();
            window.ShowDialog();
        }

        private void UpdateStats()
        {
            var count = _viewModel.FBOs.Count;
            var totalCost = _viewModel.FBOs.Sum(f => f.TotalMonthlyCostValue);

            TxtFBOCount.Text = $"{count} FBO{(count != 1 ? "s" : "")}";
            TxtTotalCost.Text = $"€{totalCost:N0}/mo";
            TxtFBOListHeader.Text = $"Your FBOs ({count})";
        }

        private void FBOListHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _fboListExpanded = !_fboListExpanded;
            FBOListScrollViewer.Visibility = _fboListExpanded ? Visibility.Visible : Visibility.Collapsed;
            TxtExpandIcon.Text = _fboListExpanded ? "▼" : "▶";
        }

        private void OnFBOListItemClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is FBOViewModel fboViewModel)
            {
                SelectFBO(fboViewModel);
            }
        }

        private void SelectFBO(FBOViewModel fboViewModel)
        {
            _selectedFBO = fboViewModel;
            _selectedFBOId = fboViewModel.Id;

            TxtDetailICAO.Text = fboViewModel.ICAO;
            TxtDetailAirportName.Text = fboViewModel.AirportName;

            foreach (ComboBoxItem item in CmbDetailFBOType.Items)
            {
                if (item.Tag?.ToString() == fboViewModel.Type)
                {
                    CmbDetailFBOType.SelectedItem = item;
                    break;
                }
            }

            LoadFBODetails(fboViewModel.Id);
            LoadAirportImage(fboViewModel.ICAO);

            FBODetailsPanel.Visibility = Visibility.Visible;

            ZoomToFBO(fboViewModel.ICAO);
        }

        private void LoadFBODetails(int fboId)
        {
            try
            {
                var fbo = _fboRepository.GetFBOById(fboId);

                if (fbo == null) return;

                CmbTerminalSize.SelectedItem = fbo.TerminalSize.ToString();
                UpdateTerminalCostDisplay(fbo.TerminalSize);

                ChkRefueling.IsChecked = fbo.HasRefuelingService;
                ChkHangar.IsChecked = fbo.HasHangarService;
                ChkCatering.IsChecked = fbo.HasCateringService;
                ChkGroundHandling.IsChecked = fbo.HasGroundHandling;
                ChkDeIcing.IsChecked = fbo.HasDeIcingService;

                TxtBaseRent.Text = $"Base: {fbo.MonthlyRent:N0} €";

                UpdateTotalCostDisplay(fbo);
            }
            catch (Exception ex)
            {
                _logger.Error($"FBOView: Error loading FBO details for {fboId}", ex);
            }
        }

        private void ZoomToFBO(string icao)
        {
            var airport = _airportDatabase.GetAirport(icao);
            if (airport == null || _map == null) return;

            var center = SphericalMercator.FromLonLat(airport.Longitude, airport.Latitude);
            _map.Navigator.CenterOn(center.x, center.y);
            _map.Navigator.ZoomTo(3);
        }

        private void CmbTerminalSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbTerminalSize.SelectedItem is string sizeStr && Enum.TryParse<TerminalSize>(sizeStr, out var size))
            {
                UpdateTerminalCostDisplay(size);
                UpdateTotalCostFromUI();
            }
        }

        private void CmbDetailFBOType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selectedFBOId == 0) return;
            if (CmbDetailFBOType.SelectedItem is not ComboBoxItem selectedItem) return;

            var newTypeStr = selectedItem.Tag?.ToString();
            if (string.IsNullOrEmpty(newTypeStr)) return;
            if (!Enum.TryParse<FBOType>(newTypeStr, out var newType)) return;

            UpdateBaseRentDisplay(newType);
        }

        private void UpdateBaseRentDisplay(FBOType type)
        {
            var settings = _settingsService.CurrentSettings;
            var baseRent = type switch
            {
                FBOType.Local => settings.FBORentLocal,
                FBOType.Regional => settings.FBORentRegional,
                FBOType.International => settings.FBORentInternational,
                _ => settings.FBORentLocal
            };

            TxtBaseRent.Text = $"Base: {baseRent:N0} €";
            UpdateTotalCostFromUIWithType(type);
        }

        private void UpdateTotalCostFromUIWithType(FBOType type)
        {
            if (_selectedFBOId == 0) return;

            var settings = _settingsService.CurrentSettings;
            decimal servicesCost = 0;

            if (ChkRefueling.IsChecked == true) servicesCost += settings.ServiceCostRefueling;
            if (ChkHangar.IsChecked == true) servicesCost += settings.ServiceCostHangar;
            if (ChkCatering.IsChecked == true) servicesCost += settings.ServiceCostCatering;
            if (ChkGroundHandling.IsChecked == true) servicesCost += settings.ServiceCostGroundHandling;
            if (ChkDeIcing.IsChecked == true) servicesCost += settings.ServiceCostDeIcing;

            decimal terminalCost = 0;
            if (CmbTerminalSize.SelectedItem is string sizeStr && Enum.TryParse<TerminalSize>(sizeStr, out var size))
            {
                terminalCost = size switch
                {
                    TerminalSize.None => 0m,
                    TerminalSize.Small => settings.TerminalCostSmall,
                    TerminalSize.Medium => settings.TerminalCostMedium,
                    TerminalSize.MediumLarge => settings.TerminalCostMediumLarge,
                    TerminalSize.Large => settings.TerminalCostLarge,
                    TerminalSize.VeryLarge => settings.TerminalCostVeryLarge,
                    _ => 0m
                };
            }

            var baseRent = type switch
            {
                FBOType.Local => settings.FBORentLocal,
                FBOType.Regional => settings.FBORentRegional,
                FBOType.International => settings.FBORentInternational,
                _ => settings.FBORentLocal
            };

            var total = baseRent + terminalCost + servicesCost;

            TxtTotalMonthlyCost.Text = $"{total:N0} €";
            TxtServicesCost.Text = $"Services: {servicesCost:N0} €";
        }

        private void UpdateTerminalCostDisplay(TerminalSize size)
        {
            var settings = _settingsService.CurrentSettings;
            var cost = size switch
            {
                TerminalSize.None => 0m,
                TerminalSize.Small => settings.TerminalCostSmall,
                TerminalSize.Medium => settings.TerminalCostMedium,
                TerminalSize.MediumLarge => settings.TerminalCostMediumLarge,
                TerminalSize.Large => settings.TerminalCostLarge,
                TerminalSize.VeryLarge => settings.TerminalCostVeryLarge,
                _ => 0m
            };
            TxtTerminalCost.Text = $"{cost:N0} €/mo";
        }

        private void ServiceCheckbox_Click(object sender, RoutedEventArgs e)
        {
            UpdateTotalCostFromUI();
        }

        private void UpdateTotalCostFromUI()
        {
            if (_selectedFBOId == 0) return;

            try
            {
                var fbo = _fboRepository.GetFBOById(_selectedFBOId);
                if (fbo == null) return;

                var settings = _settingsService.CurrentSettings;
                decimal servicesCost = 0;

                if (ChkRefueling.IsChecked == true) servicesCost += settings.ServiceCostRefueling;
                if (ChkHangar.IsChecked == true) servicesCost += settings.ServiceCostHangar;
                if (ChkCatering.IsChecked == true) servicesCost += settings.ServiceCostCatering;
                if (ChkGroundHandling.IsChecked == true) servicesCost += settings.ServiceCostGroundHandling;
                if (ChkDeIcing.IsChecked == true) servicesCost += settings.ServiceCostDeIcing;

                decimal terminalCost = 0;
                if (CmbTerminalSize.SelectedItem is string sizeStr && Enum.TryParse<TerminalSize>(sizeStr, out var size))
                {
                    terminalCost = size switch
                    {
                        TerminalSize.None => 0m,
                        TerminalSize.Small => settings.TerminalCostSmall,
                        TerminalSize.Medium => settings.TerminalCostMedium,
                        TerminalSize.MediumLarge => settings.TerminalCostMediumLarge,
                        TerminalSize.Large => settings.TerminalCostLarge,
                        TerminalSize.VeryLarge => settings.TerminalCostVeryLarge,
                        _ => 0m
                    };
                }

                var total = fbo.MonthlyRent + terminalCost + servicesCost;

                TxtTotalMonthlyCost.Text = $"{total:N0} €";
                TxtServicesCost.Text = $"Services: {servicesCost:N0} €";
            }
            catch (Exception ex)
            {
                _logger.Error("FBOView: Error updating total cost", ex);
            }
        }

        private void UpdateTotalCostDisplay(Models.FBO fbo)
        {
            var settings = _settingsService.CurrentSettings;
            decimal servicesCost = 0;

            if (fbo.HasRefuelingService) servicesCost += settings.ServiceCostRefueling;
            if (fbo.HasHangarService) servicesCost += settings.ServiceCostHangar;
            if (fbo.HasCateringService) servicesCost += settings.ServiceCostCatering;
            if (fbo.HasGroundHandling) servicesCost += settings.ServiceCostGroundHandling;
            if (fbo.HasDeIcingService) servicesCost += settings.ServiceCostDeIcing;

            var total = fbo.MonthlyRent + fbo.TerminalMonthlyCost + servicesCost;

            TxtTotalMonthlyCost.Text = $"{total:N0} €";
            TxtServicesCost.Text = $"Services: {servicesCost:N0} €";
        }

        private void CloseDetailsPanel_Click(object sender, RoutedEventArgs e)
        {
            FBODetailsPanel.Visibility = Visibility.Collapsed;
            _selectedFBO = null;
            _selectedFBOId = 0;
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedFBOId == 0) return;

            try
            {
                var fbo = _fboRepository.GetFBOById(_selectedFBOId);

                if (fbo == null) return;

                var settings = _settingsService.CurrentSettings;

                if (CmbDetailFBOType.SelectedItem is ComboBoxItem typeItem &&
                    Enum.TryParse<FBOType>(typeItem.Tag?.ToString(), out var fboType))
                {
                    fbo.Type = fboType;
                    fbo.MonthlyRent = fboType switch
                    {
                        FBOType.Local => settings.FBORentLocal,
                        FBOType.Regional => settings.FBORentRegional,
                        FBOType.International => settings.FBORentInternational,
                        _ => settings.FBORentLocal
                    };
                }

                if (CmbTerminalSize.SelectedItem is string sizeStr && Enum.TryParse<TerminalSize>(sizeStr, out var terminalSize))
                {
                    fbo.TerminalSize = terminalSize;
                    fbo.TerminalMonthlyCost = terminalSize switch
                    {
                        TerminalSize.None => 0m,
                        TerminalSize.Small => settings.TerminalCostSmall,
                        TerminalSize.Medium => settings.TerminalCostMedium,
                        TerminalSize.MediumLarge => settings.TerminalCostMediumLarge,
                        TerminalSize.Large => settings.TerminalCostLarge,
                        TerminalSize.VeryLarge => settings.TerminalCostVeryLarge,
                        _ => 0m
                    };
                }

                fbo.HasRefuelingService = ChkRefueling.IsChecked == true;
                fbo.HasHangarService = ChkHangar.IsChecked == true;
                fbo.HasCateringService = ChkCatering.IsChecked == true;
                fbo.HasGroundHandling = ChkGroundHandling.IsChecked == true;
                fbo.HasDeIcingService = ChkDeIcing.IsChecked == true;

                _fboRepository.UpdateFBO(fbo);

                _logger.Info($"FBOView: FBO configuration updated: {fbo.ICAO} (Type: {fbo.Type})");

                _viewModel.LoadFBOs();
                LoadFBOMapData();
                UpdateStats();

                if (_selectedFBO != null)
                {
                    var updatedFbo = _viewModel.FBOs.FirstOrDefault(f => f.Id == _selectedFBOId);
                    if (updatedFbo != null)
                    {
                        _selectedFBO = updatedFbo;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"FBOView: Error saving FBO changes", ex);
                var errorDialog = new MessageDialog("Error", "Failed to save changes.");
                errorDialog.Owner = Window.GetWindow(this);
                errorDialog.ShowDialog();
            }
        }

        private void BtnRentFBO_Click(object sender, RoutedEventArgs e)
        {
            var (success, message) = _viewModel.ValidateRentFBO();

            if (!success)
            {
                var errorDialog = new MessageDialog("Cannot Rent FBO", message);
                errorDialog.Owner = Window.GetWindow(this);
                errorDialog.ShowDialog();
                return;
            }

            var airportName = message;
            var confirmDialog = new ConfirmDialog(
                "Rent FBO",
                $"Rent {_viewModel.SelectedFBOType} FBO at {_viewModel.ICAOInput.ToUpper()} ({airportName})?");
            confirmDialog.Owner = Window.GetWindow(this);
            confirmDialog.ShowDialog();

            if (!confirmDialog.Result)
                return;

            if (_viewModel.RentFBO(airportName))
            {
                LoadFBOMapData();
                UpdateStats();
            }
            else
            {
                var errorDialog = new MessageDialog("Error", "Failed to rent FBO");
                errorDialog.Owner = Window.GetWindow(this);
                errorDialog.ShowDialog();
            }
        }

        private void BtnTerminateFBO_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedFBO == null) return;

            var confirmDialog = new ConfirmDialog(
                "Terminate FBO",
                $"Terminate FBO at {_selectedFBO.ICAO}?\n\nThis will remove all terminals and services.");
            confirmDialog.Owner = Window.GetWindow(this);
            confirmDialog.ShowDialog();

            if (!confirmDialog.Result)
                return;

            if (_viewModel.TerminateFBO(_selectedFBO.Id))
            {
                FBODetailsPanel.Visibility = Visibility.Collapsed;
                _selectedFBO = null;
                _selectedFBOId = 0;

                LoadFBOMapData();
                UpdateStats();
            }
            else
            {
                var errorDialog = new MessageDialog("Error", "Failed to terminate FBO");
                errorDialog.Owner = Window.GetWindow(this);
                errorDialog.ShowDialog();
            }
        }

        private void TxtICAO_TextChanged(object sender, TextChangedEventArgs e)
        {
            var icao = TxtICAO.Text?.Trim().ToUpper();
            if (string.IsNullOrEmpty(icao) || icao.Length != 4)
            {
                BtnAirportInfo.IsEnabled = false;
                return;
            }

            var airport = _airportDatabase.GetAirport(icao);
            BtnAirportInfo.IsEnabled = airport != null;
        }

        private void BtnAirportInfo_Click(object sender, RoutedEventArgs e)
        {
            var icao = TxtICAO.Text?.Trim().ToUpper();
            if (string.IsNullOrEmpty(icao) || icao.Length != 4)
                return;

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

            detailView.BackRequested += (s, args) => window.Close();
            window.ShowDialog();
        }

        private void OpenCreateRouteDialog(int originFboId, string originIcao)
        {
            var dialog = new CreateRouteDialog(
                originFboId,
                originIcao,
                _scheduledRouteService,
                _airportDatabase,
                _logger,
                _fboRepository,
                _aircraftRepository,
                _pilotRepository);
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true)
            {
                _viewModel.LoadFBOs();
                LoadFBOMapData();
                UpdateStats();
            }
        }

        private void OpenRouteDetailsDialog(int routeId)
        {
            var dialog = new RouteDetailsDialog(
                routeId,
                _scheduledRouteService,
                _airportDatabase,
                _logger,
                _routeRepository,
                _fboRepository,
                _aircraftRepository,
                _pilotRepository);
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true)
            {
                _viewModel.LoadFBOs();
                LoadFBOMapData();
                UpdateStats();
            }
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
            if (_map == null) return;

            var tileLayer = _map.Layers.FirstOrDefault(l => l is TileLayer);
            if (tileLayer != null)
            {
                _map.Layers.Remove(tileLayer);
            }

            var newTileLayer = MapTileHelper.CreateTileLayer(layerType);
            _map.Layers.Insert(0, newTileLayer);

            _settingsService.CurrentSettings.MapLayer = layerType.ToString();
            _settingsService.Save();

            UpdateMapLayerButtons(layerType);
            UpdateLegendVisibility(layerType);
            MapAttribution.SetMapLayer(layerType);

            _logger.Info($"FBOView: Switched to {layerType} map layer");
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

        private void LoadAirportImage(string icao)
        {
            var customImage = AirportImageHelper.LoadCustomImage(icao);

            if (customImage != null)
            {
                ImgAirport.Source = customImage;
                ImgAirport.Visibility = Visibility.Visible;
                AirportMapPreview.Visibility = Visibility.Collapsed;
            }
            else
            {
                ImgAirport.Source = null;
                ImgAirport.Visibility = Visibility.Collapsed;
                ShowAirportMapPreview(icao);
            }
        }

        private void ShowAirportMapPreview(string icao)
        {
            var airport = _airportDatabase.GetAirport(icao);
            if (airport == null)
            {
                AirportMapPreview.Visibility = Visibility.Collapsed;
                return;
            }

            var previewMap = new Map();
            previewMap.Widgets.Clear();

            previewMap.Layers.Add(MapTileHelper.CreateTileLayer(MapLayerType.Terrain));

            AirportMapPreview.Map = previewMap;
            AirportMapPreview.Visibility = Visibility.Visible;

            var airportDetail = _airportDatabase.GetAirportDetail(icao);
            var extent = CalculateAirportExtent(airport, airportDetail);

            previewMap.Navigator.ZoomToBox(extent, MBoxFit.Fit);
        }

        private static MRect CalculateAirportExtent(Airport airport, AirportDetail? detail)
        {
            var center = SphericalMercator.FromLonLat(airport.Longitude, airport.Latitude);

            double runwayLengthMeters = airport.LongestRunwayFt * 0.3048;
            double headingRad = 0;

            if (detail?.Runways.Count > 0)
            {
                var longestRunway = detail.Runways.OrderByDescending(r => r.LengthFt).First();
                runwayLengthMeters = longestRunway.LengthFt * 0.3048;
                headingRad = longestRunway.Heading * Math.PI / 180.0;
            }

            double padding = runwayLengthMeters * 0.6;
            double totalSize = runwayLengthMeters + padding * 2;

            double halfExtentAlongRunway = totalSize / 2;
            double halfExtentPerpendicular = totalSize / 2;

            double dx1 = halfExtentAlongRunway * Math.Sin(headingRad);
            double dy1 = halfExtentAlongRunway * Math.Cos(headingRad);
            double dx2 = halfExtentPerpendicular * Math.Cos(headingRad);
            double dy2 = halfExtentPerpendicular * Math.Sin(headingRad);

            double maxDx = Math.Max(Math.Abs(dx1), Math.Abs(dx2)) + padding;
            double maxDy = Math.Max(Math.Abs(dy1), Math.Abs(dy2)) + padding;

            return new MRect(
                center.x - maxDx,
                center.y - maxDy,
                center.x + maxDx,
                center.y + maxDy
            );
        }

        private void AirportImage_Click(object sender, MouseButtonEventArgs e)
        {
            if (_selectedFBO == null) return;

            var dialog = new OpenFileDialog
            {
                Title = $"Select Airport Image for {_selectedFBO.ICAO}",
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp|All Files|*.*",
                CheckFileExists = true
            };

            if (dialog.ShowDialog() != true) return;

            try
            {
                AirportImageHelper.EnsureDirectoryExists();

                var destPath = AirportImageHelper.GetDefaultImagePath(_selectedFBO.ICAO);
                System.IO.File.Copy(dialog.FileName, destPath, true);

                LoadAirportImage(_selectedFBO.ICAO);

                _logger.Info($"FBOView: Airport image updated for {_selectedFBO.ICAO}");
            }
            catch (Exception ex)
            {
                _logger.Error($"FBOView: Failed to copy airport image for {_selectedFBO.ICAO}", ex);
                var errorDialog = new MessageDialog("Error", "Failed to save airport image.");
                errorDialog.Owner = Window.GetWindow(this);
                errorDialog.ShowDialog();
            }
        }

        private void MeasureDistanceButton_Click(object sender, RoutedEventArgs e)
        {
            _isMeasuringDistance = !_isMeasuringDistance;
            _measureStartPoint = null;

            if (_isMeasuringDistance)
            {
                MeasureDistanceButton.Opacity = 1.0;
                MapHintText.Text = "Click on map to set first measurement point";
                MapControl.MouseLeftButtonUp -= MapControl_MouseLeftButtonUp;
                MapControl.MouseLeftButtonUp += OnMapLeftClickForMeasure;
                ClearMeasurementLine();
                MeasureDistancePanel.Visibility = Visibility.Collapsed;
                _logger.Info("FBOView: Distance measurement mode enabled");
            }
            else
            {
                MeasureDistanceButton.Opacity = 0.6;
                MapHintText.Text = "Right-click airport to rent FBO • Click FBO to edit • Scroll to zoom";
                MapControl.MouseLeftButtonUp -= OnMapLeftClickForMeasure;
                MapControl.MouseLeftButtonUp += MapControl_MouseLeftButtonUp;
                ClearMeasurementLine();
                MeasureDistancePanel.Visibility = Visibility.Collapsed;
                _logger.Info("FBOView: Distance measurement mode disabled");
            }
        }

        private void OnMapLeftClickForMeasure(object sender, MouseButtonEventArgs e)
        {
            if (!_isMeasuringDistance) return;

            var pos = e.GetPosition(MapControl);
            var worldPosition = MapControl.Map?.Navigator.Viewport.ScreenToWorld(pos.X, pos.Y);

            if (worldPosition == null) return;

            if (_measureStartPoint == null)
            {
                _measureStartPoint = worldPosition;
                MapHintText.Text = "Click on map to set second measurement point";
                _logger.Debug($"FBOView: Measurement start point set at {worldPosition.X:F0}, {worldPosition.Y:F0}");
            }
            else
            {
                var startLonLat = SphericalMercator.ToLonLat(_measureStartPoint.X, _measureStartPoint.Y);
                var endLonLat = SphericalMercator.ToLonLat(worldPosition.X, worldPosition.Y);

                var distanceNM = CalculateDistanceNM(startLonLat.lat, startLonLat.lon, endLonLat.lat, endLonLat.lon);
                var distanceKm = distanceNM * 1.852;

                DrawMeasurementLine(_measureStartPoint, worldPosition);

                MeasureDistanceText.Text = $"Distance: {distanceNM:F1} NM ({distanceKm:F1} km)";
                MeasureDistancePanel.Visibility = Visibility.Visible;

                MapHintText.Text = "Click on map to start new measurement";
                _measureStartPoint = null;

                _logger.Info($"FBOView: Measured distance: {distanceNM:F1} NM");
            }
        }

        private void ClearMeasurement_Click(object sender, RoutedEventArgs e)
        {
            _measureStartPoint = null;
            ClearMeasurementLine();
            MeasureDistancePanel.Visibility = Visibility.Collapsed;
            MapHintText.Text = _isMeasuringDistance
                ? "Click on map to set first measurement point"
                : "Right-click airport to rent FBO • Click FBO to edit • Scroll to zoom";
        }

        private void DrawMeasurementLine(MPoint start, MPoint end)
        {
            if (_measurementLayer == null) return;

            var line = new LineString(new[]
            {
                new Coordinate(start.X, start.Y),
                new Coordinate(end.X, end.Y)
            });

            var feature = new GeometryFeature(line);
            feature.Styles.Add(new MapsuiStyles.VectorStyle
            {
                Line = new MapsuiStyles.Pen(MapsuiStyles.Color.FromString("#3B82F6"), 3)
                {
                    PenStyle = MapsuiStyles.PenStyle.Dash
                }
            });

            var startFeature = new PointFeature(start);
            startFeature.Styles.Add(new MapsuiStyles.SymbolStyle
            {
                SymbolScale = 0.5,
                Fill = new MapsuiStyles.Brush(MapsuiStyles.Color.FromString("#3B82F6")),
                Outline = new MapsuiStyles.Pen(MapsuiStyles.Color.White, 2),
                SymbolType = MapsuiStyles.SymbolType.Ellipse
            });

            var endFeature = new PointFeature(end);
            endFeature.Styles.Add(new MapsuiStyles.SymbolStyle
            {
                SymbolScale = 0.5,
                Fill = new MapsuiStyles.Brush(MapsuiStyles.Color.FromString("#3B82F6")),
                Outline = new MapsuiStyles.Pen(MapsuiStyles.Color.White, 2),
                SymbolType = MapsuiStyles.SymbolType.Ellipse
            });

            _measurementLayer.Features = new List<IFeature> { feature, startFeature, endFeature };
            _measurementLayer.DataHasChanged();
        }

        private void ClearMeasurementLine()
        {
            if (_measurementLayer == null) return;
            _measurementLayer.Features = new List<IFeature>();
            _measurementLayer.DataHasChanged();
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
}
