using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ace.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDailyEarningsDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyEarningsDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AircraftId = table.Column<int>(type: "INTEGER", nullable: false),
                    AircraftRegistration = table.Column<string>(type: "TEXT", nullable: false),
                    AircraftType = table.Column<string>(type: "TEXT", nullable: false),
                    PilotName = table.Column<string>(type: "TEXT", nullable: false),
                    FlightHours = table.Column<double>(type: "REAL", nullable: false),
                    Revenue = table.Column<decimal>(type: "TEXT", nullable: false),
                    FuelCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    MaintenanceCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    InsuranceCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    DepreciationCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    CrewCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    FBOCost = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyEarningsDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyEarningsDetails");
        }
    }
}
