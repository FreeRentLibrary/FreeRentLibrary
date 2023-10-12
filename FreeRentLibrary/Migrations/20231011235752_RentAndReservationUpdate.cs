using Microsoft.EntityFrameworkCore.Migrations;

namespace FreeRentLibrary.Migrations
{
    public partial class RentAndReservationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Books_BookId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Books_BookId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Reservations",
                newName: "LibraryId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_BookId",
                table: "Reservations",
                newName: "IX_Reservations_LibraryId");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Rentals",
                newName: "LibraryId");

            migrationBuilder.RenameIndex(
                name: "IX_Rentals_BookId",
                table: "Rentals",
                newName: "IX_Rentals_LibraryId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Editions",
                newName: "Translator");

            migrationBuilder.AddColumn<int>(
                name: "BookEditionId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookEditionId",
                table: "Rentals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditionName",
                table: "Editions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditionType",
                table: "Editions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "Editions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinimumAge",
                table: "Editions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NativeLanguage",
                table: "Editions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Pages",
                table: "Editions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TranslatedLanguage",
                table: "Editions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Synopsis",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LibraryStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LibraryId = table.Column<int>(type: "int", nullable: true),
                    BookEditionId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryStock_Editions_BookEditionId",
                        column: x => x.BookEditionId,
                        principalTable: "Editions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LibraryStock_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_BookEditionId",
                table: "Reservations",
                column: "BookEditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_BookEditionId",
                table: "Rentals",
                column: "BookEditionId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryStock_BookEditionId",
                table: "LibraryStock",
                column: "BookEditionId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryStock_LibraryId",
                table: "LibraryStock",
                column: "LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Editions_BookEditionId",
                table: "Rentals",
                column: "BookEditionId",
                principalTable: "Editions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Libraries_LibraryId",
                table: "Rentals",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Editions_BookEditionId",
                table: "Reservations",
                column: "BookEditionId",
                principalTable: "Editions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Libraries_LibraryId",
                table: "Reservations",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Editions_BookEditionId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Libraries_LibraryId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Editions_BookEditionId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Libraries_LibraryId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "LibraryStock");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_BookEditionId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_BookEditionId",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "BookEditionId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "BookEditionId",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "EditionName",
                table: "Editions");

            migrationBuilder.DropColumn(
                name: "EditionType",
                table: "Editions");

            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "Editions");

            migrationBuilder.DropColumn(
                name: "MinimumAge",
                table: "Editions");

            migrationBuilder.DropColumn(
                name: "NativeLanguage",
                table: "Editions");

            migrationBuilder.DropColumn(
                name: "Pages",
                table: "Editions");

            migrationBuilder.DropColumn(
                name: "TranslatedLanguage",
                table: "Editions");

            migrationBuilder.DropColumn(
                name: "Synopsis",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "LibraryId",
                table: "Reservations",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_LibraryId",
                table: "Reservations",
                newName: "IX_Reservations_BookId");

            migrationBuilder.RenameColumn(
                name: "LibraryId",
                table: "Rentals",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Rentals_LibraryId",
                table: "Rentals",
                newName: "IX_Rentals_BookId");

            migrationBuilder.RenameColumn(
                name: "Translator",
                table: "Editions",
                newName: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Books_BookId",
                table: "Rentals",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Books_BookId",
                table: "Reservations",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
