using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueScheduler.Migrations
{
    /// <inheritdoc />
    public partial class RenamePlayerToSeasonPlayerAndUserProfileFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaguePlayers_Players_PlayerId",
                table: "LeaguePlayers");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "LeaguePlayers",
                newName: "SeasonPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_LeaguePlayers_PlayerId",
                table: "LeaguePlayers",
                newName: "IX_LeaguePlayers_SeasonPlayerId");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreferredTimeZone",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Pronouns",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PronounsCustom",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SeasonPlayers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PreferencePercent = table.Column<double>(type: "double precision", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Nudge = table.Column<string>(type: "text", nullable: false),
                    UnavailableDates = table.Column<string>(type: "jsonb", nullable: false),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeasonPlayers_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeasonPlayers_AddressId",
                table: "SeasonPlayers",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaguePlayers_SeasonPlayers_SeasonPlayerId",
                table: "LeaguePlayers",
                column: "SeasonPlayerId",
                principalTable: "SeasonPlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaguePlayers_SeasonPlayers_SeasonPlayerId",
                table: "LeaguePlayers");

            migrationBuilder.DropTable(
                name: "SeasonPlayers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PreferredTimeZone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Pronouns",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PronounsCustom",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "SeasonPlayerId",
                table: "LeaguePlayers",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_LeaguePlayers_SeasonPlayerId",
                table: "LeaguePlayers",
                newName: "IX_LeaguePlayers_PlayerId");

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Nudge = table.Column<string>(type: "text", nullable: false),
                    PreferencePercent = table.Column<double>(type: "double precision", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    UnavailableDates = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_AddressId",
                table: "Players",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaguePlayers_Players_PlayerId",
                table: "LeaguePlayers",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
