using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrowdCover.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBettorToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Book_Id",
                table: "BetSlips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BettorId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BettorAccounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Bettor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Book_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Book_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Book_Abbr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookRegion_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookRegion_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookRegion_Abbr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookRegion_Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookRegion_Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Verified = table.Column<bool>(type: "bit", nullable: false),
                    Access = table.Column<bool>(type: "bit", nullable: false),
                    Paused = table.Column<bool>(type: "bit", nullable: false),
                    BetRefreshRequested = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LatestRefreshResponse_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LatestRefreshResponse_TimeCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LatestRefreshResponse_Status = table.Column<int>(type: "int", nullable: false),
                    LatestRefreshResponse_Detail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LatestRefreshResponse_RequestId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LatestRefreshRequestId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TimeCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MissingBets = table.Column<int>(type: "int", nullable: false),
                    IsUnverifiable = table.Column<bool>(type: "bit", nullable: false),
                    RefreshInProgress = table.Column<bool>(type: "bit", nullable: false),
                    TFA = table.Column<bool>(type: "bit", nullable: false),
                    Metadata_Handle = table.Column<int>(type: "int", nullable: false),
                    Metadata_UnitSize = table.Column<int>(type: "int", nullable: false),
                    Metadata_NetProfit = table.Column<int>(type: "int", nullable: false),
                    Metadata_WinPercentage = table.Column<double>(type: "float", nullable: false),
                    Metadata_WalletShare = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BettorAccounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BettorId",
                table: "AspNetUsers",
                column: "BettorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Bettors_BettorId",
                table: "AspNetUsers",
                column: "BettorId",
                principalTable: "Bettors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Bettors_BettorId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "BettorAccounts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BettorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BettorId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Book_Id",
                table: "BetSlips",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
