using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spinfluence.Migrations
{
    public partial class ApprovedPracticeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Practice",
                type: "tinyint(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Practice");
        }
    }
}
