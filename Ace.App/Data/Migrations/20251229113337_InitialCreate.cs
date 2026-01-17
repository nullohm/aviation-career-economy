using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace Ace.App.Data.Migrations
{

    public partial class InitialCreate : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aircraft",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Registration = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Variant = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryString = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    HomeBase = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignedFBOId = table.Column<int>(type: "INTEGER", nullable: true),
                    TotalFlightHours = table.Column<double>(type: "REAL", nullable: false),
                    HoursSinceLastMaintenance = table.Column<double>(type: "REAL", nullable: false),
                    LastMaintenanceDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MaintenanceCompletionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CurrentMaintenanceType = table.Column<int>(type: "INTEGER", nullable: true),
                    LastAnnualInspection = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoursSinceTBO = table.Column<double>(type: "REAL", nullable: false),
                    HoursSinceACheck = table.Column<double>(type: "REAL", nullable: false),
                    LastACheck = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastBCheck = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastCCheck = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastDCheck = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    CurrentValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    MaxPassengers = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxRangeNM = table.Column<double>(type: "REAL", nullable: false),
                    CruiseSpeedKts = table.Column<double>(type: "REAL", nullable: false),
                    FuelCapacityGal = table.Column<double>(type: "REAL", nullable: false),
                    FuelBurnGalPerHour = table.Column<double>(type: "REAL", nullable: false),
                    HourlyOperatingCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    AssignedPilotId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircraft", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AircraftCatalog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    CrewCount = table.Column<int>(type: "INTEGER", nullable: false),
                    PassengerCapacity = table.Column<int>(type: "INTEGER", nullable: false),
                    MarketPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    CruiseSpeedKts = table.Column<double>(type: "REAL", nullable: false),
                    MaxRangeNM = table.Column<double>(type: "REAL", nullable: false),
                    FuelCapacityGal = table.Column<double>(type: "REAL", nullable: false),
                    FuelBurnGalPerHour = table.Column<double>(type: "REAL", nullable: false),
                    HourlyOperatingCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    FirstSeen = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSeen = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftCatalog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AircraftPricings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AircraftNamePattern = table.Column<string>(type: "TEXT", nullable: false),
                    BasePrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftPricings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FBOs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ICAO = table.Column<string>(type: "TEXT", nullable: false),
                    AirportName = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    MonthlyRent = table.Column<decimal>(type: "TEXT", nullable: false),
                    RentedSince = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TerminalSize = table.Column<int>(type: "INTEGER", nullable: false),
                    TerminalMonthlyCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    HasRefuelingService = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasHangarService = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasCateringService = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasGroundHandling = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasDeIcingService = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FBOs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Aircraft = table.Column<string>(type: "TEXT", nullable: false),
                    Departure = table.Column<string>(type: "TEXT", nullable: false),
                    Arrival = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    LandingRate = table.Column<double>(type: "REAL", nullable: false),
                    DistanceNM = table.Column<double>(type: "REAL", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Earnings = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    InterestRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalRepayment = table.Column<decimal>(type: "TEXT", nullable: false),
                    TakenDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RepaidDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsRepaid = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MsfsAircraft",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    CrewCount = table.Column<int>(type: "INTEGER", nullable: false),
                    PassengerCapacity = table.Column<int>(type: "INTEGER", nullable: false),
                    NewPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    CruiseSpeedKts = table.Column<double>(type: "REAL", nullable: false),
                    MaxRangeNM = table.Column<double>(type: "REAL", nullable: false),
                    FuelCapacityGal = table.Column<double>(type: "REAL", nullable: false),
                    FuelBurnGalPerHour = table.Column<double>(type: "REAL", nullable: false),
                    HourlyOperatingCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    FirstDetected = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastDetected = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsfsAircraft", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pilots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Birthday = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    TotalFlightHours = table.Column<double>(type: "REAL", nullable: false),
                    IsEmployed = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPlayer = table.Column<bool>(type: "INTEGER", nullable: false),
                    SalaryPerMonth = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pilots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsSimConnectEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutoStartTracking = table.Column<bool>(type: "INTEGER", nullable: false),
                    WindowTop = table.Column<double>(type: "REAL", nullable: false),
                    WindowLeft = table.Column<double>(type: "REAL", nullable: false),
                    WindowWidth = table.Column<double>(type: "REAL", nullable: false),
                    WindowHeight = table.Column<double>(type: "REAL", nullable: false),
                    IsMaximized = table.Column<bool>(type: "INTEGER", nullable: false),
                    RatePerPaxPerNM = table.Column<decimal>(type: "TEXT", nullable: false),
                    LastDepartureIcao = table.Column<string>(type: "TEXT", nullable: false),
                    LastArrivalIcao = table.Column<string>(type: "TEXT", nullable: false),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastDailyEarningsDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FBORentLocal = table.Column<decimal>(type: "TEXT", nullable: false),
                    FBORentRegional = table.Column<decimal>(type: "TEXT", nullable: false),
                    FBORentInternational = table.Column<decimal>(type: "TEXT", nullable: false),
                    TerminalCostSmall = table.Column<decimal>(type: "TEXT", nullable: false),
                    TerminalCostMedium = table.Column<decimal>(type: "TEXT", nullable: false),
                    TerminalCostLarge = table.Column<decimal>(type: "TEXT", nullable: false),
                    ServiceCostPerService = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    FlightId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceChecks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AircraftId = table.Column<int>(type: "INTEGER", nullable: false),
                    CheckType = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FlightHoursAtCheck = table.Column<double>(type: "REAL", nullable: false),
                    Cost = table.Column<decimal>(type: "TEXT", nullable: false),
                    DurationDays = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceChecks_Aircraft_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircraft",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PilotId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IssuingAuthority = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Licenses_Pilots_PilotId",
                        column: x => x.PilotId,
                        principalTable: "Pilots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PilotId = table.Column<int>(type: "INTEGER", nullable: false),
                    AircraftType = table.Column<string>(type: "TEXT", nullable: false),
                    EarnedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IssuingAuthority = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeRatings_Pilots_PilotId",
                        column: x => x.PilotId,
                        principalTable: "Pilots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AircraftPricings",
                columns: new[] { "Id", "AircraftNamePattern", "BasePrice", "LastUpdated", "Priority" },
                values: new object[,]
                {
                    { 1, "747", 418000000m, new DateTime(2025, 12, 29, 12, 33, 34, 824, DateTimeKind.Local).AddTicks(5045), 1 },
                    { 2, "a380", 445000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7571), 1 },
                    { 3, "787", 248000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7589), 2 },
                    { 4, "dreamliner", 248000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7592), 2 },
                    { 5, "a350", 317000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7593), 2 },
                    { 6, "777", 375000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7595), 3 },
                    { 7, "a330", 264000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7596), 3 },
                    { 8, "a340", 238000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7614), 4 },
                    { 9, "737 max", 122000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7616), 20 },
                    { 10, "737", 100000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7617), 21 },
                    { 11, "b737", 100000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7619), 21 },
                    { 12, "a320neo", 111000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7620), 20 },
                    { 13, "a320", 101000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7622), 21 },
                    { 14, "a321", 130000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7623), 21 },
                    { 15, "a319", 92000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7625), 21 },
                    { 16, "a220", 81000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7626), 22 },
                    { 17, "c series", 81000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7682), 22 },
                    { 18, "a310", 65000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7684), 23 },
                    { 19, "a300", 65000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7685), 23 },
                    { 20, "757", 75000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7687), 24 },
                    { 21, "767", 85000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7688), 24 },
                    { 22, "717", 42000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7690), 25 },
                    { 23, "md-", 42000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7691), 25 },
                    { 24, "crj", 33000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7693), 41 },
                    { 25, "e170", 47000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7694), 42 },
                    { 26, "e175", 53000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7696), 42 },
                    { 27, "e190", 50000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7697), 42 },
                    { 28, "e195", 54000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7708), 42 },
                    { 29, "embraer e", 50000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7710), 43 },
                    { 30, "e-jet", 50000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7712), 43 },
                    { 31, "atr 72", 27000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7713), 61 },
                    { 32, "atr72", 27000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7715), 61 },
                    { 33, "atr 42", 18000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7716), 62 },
                    { 34, "atr42", 18000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7718), 62 },
                    { 35, "atr", 22000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7719), 63 },
                    { 36, "dash 8", 32000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7720), 64 },
                    { 37, "q400", 32000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7722), 64 },
                    { 38, "dhc-8", 32000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7723), 64 },
                    { 39, "saab 340", 12000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7725), 65 },
                    { 40, "saab340", 12000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7726), 65 },
                    { 41, "cessna 172", 400000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7728), 81 },
                    { 42, "c172", 400000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7729), 81 },
                    { 43, "skyhawk", 400000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7731), 81 },
                    { 44, "cessna 152", 180000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7732), 82 },
                    { 45, "c152", 180000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7734), 82 },
                    { 46, "cessna 208", 2700000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7735), 83 },
                    { 47, "c208", 2700000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7736), 83 },
                    { 48, "caravan", 2700000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7745), 83 },
                    { 49, "grand caravan", 2900000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7746), 82 },
                    { 50, "citation longitude", 28000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7748), 84 },
                    { 51, "citation cj4", 11500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7749), 85 },
                    { 52, "cj4", 11500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7751), 85 },
                    { 53, "citation cj3", 9500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7752), 85 },
                    { 54, "cj3", 9500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7754), 85 },
                    { 55, "citation", 10000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7755), 86 },
                    { 56, "c400", 750000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7757), 87 },
                    { 57, "corvalis", 750000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7758), 87 },
                    { 58, "columbia", 750000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7760), 87 },
                    { 59, "cessna", 450000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7761), 88 },
                    { 60, "textron", 500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7763), 88 },
                    { 61, "piper cub", 180000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7764), 101 },
                    { 62, "pa-18", 200000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7766), 101 },
                    { 63, "j-3", 120000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7767), 101 },
                    { 64, "super cub", 220000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7768), 101 },
                    { 65, "cherokee", 320000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7770), 102 },
                    { 66, "pa-28", 320000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7771), 102 },
                    { 67, "archer", 380000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7773), 102 },
                    { 68, "warrior", 320000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7782), 102 },
                    { 69, "seminole", 680000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7783), 103 },
                    { 70, "pa-44", 680000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7785), 103 },
                    { 71, "m350", 1200000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7786), 100 },
                    { 72, "m500", 3000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7788), 100 },
                    { 73, "m600", 3500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7789), 100 },
                    { 74, "piper", 380000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7791), 104 },
                    { 75, "da62", 1400000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7792), 121 },
                    { 76, "da42", 950000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7794), 122 },
                    { 77, "twin star", 950000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7795), 122 },
                    { 78, "da40", 550000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7797), 123 },
                    { 79, "diamond star", 550000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7798), 123 },
                    { 80, "da20", 220000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7800), 124 },
                    { 81, "katana", 220000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7801), 124 },
                    { 82, "diamond", 500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7802), 125 },
                    { 83, "sr22t", 950000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7804), 140 },
                    { 84, "sr22", 850000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7805), 141 },
                    { 85, "sr20", 650000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7807), 142 },
                    { 86, "vision jet", 3200000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7808), 139 },
                    { 87, "sf50", 3200000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7818), 139 },
                    { 88, "cirrus", 800000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7819), 143 },
                    { 89, "king air 350i", 7400000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7821), 150 },
                    { 90, "king air 350", 6400000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7822), 151 },
                    { 91, "king air 260", 4800000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7824), 151 },
                    { 92, "king air", 5500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7825), 152 },
                    { 93, "kingair", 5500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7827), 152 },
                    { 94, "baron g58", 1500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7828), 153 },
                    { 95, "baron", 1300000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7830), 153 },
                    { 96, "g58", 1500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7831), 153 },
                    { 97, "bonanza g36", 1100000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7832), 154 },
                    { 98, "bonanza", 950000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7834), 154 },
                    { 99, "g36", 1100000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7835), 154 },
                    { 100, "beechcraft", 700000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7837), 155 },
                    { 101, "global 7500", 75000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7838), 171 },
                    { 102, "global7500", 75000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7840), 171 },
                    { 103, "global 6500", 58000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7841), 171 },
                    { 104, "bombardier global", 65000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7843), 172 },
                    { 105, "gulfstream g700", 78000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7844), 170 },
                    { 106, "gulfstream g650", 65000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7846), 171 },
                    { 107, "gulfstream g280", 24500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7855), 173 },
                    { 108, "gulfstream", 45000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7856), 173 },
                    { 109, "challenger 650", 32000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7858), 174 },
                    { 110, "challenger 350", 27000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7859), 174 },
                    { 111, "challenger", 28000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7861), 174 },
                    { 112, "learjet 75", 9500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7862), 175 },
                    { 113, "learjet", 9500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7864), 175 },
                    { 114, "lear", 9500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7865), 175 },
                    { 115, "phenom 300", 10500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7867), 176 },
                    { 116, "phenom", 9000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7869), 177 },
                    { 117, "praetor 600", 21000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7870), 175 },
                    { 118, "praetor 500", 17500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7871), 175 },
                    { 119, "praetor", 18000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7873), 176 },
                    { 120, "falcon 8x", 58000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7874), 172 },
                    { 121, "falcon 900", 45000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7876), 173 },
                    { 122, "falcon 2000", 35000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7877), 174 },
                    { 123, "falcon", 40000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7879), 175 },
                    { 124, "hawker", 8000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7880), 178 },
                    { 125, "hondajet", 5800000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7882), 177 },
                    { 126, "pc-12 ngx", 6200000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7883), 190 },
                    { 127, "pc-12", 6000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7891), 191 },
                    { 128, "pilatus pc12", 6000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7893), 191 },
                    { 129, "pc-6", 2500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7894), 192 },
                    { 130, "porter", 2500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7896), 192 },
                    { 131, "pc-24", 11000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7897), 189 },
                    { 132, "pilatus", 5000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7899), 193 },
                    { 133, "tbm 960", 4800000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7900), 194 },
                    { 134, "tbm960", 4800000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7901), 194 },
                    { 135, "tbm 940", 4200000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7903), 195 },
                    { 136, "tbm940", 4200000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7904), 195 },
                    { 137, "tbm 930", 3900000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7906), 196 },
                    { 138, "tbm930", 3900000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7907), 196 },
                    { 139, "tbm", 4200000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7909), 197 },
                    { 140, "kodiak 100", 2900000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7910), 197 },
                    { 141, "kodiak", 2700000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7912), 198 },
                    { 142, "bell 412", 8500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7913), 211 },
                    { 143, "bell412", 8500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7915), 211 },
                    { 144, "bell 429", 7500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7916), 211 },
                    { 145, "bell 407", 4200000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7918), 212 },
                    { 146, "bell407", 4200000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7919), 212 },
                    { 147, "bell 206", 1500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7927), 213 },
                    { 148, "jetranger", 1500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7929), 213 },
                    { 149, "bell 505", 1600000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7930), 213 },
                    { 150, "bell", 3000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7932), 215 },
                    { 151, "h225", 32000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7933), 231 },
                    { 152, "super puma", 32000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7934), 231 },
                    { 153, "h175", 18000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7936), 231 },
                    { 154, "h160", 14500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7937), 232 },
                    { 155, "h145", 12000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7939), 233 },
                    { 156, "ec145", 12000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7940), 233 },
                    { 157, "h135", 5500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7942), 234 },
                    { 158, "ec135", 5500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7943), 234 },
                    { 159, "h130", 3200000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7945), 234 },
                    { 160, "h125", 3800000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7946), 235 },
                    { 161, "as350", 3800000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7948), 235 },
                    { 162, "ecureuil", 3800000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7949), 235 },
                    { 163, "robinson r66", 1050000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7950), 251 },
                    { 164, "r66", 1050000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7984), 251 },
                    { 165, "robinson r44", 550000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7986), 252 },
                    { 166, "r44", 550000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7988), 252 },
                    { 167, "robinson r22", 350000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7998), 253 },
                    { 168, "r22", 350000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(7999), 253 },
                    { 169, "robinson", 600000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8001), 254 },
                    { 170, "cabri g2", 420000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8002), 261 },
                    { 171, "cabri", 420000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8004), 261 },
                    { 172, "schweizer 300", 350000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8005), 262 },
                    { 173, "schweizer", 350000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8007), 262 },
                    { 174, "s300", 350000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8008), 262 },
                    { 175, "300c", 350000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8010), 262 },
                    { 176, "md 500", 2800000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8011), 263 },
                    { 177, "md500", 2800000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8013), 263 },
                    { 178, "md 530", 3200000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8014), 263 },
                    { 179, "md530", 3200000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8015), 263 },
                    { 180, "leonardo aw", 15000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8017), 264 },
                    { 181, "aw139", 15000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8018), 264 },
                    { 182, "aw109", 6500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8020), 265 },
                    { 183, "sikorsky s-76", 13000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8021), 264 },
                    { 184, "sikorsky", 10000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8023), 266 },
                    { 185, "c-17", 220000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8024), 271 },
                    { 186, "globemaster", 220000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8032), 271 },
                    { 187, "a400", 120000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8034), 272 },
                    { 188, "atlas", 120000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8035), 272 },
                    { 189, "c-130", 35000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8037), 273 },
                    { 190, "hercules", 35000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8038), 273 },
                    { 191, "c-5", 180000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8040), 271 },
                    { 192, "galaxy", 180000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8041), 271 },
                    { 193, "dc-6", 1200000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8043), 291 },
                    { 194, "dc-3", 350000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8044), 292 },
                    { 195, "dakota", 350000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8046), 292 },
                    { 196, "twin otter", 6500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8047), 292 },
                    { 197, "dhc-6", 6500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8049), 292 },
                    { 198, "otter", 1500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8050), 293 },
                    { 199, "dhc-3", 1500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8051), 293 },
                    { 200, "beaver", 750000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8053), 294 },
                    { 201, "dhc-2", 750000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8054), 294 },
                    { 202, "an-2", 280000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8056), 295 },
                    { 203, "antonov", 280000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8057), 295 },
                    { 204, "jet", 12000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8059), 901 },
                    { 205, "helicopter", 2500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8060), 902 },
                    { 206, "heli", 2500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8062), 902 },
                    { 207, "turboprop", 3500000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8063), 903 },
                    { 208, "airliner", 85000000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8064), 900 },
                    { 209, "twin", 1200000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8066), 904 },
                    { 210, "single", 450000m, new DateTime(2025, 12, 29, 12, 33, 34, 826, DateTimeKind.Local).AddTicks(8067), 905 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AircraftCatalog_Title",
                table: "AircraftCatalog",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_PilotId",
                table: "Licenses",
                column: "PilotId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceChecks_AircraftId",
                table: "MaintenanceChecks",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_MsfsAircraft_Title",
                table: "MsfsAircraft",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeRatings_PilotId",
                table: "TypeRatings",
                column: "PilotId");
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AircraftCatalog");

            migrationBuilder.DropTable(
                name: "AircraftPricings");

            migrationBuilder.DropTable(
                name: "FBOs");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "MaintenanceChecks");

            migrationBuilder.DropTable(
                name: "MsfsAircraft");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "TypeRatings");

            migrationBuilder.DropTable(
                name: "Aircraft");

            migrationBuilder.DropTable(
                name: "Pilots");
        }
    }
}
