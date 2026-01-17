using System.Collections.Generic;

namespace Ace.App.Models
{
    public class AirportDetail
    {
        public string Icao { get; set; } = string.Empty;
        public string Iata { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Elevation { get; set; }
        public bool HasAvgas { get; set; }
        public bool HasJetfuel { get; set; }
        public bool IsClosed { get; set; }
        public bool IsMilitary { get; set; }
        public bool IsAddon { get; set; }
        public int TowerFrequency { get; set; }
        public int AtisFrequency { get; set; }
        public int UnicomFrequency { get; set; }

        public List<RunwayInfo> Runways { get; set; } = new();
        public List<ComFrequency> Frequencies { get; set; } = new();
        public List<IlsInfo> IlsSystems { get; set; } = new();
    }

    public class RunwayInfo
    {
        public string Name1 { get; set; } = string.Empty;
        public string Name2 { get; set; } = string.Empty;
        public int LengthFt { get; set; }
        public int WidthFt { get; set; }
        public string Surface { get; set; } = string.Empty;
        public double Heading { get; set; }
        public string Ils1Ident { get; set; } = string.Empty;
        public string Ils2Ident { get; set; } = string.Empty;
        public string LightingType { get; set; } = string.Empty;
        public bool HasCenterLights { get; set; }

        public string DisplayName => $"{Name1}/{Name2}";
        public string DimensionsDisplay => $"{LengthFt:N0} x {WidthFt} ft";
        public string SurfaceDisplay => Surface switch
        {
            "C" => "Concrete",
            "A" => "Asphalt",
            "G" => "Grass",
            "D" => "Dirt",
            "GR" => "Gravel",
            "W" => "Water",
            "T" => "Turf",
            "CE" => "Cement",
            "B" => "Bituminous",
            "BR" => "Brick",
            "M" => "Macadam",
            "PL" => "Planks",
            "S" => "Sand",
            "SH" => "Shale",
            "SN" => "Snow",
            "TR" => "Transparent",
            _ => Surface
        };
    }

    public class ComFrequency
    {
        public string Type { get; set; } = string.Empty;
        public int Frequency { get; set; }
        public string Name { get; set; } = string.Empty;

        public string FrequencyDisplay => $"{Frequency / 1000000.0:F3} MHz";
        public string TypeDisplay => Type switch
        {
            "A" => "Approach",
            "ATIS" => "ATIS",
            "C" => "Clearance",
            "D" => "Departure",
            "G" => "Ground",
            "T" => "Tower",
            "UC" => "Unicom",
            "AWOS" => "AWOS",
            "ASOS" => "ASOS",
            "CTF" => "CTF",
            "MF" => "MF",
            "FSS" => "FSS",
            "RCO" => "RCO",
            "R" => "Radar",
            _ => Type
        };
    }

    public class IlsInfo
    {
        public string Ident { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Frequency { get; set; }
        public string RunwayName { get; set; } = string.Empty;
        public double GlideSlopePitch { get; set; }
        public string Category { get; set; } = string.Empty;

        public string FrequencyDisplay => $"{Frequency / 1000000.0:F2} MHz";
        public string CategoryDisplay => Category switch
        {
            "1" => "CAT I",
            "2" => "CAT II",
            "3" => "CAT III",
            _ => $"CAT {Category}"
        };
    }
}
