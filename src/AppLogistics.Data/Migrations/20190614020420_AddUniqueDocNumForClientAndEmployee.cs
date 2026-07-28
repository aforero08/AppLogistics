using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations;

public partial class AddUniqueDocNumForClientAndEmployee : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_Employee_DocumentNumber",
            table: "Employee",
            column: "DocumentNumber",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Client_Nit",
            table: "Client",
            column: "Nit",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Employee_DocumentNumber",
            table: "Employee");

        migrationBuilder.DropIndex(
            name: "IX_Client_Nit",
            table: "Client");
    }
}
