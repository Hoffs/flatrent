using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatRent.Migrations
{
    public partial class ChangeModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientInformations_Users_UserId",
                table: "ClientInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeInformations_Users_UserId",
                table: "EmployeeInformations");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeInformations_UserId",
                table: "EmployeeInformations");

            migrationBuilder.DropIndex(
                name: "IX_ClientInformations_UserId",
                table: "ClientInformations");

            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "Id",
                keyValue: new Guid("01ec9372-db9b-4ec2-a6bb-9158d2507697"));

            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "Id",
                keyValue: new Guid("2a54b874-7955-4b44-ac08-67120bafb60b"));

            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "Id",
                keyValue: new Guid("fcffad94-2d80-46da-b508-fded62458fb5"));

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EmployeeInformations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ClientInformations");

            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("ed42ea4b-9900-4477-af32-0336ca61eab1"), "Client" },
                    { new Guid("268c6597-15cb-4ab1-9d39-8a7d7c85b3d1"), "Employee" },
                    { new Guid("ee3d96b6-4243-4235-8231-9a9fced615fe"), "Administrator" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "Id",
                keyValue: new Guid("268c6597-15cb-4ab1-9d39-8a7d7c85b3d1"));

            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "Id",
                keyValue: new Guid("ed42ea4b-9900-4477-af32-0336ca61eab1"));

            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "Id",
                keyValue: new Guid("ee3d96b6-4243-4235-8231-9a9fced615fe"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "EmployeeInformations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ClientInformations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("01ec9372-db9b-4ec2-a6bb-9158d2507697"), "Client" },
                    { new Guid("2a54b874-7955-4b44-ac08-67120bafb60b"), "Employee" },
                    { new Guid("fcffad94-2d80-46da-b508-fded62458fb5"), "Administrator" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInformations_UserId",
                table: "EmployeeInformations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInformations_UserId",
                table: "ClientInformations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientInformations_Users_UserId",
                table: "ClientInformations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeInformations_Users_UserId",
                table: "EmployeeInformations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
