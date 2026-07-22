using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations;

public partial class AddSex : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Sex",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                CreationDate = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 32, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Sex", x => x.Id);
            });

        migrationBuilder.InsertData(
            table: "Sex",
            columns: new[] { "Id", "CreationDate", "Name" },
            values: new object[] { 1, DateTime.Now, "Masculino" }
            );

        migrationBuilder.AddColumn<int>(
            name: "SexId",
            table: "Employee",
            nullable: false,
            defaultValue: 1);

        migrationBuilder.CreateIndex(
            name: "IX_Employee_SexId",
            table: "Employee",
            column: "SexId");

        migrationBuilder.AddForeignKey(
            name: "FK_Employee_Sex_SexId",
            table: "Employee",
            column: "SexId",
            principalTable: "Sex",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AlterColumn<int>(
            name: "SexId",
            table: "Employee",
            nullable: false,
            defaultValue: null);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Employee_Sex_SexId",
            table: "Employee");

        migrationBuilder.DropTable(
            name: "Sex");

        migrationBuilder.DropIndex(
            name: "IX_Employee_SexId",
            table: "Employee");

        migrationBuilder.DropColumn(
            name: "SexId",
            table: "Employee");
    }
}
