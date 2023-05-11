using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spinfluence.Migrations
{
    public partial class ImprovedSpinMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverLetter",
                table: "Practice",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Resume",
                table: "Practice",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverLetter",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "Resume",
                table: "Practice");
        }
    }
}
