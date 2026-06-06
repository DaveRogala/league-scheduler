using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueScheduler.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduleResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FairnessToleranceUsed = table.Column<int>(type: "integer", nullable: false),
                    AssignedCounts = table.Column<string>(type: "jsonb", nullable: false),
                    TargetCounts = table.Column<string>(type: "jsonb", nullable: false),
                    Conflicts = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleMatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ScheduleResultId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Court = table.Column<int>(type: "integer", nullable: false),
                    PlayerIds = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleMatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleMatches_ScheduleResults_ScheduleResultId",
                        column: x => x.ScheduleResultId,
                        principalTable: "ScheduleResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleMatches_ScheduleResultId",
                table: "ScheduleMatches",
                column: "ScheduleResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleMatches");

            migrationBuilder.DropTable(
                name: "ScheduleResults");
        }
    }
}
