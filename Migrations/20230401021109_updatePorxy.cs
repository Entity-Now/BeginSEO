using System;
using BeginSEO.Data;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeginSEO.Migrations
{
    public partial class updatePorxy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ProxyStatus>("Status", "Proxys",nullable: true);
            migrationBuilder.AddColumn<int>("Speed", "Proxys",nullable: true);
            migrationBuilder.RenameColumn("Popt","Proxys", "Port");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Status", "Proxys");
            migrationBuilder.DropColumn("Speed", "Proxys");
            migrationBuilder.RenameColumn("Port", "Proxys", "Popt");
        }
    }
}
