using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatRent.Migrations
{
    public partial class UpdateModelRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Flats_FlatId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Flats_Users_OwnerId",
                table: "Flats");

            migrationBuilder.DropForeignKey(
                name: "FK_Flats_Owners_OwnerId1",
                table: "Flats");

            migrationBuilder.DropForeignKey(
                name: "FK_RentAgreements_Flats_FlatId",
                table: "RentAgreements");

            migrationBuilder.DropIndex(
                name: "IX_Users_EmployeeInformationId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Flats_OwnerId1",
                table: "Flats");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_FlatId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "OwnerId1",
                table: "Flats");

            migrationBuilder.DropColumn(
                name: "FlatId",
                table: "Addresses");

            migrationBuilder.AlterColumn<Guid>(
                name: "FlatId",
                table: "RentAgreements",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeInformationId",
                table: "Users",
                column: "EmployeeInformationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flats_AddressId",
                table: "Flats",
                column: "AddressId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Flats_Addresses_AddressId",
                table: "Flats",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flats_Owners_OwnerId",
                table: "Flats",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentAgreements_Flats_FlatId",
                table: "RentAgreements",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flats_Addresses_AddressId",
                table: "Flats");

            migrationBuilder.DropForeignKey(
                name: "FK_Flats_Owners_OwnerId",
                table: "Flats");

            migrationBuilder.DropForeignKey(
                name: "FK_RentAgreements_Flats_FlatId",
                table: "RentAgreements");

            migrationBuilder.DropIndex(
                name: "IX_Users_EmployeeInformationId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Flats_AddressId",
                table: "Flats");

            migrationBuilder.AlterColumn<Guid>(
                name: "FlatId",
                table: "RentAgreements",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId1",
                table: "Flats",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FlatId",
                table: "Addresses",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeInformationId",
                table: "Users",
                column: "EmployeeInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Flats_OwnerId1",
                table: "Flats",
                column: "OwnerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_FlatId",
                table: "Addresses",
                column: "FlatId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Flats_FlatId",
                table: "Addresses",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flats_Users_OwnerId",
                table: "Flats",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flats_Owners_OwnerId1",
                table: "Flats",
                column: "OwnerId1",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RentAgreements_Flats_FlatId",
                table: "RentAgreements",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
