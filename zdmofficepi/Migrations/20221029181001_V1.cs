using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace zdmofficepi.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
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
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Productuui = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Filename = table.Column<string>(type: "text", nullable: true),
                    Filefolder = table.Column<string>(type: "text", nullable: true),
                    Filepath = table.Column<string>(type: "text", nullable: true),
                    Filetype = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productgroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    IsSet = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Price = table.Column<double>(type: "double", nullable: false),
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
                    table.PrimaryKey("PK_Productgroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Groupuui = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Productcode = table.Column<string>(type: "text", nullable: true),
                    Dimension = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double", nullable: false),
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
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subcategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Categoryuui = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Subcategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    Salt = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Productgroups");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Subcategories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
