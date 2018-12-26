using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatRent.Migrations
{
    public partial class InitialMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Account = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flats",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Area = table.Column<float>(nullable: false),
                    Floor = table.Column<int>(nullable: false),
                    RoomCount = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    YearOfConstruction = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    OwnerId = table.Column<Guid>(nullable: false),
                    AddressId = table.Column<Guid>(nullable: false),
                    OwnerId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flats_Owners_OwnerId1",
                        column: x => x.OwnerId1,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Street = table.Column<string>(nullable: true),
                    HouseNumber = table.Column<string>(nullable: true),
                    FlatNumber = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    FlatId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Flats_FlatId",
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
                    PhotoBytes = table.Column<byte[]>(nullable: true),
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
                name: "Faults",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Repaired = table.Column<bool>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    FlatId = table.Column<Guid>(nullable: false),
                    ClientInformationId = table.Column<Guid>(nullable: false),
                    EmployeeInformationId = table.Column<Guid>(nullable: false)
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
                name: "RentAgreements",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Verified = table.Column<bool>(nullable: false),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    ClientInformationId = table.Column<Guid>(nullable: false),
                    FlatId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentAgreements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentAgreements_Flats_FlatId",
                        column: x => x.FlatId,
                        principalTable: "Flats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AmountToPay = table.Column<float>(nullable: false),
                    AmountPaid = table.Column<float>(nullable: false),
                    PaidDate = table.Column<DateTime>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    RentAgreementId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_RentAgreements_RentAgreementId",
                        column: x => x.RentAgreementId,
                        principalTable: "RentAgreements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    Password = table.Column<string>(maxLength: 64, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 50, nullable: true),
                    TypeId = table.Column<Guid>(nullable: false),
                    EmployeeInformationId = table.Column<Guid>(nullable: false),
                    ClientInformationId = table.Column<Guid>(nullable: false)
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
                name: "ClientInformations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientInformations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeInformations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Position = table.Column<string>(maxLength: 64, nullable: true),
                    Department = table.Column<string>(maxLength: 64, nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeInformations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Addresses_FlatId",
                table: "Addresses",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInformations_UserId",
                table: "ClientInformations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInformations_UserId",
                table: "EmployeeInformations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Faults_ClientInformationId",
                table: "Faults",
                column: "ClientInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Faults_EmployeeInformationId",
                table: "Faults",
                column: "EmployeeInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Faults_FlatId",
                table: "Faults",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_Flats_AddressId",
                table: "Flats",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Flats_OwnerId",
                table: "Flats",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Flats_OwnerId1",
                table: "Flats",
                column: "OwnerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_RentAgreementId",
                table: "Invoices",
                column: "RentAgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_FlatId",
                table: "Photos",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_RentAgreements_ClientInformationId",
                table: "RentAgreements",
                column: "ClientInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_RentAgreements_FlatId",
                table: "RentAgreements",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClientInformationId",
                table: "Users",
                column: "ClientInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeInformationId",
                table: "Users",
                column: "EmployeeInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TypeId",
                table: "Users",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flats_Users_OwnerId",
                table: "Flats",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flats_Addresses_AddressId",
                table: "Flats",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_ClientInformations_ClientInformationId",
                table: "Faults",
                column: "ClientInformationId",
                principalTable: "ClientInformations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_EmployeeInformations_EmployeeInformationId",
                table: "Faults",
                column: "EmployeeInformationId",
                principalTable: "EmployeeInformations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentAgreements_ClientInformations_ClientInformationId",
                table: "RentAgreements",
                column: "ClientInformationId",
                principalTable: "ClientInformations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Flats_FlatId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientInformations_Users_UserId",
                table: "ClientInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeInformations_Users_UserId",
                table: "EmployeeInformations");

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
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ClientInformations");

            migrationBuilder.DropTable(
                name: "EmployeeInformations");

            migrationBuilder.DropTable(
                name: "UserTypes");
        }
    }
}
