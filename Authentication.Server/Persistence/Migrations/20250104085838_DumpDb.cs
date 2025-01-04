using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Authentication.Server.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DumpDb : Migration
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
                    { new Guid("97b1dc06-ebd0-455a-8a27-71bd04b7fbd9"), "student-api-resource", new Guid("eb19ede6-a6b9-4775-96f9-a8e4fbeaf85c") },
                    { new Guid("e895aaeb-d696-4d8a-978c-3718dfbd5843"), "teacher-api-resource", new Guid("c30fd222-0e8e-47ff-93cd-7719db5ab2e3") }
                });

            migrationBuilder.InsertData(
                schema: "authen",
                table: "ApiScope",
                columns: new[] { "Id", "DisplayName", "Name" },
                values: new object[,]
                {
                    { new Guid("538a056c-ae30-4ea4-8cc9-6adff622dded"), "Teacher Api Scope", "teacher-scope" },
                    { new Guid("d14f2be1-1abf-4c79-9fe0-19980ac6cd29"), "Student Api Scope", "student-scope" }
                });

            migrationBuilder.InsertData(
                schema: "authen",
                table: "Client",
                columns: new[] { "Id", "ClientId" },
                values: new object[,]
                {
                    { new Guid("4cb4b753-a1ac-476d-83d1-3b001899866d"), "ewb-student-web" },
                    { new Guid("eb2578fb-db26-4de4-b876-37244f97a6ca"), "ewb-teacher" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "authen",
                table: "ApiResource",
                keyColumn: "Id",
                keyValue: new Guid("97b1dc06-ebd0-455a-8a27-71bd04b7fbd9"));

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "ApiResource",
                keyColumn: "Id",
                keyValue: new Guid("e895aaeb-d696-4d8a-978c-3718dfbd5843"));

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "ApiScope",
                keyColumn: "Id",
                keyValue: new Guid("538a056c-ae30-4ea4-8cc9-6adff622dded"));

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "ApiScope",
                keyColumn: "Id",
                keyValue: new Guid("d14f2be1-1abf-4c79-9fe0-19980ac6cd29"));

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("4cb4b753-a1ac-476d-83d1-3b001899866d"));

            migrationBuilder.DeleteData(
                schema: "authen",
                table: "Client",
                keyColumn: "Id",
                keyValue: new Guid("eb2578fb-db26-4de4-b876-37244f97a6ca"));
        }
    }
}
