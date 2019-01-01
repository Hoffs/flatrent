using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatRent.Migrations
{
    public partial class AddBaseProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Invoices",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Faults",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RentAgreements",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "RentAgreements",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "RentAgreements",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Owners",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Owners",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Owners",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Invoices",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Flats",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Flats",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Flats",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Faults",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Faults",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EmployeeInformations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "EmployeeInformations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "EmployeeInformations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ClientInformations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "ClientInformations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "ClientInformations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Addresses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Addresses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Addresses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RentAgreements");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "RentAgreements");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "RentAgreements");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Flats");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Flats");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Flats");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EmployeeInformations");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "EmployeeInformations");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "EmployeeInformations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ClientInformations");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ClientInformations");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "ClientInformations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Invoices",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Faults",
                newName: "CreationDate");
        }
    }
}
