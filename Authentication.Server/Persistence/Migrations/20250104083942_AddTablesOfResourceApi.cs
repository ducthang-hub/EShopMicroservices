using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication.Server.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTablesOfResourceApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "authen",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "authen",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "authen",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "authen",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "authen",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "authen",
                newName: "AspNetRoleClaims");

            migrationBuilder.CreateTable(
                name: "ApiResource",
                schema: "authen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Secret = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiResource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiScope",
                schema: "authen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiScope", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                schema: "authen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiScopeResource",
                schema: "authen",
                columns: table => new
                {
                    ApiResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApiScopeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiScopeResource", x => new { x.ApiScopeId, x.ApiResourceId });
                    table.ForeignKey(
                        name: "FK_ApiScopeResource_ApiResource_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalSchema: "authen",
                        principalTable: "ApiResource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApiScopeResource_ApiScope_ApiScopeId",
                        column: x => x.ApiScopeId,
                        principalSchema: "authen",
                        principalTable: "ApiScope",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientGrantType",
                schema: "authen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GrantType = table.Column<string>(type: "text", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientGrantType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientGrantType_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "authen",
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientScope",
                schema: "authen",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApiScopeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientScope", x => new { x.ClientId, x.ApiScopeId });
                    table.ForeignKey(
                        name: "FK_ClientScope_ApiScope_ApiScopeId",
                        column: x => x.ApiScopeId,
                        principalSchema: "authen",
                        principalTable: "ApiScope",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientScope_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "authen",
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientSecret",
                schema: "authen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Secret = table.Column<string>(type: "text", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSecret", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientSecret_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "authen",
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiScopeResource_ApiResourceId",
                schema: "authen",
                table: "ApiScopeResource",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientGrantType_ClientId",
                schema: "authen",
                table: "ClientGrantType",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientScope_ApiScopeId",
                schema: "authen",
                table: "ClientScope",
                column: "ApiScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSecret_ClientId",
                schema: "authen",
                table: "ClientSecret",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiScopeResource",
                schema: "authen");

            migrationBuilder.DropTable(
                name: "ClientGrantType",
                schema: "authen");

            migrationBuilder.DropTable(
                name: "ClientScope",
                schema: "authen");

            migrationBuilder.DropTable(
                name: "ClientSecret",
                schema: "authen");

            migrationBuilder.DropTable(
                name: "ApiResource",
                schema: "authen");

            migrationBuilder.DropTable(
                name: "ApiScope",
                schema: "authen");

            migrationBuilder.DropTable(
                name: "Client",
                schema: "authen");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "authen");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "authen");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "authen");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "authen");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "authen");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "authen");
        }
    }
}
