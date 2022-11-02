using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace zdmofficepi.Migrations
{
    public partial class V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Uuid = table.Column<string>(type: "text", nullable: true),
                    Createduser = table.Column<string>(type: "text", nullable: true),
                    Updateduser = table.Column<string>(type: "text", nullable: true),
                    Deleteuser = table.Column<string>(type: "text", nullable: true),
                    Createdtime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Updatetime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deletetime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
