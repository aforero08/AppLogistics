using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations;

public partial class AddSector : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "SectorId",
            table: "Service",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "Sector",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                CreationDate = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 32, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Sector", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Service_SectorId",
            table: "Service",
            column: "SectorId");

        migrationBuilder.AddForeignKey(
            name: "FK_Service_Sector_SectorId",
            table: "Service",
            column: "SectorId",
            principalTable: "Sector",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Service_Sector_SectorId",
            table: "Service");

        migrationBuilder.DropTable(
            name: "Sector");

        migrationBuilder.DropIndex(
            name: "IX_Service_SectorId",
            table: "Service");

        migrationBuilder.DropColumn(
            name: "SectorId",
            table: "Service");
    }
}
