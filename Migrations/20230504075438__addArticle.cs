using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeginSEO.Migrations
{
    public partial class _addArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Rewrite = table.Column<string>(nullable: true),
                    Contrast = table.Column<double>(nullable: false),
                    IsUse = table.Column<bool>(nullable: false),
                    IsUseRewrite = table.Column<bool>(nullable: false),
                    IsUseReplaceKeyword = table.Column<bool>(nullable: false),
                    IsInspect = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    GrabTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Article");
        }
    }
}
