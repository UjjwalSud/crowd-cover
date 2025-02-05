using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrowdCover.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ud106 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop ForeignKey only if it exists
            migrationBuilder.Sql(@"
                IF OBJECT_ID('FK_Bet_BetSlips_BetSlipId', 'F') IS NOT NULL 
                BEGIN 
                    ALTER TABLE Bet DROP CONSTRAINT FK_Bet_BetSlips_BetSlipId 
                END
            ");

            migrationBuilder.Sql(@"
                IF OBJECT_ID('FK_Bet_Events_EventId', 'F') IS NOT NULL 
                BEGIN 
                    ALTER TABLE Bet DROP CONSTRAINT FK_Bet_Events_EventId 
                END
            ");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bet",
                table: "Bet");

            migrationBuilder.RenameTable(
                name: "Bet",
                newName: "Bets");

            migrationBuilder.RenameIndex(
                name: "IX_Bet_EventId",
                table: "Bets",
                newName: "IX_Bets_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Bet_BetSlipId",
                table: "Bets",
                newName: "IX_Bets_BetSlipId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bets",
                table: "Bets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_BetSlips_BetSlipId",
                table: "Bets",
                column: "BetSlipId",
                principalTable: "BetSlips",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Events_EventId",
                table: "Bets",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop ForeignKey only if it exists
            migrationBuilder.Sql(@"
                IF OBJECT_ID('FK_Bets_BetSlips_BetSlipId', 'F') IS NOT NULL 
                BEGIN 
                    ALTER TABLE Bets DROP CONSTRAINT FK_Bets_BetSlips_BetSlipId 
                END
            ");

            migrationBuilder.Sql(@"
                IF OBJECT_ID('FK_Bets_Events_EventId', 'F') IS NOT NULL 
                BEGIN 
                    ALTER TABLE Bets DROP CONSTRAINT FK_Bets_Events_EventId 
                END
            ");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bets",
                table: "Bets");

            migrationBuilder.RenameTable(
                name: "Bets",
                newName: "Bet");

            migrationBuilder.RenameIndex(
                name: "IX_Bets_EventId",
                table: "Bet",
                newName: "IX_Bet_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Bets_BetSlipId",
                table: "Bet",
                newName: "IX_Bet_BetSlipId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bet",
                table: "Bet",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bet_BetSlips_BetSlipId",
                table: "Bet",
                column: "BetSlipId",
                principalTable: "BetSlips",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bet_Events_EventId",
                table: "Bet",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
