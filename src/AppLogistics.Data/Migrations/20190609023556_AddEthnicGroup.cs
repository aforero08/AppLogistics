using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations;

public partial class AddEthnicGroup : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "EthnicGroup",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                CreationDate = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 32, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EthnicGroup", x => x.Id);
            });

        migrationBuilder.InsertData(
            table: "EthnicGroup",
            columns: new [] { "Id", "CreationDate", "Name" },
            values: new object[] { 1, DateTime.Now, "Ninguno" }
            );

        migrationBuilder.AddColumn<int>(
            name: "EthnicGroupId",
            table: "Employee",
            nullable: false,
            defaultValue: 1);

        migrationBuilder.CreateIndex(
            name: "IX_Employee_EthnicGroupId",
            table: "Employee",
            column: "EthnicGroupId");

        migrationBuilder.AddForeignKey(
            name: "FK_Employee_EthnicGroup_EthnicGroupId",
            table: "Employee",
            column: "EthnicGroupId",
            principalTable: "EthnicGroup",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AlterColumn<int>(
            name: "EthnicGroupId",
            table: "Employee",
            nullable: false,
            defaultValue: null);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Employee_EthnicGroup_EthnicGroupId",
            table: "Employee");

        migrationBuilder.DropTable(
            name: "EthnicGroup");

        migrationBuilder.DropIndex(
            name: "IX_Employee_EthnicGroupId",
            table: "Employee");

        migrationBuilder.DropColumn(
            name: "EthnicGroupId",
            table: "Employee");
    }
}
