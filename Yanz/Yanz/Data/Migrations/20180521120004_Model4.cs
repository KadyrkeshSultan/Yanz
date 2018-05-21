using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Yanz.Data.Migrations
{
    public partial class Model4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionSets_Folders_FolderId",
                table: "QuestionSets");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionSets_Folders_FolderId",
                table: "QuestionSets",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionSets_Folders_FolderId",
                table: "QuestionSets");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionSets_Folders_FolderId",
                table: "QuestionSets",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
