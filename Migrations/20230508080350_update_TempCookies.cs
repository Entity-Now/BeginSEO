using Microsoft.EntityFrameworkCore.Migrations;

namespace BeginSEO.Migrations
{
    public partial class update_TempCookies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Domain",
                table: "TempCookie",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "TempCookie",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Domain",
                table: "TempCookie");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "TempCookie");
        }
    }
}
