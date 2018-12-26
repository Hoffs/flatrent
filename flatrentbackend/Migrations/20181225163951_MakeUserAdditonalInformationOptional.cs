using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatRent.Migrations
{
    public partial class MakeUserAdditonalInformationOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ClientInformations_ClientInformationId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_EmployeeInformations_EmployeeInformationId",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeInformationId",
                table: "Users",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "ClientInformationId",
                table: "Users",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ClientInformations_ClientInformationId",
                table: "Users",
                column: "ClientInformationId",
                principalTable: "ClientInformations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_EmployeeInformations_EmployeeInformationId",
                table: "Users",
                column: "EmployeeInformationId",
                principalTable: "EmployeeInformations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ClientInformations_ClientInformationId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_EmployeeInformations_EmployeeInformationId",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeInformationId",
                table: "Users",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ClientInformationId",
                table: "Users",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ClientInformations_ClientInformationId",
                table: "Users",
                column: "ClientInformationId",
                principalTable: "ClientInformations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_EmployeeInformations_EmployeeInformationId",
                table: "Users",
                column: "EmployeeInformationId",
                principalTable: "EmployeeInformations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
