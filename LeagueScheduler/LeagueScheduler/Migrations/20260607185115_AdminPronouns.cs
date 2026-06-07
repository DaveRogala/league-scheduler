using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeagueScheduler.Migrations
{
    /// <inheritdoc />
    public partial class AdminPronouns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Create and seed the new table first so data migration can reference it.
            migrationBuilder.CreateTable(
                name: "SupportedPronouns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    Label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsBuiltIn = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportedPronouns", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SupportedPronouns",
                columns: new[] { "Id", "IsBuiltIn", "Label", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa1"), true, "He / Him / His", 1 },
                    { new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa2"), true, "She / Her / Hers", 2 },
                    { new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa3"), true, "They / Them / Theirs", 3 }
                });

            // 2. Add the new FK column.
            migrationBuilder.AddColumn<Guid>(
                name: "PronounsId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            // 3. Migrate existing enum values to the new FK.
            //    0 = HisHim, 1 = HerHers, 2 = Other (→ null PronounsId, PronounsCustom kept as-is).
            migrationBuilder.Sql("""
                UPDATE "AspNetUsers"
                SET "PronounsId" = '3fa85f64-5717-4562-b3fc-2c963f66afa1'
                WHERE "Pronouns" = 0;

                UPDATE "AspNetUsers"
                SET "PronounsId" = '3fa85f64-5717-4562-b3fc-2c963f66afa2'
                WHERE "Pronouns" = 1;
                """);

            // 4. Drop the legacy integer enum column.
            migrationBuilder.DropColumn(
                name: "Pronouns",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Pronouns",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "PronounsId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SupportedPronouns");
        }
    }
}
