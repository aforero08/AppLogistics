using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations;

public partial class AddServiceEndDate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "EndDate",
            table: "Service",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "EndDate",
            table: "Service");
    }
}
