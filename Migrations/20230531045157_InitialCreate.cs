using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutoFix.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PassportInfo = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
                    BaseSalary = table.Column<decimal>(type: "numeric", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndReason = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Manufacturer = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseProviders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ContactInfo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePayouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePayouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeePayouts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MasterId = table.Column<int>(type: "integer", nullable: false),
                    ClientName = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    FinishDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsCancelled = table.Column<bool>(type: "boolean", nullable: false),
                    Comments = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairOrders_Employees_MasterId",
                        column: x => x.MasterId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseRestocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    ProviderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseRestocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseRestocks_WarehouseItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "WarehouseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseRestocks_WarehouseProviders_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "WarehouseProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    ServiceId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    FinishDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsCancelled = table.Column<bool>(type: "boolean", nullable: false),
                    Comments = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceHistory_RepairOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "RepairOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceHistory_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseUses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HistoryEntryId = table.Column<int>(type: "integer", nullable: false),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseUses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseUses_ServiceHistory_HistoryEntryId",
                        column: x => x.HistoryEntryId,
                        principalTable: "ServiceHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseUses_WarehouseItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "WarehouseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayouts_EmployeeId",
                table: "EmployeePayouts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrders_MasterId",
                table: "RepairOrders",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistory_OrderId",
                table: "ServiceHistory",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistory_ServiceId",
                table: "ServiceHistory",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRestocks_ItemId",
                table: "WarehouseRestocks",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRestocks_ProviderId",
                table: "WarehouseRestocks",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseUses_HistoryEntryId",
                table: "WarehouseUses",
                column: "HistoryEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseUses_ItemId",
                table: "WarehouseUses",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeePayouts");

            migrationBuilder.DropTable(
                name: "WarehouseRestocks");

            migrationBuilder.DropTable(
                name: "WarehouseUses");

            migrationBuilder.DropTable(
                name: "WarehouseProviders");

            migrationBuilder.DropTable(
                name: "ServiceHistory");

            migrationBuilder.DropTable(
                name: "WarehouseItems");

            migrationBuilder.DropTable(
                name: "RepairOrders");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
