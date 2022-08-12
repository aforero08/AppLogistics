using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations
{
    public partial class AddClientIdToSectors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Sector",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sector_ClientId",
                table: "Sector",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sector_Client_ClientId",
                table: "Sector",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sector_Client_ClientId",
                table: "Sector");

            migrationBuilder.DropIndex(
                name: "IX_Sector_ClientId",
                table: "Sector");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Sector");
        }
    }
}
