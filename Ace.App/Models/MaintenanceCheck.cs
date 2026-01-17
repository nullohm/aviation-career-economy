using System;
using Ace.App.Services;

namespace Ace.App.Models
{
    public class MaintenanceCheck
    {
        public int Id { get; set; }
        public int AircraftId { get; set; }
        public MaintenanceCheckType CheckType { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public double FlightHoursAtCheck { get; set; }
        public decimal Cost { get; set; }
        public int DurationDays { get; set; }
        public string? Notes { get; set; }

        public virtual Aircraft? Aircraft { get; set; }
    }

    public enum MaintenanceCheckType
    {
        Check50Hour,
        Check100Hour,
        AnnualInspection,
        TBO,
        ACheck,
        BCheck,
        CCheck,
        DCheck
    }

    public enum AircraftCategory
    {
        SingleEnginePiston,
        MultiEnginePiston,
        Turboprop,
        BusinessJet,
        RegionalJet,
        NarrowBody,
        WideBody
    }

    public static class MaintenanceCheckDefinitions
    {
        public static MaintenanceCheckInfo GetCheckInfo(MaintenanceCheckType checkType, AircraftCategory category, decimal aircraftValue, AppSettings? settings = null)
        {
            return checkType switch
            {
                MaintenanceCheckType.Check50Hour => new MaintenanceCheckInfo
                {
                    Name = "50-Hour Check",
                    Description = "Oil change and visual inspection",
                    IntervalHours = 50,
                    IntervalMonths = null,
                    DurationHours = 4,
                    DurationDays = 0,
                    BaseCost = settings?.MaintenanceCheck50Hour ?? 450m,
                    CostPercentOfValue = 0
                },
                MaintenanceCheckType.Check100Hour => new MaintenanceCheckInfo
                {
                    Name = "100-Hour Check",
                    Description = "Detailed inspection of engine, controls, and systems",
                    IntervalHours = 100,
                    IntervalMonths = null,
                    DurationHours = 16,
                    DurationDays = 1,
                    BaseCost = settings?.MaintenanceCheck100Hour ?? 2500m,
                    CostPercentOfValue = 0
                },
                MaintenanceCheckType.AnnualInspection => new MaintenanceCheckInfo
                {
                    Name = "Annual Inspection",
                    Description = "Comprehensive yearly inspection required by regulations",
                    IntervalHours = null,
                    IntervalMonths = 12,
                    DurationHours = 24,
                    DurationDays = 2,
                    BaseCost = settings?.MaintenanceCheckAnnual ?? 3500m,
                    CostPercentOfValue = 0
                },
                MaintenanceCheckType.TBO => new MaintenanceCheckInfo
                {
                    Name = "Engine Overhaul (TBO)",
                    Description = "Complete engine overhaul or replacement",
                    IntervalHours = GetTBOInterval(category),
                    IntervalMonths = null,
                    DurationHours = 80,
                    DurationDays = 14,
                    BaseCost = GetTBOCost(category),
                    CostPercentOfValue = 0
                },
                MaintenanceCheckType.ACheck => new MaintenanceCheckInfo
                {
                    Name = "A-Check",
                    Description = "Routine inspection: filters, lubrication, visual checks",
                    IntervalHours = GetACheckInterval(category),
                    IntervalMonths = 2,
                    DurationHours = 10,
                    DurationDays = 1,
                    BaseCost = settings?.MaintenanceCheckACheck ?? 20000m,
                    CostPercentOfValue = 0
                },
                MaintenanceCheckType.BCheck => new MaintenanceCheckInfo
                {
                    Name = "B-Check",
                    Description = "More detailed inspection, often combined with A-Check",
                    IntervalHours = null,
                    IntervalMonths = 5,
                    DurationHours = 48,
                    DurationDays = 2,
                    BaseCost = settings?.MaintenanceCheckBCheck ?? 50000m,
                    CostPercentOfValue = 0
                },
                MaintenanceCheckType.CCheck => new MaintenanceCheckInfo
                {
                    Name = "C-Check (Heavy Maintenance)",
                    Description = "Aircraft stripped down, structural inspection with X-ray/ultrasound",
                    IntervalHours = null,
                    IntervalMonths = 20,
                    DurationHours = 5000,
                    DurationDays = 14,
                    BaseCost = settings?.MaintenanceCheckCCheck ?? 300000m,
                    CostPercentOfValue = 0
                },
                MaintenanceCheckType.DCheck => new MaintenanceCheckInfo
                {
                    Name = "D-Check (Overhaul)",
                    Description = "Complete disassembly to bare frame, aircraft is like-new after",
                    IntervalHours = null,
                    IntervalMonths = 72,
                    DurationHours = 40000,
                    DurationDays = 42,
                    BaseCost = settings?.MaintenanceCheckDCheck ?? 3500000m,
                    CostPercentOfValue = 0
                },
                _ => throw new ArgumentException($"Unknown check type: {checkType}")
            };
        }

        public static MaintenanceCheckType[] GetApplicableChecks(AircraftCategory category)
        {
            return category switch
            {
                AircraftCategory.SingleEnginePiston or AircraftCategory.MultiEnginePiston =>
                    new[] { MaintenanceCheckType.Check50Hour, MaintenanceCheckType.Check100Hour, MaintenanceCheckType.AnnualInspection, MaintenanceCheckType.TBO },
                AircraftCategory.Turboprop or AircraftCategory.BusinessJet =>
                    new[] { MaintenanceCheckType.Check100Hour, MaintenanceCheckType.AnnualInspection, MaintenanceCheckType.ACheck, MaintenanceCheckType.CCheck },
                AircraftCategory.RegionalJet or AircraftCategory.NarrowBody or AircraftCategory.WideBody =>
                    new[] { MaintenanceCheckType.ACheck, MaintenanceCheckType.BCheck, MaintenanceCheckType.CCheck, MaintenanceCheckType.DCheck },
                _ => new[] { MaintenanceCheckType.Check100Hour, MaintenanceCheckType.AnnualInspection }
            };
        }

        private static int GetTBOInterval(AircraftCategory category)
        {
            return category switch
            {
                AircraftCategory.SingleEnginePiston => 2000,
                AircraftCategory.MultiEnginePiston => 1800,
                AircraftCategory.Turboprop => 3500,
                AircraftCategory.BusinessJet => 5000,
                _ => 2000
            };
        }

        private static decimal GetTBOCost(AircraftCategory category)
        {
            return category switch
            {
                AircraftCategory.SingleEnginePiston => 40000m,
                AircraftCategory.MultiEnginePiston => 80000m,
                AircraftCategory.Turboprop => 250000m,
                AircraftCategory.BusinessJet => 500000m,
                _ => 40000m
            };
        }

        private static int GetACheckInterval(AircraftCategory category)
        {
            return category switch
            {
                AircraftCategory.Turboprop => 500,
                AircraftCategory.BusinessJet => 600,
                AircraftCategory.RegionalJet => 500,
                AircraftCategory.NarrowBody => 600,
                AircraftCategory.WideBody => 750,
                _ => 500
            };
        }

        public static AircraftCategory DetermineCategory(string categoryString, int maxPassengers, double cruiseSpeed)
        {
            var lowerCategory = categoryString?.ToLowerInvariant() ?? "";

            if (lowerCategory.Contains("airliner") || lowerCategory.Contains("jet"))
            {
                if (maxPassengers >= 200)
                    return AircraftCategory.WideBody;
                if (maxPassengers >= 100)
                    return AircraftCategory.NarrowBody;
                if (maxPassengers >= 50)
                    return AircraftCategory.RegionalJet;
                if (cruiseSpeed > 350)
                    return AircraftCategory.BusinessJet;
            }

            if (lowerCategory.Contains("turboprop") || (cruiseSpeed > 200 && cruiseSpeed <= 350 && maxPassengers < 50))
                return AircraftCategory.Turboprop;

            if (lowerCategory.Contains("multi") || lowerCategory.Contains("twin"))
                return AircraftCategory.MultiEnginePiston;

            return AircraftCategory.SingleEnginePiston;
        }
    }

    public class MaintenanceCheckInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? IntervalHours { get; set; }
        public int? IntervalMonths { get; set; }
        public int DurationHours { get; set; }
        public int DurationDays { get; set; }
        public decimal BaseCost { get; set; }
        public decimal CostPercentOfValue { get; set; }
    }
}
