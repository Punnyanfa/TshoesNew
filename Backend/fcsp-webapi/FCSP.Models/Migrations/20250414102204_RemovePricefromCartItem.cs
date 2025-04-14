using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCSP.Models.Migrations
{
    /// <inheritdoc />
    public partial class RemovePricefromCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "CartItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "CartItems",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
