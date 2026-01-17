using System;
using System.Windows;
using System.Windows.Controls;
using Mapsui;
using Mapsui.Projections;

namespace Ace.App.Controls
{
    public partial class MapScaleControl : UserControl
    {
        private Map? _map;
        private const double TargetBarWidthPixels = 60;

        public MapScaleControl()
        {
            InitializeComponent();
        }

        public void AttachToMap(Map map)
        {
            if (_map != null)
            {
                _map.Navigator.ViewportChanged -= OnViewportChanged;
            }

            _map = map;

            if (_map != null)
            {
                _map.Navigator.ViewportChanged += OnViewportChanged;
                UpdateScale();
            }
        }

        private void OnViewportChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Dispatcher.Invoke(UpdateScale);
        }

        private void UpdateScale()
        {
            if (_map?.Navigator?.Viewport == null) return;

            try
            {
                var viewport = _map.Navigator.Viewport;
                var resolution = viewport.Resolution;

                var centerX = viewport.CenterX;
                var centerY = viewport.CenterY;

                var leftPoint = SphericalMercator.ToLonLat(centerX - (TargetBarWidthPixels / 2) * resolution, centerY);
                var rightPoint = SphericalMercator.ToLonLat(centerX + (TargetBarWidthPixels / 2) * resolution, centerY);

                double distanceNM = CalculateDistanceNM(leftPoint.lat, leftPoint.lon, rightPoint.lat, rightPoint.lon);

                var (displayDistance, unit) = GetNiceScaleValue(distanceNM);

                double actualBarWidth = (displayDistance / distanceNM) * TargetBarWidthPixels;
                actualBarWidth = Math.Max(30, Math.Min(100, actualBarWidth));

                ScaleBar.Width = actualBarWidth;
                TxtScale.Text = $"{displayDistance:0.##} {unit}";
            }
            catch
            {
                TxtScale.Text = "---";
            }
        }

        private static (double value, string unit) GetNiceScaleValue(double distanceNM)
        {
            double[] niceValues = { 1, 2, 5, 10, 20, 50, 100, 200, 500, 1000, 2000, 5000 };

            foreach (var nice in niceValues)
            {
                if (distanceNM <= nice * 1.5)
                {
                    return (nice, "NM");
                }
            }

            return (Math.Round(distanceNM / 1000) * 1000, "NM");
        }

        private static double CalculateDistanceNM(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 3440.065;
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            return 2 * R * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }
    }
}
