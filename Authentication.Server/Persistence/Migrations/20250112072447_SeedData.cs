using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Authentication.Server.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "authen",
                table: "ApiResource",
                columns: new[] { "Id", "Name", "Secret" },
                values: new object[,]
                {
                    { new Guid("5b9aaeb4-d86c-4b2b-91ff-6df237697b77"), "customer-api-resource", new Guid("efd456ce-d33b-49f7-bb18-9019338fa79a") },
                    { new Guid("5ed18100-4476-4fd4-ac74-4218e939e3fe"), "shop-owner-api-resource", new Guid("9c232a28-07ae-4405-96e0-94e57ffb13b0") }
                });

            migrationBuilder.InsertData(
                schema: "authen",
                table: "ApiScope",
                columns: new[] { "Id", "DisplayName", "Name" },
                values: new object[,]
                {
                    { new Guid("75f824ab-b599-4519-8bcc-37bea9ad7cd9"), "Student Api Scope", "customer-scope" },
                    { new Guid("7b3b5167-24a4-41e0-8648-48f7f02dd44b"), "Teacher Api Scope", "shop-owner-scope" }
                });

            migrationBuilder.InsertData(
                schema: "authen",
                table: "Client",
                columns: new[] { "Id", "ClientId" },
                values: new object[,]
                {
                    { new Guid("c49ab291-6e10-49bc-a796-848f22a4936f"), "eshop-mobile" },
                    { new Guid("d120b22d-85e5-4e6f-80fe-ec49e2138d3c"), "eshop-web" }
                });

            migrationBuilder.InsertData(
                schema: "authen",
                table: "ApiScopeResource",
                columns: new[] { "ApiResourceId", "ApiScopeId" },
                values: new object[,]
                {
                    { new Guid("5b9aaeb4-d86c-4b2b-91ff-6df237697b77"), new Guid("75f824ab-b599-4519-8bcc-37bea9ad7cd9") },
                    { new Guid("5ed18100-4476-4fd4-ac74-4218e939e3fe"), new Guid("7b3b5167-24a4-41e0-8648-48f7f02dd44b") }
                });

            migrationBuilder.InsertData(
                schema: "authen",
                table: "ClientGrantType",
                columns: new[] { "Id", "ClientId", "GrantType" },
                values: new object[] { new Guid("eb3cea84-46d3-428f-b5ea-5c42b366c60f"), new Guid("d120b22d-85e5-4e6f-80fe-ec49e2138d3c"), "password" });

            migrationBuilder.InsertData(
                schema: "authen",
                table: "ClientScope",
                columns: new[] { "ApiScopeId", "ClientId" },
                values: new object[,]
                {
                    { new Guid("7b3b5167-24a4-41e0-8648-48f7f02dd44b"), new Guid("c49ab291-6e10-49bc-a796-848f22a4936f") },
                    { new Guid("75f824ab-b599-4519-8bcc-37bea9ad7cd9"), new Guid("d120b22d-85e5-4e6f-80fe-ec49e2138d3c") }
                });

            migrationBuilder.InsertData(
                schema: "authen",
                table: "ClientSecret",
                columns: new[] { "Id", "ClientId", "Secret" },
                values: new object[,]
                {
                    { new Guid("20dd1bb6-e722-41ec-84c4-0d66ac95aa6b"), new Guid("d120b22d-85e5-4e6f-80fe-ec49e2138d3c"), "client-web-secret" },
                    { new Guid("825c7420-15ba-4d07-87d7-54042b0de9c8"), new Guid("c49ab291-6e10-49bc-a796-848f22a4936f"), "client-mobile-secret" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "authen",
                table: "ApiScopeResource",
                keyColumns: new[] { "ApiResourceId", "ApiScopeId" },
                keyValues: new object[] { new Guid("5b9aaeb4-d86c-4b2b-91ff-6df237697b77"), new Guid("75f824ab-b599-4519-8bcc-37bea9ad7cd9") });

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "ApiScopeResource",
                keyColumns: new[] { "ApiResourceId", "ApiScopeId" },
                keyValues: new object[] { new Guid("5ed18100-4476-4fd4-ac74-4218e939e3fe"), new Guid("7b3b5167-24a4-41e0-8648-48f7f02dd44b") });

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "ClientGrantType",
                keyColumn: "Id",
                keyValue: new Guid("eb3cea84-46d3-428f-b5ea-5c42b366c60f"));

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "ClientScope",
                keyColumns: new[] { "ApiScopeId", "ClientId" },
                keyValues: new object[] { new Guid("7b3b5167-24a4-41e0-8648-48f7f02dd44b"), new Guid("c49ab291-6e10-49bc-a796-848f22a4936f") });

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "ClientScope",
                keyColumns: new[] { "ApiScopeId", "ClientId" },
                keyValues: new object[] { new Guid("75f824ab-b599-4519-8bcc-37bea9ad7cd9"), new Guid("d120b22d-85e5-4e6f-80fe-ec49e2138d3c") });

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "ClientSecret",
                keyColumn: "Id",
                keyValue: new Guid("20dd1bb6-e722-41ec-84c4-0d66ac95aa6b"));

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "ClientSecret",
                keyColumn: "Id",
                keyValue: new Guid("825c7420-15ba-4d07-87d7-54042b0de9c8"));

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "ApiResource",
                keyColumn: "Id",
                keyValue: new Guid("5b9aaeb4-d86c-4b2b-91ff-6df237697b77"));

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "ApiResource",
                keyColumn: "Id",
                keyValue: new Guid("5ed18100-4476-4fd4-ac74-4218e939e3fe"));

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "ApiScope",
                keyColumn: "Id",
                keyValue: new Guid("75f824ab-b599-4519-8bcc-37bea9ad7cd9"));

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "ApiScope",
                keyColumn: "Id",
                keyValue: new Guid("7b3b5167-24a4-41e0-8648-48f7f02dd44b"));

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("c49ab291-6e10-49bc-a796-848f22a4936f"));

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("d120b22d-85e5-4e6f-80fe-ec49e2138d3c"));
        }
    }
}
