using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Prxlk.Data.EntityFramework.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Proxies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Ip = table.Column<string>(nullable: true),
                    Port = table.Column<int>(nullable: false),
                    Protocol = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proxies", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proxies");
        }
    }
}
