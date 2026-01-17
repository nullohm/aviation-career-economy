using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ace.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSettingsAndAircraft : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AircraftDepreciationRate",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FuelPricePerGallon",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "InsuranceCostPerAircraftMonth",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaintenanceCostPerHour",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaintenanceIntervalHours",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PilotTrainingCost",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AircraftDepreciationRate",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "FuelPricePerGallon",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "InsuranceCostPerAircraftMonth",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MaintenanceCostPerHour",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MaintenanceIntervalHours",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PilotTrainingCost",
                table: "Settings");
        }
    }
}
