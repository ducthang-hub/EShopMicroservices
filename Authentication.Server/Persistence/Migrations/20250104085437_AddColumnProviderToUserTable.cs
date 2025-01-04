using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication.Server.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnProviderToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Provider",
                schema: "authen",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Provider",
                schema: "authen",
                table: "AspNetUsers");
        }
    }
}
