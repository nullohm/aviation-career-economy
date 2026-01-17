using System;

namespace Ace.App.Models
{
    public enum FlightLegType
    {
        Complete,
        IntermediateLanding,
        FuelStop
    }

    public class FlightRecord
    {
        public int Id { get; set; }
        public string Aircraft { get; set; } = string.Empty;
        public string AircraftTitle { get; set; } = string.Empty;
        public string Departure { get; set; } = string.Empty;
        public string Arrival { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public double LandingRate { get; set; }
        public double DistanceNM { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Earnings { get; set; }
        public FlightLegType LegType { get; set; } = FlightLegType.Complete;
        public int? ParentFlightId { get; set; }
        public string PlannedDestination { get; set; } = string.Empty;
    }
}
