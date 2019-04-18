using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatRent.Migrations
{
    public partial class UpdateFlatEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Features",
                table: "Flats",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFurnished",
                table: "Flats",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MinimumRentDays",
                table: "Flats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TenantRequirements",
                table: "Flats",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalFloors",
                table: "Flats",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Features",
                table: "Flats");

            migrationBuilder.DropColumn(
                name: "IsFurnished",
                table: "Flats");

            migrationBuilder.DropColumn(
                name: "MinimumRentDays",
                table: "Flats");

            migrationBuilder.DropColumn(
                name: "TenantRequirements",
                table: "Flats");

            migrationBuilder.DropColumn(
                name: "TotalFloors",
                table: "Flats");
        }
    }
}
