using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Yanz.Data.Migrations
{
    public partial class Model10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SetId",
                table: "Questions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ModerMsgs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false),
                    QuestionSetId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModerMsgs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModerMsgs_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModerMsgs_QuestionSets_QuestionSetId",
                        column: x => x.QuestionSetId,
                        principalTable: "QuestionSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    CopyCount = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Desc = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sets_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SetId",
                table: "Questions",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_ModerMsgs_ApplicationUserId",
                table: "ModerMsgs",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModerMsgs_QuestionSetId",
                table: "ModerMsgs",
                column: "QuestionSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_ApplicationUserId",
                table: "Sets",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Sets_SetId",
                table: "Questions",
                column: "SetId",
                principalTable: "Sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Sets_SetId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "ModerMsgs");

            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_Questions_SetId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "SetId",
                table: "Questions");
        }
    }
}
