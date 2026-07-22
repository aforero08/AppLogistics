using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace AppLogistics.Data.Migrations;

public partial class AddEducationLevel : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "EducationLevel",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                CreationDate = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 32, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EducationLevel", x => x.Id);
            });

        migrationBuilder.InsertData(
            table: "EducationLevel",
            columns: new[] { "Id", "CreationDate", "Name" },
            values: new object[] { 1, DateTime.Now, "Bachillerato" }
            );

        migrationBuilder.AddColumn<int>(
            name: "EducationLevelId",
            table: "Employee",
            nullable: false,
            defaultValue: 1);

        migrationBuilder.CreateIndex(
            name: "IX_Employee_EducationLevelId",
            table: "Employee",
            column: "EducationLevelId");

        migrationBuilder.AddForeignKey(
            name: "FK_Employee_EducationLevel_EducationLevelId",
            table: "Employee",
            column: "EducationLevelId",
            principalTable: "EducationLevel",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AlterColumn<int>(
            name: "EducationLevelId",
            table: "Employee",
            nullable: false,
            defaultValue: null);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Employee_EducationLevel_EducationLevelId",
            table: "Employee");

        migrationBuilder.DropTable(
            name: "EducationLevel");

        migrationBuilder.DropIndex(
            name: "IX_Employee_EducationLevelId",
            table: "Employee");

        migrationBuilder.DropColumn(
            name: "EducationLevelId",
            table: "Employee");
    }
}
