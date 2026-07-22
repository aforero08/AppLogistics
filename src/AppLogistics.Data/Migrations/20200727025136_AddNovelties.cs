using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations;

public partial class AddNovelties : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Novelties",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                CreationDate = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 32, nullable: false),
                Description = table.Column<string>(maxLength: 128, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Novelties", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ServiceNovelties",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                CreationDate = table.Column<DateTime>(nullable: false),
                ServiceId = table.Column<int>(nullable: false),
                NoveltyId = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ServiceNovelties", x => x.Id);
                table.ForeignKey(
                    name: "FK_ServiceNovelties_Novelties_NoveltyId",
                    column: x => x.NoveltyId,
                    principalTable: "Novelties",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_ServiceNovelties_Service_ServiceId",
                    column: x => x.ServiceId,
                    principalTable: "Service",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ServiceNovelties_NoveltyId",
            table: "ServiceNovelties",
            column: "NoveltyId");

        migrationBuilder.CreateIndex(
            name: "IX_ServiceNovelties_ServiceId",
            table: "ServiceNovelties",
            column: "ServiceId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ServiceNovelties");

        migrationBuilder.DropTable(
            name: "Novelties");
    }
}
