using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCSP.Models.Migrations
{
    /// <inheritdoc />
    public partial class updateManuServiceOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceName",
                table: "Services",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Manufacturers",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Component",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "ManufacturerId",
                table: "OrderDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ManufacturerId",
                table: "OrderDetails",
                column: "ManufacturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Manufacturers_ManufacturerId",
                table: "OrderDetails",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Manufacturers_ManufacturerId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ManufacturerId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Component",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ManufacturerId",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Services",
                newName: "ServiceName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Manufacturers",
                newName: "Name");
        }
    }
}
