using System;

namespace Ace.App.Models
{
    public class AircraftCatalogEntry
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int CrewCount { get; set; }
        public int PassengerCapacity { get; set; }
        public double MaxCargoKg { get; set; }
        public decimal MarketPrice { get; set; }
        public double CruiseSpeedKts { get; set; }
        public double MaxRangeNM { get; set; }
        public double FuelCapacityGal { get; set; }
        public double FuelBurnGalPerHour { get; set; }
        public decimal HourlyOperatingCost { get; set; }
        public int ServiceCeilingFt { get; set; }
        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }
        public string? CustomImagePath { get; set; }
        public bool IsOldtimer { get; set; }
        public bool IsFavorite { get; set; }
    }
}
