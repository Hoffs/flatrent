using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatRent.Migrations
{
    public partial class AddTestSupplyUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EmployeeInformations",
                columns: new[] { "Id", "CreatedDate", "Deleted", "Department", "ModifiedDate", "Position" },
                values: new object[] { new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11e88"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Supply", null, "Tiekimo Vadovas" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ClientInformationId", "CreatedDate", "Deleted", "Email", "EmployeeInformationId", "FirstName", "LastName", "ModifiedDate", "Password", "PhoneNumber", "TypeId" },
                values: new object[] { new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11e82"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "supply@supply.com", new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11e88"), "Test", "Test", null, "UhYWUG3vDiTZZt04YTqkBxL/RUxhyEvqpzCXJlRDMas=", "+37060286009", new Guid("268c6597-15cb-4ab1-9d39-8a7d7c85b3d1") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11e82"));

            migrationBuilder.DeleteData(
                table: "EmployeeInformations",
                keyColumn: "Id",
                keyValue: new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11e88"));
        }
    }
}
