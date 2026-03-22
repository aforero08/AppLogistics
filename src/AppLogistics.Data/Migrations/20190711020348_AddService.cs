using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations;

public partial class AddService : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Service",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                CreationDate = table.Column<DateTime>(nullable: false),
                RateId = table.Column<int>(nullable: false),
                Quantity = table.Column<int>(nullable: false),
                CarrierId = table.Column<int>(nullable: true),
                VehicleNumber = table.Column<string>(maxLength: 16, nullable: true),
                Location = table.Column<string>(maxLength: 32, nullable: false),
                CustomsInformation = table.Column<string>(maxLength: 32, nullable: true),
                FullPrice = table.Column<decimal>(nullable: false),
                HoldingPrice = table.Column<decimal>(nullable: false),
                Comments = table.Column<string>(maxLength: 128, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Service", x => x.Id);
                table.ForeignKey(
                    name: "FK_Service_Carrier_CarrierId",
                    column: x => x.CarrierId,
                    principalTable: "Carrier",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Service_Rate_RateId",
                    column: x => x.RateId,
                    principalTable: "Rate",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Service_CarrierId",
            table: "Service",
            column: "CarrierId");

        migrationBuilder.CreateIndex(
            name: "IX_Service_RateId",
            table: "Service",
            column: "RateId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Service");
    }
}
