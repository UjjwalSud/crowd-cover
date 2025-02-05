using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrowdCover.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class bettor3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StreamingRoomEvent_Events_EventId",
                table: "StreamingRoomEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamingRoomEvent_StreamingRooms_StreamingRoomId",
                table: "StreamingRoomEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StreamingRoomEvent",
                table: "StreamingRoomEvent");

            migrationBuilder.RenameTable(
                name: "StreamingRoomEvent",
                newName: "StreamingRoomEvents");

            migrationBuilder.RenameIndex(
                name: "IX_StreamingRoomEvent_EventId",
                table: "StreamingRoomEvents",
                newName: "IX_StreamingRoomEvents_EventId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Bettors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreamingRoomEvents",
                table: "StreamingRoomEvents",
                columns: new[] { "StreamingRoomId", "EventId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StreamingRoomEvents_Events_EventId",
                table: "StreamingRoomEvents",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StreamingRoomEvents_StreamingRooms_StreamingRoomId",
                table: "StreamingRoomEvents",
                column: "StreamingRoomId",
                principalTable: "StreamingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StreamingRoomEvents_Events_EventId",
                table: "StreamingRoomEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamingRoomEvents_StreamingRooms_StreamingRoomId",
                table: "StreamingRoomEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StreamingRoomEvents",
                table: "StreamingRoomEvents");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bettors");

            migrationBuilder.RenameTable(
                name: "StreamingRoomEvents",
                newName: "StreamingRoomEvent");

            migrationBuilder.RenameIndex(
                name: "IX_StreamingRoomEvents_EventId",
                table: "StreamingRoomEvent",
                newName: "IX_StreamingRoomEvent_EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreamingRoomEvent",
                table: "StreamingRoomEvent",
                columns: new[] { "StreamingRoomId", "EventId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StreamingRoomEvent_Events_EventId",
                table: "StreamingRoomEvent",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StreamingRoomEvent_StreamingRooms_StreamingRoomId",
                table: "StreamingRoomEvent",
                column: "StreamingRoomId",
                principalTable: "StreamingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
