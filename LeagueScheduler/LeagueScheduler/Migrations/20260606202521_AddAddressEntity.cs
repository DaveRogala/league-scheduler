using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueScheduler.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "Courts");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "Courts");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Courts");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Courts");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Courts");

            migrationBuilder.DropColumn(
                name: "StateProvince",
                table: "Courts");

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "Players",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "Courts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Line1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Line2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Line3 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Locality = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AdminArea = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SubAdminArea = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    VisibleFields = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_AddressId",
                table: "Players",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Courts_AddressId",
                table: "Courts",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courts_Addresses_AddressId",
                table: "Courts",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Addresses_AddressId",
                table: "Players",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courts_Addresses_AddressId",
                table: "Courts");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Addresses_AddressId",
                table: "Players");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Players_AddressId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Courts_AddressId",
                table: "Courts");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Courts");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "Courts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Courts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Courts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Courts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Courts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateProvince",
                table: "Courts",
                type: "text",
                nullable: true);
        }
    }
}
