using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PutOrderStatusIntoOneTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClosedCarOrders");

            migrationBuilder.DropTable(
                name: "OpenCarOrders");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CarOrders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "CarOrders");

            migrationBuilder.CreateTable(
                name: "ClosedCarOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CarOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CarId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosedCarOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClosedCarOrders_CarOrders_CarOrderId",
                        column: x => x.CarOrderId,
                        principalTable: "CarOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClosedCarOrders_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenCarOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CarId = table.Column<Guid>(type: "uuid", nullable: false),
                    CarOrderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenCarOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenCarOrders_CarOrders_CarOrderId",
                        column: x => x.CarOrderId,
                        principalTable: "CarOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpenCarOrders_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClosedCarOrders_CarId",
                table: "ClosedCarOrders",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_ClosedCarOrders_CarOrderId",
                table: "ClosedCarOrders",
                column: "CarOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenCarOrders_CarId",
                table: "OpenCarOrders",
                column: "CarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenCarOrders_CarOrderId",
                table: "OpenCarOrders",
                column: "CarOrderId");
        }
    }
}
