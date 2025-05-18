using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCSP.Models.Migrations
{
    /// <inheritdoc />
    public partial class removeTotalAmountFromCustomShoeDesign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "CustomShoeDesigns");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalAmount",
                table: "CustomShoeDesigns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
