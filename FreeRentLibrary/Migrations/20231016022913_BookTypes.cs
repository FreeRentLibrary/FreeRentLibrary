using Microsoft.EntityFrameworkCore.Migrations;

namespace FreeRentLibrary.Migrations
{
    public partial class BookTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditionType",
                table: "BookEditions");

            migrationBuilder.AddColumn<int>(
                name: "BookTypeId",
                table: "BookEditions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookEditions_BookTypeId",
                table: "BookEditions",
                column: "BookTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookEditions_BookTypes_BookTypeId",
                table: "BookEditions",
                column: "BookTypeId",
                principalTable: "BookTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_BookTypes_BookTypeId",
                table: "BookEditions");

            migrationBuilder.DropTable(
                name: "BookTypes");

            migrationBuilder.DropIndex(
                name: "IX_BookEditions_BookTypeId",
                table: "BookEditions");

            migrationBuilder.DropColumn(
                name: "BookTypeId",
                table: "BookEditions");

            migrationBuilder.AddColumn<string>(
                name: "EditionType",
                table: "BookEditions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
