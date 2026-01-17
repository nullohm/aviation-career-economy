using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.Tiling.Layers;
using NetTopologySuite.Geometries;
using Ace.App.Helpers;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;

namespace Ace.App.ViewModels
{
    public class FlightMapViewModel : INotifyPropertyChanged
    {
        private readonly IPersistenceService _persistenceService;
        private readonly IAirportDatabase _airportDatabase;
        private readonly ILoggingService _loggingService;
        private readonly SimConnectService _simConnectService;
        private readonly IActiveFlightPlanService _activeFlightPlanService;
        private readonly ISettingsService _settingsService;

        private Map? _map;
        private MemoryLayer? _airportLayer;
        private MemoryLayer? _flightRouteLayer;
        private MemoryLayer? _aircraftLayer;
        private MemoryLayer? _activeFlightPlanLayer;
        private double _currentLat;
        private double _currentLon;
        private bool _isConnected;

        public Map? Map
        {
            get => _map;
            set { _map = value; OnPropertyChanged(); }
        }

        public bool IsConnected
        {
            get => _isConnected;
            set { _isConnected = value; OnPropertyChanged(); }
        }

        public FlightMapViewModel(
            IPersistenceService persistenceService,
            IAirportDatabase airportDatabase,
            ILoggingService loggingService,
            SimConnectService simConnectService,
            IActiveFlightPlanService activeFlightPlanService,
            ISettingsService settingsService)
        {
            _persistenceService = persistenceService ?? throw new ArgumentNullException(nameof(persistenceService));
            _airportDatabase = airportDatabase ?? throw new ArgumentNullException(nameof(airportDatabase));
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _simConnectService = simConnectService ?? throw new ArgumentNullException(nameof(simConnectService));
            _activeFlightPlanService = activeFlightPlanService ?? throw new ArgumentNullException(nameof(activeFlightPlanService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

            InitializeMap();

            _simConnectService.FlightDataReceived += OnFlightDataReceived;
            _simConnectService.ConnectionChanged += OnConnectionChanged;
            _activeFlightPlanService.FlightPlanChanged += OnFlightPlanChanged;
        }

        private void InitializeMap()
        {
            _map = new Map();
            _map.Widgets.Clear();

            var savedLayer = Enum.TryParse<MapLayerType>(_settingsService.CurrentSettings.MapLayer, out var layer)
                ? layer
                : MapLayerType.Street;
            _map.Layers.Add(MapTileHelper.CreateTileLayer(savedLayer));

            _flightRouteLayer = new MemoryLayer
            {
                Name = "FlightRoutes",
                Style = null
            };
            _map.Layers.Add(_flightRouteLayer);

            _airportLayer = new MemoryLayer
            {
                Name = "Airports",
                Style = null
            };
            _map.Layers.Add(_airportLayer);

            _activeFlightPlanLayer = new MemoryLayer
            {
                Name = "ActiveFlightPlan",
                Style = null
            };
            _map.Layers.Add(_activeFlightPlanLayer);

            _aircraftLayer = new MemoryLayer
            {
                Name = "Aircraft",
                Style = null
            };
            _map.Layers.Add(_aircraftLayer);

            _map.Navigator.ViewportChanged += (s, e) => UpdateAirportMarkersBasedOnZoom();

            _loggingService.Info("FlightMapViewModel: Map initialized");

            // Load active flight plan if exists
            var activePlan = _activeFlightPlanService.GetActivePlan();
            if (activePlan != null)
            {
                UpdateActiveFlightPlanLayer(activePlan);
            }
        }

        private List<Airport> _allVisitedAirports = new();
        private List<FlightRecord> _allFlights = new();

        public void LoadFlightData()
        {
            try
            {
                _allFlights = _persistenceService.LoadFlightRecords();
                if (_allFlights == null || _allFlights.Count == 0)
                {
                    _loggingService.Debug("FlightMapViewModel: No flight records to display");
                    return;
                }

                var visitedAirports = new HashSet<string>();
                _allVisitedAirports.Clear();

                foreach (var flight in _allFlights)
                {
                    var depAirport = _airportDatabase.GetAirport(flight.Departure);
                    var arrAirport = _airportDatabase.GetAirport(flight.Arrival);

                    if (depAirport != null && !visitedAirports.Contains(depAirport.Icao))
                    {
                        visitedAirports.Add(depAirport.Icao);
                        _allVisitedAirports.Add(depAirport);
                    }

                    if (arrAirport != null && !visitedAirports.Contains(arrAirport.Icao))
                    {
                        visitedAirports.Add(arrAirport.Icao);
                        _allVisitedAirports.Add(arrAirport);
                    }
                }

                UpdateAirportMarkersBasedOnZoom();
                UpdateRouteMarkers();

                if (visitedAirports.Count > 0)
                {
                    ZoomToFlights(_allFlights);
                }

                _loggingService.Info($"FlightMapViewModel: Loaded {_allFlights.Count} flights, {visitedAirports.Count} airports");
            }
            catch (Exception ex)
            {
                _loggingService.Error("FlightMapViewModel: Error loading flight data", ex);
            }
        }

        private void UpdateAirportMarkersBasedOnZoom()
        {
            if (_airportLayer == null || _map == null) return;

            var resolution = _map.Navigator.Viewport.Resolution;
            var minRunwayLength = MapZoomFilterHelper.GetMinRunwayLengthForZoom(resolution);

            _loggingService.Debug($"FlightMapViewModel: UpdateAirportMarkersBasedOnZoom - Resolution: {resolution:F0}, MinRunwayLength: {minRunwayLength}");

            var airportFeatures = new List<IFeature>();

            foreach (var airport in _allVisitedAirports)
            {
                if (MapZoomFilterHelper.ShouldShowNormalAirport(airport.LongestRunwayFt, minRunwayLength))
                {
                    var isDeparture = _allFlights.Any(f => f.Departure == airport.Icao);
                    airportFeatures.Add(CreateAirportFeature(airport, isDeparture));
                }
            }

            _airportLayer.Features = airportFeatures;
            _airportLayer.DataHasChanged();
        }

        private void UpdateRouteMarkers()
        {
            if (_flightRouteLayer == null) return;

            var routeFeatures = new List<IFeature>();

            foreach (var flight in _allFlights)
            {
                var depAirport = _airportDatabase.GetAirport(flight.Departure);
                var arrAirport = _airportDatabase.GetAirport(flight.Arrival);

                if (depAirport != null && arrAirport != null)
                {
                    routeFeatures.Add(CreateRouteFeature(depAirport, arrAirport, flight));
                }
            }

            _flightRouteLayer.Features = routeFeatures;
            _flightRouteLayer.DataHasChanged();
        }

        private IFeature CreateAirportFeature(Airport airport, bool isDeparture)
        {
            var point = SphericalMercator.FromLonLat(airport.Longitude, airport.Latitude).ToMPoint();
            var feature = new PointFeature(point);

            feature["name"] = airport.Name;
            feature["icao"] = airport.Icao;
            feature["type"] = isDeparture ? "departure" : "arrival";

            feature.Styles.Add(new SymbolStyle
            {
                SymbolScale = 0.5,
                Fill = new Brush(isDeparture ? Color.FromString("#00E5FF") : Color.FromString("#E91E63")),
                Outline = new Pen(Color.Black, 2),
                SymbolType = SymbolType.Ellipse
            });

            feature.Styles.Add(new LabelStyle
            {
                Text = airport.Icao,
                ForeColor = Color.White,
                BackColor = new Brush(Color.FromArgb(180, 0, 0, 0)),
                Offset = new Offset(0, -20),
                Font = new Font { Size = 10, Bold = true },
                Halo = new Pen(Color.Black, 1)
            });

            return feature;
        }

        private IFeature CreateRouteFeature(Airport departure, Airport arrival, FlightRecord flight)
        {
            var depPoint = SphericalMercator.FromLonLat(departure.Longitude, departure.Latitude);
            var arrPoint = SphericalMercator.FromLonLat(arrival.Longitude, arrival.Latitude);

            var line = new LineString(new[]
            {
                new Coordinate(depPoint.x, depPoint.y),
                new Coordinate(arrPoint.x, arrPoint.y)
            });

            var feature = new GeometryFeature(line);

            feature["departure"] = departure.Icao;
            feature["arrival"] = arrival.Icao;
            feature["distance"] = flight.DistanceNM;
            feature["date"] = flight.Date;

            feature.Styles.Add(new VectorStyle
            {
                Line = new Pen(Color.FromArgb(180, 255, 152, 0), 2)
                {
                    PenStyle = PenStyle.Dash
                }
            });

            return feature;
        }

        private void ZoomToFlights(List<FlightRecord> flights)
        {
            if (_map == null || flights.Count == 0) return;

            double minLat = double.MaxValue, maxLat = double.MinValue;
            double minLon = double.MaxValue, maxLon = double.MinValue;

            foreach (var flight in flights)
            {
                var dep = _airportDatabase.GetAirport(flight.Departure);
                var arr = _airportDatabase.GetAirport(flight.Arrival);

                if (dep != null)
                {
                    minLat = Math.Min(minLat, dep.Latitude);
                    maxLat = Math.Max(maxLat, dep.Latitude);
                    minLon = Math.Min(minLon, dep.Longitude);
                    maxLon = Math.Max(maxLon, dep.Longitude);
                }

                if (arr != null)
                {
                    minLat = Math.Min(minLat, arr.Latitude);
                    maxLat = Math.Max(maxLat, arr.Latitude);
                    minLon = Math.Min(minLon, arr.Longitude);
                    maxLon = Math.Max(maxLon, arr.Longitude);
                }
            }

            if (minLat < double.MaxValue)
            {
                var padding = 0.5;
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

        private void OnFlightDataReceived(FlightData data)
        {
            if (!data.IsConnected) return;

            _currentLat = data.Latitude;
            _currentLon = data.Longitude;

            Application.Current?.Dispatcher.Invoke(() =>
            {
                UpdateAircraftPosition(data);
            });
        }

        private void UpdateAircraftPosition(FlightData data)
        {
            if (_aircraftLayer == null || _map == null) return;

            try
            {
                var point = SphericalMercator.FromLonLat(data.Longitude, data.Latitude).ToMPoint();
                var feature = new PointFeature(point);

                feature["aircraft"] = data.Aircraft;
                feature["altitude"] = data.Altitude;
                feature["speed"] = data.GroundSpeed;
                feature["heading"] = data.Heading;

                feature.Styles.Add(new SymbolStyle
                {
                    SymbolScale = 0.6,
                    Fill = new Brush(Color.FromString("#F44336")),
                    Outline = new Pen(Color.White, 2),
                    SymbolType = SymbolType.Triangle,
                    SymbolRotation = data.Heading
                });

                feature.Styles.Add(new LabelStyle
                {
                    Text = $"{data.Aircraft}\n{data.Altitude:F0} ft | {data.GroundSpeed:F0} kts",
                    ForeColor = Color.White,
                    BackColor = new Brush(Color.FromArgb(200, 244, 67, 54)),
                    Offset = new Offset(0, -35),
                    Font = new Font { Size = 9 }
                });

                _aircraftLayer.Features = new[] { feature };
                _aircraftLayer.DataHasChanged();
            }
            catch (Exception ex)
            {
                _loggingService.Error("FlightMapViewModel: Error updating aircraft position", ex);
            }
        }

        private void OnConnectionChanged(bool connected)
        {
            IsConnected = connected;

            if (!connected && _aircraftLayer != null)
            {
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    _aircraftLayer.Features = Array.Empty<IFeature>();
                    _aircraftLayer.DataHasChanged();
                });
            }
        }

        public void CenterOnAircraft()
        {
            if (_map == null || !_isConnected) return;

            var point = SphericalMercator.FromLonLat(_currentLon, _currentLat);
            _map.Navigator.CenterOn(point.x, point.y);
        }

        private void OnFlightPlanChanged(ActiveFlightPlan? flightPlan)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                UpdateActiveFlightPlanLayer(flightPlan);
            });
        }

        private void UpdateActiveFlightPlanLayer(ActiveFlightPlan? flightPlan)
        {
            if (_activeFlightPlanLayer == null) return;

            if (flightPlan == null)
            {
                _activeFlightPlanLayer.Features = Array.Empty<IFeature>();
                _activeFlightPlanLayer.DataHasChanged();
                _loggingService.Debug("FlightMapViewModel: Active flight plan cleared from map");
                return;
            }

            var depAirport = _airportDatabase.GetAirport(flightPlan.DepartureIcao);
            var arrAirport = _airportDatabase.GetAirport(flightPlan.ArrivalIcao);

            if (depAirport == null || arrAirport == null)
            {
                _loggingService.Warn($"FlightMapViewModel: Could not find airports for flight plan {flightPlan.DepartureIcao} -> {flightPlan.ArrivalIcao}");
                return;
            }

            var features = new List<IFeature>();

            // Create route line with dashes and arrows
            features.Add(CreateActiveFlightPlanRoute(depAirport, arrAirport));

            // Create departure marker (green)
            features.Add(CreateActiveFlightPlanAirportMarker(depAirport, true));

            // Create arrival marker (magenta/pink)
            features.Add(CreateActiveFlightPlanAirportMarker(arrAirport, false));

            // Add direction arrows along the route
            features.AddRange(CreateRouteArrows(depAirport, arrAirport));

            _activeFlightPlanLayer.Features = features;
            _activeFlightPlanLayer.DataHasChanged();

            _loggingService.Info($"FlightMapViewModel: Active flight plan displayed {flightPlan.DepartureIcao} -> {flightPlan.ArrivalIcao}");
        }

        private IFeature CreateActiveFlightPlanRoute(Airport departure, Airport arrival)
        {
            var depPoint = SphericalMercator.FromLonLat(departure.Longitude, departure.Latitude);
            var arrPoint = SphericalMercator.FromLonLat(arrival.Longitude, arrival.Latitude);

            var line = new LineString(new[]
            {
                new Coordinate(depPoint.x, depPoint.y),
                new Coordinate(arrPoint.x, arrPoint.y)
            });

            var feature = new GeometryFeature(line);

            feature.Styles.Add(new VectorStyle
            {
                Line = new Pen(Color.FromString("#E91E63"), 3)
                {
                    PenStyle = PenStyle.Dash,
                    DashArray = new[] { 4f, 4f }
                }
            });

            return feature;
        }

        private IFeature CreateActiveFlightPlanAirportMarker(Airport airport, bool isDeparture)
        {
            var point = SphericalMercator.FromLonLat(airport.Longitude, airport.Latitude).ToMPoint();
            var feature = new PointFeature(point);

            // Departure: Green, Arrival: Magenta
            var color = isDeparture ? Color.FromString("#4CAF50") : Color.FromString("#E91E63");
            var label = isDeparture ? "DEP" : "ARR";

            feature.Styles.Add(new SymbolStyle
            {
                SymbolScale = 0.8,
                Fill = new Brush(color),
                Outline = new Pen(Color.White, 3),
                SymbolType = SymbolType.Ellipse
            });

            feature.Styles.Add(new LabelStyle
            {
                Text = $"{label}: {airport.Icao}",
                ForeColor = Color.White,
                BackColor = new Brush(color),
                Offset = new Offset(0, -25),
                Font = new Font { Size = 11, Bold = true },
                Halo = new Pen(Color.Black, 1)
            });

            return feature;
        }

        private List<IFeature> CreateRouteArrows(Airport departure, Airport arrival)
        {
            var arrows = new List<IFeature>();

            var depPoint = SphericalMercator.FromLonLat(departure.Longitude, departure.Latitude);
            var arrPoint = SphericalMercator.FromLonLat(arrival.Longitude, arrival.Latitude);

            // Calculate bearing
            double dx = arrPoint.x - depPoint.x;
            double dy = arrPoint.y - depPoint.y;
            double bearing = Math.Atan2(dx, dy) * 180 / Math.PI;

            // Calculate distance and place arrows at 25%, 50%, 75%
            double totalDist = Math.Sqrt(dx * dx + dy * dy);
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

                arrows.Add(arrowFeature);
            }

            return arrows;
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

            _loggingService.Info($"FlightMapViewModel: Switched to {layerType} layer");
        }

        public void Cleanup()
        {
            _simConnectService.FlightDataReceived -= OnFlightDataReceived;
            _simConnectService.ConnectionChanged -= OnConnectionChanged;
            _activeFlightPlanService.FlightPlanChanged -= OnFlightPlanChanged;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
