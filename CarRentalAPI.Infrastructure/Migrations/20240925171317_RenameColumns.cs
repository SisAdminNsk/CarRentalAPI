using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarOrders_CarsharingUsers_CustomerId",
                table: "CarOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ClosedCarOrders_Cars_CarId",
                table: "ClosedCarOrders");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "CarOrders",
                newName: "CarsharingUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CarOrders_CustomerId",
                table: "CarOrders",
                newName: "IX_CarOrders_CarsharingUserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CarId",
                table: "ClosedCarOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarOrders_CarsharingUsers_CarsharingUserId",
                table: "CarOrders",
                column: "CarsharingUserId",
                principalTable: "CarsharingUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClosedCarOrders_Cars_CarId",
                table: "ClosedCarOrders",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarOrders_CarsharingUsers_CarsharingUserId",
                table: "CarOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ClosedCarOrders_Cars_CarId",
                table: "ClosedCarOrders");

            migrationBuilder.RenameColumn(
                name: "CarsharingUserId",
                table: "CarOrders",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_CarOrders_CarsharingUserId",
                table: "CarOrders",
                newName: "IX_CarOrders_CustomerId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CarId",
                table: "ClosedCarOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_CarOrders_CarsharingUsers_CustomerId",
                table: "CarOrders",
                column: "CustomerId",
                principalTable: "CarsharingUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClosedCarOrders_Cars_CarId",
                table: "ClosedCarOrders",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");
        }
    }
}
