using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatRent.Migrations
{
    public partial class FaultsAndAgreementsHaveConversationByDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Agreements_AgreementId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Faults_FaultId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Flats_FlatId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_AgreementId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_FaultId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_FlatId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "AgreementId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "FaultId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "FlatId",
                table: "Conversations");

            migrationBuilder.AddColumn<Guid>(
                name: "ConversationId",
                table: "Faults",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Conversations",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "ConversationId",
                table: "Agreements",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Faults_ConversationId",
                table: "Faults",
                column: "ConversationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_ConversationId",
                table: "Agreements",
                column: "ConversationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Conversations_ConversationId",
                table: "Agreements",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_Conversations_ConversationId",
                table: "Faults",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Conversations_ConversationId",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Faults_Conversations_ConversationId",
                table: "Faults");

            migrationBuilder.DropIndex(
                name: "IX_Faults_ConversationId",
                table: "Faults");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_ConversationId",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Agreements");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Conversations",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AddColumn<Guid>(
                name: "AgreementId",
                table: "Conversations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FaultId",
                table: "Conversations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FlatId",
                table: "Conversations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_AgreementId",
                table: "Conversations",
                column: "AgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_FaultId",
                table: "Conversations",
                column: "FaultId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_FlatId",
                table: "Conversations",
                column: "FlatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Agreements_AgreementId",
                table: "Conversations",
                column: "AgreementId",
                principalTable: "Agreements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Faults_FaultId",
                table: "Conversations",
                column: "FaultId",
                principalTable: "Faults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Flats_FlatId",
                table: "Conversations",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
