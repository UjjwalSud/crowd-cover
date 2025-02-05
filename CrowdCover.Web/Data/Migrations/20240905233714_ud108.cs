using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrowdCover.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ud108 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove this part since you've manually handled the Id change for Events
            // migrationBuilder.AlterColumn<string>(
            //     name: "Id",
            //     table: "Events",
            //     type: "nvarchar(450)",
            //     maxLength: 450,
            //     nullable: false,
            //     oldClrType: typeof(string),
            //     oldType: "nvarchar(128)",
            //     oldMaxLength: 128);

            // Keep this part to modify EventId in the Bets table
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No need to revert the Id change for Events if it has been manually done
            // migrationBuilder.AlterColumn<string>(
            //     name: "Id",
            //     table: "Events",
            //     type: "nvarchar(128)",
            //     maxLength: 128,
            //     nullable: false,
            //     oldClrType: typeof(string),
            //     oldType: "nvarchar(450)",
            //     oldMaxLength: 450);

            // Revert the EventId change in the Bets table
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
        }
    }
}
