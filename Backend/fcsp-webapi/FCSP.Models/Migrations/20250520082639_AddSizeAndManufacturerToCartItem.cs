using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCSP.Models.Migrations
{
    /// <inheritdoc />
    public partial class AddSizeAndManufacturerToCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ManufacturerId",
                table: "CartItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SizeId",
                table: "CartItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ManufacturerId",
                table: "CartItems",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_SizeId",
                table: "CartItems",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Manufacturers_ManufacturerId",
                table: "CartItems",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Sizes_SizeId",
                table: "CartItems",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Manufacturers_ManufacturerId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Sizes_SizeId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ManufacturerId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_SizeId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ManufacturerId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "CartItems");
        }
    }
}
