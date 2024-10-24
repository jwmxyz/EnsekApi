using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ensek.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "CreatedAt", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1234, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9010), new TimeSpan(0, 0, 0, 0, 0)), "Freya", "Test" },
                    { 1239, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9100), new TimeSpan(0, 0, 0, 0, 0)), "Noddy", "Test" },
                    { 1240, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9210), new TimeSpan(0, 0, 0, 0, 0)), "Archie", "Test" },
                    { 1241, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9230), new TimeSpan(0, 0, 0, 0, 0)), "Lara", "Test" },
                    { 1242, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9240), new TimeSpan(0, 0, 0, 0, 0)), "Tim", "Test" },
                    { 1243, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9260), new TimeSpan(0, 0, 0, 0, 0)), "Graham", "Test" },
                    { 1244, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9270), new TimeSpan(0, 0, 0, 0, 0)), "Tony", "Test" },
                    { 1245, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9290), new TimeSpan(0, 0, 0, 0, 0)), "Neville", "Test" },
                    { 1246, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9300), new TimeSpan(0, 0, 0, 0, 0)), "Jo", "Test" },
                    { 1247, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9310), new TimeSpan(0, 0, 0, 0, 0)), "Jim", "Test" },
                    { 1248, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9330), new TimeSpan(0, 0, 0, 0, 0)), "Pam", "Test" },
                    { 2233, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9340), new TimeSpan(0, 0, 0, 0, 0)), "Barry", "Test" },
                    { 2344, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9350), new TimeSpan(0, 0, 0, 0, 0)), "Tommy", "Test" },
                    { 2345, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9370), new TimeSpan(0, 0, 0, 0, 0)), "Jerry", "Test" },
                    { 2346, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9380), new TimeSpan(0, 0, 0, 0, 0)), "Ollie", "Test" },
                    { 2347, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9390), new TimeSpan(0, 0, 0, 0, 0)), "Tara", "Test" },
                    { 2348, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9410), new TimeSpan(0, 0, 0, 0, 0)), "Tammy", "Test" },
                    { 2349, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9420), new TimeSpan(0, 0, 0, 0, 0)), "Simon", "Test" },
                    { 2350, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9430), new TimeSpan(0, 0, 0, 0, 0)), "Colin", "Test" },
                    { 2351, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9450), new TimeSpan(0, 0, 0, 0, 0)), "Gladys", "Test" },
                    { 2352, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9460), new TimeSpan(0, 0, 0, 0, 0)), "Greg", "Test" },
                    { 2353, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9470), new TimeSpan(0, 0, 0, 0, 0)), "Tony", "Test" },
                    { 2355, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9490), new TimeSpan(0, 0, 0, 0, 0)), "Arthur", "Test" },
                    { 2356, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9500), new TimeSpan(0, 0, 0, 0, 0)), "Craig", "Test" },
                    { 4534, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9510), new TimeSpan(0, 0, 0, 0, 0)), "JOSH", "TEST" },
                    { 6776, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9520), new TimeSpan(0, 0, 0, 0, 0)), "Laura", "Test" },
                    { 8766, new DateTimeOffset(new DateTime(2024, 10, 24, 10, 54, 13, 167, DateTimeKind.Unspecified).AddTicks(9630), new TimeSpan(0, 0, 0, 0, 0)), "Sally", "Test" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1234);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1239);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1240);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1241);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1242);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1243);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1244);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1245);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1246);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1247);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1248);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2233);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2344);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2345);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2346);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2347);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2348);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2349);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2350);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2351);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2352);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2353);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2355);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2356);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4534);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 6776);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 8766);
        }
    }
}
