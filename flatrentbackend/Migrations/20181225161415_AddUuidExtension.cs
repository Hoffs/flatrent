using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatRent.Migrations
{
    public partial class AddUuidExtension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flats_Addresses_AddressId",
                table: "Flats");

            migrationBuilder.DropIndex(
                name: "IX_Flats_AddressId",
                table: "Flats");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_FlatId",
                table: "Addresses");

            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "Id",
                keyValue: new Guid("0a7389f2-93da-4731-8bd1-71e752d82fc2"));

            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "Id",
                keyValue: new Guid("3ea892fa-377a-42bf-8889-ccc546f114b4"));

            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "Id",
                keyValue: new Guid("d26510ea-3252-44bb-9d75-7cfc967ab3f2"));

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
                name: "IX_Addresses_FlatId",
                table: "Addresses",
                column: "FlatId",
                unique: true);

            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS pgcrypto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_FlatId",
                table: "Addresses");

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

            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("d26510ea-3252-44bb-9d75-7cfc967ab3f2"), "Client" },
                    { new Guid("3ea892fa-377a-42bf-8889-ccc546f114b4"), "Employee" },
                    { new Guid("0a7389f2-93da-4731-8bd1-71e752d82fc2"), "Administrator" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flats_AddressId",
                table: "Flats",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_FlatId",
                table: "Addresses",
                column: "FlatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flats_Addresses_AddressId",
                table: "Flats",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
