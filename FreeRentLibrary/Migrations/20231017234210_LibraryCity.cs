using Microsoft.EntityFrameworkCore.Migrations;

namespace FreeRentLibrary.Migrations
{
    public partial class LibraryCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libraries_Cities_CityId",
                table: "Libraries");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Libraries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Libraries_Cities_CityId",
                table: "Libraries",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libraries_Cities_CityId",
                table: "Libraries");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Libraries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Libraries_Cities_CityId",
                table: "Libraries",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
