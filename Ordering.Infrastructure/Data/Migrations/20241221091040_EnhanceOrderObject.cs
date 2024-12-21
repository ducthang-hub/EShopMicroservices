using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceOrderObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "order",
                table: "OrderItem",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "order",
                table: "Order",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "order",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "order",
                table: "Order");
        }
    }
}
