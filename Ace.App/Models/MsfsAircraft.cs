using System;

namespace Ace.App.Models
{
    public class MsfsAircraft
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int CrewCount { get; set; }
        public int PassengerCapacity { get; set; }
        public decimal NewPrice { get; set; }
        public double CruiseSpeedKts { get; set; }
        public double MaxRangeNM { get; set; }
        public double FuelCapacityGal { get; set; }
        public double FuelBurnGalPerHour { get; set; }
        public decimal HourlyOperatingCost { get; set; }
        public DateTime FirstDetected { get; set; }
        public DateTime LastDetected { get; set; }
    }
}
