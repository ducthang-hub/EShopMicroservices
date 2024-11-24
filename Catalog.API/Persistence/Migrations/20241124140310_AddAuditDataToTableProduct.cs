using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.API.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditDataToTableProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                schema: "catalog",
                table: "Product",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUser",
                schema: "catalog",
                table: "Product",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                schema: "catalog",
                table: "Product",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedUser",
                schema: "catalog",
                table: "Product",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                schema: "catalog",
                table: "Product",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedUser",
                schema: "catalog",
                table: "Product",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                schema: "catalog",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CreateUser",
                schema: "catalog",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                schema: "catalog",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DeletedUser",
                schema: "catalog",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                schema: "catalog",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ModifiedUser",
                schema: "catalog",
                table: "Product");
        }
    }
}
