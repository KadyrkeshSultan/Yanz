using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Yanz.Data.Migrations
{
    public partial class Model2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Choice_Questions_QuestionId",
                table: "Choice");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "SessionQstView");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Choice",
                table: "Choice");

            migrationBuilder.RenameTable(
                name: "Choice",
                newName: "Choices");

            migrationBuilder.RenameIndex(
                name: "IX_Choice_QuestionId",
                table: "Choices",
                newName: "IX_Choices_QuestionId");

            migrationBuilder.AddColumn<string>(
                name: "QuestionSetId",
                table: "Sessions",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Choices",
                table: "Choices",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_QuestionSetId",
                table: "Sessions",
                column: "QuestionSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Choices_Questions_QuestionId",
                table: "Choices",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_QuestionSets_QuestionSetId",
                table: "Sessions",
                column: "QuestionSetId",
                principalTable: "QuestionSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Choices_Questions_QuestionId",
                table: "Choices");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_QuestionSets_QuestionSetId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_QuestionSetId",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Choices",
                table: "Choices");

            migrationBuilder.DropColumn(
                name: "QuestionSetId",
                table: "Sessions");

            migrationBuilder.RenameTable(
                name: "Choices",
                newName: "Choice");

            migrationBuilder.RenameIndex(
                name: "IX_Choices_QuestionId",
                table: "Choice",
                newName: "IX_Choice_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Choice",
                table: "Choice",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SessionQstView",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsPoll = table.Column<bool>(nullable: false),
                    IsTrueCorrect = table.Column<bool>(nullable: true),
                    Kind = table.Column<string>(nullable: true),
                    OnBoardingId = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    SessionId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Weight = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionQstView", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionQstView_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Kind = table.Column<string>(nullable: true),
                    QuestionsCount = table.Column<int>(nullable: true),
                    SessionQstViewId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_SessionQstView_SessionQstViewId",
                        column: x => x.SessionQstViewId,
                        principalTable: "SessionQstView",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Item_SessionQstViewId",
                table: "Item",
                column: "SessionQstViewId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionQstView_SessionId",
                table: "SessionQstView",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Choice_Questions_QuestionId",
                table: "Choice",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
