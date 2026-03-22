using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations;

public partial class AddCountry : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Country",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                CreationDate = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 32, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Country", x => x.Id);
            });

        migrationBuilder.InsertData(
            table: "Country",
            columns: new[] { "Id", "CreationDate", "Name" },
            values: new object[] { 1, DateTime.Now, "Colombia" }
            );

        migrationBuilder.AddColumn<int>(
            name: "CountryId",
            table: "Employee",
            nullable: false,
            defaultValue: 1);

        migrationBuilder.CreateIndex(
            name: "IX_Employee_CountryId",
            table: "Employee",
            column: "CountryId");

        migrationBuilder.AddForeignKey(
            name: "FK_Employee_Country_CountryId",
            table: "Employee",
            column: "CountryId",
            principalTable: "Country",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AlterColumn<int>(
            name: "CountryId",
            table: "Employee",
            nullable: false,
            defaultValue: null);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Employee_Country_CountryId",
            table: "Employee");

        migrationBuilder.DropTable(
            name: "Country");

        migrationBuilder.DropIndex(
            name: "IX_Employee_CountryId",
            table: "Employee");

        migrationBuilder.DropColumn(
            name: "CountryId",
            table: "Employee");
    }
}
