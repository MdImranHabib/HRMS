using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Data.Migrations
{
    public partial class removedStatuspropfromResidentFlatclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ResidentFlats");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "ResidentFlats",
                nullable: false,
                defaultValue: false);
        }
    }
}
