using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatRent.Migrations
{
    public partial class AddInvoiceAndUpdateOtherModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faults_Flats_FlatId",
                table: "Faults");

            migrationBuilder.DropIndex(
                name: "IX_Faults_FlatId",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "FlatId",
                table: "Faults");

            migrationBuilder.AddColumn<Guid>(
                name: "InvoiceId",
                table: "Faults",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faults_InvoiceId",
                table: "Faults",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_Invoices_InvoiceId",
                table: "Faults",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faults_Invoices_InvoiceId",
                table: "Faults");

            migrationBuilder.DropIndex(
                name: "IX_Faults_InvoiceId",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "Faults");

            migrationBuilder.AddColumn<Guid>(
                name: "FlatId",
                table: "Faults",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Faults_FlatId",
                table: "Faults",
                column: "FlatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_Flats_FlatId",
                table: "Faults",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
