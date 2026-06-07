using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueScheduler.Migrations
{
    /// <inheritdoc />
    public partial class LowercaseLogNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Logs",
                table: "Logs");

            migrationBuilder.RenameTable(
                name: "Logs",
                newName: "logs");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "logs",
                newName: "timestamp");

            migrationBuilder.RenameColumn(
                name: "Template",
                table: "logs",
                newName: "template");

            migrationBuilder.RenameColumn(
                name: "Properties",
                table: "logs",
                newName: "properties");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "logs",
                newName: "message");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "logs",
                newName: "level");

            migrationBuilder.RenameColumn(
                name: "Exception",
                table: "logs",
                newName: "exception");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "logs",
                newName: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_logs",
                table: "logs",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_logs",
                table: "logs");

            migrationBuilder.RenameTable(
                name: "logs",
                newName: "Logs");

            migrationBuilder.RenameColumn(
                name: "timestamp",
                table: "Logs",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "template",
                table: "Logs",
                newName: "Template");

            migrationBuilder.RenameColumn(
                name: "properties",
                table: "Logs",
                newName: "Properties");

            migrationBuilder.RenameColumn(
                name: "message",
                table: "Logs",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "level",
                table: "Logs",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "exception",
                table: "Logs",
                newName: "Exception");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Logs",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logs",
                table: "Logs",
                column: "Id");
        }
    }
}
