using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrowdCover.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class lastOne1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTimeUTC",
                table: "StreamingRooms");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "StreamingRooms");

            migrationBuilder.DropColumn(
                name: "StartTimeUTC",
                table: "StreamingRooms");

            migrationBuilder.AddColumn<string>(
                name: "SharpsportBettorId",
                table: "UserExtras",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SharpsportBettorId",
                table: "UserExtras");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTimeUTC",
                table: "StreamingRooms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GameId",
                table: "StreamingRooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTimeUTC",
                table: "StreamingRooms",
                type: "datetime2",
                nullable: true);
        }
    }
}
