#r "nuget: Microsoft.EntityFrameworkCore.Sqlite, 9.0.0"

using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

var solutionRoot = FindSolutionRoot() ?? AppContext.BaseDirectory;
var dbPath = Path.Combine(solutionRoot, "Savegames", "Current", "ace.db");

Console.WriteLine($"Using database: {dbPath}");

if (!File.Exists(dbPath))
{
    Console.WriteLine("Database not found!");
    return;
}

var optionsBuilder = new DbContextOptionsBuilder<AceContext>();
optionsBuilder.UseSqlite($"Data Source={dbPath}");

var context = new AceContext(optionsBuilder.Options);

var now = DateTime.Now;

var catalogEntries = new[]
{
    new AircraftCatalogEntry
    {
        Title = "Cessna 172 Skyhawk G1000",
        Manufacturer = "Textron Aviation",
        Type = "C172",
        Category = "Airplane",
        CrewCount = 1,
        PassengerCapacity = 3,
        MaxCargoKg = 200,
        MarketPrice = 400000m,
        CruiseSpeedKts = 122,
        MaxRangeNM = 640,
        FuelCapacityGal = 56,
        FuelBurnGalPerHour = 8.5,
        HourlyOperatingCost = 77m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Cessna 152",
        Manufacturer = "Textron Aviation",
        Type = "C152",
        Category = "Airplane",
        CrewCount = 1,
        PassengerCapacity = 1,
        MaxCargoKg = 120,
        MarketPrice = 180000m,
        CruiseSpeedKts = 107,
        MaxRangeNM = 415,
        FuelCapacityGal = 26,
        FuelBurnGalPerHour = 6.1,
        HourlyOperatingCost = 55m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Cessna 208B Grand Caravan EX",
        Manufacturer = "Textron Aviation",
        Type = "C208",
        Category = "Airplane",
        CrewCount = 1,
        PassengerCapacity = 13,
        MaxCargoKg = 1200,
        MarketPrice = 2900000m,
        CruiseSpeedKts = 185,
        MaxRangeNM = 912,
        FuelCapacityGal = 335,
        FuelBurnGalPerHour = 47,
        HourlyOperatingCost = 423m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Cessna Citation CJ4 Gen2",
        Manufacturer = "Textron Aviation",
        Type = "CJ4",
        Category = "Airplane",
        CrewCount = 2,
        PassengerCapacity = 9,
        MaxCargoKg = 500,
        MarketPrice = 11500000m,
        CruiseSpeedKts = 451,
        MaxRangeNM = 2165,
        FuelCapacityGal = 5760,
        FuelBurnGalPerHour = 190,
        HourlyOperatingCost = 1710m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Piper PA-28-181 Archer III",
        Manufacturer = "Piper Aircraft",
        Type = "PA28",
        Category = "Airplane",
        CrewCount = 1,
        PassengerCapacity = 3,
        MaxCargoKg = 250,
        MarketPrice = 380000m,
        CruiseSpeedKts = 128,
        MaxRangeNM = 696,
        FuelCapacityGal = 50,
        FuelBurnGalPerHour = 9.2,
        HourlyOperatingCost = 83m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Piper PA-44-180 Seminole",
        Manufacturer = "Piper Aircraft",
        Type = "PA44",
        Category = "Airplane",
        CrewCount = 1,
        PassengerCapacity = 3,
        MaxCargoKg = 350,
        MarketPrice = 680000m,
        CruiseSpeedKts = 165,
        MaxRangeNM = 828,
        FuelCapacityGal = 108,
        FuelBurnGalPerHour = 18,
        HourlyOperatingCost = 162m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Diamond DA40 NG",
        Manufacturer = "Diamond Aircraft",
        Type = "DA40",
        Category = "Airplane",
        CrewCount = 1,
        PassengerCapacity = 3,
        MaxCargoKg = 230,
        MarketPrice = 550000m,
        CruiseSpeedKts = 142,
        MaxRangeNM = 750,
        FuelCapacityGal = 40,
        FuelBurnGalPerHour = 7.8,
        HourlyOperatingCost = 70m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Diamond DA62",
        Manufacturer = "Diamond Aircraft",
        Type = "DA62",
        Category = "Airplane",
        CrewCount = 1,
        PassengerCapacity = 6,
        MaxCargoKg = 400,
        MarketPrice = 1400000m,
        CruiseSpeedKts = 192,
        MaxRangeNM = 1285,
        FuelCapacityGal = 93,
        FuelBurnGalPerHour = 14,
        HourlyOperatingCost = 126m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Cirrus SR22T G6",
        Manufacturer = "Cirrus Aircraft",
        Type = "SR22",
        Category = "Airplane",
        CrewCount = 1,
        PassengerCapacity = 4,
        MaxCargoKg = 180,
        MarketPrice = 950000m,
        CruiseSpeedKts = 213,
        MaxRangeNM = 1207,
        FuelCapacityGal = 92,
        FuelBurnGalPerHour = 17.5,
        HourlyOperatingCost = 158m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Beechcraft Baron G58",
        Manufacturer = "Textron Aviation",
        Type = "BE58",
        Category = "Airplane",
        CrewCount = 1,
        PassengerCapacity = 5,
        MaxCargoKg = 450,
        MarketPrice = 1500000m,
        CruiseSpeedKts = 200,
        MaxRangeNM = 1480,
        FuelCapacityGal = 144,
        FuelBurnGalPerHour = 30,
        HourlyOperatingCost = 270m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Beechcraft Bonanza G36",
        Manufacturer = "Textron Aviation",
        Type = "BE36",
        Category = "Airplane",
        CrewCount = 1,
        PassengerCapacity = 5,
        MaxCargoKg = 380,
        MarketPrice = 1100000m,
        CruiseSpeedKts = 176,
        MaxRangeNM = 920,
        FuelCapacityGal = 74,
        FuelBurnGalPerHour = 15,
        HourlyOperatingCost = 135m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Beechcraft King Air 350i",
        Manufacturer = "Textron Aviation",
        Type = "B350",
        Category = "Airplane",
        CrewCount = 2,
        PassengerCapacity = 11,
        MaxCargoKg = 550,
        MarketPrice = 7400000m,
        CruiseSpeedKts = 312,
        MaxRangeNM = 1806,
        FuelCapacityGal = 539,
        FuelBurnGalPerHour = 115,
        HourlyOperatingCost = 1035m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Pilatus PC-12 NGX",
        Manufacturer = "Pilatus",
        Type = "PC12",
        Category = "Airplane",
        CrewCount = 1,
        PassengerCapacity = 9,
        MaxCargoKg = 600,
        MarketPrice = 6200000m,
        CruiseSpeedKts = 290,
        MaxRangeNM = 1803,
        FuelCapacityGal = 402,
        FuelBurnGalPerHour = 70,
        HourlyOperatingCost = 630m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "TBM 960",
        Manufacturer = "Daher",
        Type = "TBM9",
        Category = "Airplane",
        CrewCount = 1,
        PassengerCapacity = 6,
        MaxCargoKg = 300,
        MarketPrice = 4800000m,
        CruiseSpeedKts = 330,
        MaxRangeNM = 1730,
        FuelCapacityGal = 284,
        FuelBurnGalPerHour = 60,
        HourlyOperatingCost = 540m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Airbus A320neo",
        Manufacturer = "Airbus",
        Type = "A320",
        Category = "Airplane",
        CrewCount = 2,
        PassengerCapacity = 180,
        MaxCargoKg = 3500,
        MarketPrice = 111000000m,
        CruiseSpeedKts = 450,
        MaxRangeNM = 3400,
        FuelCapacityGal = 7190,
        FuelBurnGalPerHour = 634,
        HourlyOperatingCost = 5706m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Boeing 737-800",
        Manufacturer = "Boeing",
        Type = "B738",
        Category = "Airplane",
        CrewCount = 2,
        PassengerCapacity = 189,
        MaxCargoKg = 3400,
        MarketPrice = 102000000m,
        CruiseSpeedKts = 453,
        MaxRangeNM = 3115,
        FuelCapacityGal = 6875,
        FuelBurnGalPerHour = 660,
        HourlyOperatingCost = 5940m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Boeing 787-9 Dreamliner",
        Manufacturer = "Boeing",
        Type = "B789",
        Category = "Airplane",
        CrewCount = 2,
        PassengerCapacity = 296,
        MaxCargoKg = 11000,
        MarketPrice = 292000000m,
        CruiseSpeedKts = 490,
        MaxRangeNM = 7635,
        FuelCapacityGal = 33340,
        FuelBurnGalPerHour = 1426,
        HourlyOperatingCost = 12834m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Airbus A380-800",
        Manufacturer = "Airbus",
        Type = "A388",
        Category = "Airplane",
        CrewCount = 2,
        PassengerCapacity = 575,
        MaxCargoKg = 18000,
        MarketPrice = 445000000m,
        CruiseSpeedKts = 488,
        MaxRangeNM = 8208,
        FuelCapacityGal = 84600,
        FuelBurnGalPerHour = 3170,
        HourlyOperatingCost = 28530m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "ATR 72-600",
        Manufacturer = "ATR",
        Type = "AT76",
        Category = "Airplane",
        CrewCount = 2,
        PassengerCapacity = 78,
        MaxCargoKg = 1500,
        MarketPrice = 27000000m,
        CruiseSpeedKts = 276,
        MaxRangeNM = 900,
        FuelCapacityGal = 1320,
        FuelBurnGalPerHour = 145,
        HourlyOperatingCost = 1305m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Embraer E175",
        Manufacturer = "Embraer",
        Type = "E75L",
        Category = "Airplane",
        CrewCount = 2,
        PassengerCapacity = 88,
        MaxCargoKg = 1800,
        MarketPrice = 53000000m,
        CruiseSpeedKts = 447,
        MaxRangeNM = 2200,
        FuelCapacityGal = 3400,
        FuelBurnGalPerHour = 395,
        HourlyOperatingCost = 3555m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Robinson R44 Raven II",
        Manufacturer = "Robinson Helicopter",
        Type = "R44",
        Category = "Helicopter",
        CrewCount = 1,
        PassengerCapacity = 3,
        MaxCargoKg = 200,
        MarketPrice = 550000m,
        CruiseSpeedKts = 110,
        MaxRangeNM = 300,
        FuelCapacityGal = 30,
        FuelBurnGalPerHour = 15,
        HourlyOperatingCost = 135m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Bell 407GXi",
        Manufacturer = "Bell",
        Type = "B407",
        Category = "Helicopter",
        CrewCount = 1,
        PassengerCapacity = 6,
        MaxCargoKg = 450,
        MarketPrice = 4200000m,
        CruiseSpeedKts = 133,
        MaxRangeNM = 340,
        FuelCapacityGal = 110,
        FuelBurnGalPerHour = 35,
        HourlyOperatingCost = 315m,
        FirstSeen = now,
        LastSeen = now
    },
    new AircraftCatalogEntry
    {
        Title = "Airbus H145",
        Manufacturer = "Airbus Helicopters",
        Type = "H145",
        Category = "Helicopter",
        CrewCount = 1,
        PassengerCapacity = 9,
        MaxCargoKg = 600,
        MarketPrice = 12000000m,
        CruiseSpeedKts = 150,
        MaxRangeNM = 370,
        FuelCapacityGal = 250,
        FuelBurnGalPerHour = 65,
        HourlyOperatingCost = 585m,
        FirstSeen = now,
        LastSeen = now
    }
};

foreach (var entry in catalogEntries)
{
    var existing = context.AircraftCatalog.FirstOrDefault(a => a.Title == entry.Title);
    if (existing == null)
    {
        context.AircraftCatalog.Add(entry);
        Console.WriteLine($"Added: {entry.Manufacturer} {entry.Title} - {entry.MarketPrice:N0} €");
    }
    else
    {
        Console.WriteLine($"Already exists: {entry.Title}");
    }
}

var count = context.SaveChanges();
Console.WriteLine($"\nSuccessfully seeded {count} aircraft to the market catalog!");

string FindSolutionRoot()
{
    var currentDir = Directory.GetCurrentDirectory();
    while (currentDir != null)
    {
        if (Directory.GetFiles(currentDir, "*.sln").Length > 0)
            return currentDir;
        currentDir = Directory.GetParent(currentDir)?.FullName;
    }
    return null;
}

public class AceContext : DbContext
{
    public AceContext(DbContextOptions<AceContext> options) : base(options) { }
    public DbSet<AircraftCatalogEntry> AircraftCatalog { get; set; }
}

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
    public DateTime FirstSeen { get; set; }
    public DateTime LastSeen { get; set; }
}
