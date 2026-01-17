using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ace.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSizeBasedMaintenanceCosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MaintenanceCostPerHourLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaintenanceCostPerHourMedium",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaintenanceCostPerHourMediumLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaintenanceCostPerHourSmall",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaintenanceCostPerHourVeryLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaintenanceCostPerHourLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MaintenanceCostPerHourMedium",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MaintenanceCostPerHourMediumLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MaintenanceCostPerHourSmall",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MaintenanceCostPerHourVeryLarge",
                table: "Settings");
        }
    }
}
