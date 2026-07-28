using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations;

public partial class RemoveBranchOfficeFromClientAndEmployee : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Client_BranchOffice_BranchOfficeId",
            table: "Client");

        migrationBuilder.DropForeignKey(
            name: "FK_Employee_BranchOffice_BranchOfficeId",
            table: "Employee");

        migrationBuilder.DropIndex(
            name: "IX_Employee_BranchOfficeId",
            table: "Employee");

        migrationBuilder.DropIndex(
            name: "IX_Client_BranchOfficeId",
            table: "Client");

        migrationBuilder.DropColumn(
            name: "BranchOfficeId",
            table: "Employee");

        migrationBuilder.DropColumn(
            name: "BranchOfficeId",
            table: "Client");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "BranchOfficeId",
            table: "Employee",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "BranchOfficeId",
            table: "Client",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_Employee_BranchOfficeId",
            table: "Employee",
            column: "BranchOfficeId");

        migrationBuilder.CreateIndex(
            name: "IX_Client_BranchOfficeId",
            table: "Client",
            column: "BranchOfficeId");

        migrationBuilder.AddForeignKey(
            name: "FK_Client_BranchOffice_BranchOfficeId",
            table: "Client",
            column: "BranchOfficeId",
            principalTable: "BranchOffice",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Employee_BranchOffice_BranchOfficeId",
            table: "Employee",
            column: "BranchOfficeId",
            principalTable: "BranchOffice",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
