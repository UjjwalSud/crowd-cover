using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrowdCover.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ud103 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bet_Events_EventId",
                table: "Bet");

            migrationBuilder.AddForeignKey(
                name: "FK_Bet_Events_EventId",
                table: "Bet",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bet_Events_EventId",
                table: "Bet");

            migrationBuilder.AddForeignKey(
                name: "FK_Bet_Events_EventId",
                table: "Bet",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
