using Microsoft.EntityFrameworkCore.Migrations;

namespace AppLogistics.Data.Migrations;

public partial class AddEmployeeAttributes : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "Active",
            table: "Employee",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "BirthPlace",
            table: "Employee",
            maxLength: 128,
            nullable: false,
            defaultValue: "Bogotá");

        migrationBuilder.AlterColumn<string>(
            name: "BirthPlace",
            table: "Employee",
            maxLength: 128,
            nullable: false,
            defaultValue: null);

        migrationBuilder.AddColumn<bool>(
            name: "HasResidencePermit",
            table: "Employee",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<int>(
            name: "SocialClass",
            table: "Employee",
            nullable: false,
            defaultValue: 3);

        migrationBuilder.AlterColumn<int>(
            name: "SocialClass",
            table: "Employee",
            nullable: false,
            defaultValue: null);

        migrationBuilder.AddColumn<bool>(
            name: "TrainingBASC",
            table: "Employee",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "TrainingBPM",
            table: "Employee",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "TrainingOthers",
            table: "Employee",
            maxLength: 512,
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "TrainingSGSST",
            table: "Employee",
            nullable: false,
            defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Active",
            table: "Employee");

        migrationBuilder.DropColumn(
            name: "BirthPlace",
            table: "Employee");

        migrationBuilder.DropColumn(
            name: "HasResidencePermit",
            table: "Employee");

        migrationBuilder.DropColumn(
            name: "SocialClass",
            table: "Employee");

        migrationBuilder.DropColumn(
            name: "TrainingBASC",
            table: "Employee");

        migrationBuilder.DropColumn(
            name: "TrainingBPM",
            table: "Employee");

        migrationBuilder.DropColumn(
            name: "TrainingOthers",
            table: "Employee");

        migrationBuilder.DropColumn(
            name: "TrainingSGSST",
            table: "Employee");
    }
}
