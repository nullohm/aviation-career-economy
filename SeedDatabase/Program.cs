using System;
using System.Linq;
using Ace.App.Data;
using Ace.App.Models;

var dbPath = @"e:\02_SW_Projects\aviation-career-economy\Savegames\Current\ace.db";

Console.WriteLine($"Seeding database at: {dbPath}");

using var context = new AceDbContext();

var existingCount = context.Aircraft.Count();
Console.WriteLine($"Current aircraft count: {existingCount}");

var aircraft = new[]
{
    new Aircraft
    {
        Registration = "D-ESKY",
        Type = "Cessna 172 Skyhawk",
        Variant = "S",
        HomeBase = "EDDF",
        Status = AircraftStatus.Available,
        TotalFlightHours = 245.5,
        HoursSinceLastMaintenance = 45.5,
        LastMaintenanceDate = DateTime.Now.AddMonths(-2),
        PurchaseDate = DateTime.Now.AddYears(-2),
        PurchasePrice = 400000m,
        CurrentValue = 380000m,
        MaxPassengers = 3,
        MaxCargoKg = 200,
        MaxRangeNM = 640,
        CruiseSpeedKts = 122,
        FuelCapacityGal = 56,
        FuelBurnGalPerHour = 8.5,
        HourlyOperatingCost = 85m
    },
    new Aircraft
    {
        Registration = "D-AIAA",
        Type = "Airbus A320neo",
        Variant = "251N",
        HomeBase = "EDDF",
        Status = AircraftStatus.Available,
        TotalFlightHours = 8520.0,
        HoursSinceLastMaintenance = 320.0,
        LastMaintenanceDate = DateTime.Now.AddMonths(-1),
        PurchaseDate = DateTime.Now.AddYears(-5),
        PurchasePrice = 111000000m,
        CurrentValue = 98000000m,
        MaxPassengers = 180,
        MaxCargoKg = 3500,
        MaxRangeNM = 3400,
        CruiseSpeedKts = 450,
        FuelCapacityGal = 7190,
        FuelBurnGalPerHour = 634,
        HourlyOperatingCost = 4500m
    },
    new Aircraft
    {
        Registration = "D-BARO",
        Type = "Beechcraft Baron G58",
        Variant = "G58",
        HomeBase = "EDDM",
        Status = AircraftStatus.Available,
        TotalFlightHours = 1250.0,
        HoursSinceLastMaintenance = 125.0,
        LastMaintenanceDate = DateTime.Now.AddMonths(-3),
        PurchaseDate = DateTime.Now.AddYears(-3),
        PurchasePrice = 1500000m,
        CurrentValue = 1350000m,
        MaxPassengers = 5,
        MaxCargoKg = 450,
        MaxRangeNM = 1480,
        CruiseSpeedKts = 200,
        FuelCapacityGal = 144,
        FuelBurnGalPerHour = 30,
        HourlyOperatingCost = 280m
    },
    new Aircraft
    {
        Registration = "D-PIPA",
        Type = "Piper PA-28 Cherokee",
        Variant = "Archer III",
        HomeBase = "EDDF",
        Status = AircraftStatus.Available,
        TotalFlightHours = 4560.0,
        HoursSinceLastMaintenance = 78.0,
        LastMaintenanceDate = DateTime.Now.AddMonths(-1).AddDays(-15),
        PurchaseDate = DateTime.Now.AddYears(-8),
        PurchasePrice = 280000m,
        CurrentValue = 220000m,
        MaxPassengers = 3,
        MaxCargoKg = 250,
        MaxRangeNM = 520,
        CruiseSpeedKts = 115,
        FuelCapacityGal = 50,
        FuelBurnGalPerHour = 9.2,
        HourlyOperatingCost = 75m
    },
    new Aircraft
    {
        Registration = "OE-DIA",
        Type = "Diamond DA40",
        Variant = "NG",
        HomeBase = "LOWW",
        Status = AircraftStatus.Available,
        TotalFlightHours = 125.0,
        HoursSinceLastMaintenance = 25.0,
        LastMaintenanceDate = DateTime.Now.AddDays(-20),
        PurchaseDate = DateTime.Now.AddMonths(-6),
        PurchasePrice = 550000m,
        CurrentValue = 545000m,
        MaxPassengers = 3,
        MaxCargoKg = 230,
        MaxRangeNM = 750,
        CruiseSpeedKts = 142,
        FuelCapacityGal = 42.2,
        FuelBurnGalPerHour = 7.8,
        HourlyOperatingCost = 95m
    },
    new Aircraft
    {
        Registration = "D-CIRC",
        Type = "Cirrus SR22",
        Variant = "T",
        HomeBase = "EDDH",
        Status = AircraftStatus.Available,
        TotalFlightHours = 890.0,
        HoursSinceLastMaintenance = 90.0,
        LastMaintenanceDate = DateTime.Now.AddMonths(-2),
        PurchaseDate = DateTime.Now.AddYears(-1),
        PurchasePrice = 950000m,
        CurrentValue = 920000m,
        MaxPassengers = 4,
        MaxCargoKg = 180,
        MaxRangeNM = 1000,
        CruiseSpeedKts = 213,
        FuelCapacityGal = 92,
        FuelBurnGalPerHour = 18.5,
        HourlyOperatingCost = 165m
    },
    new Aircraft
    {
        Registration = "HB-PIL",
        Type = "Pilatus PC-12",
        Variant = "NGX",
        HomeBase = "LSZH",
        Status = AircraftStatus.Available,
        TotalFlightHours = 2340.0,
        HoursSinceLastMaintenance = 234.0,
        LastMaintenanceDate = DateTime.Now.AddMonths(-4),
        PurchaseDate = DateTime.Now.AddYears(-4),
        PurchasePrice = 6200000m,
        CurrentValue = 5500000m,
        MaxPassengers = 9,
        MaxCargoKg = 600,
        MaxRangeNM = 1845,
        CruiseSpeedKts = 290,
        FuelCapacityGal = 402,
        FuelBurnGalPerHour = 75,
        HourlyOperatingCost = 850m
    },
    new Aircraft
    {
        Registration = "N123CJ",
        Type = "Cessna Citation CJ4",
        Variant = "Gen2",
        HomeBase = "KJFK",
        Status = AircraftStatus.Available,
        TotalFlightHours = 3200.0,
        HoursSinceLastMaintenance = 200.0,
        LastMaintenanceDate = DateTime.Now.AddMonths(-3),
        PurchaseDate = DateTime.Now.AddYears(-6),
        PurchasePrice = 11500000m,
        CurrentValue = 9200000m,
        MaxPassengers = 9,
        MaxCargoKg = 500,
        MaxRangeNM = 2165,
        CruiseSpeedKts = 451,
        FuelCapacityGal = 1522,
        FuelBurnGalPerHour = 190,
        HourlyOperatingCost = 1850m
    },
    new Aircraft
    {
        Registration = "N787BA",
        Type = "Boeing 737-800",
        Variant = "8AS",
        HomeBase = "KLAX",
        Status = AircraftStatus.Available,
        TotalFlightHours = 25600.0,
        HoursSinceLastMaintenance = 456.0,
        LastMaintenanceDate = DateTime.Now.AddMonths(-2),
        PurchaseDate = DateTime.Now.AddYears(-10),
        PurchasePrice = 102000000m,
        CurrentValue = 58000000m,
        MaxPassengers = 189,
        MaxCargoKg = 3400,
        MaxRangeNM = 3115,
        CruiseSpeedKts = 453,
        FuelCapacityGal = 6875,
        FuelBurnGalPerHour = 660,
        HourlyOperatingCost = 4800m
    },
    new Aircraft
    {
        Registration = "D-KING",
        Type = "Beechcraft King Air 350i",
        Variant = "350i",
        HomeBase = "EDDL",
        Status = AircraftStatus.Available,
        TotalFlightHours = 4500.0,
        HoursSinceLastMaintenance = 250.0,
        LastMaintenanceDate = DateTime.Now.AddMonths(-3),
        PurchaseDate = DateTime.Now.AddYears(-7),
        PurchasePrice = 7400000m,
        CurrentValue = 5800000m,
        MaxPassengers = 11,
        MaxCargoKg = 550,
        MaxRangeNM = 1806,
        CruiseSpeedKts = 312,
        FuelCapacityGal = 539,
        FuelBurnGalPerHour = 90,
        HourlyOperatingCost = 1200m
    },
    new Aircraft
    {
        Registration = "F-GTBM",
        Type = "TBM 960",
        Variant = "960",
        HomeBase = "LFPG",
        Status = AircraftStatus.Available,
        TotalFlightHours = 680.0,
        HoursSinceLastMaintenance = 80.0,
        LastMaintenanceDate = DateTime.Now.AddMonths(-1).AddDays(-10),
        PurchaseDate = DateTime.Now.AddMonths(-9),
        PurchasePrice = 4800000m,
        CurrentValue = 4750000m,
        MaxPassengers = 6,
        MaxCargoKg = 300,
        MaxRangeNM = 1730,
        CruiseSpeedKts = 330,
        FuelCapacityGal = 272,
        FuelBurnGalPerHour = 55,
        HourlyOperatingCost = 720m
    },
    new Aircraft
    {
        Registration = "G-GULF",
        Type = "Gulfstream G650",
        Variant = "ER",
        HomeBase = "EGLL",
        Status = AircraftStatus.Available,
        TotalFlightHours = 1890.0,
        HoursSinceLastMaintenance = 190.0,
        LastMaintenanceDate = DateTime.Now.AddMonths(-2).AddDays(-15),
        PurchaseDate = DateTime.Now.AddYears(-3),
        PurchasePrice = 65000000m,
        CurrentValue = 58000000m,
        MaxPassengers = 19,
        MaxCargoKg = 2500,
        MaxRangeNM = 7500,
        CruiseSpeedKts = 516,
        FuelCapacityGal = 11820,
        FuelBurnGalPerHour = 470,
        HourlyOperatingCost = 6500m
    }
};

context.Aircraft.AddRange(aircraft);
var count = context.SaveChanges();

Console.WriteLine($"Successfully added {count} aircraft!");
Console.WriteLine($"New total: {context.Aircraft.Count()} aircraft");

Console.WriteLine("\nAdded aircraft:");
foreach (var ac in aircraft)
{
    Console.WriteLine($"  {ac.Registration} - {ac.Type} ({ac.Variant}) - {ac.HomeBase}");
}
