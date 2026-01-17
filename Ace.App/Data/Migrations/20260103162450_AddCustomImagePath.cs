using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ace.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CargoRatePerKgPerNMLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CargoRatePerKgPerNMMedium",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CargoRatePerKgPerNMMediumLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CargoRatePerKgPerNMSmall",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CargoRatePerKgPerNMVeryLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NetworkBonusPercent",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PlayerFlightBonusPercent",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Theme",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LegType",
                table: "Flights",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentFlightId",
                table: "Flights",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlannedDestination",
                table: "Flights",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "CruiseSpeedKts",
                table: "AircraftPricings",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FuelBurnGph",
                table: "AircraftPricings",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FuelCapacityGal",
                table: "AircraftPricings",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Passengers",
                table: "AircraftPricings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "RangeNM",
                table: "AircraftPricings",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CustomImagePath",
                table: "AircraftCatalog",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomImagePath",
                table: "Aircraft",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 488.0, 3000.0, 64225.0, 467, 8000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 500.0, 3200.0, 84600.0, 555, 8000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 488.0, 1500.0, 33340.0, 290, 8300.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 488.0, 1500.0, 33340.0, 290, 8300.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 488.0, 1900.0, 35000.0, 315, 8100.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 510.0, 2300.0, 47890.0, 386, 7730.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 470.0, 1700.0, 26000.0, 277, 6350.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 470.0, 2100.0, 36700.0, 295, 7900.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 453.0, 550.0, 6853.0, 189, 3550.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 453.0, 600.0, 6875.0, 162, 3115.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 453.0, 600.0, 6875.0, 162, 3115.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 450.0, 580.0, 6300.0, 180, 3500.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 447.0, 600.0, 6270.0, 180, 3400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 447.0, 650.0, 6875.0, 200, 3200.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 470.0, 580.0, 6400.0, 140, 3700.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 470.0, 520.0, 5790.0, 130, 3400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 470.0, 520.0, 5790.0, 130, 3400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 459.0, 1400.0, 15800.0, 220, 5150.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 459.0, 1400.0, 15000.0, 250, 4050.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 461.0, 900.0, 11466.0, 200, 3900.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 459.0, 1300.0, 24140.0, 245, 5990.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "RangeNM" },
                values: new object[] { 438.0, 600.0, 3533.0, 117, 2060.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "707", 2500000m, 525.0, 2000.0, 23855.0, 189, 26, 6160.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "md-11", 45000000m, 480.0, 2200.0, 35000.0, 298, 24, 7200.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "md-10", 15000000m, 500.0, 2000.0, 28500.0, 270, 24, 4000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "dc-10", 12000000m, 500.0, 2000.0, 28500.0, 270, 24, 4000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "md80", 8000000m, 438.0, 700.0, 5840.0, 155, 24, 2050.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "md-80", 8000000m, 438.0, 700.0, 5840.0, 155, 24, 2050.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "dc-9", 6000000m, 430.0, 550.0, 4000.0, 115, 25, 1500.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "crj", 33000000m, 475.0, 400.0, 2903.0, 90, 41, 1828.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "e170", 47000000m, 447.0, 380.0, 3700.0, 70, 42, 2150.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "e175", 53000000m, 447.0, 400.0, 3700.0, 78, 42, 2200.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "e190", 50000000m, 447.0, 420.0, 3700.0, 98, 42, 2450.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "e195", 54000000m, 447.0, 450.0, 3700.0, 108, 42, 2300.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "embraer e", 50000000m, 447.0, 420.0, 3700.0, 98, 43, 2450.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "e-jet", 50000000m, 447.0, 420.0, 3700.0, 98, 43, 2450.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "erj", 35000000m, 450.0, 340.0, 1800.0, 50, 45, 2000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "atr 72", 27000000m, 276.0, 140.0, 1690.0, 72, 61, 920.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "atr72", 27000000m, 276.0, 140.0, 1690.0, 72, 61, 920.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "atr 42", 18000000m, 276.0, 110.0, 1200.0, 48, 62, 820.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "atr42", 18000000m, 276.0, 110.0, 1200.0, 48, 62, 820.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "atr", 22000000m, 276.0, 140.0, 1690.0, 72, 63, 820.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "dash 8", 32000000m, 360.0, 200.0, 1724.0, 78, 64, 1360.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "q400", 32000000m, 360.0, 200.0, 1724.0, 90, 64, 1360.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "dhc-8", 32000000m, 360.0, 200.0, 1724.0, 78, 64, 1360.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "dhc8", 32000000m, 360.0, 200.0, 1724.0, 78, 64, 1360.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "saab 340", 12000000m, 282.0, 120.0, 713.0, 34, 65, 900.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "saab340", 12000000m, 282.0, 120.0, 713.0, 34, 65, 900.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "s340", 12000000m, 282.0, 120.0, 713.0, 34, 65, 900.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "beech 1900", 4500000m, 270.0, 120.0, 665.0, 19, 66, 1247.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "b190", 4500000m, 270.0, 120.0, 665.0, 19, 66, 1247.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "1900", 4500000m, 270.0, 120.0, 665.0, 19, 66, 1247.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "model 99", 2000000m, 250.0, 85.0, 360.0, 15, 67, 1000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 172", 400000m, 122.0, 8.5, 56.0, 3, 81, 640.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c172", 400000m, 122.0, 8.5, 56.0, 3, 81, 640.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "skyhawk", 400000m, 122.0, 8.5, 56.0, 3, 81, 640.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 182", 550000m, 145.0, 14.0, 92.0, 3, 80, 930.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c182", 550000m, 145.0, 14.0, 92.0, 3, 80, 930.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "skylane", 550000m, 145.0, 14.0, 92.0, 3, 80, 930.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 152", 180000m, 107.0, 6.0999999999999996, 26.0, 1, 82, 415.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "AircraftNamePattern", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c152", 107.0, 6.0999999999999996, 26.0, 1, 82, 415.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 140", 80000m, 100.0, 6.0, 25.0, 1, 82, 420.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c140", 80000m, 100.0, 6.0, 25.0, 1, 82, 420.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 170", 120000m, 120.0, 9.0, 42.0, 3, 82, 600.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c170", 120000m, 120.0, 9.0, 42.0, 3, 82, 600.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 177", 150000m, 128.0, 10.0, 52.0, 3, 82, 700.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c177", 150000m, 128.0, 10.0, 52.0, 3, 82, 700.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cardinal", 150000m, 128.0, 10.0, 52.0, 3, 82, 700.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 185", 350000m, 160.0, 15.0, 84.0, 5, 79, 700.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c185", 350000m, 160.0, 15.0, 84.0, 5, 79, 700.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "185f", 350000m, 160.0, 15.0, 84.0, 5, 79, 700.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "skywagon", 350000m, 160.0, 15.0, 84.0, 5, 79, 700.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 206", 750000m, 160.0, 15.0, 87.0, 5, 79, 800.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c206", 750000m, 160.0, 15.0, 87.0, 5, 79, 800.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "stationair", 750000m, 160.0, 15.0, 87.0, 5, 79, 800.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 210", 450000m, 165.0, 14.0, 90.0, 5, 79, 900.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c210", 450000m, 165.0, 14.0, 90.0, 5, 79, 900.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "centurion", 450000m, 165.0, 14.0, 90.0, 5, 79, 900.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c195", 180000m, 145.0, 14.0, 80.0, 3, 80, 700.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 303", 350000m, 187.0, 24.0, 155.0, 5, 87, 1200.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c303", 350000m, 187.0, 24.0, 155.0, 5, 87, 1200.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 310", 250000m, 195.0, 24.0, 140.0, 5, 87, 1000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c310", 250000m, 195.0, 24.0, 140.0, 5, 87, 1000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 337", 220000m, 180.0, 24.0, 131.0, 5, 87, 1000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 85,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c337", 220000m, 180.0, 24.0, 131.0, 5, 87, 1000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "skymaster", 220000m, 180.0, 24.0, 131.0, 5, 87, 1000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 340", 400000m, 230.0, 32.0, 163.0, 5, 87, 1500.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c340", 400000m, 230.0, 32.0, 163.0, 5, 87, 1500.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 402", 600000m, 200.0, 46.0, 213.0, 9, 87, 1400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c402", 600000m, 200.0, 46.0, 213.0, 9, 87, 1400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 404", 700000m, 190.0, 55.0, 213.0, 9, 87, 1000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c404", 700000m, 190.0, 55.0, 213.0, 9, 87, 1000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "titan", 700000m, 190.0, 55.0, 213.0, 9, 87, 1000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 421", 800000m, 235.0, 44.0, 213.0, 7, 87, 1550.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c421", 800000m, 235.0, 44.0, 213.0, 7, 87, 1550.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 96,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c400", 750000m, 190.0, 18.0, 106.0, 4, 87, 1300.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 97,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "corvalis", 750000m, 190.0, 18.0, 106.0, 4, 87, 1300.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 98,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "columbia", 750000m, 190.0, 18.0, 106.0, 4, 87, 1300.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 208", 2700000m, 186.0, 53.0, 335.0, 9, 83, 1070.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c208", 2700000m, 186.0, 53.0, 335.0, 9, 83, 1070.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "caravan", 2700000m, 186.0, 53.0, 335.0, 9, 83, 1070.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "grand caravan", 2900000m, 186.0, 55.0, 335.0, 13, 82, 1000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 408", 5500000m, 200.0, 60.0, 200.0, 19, 83, 900.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c408", 5500000m, 200.0, 60.0, 200.0, 19, 83, 900.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "skycourier", 5500000m, 200.0, 60.0, 200.0, 19, 83, 900.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna 441", 1200000m, 290.0, 65.0, 475.0, 9, 83, 1400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 107,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c441", 1200000m, 290.0, 65.0, 475.0, 9, 83, 1400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 108,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "conquest", 1200000m, 290.0, 65.0, 475.0, 9, 83, 1400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 109,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "citation longitude", 28000000m, 483.0, 270.0, 2166.0, 8, 84, 3500.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 110,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "longitude", 28000000m, 483.0, 270.0, 2166.0, 8, 84, 3500.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 111,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "citation cj4", 11500000m, 451.0, 180.0, 870.0, 9, 85, 2165.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 112,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cj4", 11500000m, 451.0, 180.0, 870.0, 9, 85, 2165.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 113,
                columns: new[] { "AircraftNamePattern", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "citation cj3", 416.0, 150.0, 700.0, 8, 85, 2000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 114,
                columns: new[] { "AircraftNamePattern", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cj3", 416.0, 150.0, 700.0, 8, 85, 2000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 115,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "citation", 10000000m, 400.0, 180.0, 750.0, 8, 86, 1900.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 116,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cessna", 450000m, 130.0, 9.0, 50.0, 3, 88, 600.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 117,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "textron", 500000m, 130.0, 9.0, 50.0, 3, 88, 600.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 118,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "piper cub", 180000m, 87.0, 5.0, 18.0, 1, 101, 220.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 119,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "pa-18", 200000m, 87.0, 5.0, 18.0, 1, 101, 220.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 120,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "j-3", 120000m, 87.0, 5.0, 18.0, 1, 101, 220.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 121,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "super cub", 220000m, 100.0, 8.0, 36.0, 1, 101, 400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 122,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cherokee", 320000m, 124.0, 9.0, 50.0, 3, 102, 640.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 123,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "pa-28", 320000m, 124.0, 9.0, 50.0, 3, 102, 640.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 124,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "archer", 380000m, 135.0, 10.0, 50.0, 3, 102, 520.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 125,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "warrior", 320000m, 124.0, 9.0, 50.0, 3, 102, 640.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 126,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "seminole", 680000m, 160.0, 16.0, 102.0, 3, 103, 700.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 127,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "pa-44", 680000m, 160.0, 16.0, 102.0, 3, 103, 700.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 128,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "seneca", 700000m, 180.0, 22.0, 123.0, 5, 103, 1000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 129,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "pa34", 700000m, 180.0, 22.0, 123.0, 5, 103, 1000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 130,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "m350", 1200000m, 213.0, 17.0, 120.0, 5, 100, 1343.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 131,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "m500", 3000000m, 260.0, 30.0, 170.0, 5, 100, 1000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 132,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "m600", 3500000m, 274.0, 40.0, 260.0, 5, 100, 1658.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 133,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "piper", 380000m, 130.0, 9.0, 50.0, 3, 104, 600.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 134,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "da62", 1400000m, 192.0, 13.0, 79.0, 6, 121, 1100.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 135,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "da42", 950000m, 180.0, 9.0, 50.0, 3, 122, 1100.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 136,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "twin star", 950000m, 180.0, 9.0, 50.0, 3, 122, 1100.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 137,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "da40", 550000m, 147.0, 8.0, 40.0, 3, 123, 720.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 138,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "diamond star", 550000m, 147.0, 8.0, 40.0, 3, 123, 720.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 139,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "da20", 220000m, 135.0, 6.0, 24.0, 2, 124, 600.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 140,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "dv20", 220000m, 135.0, 6.0, 24.0, 2, 124, 600.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 141,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "katana", 220000m, 135.0, 6.0, 24.0, 2, 124, 600.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 142,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "diamond", 500000m, 147.0, 8.0, 40.0, 3, 125, 720.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 143,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "sr22t", 950000m, 214.0, 18.0, 92.0, 4, 140, 1000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 144,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "sr22", 850000m, 183.0, 17.0, 92.0, 4, 141, 1050.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 145,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "sr20", 720000m, 155.0, 10.5, 56.0, 3, 142, 810.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 146,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "vision jet", 3200000m, 300.0, 50.0, 296.0, 5, 139, 1200.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 147,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "sf50", 3200000m, 300.0, 50.0, 296.0, 5, 139, 1200.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 148,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cirrus", 800000m, 183.0, 15.0, 80.0, 4, 143, 1000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 149,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "king air 350i", 7400000m, 312.0, 100.0, 544.0, 11, 150, 1806.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 150,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "king air 350", 6400000m, 312.0, 100.0, 544.0, 11, 151, 1806.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 151,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "king air 260", 4800000m, 282.0, 100.0, 544.0, 8, 151, 1580.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 152,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "king air", 5500000m, 282.0, 100.0, 544.0, 8, 152, 1580.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 153,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "kingair", 5500000m, 282.0, 100.0, 544.0, 8, 152, 1580.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 154,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "b200", 5500000m, 282.0, 100.0, 544.0, 8, 152, 1580.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 155,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c90", 1500000m, 217.0, 71.0, 384.0, 7, 152, 1120.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 156,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "baron g58", 1500000m, 200.0, 26.0, 142.0, 5, 153, 1225.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 157,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "baron", 1300000m, 200.0, 26.0, 142.0, 5, 153, 1225.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 158,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "g58", 1500000m, 200.0, 26.0, 142.0, 5, 153, 1225.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 159,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "bonanza g36", 1100000m, 176.0, 14.5, 74.0, 4, 154, 920.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 160,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "bonanza", 950000m, 176.0, 14.5, 74.0, 4, 154, 920.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 161,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "g36", 1100000m, 176.0, 14.5, 74.0, 4, 154, 920.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 162,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "staggerwing", 350000m, 200.0, 22.0, 100.0, 4, 155, 600.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 163,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "d17", 350000m, 200.0, 22.0, 100.0, 4, 155, 600.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 164,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "d18", 400000m, 195.0, 48.0, 198.0, 9, 155, 900.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 165,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "model 18", 400000m, 195.0, 48.0, 198.0, 9, 155, 900.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 166,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "model 76", 450000m, 160.0, 16.0, 102.0, 3, 155, 700.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 167,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "duchess", 450000m, 160.0, 16.0, 102.0, 3, 155, 700.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 168,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "beechcraft", 700000m, 176.0, 14.5, 74.0, 4, 155, 920.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 169,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "global 7500", 75000000m, 530.0, 450.0, 7687.0, 19, 171, 7700.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 170,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "global7500", 75000000m, 530.0, 450.0, 7687.0, 19, 171, 7700.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 171,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "global 6500", 58000000m, 530.0, 420.0, 6600.0, 17, 171, 6600.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 172,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "bombardier global", 65000000m, 530.0, 450.0, 7687.0, 19, 172, 7700.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 173,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "gulfstream g700", 78000000m, 516.0, 430.0, 6590.0, 19, 170, 7500.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 174,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "gulfstream g650", 65000000m, 516.0, 420.0, 6590.0, 19, 171, 7000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 175,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "gulfstream g280", 24500000m, 482.0, 200.0, 2550.0, 10, 173, 3600.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 176,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "gulfstream", 45000000m, 516.0, 420.0, 6590.0, 19, 173, 7000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 177,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "challenger 650", 32000000m, 470.0, 227.0, 2700.0, 12, 174, 4000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 178,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "challenger 350", 27000000m, 470.0, 210.0, 2500.0, 10, 174, 3200.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 179,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "challenger", 28000000m, 470.0, 227.0, 2700.0, 12, 174, 4000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 180,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "learjet 75", 9500000m, 465.0, 206.0, 906.0, 9, 175, 2050.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 181,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "learjet", 9500000m, 465.0, 206.0, 906.0, 9, 175, 2050.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 182,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "lear", 9500000m, 465.0, 206.0, 906.0, 9, 175, 2050.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 183,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "phenom 300", 10500000m, 453.0, 158.0, 799.0, 8, 176, 1971.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 184,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "phenom", 9000000m, 453.0, 158.0, 799.0, 8, 177, 1971.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 185,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "praetor 600", 21000000m, 466.0, 210.0, 2392.0, 8, 175, 4018.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 186,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "praetor 500", 17500000m, 466.0, 190.0, 2000.0, 8, 175, 3340.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 187,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "praetor", 18000000m, 466.0, 210.0, 2392.0, 8, 176, 4018.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 188,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "legacy 500", 20000000m, 466.0, 200.0, 2100.0, 12, 175, 3125.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 189,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "legacy500", 20000000m, 466.0, 200.0, 2100.0, 12, 175, 3125.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 190,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "legacy 450", 18000000m, 466.0, 190.0, 1900.0, 9, 175, 2900.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 191,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "legacy 600", 16000000m, 459.0, 220.0, 2300.0, 13, 175, 3400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 192,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "legacy 650", 26000000m, 465.0, 250.0, 2800.0, 14, 175, 3900.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 193,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "legacy", 18000000m, 466.0, 200.0, 2100.0, 12, 176, 3125.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 194,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "falcon 8x", 58000000m, 488.0, 340.0, 4188.0, 14, 172, 6450.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 195,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "falcon 900", 45000000m, 480.0, 280.0, 3300.0, 12, 173, 4750.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 196,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "falcon 2000", 35000000m, 470.0, 240.0, 2600.0, 10, 174, 4000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 197,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "falcon", 40000000m, 488.0, 340.0, 4188.0, 14, 175, 6450.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 198,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "hawker", 8000000m, 430.0, 180.0, 1000.0, 8, 178, 2600.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 199,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "hondajet", 5800000m, 420.0, 85.0, 350.0, 5, 177, 1437.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 200,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "pc-12 ngx", 6200000m, 290.0, 77.0, 402.0, 9, 190, 1800.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 201,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "pc-12", 6000000m, 290.0, 77.0, 402.0, 9, 191, 1800.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 202,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "pc12", 6000000m, 290.0, 77.0, 402.0, 9, 191, 1800.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 203,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "pilatus pc12", 6000000m, 290.0, 77.0, 402.0, 9, 191, 1800.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 204,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "pc-6", 2500000m, 115.0, 39.0, 298.0, 9, 192, 500.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 205,
                columns: new[] { "AircraftNamePattern", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "pc6", 115.0, 39.0, 298.0, 9, 192, 500.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 206,
                columns: new[] { "AircraftNamePattern", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "porter", 115.0, 39.0, 298.0, 9, 192, 500.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 207,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "pc-24", 11000000m, 440.0, 180.0, 893.0, 8, 189, 2000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 208,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "pc24", 11000000m, 440.0, 180.0, 893.0, 8, 189, 2000.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 209,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "pilatus", 5000000m, 290.0, 77.0, 402.0, 9, 193, 1800.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 210,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "tbm 960", 4800000m, 330.0, 57.0, 292.0, 5, 194, 1730.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 211,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "tbm960", 4800000m, 330.0, 57.0, 292.0, 5, 194, 1730.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 212,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "tbm 940", 4200000m, 330.0, 57.0, 292.0, 5, 195, 1730.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 213,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "tbm940", 4200000m, 330.0, 57.0, 292.0, 5, 195, 1730.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 214,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "tbm 930", 3900000m, 330.0, 57.0, 292.0, 5, 196, 1730.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 215,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "tbm930", 3900000m, 330.0, 57.0, 292.0, 5, 196, 1730.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 216,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "tbm", 4200000m, 330.0, 57.0, 292.0, 5, 197, 1730.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 217,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "kodiak 100", 2900000m, 174.0, 48.0, 315.0, 9, 197, 1132.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 218,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "kodiak", 2700000m, 174.0, 48.0, 315.0, 9, 198, 1132.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 219,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "quest", 2700000m, 174.0, 48.0, 315.0, 9, 198, 1132.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 220,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "mu-2", 1200000m, 300.0, 65.0, 380.0, 7, 198, 1300.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 221,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "mu2", 1200000m, 300.0, 65.0, 380.0, 7, 198, 1300.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 222,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "bell 412", 8500000m, 140.0, 90.0, 336.0, 14, 211, 400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 223,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "bell412", 8500000m, 140.0, 90.0, 336.0, 14, 211, 400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 224,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "b412", 8500000m, 140.0, 90.0, 336.0, 14, 211, 400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 225,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "bell 429", 7500000m, 155.0, 70.0, 225.0, 7, 211, 400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 226,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "b429", 7500000m, 155.0, 70.0, 225.0, 7, 211, 400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 227,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "bell 427", 4000000m, 140.0, 60.0, 200.0, 6, 211, 350.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 228,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "b427", 4000000m, 140.0, 60.0, 200.0, 6, 211, 350.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 229,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "bell 407", 4200000m, 133.0, 42.0, 145.0, 6, 212, 324.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 230,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "bell407", 4200000m, 133.0, 42.0, 145.0, 6, 212, 324.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 231,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "model 407", 4200000m, 133.0, 42.0, 145.0, 6, 212, 324.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 232,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "bell 206", 1500000m, 120.0, 26.0, 91.0, 4, 213, 260.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 233,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "jetranger", 1500000m, 120.0, 26.0, 91.0, 4, 213, 260.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 234,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "bell 505", 1600000m, 125.0, 25.0, 75.0, 4, 213, 300.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 235,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "b505", 1600000m, 125.0, 25.0, 75.0, 4, 213, 300.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 236,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "bell 47", 300000m, 84.0, 12.0, 30.0, 2, 214, 200.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 237,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "bell 204", 2500000m, 110.0, 60.0, 220.0, 13, 214, 265.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 238,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "b204", 2500000m, 110.0, 60.0, 220.0, 13, 214, 265.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 239,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "bell 230", 3000000m, 140.0, 55.0, 200.0, 8, 214, 350.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 240,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "b230", 3000000m, 140.0, 55.0, 200.0, 8, 214, 350.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 241,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "bell", 3000000m, 130.0, 40.0, 150.0, 6, 215, 300.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 242,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "h225", 32000000m, 150.0, 120.0, 700.0, 19, 231, 480.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 243,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "super puma", 32000000m, 150.0, 120.0, 700.0, 19, 231, 480.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 244,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "h175", 18000000m, 152.0, 100.0, 470.0, 16, 231, 380.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 245,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "h160", 14500000m, 160.0, 85.0, 320.0, 12, 232, 450.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 246,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "h145", 12000000m, 129.0, 58.0, 239.0, 9, 233, 352.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 247,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "ec145", 12000000m, 129.0, 58.0, 239.0, 9, 233, 352.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 248,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "h135", 5500000m, 137.0, 52.0, 175.0, 6, 234, 395.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 249,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "ec135", 5500000m, 137.0, 52.0, 175.0, 6, 234, 395.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 250,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "ec-135", 5500000m, 137.0, 52.0, 175.0, 6, 234, 395.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 251,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "h130", 3200000m, 130.0, 45.0, 145.0, 6, 234, 330.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 252,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "h125", 3800000m, 137.0, 45.0, 143.0, 5, 235, 340.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 253,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "as350", 3800000m, 137.0, 45.0, 143.0, 5, 235, 340.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 254,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "ecureuil", 3800000m, 137.0, 45.0, 143.0, 5, 235, 340.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 255,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "robinson r66", 1050000m, 110.0, 17.0, 73.0, 4, 251, 350.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 256,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "r66", 1050000m, 110.0, 17.0, 73.0, 4, 251, 350.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 257,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "robinson r44", 550000m, 117.0, 16.0, 49.0, 3, 252, 245.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 258,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "r44", 550000m, 117.0, 16.0, 49.0, 3, 252, 245.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 259,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "robinson r22", 350000m, 96.0, 8.0, 27.0, 1, 253, 200.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 260,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "r22", 350000m, 96.0, 8.0, 27.0, 1, 253, 200.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 261,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "robinson", 600000m, 110.0, 14.0, 50.0, 3, 254, 250.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 262,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cabri g2", 420000m, 100.0, 9.0, 41.0, 1, 261, 370.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 263,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "cabri", 420000m, 100.0, 9.0, 41.0, 1, 261, 370.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 264,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "schweizer 300", 350000m, 95.0, 10.0, 35.0, 2, 262, 250.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 265,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "schweizer", 350000m, 95.0, 10.0, 35.0, 2, 262, 250.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 266,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "s300", 350000m, 95.0, 10.0, 35.0, 2, 262, 250.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 267,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "md 500", 2800000m, 135.0, 30.0, 60.0, 4, 263, 250.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 268,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "md500", 2800000m, 135.0, 30.0, 60.0, 4, 263, 250.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 269,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "md 530", 3200000m, 135.0, 32.0, 60.0, 4, 263, 250.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 270,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "md530", 3200000m, 135.0, 32.0, 60.0, 4, 263, 250.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 271,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "leonardo aw", 15000000m, 151.0, 95.0, 445.0, 15, 264, 448.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 272,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "aw139", 15000000m, 151.0, 95.0, 445.0, 15, 264, 448.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 273,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "aw109", 6500000m, 155.0, 60.0, 200.0, 7, 265, 500.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 274,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "sikorsky s-76", 13000000m, 155.0, 80.0, 270.0, 12, 264, 400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 275,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "sikorsky", 10000000m, 140.0, 75.0, 250.0, 12, 266, 350.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 276,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "chinook", 40000000m, 145.0, 250.0, 1030.0, 33, 267, 400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 277,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "ch-47", 40000000m, 145.0, 250.0, 1030.0, 33, 267, 400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 278,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c-17", 220000000m, 450.0, 2500.0, 35546.0, 0, 271, 4400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 279,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "globemaster", 220000000m, 450.0, 2500.0, 35546.0, 0, 271, 4400.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 280,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "a400", 120000000m, 400.0, 1200.0, 15000.0, 0, 272, 4500.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 281,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "a400m", 120000000m, 400.0, 1200.0, 15000.0, 0, 272, 4500.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 282,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "atlas", 120000000m, 400.0, 1200.0, 15000.0, 0, 272, 4500.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 283,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c-130", 35000000m, 320.0, 800.0, 6700.0, 0, 273, 2360.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 284,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "hercules", 35000000m, 320.0, 800.0, 6700.0, 0, 273, 2360.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 285,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "c-5", 180000000m, 450.0, 3500.0, 51150.0, 0, 271, 4440.0 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 286,
                columns: new[] { "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "Passengers", "Priority", "RangeNM" },
                values: new object[] { "galaxy", 180000000m, 450.0, 3500.0, 51150.0, 0, 271, 4440.0 });

            migrationBuilder.InsertData(
                table: "AircraftPricings",
                columns: new[] { "Id", "AircraftNamePattern", "BasePrice", "CruiseSpeedKts", "FuelBurnGph", "FuelCapacityGal", "LastUpdated", "Passengers", "Priority", "RangeNM" },
                values: new object[,]
                {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 287);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 288);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 289);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 290);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 291);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 292);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 293);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 294);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 295);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 296);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 297);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 298);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 299);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 300);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 301);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 302);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 303);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 304);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 305);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 306);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 307);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 308);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 309);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 310);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 311);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 312);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 313);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 314);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 315);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 316);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 317);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 318);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 319);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 320);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 321);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 322);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 323);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 324);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 325);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 326);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 327);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 328);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 329);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 330);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 331);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 332);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 333);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 334);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 335);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 336);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 337);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 338);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 339);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 340);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 341);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 342);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 343);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 344);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 345);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 346);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 347);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 348);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 349);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 350);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 351);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 352);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 353);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 354);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 355);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 356);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 357);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 358);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 359);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 360);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 361);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 362);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 363);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 364);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 365);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 366);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 367);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 368);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 369);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 370);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 371);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 372);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 373);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 374);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 375);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 376);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 377);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 378);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 379);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 380);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 381);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 382);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 383);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 384);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 385);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 386);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 387);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 388);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 389);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 390);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 391);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 392);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 393);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 394);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 395);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 396);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 397);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 398);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 399);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 400);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 401);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 402);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 403);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 404);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 405);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 406);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 407);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 408);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 409);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 410);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 411);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 412);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 413);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 414);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 415);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 416);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 417);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 418);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 419);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 420);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 421);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 422);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 423);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 424);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 425);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 426);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 427);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 428);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 429);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 430);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 431);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 432);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 433);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 434);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 435);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 436);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 437);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 438);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 439);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 440);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 441);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 442);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 443);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 444);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 445);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 446);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 447);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 448);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 449);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 450);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 451);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 452);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 453);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 454);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 455);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 456);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 457);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 458);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 459);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 460);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 461);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 462);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 463);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 464);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 465);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 466);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 467);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 468);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 469);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 470);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 471);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 472);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 473);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 474);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 475);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 476);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 477);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 478);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 479);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 480);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 481);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 482);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 483);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 484);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 485);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 486);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 487);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 488);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 489);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 490);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 491);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 492);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 493);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 494);

            migrationBuilder.DropColumn(
                name: "CargoRatePerKgPerNMLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "CargoRatePerKgPerNMMedium",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "CargoRatePerKgPerNMMediumLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "CargoRatePerKgPerNMSmall",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "CargoRatePerKgPerNMVeryLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "NetworkBonusPercent",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PlayerFlightBonusPercent",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Theme",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "LegType",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "ParentFlightId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "PlannedDestination",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "CruiseSpeedKts",
                table: "AircraftPricings");

            migrationBuilder.DropColumn(
                name: "FuelBurnGph",
                table: "AircraftPricings");

            migrationBuilder.DropColumn(
                name: "FuelCapacityGal",
                table: "AircraftPricings");

            migrationBuilder.DropColumn(
                name: "Passengers",
                table: "AircraftPricings");

            migrationBuilder.DropColumn(
                name: "RangeNM",
                table: "AircraftPricings");

            migrationBuilder.DropColumn(
                name: "CustomImagePath",
                table: "AircraftCatalog");

            migrationBuilder.DropColumn(
                name: "CustomImagePath",
                table: "Aircraft");

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "md-", 42000000m, 25 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "crj", 33000000m, 41 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "e170", 47000000m, 42 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "e175", 53000000m, 42 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "e190", 50000000m, 42 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "e195", 54000000m, 42 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "embraer e", 50000000m, 43 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "e-jet", 50000000m, 43 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "atr 72", 27000000m, 61 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "atr72", 27000000m, 61 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "atr 42", 18000000m, 62 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "atr42", 18000000m, 62 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "atr", 22000000m, 63 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "dash 8", 32000000m, 64 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "q400", 32000000m, 64 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "dhc-8", 32000000m, 64 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "saab 340", 12000000m, 65 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "saab340", 12000000m, 65 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cessna 172", 400000m, 81 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "c172", 400000m, 81 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "skyhawk", 400000m, 81 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cessna 152", 180000m, 82 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "c152", 180000m, 82 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cessna 208", 2700000m, 83 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "c208", 2700000m, 83 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "caravan", 2700000m, 83 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "grand caravan", 2900000m, 82 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "citation longitude", 28000000m, 84 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "citation cj4", 11500000m, 85 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cj4", 11500000m, 85 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "citation cj3", 9500000m, 85 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cj3", 9500000m, 85 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "citation", 10000000m, 86 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "c400", 750000m, 87 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "corvalis", 750000m, 87 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "columbia", 750000m, 87 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cessna", 450000m, 88 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "textron", 500000m, 88 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "AircraftNamePattern", "Priority" },
                values: new object[] { "piper cub", 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "pa-18", 200000m, 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "j-3", 120000m, 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "super cub", 220000m, 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cherokee", 320000m, 102 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "pa-28", 320000m, 102 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "archer", 380000m, 102 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "warrior", 320000m, 102 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "seminole", 680000m, 103 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "pa-44", 680000m, 103 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "m350", 1200000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "m500", 3000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "m600", 3500000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "piper", 380000m, 104 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "da62", 1400000m, 121 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "da42", 950000m, 122 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "twin star", 950000m, 122 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "da40", 550000m, 123 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "diamond star", 550000m, 123 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "da20", 220000m, 124 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "katana", 220000m, 124 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "diamond", 500000m, 125 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "sr22t", 950000m, 140 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "sr22", 850000m, 141 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 85,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "sr20", 720000m, 142 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "vision jet", 3200000m, 139 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "sf50", 3200000m, 139 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cirrus", 800000m, 143 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "king air 350i", 7400000m, 150 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "king air 350", 6400000m, 151 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "king air 260", 4800000m, 151 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "king air", 5500000m, 152 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "kingair", 5500000m, 152 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "baron g58", 1500000m, 153 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "baron", 1300000m, 153 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 96,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "g58", 1500000m, 153 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 97,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "bonanza g36", 1100000m, 154 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 98,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "bonanza", 950000m, 154 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "g36", 1100000m, 154 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "beechcraft", 700000m, 155 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "global 7500", 75000000m, 171 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "global7500", 75000000m, 171 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "global 6500", 58000000m, 171 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "bombardier global", 65000000m, 172 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "gulfstream g700", 78000000m, 170 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "gulfstream g650", 65000000m, 171 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 107,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "gulfstream g280", 24500000m, 173 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 108,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "gulfstream", 45000000m, 173 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 109,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "challenger 650", 32000000m, 174 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 110,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "challenger 350", 27000000m, 174 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 111,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "challenger", 28000000m, 174 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 112,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "learjet 75", 9500000m, 175 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 113,
                columns: new[] { "AircraftNamePattern", "Priority" },
                values: new object[] { "learjet", 175 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 114,
                columns: new[] { "AircraftNamePattern", "Priority" },
                values: new object[] { "lear", 175 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 115,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "phenom 300", 10500000m, 176 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 116,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "phenom", 9000000m, 177 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 117,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "praetor 600", 21000000m, 175 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 118,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "praetor 500", 17500000m, 175 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 119,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "praetor", 18000000m, 176 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 120,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "falcon 8x", 58000000m, 172 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 121,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "falcon 900", 45000000m, 173 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 122,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "falcon 2000", 35000000m, 174 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 123,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "falcon", 40000000m, 175 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 124,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "hawker", 8000000m, 178 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 125,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "hondajet", 5800000m, 177 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 126,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "pc-12 ngx", 6200000m, 190 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 127,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "pc-12", 6000000m, 191 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 128,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "pilatus pc12", 6000000m, 191 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 129,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "pc-6", 2500000m, 192 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 130,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "porter", 2500000m, 192 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 131,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "pc-24", 11000000m, 189 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 132,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "pilatus", 5000000m, 193 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 133,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "tbm 960", 4800000m, 194 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 134,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "tbm960", 4800000m, 194 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 135,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "tbm 940", 4200000m, 195 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 136,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "tbm940", 4200000m, 195 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 137,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "tbm 930", 3900000m, 196 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 138,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "tbm930", 3900000m, 196 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 139,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "tbm", 4200000m, 197 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 140,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "kodiak 100", 2900000m, 197 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 141,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "kodiak", 2700000m, 198 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 142,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "bell 412", 8500000m, 211 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 143,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "bell412", 8500000m, 211 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 144,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "bell 429", 7500000m, 211 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 145,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "bell 407", 4200000m, 212 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 146,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "bell407", 4200000m, 212 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 147,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "bell 206", 1500000m, 213 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 148,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "jetranger", 1500000m, 213 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 149,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "bell 505", 1600000m, 213 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 150,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "bell", 3000000m, 215 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 151,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "h225", 32000000m, 231 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 152,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "super puma", 32000000m, 231 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 153,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "h175", 18000000m, 231 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 154,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "h160", 14500000m, 232 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 155,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "h145", 12000000m, 233 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 156,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "ec145", 12000000m, 233 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 157,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "h135", 5500000m, 234 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 158,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "ec135", 5500000m, 234 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 159,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "h130", 3200000m, 234 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 160,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "h125", 3800000m, 235 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 161,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "as350", 3800000m, 235 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 162,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "ecureuil", 3800000m, 235 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 163,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "robinson r66", 1050000m, 251 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 164,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "r66", 1050000m, 251 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 165,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "robinson r44", 550000m, 252 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 166,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "r44", 550000m, 252 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 167,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "robinson r22", 350000m, 253 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 168,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "r22", 350000m, 253 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 169,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "robinson", 600000m, 254 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 170,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cabri g2", 420000m, 261 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 171,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cabri", 420000m, 261 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 172,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "schweizer 300", 350000m, 262 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 173,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "schweizer", 350000m, 262 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 174,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "s300", 350000m, 262 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 175,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "300c", 350000m, 262 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 176,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "md 500", 2800000m, 263 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 177,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "md500", 2800000m, 263 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 178,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "md 530", 3200000m, 263 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 179,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "md530", 3200000m, 263 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 180,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "leonardo aw", 15000000m, 264 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 181,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "aw139", 15000000m, 264 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 182,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "aw109", 6500000m, 265 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 183,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "sikorsky s-76", 13000000m, 264 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 184,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "sikorsky", 10000000m, 266 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 185,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "c-17", 220000000m, 271 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 186,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "globemaster", 220000000m, 271 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 187,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "a400", 120000000m, 272 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 188,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "atlas", 120000000m, 272 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 189,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "c-130", 35000000m, 273 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 190,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "hercules", 35000000m, 273 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 191,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "c-5", 180000000m, 271 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 192,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "galaxy", 180000000m, 271 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 193,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "dc-6", 1200000m, 291 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 194,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "dc-3", 350000m, 292 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 195,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "dakota", 350000m, 292 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 196,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "twin otter", 6500000m, 292 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 197,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "dhc-6", 6500000m, 292 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 198,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "otter", 1500000m, 293 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 199,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "dhc-3", 1500000m, 293 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 200,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "beaver", 750000m, 294 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 201,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "dhc-2", 750000m, 294 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 202,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "an-2", 280000m, 295 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 203,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "antonov", 280000m, 295 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 204,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "jet", 12000000m, 901 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 205,
                columns: new[] { "AircraftNamePattern", "Priority" },
                values: new object[] { "helicopter", 902 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 206,
                columns: new[] { "AircraftNamePattern", "Priority" },
                values: new object[] { "heli", 902 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 207,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "turboprop", 3500000m, 903 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 208,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "airliner", 85000000m, 900 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 209,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "twin", 1200000m, 904 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 210,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "single", 450000m, 905 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 211,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cessna 182", 550000m, 80 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 212,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "c182", 550000m, 80 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 213,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "skylane", 550000m, 80 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 214,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cessna 206", 750000m, 79 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 215,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "c206", 750000m, 79 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 216,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "stationair", 750000m, 79 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 217,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "xcub", 380000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 218,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cubcrafters", 380000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 219,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "shock ultra", 200000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 220,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "extra 300", 450000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 221,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "extra", 400000m, 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 222,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "rv-", 180000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 223,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "mooney", 650000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 224,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "icon a5", 400000m, 99 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 225,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "icon", 400000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 226,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "wilga", 180000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 227,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "pitts", 250000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 228,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "l39", 650000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 229,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "albatros", 650000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 230,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "optica", 200000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 231,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "husky", 280000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 232,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "aviat", 280000m, 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 233,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "nx cub", 380000m, 99 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 234,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "nxcub", 380000m, 99 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 235,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cap10", 220000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 236,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cap 10", 220000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 237,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "paulistinha", 100000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 238,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "jenny", 180000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 239,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "jn4", 180000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 240,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "caribou", 1800000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 241,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "dhc-4", 1800000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 242,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cl415", 35000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 243,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cl-415", 35000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 244,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "canadair", 35000000m, 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 245,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "chinook", 40000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 246,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "ch-47", 40000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 247,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "seastar", 5500000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 248,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "dornier", 3000000m, 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 249,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "at802", 3200000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 250,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "air tractor", 3200000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 251,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "ae-45", 150000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 252,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "ae-145", 180000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 253,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "aero ae", 160000m, 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 254,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "c-46", 800000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 255,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "commando", 800000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 256,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "eagle", 350000m, 900 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 257,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "hawk arrow", 120000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 258,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "cgs hawk", 120000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 259,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "ultralight", 80000m, 900 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 260,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "glider", 120000m, 900 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 261,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "dg1001", 180000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 262,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "ls8", 150000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 263,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "a10", 18000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 264,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "thunderbolt", 18000000m, 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 265,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "warthog", 18000000m, 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 266,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "boom", 50000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 267,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "xb-1", 50000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 268,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "skyship", 8000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 269,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "airship", 5000000m, 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 270,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "zeppelin", 12000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 271,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "blimp", 3000000m, 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 272,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "belugaxl", 150000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 273,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "beluga xl", 150000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 274,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "beluga", 150000000m, 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 275,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "f-16", 35000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 276,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "f16", 35000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 277,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "viper", 35000000m, 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 278,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "f-18", 70000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 279,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "f18", 70000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 280,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "fa18", 70000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 281,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "hornet", 70000000m, 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 282,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "f-14", 50000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 283,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "f14", 50000000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 284,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "tomcat", 50000000m, 101 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 285,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "bl8", 80000m, 100 });

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 286,
                columns: new[] { "AircraftNamePattern", "BasePrice", "Priority" },
                values: new object[] { "modular", 100000m, 950 });
        }
    }
}
