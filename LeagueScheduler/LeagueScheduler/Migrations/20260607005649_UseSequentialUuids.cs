using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueScheduler.Migrations
{
    /// <inheritdoc />
    public partial class UseSequentialUuids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Seasons",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v1mc()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "SeasonPlayers",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v1mc()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ScheduleResults",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v1mc()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ScheduleMatches",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v1mc()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "PrePlannedEvents",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v1mc()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Leagues",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v1mc()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Courts",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v1mc()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Addresses",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v1mc()",
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Seasons",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_generate_v1mc()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "SeasonPlayers",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_generate_v1mc()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ScheduleResults",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_generate_v1mc()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ScheduleMatches",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_generate_v1mc()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "PrePlannedEvents",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_generate_v1mc()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Leagues",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_generate_v1mc()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Courts",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_generate_v1mc()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Addresses",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_generate_v1mc()");
        }
    }
}
