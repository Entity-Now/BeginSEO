using Microsoft.EntityFrameworkCore.Migrations;

namespace BeginSEO.Migrations
{
    public partial class update_Article : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Article",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KeyWord",
                table: "Article",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Other",
                table: "Article",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Article",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "KeyWord",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "Other",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Article");
        }
    }
}
