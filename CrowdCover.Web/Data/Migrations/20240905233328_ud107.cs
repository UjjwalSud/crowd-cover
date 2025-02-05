using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrowdCover.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ud107 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint on EventId
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Events_EventId",
                table: "Bets");

            // Ensure consistent length for EventId and Id (set both to nvarchar(450))
            migrationBuilder.AlterColumn<string>(
                name: "EventId",
                table: "Bets",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Events",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            // Recreate foreign key constraint after altering EventId
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
            // Drop foreign key constraint on EventId
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Events_EventId",
                table: "Bets");

            // Revert EventId column in the Bets table
            migrationBuilder.AlterColumn<string>(
                name: "EventId",
                table: "Bets",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            // Revert Id column in the Events table
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Events",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            // Recreate foreign key constraint after reverting EventId
            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Events_EventId",
                table: "Bets",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
