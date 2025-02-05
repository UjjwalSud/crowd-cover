using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrowdCover.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class booksEvent2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BetPlaceStatus_Android",
                table: "Books",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BetPlaceStatus_WebBrowser",
                table: "Books",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BetPlaceStatus_iOS",
                table: "Books",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HistoryDetail",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxHistoryBets",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxHistoryMonths",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MobileOnly",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PullBackToDate",
                table: "Books",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RefreshCadenceActive",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SdkRequired",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BetPlaceStatus_Android",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BetPlaceStatus_WebBrowser",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BetPlaceStatus_iOS",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "HistoryDetail",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "MaxHistoryBets",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "MaxHistoryMonths",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "MobileOnly",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PullBackToDate",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "RefreshCadenceActive",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "SdkRequired",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Books");
        }
    }
}
