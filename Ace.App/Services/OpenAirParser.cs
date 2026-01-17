using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Ace.App.Models;

namespace Ace.App.Services
{
    public static class OpenAirParser
    {
        private static readonly Regex CoordinateRegex = new(
            @"(\d{1,3}):(\d{1,2}):?([\d.]*)\s*([NS])\s+(\d{1,3}):(\d{1,2}):?([\d.]*)\s*([EW])",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex DecimalCoordinateRegex = new(
            @"([\d.]+)\s*([NS])\s+([\d.]+)\s*([EW])",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static List<Airspace> ParseFile(string filePath)
        {
            var airspaces = new List<Airspace>();

            if (!File.Exists(filePath))
                return airspaces;

            var lines = File.ReadAllLines(filePath);
            Airspace? current = null;
            AirspaceCoordinate? variableCenter = null;
            bool arcDirectionClockwise = true;

            foreach (var rawLine in lines)
            {
                var line = rawLine.Trim();

                if (string.IsNullOrEmpty(line) || line.StartsWith("*"))
                    continue;

                var commentIndex = line.IndexOf('*');
                if (commentIndex > 0)
                    line = line.Substring(0, commentIndex).Trim();

                if (line.StartsWith("AC ", StringComparison.OrdinalIgnoreCase))
                {
                    if (current != null && (current.Coordinates.Count > 0 || current.IsCircle))
                        airspaces.Add(current);

                    current = new Airspace { Class = ParseAirspaceClass(line.Substring(3).Trim()) };
                    variableCenter = null;
                    arcDirectionClockwise = true;
                }
                else if (current != null)
                {
                    if (line.StartsWith("AN ", StringComparison.OrdinalIgnoreCase))
                    {
                        current.Name = line.Substring(3).Trim();
                    }
                    else if (line.StartsWith("AL ", StringComparison.OrdinalIgnoreCase))
                    {
                        current.LowerAltitude = line.Substring(3).Trim();
                    }
                    else if (line.StartsWith("AH ", StringComparison.OrdinalIgnoreCase))
                    {
                        current.UpperAltitude = line.Substring(3).Trim();
                    }
                    else if (line.StartsWith("V X=", StringComparison.OrdinalIgnoreCase))
                    {
                        variableCenter = ParseCoordinate(line.Substring(4).Trim());
                    }
                    else if (line.StartsWith("V D=", StringComparison.OrdinalIgnoreCase))
                    {
                        var direction = line.Substring(4).Trim();
                        arcDirectionClockwise = direction == "+" || direction.Equals("CW", StringComparison.OrdinalIgnoreCase);
                    }
                    else if (line.StartsWith("DP ", StringComparison.OrdinalIgnoreCase))
                    {
                        var coord = ParseCoordinate(line.Substring(3).Trim());
                        if (coord != null)
                            current.Coordinates.Add(coord);
                    }
                    else if (line.StartsWith("DC ", StringComparison.OrdinalIgnoreCase))
                    {
                        if (variableCenter != null && double.TryParse(line.Substring(3).Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var radius))
                        {
                            current.IsCircle = true;
                            current.CircleRadiusNM = radius;
                            current.CircleCenter = variableCenter;
                        }
                    }
                    else if (line.StartsWith("DA ", StringComparison.OrdinalIgnoreCase))
                    {
                        if (variableCenter != null)
                        {
                            var arcPoints = ParseArc(line.Substring(3).Trim(), variableCenter, arcDirectionClockwise);
                            current.Coordinates.AddRange(arcPoints);
                        }
                    }
                    else if (line.StartsWith("DB ", StringComparison.OrdinalIgnoreCase))
                    {
                        if (variableCenter != null)
                        {
                            var arcPoints = ParseArcByPoints(line.Substring(3).Trim(), variableCenter, arcDirectionClockwise);
                            current.Coordinates.AddRange(arcPoints);
                        }
                    }
                }
            }

            if (current != null && (current.Coordinates.Count > 0 || current.IsCircle))
                airspaces.Add(current);

            return airspaces;
        }

        public static List<Airspace> ParseDirectory(string directoryPath)
        {
            var allAirspaces = new List<Airspace>();

            if (!Directory.Exists(directoryPath))
                return allAirspaces;

            foreach (var file in Directory.GetFiles(directoryPath, "*.txt"))
            {
                allAirspaces.AddRange(ParseFile(file));
            }

            return allAirspaces;
        }

        private static AirspaceClass ParseAirspaceClass(string classStr)
        {
            return classStr.ToUpperInvariant() switch
            {
                "A" => AirspaceClass.A,
                "B" => AirspaceClass.B,
                "C" => AirspaceClass.C,
                "D" => AirspaceClass.D,
                "E" => AirspaceClass.E,
                "F" => AirspaceClass.F,
                "G" => AirspaceClass.G,
                "CTR" => AirspaceClass.CTR,
                "TMA" => AirspaceClass.TMA,
                "ATZ" => AirspaceClass.ATZ,
                "RMZ" => AirspaceClass.RMZ,
                "TMZ" => AirspaceClass.TMZ,
                "P" => AirspaceClass.Prohibited,
                "R" => AirspaceClass.Restricted,
                "Q" or "D" when classStr.Length == 1 => AirspaceClass.Danger,
                "GP" or "GLIDING" => AirspaceClass.Glider,
                "W" or "WAVE" => AirspaceClass.Wave,
                _ => AirspaceClass.Other
            };
        }

        private static AirspaceCoordinate? ParseCoordinate(string coordStr)
        {
            var match = CoordinateRegex.Match(coordStr);
            if (match.Success)
            {
                var latDeg = int.Parse(match.Groups[1].Value);
                var latMin = int.Parse(match.Groups[2].Value);
                var latSec = string.IsNullOrEmpty(match.Groups[3].Value) ? 0.0 : double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
                var latDir = match.Groups[4].Value.ToUpperInvariant();

                var lonDeg = int.Parse(match.Groups[5].Value);
                var lonMin = int.Parse(match.Groups[6].Value);
                var lonSec = string.IsNullOrEmpty(match.Groups[7].Value) ? 0.0 : double.Parse(match.Groups[7].Value, CultureInfo.InvariantCulture);
                var lonDir = match.Groups[8].Value.ToUpperInvariant();

                var lat = latDeg + latMin / 60.0 + latSec / 3600.0;
                if (latDir == "S") lat = -lat;

                var lon = lonDeg + lonMin / 60.0 + lonSec / 3600.0;
                if (lonDir == "W") lon = -lon;

                return new AirspaceCoordinate(lat, lon);
            }

            var decMatch = DecimalCoordinateRegex.Match(coordStr);
            if (decMatch.Success)
            {
                var lat = double.Parse(decMatch.Groups[1].Value, CultureInfo.InvariantCulture);
                if (decMatch.Groups[2].Value.ToUpperInvariant() == "S") lat = -lat;

                var lon = double.Parse(decMatch.Groups[3].Value, CultureInfo.InvariantCulture);
                if (decMatch.Groups[4].Value.ToUpperInvariant() == "W") lon = -lon;

                return new AirspaceCoordinate(lat, lon);
            }

            return null;
        }

        private static List<AirspaceCoordinate> ParseArc(string arcStr, AirspaceCoordinate center, bool clockwise)
        {
            var points = new List<AirspaceCoordinate>();
            var parts = arcStr.Split(',');

            if (parts.Length >= 3 &&
                double.TryParse(parts[0].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var radiusNM) &&
                double.TryParse(parts[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var startAngle) &&
                double.TryParse(parts[2].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var endAngle))
            {
                points.AddRange(GenerateArcPoints(center, radiusNM, startAngle, endAngle, clockwise));
            }

            return points;
        }

        private static List<AirspaceCoordinate> ParseArcByPoints(string arcStr, AirspaceCoordinate center, bool clockwise)
        {
            var points = new List<AirspaceCoordinate>();
            var parts = arcStr.Split(',');

            if (parts.Length >= 2)
            {
                var startCoord = ParseCoordinate(parts[0].Trim());
                var endCoord = ParseCoordinate(parts[1].Trim());

                if (startCoord != null && endCoord != null)
                {
                    var radiusNM = CalculateDistanceNM(center.Latitude, center.Longitude, startCoord.Latitude, startCoord.Longitude);
                    var startAngle = CalculateBearing(center.Latitude, center.Longitude, startCoord.Latitude, startCoord.Longitude);
                    var endAngle = CalculateBearing(center.Latitude, center.Longitude, endCoord.Latitude, endCoord.Longitude);

                    points.AddRange(GenerateArcPoints(center, radiusNM, startAngle, endAngle, clockwise));
                }
            }

            return points;
        }

        private static List<AirspaceCoordinate> GenerateArcPoints(AirspaceCoordinate center, double radiusNM, double startAngle, double endAngle, bool clockwise)
        {
            var points = new List<AirspaceCoordinate>();
            const double EarthRadiusNM = 3440.065;
            const int segments = 36;

            if (!clockwise)
            {
                (startAngle, endAngle) = (endAngle, startAngle);
            }

            var angleDiff = endAngle - startAngle;
            if (clockwise && angleDiff < 0) angleDiff += 360;
            if (!clockwise && angleDiff > 0) angleDiff -= 360;

            var step = angleDiff / segments;

            for (int i = 0; i <= segments; i++)
            {
                var angle = startAngle + i * step;
                var bearingRad = angle * Math.PI / 180.0;
                var angularDist = radiusNM / EarthRadiusNM;

                var lat1 = center.Latitude * Math.PI / 180.0;
                var lon1 = center.Longitude * Math.PI / 180.0;

                var lat2 = Math.Asin(
                    Math.Sin(lat1) * Math.Cos(angularDist) +
                    Math.Cos(lat1) * Math.Sin(angularDist) * Math.Cos(bearingRad));

                var lon2 = lon1 + Math.Atan2(
                    Math.Sin(bearingRad) * Math.Sin(angularDist) * Math.Cos(lat1),
                    Math.Cos(angularDist) - Math.Sin(lat1) * Math.Sin(lat2));

                points.Add(new AirspaceCoordinate(lat2 * 180.0 / Math.PI, lon2 * 180.0 / Math.PI));
            }

            if (!clockwise)
                points.Reverse();

            return points;
        }

        private static double CalculateDistanceNM(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 3440.065;
            var dLat = (lat2 - lat1) * Math.PI / 180.0;
            var dLon = (lon2 - lon1) * Math.PI / 180.0;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            return 2 * R * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }

        private static double CalculateBearing(double lat1, double lon1, double lat2, double lon2)
        {
            var lat1Rad = lat1 * Math.PI / 180.0;
            var lat2Rad = lat2 * Math.PI / 180.0;
            var dLon = (lon2 - lon1) * Math.PI / 180.0;

            var y = Math.Sin(dLon) * Math.Cos(lat2Rad);
            var x = Math.Cos(lat1Rad) * Math.Sin(lat2Rad) -
                    Math.Sin(lat1Rad) * Math.Cos(lat2Rad) * Math.Cos(dLon);

            var bearing = Math.Atan2(y, x) * 180.0 / Math.PI;
            return (bearing + 360) % 360;
        }
    }
}
