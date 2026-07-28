using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations;

public partial class IncreaseClientNameLength : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Client",
            maxLength: 128,
            nullable: false,
            oldClrType: typeof(string),
            oldMaxLength: 32);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Client",
            maxLength: 32,
            nullable: false,
            oldClrType: typeof(string),
            oldMaxLength: 128);
    }
}
