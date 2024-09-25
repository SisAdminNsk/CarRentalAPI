using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnCarIdToTableOpenCarOrder4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CarForeignKey",
                table: "OpenCarOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CarId",
                table: "OpenCarOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OpenCarOrders_CarId",
                table: "OpenCarOrders",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenCarOrders_Cars_CarId",
                table: "OpenCarOrders",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenCarOrders_Cars_CarId",
                table: "OpenCarOrders");

            migrationBuilder.DropIndex(
                name: "IX_OpenCarOrders_CarId",
                table: "OpenCarOrders");

            migrationBuilder.DropColumn(
                name: "CarForeignKey",
                table: "OpenCarOrders");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "OpenCarOrders");
        }
    }
}
