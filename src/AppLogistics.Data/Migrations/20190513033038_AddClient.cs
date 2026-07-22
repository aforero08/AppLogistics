using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace AppLogistics.Data.Migrations;

public partial class AddClient : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Client",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                CreationDate = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 32, nullable: false),
                Nit = table.Column<string>(maxLength: 16, nullable: false),
                Address = table.Column<string>(maxLength: 64, nullable: false),
                Phone = table.Column<string>(maxLength: 16, nullable: true),
                Contact = table.Column<string>(maxLength: 32, nullable: true),
                BranchOfficeId = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Client", x => x.Id);
                table.ForeignKey(
                    name: "FK_Client_BranchOffice_BranchOfficeId",
                    column: x => x.BranchOfficeId,
                    principalTable: "BranchOffice",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Client_BranchOfficeId",
            table: "Client",
            column: "BranchOfficeId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Client");
    }
}
