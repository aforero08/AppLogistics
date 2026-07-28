using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace AppLogistics.Data.Migrations;

public partial class AddCarrier : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Carrier",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                CreationDate = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 32, nullable: false),
                Nit = table.Column<string>(maxLength: 16, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Carrier", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Carrier");
    }
}
