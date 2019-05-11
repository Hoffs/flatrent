using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatRent.Migrations
{
    public partial class ChangeFaultToIncident : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Faults_FaultId",
                table: "Attachments");

            migrationBuilder.RenameTable(name: "Faults", newName: "Incidents");

            migrationBuilder.RenameColumn(
                name: "FaultId",
                table: "Attachments",
                newName: "IncidentId");

            migrationBuilder.RenameIndex(
                name: "IX_Attachments_FaultId",
                table: "Attachments",
                newName: "IX_Attachments_IncidentId");

            migrationBuilder.AlterColumn<string>(
                name: "TenantRequirements",
                table: "Flats",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Conversations",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_AgreementId",
                table: "Incidents",
                column: "AgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_AuthorId",
                table: "Incidents",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_ConversationId",
                table: "Incidents",
                column: "ConversationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_InvoiceId",
                table: "Incidents",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Incidents_IncidentId",
                table: "Attachments",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Incidents_IncidentId",
                table: "Attachments");

            migrationBuilder.RenameTable(name: "Incidents", newName: "Faults");

            migrationBuilder.RenameColumn(
                name: "IncidentId",
                table: "Attachments",
                newName: "FaultId");

            migrationBuilder.RenameIndex(
                name: "IX_Attachments_IncidentId",
                table: "Attachments",
                newName: "IX_Attachments_FaultId");

            migrationBuilder.AlterColumn<string>(
                name: "TenantRequirements",
                table: "Flats",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Conversations",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faults_AgreementId",
                table: "Faults",
                column: "AgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_Faults_AuthorId",
                table: "Faults",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Faults_ConversationId",
                table: "Faults",
                column: "ConversationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faults_InvoiceId",
                table: "Faults",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Faults_FaultId",
                table: "Attachments",
                column: "FaultId",
                principalTable: "Faults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
