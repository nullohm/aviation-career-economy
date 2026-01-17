using System;
using System.Collections.Generic;

namespace Ace.App.Models
{
    public class Aircraft
    {
        public int Id { get; set; }
        public string Registration { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Variant { get; set; } = string.Empty;
        public string CategoryString { get; set; } = string.Empty;
        public AircraftCategory Category { get; set; } = AircraftCategory.SingleEnginePiston;
        public string HomeBase { get; set; } = string.Empty;
        public AircraftStatus Status { get; set; } = AircraftStatus.Available;
        public int? AssignedFBOId { get; set; }
        public double TotalFlightHours { get; set; }
        public double HoursSinceLastMaintenance { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public DateTime? MaintenanceCompletionDate { get; set; }
        public MaintenanceCheckType? CurrentMaintenanceType { get; set; }
        public DateTime? LastAnnualInspection { get; set; }
        public double HoursSinceTBO { get; set; }
        public double HoursSinceACheck { get; set; }
        public DateTime? LastACheck { get; set; }
        public DateTime? LastBCheck { get; set; }
        public DateTime? LastCCheck { get; set; }
        public DateTime? LastDCheck { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal CurrentValue { get; set; }
        public int MaxPassengers { get; set; }
        public double MaxCargoKg { get; set; }
        public double MaxRangeNM { get; set; }
        public double CruiseSpeedKts { get; set; }
        public double FuelCapacityGal { get; set; }
        public double FuelBurnGalPerHour { get; set; }
        public decimal HourlyOperatingCost { get; set; }
        public int ServiceCeilingFt { get; set; }
        public int? AssignedPilotId { get; set; }
        public string? CustomImagePath { get; set; }
        public bool IsOldtimer { get; set; }

        public virtual ICollection<MaintenanceCheck> MaintenanceHistory { get; set; } = new List<MaintenanceCheck>();
    }

    public enum AircraftStatus
    {
        Available,
        InFlight,
        Maintenance,
        Grounded,
        Stationed
    }
}
