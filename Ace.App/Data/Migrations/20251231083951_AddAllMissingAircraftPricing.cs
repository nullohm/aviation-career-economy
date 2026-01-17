using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ace.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAllMissingAircraftPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AircraftPricings",
                columns: new[] { "Id", "AircraftNamePattern", "BasePrice", "LastUpdated", "Priority" },
                values: new object[,]
                {
                    { 272, "belugaxl", 150000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 273, "beluga xl", 150000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 274, "beluga", 150000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 275, "f-16", 35000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 276, "f16", 35000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 277, "viper", 35000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 278, "f-18", 70000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 279, "f18", 70000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 280, "fa18", 70000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 281, "hornet", 70000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 282, "f-14", 50000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 283, "f14", 50000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 284, "tomcat", 50000000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 285, "bl8", 80000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 286, "modular", 100000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 950 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 272);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 273);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 274);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 275);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 276);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 277);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 278);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 279);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 280);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 281);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 282);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 283);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 284);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 285);

            migrationBuilder.DeleteData(
                table: "AircraftPricings",
                keyColumn: "Id",
                keyValue: 286);
        }
    }
}
