using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ace.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreAircraftPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 85,
                column: "BasePrice",
                value: 720000m);

            migrationBuilder.InsertData(
                table: "AircraftPricings",
                columns: new[] { "Id", "AircraftNamePattern", "BasePrice", "LastUpdated", "Priority" },
                values: new object[,]
                {
                    { 211, "cessna 182", 550000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 80 },
                    { 212, "c182", 550000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 80 },
                    { 213, "skylane", 550000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 80 },
                    { 214, "cessna 206", 750000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 79 },
                    { 215, "c206", 750000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 79 },
                    { 216, "stationair", 750000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 79 },
                    { 217, "xcub", 380000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 218, "cubcrafters", 380000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 219, "shock ultra", 200000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 220, "extra 300", 450000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 221, "extra", 400000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 222, "rv-", 180000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 223, "mooney", 650000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 224, "icon a5", 400000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 99 },
                    { 225, "icon", 400000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 226, "wilga", 180000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 227, "pitts", 250000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 227);

            migrationBuilder.UpdateData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 85,
                column: "BasePrice",
                value: 650000m);
        }
    }
}
