using System;
using System.Collections.Generic;
using Mapsui.Projections;
using NetTopologySuite.Geometries;

namespace Ace.App.Helpers
{
    public static class GreatCircleHelper
    {
        private const int DefaultSegments = 50;

        public static LineString CreateGreatCircleLine(double lon1, double lat1, double lon2, double lat2, int segments = DefaultSegments)
        {
            var coordinates = InterpolateGreatCircle(lon1, lat1, lon2, lat2, segments);
            var projected = new Coordinate[coordinates.Count];

            for (int i = 0; i < coordinates.Count; i++)
            {
                var (lon, lat) = coordinates[i];
                var point = SphericalMercator.FromLonLat(lon, lat);
                projected[i] = new Coordinate(point.x, point.y);
            }

            return new LineString(projected);
        }

        public static (double lon, double lat) InterpolateGreatCirclePoint(double lon1, double lat1, double lon2, double lat2, double fraction)
        {
            double phi1 = lat1 * Math.PI / 180;
            double phi2 = lat2 * Math.PI / 180;
            double lambda1 = lon1 * Math.PI / 180;
            double lambda2 = lon2 * Math.PI / 180;

            double d = 2 * Math.Asin(Math.Sqrt(
                Math.Pow(Math.Sin((phi2 - phi1) / 2), 2) +
                Math.Cos(phi1) * Math.Cos(phi2) * Math.Pow(Math.Sin((lambda2 - lambda1) / 2), 2)));

            if (d < 1e-10)
                return (lon1, lat1);

            double a = Math.Sin((1 - fraction) * d) / Math.Sin(d);
            double b = Math.Sin(fraction * d) / Math.Sin(d);

            double x = a * Math.Cos(phi1) * Math.Cos(lambda1) + b * Math.Cos(phi2) * Math.Cos(lambda2);
            double y = a * Math.Cos(phi1) * Math.Sin(lambda1) + b * Math.Cos(phi2) * Math.Sin(lambda2);
            double z = a * Math.Sin(phi1) + b * Math.Sin(phi2);

            double lat = Math.Atan2(z, Math.Sqrt(x * x + y * y)) * 180 / Math.PI;
            double lon = Math.Atan2(y, x) * 180 / Math.PI;

            return (lon, lat);
        }

        private static List<(double lon, double lat)> InterpolateGreatCircle(double lon1, double lat1, double lon2, double lat2, int segments)
        {
            var points = new List<(double lon, double lat)>(segments + 1);

            for (int i = 0; i <= segments; i++)
            {
                double fraction = (double)i / segments;
                points.Add(InterpolateGreatCirclePoint(lon1, lat1, lon2, lat2, fraction));
            }

            return points;
        }
    }
}
