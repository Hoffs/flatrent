using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatRent.Migrations
{
    public partial class AddNameToFaultEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Faults",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Faults");
        }
    }
}
