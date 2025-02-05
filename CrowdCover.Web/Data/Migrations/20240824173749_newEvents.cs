using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrowdCover.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class newEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BetSlips",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Bettor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Book_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Book_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Book_Abbr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BettorAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookRef = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimePlaced = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subtype = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OddsAmerican = table.Column<int>(type: "int", nullable: false),
                    AtRisk = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ToWin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Outcome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshResponse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Incomplete = table.Column<bool>(type: "bit", nullable: false),
                    NetProfit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateClosed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeClosed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TypeSpecial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adjusted_Odds = table.Column<bool>(type: "bit", nullable: false),
                    Adjusted_Line = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adjusted_AtRisk = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetSlips", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bettors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BetRefreshRequested = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Metadata_Handle = table.Column<long>(type: "bigint", nullable: false),
                    Metadata_UnitSize = table.Column<int>(type: "int", nullable: false),
                    Metadata_NetProfit = table.Column<int>(type: "int", nullable: false),
                    Metadata_WinPercentage = table.Column<double>(type: "float", nullable: false),
                    Metadata_TotalAccounts = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bettors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SportsdataioId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SportradarId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OddsjamId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TheOddsApiId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SportId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sport = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeagueId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    League = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameSpecial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContestantAway_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContestantAway_FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContestantHome_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContestantHome_FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NeutralVenue = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bet",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Segment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Proposition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SegmentDetail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Line = table.Column<double>(type: "float", nullable: false),
                    OddsAmerican = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Outcome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Live = table.Column<bool>(type: "bit", nullable: false),
                    Incomplete = table.Column<bool>(type: "bit", nullable: false),
                    BookDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarketSelection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutoGrade = table.Column<bool>(type: "bit", nullable: false),
                    SegmentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PositionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SdioMarketId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SportradarMarketId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OddsjamMarketId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BetSlipId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bet_BetSlips_BetSlipId",
                        column: x => x.BetSlipId,
                        principalTable: "BetSlips",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bet_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bet_BetSlipId",
                table: "Bet",
                column: "BetSlipId");

            migrationBuilder.CreateIndex(
                name: "IX_Bet_EventId",
                table: "Bet",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bet");

            migrationBuilder.DropTable(
                name: "Bettors");

            migrationBuilder.DropTable(
                name: "BetSlips");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
