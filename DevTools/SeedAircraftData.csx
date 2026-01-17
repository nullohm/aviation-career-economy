#r "nuget: Microsoft.EntityFrameworkCore.Sqlite, 9.0.0"

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

var dbPath = @"e:\02_SW_Projects\aviation-career-economy\Savegames\Current\ace.db";

var optionsBuilder = new DbContextOptionsBuilder<AceContext>();
optionsBuilder.UseSqlite($"Data Source={dbPath}");

using var context = new AceContext(optionsBuilder.Options);

var aircraft = new List<Aircraft>
{
    new Aircraft
    {
        Name = "Cessna 172 Skyhawk",
        Type = "SEP",
        Manufacturer = "Cessna",
        CruiseSpeed = 122,
        FuelCapacity = 212,
        FuelConsumption = 8.5m,
        Range = 640,
        PassengerCapacity = 3,
        CargoCapacity = 120,
        PurchasePrice = 250000,
        HourlyOperatingCost = 85,
        IsOwned = true,
        Condition = 100,
        Location = "EDDF",
        TotalFlightHours = 0
    },
    new Aircraft
    {
        Name = "Airbus A320neo",
        Type = "JET",
        Manufacturer = "Airbus",
        CruiseSpeed = 450,
        FuelCapacity = 27200,
        FuelConsumption = 2400m,
        Range = 3400,
        PassengerCapacity = 180,
        CargoCapacity = 3000,
        PurchasePrice = 110000000,
        HourlyOperatingCost = 4500,
        IsOwned = false,
        Condition = 100,
        Location = "EDDF",
        TotalFlightHours = 0
    },
    new Aircraft
    {
        Name = "Boeing 737-800",
        Type = "JET",
        Manufacturer = "Boeing",
        CruiseSpeed = 453,
        FuelCapacity = 26020,
        FuelConsumption = 2500m,
        Range = 3115,
        PassengerCapacity = 189,
        CargoCapacity = 2860,
        PurchasePrice = 102000000,
        HourlyOperatingCost = 4800,
        IsOwned = false,
        Condition = 100,
        Location = "KJFK",
        TotalFlightHours = 0
    },
    new Aircraft
    {
        Name = "Beechcraft Baron G58",
        Type = "MEP",
        Manufacturer = "Beechcraft",
        CruiseSpeed = 200,
        FuelCapacity = 544,
        FuelConsumption = 30m,
        Range = 1480,
        PassengerCapacity = 5,
        CargoCapacity = 450,
        PurchasePrice = 1200000,
        HourlyOperatingCost = 280,
        IsOwned = true,
        Condition = 95,
        Location = "EDDM",
        TotalFlightHours = 125
    },
    new Aircraft
    {
        Name = "Cessna Citation CJ4",
        Type = "BIZ",
        Manufacturer = "Cessna",
        CruiseSpeed = 451,
        FuelCapacity = 5760,
        FuelConsumption = 190m,
        Range = 2165,
        PassengerCapacity = 9,
        CargoCapacity = 450,
        PurchasePrice = 9000000,
        HourlyOperatingCost = 1850,
        IsOwned = false,
        Condition = 100,
        Location = "LSZH",
        TotalFlightHours = 0
    },
    new Aircraft
    {
        Name = "Piper PA-28 Cherokee",
        Type = "SEP",
        Manufacturer = "Piper",
        CruiseSpeed = 115,
        FuelCapacity = 190,
        FuelConsumption = 9.2m,
        Range = 520,
        PassengerCapacity = 3,
        CargoCapacity = 90,
        PurchasePrice = 180000,
        HourlyOperatingCost = 75,
        IsOwned = true,
        Condition = 88,
        Location = "EDDF",
        TotalFlightHours = 456
    },
    new Aircraft
    {
        Name = "Diamond DA40",
        Type = "SEP",
        Manufacturer = "Diamond",
        CruiseSpeed = 142,
        FuelCapacity = 160,
        FuelConsumption = 7.8m,
        Range = 750,
        PassengerCapacity = 3,
        CargoCapacity = 110,
        PurchasePrice = 320000,
        HourlyOperatingCost = 95,
        IsOwned = true,
        Condition = 100,
        Location = "LOWW",
        TotalFlightHours = 0
    },
    new Aircraft
    {
        Name = "Boeing 787-9 Dreamliner",
        Type = "JET",
        Manufacturer = "Boeing",
        CruiseSpeed = 490,
        FuelCapacity = 126206,
        FuelConsumption = 5400m,
        Range = 7635,
        PassengerCapacity = 296,
        CargoCapacity = 5500,
        PurchasePrice = 292000000,
        HourlyOperatingCost = 8500,
        IsOwned = false,
        Condition = 100,
        Location = "KLAX",
        TotalFlightHours = 0
    }
};

context.Aircraft.AddRange(aircraft);
var count = context.SaveChanges();

Console.WriteLine($"Successfully added {count} aircraft to the database!");

public class AceContext : DbContext
{
    public AceContext(DbContextOptions<AceContext> options) : base(options) { }
    public DbSet<Aircraft> Aircraft { get; set; }
}

public class Aircraft
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Manufacturer { get; set; }
    public int CruiseSpeed { get; set; }
    public decimal FuelCapacity { get; set; }
    public decimal FuelConsumption { get; set; }
    public int Range { get; set; }
    public int PassengerCapacity { get; set; }
    public int CargoCapacity { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal HourlyOperatingCost { get; set; }
    public bool IsOwned { get; set; }
    public int Condition { get; set; }
    public string Location { get; set; }
    public decimal TotalFlightHours { get; set; }
}
