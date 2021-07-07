using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMS.Data.Migrations
{
    public partial class createdResidentclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Residents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    ContactNo = table.Column<string>(maxLength: 15, nullable: false),
                    NIDNo = table.Column<string>(maxLength: 17, nullable: false),
                    PhotoPath = table.Column<string>(nullable: true),
                    Occupation = table.Column<string>(maxLength: 100, nullable: false),
                    PrevAddress = table.Column<string>(maxLength: 200, nullable: true),
                    OpeningBalance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Residents", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Residents");
        }
    }
}
