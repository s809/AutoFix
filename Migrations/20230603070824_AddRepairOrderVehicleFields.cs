using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoFix.Migrations
{
    /// <inheritdoc />
    public partial class AddRepairOrderVehicleFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "WarehouseRestocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClientPhoneNumber",
                table: "RepairOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VehicleManufacturer",
                table: "RepairOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VehicleModel",
                table: "RepairOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VehicleYear",
                table: "RepairOrders",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "WarehouseRestocks");

            migrationBuilder.DropColumn(
                name: "ClientPhoneNumber",
                table: "RepairOrders");

            migrationBuilder.DropColumn(
                name: "VehicleManufacturer",
                table: "RepairOrders");

            migrationBuilder.DropColumn(
                name: "VehicleModel",
                table: "RepairOrders");

            migrationBuilder.DropColumn(
                name: "VehicleYear",
                table: "RepairOrders");
        }
    }
}
