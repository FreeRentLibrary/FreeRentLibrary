using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FreeRentLibrary.Migrations
{
    public partial class BookCoverImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_Books_BookId",
                table: "BookEditions");

            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_BookTypes_BookTypeId",
                table: "BookEditions");

            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_Publishers_PublisherId",
                table: "BookEditions");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_BookEditions_PublisherId",
                table: "BookEditions");

            migrationBuilder.DropColumn(
                name: "Pages",
                table: "BookEditions");

            migrationBuilder.RenameColumn(
                name: "PublisherId",
                table: "BookEditions",
                newName: "PageCount");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ISBN",
                table: "BookEditions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookTypeId",
                table: "BookEditions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "BookEditions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookPublisherId",
                table: "BookEditions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CoverId",
                table: "BookEditions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookEditions_BookPublisherId",
                table: "BookEditions",
                column: "BookPublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_BookEditions_ISBN",
                table: "BookEditions",
                column: "ISBN",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookEditions_Books_BookId",
                table: "BookEditions",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookEditions_BookTypes_BookTypeId",
                table: "BookEditions",
                column: "BookTypeId",
                principalTable: "BookTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookEditions_Publishers_BookPublisherId",
                table: "BookEditions",
                column: "BookPublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_Books_BookId",
                table: "BookEditions");

            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_BookTypes_BookTypeId",
                table: "BookEditions");

            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_Publishers_BookPublisherId",
                table: "BookEditions");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_BookEditions_BookPublisherId",
                table: "BookEditions");

            migrationBuilder.DropIndex(
                name: "IX_BookEditions_ISBN",
                table: "BookEditions");

            migrationBuilder.DropColumn(
                name: "BookPublisherId",
                table: "BookEditions");

            migrationBuilder.DropColumn(
                name: "CoverId",
                table: "BookEditions");

            migrationBuilder.RenameColumn(
                name: "PageCount",
                table: "BookEditions",
                newName: "PublisherId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ISBN",
                table: "BookEditions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "BookTypeId",
                table: "BookEditions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "BookEditions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Pages",
                table: "BookEditions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_BookEditions_PublisherId",
                table: "BookEditions",
                column: "PublisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookEditions_Books_BookId",
                table: "BookEditions",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookEditions_BookTypes_BookTypeId",
                table: "BookEditions",
                column: "BookTypeId",
                principalTable: "BookTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookEditions_Publishers_PublisherId",
                table: "BookEditions",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
