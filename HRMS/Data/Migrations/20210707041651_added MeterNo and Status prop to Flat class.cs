using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Data.Migrations
{
    public partial class addedMeterNoandStatusproptoFlatclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeterNo",
                table: "Flats",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Flats",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeterNo",
                table: "Flats");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Flats");
        }
    }
}
