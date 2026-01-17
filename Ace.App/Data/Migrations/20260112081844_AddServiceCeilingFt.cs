using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ace.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceCeilingFt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AircraftPricings");

            migrationBuilder.RenameColumn(
                name: "RatePerPaxPerNM",
                table: "Settings",
                newName: "TypeRatingCostVeryLarge");

            migrationBuilder.RenameColumn(
                name: "PilotTrainingCost",
                table: "Settings",
                newName: "TypeRatingCostSmall");

            migrationBuilder.RenameColumn(
                name: "MaintenanceIntervalHours",
                table: "Settings",
                newName: "TypeRatingCostMediumLarge");

            migrationBuilder.RenameColumn(
                name: "MaintenanceCostPerHour",
                table: "Settings",
                newName: "TypeRatingCostMedium");

            migrationBuilder.AddColumn<decimal>(
                name: "AchievementRewardMultiplier",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "AllowAllAircraftForFlightPlan",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastSelectedAircraftRegistration",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MapStyle",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "OldtimerROIMalusPercent",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ROIPercentLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ROIPercentMedium",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ROIPercentMediumLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ROIPercentSmall",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ROIPercentVeryLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RouteSlotLimitInternational",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RouteSlotLimitLocal",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RouteSlotLimitRegional",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoutesPerFBOPairLimit",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ScheduledRouteBonusPercent",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TypeRatingCostLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "TotalDistanceNM",
                table: "Pilots",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DistanceNM",
                table: "DailyEarningsDetails",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsOldtimer",
                table: "AircraftCatalog",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ServiceCeilingFt",
                table: "AircraftCatalog",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsOldtimer",
                table: "Aircraft",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ServiceCeilingFt",
                table: "Aircraft",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    Tier = table.Column<int>(type: "INTEGER", nullable: false),
                    Icon = table.Column<string>(type: "TEXT", nullable: false),
                    IsUnlocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    UnlockedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Progress = table.Column<int>(type: "INTEGER", nullable: false),
                    Target = table.Column<int>(type: "INTEGER", nullable: false),
                    Reward = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OriginFBOId = table.Column<int>(type: "INTEGER", nullable: false),
                    DestinationFBOId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignedAircraftId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledRoutes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_Key",
                table: "Achievements",
                column: "Key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Achievements");

            migrationBuilder.DropTable(
                name: "ScheduledRoutes");

            migrationBuilder.DropColumn(
                name: "AchievementRewardMultiplier",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "AllowAllAircraftForFlightPlan",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "LastSelectedAircraftRegistration",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MapStyle",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "OldtimerROIMalusPercent",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ROIPercentLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ROIPercentMedium",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ROIPercentMediumLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ROIPercentSmall",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ROIPercentVeryLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "RouteSlotLimitInternational",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "RouteSlotLimitLocal",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "RouteSlotLimitRegional",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "RoutesPerFBOPairLimit",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ScheduledRouteBonusPercent",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "TypeRatingCostLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "TotalDistanceNM",
                table: "Pilots");

            migrationBuilder.DropColumn(
                name: "DistanceNM",
                table: "DailyEarningsDetails");

            migrationBuilder.DropColumn(
                name: "IsOldtimer",
                table: "AircraftCatalog");

            migrationBuilder.DropColumn(
                name: "ServiceCeilingFt",
                table: "AircraftCatalog");

            migrationBuilder.DropColumn(
                name: "IsOldtimer",
                table: "Aircraft");

            migrationBuilder.DropColumn(
                name: "ServiceCeilingFt",
                table: "Aircraft");

            migrationBuilder.RenameColumn(
                name: "TypeRatingCostVeryLarge",
                table: "Settings",
                newName: "RatePerPaxPerNM");

            migrationBuilder.RenameColumn(
                name: "TypeRatingCostSmall",
                table: "Settings",
                newName: "PilotTrainingCost");

            migrationBuilder.RenameColumn(
                name: "TypeRatingCostMediumLarge",
                table: "Settings",
                newName: "MaintenanceIntervalHours");

            migrationBuilder.RenameColumn(
                name: "TypeRatingCostMedium",
                table: "Settings",
                newName: "MaintenanceCostPerHour");

            migrationBuilder.CreateTable(
                name: "AircraftPricings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AircraftNamePattern = table.Column<string>(type: "TEXT", nullable: false),
                    BasePrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    CruiseSpeedKts = table.Column<double>(type: "REAL", nullable: false),
                    FuelBurnGph = table.Column<double>(type: "REAL", nullable: false),
                    FuelCapacityGal = table.Column<double>(type: "REAL", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Passengers = table.Column<int>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    RangeNM = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftPricings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AircraftPricings",
                columns: new[] { "Id", "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "LastUpdated", "Passengers", "Priority", "RangeNM" },
                values: new object[,]
                {
                    { 1, "747", 418000000m, 488.0, 3000.0, 64225.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 467, 1, 8000.0 },
                    { 2, "a380", 445000000m, 500.0, 3200.0, 84600.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 555, 1, 8000.0 },
                    { 3, "787", 248000000m, 488.0, 1500.0, 33340.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 290, 2, 8300.0 },
                    { 4, "dreamliner", 248000000m, 488.0, 1500.0, 33340.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 290, 2, 8300.0 },
                    { 5, "a350", 317000000m, 488.0, 1900.0, 35000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 315, 2, 8100.0 },
                    { 6, "777", 375000000m, 510.0, 2300.0, 47890.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 386, 3, 7730.0 },
                    { 7, "a330", 264000000m, 470.0, 1700.0, 26000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 277, 3, 6350.0 },
                    { 8, "a340", 238000000m, 470.0, 2100.0, 36700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 295, 4, 7900.0 },
                    { 9, "737 max", 122000000m, 453.0, 550.0, 6853.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 189, 20, 3550.0 },
                    { 10, "737", 100000000m, 453.0, 600.0, 6875.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 162, 21, 3115.0 },
                    { 11, "b737", 100000000m, 453.0, 600.0, 6875.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 162, 21, 3115.0 },
                    { 12, "a320neo", 111000000m, 450.0, 580.0, 6300.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 180, 20, 3500.0 },
                    { 13, "a320", 101000000m, 447.0, 600.0, 6270.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 180, 21, 3400.0 },
                    { 14, "a321", 130000000m, 447.0, 650.0, 6875.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 200, 21, 3200.0 },
                    { 15, "a319", 92000000m, 470.0, 580.0, 6400.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 140, 21, 3700.0 },
                    { 16, "a220", 81000000m, 470.0, 520.0, 5790.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 130, 22, 3400.0 },
                    { 17, "c series", 81000000m, 470.0, 520.0, 5790.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 130, 22, 3400.0 },
                    { 18, "a310", 65000000m, 459.0, 1400.0, 15800.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 220, 23, 5150.0 },
                    { 19, "a300", 65000000m, 459.0, 1400.0, 15000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 250, 23, 4050.0 },
                    { 20, "757", 75000000m, 461.0, 900.0, 11466.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 200, 24, 3900.0 },
                    { 21, "767", 85000000m, 459.0, 1300.0, 24140.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 245, 24, 5990.0 },
                    { 22, "717", 42000000m, 438.0, 600.0, 3533.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 117, 25, 2060.0 },
                    { 23, "707", 2500000m, 525.0, 2000.0, 23855.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 189, 26, 6160.0 },
                    { 24, "md-11", 45000000m, 480.0, 2200.0, 35000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 298, 24, 7200.0 },
                    { 25, "md-10", 15000000m, 500.0, 2000.0, 28500.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 270, 24, 4000.0 },
                    { 26, "dc-10", 12000000m, 500.0, 2000.0, 28500.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 270, 24, 4000.0 },
                    { 27, "md80", 8000000m, 438.0, 700.0, 5840.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 155, 24, 2050.0 },
                    { 28, "md-80", 8000000m, 438.0, 700.0, 5840.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 155, 24, 2050.0 },
                    { 29, "dc-9", 6000000m, 430.0, 550.0, 4000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 115, 25, 1500.0 },
                    { 30, "crj", 33000000m, 475.0, 400.0, 2903.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 90, 41, 1828.0 },
                    { 31, "e170", 47000000m, 447.0, 380.0, 3700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 70, 42, 2150.0 },
                    { 32, "e175", 53000000m, 447.0, 400.0, 3700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 78, 42, 2200.0 },
                    { 33, "e190", 50000000m, 447.0, 420.0, 3700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 98, 42, 2450.0 },
                    { 34, "e195", 54000000m, 447.0, 450.0, 3700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 108, 42, 2300.0 },
                    { 35, "embraer e", 50000000m, 447.0, 420.0, 3700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 98, 43, 2450.0 },
                    { 36, "e-jet", 50000000m, 447.0, 420.0, 3700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 98, 43, 2450.0 },
                    { 37, "erj", 35000000m, 450.0, 340.0, 1800.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, 45, 2000.0 },
                    { 38, "atr 72", 27000000m, 276.0, 140.0, 1690.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 72, 61, 920.0 },
                    { 39, "atr72", 27000000m, 276.0, 140.0, 1690.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 72, 61, 920.0 },
                    { 40, "atr 42", 18000000m, 276.0, 110.0, 1200.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 48, 62, 820.0 },
                    { 41, "atr42", 18000000m, 276.0, 110.0, 1200.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 48, 62, 820.0 },
                    { 42, "atr", 22000000m, 276.0, 140.0, 1690.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 72, 63, 820.0 },
                    { 43, "dash 8", 32000000m, 360.0, 200.0, 1724.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 78, 64, 1360.0 },
                    { 44, "q400", 32000000m, 360.0, 200.0, 1724.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 90, 64, 1360.0 },
                    { 45, "dhc-8", 32000000m, 360.0, 200.0, 1724.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 78, 64, 1360.0 },
                    { 46, "dhc8", 32000000m, 360.0, 200.0, 1724.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 78, 64, 1360.0 },
                    { 47, "saab 340", 12000000m, 282.0, 120.0, 713.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 34, 65, 900.0 },
                    { 48, "saab340", 12000000m, 282.0, 120.0, 713.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 34, 65, 900.0 },
                    { 49, "s340", 12000000m, 282.0, 120.0, 713.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 34, 65, 900.0 },
                    { 50, "beech 1900", 4500000m, 270.0, 120.0, 665.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 66, 1247.0 },
                    { 51, "b190", 4500000m, 270.0, 120.0, 665.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 66, 1247.0 },
                    { 52, "1900", 4500000m, 270.0, 120.0, 665.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 66, 1247.0 },
                    { 53, "model 99", 2000000m, 250.0, 85.0, 360.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, 67, 1000.0 },
                    { 54, "cessna 172", 400000m, 122.0, 8.5, 56.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 81, 640.0 },
                    { 55, "c172", 400000m, 122.0, 8.5, 56.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 81, 640.0 },
                    { 56, "skyhawk", 400000m, 122.0, 8.5, 56.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 81, 640.0 },
                    { 57, "cessna 182", 550000m, 145.0, 14.0, 92.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 80, 930.0 },
                    { 58, "c182", 550000m, 145.0, 14.0, 92.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 80, 930.0 },
                    { 59, "skylane", 550000m, 145.0, 14.0, 92.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 80, 930.0 },
                    { 60, "cessna 152", 180000m, 107.0, 6.0999999999999996, 26.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 82, 415.0 },
                    { 61, "c152", 180000m, 107.0, 6.0999999999999996, 26.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 82, 415.0 },
                    { 62, "cessna 140", 80000m, 100.0, 6.0, 25.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 82, 420.0 },
                    { 63, "c140", 80000m, 100.0, 6.0, 25.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 82, 420.0 },
                    { 64, "cessna 170", 120000m, 120.0, 9.0, 42.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 82, 600.0 },
                    { 65, "c170", 120000m, 120.0, 9.0, 42.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 82, 600.0 },
                    { 66, "cessna 177", 150000m, 128.0, 10.0, 52.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 82, 700.0 },
                    { 67, "c177", 150000m, 128.0, 10.0, 52.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 82, 700.0 },
                    { 68, "cardinal", 150000m, 128.0, 10.0, 52.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 82, 700.0 },
                    { 69, "cessna 185", 350000m, 160.0, 15.0, 84.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 79, 700.0 },
                    { 70, "c185", 350000m, 160.0, 15.0, 84.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 79, 700.0 },
                    { 71, "185f", 350000m, 160.0, 15.0, 84.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 79, 700.0 },
                    { 72, "skywagon", 350000m, 160.0, 15.0, 84.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 79, 700.0 },
                    { 73, "cessna 206", 750000m, 160.0, 15.0, 87.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 79, 800.0 },
                    { 74, "c206", 750000m, 160.0, 15.0, 87.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 79, 800.0 },
                    { 75, "stationair", 750000m, 160.0, 15.0, 87.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 79, 800.0 },
                    { 76, "cessna 210", 450000m, 165.0, 14.0, 90.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 79, 900.0 },
                    { 77, "c210", 450000m, 165.0, 14.0, 90.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 79, 900.0 },
                    { 78, "centurion", 450000m, 165.0, 14.0, 90.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 79, 900.0 },
                    { 79, "c195", 180000m, 145.0, 14.0, 80.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 80, 700.0 },
                    { 80, "cessna 303", 350000m, 187.0, 24.0, 155.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 87, 1200.0 },
                    { 81, "c303", 350000m, 187.0, 24.0, 155.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 87, 1200.0 },
                    { 82, "cessna 310", 250000m, 195.0, 24.0, 140.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 87, 1000.0 },
                    { 83, "c310", 250000m, 195.0, 24.0, 140.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 87, 1000.0 },
                    { 84, "cessna 337", 220000m, 180.0, 24.0, 131.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 87, 1000.0 },
                    { 85, "c337", 220000m, 180.0, 24.0, 131.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 87, 1000.0 },
                    { 86, "skymaster", 220000m, 180.0, 24.0, 131.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 87, 1000.0 },
                    { 87, "cessna 340", 400000m, 230.0, 32.0, 163.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 87, 1500.0 },
                    { 88, "c340", 400000m, 230.0, 32.0, 163.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 87, 1500.0 },
                    { 89, "cessna 402", 600000m, 200.0, 46.0, 213.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 87, 1400.0 },
                    { 90, "c402", 600000m, 200.0, 46.0, 213.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 87, 1400.0 },
                    { 91, "cessna 404", 700000m, 190.0, 55.0, 213.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 87, 1000.0 },
                    { 92, "c404", 700000m, 190.0, 55.0, 213.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 87, 1000.0 },
                    { 93, "titan", 700000m, 190.0, 55.0, 213.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 87, 1000.0 },
                    { 94, "cessna 421", 800000m, 235.0, 44.0, 213.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 87, 1550.0 },
                    { 95, "c421", 800000m, 235.0, 44.0, 213.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 87, 1550.0 },
                    { 96, "c400", 750000m, 190.0, 18.0, 106.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 87, 1300.0 },
                    { 97, "corvalis", 750000m, 190.0, 18.0, 106.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 87, 1300.0 },
                    { 98, "columbia", 750000m, 190.0, 18.0, 106.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 87, 1300.0 },
                    { 99, "cessna 208", 2700000m, 186.0, 53.0, 335.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 83, 1070.0 },
                    { 100, "c208", 2700000m, 186.0, 53.0, 335.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 83, 1070.0 },
                    { 101, "caravan", 2700000m, 186.0, 53.0, 335.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 83, 1070.0 },
                    { 102, "grand caravan", 2900000m, 186.0, 55.0, 335.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, 82, 1000.0 },
                    { 103, "cessna 408", 5500000m, 200.0, 60.0, 200.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 83, 900.0 },
                    { 104, "c408", 5500000m, 200.0, 60.0, 200.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 83, 900.0 },
                    { 105, "skycourier", 5500000m, 200.0, 60.0, 200.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 83, 900.0 },
                    { 106, "cessna 441", 1200000m, 290.0, 65.0, 475.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 83, 1400.0 },
                    { 107, "c441", 1200000m, 290.0, 65.0, 475.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 83, 1400.0 },
                    { 108, "conquest", 1200000m, 290.0, 65.0, 475.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 83, 1400.0 },
                    { 109, "citation longitude", 28000000m, 483.0, 270.0, 2166.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 84, 3500.0 },
                    { 110, "longitude", 28000000m, 483.0, 270.0, 2166.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 84, 3500.0 },
                    { 111, "citation cj4", 11500000m, 451.0, 180.0, 870.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 85, 2165.0 },
                    { 112, "cj4", 11500000m, 451.0, 180.0, 870.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 85, 2165.0 },
                    { 113, "citation cj3", 9500000m, 416.0, 150.0, 700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 85, 2000.0 },
                    { 114, "cj3", 9500000m, 416.0, 150.0, 700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 85, 2000.0 },
                    { 115, "citation", 10000000m, 400.0, 180.0, 750.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 86, 1900.0 },
                    { 116, "cessna", 450000m, 130.0, 9.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 88, 600.0 },
                    { 117, "textron", 500000m, 130.0, 9.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 88, 600.0 },
                    { 118, "piper cub", 180000m, 87.0, 5.0, 18.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 101, 220.0 },
                    { 119, "pa-18", 200000m, 87.0, 5.0, 18.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 101, 220.0 },
                    { 120, "j-3", 120000m, 87.0, 5.0, 18.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 101, 220.0 },
                    { 121, "super cub", 220000m, 100.0, 8.0, 36.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 101, 400.0 },
                    { 122, "cherokee", 320000m, 124.0, 9.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 102, 640.0 },
                    { 123, "pa-28", 320000m, 124.0, 9.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 102, 640.0 },
                    { 124, "archer", 380000m, 135.0, 10.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 102, 520.0 },
                    { 125, "warrior", 320000m, 124.0, 9.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 102, 640.0 },
                    { 126, "seminole", 680000m, 160.0, 16.0, 102.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 103, 700.0 },
                    { 127, "pa-44", 680000m, 160.0, 16.0, 102.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 103, 700.0 },
                    { 128, "seneca", 700000m, 180.0, 22.0, 123.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 103, 1000.0 },
                    { 129, "pa34", 700000m, 180.0, 22.0, 123.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 103, 1000.0 },
                    { 130, "m350", 1200000m, 213.0, 17.0, 120.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 100, 1343.0 },
                    { 131, "m500", 3000000m, 260.0, 30.0, 170.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 100, 1000.0 },
                    { 132, "m600", 3500000m, 274.0, 40.0, 260.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 100, 1658.0 },
                    { 133, "piper", 380000m, 130.0, 9.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 104, 600.0 },
                    { 134, "da62", 1400000m, 192.0, 13.0, 79.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 121, 1100.0 },
                    { 135, "da42", 950000m, 180.0, 9.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 122, 1100.0 },
                    { 136, "twin star", 950000m, 180.0, 9.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 122, 1100.0 },
                    { 137, "da40", 550000m, 147.0, 8.0, 40.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 123, 720.0 },
                    { 138, "diamond star", 550000m, 147.0, 8.0, 40.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 123, 720.0 },
                    { 139, "da20", 220000m, 135.0, 6.0, 24.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 124, 600.0 },
                    { 140, "dv20", 220000m, 135.0, 6.0, 24.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 124, 600.0 },
                    { 141, "katana", 220000m, 135.0, 6.0, 24.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 124, 600.0 },
                    { 142, "diamond", 500000m, 147.0, 8.0, 40.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 125, 720.0 },
                    { 143, "sr22t", 950000m, 214.0, 18.0, 92.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 140, 1000.0 },
                    { 144, "sr22", 850000m, 183.0, 17.0, 92.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 141, 1050.0 },
                    { 145, "sr20", 720000m, 155.0, 10.5, 56.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 142, 810.0 },
                    { 146, "vision jet", 3200000m, 300.0, 50.0, 296.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 139, 1200.0 },
                    { 147, "sf50", 3200000m, 300.0, 50.0, 296.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 139, 1200.0 },
                    { 148, "cirrus", 800000m, 183.0, 15.0, 80.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 143, 1000.0 },
                    { 149, "king air 350i", 7400000m, 312.0, 100.0, 544.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 150, 1806.0 },
                    { 150, "king air 350", 6400000m, 312.0, 100.0, 544.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 151, 1806.0 },
                    { 151, "king air 260", 4800000m, 282.0, 100.0, 544.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 151, 1580.0 },
                    { 152, "king air", 5500000m, 282.0, 100.0, 544.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 152, 1580.0 },
                    { 153, "kingair", 5500000m, 282.0, 100.0, 544.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 152, 1580.0 },
                    { 154, "b200", 5500000m, 282.0, 100.0, 544.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 152, 1580.0 },
                    { 155, "c90", 1500000m, 217.0, 71.0, 384.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 152, 1120.0 },
                    { 156, "baron g58", 1500000m, 200.0, 26.0, 142.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 153, 1225.0 },
                    { 157, "baron", 1300000m, 200.0, 26.0, 142.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 153, 1225.0 },
                    { 158, "g58", 1500000m, 200.0, 26.0, 142.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 153, 1225.0 },
                    { 159, "bonanza g36", 1100000m, 176.0, 14.5, 74.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 154, 920.0 },
                    { 160, "bonanza", 950000m, 176.0, 14.5, 74.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 154, 920.0 },
                    { 161, "g36", 1100000m, 176.0, 14.5, 74.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 154, 920.0 },
                    { 162, "staggerwing", 350000m, 200.0, 22.0, 100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 155, 600.0 },
                    { 163, "d17", 350000m, 200.0, 22.0, 100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 155, 600.0 },
                    { 164, "d18", 400000m, 195.0, 48.0, 198.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 155, 900.0 },
                    { 165, "model 18", 400000m, 195.0, 48.0, 198.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 155, 900.0 },
                    { 166, "model 76", 450000m, 160.0, 16.0, 102.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 155, 700.0 },
                    { 167, "duchess", 450000m, 160.0, 16.0, 102.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 155, 700.0 },
                    { 168, "beechcraft", 700000m, 176.0, 14.5, 74.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 155, 920.0 },
                    { 169, "global 7500", 75000000m, 530.0, 450.0, 7687.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 171, 7700.0 },
                    { 170, "global7500", 75000000m, 530.0, 450.0, 7687.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 171, 7700.0 },
                    { 171, "global 6500", 58000000m, 530.0, 420.0, 6600.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, 171, 6600.0 },
                    { 172, "bombardier global", 65000000m, 530.0, 450.0, 7687.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 172, 7700.0 },
                    { 173, "gulfstream g700", 78000000m, 516.0, 430.0, 6590.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 170, 7500.0 },
                    { 174, "gulfstream g650", 65000000m, 516.0, 420.0, 6590.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 171, 7000.0 },
                    { 175, "gulfstream g280", 24500000m, 482.0, 200.0, 2550.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 173, 3600.0 },
                    { 176, "gulfstream", 45000000m, 516.0, 420.0, 6590.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 173, 7000.0 },
                    { 177, "challenger 650", 32000000m, 470.0, 227.0, 2700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 174, 4000.0 },
                    { 178, "challenger 350", 27000000m, 470.0, 210.0, 2500.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 174, 3200.0 },
                    { 179, "challenger", 28000000m, 470.0, 227.0, 2700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 174, 4000.0 },
                    { 180, "learjet 75", 9500000m, 465.0, 206.0, 906.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 175, 2050.0 },
                    { 181, "learjet", 9500000m, 465.0, 206.0, 906.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 175, 2050.0 },
                    { 182, "lear", 9500000m, 465.0, 206.0, 906.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 175, 2050.0 },
                    { 183, "phenom 300", 10500000m, 453.0, 158.0, 799.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 176, 1971.0 },
                    { 184, "phenom", 9000000m, 453.0, 158.0, 799.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 177, 1971.0 },
                    { 185, "praetor 600", 21000000m, 466.0, 210.0, 2392.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 175, 4018.0 },
                    { 186, "praetor 500", 17500000m, 466.0, 190.0, 2000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 175, 3340.0 },
                    { 187, "praetor", 18000000m, 466.0, 210.0, 2392.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 176, 4018.0 },
                    { 188, "legacy 500", 20000000m, 466.0, 200.0, 2100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 175, 3125.0 },
                    { 189, "legacy500", 20000000m, 466.0, 200.0, 2100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 175, 3125.0 },
                    { 190, "legacy 450", 18000000m, 466.0, 190.0, 1900.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 175, 2900.0 },
                    { 191, "legacy 600", 16000000m, 459.0, 220.0, 2300.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, 175, 3400.0 },
                    { 192, "legacy 650", 26000000m, 465.0, 250.0, 2800.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, 175, 3900.0 },
                    { 193, "legacy", 18000000m, 466.0, 200.0, 2100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 176, 3125.0 },
                    { 194, "falcon 8x", 58000000m, 488.0, 340.0, 4188.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, 172, 6450.0 },
                    { 195, "falcon 900", 45000000m, 480.0, 280.0, 3300.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 173, 4750.0 },
                    { 196, "falcon 2000", 35000000m, 470.0, 240.0, 2600.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 174, 4000.0 },
                    { 197, "falcon", 40000000m, 488.0, 340.0, 4188.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, 175, 6450.0 },
                    { 198, "hawker", 8000000m, 430.0, 180.0, 1000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 178, 2600.0 },
                    { 199, "hondajet", 5800000m, 420.0, 85.0, 350.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 177, 1437.0 },
                    { 200, "pc-12 ngx", 6200000m, 290.0, 77.0, 402.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 190, 1800.0 },
                    { 201, "pc-12", 6000000m, 290.0, 77.0, 402.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 191, 1800.0 },
                    { 202, "pc12", 6000000m, 290.0, 77.0, 402.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 191, 1800.0 },
                    { 203, "pilatus pc12", 6000000m, 290.0, 77.0, 402.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 191, 1800.0 },
                    { 204, "pc-6", 2500000m, 115.0, 39.0, 298.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 192, 500.0 },
                    { 205, "pc6", 2500000m, 115.0, 39.0, 298.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 192, 500.0 },
                    { 206, "porter", 2500000m, 115.0, 39.0, 298.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 192, 500.0 },
                    { 207, "pc-24", 11000000m, 440.0, 180.0, 893.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 189, 2000.0 },
                    { 208, "pc24", 11000000m, 440.0, 180.0, 893.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 189, 2000.0 },
                    { 209, "pilatus", 5000000m, 290.0, 77.0, 402.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 193, 1800.0 },
                    { 210, "tbm 960", 4800000m, 330.0, 57.0, 292.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 194, 1730.0 },
                    { 211, "tbm960", 4800000m, 330.0, 57.0, 292.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 194, 1730.0 },
                    { 212, "tbm 940", 4200000m, 330.0, 57.0, 292.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 195, 1730.0 },
                    { 213, "tbm940", 4200000m, 330.0, 57.0, 292.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 195, 1730.0 },
                    { 214, "tbm 930", 3900000m, 330.0, 57.0, 292.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 196, 1730.0 },
                    { 215, "tbm930", 3900000m, 330.0, 57.0, 292.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 196, 1730.0 },
                    { 216, "tbm", 4200000m, 330.0, 57.0, 292.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 197, 1730.0 },
                    { 217, "kodiak 100", 2900000m, 174.0, 48.0, 315.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 197, 1132.0 },
                    { 218, "kodiak", 2700000m, 174.0, 48.0, 315.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 198, 1132.0 },
                    { 219, "quest", 2700000m, 174.0, 48.0, 315.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 198, 1132.0 },
                    { 220, "mu-2", 1200000m, 300.0, 65.0, 380.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 198, 1300.0 },
                    { 221, "mu2", 1200000m, 300.0, 65.0, 380.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 198, 1300.0 },
                    { 222, "bell 412", 8500000m, 140.0, 90.0, 336.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, 211, 400.0 },
                    { 223, "bell412", 8500000m, 140.0, 90.0, 336.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, 211, 400.0 },
                    { 224, "b412", 8500000m, 140.0, 90.0, 336.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, 211, 400.0 },
                    { 225, "bell 429", 7500000m, 155.0, 70.0, 225.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 211, 400.0 },
                    { 226, "b429", 7500000m, 155.0, 70.0, 225.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 211, 400.0 },
                    { 227, "bell 427", 4000000m, 140.0, 60.0, 200.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 211, 350.0 },
                    { 228, "b427", 4000000m, 140.0, 60.0, 200.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 211, 350.0 },
                    { 229, "bell 407", 4200000m, 133.0, 42.0, 145.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 212, 324.0 },
                    { 230, "bell407", 4200000m, 133.0, 42.0, 145.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 212, 324.0 },
                    { 231, "model 407", 4200000m, 133.0, 42.0, 145.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 212, 324.0 },
                    { 232, "bell 206", 1500000m, 120.0, 26.0, 91.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 213, 260.0 },
                    { 233, "jetranger", 1500000m, 120.0, 26.0, 91.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 213, 260.0 },
                    { 234, "bell 505", 1600000m, 125.0, 25.0, 75.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 213, 300.0 },
                    { 235, "b505", 1600000m, 125.0, 25.0, 75.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 213, 300.0 },
                    { 236, "bell 47", 300000m, 84.0, 12.0, 30.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 214, 200.0 },
                    { 237, "bell 204", 2500000m, 110.0, 60.0, 220.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, 214, 265.0 },
                    { 238, "b204", 2500000m, 110.0, 60.0, 220.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, 214, 265.0 },
                    { 239, "bell 230", 3000000m, 140.0, 55.0, 200.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 214, 350.0 },
                    { 240, "b230", 3000000m, 140.0, 55.0, 200.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 214, 350.0 },
                    { 241, "bell", 3000000m, 130.0, 40.0, 150.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 215, 300.0 },
                    { 242, "h225", 32000000m, 150.0, 120.0, 700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 231, 480.0 },
                    { 243, "super puma", 32000000m, 150.0, 120.0, 700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 231, 480.0 },
                    { 244, "h175", 18000000m, 152.0, 100.0, 470.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, 231, 380.0 },
                    { 245, "h160", 14500000m, 160.0, 85.0, 320.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 232, 450.0 },
                    { 246, "h145", 12000000m, 129.0, 58.0, 239.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 233, 352.0 },
                    { 247, "ec145", 12000000m, 129.0, 58.0, 239.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 233, 352.0 },
                    { 248, "h135", 5500000m, 137.0, 52.0, 175.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 234, 395.0 },
                    { 249, "ec135", 5500000m, 137.0, 52.0, 175.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 234, 395.0 },
                    { 250, "ec-135", 5500000m, 137.0, 52.0, 175.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 234, 395.0 },
                    { 251, "h130", 3200000m, 130.0, 45.0, 145.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 234, 330.0 },
                    { 252, "h125", 3800000m, 137.0, 45.0, 143.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 235, 340.0 },
                    { 253, "as350", 3800000m, 137.0, 45.0, 143.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 235, 340.0 },
                    { 254, "ecureuil", 3800000m, 137.0, 45.0, 143.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 235, 340.0 },
                    { 255, "robinson r66", 1050000m, 110.0, 17.0, 73.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 251, 350.0 },
                    { 256, "r66", 1050000m, 110.0, 17.0, 73.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 251, 350.0 },
                    { 257, "robinson r44", 550000m, 117.0, 16.0, 49.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 252, 245.0 },
                    { 258, "r44", 550000m, 117.0, 16.0, 49.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 252, 245.0 },
                    { 259, "robinson r22", 350000m, 96.0, 8.0, 27.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 253, 200.0 },
                    { 260, "r22", 350000m, 96.0, 8.0, 27.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 253, 200.0 },
                    { 261, "robinson", 600000m, 110.0, 14.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 254, 250.0 },
                    { 262, "cabri g2", 420000m, 100.0, 9.0, 41.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 261, 370.0 },
                    { 263, "cabri", 420000m, 100.0, 9.0, 41.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 261, 370.0 },
                    { 264, "schweizer 300", 350000m, 95.0, 10.0, 35.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 262, 250.0 },
                    { 265, "schweizer", 350000m, 95.0, 10.0, 35.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 262, 250.0 },
                    { 266, "s300", 350000m, 95.0, 10.0, 35.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 262, 250.0 },
                    { 267, "md 500", 2800000m, 135.0, 30.0, 60.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 263, 250.0 },
                    { 268, "md500", 2800000m, 135.0, 30.0, 60.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 263, 250.0 },
                    { 269, "md 530", 3200000m, 135.0, 32.0, 60.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 263, 250.0 },
                    { 270, "md530", 3200000m, 135.0, 32.0, 60.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 263, 250.0 },
                    { 271, "leonardo aw", 15000000m, 151.0, 95.0, 445.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, 264, 448.0 },
                    { 272, "aw139", 15000000m, 151.0, 95.0, 445.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, 264, 448.0 },
                    { 273, "aw109", 6500000m, 155.0, 60.0, 200.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 265, 500.0 },
                    { 274, "sikorsky s-76", 13000000m, 155.0, 80.0, 270.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 264, 400.0 },
                    { 275, "sikorsky", 10000000m, 140.0, 75.0, 250.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 266, 350.0 },
                    { 276, "chinook", 40000000m, 145.0, 250.0, 1030.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 33, 267, 400.0 },
                    { 277, "ch-47", 40000000m, 145.0, 250.0, 1030.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 33, 267, 400.0 },
                    { 278, "c-17", 220000000m, 450.0, 2500.0, 35546.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 271, 4400.0 },
                    { 279, "globemaster", 220000000m, 450.0, 2500.0, 35546.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 271, 4400.0 },
                    { 280, "a400", 120000000m, 400.0, 1200.0, 15000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 272, 4500.0 },
                    { 281, "a400m", 120000000m, 400.0, 1200.0, 15000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 272, 4500.0 },
                    { 282, "atlas", 120000000m, 400.0, 1200.0, 15000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 272, 4500.0 },
                    { 283, "c-130", 35000000m, 320.0, 800.0, 6700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 273, 2360.0 },
                    { 284, "hercules", 35000000m, 320.0, 800.0, 6700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 273, 2360.0 },
                    { 285, "c-5", 180000000m, 450.0, 3500.0, 51150.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 271, 4440.0 },
                    { 286, "galaxy", 180000000m, 450.0, 3500.0, 51150.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 271, 4440.0 },
                    { 287, "c-46", 800000m, 175.0, 100.0, 700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, 274, 1200.0 },
                    { 288, "commando", 800000m, 175.0, 100.0, 700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, 274, 1200.0 },
                    { 289, "f-16", 35000000m, 530.0, 320.0, 845.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 280, 2280.0 },
                    { 290, "f16", 35000000m, 530.0, 320.0, 845.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 280, 2280.0 },
                    { 291, "viper", 35000000m, 530.0, 320.0, 845.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 281, 2280.0 },
                    { 292, "f-18", 70000000m, 500.0, 350.0, 1100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 280, 1800.0 },
                    { 293, "f18", 70000000m, 500.0, 350.0, 1100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 280, 1800.0 },
                    { 294, "fa18", 70000000m, 500.0, 350.0, 1100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 280, 1800.0 },
                    { 295, "fa-18", 70000000m, 500.0, 350.0, 1100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 280, 1800.0 },
                    { 296, "hornet", 70000000m, 500.0, 350.0, 1100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 281, 1800.0 },
                    { 297, "f-14", 50000000m, 520.0, 360.0, 1200.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 280, 1600.0 },
                    { 298, "f14", 50000000m, 520.0, 360.0, 1200.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 280, 1600.0 },
                    { 299, "tomcat", 50000000m, 520.0, 360.0, 1200.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 281, 1600.0 },
                    { 300, "a-10", 18000000m, 340.0, 200.0, 1400.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 282, 800.0 },
                    { 301, "a10", 18000000m, 340.0, 200.0, 1400.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 282, 800.0 },
                    { 302, "thunderbolt", 18000000m, 340.0, 200.0, 1400.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 283, 800.0 },
                    { 303, "warthog", 18000000m, 340.0, 200.0, 1400.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 283, 800.0 },
                    { 304, "t-38", 3500000m, 550.0, 175.0, 580.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 284, 1000.0 },
                    { 305, "t38", 3500000m, 550.0, 175.0, 580.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 284, 1000.0 },
                    { 306, "talon", 3500000m, 550.0, 175.0, 580.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 285, 1000.0 },
                    { 307, "l39", 650000m, 430.0, 100.0, 280.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 286, 650.0 },
                    { 308, "albatros", 650000m, 430.0, 100.0, 280.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 286, 650.0 },
                    { 309, "p51", 2500000m, 380.0, 70.0, 269.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 287, 1000.0 },
                    { 310, "p-51", 2500000m, 380.0, 70.0, 269.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 287, 1000.0 },
                    { 311, "mustang", 2500000m, 380.0, 70.0, 269.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 287, 1000.0 },
                    { 312, "dc-6", 1200000m, 280.0, 250.0, 5500.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 68, 291, 3000.0 },
                    { 313, "dc-3", 350000m, 180.0, 100.0, 822.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 28, 292, 1370.0 },
                    { 314, "c-47", 350000m, 180.0, 100.0, 822.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 28, 292, 1370.0 },
                    { 315, "dakota", 350000m, 180.0, 100.0, 822.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 28, 292, 1370.0 },
                    { 316, "ju52", 600000m, 140.0, 65.0, 290.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, 293, 600.0 },
                    { 317, "ju 52", 600000m, 140.0, 65.0, 290.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, 293, 600.0 },
                    { 318, "junkers", 500000m, 140.0, 65.0, 290.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, 294, 600.0 },
                    { 319, "junkers f13", 400000m, 95.0, 12.0, 65.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 293, 420.0 },
                    { 320, "trimotor", 400000m, 115.0, 45.0, 235.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 295, 500.0 },
                    { 321, "4-at", 400000m, 115.0, 45.0, 235.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 295, 500.0 },
                    { 322, "fw 200", 1500000m, 220.0, 200.0, 2000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 26, 295, 2000.0 },
                    { 323, "fw200", 1500000m, 220.0, 200.0, 2000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 26, 295, 2000.0 },
                    { 324, "condor", 1500000m, 220.0, 200.0, 2000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 26, 296, 2000.0 },
                    { 325, "focke-wulf", 1000000m, 200.0, 150.0, 1500.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 297, 1500.0 },
                    { 326, "fokker fvii", 300000m, 115.0, 45.0, 240.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 295, 500.0 },
                    { 327, "fokker f.vii", 300000m, 115.0, 45.0, 240.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 295, 500.0 },
                    { 328, "fokker", 5000000m, 300.0, 200.0, 2000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, 297, 1500.0 },
                    { 329, "stratoliner", 1500000m, 220.0, 200.0, 1700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 33, 296, 1750.0 },
                    { 330, "b307", 1500000m, 220.0, 200.0, 1700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 33, 296, 1750.0 },
                    { 331, "twin otter", 6500000m, 175.0, 70.0, 382.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 292, 527.0 },
                    { 332, "dhc-6", 6500000m, 175.0, 70.0, 382.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 292, 527.0 },
                    { 333, "otter", 1500000m, 121.0, 40.0, 214.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 293, 760.0 },
                    { 334, "dhc-3", 1500000m, 121.0, 40.0, 214.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 293, 760.0 },
                    { 335, "beaver", 750000m, 113.0, 25.0, 138.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 294, 395.0 },
                    { 336, "dhc-2", 750000m, 113.0, 25.0, 138.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 294, 395.0 },
                    { 337, "dhc-4", 1800000m, 158.0, 95.0, 476.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, 293, 1100.0 },
                    { 338, "caribou", 1800000m, 158.0, 95.0, 476.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, 293, 1100.0 },
                    { 339, "an-2", 280000m, 100.0, 60.0, 300.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 295, 525.0 },
                    { 340, "antonov", 280000m, 100.0, 60.0, 300.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 295, 525.0 },
                    { 341, "goose", 600000m, 175.0, 50.0, 220.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 295, 640.0 },
                    { 342, "g-21", 600000m, 175.0, 50.0, 220.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 295, 640.0 },
                    { 343, "albatross", 1500000m, 225.0, 150.0, 700.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 295, 2700.0 },
                    { 344, "skyvan", 1500000m, 173.0, 55.0, 250.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 296, 420.0 },
                    { 345, "sc.7", 1500000m, 173.0, 55.0, 250.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 296, 420.0 },
                    { 346, "shorts", 1200000m, 173.0, 55.0, 250.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 297, 420.0 },
                    { 347, "extra 300", 450000m, 185.0, 12.0, 28.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 300, 550.0 },
                    { 348, "extra", 400000m, 185.0, 12.0, 28.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 301, 550.0 },
                    { 349, "edge 540", 500000m, 220.0, 18.0, 42.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 300, 400.0 },
                    { 350, "zivko", 500000m, 220.0, 18.0, 42.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 301, 400.0 },
                    { 351, "mxs-r", 450000m, 250.0, 20.0, 42.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 300, 400.0 },
                    { 352, "mxs", 400000m, 250.0, 20.0, 42.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 301, 400.0 },
                    { 353, "pitts", 250000m, 154.0, 11.0, 29.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 302, 300.0 },
                    { 354, "cap10", 220000m, 155.0, 10.0, 30.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 302, 600.0 },
                    { 355, "cap 10", 220000m, 155.0, 10.0, 30.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 302, 600.0 },
                    { 356, "gee bee", 500000m, 230.0, 30.0, 60.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 303, 350.0 },
                    { 357, "geebee", 500000m, 230.0, 30.0, 60.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 303, 350.0 },
                    { 358, "model r2", 500000m, 230.0, 30.0, 60.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 303, 350.0 },
                    { 359, "model z", 450000m, 220.0, 25.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 303, 300.0 },
                    { 360, "e330", 450000m, 200.0, 15.0, 40.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 304, 400.0 },
                    { 361, "icon a5", 400000m, 109.0, 6.0, 20.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 310, 427.0 },
                    { 362, "icon", 400000m, 109.0, 6.0, 20.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 311, 427.0 },
                    { 363, "xcub", 380000m, 142.0, 10.0, 52.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 310, 800.0 },
                    { 364, "cubcrafters", 380000m, 142.0, 10.0, 52.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 311, 800.0 },
                    { 365, "nx cub", 380000m, 142.0, 10.0, 52.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 310, 800.0 },
                    { 366, "nxcub", 380000m, 142.0, 10.0, 52.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 310, 800.0 },
                    { 367, "shock ultra", 200000m, 130.0, 7.0, 33.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 312, 540.0 },
                    { 368, "wilga", 180000m, 120.0, 12.0, 60.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 312, 620.0 },
                    { 369, "savage", 150000m, 87.0, 5.0, 18.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 313, 220.0 },
                    { 370, "husky", 280000m, 120.0, 10.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 312, 600.0 },
                    { 371, "aviat", 280000m, 120.0, 10.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 313, 600.0 },
                    { 372, "rv-", 180000m, 185.0, 8.5, 42.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 314, 750.0 },
                    { 373, "mooney", 650000m, 175.0, 11.0, 64.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 315, 1050.0 },
                    { 374, "optica", 200000m, 108.0, 9.0, 55.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 316, 650.0 },
                    { 375, "p2002", 180000m, 117.0, 5.0, 27.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 320, 700.0 },
                    { 376, "p2004", 200000m, 115.0, 6.0, 35.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 320, 650.0 },
                    { 377, "p2006", 420000m, 137.0, 8.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 320, 640.0 },
                    { 378, "p2008", 350000m, 130.0, 7.0, 40.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 320, 600.0 },
                    { 379, "p2010", 380000m, 140.0, 10.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 320, 750.0 },
                    { 380, "p2012", 3500000m, 175.0, 35.0, 150.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 320, 900.0 },
                    { 381, "p92", 130000m, 110.0, 5.0, 25.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 320, 500.0 },
                    { 382, "tecnam", 250000m, 130.0, 8.0, 40.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 321, 700.0 },
                    { 383, "dr400", 180000m, 130.0, 10.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 322, 620.0 },
                    { 384, "dr 400", 180000m, 130.0, 10.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 322, 620.0 },
                    { 385, "robin", 180000m, 130.0, 10.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 323, 620.0 },
                    { 386, "ctsl", 200000m, 130.0, 5.0, 32.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 324, 600.0 },
                    { 387, "flight design", 180000m, 125.0, 5.0, 30.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 325, 550.0 },
                    { 388, "aa1", 45000m, 112.0, 6.0, 22.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 330, 500.0 },
                    { 389, "aa5", 85000m, 135.0, 9.0, 38.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 330, 550.0 },
                    { 390, "yankee", 45000m, 112.0, 6.0, 22.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 331, 500.0 },
                    { 391, "tiger", 120000m, 140.0, 10.0, 51.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 332, 600.0 },
                    { 392, "m7family", 280000m, 145.0, 12.0, 75.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 335, 650.0 },
                    { 393, "m7 family", 280000m, 145.0, 12.0, 75.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 335, 650.0 },
                    { 394, "maule m7", 280000m, 145.0, 12.0, 75.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 335, 650.0 },
                    { 395, "maule", 250000m, 140.0, 14.0, 70.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 336, 600.0 },
                    { 396, "dg1001", 180000m, 65.0, 0.0, 0.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 340, 600.0 },
                    { 397, "ls8", 150000m, 65.0, 0.0, 0.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 340, 600.0 },
                    { 398, "taurus m", 200000m, 100.0, 3.0, 20.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 341, 700.0 },
                    { 399, "taurus", 180000m, 95.0, 3.0, 18.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 342, 650.0 },
                    { 400, "pipistrel", 250000m, 100.0, 3.0, 20.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 343, 700.0 },
                    { 401, "virus sw", 200000m, 100.0, 3.0, 20.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 341, 700.0 },
                    { 402, "virus", 180000m, 100.0, 3.0, 20.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 342, 700.0 },
                    { 403, "sw121", 200000m, 100.0, 3.0, 20.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 341, 700.0 },
                    { 404, "vl-3", 180000m, 160.0, 6.0, 30.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 344, 700.0 },
                    { 405, "vl3", 180000m, 160.0, 6.0, 30.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 344, 700.0 },
                    { 406, "glider", 120000m, 60.0, 0.0, 0.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 350, 500.0 },
                    { 407, "magni m24", 150000m, 85.0, 6.0, 30.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 360, 300.0 },
                    { 408, "magni", 120000m, 85.0, 6.0, 30.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 361, 300.0 },
                    { 409, "gyroplane", 120000m, 80.0, 5.0, 25.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 362, 250.0 },
                    { 410, "gyrocopter", 120000m, 80.0, 5.0, 25.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 362, 250.0 },
                    { 411, "autogyro", 100000m, 80.0, 5.0, 25.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 363, 250.0 },
                    { 412, "skycrane", 18000000m, 91.0, 280.0, 1100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 370, 200.0 },
                    { 413, "s-64", 18000000m, 91.0, 280.0, 1100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 370, 200.0 },
                    { 414, "s64", 18000000m, 91.0, 280.0, 1100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 370, 200.0 },
                    { 415, "erickson", 18000000m, 91.0, 280.0, 1100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 371, 200.0 },
                    { 416, "cl415", 35000000m, 180.0, 100.0, 400.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 372, 1300.0 },
                    { 417, "cl-415", 35000000m, 180.0, 100.0, 400.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 372, 1300.0 },
                    { 418, "canadair", 35000000m, 180.0, 100.0, 400.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 373, 1300.0 },
                    { 419, "at802", 3200000m, 190.0, 60.0, 250.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 374, 800.0 },
                    { 420, "air tractor", 3200000m, 190.0, 60.0, 250.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 375, 800.0 },
                    { 421, "volocity", 500000m, 60.0, 0.0, 0.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 380, 20.0 },
                    { 422, "volocopter", 500000m, 60.0, 0.0, 0.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 381, 20.0 },
                    { 423, "evtol", 400000m, 60.0, 0.0, 0.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 382, 30.0 },
                    { 424, "joby", 500000m, 150.0, 0.0, 0.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 383, 100.0 },
                    { 425, "es30", 40000000m, 200.0, 0.0, 0.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, 384, 1100.0 },
                    { 426, "es-30", 40000000m, 200.0, 0.0, 0.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, 384, 1100.0 },
                    { 427, "heart aerospace", 40000000m, 200.0, 0.0, 0.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, 385, 1100.0 },
                    { 428, "huey", 2500000m, 110.0, 60.0, 220.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, 390, 265.0 },
                    { 429, "uh-1", 2500000m, 110.0, 60.0, 220.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, 390, 265.0 },
                    { 430, "uh1", 2500000m, 110.0, 60.0, 220.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, 390, 265.0 },
                    { 431, "e2 hawkeye", 176000000m, 320.0, 250.0, 1824.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 391, 1540.0 },
                    { 432, "e-2", 176000000m, 320.0, 250.0, 1824.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 391, 1540.0 },
                    { 433, "hawkeye", 176000000m, 320.0, 250.0, 1824.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 392, 1540.0 },
                    { 434, "t-6", 500000m, 175.0, 25.0, 110.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 393, 730.0 },
                    { 435, "t6", 500000m, 175.0, 25.0, 110.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 393, 730.0 },
                    { 436, "texan", 500000m, 175.0, 25.0, 110.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 394, 730.0 },
                    { 437, "cap-4", 60000m, 90.0, 5.0, 20.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 400, 400.0 },
                    { 438, "paulistinha", 60000m, 90.0, 5.0, 20.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 400, 400.0 },
                    { 439, "cg4a", 80000m, 75.0, 0.0, 0.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 401, 0.0 },
                    { 440, "waco", 150000m, 110.0, 10.0, 40.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 402, 450.0 },
                    { 441, "curtiss", 200000m, 75.0, 8.0, 25.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 403, 200.0 },
                    { 442, "jn4", 180000m, 75.0, 8.0, 25.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 403, 200.0 },
                    { 443, "jenny", 180000m, 75.0, 8.0, 25.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 404, 200.0 },
                    { 444, "savoia", 600000m, 145.0, 150.0, 900.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, 405, 1860.0 },
                    { 445, "s.55", 600000m, 145.0, 150.0, 900.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, 405, 1860.0 },
                    { 446, "marchetti", 500000m, 130.0, 100.0, 600.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 406, 1500.0 },
                    { 447, "latecoere", 1200000m, 185.0, 400.0, 4500.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 46, 407, 3700.0 },
                    { 448, "dornier", 3000000m, 180.0, 80.0, 400.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 408, 1000.0 },
                    { 449, "seastar", 5500000m, 180.0, 70.0, 300.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 408, 900.0 },
                    { 450, "do x", 2000000m, 118.0, 500.0, 4000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 66, 409, 1050.0 },
                    { 451, "do-31", 5000000m, 400.0, 200.0, 800.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 410, 1000.0 },
                    { 452, "do-j", 400000m, 90.0, 40.0, 250.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 411, 600.0 },
                    { 453, "saab 17", 350000m, 230.0, 40.0, 200.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 412, 600.0 },
                    { 454, "spirit of st", 500000m, 100.0, 10.0, 450.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 413, 4000.0 },
                    { 455, "ryan nyp", 500000m, 100.0, 10.0, 450.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 413, 4000.0 },
                    { 456, "wright flyer", 300000m, 30.0, 4.0, 4.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 414, 1.0 },
                    { 457, "wright", 300000m, 30.0, 4.0, 4.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 415, 1.0 },
                    { 458, "bl8", 80000m, 100.0, 6.0, 25.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 420, 400.0 },
                    { 459, "american champion", 80000m, 100.0, 6.0, 25.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 420, 400.0 },
                    { 460, "champion", 80000m, 100.0, 6.0, 25.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 421, 400.0 },
                    { 461, "flydoo", 80000m, 60.0, 3.0, 15.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 422, 150.0 },
                    { 462, "skyrascal", 50000m, 55.0, 2.0, 10.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 423, 100.0 },
                    { 463, "sky rascal", 50000m, 55.0, 2.0, 10.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 423, 100.0 },
                    { 464, "s12-g", 80000m, 90.0, 4.0, 20.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 424, 300.0 },
                    { 465, "s12g", 80000m, 90.0, 4.0, 20.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 424, 300.0 },
                    { 466, "stemme", 200000m, 95.0, 5.0, 25.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 425, 600.0 },
                    { 467, "cgs hawk", 120000m, 65.0, 4.0, 15.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 426, 200.0 },
                    { 468, "hawk arrow", 120000m, 65.0, 4.0, 15.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 426, 200.0 },
                    { 469, "midnight", 150000m, 80.0, 5.0, 20.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 427, 250.0 },
                    { 470, "draco", 500000m, 180.0, 15.0, 60.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 428, 600.0 },
                    { 471, "skyship", 8000000m, 50.0, 30.0, 200.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 430, 500.0 },
                    { 472, "airship", 5000000m, 45.0, 25.0, 150.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 431, 400.0 },
                    { 473, "zeppelin", 12000000m, 75.0, 40.0, 300.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 430, 1000.0 },
                    { 474, "blimp", 3000000m, 35.0, 15.0, 100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 432, 200.0 },
                    { 475, "hot air balloon", 50000m, 0.0, 30.0, 40.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 433, 0.0 },
                    { 476, "balloon", 50000m, 0.0, 30.0, 40.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 434, 0.0 },
                    { 477, "belugaxl", 150000000m, 430.0, 1500.0, 22000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 440, 2500.0 },
                    { 478, "beluga xl", 150000000m, 430.0, 1500.0, 22000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 440, 2500.0 },
                    { 479, "beluga", 150000000m, 430.0, 1500.0, 22000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 441, 2500.0 },
                    { 480, "boom", 50000000m, 1000.0, 500.0, 5000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 442, 4500.0 },
                    { 481, "xb-1", 50000000m, 1000.0, 500.0, 5000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 442, 4500.0 },
                    { 482, "jet", 12000000m, 400.0, 150.0, 500.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 901, 2000.0 },
                    { 483, "helicopter", 2500000m, 120.0, 30.0, 100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 902, 300.0 },
                    { 484, "heli", 2500000m, 120.0, 30.0, 100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 902, 300.0 },
                    { 485, "turboprop", 3500000m, 250.0, 60.0, 300.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 903, 1000.0 },
                    { 486, "airliner", 85000000m, 450.0, 800.0, 5000.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 150, 900, 3000.0 },
                    { 487, "twin", 1200000m, 185.0, 24.0, 120.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 904, 1000.0 },
                    { 488, "single", 450000m, 130.0, 9.0, 50.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 905, 600.0 },
                    { 489, "modular", 100000m, 60.0, 5.0, 20.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 950, 100.0 },
                    { 490, "ultralight", 80000m, 55.0, 3.0, 15.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 951, 150.0 },
                    { 491, "eagle", 350000m, 120.0, 8.0, 40.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 952, 500.0 },
                    { 492, "wasp", 100000m, 80.0, 5.0, 20.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 953, 200.0 },
                    { 493, "scout", 100000m, 80.0, 5.0, 20.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 953, 200.0 },
                    { 494, "flyer", 100000m, 60.0, 4.0, 15.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 960, 100.0 }
                });
        }
    }
}
