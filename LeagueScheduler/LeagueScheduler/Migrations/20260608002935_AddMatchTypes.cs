using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeagueScheduler.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Create the MatchTypes table first.
            migrationBuilder.CreateTable(
                name: "MatchTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    MinPlayersPerCourt = table.Column<int>(type: "integer", nullable: false),
                    MaxPlayersPerCourt = table.Column<int>(type: "integer", nullable: false),
                    MustHaveEvenPlayers = table.Column<bool>(type: "boolean", nullable: false),
                    IsBuiltIn = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchTypes", x => x.Id);
                });

            // 2. Seed the five built-in match types.
            migrationBuilder.InsertData(
                table: "MatchTypes",
                columns: new[] { "Id", "Description", "IsBuiltIn", "MaxPlayersPerCourt", "MinPlayersPerCourt", "MustHaveEvenPlayers", "Name", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("4b6f3f9e-0002-4000-a000-000000000001"), "Singles", true, 2, 2, true, "Singles", 1 },
                    { new Guid("4b6f3f9e-0002-4000-a000-000000000002"), "Doubles", true, 4, 4, true, "Doubles", 2 },
                    { new Guid("4b6f3f9e-0002-4000-a000-000000000003"), "Three player tennis where the single player takes on two players. The single player can hit into the entire doubles court where the doubles side can only hit into the singles court. Additional rule modifications include rotating players so that the single player is always serving and counting only wins when serving with a 'first to N wins' rule.", true, 3, 3, false, "Canadian Doubles", 3 },
                    { new Guid("4b6f3f9e-0002-4000-a000-000000000004"), "Also known as 'two-ball live.' A point starts with both players on one side simultaneously feeding a ball and playing a singles point until one pair misses, at which point a player on the pair that misses yells 'Dingles!' or 'Live!' where it becomes a standard doubles point. Singles play can be down the line or cross-court. Simultaneous 'Dingles' (both balls miss at the same time) results in no-point and the point starts again. Sides alternate feeding either every point or after an agreed upon number of points.", true, 4, 4, true, "Dingles", 4 },
                    { new Guid("4b6f3f9e-0002-4000-a000-000000000005"), "A format where there are more players than court slots. Players rotate on an agreed upon basis, usually after a fixed time or fixed number of games. If score is kept it is usually on a player-by-player basis by counting games that the player's team won. If there are only four players then the players can either play standard doubles matches or rotate.", true, 8, 4, false, "Round-Robin", 5 }
                });

            // 3. Add the MatchTypeId FK column, defaulting existing rows to Doubles.
            migrationBuilder.AddColumn<Guid>(
                name: "MatchTypeId",
                table: "Leagues",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("4b6f3f9e-0002-4000-a000-000000000002"));

            // 4. Migrate existing leagues: Singles → Singles GUID; everything else already defaulted to Doubles.
            migrationBuilder.Sql("""
                UPDATE "Leagues"
                SET "MatchTypeId" = '4b6f3f9e-0002-4000-a000-000000000001'
                WHERE "MatchType" = 'Singles';
                """);

            // 5. Drop the legacy MatchType text column.
            migrationBuilder.DropColumn(
                name: "MatchType",
                table: "Leagues");

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_MatchTypeId",
                table: "Leagues",
                column: "MatchTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTypes_Name",
                table: "MatchTypes",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Leagues_MatchTypes_MatchTypeId",
                table: "Leagues",
                column: "MatchTypeId",
                principalTable: "MatchTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leagues_MatchTypes_MatchTypeId",
                table: "Leagues");

            migrationBuilder.DropIndex(
                name: "IX_Leagues_MatchTypeId",
                table: "Leagues");

            migrationBuilder.DropIndex(
                name: "IX_MatchTypes_Name",
                table: "MatchTypes");

            migrationBuilder.AddColumn<string>(
                name: "MatchType",
                table: "Leagues",
                type: "text",
                nullable: false,
                defaultValue: "");

            // Restore MatchType text from the FK before dropping.
            migrationBuilder.Sql("""
                UPDATE "Leagues" l
                SET "MatchType" = m."Name"
                FROM "MatchTypes" m
                WHERE l."MatchTypeId" = m."Id";
                """);

            migrationBuilder.DropColumn(
                name: "MatchTypeId",
                table: "Leagues");

            migrationBuilder.DropTable(
                name: "MatchTypes");
        }
    }
}
