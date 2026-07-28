using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace AppLogistics.Data.Migrations;

public partial class AddEmployee : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Employee",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                CreationDate = table.Column<DateTime>(nullable: false),
                InternalCode = table.Column<string>(maxLength: 16, nullable: false),
                BranchOfficeId = table.Column<int>(nullable: false),
                DocumentTypeId = table.Column<int>(nullable: false),
                DocumentNumber = table.Column<string>(maxLength: 16, nullable: false),
                Name = table.Column<string>(maxLength: 32, nullable: false),
                Surname = table.Column<string>(maxLength: 32, nullable: false),
                BornDate = table.Column<DateTime>(nullable: false),
                HireDate = table.Column<DateTime>(nullable: false),
                RetirementDate = table.Column<DateTime>(nullable: true),
                ResidenceCity = table.Column<string>(maxLength: 32, nullable: false),
                Address = table.Column<string>(maxLength: 128, nullable: false),
                MobilePhone = table.Column<string>(maxLength: 16, nullable: false),
                HomePhone = table.Column<string>(maxLength: 16, nullable: true),
                Email = table.Column<string>(maxLength: 128, nullable: true),
                MaritalStatusId = table.Column<int>(nullable: false),
                EmergencyContact = table.Column<string>(maxLength: 32, nullable: true),
                EmergencyContactPhone = table.Column<string>(maxLength: 16, nullable: true),
                AfpId = table.Column<int>(nullable: false),
                EpsId = table.Column<int>(nullable: false),
                HasCurriculumVitae = table.Column<bool>(nullable: false),
                HasDocumentCopy = table.Column<bool>(nullable: false),
                HasPhotos = table.Column<bool>(nullable: false),
                HasMilitaryIdCopy = table.Column<bool>(nullable: false),
                HasLaborCertification = table.Column<bool>(nullable: false),
                HasPersonalReference = table.Column<bool>(nullable: false),
                HasDisciplinaryBackground = table.Column<bool>(nullable: false),
                HasKnowledgeTest = table.Column<bool>(nullable: false),
                HasAdmissionTest = table.Column<bool>(nullable: false),
                HasContract = table.Column<bool>(nullable: false),
                HasInternalRegulations = table.Column<bool>(nullable: false),
                HasEndownmentLetter = table.Column<bool>(nullable: false),
                IsCriticalPosition = table.Column<bool>(nullable: false),
                Comments = table.Column<string>(maxLength: 512, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Employee", x => x.Id);
                table.ForeignKey(
                    name: "FK_Employee_Afp_AfpId",
                    column: x => x.AfpId,
                    principalTable: "Afp",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Employee_BranchOffice_BranchOfficeId",
                    column: x => x.BranchOfficeId,
                    principalTable: "BranchOffice",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Employee_DocumentType_DocumentTypeId",
                    column: x => x.DocumentTypeId,
                    principalTable: "DocumentType",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Employee_Eps_EpsId",
                    column: x => x.EpsId,
                    principalTable: "Eps",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Employee_MaritalStatus_MaritalStatusId",
                    column: x => x.MaritalStatusId,
                    principalTable: "MaritalStatus",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Employee_AfpId",
            table: "Employee",
            column: "AfpId");

        migrationBuilder.CreateIndex(
            name: "IX_Employee_BranchOfficeId",
            table: "Employee",
            column: "BranchOfficeId");

        migrationBuilder.CreateIndex(
            name: "IX_Employee_DocumentTypeId",
            table: "Employee",
            column: "DocumentTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_Employee_EpsId",
            table: "Employee",
            column: "EpsId");

        migrationBuilder.CreateIndex(
            name: "IX_Employee_InternalCode",
            table: "Employee",
            column: "InternalCode",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Employee_MaritalStatusId",
            table: "Employee",
            column: "MaritalStatusId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Employee");
    }
}
