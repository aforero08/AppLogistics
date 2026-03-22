using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace AppLogistics.Data.Migrations;

public partial class AddBranchOffice : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "BranchOffice",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                CreationDate = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 32, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BranchOffice", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "BranchOffice");
    }
}
