using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ace.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinanceTestCleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceCostPerService",
                table: "Settings",
                newName: "TerminalCostVeryLarge");

            migrationBuilder.RenameColumn(
                name: "InsuranceCostPerAircraftMonth",
                table: "Settings",
                newName: "TerminalCostMediumLarge");

            migrationBuilder.AddColumn<decimal>(
                name: "InsuranceRatePercentage",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaintenanceCheck100Hour",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaintenanceCheck50Hour",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaintenanceCheckACheck",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaintenanceCheckAnnual",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaintenanceCheckBCheck",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaintenanceCheckCCheck",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaintenanceCheckDCheck",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PilotBaseSalary",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PilotFlightHoursPerDay",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PilotRankCaptainBonus",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PilotRankCaptainHours",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PilotRankChiefPilotBonus",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PilotRankChiefPilotHours",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PilotRankJuniorBonus",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PilotRankJuniorHours",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PilotRankSeniorBonus",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PilotRankSeniorCaptainBonus",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PilotRankSeniorCaptainHours",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PilotRankSeniorHours",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RatePerPaxPerNMLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RatePerPaxPerNMMedium",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RatePerPaxPerNMMediumLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RatePerPaxPerNMSmall",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RatePerPaxPerNMVeryLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostCatering",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostDeIcing",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostGroundHandling",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostHangar",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostRefueling",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RunwayLengthFt",
                table: "FBOs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsuranceRatePercentage",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MaintenanceCheck100Hour",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MaintenanceCheck50Hour",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MaintenanceCheckACheck",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MaintenanceCheckAnnual",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MaintenanceCheckBCheck",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MaintenanceCheckCCheck",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MaintenanceCheckDCheck",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PilotBaseSalary",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PilotFlightHoursPerDay",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PilotRankCaptainBonus",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PilotRankCaptainHours",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PilotRankChiefPilotBonus",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PilotRankChiefPilotHours",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PilotRankJuniorBonus",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PilotRankJuniorHours",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PilotRankSeniorBonus",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PilotRankSeniorCaptainBonus",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PilotRankSeniorCaptainHours",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PilotRankSeniorHours",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "RatePerPaxPerNMLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "RatePerPaxPerNMMedium",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "RatePerPaxPerNMMediumLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "RatePerPaxPerNMSmall",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "RatePerPaxPerNMVeryLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostCatering",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostDeIcing",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostGroundHandling",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostHangar",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostRefueling",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "RunwayLengthFt",
                table: "FBOs");

            migrationBuilder.RenameColumn(
                name: "TerminalCostVeryLarge",
                table: "Settings",
                newName: "ServiceCostPerService");

            migrationBuilder.RenameColumn(
                name: "TerminalCostMediumLarge",
                table: "Settings",
                newName: "InsuranceCostPerAircraftMonth");
        }
    }
}
