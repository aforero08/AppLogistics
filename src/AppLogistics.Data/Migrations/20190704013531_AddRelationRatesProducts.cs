using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations;

public partial class AddRelationRatesProducts : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "ProductId",
            table: "Rate",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Rate_ProductId",
            table: "Rate",
            column: "ProductId");

        migrationBuilder.AddForeignKey(
            name: "FK_Rate_Product_ProductId",
            table: "Rate",
            column: "ProductId",
            principalTable: "Product",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Rate_Product_ProductId",
            table: "Rate");

        migrationBuilder.DropIndex(
            name: "IX_Rate_ProductId",
            table: "Rate");

        migrationBuilder.DropColumn(
            name: "ProductId",
            table: "Rate");
    }
}
