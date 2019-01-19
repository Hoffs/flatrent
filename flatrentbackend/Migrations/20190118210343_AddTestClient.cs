using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatRent.Migrations
{
    public partial class AddTestClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_ClientInformationId",
                table: "Users");

            migrationBuilder.InsertData(
                table: "ClientInformations",
                columns: new[] { "Id", "CreatedDate", "Deleted", "Description", "ModifiedDate" },
                values: new object[] { new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11bc9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Cool client", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ClientInformationId", "CreatedDate", "Deleted", "Email", "EmployeeInformationId", "FirstName", "LastName", "ModifiedDate", "Password", "PhoneNumber", "TypeId" },
                values: new object[] { new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11bc8"), new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11bc9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "client@client.com", null, "Test", "Test", null, "UhYWUG3vDiTZZt04YTqkBxL/RUxhyEvqpzCXJlRDMas=", "+37060286001", new Guid("ed42ea4b-9900-4477-af32-0336ca61eab1") });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClientInformationId",
                table: "Users",
                column: "ClientInformationId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_ClientInformationId",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11bc8"));

            migrationBuilder.DeleteData(
                table: "ClientInformations",
                keyColumn: "Id",
                keyValue: new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11bc9"));

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClientInformationId",
                table: "Users",
                column: "ClientInformationId");
        }
    }
}
