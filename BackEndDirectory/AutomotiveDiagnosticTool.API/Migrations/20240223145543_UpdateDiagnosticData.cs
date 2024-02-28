using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomotiveDiagnosticTool.API.Migrations
{
    public partial class UpdateDiagnosticData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BatteryStatus",
                table: "DiagnosticData",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EngineHealth",
                table: "DiagnosticData",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TirePressure",
                table: "DiagnosticData",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "DiagnosticData",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticData_UserId",
                table: "DiagnosticData",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiagnosticData_Users_UserId",
                table: "DiagnosticData",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiagnosticData_Users_UserId",
                table: "DiagnosticData");

            migrationBuilder.DropIndex(
                name: "IX_DiagnosticData_UserId",
                table: "DiagnosticData");

            migrationBuilder.DropColumn(
                name: "BatteryStatus",
                table: "DiagnosticData");

            migrationBuilder.DropColumn(
                name: "EngineHealth",
                table: "DiagnosticData");

            migrationBuilder.DropColumn(
                name: "TirePressure",
                table: "DiagnosticData");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DiagnosticData");
        }
    }
}
