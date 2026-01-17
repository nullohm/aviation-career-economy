using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IPricingService
    {
        FlightPriceBreakdown CalculateFlightPrice(Aircraft aircraft, double distanceNM, int passengers, double flightHours);

        decimal CalculateCargoFlightPrice(Aircraft aircraft, double distanceNM, double cargoKg, double flightHours);

        DailyPassiveEarnings CalculateDailyPassiveEarnings(Aircraft aircraft, double dailyFlightHours);

        DailyPassiveEarnings CalculateDailyPassiveEarnings(Aircraft aircraft, double dailyFlightHours, Pilot? pilot);

        decimal CalculateProfitPerHour(AircraftCatalogEntry catalogEntry);

        DailyOperatingCosts CalculateDailyOperatingCosts(Aircraft aircraft, double flightHours);

        decimal GetRateForAircraftSize(int maxPassengers);

        decimal GetCargoRateForAircraftSize(int maxPassengers);

        decimal GetROIPercentForSize(int maxPassengers);

        decimal CalculateROIPerFlightHour(decimal purchasePrice, int maxPassengers, bool isOldtimer);

        bool IsNetworkFlight(string departureIcao, string arrivalIcao);

        decimal GetNetworkBonusPercent();
    }

    public class FlightPriceBreakdown
    {
        public decimal BaseOperatingCost { get; set; }
        public decimal FuelCost { get; set; }
        public decimal MaintenanceCost { get; set; }
        public decimal DepreciationCost { get; set; }
        public decimal InsuranceCost { get; set; }
        public decimal CrewCost { get; set; }
        public decimal LandingFees { get; set; }
        public decimal GroundHandlingCost { get; set; }
        public decimal CateringCost { get; set; }
        public decimal FBOCost { get; set; }
        public decimal TotalOperatingCost => FuelCost + MaintenanceCost + DepreciationCost + InsuranceCost + CrewCost + LandingFees + GroundHandlingCost + CateringCost + FBOCost;
        public decimal PassengerRevenue { get; set; }
        public decimal CargoRevenue { get; set; }
        public decimal TotalRevenue => PassengerRevenue + CargoRevenue;
        public decimal TotalPrice { get; set; }
        public decimal Profit => TotalPrice - TotalOperatingCost;
        public int Passengers { get; set; }
        public double CargoKg { get; set; }
        public double DistanceNM { get; set; }
        public double FlightHours { get; set; }
    }

    public class DailyOperatingCosts
    {
        public decimal FuelCost { get; set; }
        public decimal MaintenanceCost { get; set; }
        public decimal DepreciationCost { get; set; }
        public decimal InsuranceCost { get; set; }
        public decimal CrewCost { get; set; }
        public decimal FBOCost { get; set; }
        public decimal TotalDailyCost => FuelCost + MaintenanceCost + DepreciationCost + InsuranceCost + CrewCost + FBOCost;
    }

    public class DailyPassiveEarnings
    {
        public decimal Revenue { get; set; }
        public decimal FuelCost { get; set; }
        public decimal MaintenanceCost { get; set; }
        public decimal DepreciationCost { get; set; }
        public decimal InsuranceCost { get; set; }
        public decimal CrewCost { get; set; }
        public decimal FBOCost { get; set; }
        public decimal ServiceBonusAmount { get; set; }
        public decimal TotalCost => FuelCost + MaintenanceCost + DepreciationCost + InsuranceCost + CrewCost + FBOCost;
        public decimal Profit => Revenue + ServiceBonusAmount - TotalCost;
        public double DailyFlightHours { get; set; }
        public double DailyDistanceNM { get; set; }
        public int Passengers { get; set; }
    }
}
