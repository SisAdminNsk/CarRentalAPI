using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnCarIdToTableOpenCarOrders5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OpenCarOrders_CarId",
                table: "OpenCarOrders");

            migrationBuilder.DropColumn(
                name: "CarForeignKey",
                table: "OpenCarOrders");

            migrationBuilder.AddColumn<Guid>(
                name: "CarId",
                table: "ClosedCarOrders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenCarOrders_CarId",
                table: "OpenCarOrders",
                column: "CarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClosedCarOrders_CarId",
                table: "ClosedCarOrders",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClosedCarOrders_Cars_CarId",
                table: "ClosedCarOrders",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClosedCarOrders_Cars_CarId",
                table: "ClosedCarOrders");

            migrationBuilder.DropIndex(
                name: "IX_OpenCarOrders_CarId",
                table: "OpenCarOrders");

            migrationBuilder.DropIndex(
                name: "IX_ClosedCarOrders_CarId",
                table: "ClosedCarOrders");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "ClosedCarOrders");

            migrationBuilder.AddColumn<Guid>(
                name: "CarForeignKey",
                table: "OpenCarOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OpenCarOrders_CarId",
                table: "OpenCarOrders",
                column: "CarId");
        }
    }
}
