using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeagueScheduler.Migrations
{
    /// <inheritdoc />
    public partial class AddCountriesAndRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Create Countries and CountryRegions tables first.
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsBuiltIn = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    Region1Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Region1UseList = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayRegion2 = table.Column<bool>(type: "boolean", nullable: false),
                    Region2Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CountryRegions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryRegions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryRegions_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // 2. Seed built-in countries.
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "DisplayRegion2", "IsBuiltIn", "Name", "Region1Name", "Region1UseList", "Region2Name", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("4b6f3f9e-0001-4000-a000-000000000001"), "US", false, true, "United States", "State", true, null, 1 },
                    { new Guid("4b6f3f9e-0001-4000-a000-000000000002"), "CA", false, true, "Canada", "Province", true, null, 2 }
                });

            // 3. Seed US states and Canadian provinces/territories via SQL.
            migrationBuilder.Sql("""
                INSERT INTO "CountryRegions" ("Id", "CountryId", "Code", "Name", "SortOrder") VALUES
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'AL', 'Alabama', 1),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'AK', 'Alaska', 2),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'AZ', 'Arizona', 3),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'AR', 'Arkansas', 4),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'CA', 'California', 5),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'CO', 'Colorado', 6),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'CT', 'Connecticut', 7),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'DE', 'Delaware', 8),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'DC', 'District of Columbia', 9),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'FL', 'Florida', 10),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'GA', 'Georgia', 11),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'HI', 'Hawaii', 12),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'ID', 'Idaho', 13),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'IL', 'Illinois', 14),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'IN', 'Indiana', 15),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'IA', 'Iowa', 16),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'KS', 'Kansas', 17),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'KY', 'Kentucky', 18),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'LA', 'Louisiana', 19),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'ME', 'Maine', 20),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'MD', 'Maryland', 21),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'MA', 'Massachusetts', 22),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'MI', 'Michigan', 23),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'MN', 'Minnesota', 24),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'MS', 'Mississippi', 25),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'MO', 'Missouri', 26),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'MT', 'Montana', 27),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'NE', 'Nebraska', 28),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'NV', 'Nevada', 29),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'NH', 'New Hampshire', 30),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'NJ', 'New Jersey', 31),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'NM', 'New Mexico', 32),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'NY', 'New York', 33),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'NC', 'North Carolina', 34),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'ND', 'North Dakota', 35),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'OH', 'Ohio', 36),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'OK', 'Oklahoma', 37),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'OR', 'Oregon', 38),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'PA', 'Pennsylvania', 39),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'RI', 'Rhode Island', 40),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'SC', 'South Carolina', 41),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'SD', 'South Dakota', 42),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'TN', 'Tennessee', 43),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'TX', 'Texas', 44),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'UT', 'Utah', 45),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'VT', 'Vermont', 46),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'VA', 'Virginia', 47),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'WA', 'Washington', 48),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'WV', 'West Virginia', 49),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'WI', 'Wisconsin', 50),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000001', 'WY', 'Wyoming', 51),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000002', 'AB', 'Alberta', 1),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000002', 'BC', 'British Columbia', 2),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000002', 'MB', 'Manitoba', 3),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000002', 'NB', 'New Brunswick', 4),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000002', 'NL', 'Newfoundland and Labrador', 5),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000002', 'NT', 'Northwest Territories', 6),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000002', 'NS', 'Nova Scotia', 7),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000002', 'NU', 'Nunavut', 8),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000002', 'ON', 'Ontario', 9),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000002', 'PE', 'Prince Edward Island', 10),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000002', 'QC', 'Quebec', 11),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000002', 'SK', 'Saskatchewan', 12),
                (uuid_generate_v1mc(), '4b6f3f9e-0001-4000-a000-000000000002', 'YT', 'Yukon', 13);
                """);

            // 4. Add new FK columns to Addresses.
            migrationBuilder.AddColumn<Guid>(
                name: "AdminAreaId",
                table: "Addresses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Addresses",
                type: "uuid",
                nullable: true);

            // 5. Migrate existing CountryCode values to the new CountryId FK.
            migrationBuilder.Sql("""
                UPDATE "Addresses"
                SET "CountryId" = '4b6f3f9e-0001-4000-a000-000000000001'
                WHERE "CountryCode" = 'US';

                UPDATE "Addresses"
                SET "CountryId" = '4b6f3f9e-0001-4000-a000-000000000002'
                WHERE "CountryCode" = 'CA';
                """);

            // 6. Drop the legacy text CountryCode column.
            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AdminAreaId",
                table: "Addresses",
                column: "AdminAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryId",
                table: "Addresses",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Code",
                table: "Countries",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountryRegions_CountryId_Code",
                table: "CountryRegions",
                columns: new[] { "CountryId", "Code" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Countries_CountryId",
                table: "Addresses",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_CountryRegions_AdminAreaId",
                table: "Addresses",
                column: "AdminAreaId",
                principalTable: "CountryRegions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Countries_CountryId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_CountryRegions_AdminAreaId",
                table: "Addresses");

            migrationBuilder.DropTable(
                name: "CountryRegions");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_AdminAreaId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CountryId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "AdminAreaId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Addresses",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true);
        }
    }
}
