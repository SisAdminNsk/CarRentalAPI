using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceColumnToCarOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Patronymic",
                table: "CarsharingUsers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "CarOrders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Patronymic",
                table: "CarsharingUsers");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CarOrders");
        }
    }
}
