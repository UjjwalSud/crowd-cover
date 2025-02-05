using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrowdCover.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class lastOne2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_BetSlips_BetSlipId",
                table: "Bets");

            migrationBuilder.RenameColumn(
                name: "BetSlipId",
                table: "Bets",
                newName: "BetslipId");

            migrationBuilder.RenameIndex(
                name: "IX_Bets_BetSlipId",
                table: "Bets",
                newName: "IX_Bets_BetslipId");

            migrationBuilder.AlterColumn<string>(
                name: "BetslipId",
                table: "Bets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_BetSlips_BetslipId",
                table: "Bets",
                column: "BetslipId",
                principalTable: "BetSlips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_BetSlips_BetslipId",
                table: "Bets");

            migrationBuilder.RenameColumn(
                name: "BetslipId",
                table: "Bets",
                newName: "BetSlipId");

            migrationBuilder.RenameIndex(
                name: "IX_Bets_BetslipId",
                table: "Bets",
                newName: "IX_Bets_BetSlipId");

            migrationBuilder.AlterColumn<string>(
                name: "BetSlipId",
                table: "Bets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_BetSlips_BetSlipId",
                table: "Bets",
                column: "BetSlipId",
                principalTable: "BetSlips",
                principalColumn: "Id");
        }
    }
}
