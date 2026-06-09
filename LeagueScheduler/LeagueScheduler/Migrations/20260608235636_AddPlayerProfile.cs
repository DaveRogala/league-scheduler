using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueScheduler.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerProfiles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PrivacyLevel = table.Column<int>(type: "integer", nullable: false),
                    ShowEmail = table.Column<bool>(type: "boolean", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ShowPhone = table.Column<bool>(type: "boolean", nullable: false),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: true),
                    ShowAddress = table.Column<bool>(type: "boolean", nullable: false),
                    ShowCityPostalOnly = table.Column<bool>(type: "boolean", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    GenderDescription = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NtrpRating = table.Column<decimal>(type: "numeric(3,1)", precision: 3, scale: 1, nullable: true),
                    WtnSingles = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    WtnDoubles = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    UtrRating = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    HandPreference = table.Column<int>(type: "integer", nullable: true),
                    PreferredSide = table.Column<int>(type: "integer", nullable: true),
                    PreferredSurface = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerProfiles", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_PlayerProfiles_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PlayerProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerProfiles_AddressId",
                table: "PlayerProfiles",
                column: "AddressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerProfiles");
        }
    }
}
