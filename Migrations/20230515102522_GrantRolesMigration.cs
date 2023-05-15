using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spinfluence.Migrations
{
    public partial class GrantRolesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "admin",
                table: "Account");

            migrationBuilder.AddColumn<int>(
                name: "GrantType",
                table: "Account",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GrantType",
                table: "Account");

            migrationBuilder.AddColumn<bool>(
                name: "admin",
                table: "Account",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
