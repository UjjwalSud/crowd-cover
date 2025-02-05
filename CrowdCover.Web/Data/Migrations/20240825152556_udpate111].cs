using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrowdCover.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class udpate111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Bettors_BettorId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BettorId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "BettorId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BettorId",
                table: "AspNetUsers",
                column: "BettorId",
                unique: true,
                filter: "[BettorId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Bettors_BettorId",
                table: "AspNetUsers",
                column: "BettorId",
                principalTable: "Bettors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Bettors_BettorId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BettorId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "BettorId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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
    }
}
