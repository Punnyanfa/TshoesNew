using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCSP.Models.Migrations
{
    /// <inheritdoc />
    public partial class RemovePriceFromDesignServiceAndRemoveManufacturerIdFromCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Manufacturers_ManufacturerId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ManufacturerId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "DesignServices");

            migrationBuilder.DropColumn(
                name: "ManufacturerId",
                table: "CartItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "DesignServices",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ManufacturerId",
                table: "CartItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ManufacturerId",
                table: "CartItems",
                column: "ManufacturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Manufacturers_ManufacturerId",
                table: "CartItems",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
