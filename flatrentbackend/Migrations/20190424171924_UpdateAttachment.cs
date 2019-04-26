using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatRent.Migrations
{
    public partial class UpdateAttachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "MessageId",
                table: "Attachments",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "AgreementId",
                table: "Attachments",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FaultId",
                table: "Attachments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_AgreementId",
                table: "Attachments",
                column: "AgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_FaultId",
                table: "Attachments",
                column: "FaultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Agreements_AgreementId",
                table: "Attachments",
                column: "AgreementId",
                principalTable: "Agreements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Faults_FaultId",
                table: "Attachments",
                column: "FaultId",
                principalTable: "Faults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Agreements_AgreementId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Faults_FaultId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_AgreementId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_FaultId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "AgreementId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "FaultId",
                table: "Attachments");

            migrationBuilder.AlterColumn<Guid>(
                name: "MessageId",
                table: "Attachments",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
