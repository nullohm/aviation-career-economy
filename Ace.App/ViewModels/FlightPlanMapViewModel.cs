using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.Tiling.Layers;
using NetTopologySuite.Geometries;
using Ace.App.Data;
using Ace.App.Helpers;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;
using Color = Mapsui.Styles.Color;

namespace Ace.App.ViewModels
{
    public enum AirportSelectionMode
    {
        Departure,
        Arrival
    }

    public class FlightPlanMapViewModel : INotifyPropertyChanged
    {
        private readonly IAirportDatabase _airportDatabase;
        private readonly ILoggingService _loggingService;
        private readonly IScheduledRouteService _scheduledRouteService;
        private readonly ISettingsService _settingsService;
        private readonly IAirspaceService _airspaceService;

        private Map? _map;
        private MemoryLayer? _airportLayer;
        private MemoryLayer? _fboLayer;
        private MemoryLayer? _routePreviewLayer;
        private MemoryLayer? _rangeCircleLayer;
        private MemoryLayer? _selectionLayer;
        private MemoryLayer? _airspaceLayer;
        private MemoryLayer? _measurementLayer;
        private AirportSelectionMode _selectionMode = AirportSelectionMode.Departure;
        private string? _departureIcao;
        private string? _arrivalIcao;
        private List<Airport> _allAirports = new();
        private Dictionary<string, FBO> _fbosByIcao = new();

        public event Action<Airport, AirportSelectionMode>? AirportSelected;

        public Map? Map
        {
            get => _map;
            set { _map = value; OnPropertyChanged(); }
        }

        public AirportSelectionMode SelectionMode
        {
            get => _selectionMode;
            set { _selectionMode = value; OnPropertyChanged(); }
        }

        public FlightPlanMapViewModel(
            IAirportDatabase airportDatabase,
            ILoggingService loggingService,
            IScheduledRouteService scheduledRouteService,
            ISettingsService settingsService,
            IAirspaceService airspaceService)
        {
            _airportDatabase = airportDatabase ?? throw new ArgumentNullException(nameof(airportDatabase));
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _scheduledRouteService = scheduledRouteService ?? throw new ArgumentNullException(nameof(scheduledRouteService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _airspaceService = airspaceService ?? throw new ArgumentNullException(nameof(airspaceService));

            InitializeMap();
            LoadAirportsAndFBOs();
        }

        private void InitializeMap()
        {
            _map = new Map();
            _map.Widgets.Clear();

            var savedLayer = Enum.TryParse<MapLayerType>(_settingsService.CurrentSettings.MapLayer, out var layer)
                ? layer
                : MapLayerType.Street;
            _map.Layers.Add(MapTileHelper.CreateTileLayer(savedLayer));

            _airspaceLayer = new MemoryLayer
            {
                Name = "Airspaces",
                Style = null
            };
            _map.Layers.Add(_airspaceLayer);

            if (_settingsService.CurrentSettings.ShowAirspaceOverlay)
            {
                UpdateAirspaceLayer();
            }

            _airportLayer = new MemoryLayer
            {
                Name = "Airports",
                Style = null
            };
            _map.Layers.Add(_airportLayer);

            _fboLayer = new MemoryLayer
            {
                Name = "FBOs",
                Style = null
            };
            _map.Layers.Add(_fboLayer);

            // Range circle layer (below route so route is visible)
            _rangeCircleLayer = new MemoryLayer
            {
                Name = "RangeCircle",
                Style = null
            };
            _map.Layers.Add(_rangeCircleLayer);

            // Route layer above airports so it's always visible
            _routePreviewLayer = new MemoryLayer
            {
                Name = "RoutePreview",
                Style = null
            };
            _map.Layers.Add(_routePreviewLayer);

            // Selection layer on top
            _selectionLayer = new MemoryLayer
            {
                Name = "Selection",
                Style = null
            };
            _map.Layers.Add(_selectionLayer);

            // Measurement layer
            _measurementLayer = new MemoryLayer
            {
                Name = "Measurement",
                Style = null
            };
            _map.Layers.Add(_measurementLayer);

            _map.Navigator.ViewportChanged += (s, e) => UpdateAirportMarkersBasedOnZoom();

            RestoreMapPosition();

            _loggingService.Info("FlightPlanMapViewModel: Map initialized");
        }

        private void LoadAirportsAndFBOs()
        {
            try
            {
                using var context = new AceDbContext();
                _fbosByIcao = context.FBOs.ToDictionary(f => f.ICAO, f => f);

                _allAirports = _airportDatabase.GetAllAirports().ToList();

                UpdateAirportMarkers();

                _loggingService.Info($"FlightPlanMapViewModel: Loaded {_allAirports.Count} airports, {_fbosByIcao.Count} FBOs");
            }
            catch (Exception ex)
            {
                _loggingService.Error("FlightPlanMapViewModel: Error loading airports", ex);
            }
        }

        private void UpdateAirportMarkers()
        {
            UpdateAirportMarkersBasedOnZoom();
        }

        private void UpdateAirportMarkersBasedOnZoom()
        {
            if (_airportLayer == null || _fboLayer == null || _map == null) return;

            var resolution = _map.Navigator.Viewport.Resolution;
            var minRunwayLength = MapZoomFilterHelper.GetMinRunwayLengthForZoom(resolution);

            _loggingService.Debug($"FlightPlanMapViewModel: UpdateAirportMarkersBasedOnZoom - Resolution: {resolution:F0}, MinRunwayLength: {minRunwayLength}");

            var airportFeatures = new List<IFeature>();
            var fboFeatures = new List<IFeature>();
            var settings = _settingsService.CurrentSettings;

            foreach (var airport in _allAirports)
            {
                if (_fbosByIcao.TryGetValue(airport.Icao, out var fbo))
                {
                    var outgoingRouteCount = _scheduledRouteService.GetOutgoingRouteCountForFBO(fbo.Id);
                    var maxRouteSlots = fbo.Type switch
                    {
                        FBOType.Local => settings.RouteSlotLimitLocal,
                        FBOType.Regional => settings.RouteSlotLimitRegional,
                        FBOType.International => settings.RouteSlotLimitInternational,
                        _ => settings.RouteSlotLimitLocal
                    };
                    fboFeatures.Add(CreateFBOFeature(airport, fbo, outgoingRouteCount, maxRouteSlots));
                }
                else
                {
                    if (MapZoomFilterHelper.ShouldShowNormalAirport(airport.LongestRunwayFt, minRunwayLength))
                    {
                        airportFeatures.Add(CreateAirportFeature(airport));
                    }
                }
            }

            _airportLayer.Features = airportFeatures;
            _fboLayer.Features = fboFeatures;

            _airportLayer.DataHasChanged();
            _fboLayer.DataHasChanged();
        }

        private IFeature CreateAirportFeature(Airport airport)
        {
            var point = SphericalMercator.FromLonLat(airport.Longitude, airport.Latitude).ToMPoint();
            var feature = new PointFeature(point);

            feature["icao"] = airport.Icao;
            feature["name"] = airport.Name;
            feature["isFbo"] = false;

            var (scale, color, showLabel) = AirportSymbolHelper.GetAirportStyle(airport.LongestRunwayFt);

            feature.Styles.Add(new SymbolStyle
            {
                SymbolScale = scale,
                Fill = new Brush(color),
                Outline = new Pen(Color.White, 1),
                SymbolType = SymbolType.Ellipse
            });

            if (showLabel)
            {
                feature.Styles.Add(new LabelStyle
                {
                    Text = airport.Icao,
                    ForeColor = Color.White,
                    BackColor = new Brush(Color.FromArgb(180, 0, 0, 0)),
                    Offset = new Offset(0, -20),
                    Font = new Font { Size = 9, Bold = true },
                    Halo = new Pen(Color.Black, 1)
                });
            }

            return feature;
        }

        private IFeature CreateFBOFeature(Airport airport, FBO fbo, int outgoingRouteCount, int maxRouteSlots)
        {
            var point = SphericalMercator.FromLonLat(airport.Longitude, airport.Latitude).ToMPoint();
            var feature = new PointFeature(point);

            feature["icao"] = airport.Icao;
            feature["name"] = airport.Name;
            feature["isFbo"] = true;

            var symbolType = fbo.Type switch
            {
                FBOType.Local => SymbolType.Triangle,
                FBOType.Regional => SymbolType.Rectangle,
                FBOType.International => SymbolType.Ellipse,
                _ => SymbolType.Rectangle
            };

            Color color;
            if (fbo.TerminalSize == TerminalSize.None)
            {
                color = Color.FromString("#EF4444");
            }
            else if (outgoingRouteCount >= maxRouteSlots)
            {
                color = Color.FromString("#22C55E");
            }
            else
            {
                color = Color.FromString("#EAB308");
            }

            feature.Styles.Add(new SymbolStyle
            {
                SymbolScale = 0.8,
                Fill = new Brush(color),
                Outline = new Pen(Color.Black, 2),
                SymbolType = symbolType
            });

            feature.Styles.Add(new LabelStyle
            {
                Text = airport.Icao,
                ForeColor = Color.White,
                BackColor = new Brush(Color.FromArgb(200, 0, 0, 0)),
                Offset = new Offset(0, -25),
                Font = new Font { Size = 10, Bold = true },
                Halo = new Pen(Color.Black, 1)
            });

            return feature;
        }

        public void HandleMapInfo(MapInfo? mapInfo)
        {
            if (mapInfo?.Feature == null) return;

            var icao = mapInfo.Feature["icao"]?.ToString();
            if (string.IsNullOrEmpty(icao)) return;

            var airport = _airportDatabase.GetAirport(icao);
            if (airport == null) return;

            _loggingService.Info($"FlightPlanMapViewModel: Airport clicked - {icao} ({airport.Name}), Mode: {_selectionMode}");

            if (_selectionMode == AirportSelectionMode.Departure)
            {
                _departureIcao = icao;
            }
            else
            {
                _arrivalIcao = icao;
            }

            UpdateSelectionMarkers();
            UpdateRoutePreview();

            AirportSelected?.Invoke(airport, _selectionMode);
        }

        public void SetDepartureIcao(string? icao)
        {
            _departureIcao = icao;
            UpdateSelectionMarkers();
            UpdateRoutePreview();

            if (!string.IsNullOrEmpty(icao))
            {
                CenterOnAirport(icao);
            }
        }

        public void UpdateRangeCircle(int? aircraftRangeNM)
        {
            if (_rangeCircleLayer == null) return;

            if (string.IsNullOrEmpty(_departureIcao) || !aircraftRangeNM.HasValue || aircraftRangeNM.Value <= 0)
            {
                _rangeCircleLayer.Features = Array.Empty<IFeature>();
                _rangeCircleLayer.DataHasChanged();
                return;
            }

            var depAirport = _airportDatabase.GetAirport(_departureIcao);
            if (depAirport == null)
            {
                _rangeCircleLayer.Features = Array.Empty<IFeature>();
                _rangeCircleLayer.DataHasChanged();
                return;
            }

            var circle = CreateGeodesicCircle(depAirport.Latitude, depAirport.Longitude, aircraftRangeNM.Value, 72);

            var circleFeature = new GeometryFeature(circle);
            circleFeature.Styles.Add(new VectorStyle
            {
                Fill = new Brush(Color.FromArgb(30, 33, 150, 243)),
                Outline = new Pen(Color.FromArgb(180, 33, 150, 243), 2)
                {
                    PenStyle = PenStyle.Dash,
                    DashArray = new[] { 8f, 4f }
                }
            });

            _rangeCircleLayer.Features = new[] { circleFeature };
            _rangeCircleLayer.DataHasChanged();

            _loggingService.Debug($"FlightPlanMapViewModel: Range circle updated for {_departureIcao} with range {aircraftRangeNM} NM");
        }

        private Polygon CreateGeodesicCircle(double centerLatDeg, double centerLonDeg, double radiusNM, int segments)
        {
            const double EarthRadiusNM = 3440.065;
            var coordinates = new Coordinate[segments + 1];

            double lat1 = centerLatDeg * Math.PI / 180.0;
            double lon1 = centerLonDeg * Math.PI / 180.0;
            double angularDist = radiusNM / EarthRadiusNM;

            for (int i = 0; i < segments; i++)
            {
                double bearing = 2.0 * Math.PI * i / segments;

                double lat2 = Math.Asin(
                    Math.Sin(lat1) * Math.Cos(angularDist) +
                    Math.Cos(lat1) * Math.Sin(angularDist) * Math.Cos(bearing));

                double lon2 = lon1 + Math.Atan2(
                    Math.Sin(bearing) * Math.Sin(angularDist) * Math.Cos(lat1),
                    Math.Cos(angularDist) - Math.Sin(lat1) * Math.Sin(lat2));

                double destLatDeg = lat2 * 180.0 / Math.PI;
                double destLonDeg = lon2 * 180.0 / Math.PI;

                var mercator = SphericalMercator.FromLonLat(destLonDeg, destLatDeg);
                coordinates[i] = new Coordinate(mercator.x, mercator.y);
            }

            coordinates[segments] = coordinates[0];
            return new Polygon(new LinearRing(coordinates));
        }

        public void SetArrivalIcao(string? icao)
        {
            _arrivalIcao = icao;
            UpdateSelectionMarkers();
            UpdateRoutePreview();
        }

        private void UpdateSelectionMarkers()
        {
            if (_selectionLayer == null) return;

            var features = new List<IFeature>();

            if (!string.IsNullOrEmpty(_departureIcao))
            {
                var depAirport = _airportDatabase.GetAirport(_departureIcao);
                if (depAirport != null)
                {
                    features.Add(CreateSelectionMarker(depAirport, true));
                }
            }

            if (!string.IsNullOrEmpty(_arrivalIcao))
            {
                var arrAirport = _airportDatabase.GetAirport(_arrivalIcao);
                if (arrAirport != null)
                {
                    features.Add(CreateSelectionMarker(arrAirport, false));
                }
            }

            _selectionLayer.Features = features;
            _selectionLayer.DataHasChanged();
        }

        private IFeature CreateSelectionMarker(Airport airport, bool isDeparture)
        {
            var point = SphericalMercator.FromLonLat(airport.Longitude, airport.Latitude).ToMPoint();
            var feature = new PointFeature(point);

            feature["icao"] = airport.Icao;

            var color = isDeparture ? Color.FromString("#4CAF50") : Color.FromString("#E91E63");
            var label = isDeparture ? "DEP" : "ARR";

            feature.Styles.Add(new SymbolStyle
            {
                SymbolScale = 1.0,
                Fill = new Brush(color),
                Outline = new Pen(Color.White, 3),
                SymbolType = SymbolType.Ellipse
            });

            feature.Styles.Add(new LabelStyle
            {
                Text = $"{label}: {airport.Icao}",
                ForeColor = Color.White,
                BackColor = new Brush(color),
                Offset = new Offset(0, -30),
                Font = new Font { Size = 12, Bold = true },
                Halo = new Pen(Color.Black, 1)
            });

            return feature;
        }

        private void UpdateRoutePreview()
        {
            if (_routePreviewLayer == null) return;

            if (string.IsNullOrEmpty(_departureIcao) || string.IsNullOrEmpty(_arrivalIcao))
            {
                _routePreviewLayer.Features = Array.Empty<IFeature>();
                _routePreviewLayer.DataHasChanged();
                return;
            }

            var depAirport = _airportDatabase.GetAirport(_departureIcao);
            var arrAirport = _airportDatabase.GetAirport(_arrivalIcao);

            if (depAirport == null || arrAirport == null)
            {
                _routePreviewLayer.Features = Array.Empty<IFeature>();
                _routePreviewLayer.DataHasChanged();
                return;
            }

            var features = new List<IFeature>();

            // Create route line
            var depPoint = SphericalMercator.FromLonLat(depAirport.Longitude, depAirport.Latitude);
            var arrPoint = SphericalMercator.FromLonLat(arrAirport.Longitude, arrAirport.Latitude);

            var line = new LineString(new[]
            {
                new Coordinate(depPoint.x, depPoint.y),
                new Coordinate(arrPoint.x, arrPoint.y)
            });

            var routeFeature = new GeometryFeature(line);
            routeFeature.Styles.Add(new VectorStyle
            {
                Line = new Pen(Color.FromString("#E91E63"), 3)
                {
                    PenStyle = PenStyle.Dash,
                    DashArray = new[] { 4f, 4f }
                }
            });
            features.Add(routeFeature);

            // Add direction arrows
            double dx = arrPoint.x - depPoint.x;
            double dy = arrPoint.y - depPoint.y;
            double bearing = Math.Atan2(dx, dy) * 180 / Math.PI;

            var positions = new[] { 0.25, 0.5, 0.75 };
            foreach (var pos in positions)
            {
                double x = depPoint.x + dx * pos;
                double y = depPoint.y + dy * pos;

                var arrowFeature = new PointFeature(new MPoint(x, y));
                arrowFeature.Styles.Add(new SymbolStyle
                {
                    SymbolScale = 0.5,
                    Fill = new Brush(Color.FromString("#E91E63")),
                    Outline = new Pen(Color.White, 1),
                    SymbolType = SymbolType.Triangle,
                    SymbolRotation = bearing
                });
                features.Add(arrowFeature);
            }

            _routePreviewLayer.Features = features;
            _routePreviewLayer.DataHasChanged();
        }

        public void CenterOnAirport(string icao)
        {
            if (_map == null) return;

            var airport = _airportDatabase.GetAirport(icao);
            if (airport == null) return;

            var point = SphericalMercator.FromLonLat(airport.Longitude, airport.Latitude);
            _map.Navigator.CenterOn(point.x, point.y);
            _map.Navigator.ZoomTo(10);
        }

        public void ZoomToRoute()
        {
            if (_map == null || string.IsNullOrEmpty(_departureIcao) || string.IsNullOrEmpty(_arrivalIcao)) return;

            var depAirport = _airportDatabase.GetAirport(_departureIcao);
            var arrAirport = _airportDatabase.GetAirport(_arrivalIcao);

            if (depAirport == null || arrAirport == null) return;

            var minLat = Math.Min(depAirport.Latitude, arrAirport.Latitude) - 0.5;
            var maxLat = Math.Max(depAirport.Latitude, arrAirport.Latitude) + 0.5;
            var minLon = Math.Min(depAirport.Longitude, arrAirport.Longitude) - 0.5;
            var maxLon = Math.Max(depAirport.Longitude, arrAirport.Longitude) + 0.5;

            var min = SphericalMercator.FromLonLat(minLon, minLat);
            var max = SphericalMercator.FromLonLat(maxLon, maxLat);

            var extent = new MRect(min.x, min.y, max.x, max.y);
            _map.Navigator.ZoomToBox(extent, MBoxFit.Fit);
        }

        public void RefreshMapLayer(MapLayerType layerType)
        {
            if (_map == null) return;

            var tileLayer = _map.Layers.FirstOrDefault(l => l is TileLayer);
            if (tileLayer != null)
            {
                _map.Layers.Remove(tileLayer);
            }

            var newTileLayer = MapTileHelper.CreateTileLayer(layerType);
            _map.Layers.Insert(0, newTileLayer);

            _loggingService.Info($"FlightPlanMapViewModel: Switched to {layerType} layer");
        }

        public void SetAirspaceVisibility(bool visible)
        {
            if (_airspaceLayer == null) return;

            if (visible)
            {
                UpdateAirspaceLayer();
            }
            else
            {
                _airspaceLayer.Features = Array.Empty<IFeature>();
                _airspaceLayer.DataHasChanged();
            }

            _loggingService.Info($"FlightPlanMapViewModel: Airspace visibility set to {visible}");
        }

        public void SetAirportVisibility(bool visible)
        {
            if (_airportLayer == null) return;

            if (visible)
            {
                UpdateAirportMarkers();
            }
            else
            {
                _airportLayer.Features = Array.Empty<IFeature>();
                _airportLayer.DataHasChanged();
            }

            _loggingService.Info($"FlightPlanMapViewModel: Airport visibility set to {visible}");
        }

        public void RefreshAirspaces()
        {
            _airspaceService.RefreshAirspaces();
            if (_settingsService.CurrentSettings.ShowAirspaceOverlay)
            {
                UpdateAirspaceLayer();
            }
        }

        public void UpdateAirspaceFilter()
        {
            if (_settingsService.CurrentSettings.ShowAirspaceOverlay)
            {
                UpdateAirspaceLayer();
            }
        }

        private void UpdateAirspaceLayer()
        {
            if (_airspaceLayer == null) return;

            var airspaces = _airspaceService.GetAllAirspaces();
            var features = new List<IFeature>();

            foreach (var airspace in airspaces)
            {
                if (!IsAirspaceClassVisible(airspace.Class))
                    continue;

                var feature = CreateAirspaceFeature(airspace);
                if (feature != null)
                {
                    features.Add(feature);
                }
            }

            _airspaceLayer.Features = features;
            _airspaceLayer.DataHasChanged();

            _loggingService.Info($"FlightPlanMapViewModel: Loaded {features.Count} airspace features from {airspaces.Count} airspaces");
        }

        private bool IsAirspaceClassVisible(AirspaceClass airspaceClass)
        {
            var settings = _settingsService.CurrentSettings;
            return airspaceClass switch
            {
                AirspaceClass.A => settings.ShowAirspaceClassA,
                AirspaceClass.B => settings.ShowAirspaceClassB,
                AirspaceClass.C => settings.ShowAirspaceClassC,
                AirspaceClass.D => settings.ShowAirspaceClassD,
                AirspaceClass.E => settings.ShowAirspaceClassE,
                AirspaceClass.CTR or AirspaceClass.TMA or AirspaceClass.ATZ => settings.ShowAirspaceCTR,
                AirspaceClass.Restricted or AirspaceClass.RMZ or AirspaceClass.TMZ => settings.ShowAirspaceRestricted,
                AirspaceClass.Prohibited => settings.ShowAirspaceProhibited,
                AirspaceClass.Danger => settings.ShowAirspaceDanger,
                AirspaceClass.Glider or AirspaceClass.Wave => settings.ShowAirspaceGlider,
                _ => settings.ShowAirspaceOther
            };
        }

        private IFeature? CreateAirspaceFeature(Airspace airspace)
        {
            try
            {
                if (airspace.IsCircle && airspace.CircleCenter != null)
                {
                    var circle = CreateGeodesicCircle(
                        airspace.CircleCenter.Latitude,
                        airspace.CircleCenter.Longitude,
                        airspace.CircleRadiusNM,
                        36);

                    var feature = new GeometryFeature(circle);
                    feature.Styles.Add(AirspaceStyleHelper.GetAirspaceStyle(airspace.Class));
                    feature["name"] = airspace.Name;
                    feature["class"] = airspace.Class.ToString();
                    feature["lowerAltitude"] = airspace.LowerAltitude;
                    feature["upperAltitude"] = airspace.UpperAltitude;
                    return feature;
                }
                else if (airspace.Coordinates.Count >= 3)
                {
                    if (!IsValidAirspaceForRendering(airspace))
                        return null;

                    var coordinates = airspace.Coordinates
                        .Select(c =>
                        {
                            var mercator = SphericalMercator.FromLonLat(c.Longitude, c.Latitude);
                            return new Coordinate(mercator.x, mercator.y);
                        })
                        .ToList();

                    coordinates.Add(coordinates[0]);

                    var ring = new LinearRing(coordinates.ToArray());
                    var polygon = new Polygon(ring);

                    var feature = new GeometryFeature(polygon);
                    feature.Styles.Add(AirspaceStyleHelper.GetAirspaceStyle(airspace.Class));
                    feature["name"] = airspace.Name;
                    feature["class"] = airspace.Class.ToString();
                    feature["lowerAltitude"] = airspace.LowerAltitude;
                    feature["upperAltitude"] = airspace.UpperAltitude;
                    return feature;
                }
            }
            catch (Exception ex)
            {
                _loggingService.Debug($"FlightPlanMapViewModel: Failed to create airspace feature for {airspace.Name}: {ex.Message}");
            }

            return null;
        }

        private static bool IsValidAirspaceForRendering(Airspace airspace)
        {
            var lons = airspace.Coordinates.Select(c => c.Longitude).ToList();
            var lats = airspace.Coordinates.Select(c => c.Latitude).ToList();

            var lonSpan = lons.Max() - lons.Min();
            var latSpan = lats.Max() - lats.Min();

            if (lonSpan > 5 || latSpan > 5)
                return false;

            if (lons.Any(l => l > 170) && lons.Any(l => l < -170))
                return false;

            for (int i = 1; i < airspace.Coordinates.Count; i++)
            {
                var dlon = Math.Abs(airspace.Coordinates[i].Longitude - airspace.Coordinates[i - 1].Longitude);
                var dlat = Math.Abs(airspace.Coordinates[i].Latitude - airspace.Coordinates[i - 1].Latitude);
                if (dlon > 2 || dlat > 2)
                    return false;
            }

            return true;
        }

        public void ZoomToAirport(double latitude, double longitude, double zoomLevel = 12)
        {
            if (_map == null) return;

            var point = SphericalMercator.FromLonLat(longitude, latitude);
            _map.Navigator.CenterOn(point.x, point.y);
            _map.Navigator.ZoomTo(zoomLevel);

            _loggingService.Debug($"FlightPlanMapViewModel: Zoomed to {latitude:F4}, {longitude:F4}");
        }

        private void RestoreMapPosition()
        {
            if (_map == null) return;

            var settings = _settingsService.CurrentSettings;
            var point = SphericalMercator.FromLonLat(settings.MapCenterLongitude, settings.MapCenterLatitude);
            _map.Navigator.CenterOn(point.x, point.y);
            _map.Navigator.ZoomTo(settings.MapZoomLevel);

            _loggingService.Info($"FlightPlanMapViewModel: Restored map position to {settings.MapCenterLatitude:F4}, {settings.MapCenterLongitude:F4} at zoom {settings.MapZoomLevel:F1}");
        }

        public void SaveMapPosition()
        {
            if (_map == null) return;

            (double lon, double lat) center;
            if (_map.Navigator.Viewport.CenterX != 0 || _map.Navigator.Viewport.CenterY != 0)
            {
                center = SphericalMercator.ToLonLat(_map.Navigator.Viewport.CenterX, _map.Navigator.Viewport.CenterY);
            }
            else
            {
                center = (lon: 10.0, lat: 50.0);
            }

            var settings = _settingsService.CurrentSettings;
            settings.MapCenterLatitude = center.lat;
            settings.MapCenterLongitude = center.lon;
            settings.MapZoomLevel = _map.Navigator.Viewport.Resolution > 0 ? GetZoomLevelFromResolution(_map.Navigator.Viewport.Resolution) : 6.0;

            _settingsService.Save();
            _loggingService.Debug($"FlightPlanMapViewModel: Saved map position {center.lat:F4}, {center.lon:F4} at zoom {settings.MapZoomLevel:F1}");
        }

        private static double GetZoomLevelFromResolution(double resolution)
        {
            return Math.Log(156543.03392 / resolution, 2);
        }

        public void DrawMeasurementLine(MPoint startPoint, MPoint endPoint)
        {
            if (_measurementLayer == null) return;

            var features = new List<IFeature>();

            var line = new LineString(new[]
            {
                new Coordinate(startPoint.X, startPoint.Y),
                new Coordinate(endPoint.X, endPoint.Y)
            });

            var lineFeature = new GeometryFeature(line);
            lineFeature.Styles.Add(new VectorStyle
            {
                Line = new Pen(Color.FromString("#F59E0B"), 3)
                {
                    PenStyle = PenStyle.Dash,
                    DashArray = new[] { 6f, 4f }
                }
            });
            features.Add(lineFeature);

            var startFeature = new PointFeature(startPoint);
            startFeature.Styles.Add(new SymbolStyle
            {
                SymbolScale = 0.6,
                Fill = new Brush(Color.FromString("#F59E0B")),
                Outline = new Pen(Color.White, 2),
                SymbolType = SymbolType.Ellipse
            });
            features.Add(startFeature);

            var endFeature = new PointFeature(endPoint);
            endFeature.Styles.Add(new SymbolStyle
            {
                SymbolScale = 0.6,
                Fill = new Brush(Color.FromString("#F59E0B")),
                Outline = new Pen(Color.White, 2),
                SymbolType = SymbolType.Ellipse
            });
            features.Add(endFeature);

            _measurementLayer.Features = features;
            _measurementLayer.DataHasChanged();
        }

        public void ClearMeasurementLine()
        {
            if (_measurementLayer == null) return;

            _measurementLayer.Features = Array.Empty<IFeature>();
            _measurementLayer.DataHasChanged();
        }

        public (string Name, string Class, string LowerAltitude, string UpperAltitude)? GetAirspaceAtPosition(Mapsui.Manipulations.ScreenPosition screenPosition)
        {
            if (_map == null || _airspaceLayer == null) return null;

            var worldPosition = _map.Navigator.Viewport.ScreenToWorld(screenPosition.X, screenPosition.Y);
            if (worldPosition == null) return null;

            foreach (var feature in _airspaceLayer.Features)
            {
                if (feature is GeometryFeature gf && gf.Geometry != null)
                {
                    var point = new Point(worldPosition.X, worldPosition.Y);
                    if (gf.Geometry.Contains(point) || gf.Geometry.Distance(point) < 1000)
                    {
                        var name = feature["name"]?.ToString();
                        var airspaceClass = feature["class"]?.ToString();
                        var lowerAlt = feature["lowerAltitude"]?.ToString();
                        var upperAlt = feature["upperAltitude"]?.ToString();

                        if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(airspaceClass))
                            return (name ?? "Unknown", airspaceClass ?? "Unknown", lowerAlt ?? "GND", upperAlt ?? "UNL");
                    }
                }
            }

            return null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
