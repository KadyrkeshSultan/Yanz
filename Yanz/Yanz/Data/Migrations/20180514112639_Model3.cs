using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Yanz.Data.Migrations
{
    public partial class Model3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_QuestionSets_QuestionSetId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_QuestionSetId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "QuestionSetId",
                table: "Sessions");

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "Questions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SessionId",
                table: "Questions",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Sessions_SessionId",
                table: "Questions",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Sessions_SessionId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_SessionId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "QuestionSetId",
                table: "Sessions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_QuestionSetId",
                table: "Sessions",
                column: "QuestionSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_QuestionSets_QuestionSetId",
                table: "Sessions",
                column: "QuestionSetId",
                principalTable: "QuestionSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
