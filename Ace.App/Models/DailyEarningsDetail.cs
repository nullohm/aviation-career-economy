using System;
using System.ComponentModel.DataAnnotations;

namespace Ace.App.Models
{
    public class DailyEarningsDetail
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int AircraftId { get; set; }
        public string AircraftRegistration { get; set; } = string.Empty;
        public string AircraftType { get; set; } = string.Empty;
        public string PilotName { get; set; } = string.Empty;
        public double FlightHours { get; set; }
        public double DistanceNM { get; set; }
        public decimal Revenue { get; set; }
        public decimal FuelCost { get; set; }
        public decimal MaintenanceCost { get; set; }
        public decimal InsuranceCost { get; set; }
        public decimal DepreciationCost { get; set; }
        public decimal CrewCost { get; set; }
        public decimal FBOCost { get; set; }
        public decimal ServiceBonusAmount { get; set; }

        public decimal TotalCost => FuelCost + MaintenanceCost + InsuranceCost + DepreciationCost + CrewCost + FBOCost;
        public decimal Profit => Revenue + ServiceBonusAmount - TotalCost;

        // UI helper properties
        public decimal OtherCosts => DepreciationCost + FBOCost;
        public bool IsProfitNegative => Profit < 0;
    }
}
