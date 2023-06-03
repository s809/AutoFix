using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoFix.Migrations
{
    /// <inheritdoc />
    public partial class MoveWarehouseUses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseUses_ServiceHistory_HistoryEntryId",
                table: "WarehouseUses");

            migrationBuilder.RenameColumn(
                name: "HistoryEntryId",
                table: "WarehouseUses",
                newName: "RepairOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseUses_HistoryEntryId",
                table: "WarehouseUses",
                newName: "IX_WarehouseUses_RepairOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseUses_RepairOrders_RepairOrderId",
                table: "WarehouseUses",
                column: "RepairOrderId",
                principalTable: "RepairOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseUses_RepairOrders_RepairOrderId",
                table: "WarehouseUses");

            migrationBuilder.RenameColumn(
                name: "RepairOrderId",
                table: "WarehouseUses",
                newName: "HistoryEntryId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseUses_RepairOrderId",
                table: "WarehouseUses",
                newName: "IX_WarehouseUses_HistoryEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseUses_ServiceHistory_HistoryEntryId",
                table: "WarehouseUses",
                column: "HistoryEntryId",
                principalTable: "ServiceHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
