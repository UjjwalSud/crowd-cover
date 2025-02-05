using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrowdCover.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class booksEvent1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Book_Abbr",
                table: "BettorAccounts");

            migrationBuilder.DropColumn(
                name: "Book_Id",
                table: "BettorAccounts");

            migrationBuilder.DropColumn(
                name: "Book_Name",
                table: "BettorAccounts");

            migrationBuilder.DropColumn(
                name: "Book_Abbr",
                table: "BetSlips");

            migrationBuilder.DropColumn(
                name: "Book_Id",
                table: "BetSlips");

            migrationBuilder.DropColumn(
                name: "Book_Name",
                table: "BetSlips");

            migrationBuilder.AddColumn<string>(
                name: "BookId",
                table: "BettorAccounts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookId",
                table: "BetSlips",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Abbr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventBook",
                columns: table => new
                {
                    EventId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventBook", x => new { x.EventId, x.BookId });
                    table.ForeignKey(
                        name: "FK_EventBook_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventBook_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BettorAccounts_BookId",
                table: "BettorAccounts",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BetSlips_BookId",
                table: "BetSlips",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_EventBook_BookId",
                table: "EventBook",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BetSlips_Books_BookId",
                table: "BetSlips",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BettorAccounts_Books_BookId",
                table: "BettorAccounts",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BetSlips_Books_BookId",
                table: "BetSlips");

            migrationBuilder.DropForeignKey(
                name: "FK_BettorAccounts_Books_BookId",
                table: "BettorAccounts");

            migrationBuilder.DropTable(
                name: "EventBook");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropIndex(
                name: "IX_BettorAccounts_BookId",
                table: "BettorAccounts");

            migrationBuilder.DropIndex(
                name: "IX_BetSlips_BookId",
                table: "BetSlips");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "BettorAccounts");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "BetSlips");

            migrationBuilder.AddColumn<string>(
                name: "Book_Abbr",
                table: "BettorAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Book_Id",
                table: "BettorAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Book_Name",
                table: "BettorAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Book_Abbr",
                table: "BetSlips",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Book_Id",
                table: "BetSlips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Book_Name",
                table: "BetSlips",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
