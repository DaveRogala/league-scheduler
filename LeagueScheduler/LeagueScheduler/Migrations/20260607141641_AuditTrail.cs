using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueScheduler.Migrations
{
    /// <inheritdoc />
    public partial class AuditTrail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Seasons",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Seasons",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "Seasons",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Seasons",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Seasons",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedById",
                table: "Seasons",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "SeasonPlayers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "SeasonPlayers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "SeasonPlayers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "SeasonPlayers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "SeasonPlayers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedById",
                table: "SeasonPlayers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Leagues",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Leagues",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "Leagues",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Leagues",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Leagues",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedById",
                table: "Leagues",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "LeaguePlayers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "LeaguePlayers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "LeaguePlayers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "LeaguePlayers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "LeaguePlayers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedById",
                table: "LeaguePlayers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Courts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Courts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "Courts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "Courts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Courts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedById",
                table: "Courts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourtHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    CourtId = table.Column<Guid>(type: "uuid", nullable: false),
                    Operation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ChangedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ChangedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    NumberOfCourts = table.Column<int>(type: "integer", nullable: false),
                    Lighted = table.Column<bool>(type: "boolean", nullable: false),
                    HoursOfOperation = table.Column<string>(type: "text", nullable: true),
                    AccessNotes = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Condition = table.Column<string>(type: "text", nullable: true),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourtHistory", x => x.Id);
                });

            migrationBuilder.Sql("""
                CREATE OR REPLACE FUNCTION fn_court_history()
                RETURNS TRIGGER AS $$
                BEGIN
                    IF TG_OP = 'DELETE' THEN
                        INSERT INTO "CourtHistory" (
                            "CourtId", "Operation", "ChangedAt", "ChangedById",
                            "Name", "Type", "NumberOfCourts", "Lighted",
                            "HoursOfOperation", "AccessNotes", "Description", "Condition", "AddressId"
                        ) VALUES (
                            OLD."Id", TG_OP, now(), OLD."UpdatedById",
                            OLD."Name", OLD."Type", OLD."NumberOfCourts", OLD."Lighted",
                            OLD."HoursOfOperation", OLD."AccessNotes", OLD."Description", OLD."Condition", OLD."AddressId"
                        );
                        RETURN OLD;
                    ELSE
                        INSERT INTO "CourtHistory" (
                            "CourtId", "Operation", "ChangedAt", "ChangedById",
                            "Name", "Type", "NumberOfCourts", "Lighted",
                            "HoursOfOperation", "AccessNotes", "Description", "Condition", "AddressId"
                        ) VALUES (
                            NEW."Id", TG_OP, now(), NEW."UpdatedById",
                            NEW."Name", NEW."Type", NEW."NumberOfCourts", NEW."Lighted",
                            NEW."HoursOfOperation", NEW."AccessNotes", NEW."Description", NEW."Condition", NEW."AddressId"
                        );
                        RETURN NEW;
                    END IF;
                END;
                $$ LANGUAGE plpgsql;

                CREATE TRIGGER tr_court_history
                AFTER INSERT OR UPDATE OR DELETE ON "Courts"
                FOR EACH ROW EXECUTE FUNCTION fn_court_history();
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP TRIGGER IF EXISTS tr_court_history ON "Courts";
                DROP FUNCTION IF EXISTS fn_court_history();
                """);

            migrationBuilder.DropTable(
                name: "CourtHistory");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SeasonPlayers");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "SeasonPlayers");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "SeasonPlayers");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "SeasonPlayers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SeasonPlayers");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "SeasonPlayers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "LeaguePlayers");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "LeaguePlayers");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "LeaguePlayers");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "LeaguePlayers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "LeaguePlayers");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "LeaguePlayers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Courts");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Courts");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Courts");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Courts");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Courts");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Courts");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "AspNetUsers");
        }
    }
}
