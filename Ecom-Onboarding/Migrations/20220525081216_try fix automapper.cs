using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecom_Onboarding.Migrations
{
    public partial class tryfixautomapper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tes",
                table: "Game");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tes",
                table: "Game",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
