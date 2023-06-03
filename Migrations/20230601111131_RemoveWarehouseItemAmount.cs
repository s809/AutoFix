using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoFix.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWarehouseItemAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "WarehouseItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "WarehouseItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
