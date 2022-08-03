using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations
{
    public partial class AddInternalAndExternalDocumentsToService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalDocument",
                table: "Service",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalDocument",
                table: "Service",
                maxLength: 32,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalDocument",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "InternalDocument",
                table: "Service");
        }
    }
}
