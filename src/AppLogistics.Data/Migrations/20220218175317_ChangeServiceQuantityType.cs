using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations;

public partial class ChangeServiceQuantityType : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<double>(
            name: "Quantity",
            table: "Service",
            nullable: false,
            oldClrType: typeof(int));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "Quantity",
            table: "Service",
            nullable: false,
            oldClrType: typeof(double));
    }
}
