using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrowdCover.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class booksEvent4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StreamingRoomBook_Books_BookId",
                table: "StreamingRoomBook");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamingRoomBook_StreamingRooms_StreamingRoomId",
                table: "StreamingRoomBook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StreamingRoomBook",
                table: "StreamingRoomBook");

            migrationBuilder.RenameTable(
                name: "StreamingRoomBook",
                newName: "StreamingRoomBooks");

            migrationBuilder.RenameIndex(
                name: "IX_StreamingRoomBook_BookId",
                table: "StreamingRoomBooks",
                newName: "IX_StreamingRoomBooks_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreamingRoomBooks",
                table: "StreamingRoomBooks",
                columns: new[] { "StreamingRoomId", "BookId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StreamingRoomBooks_Books_BookId",
                table: "StreamingRoomBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StreamingRoomBooks_StreamingRooms_StreamingRoomId",
                table: "StreamingRoomBooks",
                column: "StreamingRoomId",
                principalTable: "StreamingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StreamingRoomBooks_Books_BookId",
                table: "StreamingRoomBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamingRoomBooks_StreamingRooms_StreamingRoomId",
                table: "StreamingRoomBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StreamingRoomBooks",
                table: "StreamingRoomBooks");

            migrationBuilder.RenameTable(
                name: "StreamingRoomBooks",
                newName: "StreamingRoomBook");

            migrationBuilder.RenameIndex(
                name: "IX_StreamingRoomBooks_BookId",
                table: "StreamingRoomBook",
                newName: "IX_StreamingRoomBook_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreamingRoomBook",
                table: "StreamingRoomBook",
                columns: new[] { "StreamingRoomId", "BookId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StreamingRoomBook_Books_BookId",
                table: "StreamingRoomBook",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StreamingRoomBook_StreamingRooms_StreamingRoomId",
                table: "StreamingRoomBook",
                column: "StreamingRoomId",
                principalTable: "StreamingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
