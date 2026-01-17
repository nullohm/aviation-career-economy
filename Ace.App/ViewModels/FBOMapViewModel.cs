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
using Ace.App.Data;
using Ace.App.Helpers;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;

namespace Ace.App.ViewModels
{
    public class FBOMapViewModel : INotifyPropertyChanged
    {
        private readonly IAirportDatabase _airportDatabase;
        private readonly ILoggingService _loggingService;
        private readonly ISettingsService _settingsService;

        private Map? _map;
        private MemoryLayer? _airportLayer;
        private MemoryLayer? _fboLayer;
        private List<Airport> _allAirports = new();
        private HashSet<string> _fboIcaos = new();

        public Map? Map
        {
            get => _map;
            set { _map = value; OnPropertyChanged(); }
        }

        public FBOMapViewModel(
            IAirportDatabase airportDatabase,
            ILoggingService loggingService,
            ISettingsService settingsService)
        {
            _airportDatabase = airportDatabase ?? throw new ArgumentNullException(nameof(airportDatabase));
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

            InitializeMap();
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

            _fboLayer = new MemoryLayer
            {
                Name = "FBOs",
                Style = null
            };
            _map.Layers.Add(_fboLayer);

            _map.Navigator.ViewportChanged += (s, e) => UpdateAirportMarkersBasedOnZoom();

            _loggingService.Info("FBOMapViewModel: Map initialized");
        }

        public void LoadFBOData()
        {
            try
            {
                using var db = new AceDbContext();
                var fbos = db.FBOs.ToList();
                _fboIcaos = fbos.Select(f => f.ICAO).ToHashSet();

                _allAirports = _airportDatabase.GetAllAirports().ToList();
                UpdateAirportMarkers();

                if (fbos.Count == 0)
                {
                    _fboLayer!.Features = new List<IFeature>();
                    _fboLayer.DataHasChanged();
                    _loggingService.Debug("FBOMapViewModel: No FBOs to display");
                    return;
                }

                var aircraftByFbo = db.Aircraft
                    .Where(a => a.AssignedFBOId != null)
                    .GroupBy(a => a.AssignedFBOId!.Value)
                    .ToDictionary(g => g.Key, g => g.Count());

                var fboFeatures = new List<IFeature>();

                foreach (var fbo in fbos)
                {
                    var airport = _airportDatabase.GetAirport(fbo.ICAO);
                    if (airport != null)
                    {
                        var aircraftCount = aircraftByFbo.GetValueOrDefault(fbo.Id, 0);
                        fboFeatures.Add(CreateFBOFeature(airport, fbo, aircraftCount));
                    }
                }

                _fboLayer!.Features = fboFeatures;
                _fboLayer.DataHasChanged();

                if (fboFeatures.Count > 0)
                {
                    ZoomToFBOs(fbos);
                }

                _loggingService.Info($"FBOMapViewModel: Loaded {_allAirports.Count} airports, {fbos.Count} FBOs on map");
            }
            catch (Exception ex)
            {
                _loggingService.Error("FBOMapViewModel: Error loading FBO data", ex);
            }
        }

        private void UpdateAirportMarkers()
        {
            UpdateAirportMarkersBasedOnZoom();
        }

        private void UpdateAirportMarkersBasedOnZoom()
        {
            if (_airportLayer == null || _map == null) return;

            var resolution = _map.Navigator.Viewport.Resolution;
            var minRunwayLength = MapZoomFilterHelper.GetMinRunwayLengthForZoom(resolution);

            _loggingService.Debug($"FBOMapViewModel: UpdateAirportMarkersBasedOnZoom - Resolution: {resolution:F0}, MinRunwayLength: {minRunwayLength}");

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

        private IFeature CreateFBOFeature(Airport airport, FBO fbo, int aircraftCount)
        {
            var point = SphericalMercator.FromLonLat(airport.Longitude, airport.Latitude).ToMPoint();
            var feature = new PointFeature(point);

            feature["name"] = fbo.AirportName;
            feature["icao"] = airport.Icao;
            feature["type"] = fbo.Type.ToString();

            // Color based on FBO type
            var color = fbo.Type switch
            {
                FBOType.International => Color.FromString("#E91E63"), // Pink/Magenta
                FBOType.Regional => Color.FromString("#2196F3"),      // Blue
                FBOType.Local => Color.FromString("#4CAF50"),         // Green
                _ => Color.FromString("#FF9800")                      // Orange
            };

            var symbolType = fbo.Type switch
            {
                FBOType.Local => SymbolType.Triangle,
                FBOType.Regional => SymbolType.Rectangle,
                FBOType.International => SymbolType.Ellipse,
                _ => SymbolType.Rectangle
            };

            feature.Styles.Add(new SymbolStyle
            {
                SymbolScale = 0.7,
                Fill = new Brush(color),
                Outline = new Pen(Color.Black, 2),
                SymbolType = symbolType
            });

            // Build services text
            var services = new List<string>();
            if (fbo.HasRefuelingService) services.Add("Fuel");
            if (fbo.HasHangarService) services.Add("Hangar");
            if (fbo.HasCateringService) services.Add("Catering");
            if (fbo.HasGroundHandling) services.Add("Ground");
            if (fbo.HasDeIcingService) services.Add("De-Ice");

            var servicesText = services.Count > 0 ? string.Join(", ", services) : "No services";

            var aircraftText = aircraftCount > 0 ? $"\n✈ {aircraftCount}" : "";
            feature.Styles.Add(new LabelStyle
            {
                Text = $"{airport.Icao}\n{fbo.Type}{aircraftText}",
                ForeColor = Color.White,
                BackColor = new Brush(Color.FromArgb(200, 0, 0, 0)),
                Offset = new Offset(0, -30),
                Font = new Font { Size = 10, Bold = true },
                Halo = new Pen(Color.Black, 1)
            });

            return feature;
        }

        private void ZoomToFBOs(List<FBO> fbos)
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

            _loggingService.Info($"FBOMapViewModel: Switched to {layerType} layer");
        }

        public void Cleanup()
        {
            // No event subscriptions to clean up currently
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
