using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueScheduler.Migrations
{
    /// <inheritdoc />
    public partial class AddFullEntityModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SeasonId",
                table: "ScheduleResults",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CourtId",
                table: "ScheduleMatches",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Courts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AddressLine1 = table.Column<string>(type: "text", nullable: true),
                    AddressLine2 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    StateProvince = table.Column<string>(type: "text", nullable: true),
                    PostalCode = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false),
                    NumberOfCourts = table.Column<int>(type: "integer", nullable: false),
                    AccessNotes = table.Column<string>(type: "text", nullable: true),
                    HoursOfOperation = table.Column<string>(type: "text", nullable: true),
                    Lighted = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Condition = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Mode = table.Column<string>(type: "text", nullable: false),
                    MatchType = table.Column<string>(type: "text", nullable: false),
                    RequireApprovalToJoin = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PreferencePercent = table.Column<double>(type: "double precision", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Nudge = table.Column<string>(type: "text", nullable: false),
                    UnavailableDates = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LeagueId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DaysOfWeek = table.Column<string>(type: "jsonb", nullable: false),
                    NonPlayDates = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seasons_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaguePlayers",
                columns: table => new
                {
                    LeagueId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaguePlayers", x => new { x.LeagueId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_LeaguePlayers_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaguePlayers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrePlannedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SeasonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CourtId = table.Column<Guid>(type: "uuid", nullable: true),
                    PlayerIds = table.Column<string>(type: "jsonb", nullable: false),
                    IsMatch = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrePlannedEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrePlannedEvents_Courts_CourtId",
                        column: x => x.CourtId,
                        principalTable: "Courts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrePlannedEvents_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeasonCourts",
                columns: table => new
                {
                    SeasonId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourtId = table.Column<Guid>(type: "uuid", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonCourts", x => new { x.SeasonId, x.CourtId });
                    table.ForeignKey(
                        name: "FK_SeasonCourts_Courts_CourtId",
                        column: x => x.CourtId,
                        principalTable: "Courts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeasonCourts_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleResults_SeasonId",
                table: "ScheduleResults",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleMatches_CourtId",
                table: "ScheduleMatches",
                column: "CourtId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaguePlayers_PlayerId",
                table: "LeaguePlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PrePlannedEvents_CourtId",
                table: "PrePlannedEvents",
                column: "CourtId");

            migrationBuilder.CreateIndex(
                name: "IX_PrePlannedEvents_SeasonId",
                table: "PrePlannedEvents",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonCourts_CourtId",
                table: "SeasonCourts",
                column: "CourtId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_LeagueId",
                table: "Seasons",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleMatches_Courts_CourtId",
                table: "ScheduleMatches",
                column: "CourtId",
                principalTable: "Courts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleResults_Seasons_SeasonId",
                table: "ScheduleResults",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleMatches_Courts_CourtId",
                table: "ScheduleMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleResults_Seasons_SeasonId",
                table: "ScheduleResults");

            migrationBuilder.DropTable(
                name: "LeaguePlayers");

            migrationBuilder.DropTable(
                name: "PrePlannedEvents");

            migrationBuilder.DropTable(
                name: "SeasonCourts");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Courts");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleResults_SeasonId",
                table: "ScheduleResults");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleMatches_CourtId",
                table: "ScheduleMatches");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "ScheduleResults");

            migrationBuilder.DropColumn(
                name: "CourtId",
                table: "ScheduleMatches");
        }
    }
}
