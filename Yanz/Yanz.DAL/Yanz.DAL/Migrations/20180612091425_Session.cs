using Microsoft.EntityFrameworkCore.Migrations;

namespace Yanz.DAL.Yanz.DAL.Migrations
{
    public partial class Session : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    FeedbackMode = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsArchived = table.Column<bool>(nullable: false),
                    IsOneAttempt = table.Column<bool>(nullable: false),
                    IsPartialGrading = table.Column<bool>(nullable: false),
                    IsRevealed = table.Column<bool>(nullable: false),
                    IsShuffleChoices = table.Column<bool>(nullable: false),
                    Mode = table.Column<string>(nullable: true),
                    PlatformOnly = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_AppUserId",
                table: "Sessions",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions");
        }
    }
}
