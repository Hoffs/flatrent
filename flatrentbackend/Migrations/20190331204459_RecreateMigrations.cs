using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatRent.Migrations
{
    public partial class RecreateMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    Street = table.Column<string>(maxLength: 128, nullable: false),
                    HouseNumber = table.Column<string>(nullable: false),
                    FlatNumber = table.Column<string>(nullable: false),
                    City = table.Column<string>(maxLength: 128, nullable: false),
                    Country = table.Column<string>(maxLength: 128, nullable: false),
                    PostCode = table.Column<string>(maxLength: 24, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgreementStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgreementStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: false),
                    Password = table.Column<string>(maxLength: 64, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 50, nullable: false),
                    About = table.Column<string>(maxLength: 64000, nullable: true),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "UserTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flats",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Area = table.Column<float>(nullable: false),
                    Floor = table.Column<int>(nullable: false),
                    RoomCount = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    YearOfConstruction = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    IsPublic = table.Column<bool>(nullable: false),
                    OwnerId = table.Column<Guid>(nullable: false),
                    AddressId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flats_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Flats_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Faults",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Repaired = table.Column<bool>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    FlatId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Faults_Flats_FlatId",
                        column: x => x.FlatId,
                        principalTable: "Flats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PhotoBytes = table.Column<byte[]>(maxLength: 65536, nullable: false),
                    FlatId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Flats_FlatId",
                        column: x => x.FlatId,
                        principalTable: "Flats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RentAgreements",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false),
                    Comments = table.Column<string>(maxLength: 65536, nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    RenterId = table.Column<Guid>(nullable: false),
                    FlatId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentAgreements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentAgreements_Flats_FlatId",
                        column: x => x.FlatId,
                        principalTable: "Flats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentAgreements_Users_RenterId",
                        column: x => x.RenterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentAgreements_AgreementStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "AgreementStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    AmountToPay = table.Column<float>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    AmountPaid = table.Column<float>(nullable: false),
                    PaidDate = table.Column<DateTime>(nullable: false),
                    AgreementId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_RentAgreements_AgreementId",
                        column: x => x.AgreementId,
                        principalTable: "RentAgreements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AgreementStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Requested" },
                    { 2, "Accepted" },
                    { 3, "Rejected" },
                    { 4, "Expired" },
                    { 5, "Ended" }
                });

            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Administrator" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "About", "CreatedDate", "Deleted", "Email", "FirstName", "LastName", "ModifiedDate", "Password", "PhoneNumber", "TypeId" },
                values: new object[] { new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11bb4"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "admin@admin.com", "Test", "Test", null, "UhYWUG3vDiTZZt04YTqkBxL/RUxhyEvqpzCXJlRDMas=", "+37060286000", 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "About", "CreatedDate", "Deleted", "Email", "FirstName", "LastName", "ModifiedDate", "Password", "PhoneNumber", "TypeId" },
                values: new object[] { new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11bc8"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "client@client.com", "Test", "Test", null, "UhYWUG3vDiTZZt04YTqkBxL/RUxhyEvqpzCXJlRDMas=", "+37060286001", 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Faults_FlatId",
                table: "Faults",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_Flats_AddressId",
                table: "Flats",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flats_OwnerId",
                table: "Flats",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_AgreementId",
                table: "Invoices",
                column: "AgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_FlatId",
                table: "Photos",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_RentAgreements_FlatId",
                table: "RentAgreements",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_RentAgreements_RenterId",
                table: "RentAgreements",
                column: "RenterId");

            migrationBuilder.CreateIndex(
                name: "IX_RentAgreements_StatusId",
                table: "RentAgreements",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TypeId",
                table: "Users",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Faults");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "RentAgreements");

            migrationBuilder.DropTable(
                name: "Flats");

            migrationBuilder.DropTable(
                name: "AgreementStatuses");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserTypes");
        }
    }
}
