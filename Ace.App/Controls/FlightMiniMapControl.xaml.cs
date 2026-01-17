using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Projections;
using Mapsui.Styles;
using NetTopologySuite.Geometries;
using Ace.App.Helpers;
using Ace.App.Infrastructure;
using Ace.App.Interfaces;

namespace Ace.App.Controls
{
    public partial class FlightMiniMapControl : UserControl
    {
        private readonly IAirportDatabase _airportDatabase;
        private MemoryLayer? _routeLayer;
        private MemoryLayer? _aircraftLayer;

        public static readonly DependencyProperty DepartureIcaoProperty =
            DependencyProperty.Register(nameof(DepartureIcao), typeof(string), typeof(FlightMiniMapControl),
                new PropertyMetadata(string.Empty, OnRouteChanged));

        public static readonly DependencyProperty ArrivalIcaoProperty =
            DependencyProperty.Register(nameof(ArrivalIcao), typeof(string), typeof(FlightMiniMapControl),
                new PropertyMetadata(string.Empty, OnRouteChanged));

        public static readonly DependencyProperty AircraftLatitudeProperty =
            DependencyProperty.Register(nameof(AircraftLatitude), typeof(double), typeof(FlightMiniMapControl),
                new PropertyMetadata(0.0, OnAircraftPositionChanged));

        public static readonly DependencyProperty AircraftLongitudeProperty =
            DependencyProperty.Register(nameof(AircraftLongitude), typeof(double), typeof(FlightMiniMapControl),
                new PropertyMetadata(0.0, OnAircraftPositionChanged));

        public static readonly DependencyProperty AircraftHeadingProperty =
            DependencyProperty.Register(nameof(AircraftHeading), typeof(double), typeof(FlightMiniMapControl),
                new PropertyMetadata(0.0, OnAircraftPositionChanged));

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(FlightMiniMapControl),
                new PropertyMetadata(false, OnIsActiveChanged));

        public string DepartureIcao
        {
            get => (string)GetValue(DepartureIcaoProperty);
            set => SetValue(DepartureIcaoProperty, value);
        }

        public string ArrivalIcao
        {
            get => (string)GetValue(ArrivalIcaoProperty);
            set => SetValue(ArrivalIcaoProperty, value);
        }

        public double AircraftLatitude
        {
            get => (double)GetValue(AircraftLatitudeProperty);
            set => SetValue(AircraftLatitudeProperty, value);
        }

        public double AircraftLongitude
        {
            get => (double)GetValue(AircraftLongitudeProperty);
            set => SetValue(AircraftLongitudeProperty, value);
        }

        public double AircraftHeading
        {
            get => (double)GetValue(AircraftHeadingProperty);
            set => SetValue(AircraftHeadingProperty, value);
        }

        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        public event EventHandler? MapClicked;

        public FlightMiniMapControl()
        {
            _airportDatabase = ServiceLocator.GetService<IAirportDatabase>();
            InitializeComponent();
            InitializeMap();
            MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        private void InitializeMap()
        {
            MapControl.Map.BackColor = Mapsui.Styles.Color.FromString("#1A1A2E");
            MapControl.Map.Widgets.Clear();

            MapControl.Map.Layers.Add(MapTileHelper.CreateTileLayer());

            _routeLayer = new MemoryLayer { Name = "Route", Style = null };
            _aircraftLayer = new MemoryLayer { Name = "Aircraft", Style = null };

            MapControl.Map.Layers.Add(_routeLayer);
            MapControl.Map.Layers.Add(_aircraftLayer);

            var extent = new MRect(-20037508, -10018754, 20037508, 10018754);
            MapControl.Map.Navigator.ZoomToBox(extent, MBoxFit.Fit);

            ScaleControl.AttachToMap(MapControl.Map);
        }

        private static void OnRouteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlightMiniMapControl control)
                control.UpdateRoute();
        }

        private static void OnAircraftPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlightMiniMapControl control)
                control.UpdateAircraftPosition();
        }

        private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlightMiniMapControl control)
            {
                control.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
                if ((bool)e.NewValue)
                    control.UpdateRoute();
            }
        }

        private void UpdateRoute()
        {
            if (_routeLayer == null || string.IsNullOrEmpty(DepartureIcao) || string.IsNullOrEmpty(ArrivalIcao))
            {
                if (_routeLayer != null)
                {
                    _routeLayer.Features = new List<IFeature>();
                    _routeLayer.DataHasChanged();
                }
                return;
            }

            var departure = _airportDatabase.GetAirport(DepartureIcao);
            var arrival = _airportDatabase.GetAirport(ArrivalIcao);

            if (departure == null || arrival == null)
            {
                _routeLayer.Features = new List<IFeature>();
                _routeLayer.DataHasChanged();
                return;
            }

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
                Line = new Pen(Color.FromString("#FF9800"), 2) { PenStyle = PenStyle.Dash }
            });
            features.Add(routeFeature);

            var depFeature = new PointFeature(depPoint);
            depFeature.Styles.Add(new SymbolStyle
            {
                Fill = new Brush(Color.FromString("#4CAF50")),
                Outline = new Pen(Color.White, 2),
                SymbolScale = 0.4,
                SymbolType = SymbolType.Ellipse
            });
            features.Add(depFeature);

            var arrFeature = new PointFeature(arrPoint);
            arrFeature.Styles.Add(new SymbolStyle
            {
                Fill = new Brush(Color.FromString("#2196F3")),
                Outline = new Pen(Color.White, 2),
                SymbolScale = 0.4,
                SymbolType = SymbolType.Ellipse
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

            double paddingX = (maxX - minX) * 0.2;
            double paddingY = (maxY - minY) * 0.2;

            if (paddingX < 50000) paddingX = 50000;
            if (paddingY < 50000) paddingY = 50000;

            var extent = new MRect(minX - paddingX, minY - paddingY, maxX + paddingX, maxY + paddingY);
            MapControl.Map.Navigator.ZoomToBox(extent, MBoxFit.Fit);
        }

        private void UpdateAircraftPosition()
        {
            if (_aircraftLayer == null) return;

            if (AircraftLatitude == 0 && AircraftLongitude == 0)
            {
                _aircraftLayer.Features = new List<IFeature>();
                _aircraftLayer.DataHasChanged();
                return;
            }

            var coords = SphericalMercator.FromLonLat(AircraftLongitude, AircraftLatitude);
            var point = new MPoint(coords.x, coords.y);
            var feature = new PointFeature(point);
            foreach (var style in AircraftMapSymbolHelper.CreateAircraftStyleCompact(AircraftHeading))
            {
                feature.Styles.Add(style);
            }

            _aircraftLayer.Features = new List<IFeature> { feature };
            _aircraftLayer.DataHasChanged();
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MapClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
