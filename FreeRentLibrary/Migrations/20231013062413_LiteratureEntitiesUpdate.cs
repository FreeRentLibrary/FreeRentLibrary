using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FreeRentLibrary.Migrations
{
    public partial class LiteratureEntitiesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Editions_Books_BookId",
                table: "Editions");

            migrationBuilder.DropForeignKey(
                name: "FK_Editions_Publishers_PublisherId",
                table: "Editions");

            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Authors_AuthorId",
                table: "Genres");

            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Books_BookId",
                table: "Genres");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryStock_Editions_BookEditionId",
                table: "LibraryStock");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryStock_Libraries_LibraryId",
                table: "LibraryStock");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Editions_BookEditionId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Editions_BookEditionId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Genres_AuthorId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_BookId",
                table: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LibraryStock",
                table: "LibraryStock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Editions",
                table: "Editions");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PenName",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "NativeLanguage",
                table: "Editions");

            migrationBuilder.RenameTable(
                name: "LibraryStock",
                newName: "LibraryStocks");

            migrationBuilder.RenameTable(
                name: "Editions",
                newName: "BookEditions");

            migrationBuilder.RenameIndex(
                name: "IX_LibraryStock_LibraryId",
                table: "LibraryStocks",
                newName: "IX_LibraryStocks_LibraryId");

            migrationBuilder.RenameIndex(
                name: "IX_LibraryStock_BookEditionId",
                table: "LibraryStocks",
                newName: "IX_LibraryStocks_BookEditionId");

            migrationBuilder.RenameIndex(
                name: "IX_Editions_PublisherId",
                table: "BookEditions",
                newName: "IX_BookEditions_PublisherId");

            migrationBuilder.RenameIndex(
                name: "IX_Editions_BookId",
                table: "BookEditions",
                newName: "IX_BookEditions_BookId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Genres",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "NativeLanguage",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "BookEditions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "Pages",
                table: "BookEditions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MinimumAge",
                table: "BookEditions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LibraryStocks",
                table: "LibraryStocks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookEditions",
                table: "BookEditions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AuthorGenre",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "int", nullable: false),
                    GenresId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorGenre", x => new { x.AuthorsId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_AuthorGenre_Authors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorGenre_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookGenre",
                columns: table => new
                {
                    BooksId = table.Column<int>(type: "int", nullable: false),
                    GenresId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenre", x => new { x.BooksId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_BookGenre_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookGenre_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorGenre_GenresId",
                table: "AuthorGenre",
                column: "GenresId");

            migrationBuilder.CreateIndex(
                name: "IX_BookGenre_GenresId",
                table: "BookGenre",
                column: "GenresId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookEditions_Books_BookId",
                table: "BookEditions",
                column: "BookId",
                principalTable: "Books",
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
                name: "FK_LibraryStocks_BookEditions_BookEditionId",
                table: "LibraryStocks",
                column: "BookEditionId",
                principalTable: "BookEditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryStocks_Libraries_LibraryId",
                table: "LibraryStocks",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_BookEditions_BookEditionId",
                table: "Rentals",
                column: "BookEditionId",
                principalTable: "BookEditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_BookEditions_BookEditionId",
                table: "Reservations",
                column: "BookEditionId",
                principalTable: "BookEditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_Books_BookId",
                table: "BookEditions");

            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_Publishers_PublisherId",
                table: "BookEditions");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryStocks_BookEditions_BookEditionId",
                table: "LibraryStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryStocks_Libraries_LibraryId",
                table: "LibraryStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_BookEditions_BookEditionId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_BookEditions_BookEditionId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "AuthorGenre");

            migrationBuilder.DropTable(
                name: "BookGenre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LibraryStocks",
                table: "LibraryStocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookEditions",
                table: "BookEditions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "NativeLanguage",
                table: "Books");

            migrationBuilder.RenameTable(
                name: "LibraryStocks",
                newName: "LibraryStock");

            migrationBuilder.RenameTable(
                name: "BookEditions",
                newName: "Editions");

            migrationBuilder.RenameIndex(
                name: "IX_LibraryStocks_LibraryId",
                table: "LibraryStock",
                newName: "IX_LibraryStock_LibraryId");

            migrationBuilder.RenameIndex(
                name: "IX_LibraryStocks_BookEditionId",
                table: "LibraryStock",
                newName: "IX_LibraryStock_BookEditionId");

            migrationBuilder.RenameIndex(
                name: "IX_BookEditions_PublisherId",
                table: "Editions",
                newName: "IX_Editions_PublisherId");

            migrationBuilder.RenameIndex(
                name: "IX_BookEditions_BookId",
                table: "Editions",
                newName: "IX_Editions_BookId");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Genres",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Genres",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Books",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Books",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PenName",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Editions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Pages",
                table: "Editions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MinimumAge",
                table: "Editions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NativeLanguage",
                table: "Editions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LibraryStock",
                table: "LibraryStock",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Editions",
                table: "Editions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_AuthorId",
                table: "Genres",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_BookId",
                table: "Genres",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Editions_Books_BookId",
                table: "Editions",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Editions_Publishers_PublisherId",
                table: "Editions",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Authors_AuthorId",
                table: "Genres",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Books_BookId",
                table: "Genres",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryStock_Editions_BookEditionId",
                table: "LibraryStock",
                column: "BookEditionId",
                principalTable: "Editions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryStock_Libraries_LibraryId",
                table: "LibraryStock",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Editions_BookEditionId",
                table: "Rentals",
                column: "BookEditionId",
                principalTable: "Editions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Editions_BookEditionId",
                table: "Reservations",
                column: "BookEditionId",
                principalTable: "Editions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
