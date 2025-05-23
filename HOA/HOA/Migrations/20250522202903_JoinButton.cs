using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HOA.Migrations
{
    /// <inheritdoc />
    public partial class JoinButton : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipants_User_UserId1",
                table: "EventParticipants");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_EventParticipants_EventId",
                table: "EventParticipants");

            migrationBuilder.DropIndex(
                name: "IX_EventParticipants_UserId1",
                table: "EventParticipants");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "EventParticipants");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "EventParticipants",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_EventId_UserId",
                table: "EventParticipants",
                columns: new[] { "EventId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventParticipants_EventId_UserId",
                table: "EventParticipants");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "EventParticipants",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "EventParticipants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_EventId",
                table: "EventParticipants",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_UserId1",
                table: "EventParticipants",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipants_User_UserId1",
                table: "EventParticipants",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
