using System.Collections.Generic;

namespace Ace.App.Models
{
    public class Airspace
    {
        public string Name { get; set; } = string.Empty;
        public AirspaceClass Class { get; set; } = AirspaceClass.Other;
        public string LowerAltitude { get; set; } = "GND";
        public string UpperAltitude { get; set; } = "UNL";
        public List<AirspaceCoordinate> Coordinates { get; } = new();
        public bool IsCircle { get; set; }
        public double CircleRadiusNM { get; set; }
        public AirspaceCoordinate? CircleCenter { get; set; }
    }

    public class AirspaceCoordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public AirspaceCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
