using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ace.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingAircraftPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AircraftPricings",
                columns: new[] { "Id", "AircraftNamePattern", "BasePrice", "LastUpdated", "Priority" },
                values: new object[,]
                {
                    { 228, "l39", 650000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 229, "albatros", 650000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 230, "optica", 200000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 231, "husky", 280000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 232, "aviat", 280000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 233, "nx cub", 380000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 99 },
                    { 234, "nxcub", 380000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 99 },
                    { 235, "cap10", 220000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 236, "cap 10", 220000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 237, "paulistinha", 100000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 238, "jenny", 180000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 239, "jn4", 180000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 240, "caribou", 1800000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 241, "dhc-4", 1800000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 242, "cl415", 35000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 243, "cl-415", 35000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 244, "canadair", 35000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 245, "chinook", 40000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 246, "ch-47", 40000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 247, "seastar", 5500000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 248, "dornier", 3000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 249, "at802", 3200000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 250, "air tractor", 3200000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 251, "ae-45", 150000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 252, "ae-145", 180000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 253, "aero ae", 160000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 254, "c-46", 800000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 255, "commando", 800000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 256, "eagle", 350000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 900 },
                    { 257, "hawk arrow", 120000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 258, "cgs hawk", 120000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 259, "ultralight", 80000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 900 },
                    { 260, "glider", 120000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 900 },
                    { 261, "dg1001", 180000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 262, "ls8", 150000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 263, "a10", 18000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 264, "thunderbolt", 18000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 265, "warthog", 18000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 266, "boom", 50000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 267, "xb-1", 50000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 268, "skyship", 8000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 269, "airship", 5000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 270, "zeppelin", 12000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 271, "blimp", 3000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 237);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 238);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 239);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 240);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 241);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 242);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 243);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 244);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 245);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 246);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 247);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 248);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 249);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 250);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 251);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 252);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 253);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 254);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 255);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 256);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 257);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 258);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 259);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 260);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 261);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 262);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 263);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 264);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 265);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 266);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 267);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 268);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 269);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 270);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 271);
        }
    }
}
