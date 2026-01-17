using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ace.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAircraftTitleToFlightRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScheduledRouteBonusPercent",
                table: "Settings",
                newName: "ServiceCostRefuelingVeryLarge");

            migrationBuilder.AddColumn<decimal>(
                name: "CargoLoadFactorPercent",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "LittleNavmapDatabasePath",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "MapCenterLatitude",
                table: "Settings",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MapCenterLongitude",
                table: "Settings",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "MapLegendExpanded",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "MapZoomLevel",
                table: "Settings",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "PassengerLoadFactorPercent",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceBonusFactorPercent",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostCateringLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostCateringMedium",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostCateringMediumLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostCateringVeryLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostDeIcingLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostDeIcingMedium",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostDeIcingMediumLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostDeIcingVeryLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostGroundHandlingLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostGroundHandlingMedium",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostGroundHandlingMediumLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostGroundHandlingVeryLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostHangarLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostHangarMedium",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostHangarMediumLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostHangarVeryLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostRefuelingLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostRefuelingMedium",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceCostRefuelingMediumLarge",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAirports",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAirspaceCTR",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAirspaceClassA",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAirspaceClassB",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAirspaceClassC",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAirspaceClassD",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAirspaceClassE",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAirspaceDanger",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAirspaceGlider",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAirspaceOther",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAirspaceOverlay",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAirspaceProhibited",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAirspaceRestricted",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoundAchievementEnabled",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoundButtonClickEnabled",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoundEnabled",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoundFlightCompletedEnabled",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoundNotificationEnabled",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoundTopOfDescentEnabled",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "SoundVolume",
                table: "Settings",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "SoundWarningEnabled",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AircraftTitle",
                table: "Flights",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceBonusAmount",
                table: "DailyEarningsDetails",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CargoLoadFactorPercent",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "LittleNavmapDatabasePath",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MapCenterLatitude",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MapCenterLongitude",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MapLegendExpanded",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MapZoomLevel",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PassengerLoadFactorPercent",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceBonusFactorPercent",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostCateringLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostCateringMedium",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostCateringMediumLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostCateringVeryLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostDeIcingLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostDeIcingMedium",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostDeIcingMediumLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostDeIcingVeryLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostGroundHandlingLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostGroundHandlingMedium",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostGroundHandlingMediumLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostGroundHandlingVeryLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostHangarLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostHangarMedium",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostHangarMediumLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostHangarVeryLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostRefuelingLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostRefuelingMedium",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ServiceCostRefuelingMediumLarge",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ShowAirports",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ShowAirspaceCTR",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ShowAirspaceClassA",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ShowAirspaceClassB",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ShowAirspaceClassC",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ShowAirspaceClassD",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ShowAirspaceClassE",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ShowAirspaceDanger",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ShowAirspaceGlider",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ShowAirspaceOther",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ShowAirspaceOverlay",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ShowAirspaceProhibited",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ShowAirspaceRestricted",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "SoundAchievementEnabled",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "SoundButtonClickEnabled",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "SoundEnabled",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "SoundFlightCompletedEnabled",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "SoundNotificationEnabled",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "SoundTopOfDescentEnabled",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "SoundVolume",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "SoundWarningEnabled",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "AircraftTitle",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "ServiceBonusAmount",
                table: "DailyEarningsDetails");

            migrationBuilder.RenameColumn(
                name: "ServiceCostRefuelingVeryLarge",
                table: "Settings",
                newName: "ScheduledRouteBonusPercent");
        }
    }
}
