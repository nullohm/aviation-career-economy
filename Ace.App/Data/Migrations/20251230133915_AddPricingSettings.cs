using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ace.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPricingSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CateringCostPerPassenger",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 8m);

            migrationBuilder.AddColumn<decimal>(
                name: "CrewCostMultiplier",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 1.3m);

            migrationBuilder.AddColumn<decimal>(
                name: "LandingFeeLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 750m);

            migrationBuilder.AddColumn<decimal>(
                name: "LandingFeeMedium",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 150m);

            migrationBuilder.AddColumn<decimal>(
                name: "LandingFeeMediumLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 350m);

            migrationBuilder.AddColumn<decimal>(
                name: "LandingFeeSmall",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 50m);

            migrationBuilder.AddColumn<decimal>(
                name: "LandingFeeVeryLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 1500m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CateringCostPerPassenger",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "CrewCostMultiplier",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "LandingFeeLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "LandingFeeMedium",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "LandingFeeMediumLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "LandingFeeSmall",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "LandingFeeVeryLarge",
                table: "Settings");
        }
    }
}
