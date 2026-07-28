using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace AppLogistics.Data.Migrations;

public partial class AddRate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Rate",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                CreationDate = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 32, nullable: false),
                ClientId = table.Column<int>(nullable: false),
                ActivityId = table.Column<int>(nullable: false),
                VehicleTypeId = table.Column<int>(nullable: true),
                Price = table.Column<decimal>(nullable: false),
                EmployeePercentage = table.Column<float>(nullable: false),
                SplitFare = table.Column<bool>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Rate", x => x.Id);
                table.ForeignKey(
                    name: "FK_Rate_Activity_ActivityId",
                    column: x => x.ActivityId,
                    principalTable: "Activity",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Rate_Client_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Client",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Rate_VehicleType_VehicleTypeId",
                    column: x => x.VehicleTypeId,
                    principalTable: "VehicleType",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Rate_ActivityId",
            table: "Rate",
            column: "ActivityId");

        migrationBuilder.CreateIndex(
            name: "IX_Rate_ClientId",
            table: "Rate",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            name: "IX_Rate_VehicleTypeId",
            table: "Rate",
            column: "VehicleTypeId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Rate");
    }
}
