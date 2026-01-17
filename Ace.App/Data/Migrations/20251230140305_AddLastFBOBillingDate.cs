using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ace.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLastFBOBillingDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastFBOBillingDate",
                table: "Settings",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastFBOBillingDate",
                table: "Settings");
        }
    }
}
