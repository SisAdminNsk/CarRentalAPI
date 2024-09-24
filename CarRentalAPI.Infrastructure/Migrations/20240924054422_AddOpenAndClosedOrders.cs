using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOpenAndClosedOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CarsharingUsers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "CarsharingUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "CarsharingUsers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Сomment",
                table: "CarOrders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ClosedCarOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CarOrderId = table.Column<Guid>(type: "uuid", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "OpenCarOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClosedCarOrders_CarOrderId",
                table: "ClosedCarOrders",
                column: "CarOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenCarOrders_CarOrderId",
                table: "OpenCarOrders",
                column: "CarOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClosedCarOrders");

            migrationBuilder.DropTable(
                name: "OpenCarOrders");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "CarsharingUsers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "CarsharingUsers");

            migrationBuilder.DropColumn(
                name: "Сomment",
                table: "CarOrders");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CarsharingUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
