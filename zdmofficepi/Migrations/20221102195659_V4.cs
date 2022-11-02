using Microsoft.EntityFrameworkCore.Migrations;

namespace zdmofficepi.Migrations
{
    public partial class V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Companyuuid",
                table: "Productgroups",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Companyuuid",
                table: "Productgroups");
        }
    }
}
