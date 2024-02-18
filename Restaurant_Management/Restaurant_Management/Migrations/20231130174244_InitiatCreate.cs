using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant_Management.Migrations
{
    /// <inheritdoc />
    public partial class InitiatCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    FoodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.FoodId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "money", nullable: false),
                    FoodId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "FoodId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Foods",
                columns: new[] { "FoodId", "Category", "Description", "FoodName", "IsAvailable", "Picture", "Price" },
                values: new object[,]
                {
                    { 1, 4, "ABC1", "Food 1", true, "1.jpg", 1270.00m },
                    { 2, 1, "ABC2", "Food 2", true, "2.jpg", 1297.00m },
                    { 3, 2, "ABC3", "Food 3", true, "3.jpg", 1421.00m },
                    { 4, 4, "ABC4", "Food 4", true, "4.jpg", 1498.00m },
                    { 5, 2, "ABC5", "Food 5", true, "5.jpg", 1447.00m }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "Address", "CustomerName", "Email", "FoodId", "OrderDate", "Phone", "Quantity", "TotalAmount" },
                values: new object[,]
                {
                    { 1, "Address 1", "Customer 1", "customer1@example.com", 1, new DateTime(2022, 8, 17, 23, 42, 44, 486, DateTimeKind.Local).AddTicks(5170), 580456418, 289, 1579.00m },
                    { 2, "Address 2", "Customer 2", "customer2@example.com", 2, new DateTime(2022, 12, 22, 23, 42, 44, 486, DateTimeKind.Local).AddTicks(5259), 538088507, 140, 1271.00m },
                    { 3, "Address 3", "Customer 3", "customer3@example.com", 3, new DateTime(2022, 8, 8, 23, 42, 44, 486, DateTimeKind.Local).AddTicks(5320), 703407400, 201, 1207.00m },
                    { 4, "Address 4", "Customer 4", "customer4@example.com", 4, new DateTime(2022, 10, 2, 23, 42, 44, 486, DateTimeKind.Local).AddTicks(5382), 248482140, 280, 1569.00m },
                    { 5, "Address 5", "Customer 5", "customer5@example.com", 5, new DateTime(2023, 3, 13, 23, 42, 44, 486, DateTimeKind.Local).AddTicks(5443), 781364099, 162, 1346.00m },
                    { 6, "Address 6", "Customer 6", "customer6@example.com", 1, new DateTime(2023, 1, 22, 23, 42, 44, 486, DateTimeKind.Local).AddTicks(5482), 654247633, 232, 1600.00m },
                    { 7, "Address 7", "Customer 7", "customer7@example.com", 2, new DateTime(2023, 3, 9, 23, 42, 44, 486, DateTimeKind.Local).AddTicks(5518), 864943268, 197, 1077.00m },
                    { 8, "Address 8", "Customer 8", "customer8@example.com", 3, new DateTime(2022, 9, 10, 23, 42, 44, 486, DateTimeKind.Local).AddTicks(5580), 341199118, 252, 1902.00m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_FoodId",
                table: "Orders",
                column: "FoodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Foods");
        }
    }
}
