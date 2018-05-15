using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Yanz.Data.Migrations
{
    public partial class FixModel3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folder_AspNetUsers_ApplicationUserId",
                table: "Folder");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionSets_Folder_FolderId",
                table: "QuestionSets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Folder",
                table: "Folder");

            migrationBuilder.RenameTable(
                name: "Folder",
                newName: "Folders");

            migrationBuilder.RenameIndex(
                name: "IX_Folder_ApplicationUserId",
                table: "Folders",
                newName: "IX_Folders_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Folders",
                table: "Folders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_AspNetUsers_ApplicationUserId",
                table: "Folders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionSets_Folders_FolderId",
                table: "QuestionSets",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_AspNetUsers_ApplicationUserId",
                table: "Folders");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionSets_Folders_FolderId",
                table: "QuestionSets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Folders",
                table: "Folders");

            migrationBuilder.RenameTable(
                name: "Folders",
                newName: "Folder");

            migrationBuilder.RenameIndex(
                name: "IX_Folders_ApplicationUserId",
                table: "Folder",
                newName: "IX_Folder_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Folder",
                table: "Folder",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Folder_AspNetUsers_ApplicationUserId",
                table: "Folder",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionSets_Folder_FolderId",
                table: "QuestionSets",
                column: "FolderId",
                principalTable: "Folder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
