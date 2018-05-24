using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Yanz.Data.Migrations
{
    public partial class Model6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
