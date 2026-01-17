using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IFlightEarningsCalculator
    {
        FlightEarningsResult CalculateEarnings(FlightEarningsRequest request);
    }

    public class FlightEarningsRequest
    {
        public required string AircraftRegistration { get; init; }
        public required string DepartureIcao { get; init; }
        public required string ArrivalIcao { get; init; }
        public double DistanceNM { get; init; }
        public int Passengers { get; init; }
        public double FlightHours { get; init; }
        public bool IsManualCompletion { get; init; }
    }

    public class FlightEarningsResult
    {
        public decimal TotalEarnings { get; set; }
        public decimal BaseRevenue { get; set; }
        public decimal TotalOperatingCost { get; set; }
        public decimal PlayerBonusAmount { get; set; }
        public decimal NetworkBonusAmount { get; set; }
        public decimal ServiceBonusAmount { get; set; }
        public decimal PlayerBonusPercent { get; set; }
        public decimal NetworkBonusPercent { get; set; }
        public decimal ServiceBonusPercent { get; set; }
        public bool IsNetworkFlight { get; set; }
        public bool HasDepartureFBOServices { get; set; }
        public bool AircraftFound { get; set; }
        public FlightPriceBreakdown? PriceBreakdown { get; set; }
    }
}
