using Microsoft.EntityFrameworkCore.Migrations;

namespace FreeRentLibrary.Migrations
{
    public partial class PublisherUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publishers_Countries_CountryId",
                table: "Publishers");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Publishers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Publishers_Countries_CountryId",
                table: "Publishers",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publishers_Countries_CountryId",
                table: "Publishers");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Publishers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Publishers_Countries_CountryId",
                table: "Publishers",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
