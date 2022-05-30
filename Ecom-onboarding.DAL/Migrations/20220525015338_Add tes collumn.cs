using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecom_Onboarding.DAL.Migrations
{
    public partial class Addtescollumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tes",
                table: "Game",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tes",
                table: "Game");
        }
    }
}
