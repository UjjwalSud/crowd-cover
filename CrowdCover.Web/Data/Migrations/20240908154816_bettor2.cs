using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrowdCover.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class bettor2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StreamingRoomEvent",
                columns: table => new
                {
                    StreamingRoomId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamingRoomEvent", x => new { x.StreamingRoomId, x.EventId });
                    table.ForeignKey(
                        name: "FK_StreamingRoomEvent_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StreamingRoomEvent_StreamingRooms_StreamingRoomId",
                        column: x => x.StreamingRoomId,
                        principalTable: "StreamingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StreamingRoomEvent_EventId",
                table: "StreamingRoomEvent",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StreamingRoomEvent");
        }
    }
}
