using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeagueScheduler.Migrations
{
    /// <inheritdoc />
    public partial class AddSupportedTimeZones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupportedTimeZones",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UtcOffsetHours = table.Column<double>(type: "double precision", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportedTimeZones", x => x.Id);
                });

            // Seed common time zones; all enabled by default — admins can disable as needed.
            migrationBuilder.InsertData("SupportedTimeZones",
                columns: ["Id", "DisplayName", "UtcOffsetHours", "IsEnabled"],
                values: new object[,]
                {
                    // UTC
                    { "UTC",                              "Coordinated Universal Time",          0.0,   true },
                    // Americas
                    { "America/St_Johns",                 "Newfoundland",                       -3.5,   true },
                    { "America/Halifax",                  "Atlantic Time (Canada)",              -4.0,   true },
                    { "America/New_York",                 "Eastern Time (US & Canada)",          -5.0,   true },
                    { "America/Toronto",                  "Eastern Time (Canada)",               -5.0,   true },
                    { "America/Bogota",                   "Bogota, Lima, Quito",                 -5.0,   true },
                    { "America/Chicago",                  "Central Time (US & Canada)",          -6.0,   true },
                    { "America/Mexico_City",              "Mexico City, Guadalajara",            -6.0,   true },
                    { "America/Denver",                   "Mountain Time (US & Canada)",         -7.0,   true },
                    { "America/Phoenix",                  "Arizona (no DST)",                    -7.0,   true },
                    { "America/Los_Angeles",              "Pacific Time (US & Canada)",          -8.0,   true },
                    { "America/Vancouver",                "Pacific Time (Canada)",               -8.0,   true },
                    { "America/Anchorage",                "Alaska",                             -9.0,   true },
                    { "Pacific/Honolulu",                 "Hawaii",                             -10.0,  true },
                    { "America/Sao_Paulo",                "Brasilia",                            -3.0,   true },
                    { "America/Argentina/Buenos_Aires",   "Buenos Aires",                        -3.0,   true },
                    // Europe
                    { "Atlantic/Azores",                  "Azores",                              -1.0,   true },
                    { "Europe/London",                    "London, Edinburgh, Dublin",            0.0,   true },
                    { "Europe/Lisbon",                    "Lisbon",                               0.0,   true },
                    { "Europe/Paris",                     "Paris, Brussels, Madrid",              1.0,   true },
                    { "Europe/Berlin",                    "Berlin, Frankfurt, Vienna",            1.0,   true },
                    { "Europe/Rome",                      "Rome, Milan",                          1.0,   true },
                    { "Europe/Amsterdam",                 "Amsterdam, Copenhagen, Stockholm",     1.0,   true },
                    { "Europe/Warsaw",                    "Warsaw, Prague",                       1.0,   true },
                    { "Europe/Athens",                    "Athens, Bucharest",                    2.0,   true },
                    { "Europe/Helsinki",                  "Helsinki, Tallinn, Riga",              2.0,   true },
                    { "Europe/Istanbul",                  "Istanbul",                             3.0,   true },
                    { "Europe/Moscow",                    "Moscow, St. Petersburg",               3.0,   true },
                    // Africa
                    { "Africa/Lagos",                     "West Central Africa",                  1.0,   true },
                    { "Africa/Cairo",                     "Cairo",                                2.0,   true },
                    { "Africa/Johannesburg",              "Johannesburg, Pretoria",               2.0,   true },
                    { "Africa/Nairobi",                   "Nairobi",                              3.0,   true },
                    // Middle East / Asia
                    { "Asia/Dubai",                       "Dubai, Abu Dhabi",                     4.0,   true },
                    { "Asia/Kabul",                       "Kabul",                                4.5,   true },
                    { "Asia/Karachi",                     "Karachi, Islamabad",                   5.0,   true },
                    { "Asia/Kolkata",                     "India (Kolkata)",                      5.5,   true },
                    { "Asia/Colombo",                     "Sri Lanka",                            5.5,   true },
                    { "Asia/Dhaka",                       "Dhaka",                                6.0,   true },
                    { "Asia/Yangon",                      "Rangoon (Yangon)",                     6.5,   true },
                    { "Asia/Bangkok",                     "Bangkok, Hanoi, Jakarta",              7.0,   true },
                    { "Asia/Singapore",                   "Singapore, Kuala Lumpur",              8.0,   true },
                    { "Asia/Shanghai",                    "Beijing, Chongqing, Shanghai",         8.0,   true },
                    { "Asia/Hong_Kong",                   "Hong Kong",                            8.0,   true },
                    { "Asia/Taipei",                      "Taipei",                               8.0,   true },
                    { "Asia/Seoul",                       "Seoul",                                9.0,   true },
                    { "Asia/Tokyo",                       "Tokyo, Osaka",                         9.0,   true },
                    // Australia / Pacific
                    { "Australia/Perth",                  "Perth",                                8.0,   true },
                    { "Australia/Darwin",                 "Darwin",                               9.5,   true },
                    { "Australia/Adelaide",               "Adelaide",                             9.5,   true },
                    { "Australia/Brisbane",               "Brisbane",                            10.0,   true },
                    { "Australia/Sydney",                 "Sydney, Canberra",                    10.0,   true },
                    { "Australia/Melbourne",              "Melbourne",                           10.0,   true },
                    { "Pacific/Auckland",                 "Auckland, Wellington",                12.0,   true },
                    { "Pacific/Fiji",                     "Fiji",                                12.0,   true },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportedTimeZones");
        }
    }
}
