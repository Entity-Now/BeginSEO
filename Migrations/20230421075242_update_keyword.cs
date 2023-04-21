using Microsoft.EntityFrameworkCore.Migrations;

namespace BeginSEO.Migrations
{
    public partial class update_keyword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "level",
                table: "KeyWord",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "level",
                table: "KeyWord");
        }
    }
}
