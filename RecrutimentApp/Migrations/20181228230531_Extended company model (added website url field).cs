using Microsoft.EntityFrameworkCore.Migrations;

namespace RecrutimentApp.Migrations
{
    public partial class Extendedcompanymodeladdedwebsiteurlfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WebSiteUrl",
                table: "Companies",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WebSiteUrl",
                table: "Companies");
        }
    }
}
