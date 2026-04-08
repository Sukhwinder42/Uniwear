using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Uniwear.Migrations
{
    /// <inheritdoc />
    public partial class GuidtoInt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Clothing" },
                    { 2, "Accessories" },
                    { 3, "Bags" },
                    { 4, "Headwear" },
                    { 5, "Loungewear" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[,]
                {
                    { 1, 1, "A comfortable and stylish hoodie for university students.", "https://in.puma.com/in/en/pd/essentials-no--1-logo-mens-comfort-hoodie/682571?size=0140&swatch=55&utm_source=BING-DDA&utm_medium=DSP&utm_campaign=DSP_BING_DDA_IN_PMAX_agency_1000067495857508873&msclkid=2707980aa4df1e51bf0bf196b51250c6", "Uniwear Hoodie", 39.99m, 100 },
                    { 2, 2, "A cool t-shirt with the Uniwear logo.", "https://sl.bing.net/fw3YOvHs6H6", "Uniwear T-Shirt", 19.99m, 200 },
                    { 3, 3, "A durable backpack perfect for carrying your books and laptop.", "https://images.puma.com/image/upload/f_auto,q_auto,b_rgb:fafafa,w_750,h_750/global/091993/01/fnd/IND/fmt/png/Archive-KING-Backpack-28L", "Uniwear Backpack", 49.99m, 150 },
                    { 4, 4, "A stylish cap to complete your university look.", "https://sl.bing.net/eEj6P0lzOc8", "Uniwear Cap", 14.99m, 300 },
                    { 5, 5, "Comfortable sweatpants for lounging or studying.", "https://images.puma.com/image/upload/f_auto,q_auto,b_rgb:fafafa,w_750,h_750/global/691700/16/mod01/fnd/IND/fmt/png/Essentials-Block-Men's-Knitted-Sweatpants", "Uniwear Sweatpants", 29.99m, 120 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
