using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCSP.Models.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomShoeDesigns_Sizes_SizeId",
                table: "CustomShoeDesigns");

            migrationBuilder.AlterColumn<long>(
                name: "SizeId",
                table: "CustomShoeDesigns",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomShoeDesigns_Sizes_SizeId",
                table: "CustomShoeDesigns",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomShoeDesigns_Sizes_SizeId",
                table: "CustomShoeDesigns");

            migrationBuilder.AlterColumn<long>(
                name: "SizeId",
                table: "CustomShoeDesigns",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomShoeDesigns_Sizes_SizeId",
                table: "CustomShoeDesigns",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
