using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations;

public partial class AddHolding : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Holding",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                CreationDate = table.Column<DateTime>(nullable: false),
                ServiceId = table.Column<int>(nullable: false),
                EmployeeId = table.Column<int>(nullable: false),
                Price = table.Column<decimal>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Holding", x => x.Id);
                table.ForeignKey(
                    name: "FK_Holding_Employee_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employee",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Holding_Service_ServiceId",
                    column: x => x.ServiceId,
                    principalTable: "Service",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Holding_EmployeeId",
            table: "Holding",
            column: "EmployeeId");

        migrationBuilder.CreateIndex(
            name: "IX_Holding_ServiceId",
            table: "Holding",
            column: "ServiceId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Holding");
    }
}
