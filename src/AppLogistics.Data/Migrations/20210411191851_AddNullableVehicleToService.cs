using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations;

public partial class AddNullableVehicleToService : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "VehicleTypeId",
            table: "Service",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Service_VehicleTypeId",
            table: "Service",
            column: "VehicleTypeId");

        migrationBuilder.AddForeignKey(
            name: "FK_Service_VehicleType_VehicleTypeId",
            table: "Service",
            column: "VehicleTypeId",
            principalTable: "VehicleType",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Service_VehicleType_VehicleTypeId",
            table: "Service");

        migrationBuilder.DropIndex(
            name: "IX_Service_VehicleTypeId",
            table: "Service");

        migrationBuilder.DropColumn(
            name: "VehicleTypeId",
            table: "Service");
    }
}
