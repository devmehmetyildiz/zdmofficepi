using Microsoft.EntityFrameworkCore.Migrations;

namespace zdmofficepi.Migrations
{
    public partial class V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Categoryuuid",
                table: "Productgroups",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subcategoryuuid",
                table: "Productgroups",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categoryuuid",
                table: "Productgroups");

            migrationBuilder.DropColumn(
                name: "Subcategoryuuid",
                table: "Productgroups");
        }
    }
}
