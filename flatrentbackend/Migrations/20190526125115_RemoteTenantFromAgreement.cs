using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatRent.Migrations
{
    public partial class RemoteTenantFromAgreement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Users_AuthorId",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Users_TenantId",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_TenantId",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Agreements");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Users_AuthorId",
                table: "Agreements",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Users_AuthorId",
                table: "Agreements");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Agreements",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_TenantId",
                table: "Agreements",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Users_AuthorId",
                table: "Agreements",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Users_TenantId",
                table: "Agreements",
                column: "TenantId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
