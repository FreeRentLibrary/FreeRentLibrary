using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FreeRentLibrary.Migrations
{
    public partial class AddedBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetailTemps_Products_ProductId",
                table: "OrderDetailTemps");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderDetailTemps",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetailTemps_ProductId",
                table: "OrderDetailTemps",
                newName: "IX_OrderDetailTemps_BookId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderDetails",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_BookId");

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Stock = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserId",
                table: "Books",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Books_BookId",
                table: "OrderDetails",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetailTemps_Books_BookId",
                table: "OrderDetailTemps",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Books_BookId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetailTemps_Books_BookId",
                table: "OrderDetailTemps");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Countries_Name",
                table: "Countries");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "OrderDetailTemps",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetailTemps_BookId",
                table: "OrderDetailTemps",
                newName: "IX_OrderDetailTemps_ProductId");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "OrderDetails",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_BookId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductId");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    LastPurchase = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastSale = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserId",
                table: "Products",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetailTemps_Products_ProductId",
                table: "OrderDetailTemps",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
