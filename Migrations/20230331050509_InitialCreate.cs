using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NPOI.SS.UserModel;

namespace BeginSEO.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Proxys",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IP = table.Column<string>(nullable: true),
                    Popt = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proxys", x => x.Id);
                });
            migrationBuilder.AddColumn<string>(
                name: "ProxyId",
                table: "TempCookie",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proxys");
            migrationBuilder.DropColumn("ProxyId", "TempCookie");
        }
    }
}
