using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionTokenActiveMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "39d09860-4a4a-4424-9a67-ec3dd0f67a46");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "85c67793-e06f-44be-8bc5-91b46baadb2f");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "axonscms_session",
                newName: "token");

            migrationBuilder.AlterColumn<string>(
                name: "token",
                table: "axonscms_session",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Create_At", "Create_By", "Name", "NormalizedName", "RoleCode", "Update_At", "Update_By" },
                values: new object[,]
                {
                    { "053c95d0-2749-4e76-8391-80859e5b56b1", null, new DateTimeOffset(new DateTime(2024, 1, 24, 19, 43, 23, 236, DateTimeKind.Unspecified).AddTicks(8267), new TimeSpan(0, 7, 0, 0, 0)), "Configure", "User", "USER", "R02", new DateTimeOffset(new DateTime(2024, 1, 24, 19, 43, 23, 236, DateTimeKind.Unspecified).AddTicks(8268), new TimeSpan(0, 7, 0, 0, 0)), "" },
                    { "e946f4cc-fca1-439c-8f67-55584676103d", null, new DateTimeOffset(new DateTime(2024, 1, 24, 19, 43, 23, 236, DateTimeKind.Unspecified).AddTicks(8231), new TimeSpan(0, 7, 0, 0, 0)), "Configure", "Administrator", "ADMINISTRATOR", "R01", new DateTimeOffset(new DateTime(2024, 1, 24, 19, 43, 23, 236, DateTimeKind.Unspecified).AddTicks(8262), new TimeSpan(0, 7, 0, 0, 0)), "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "053c95d0-2749-4e76-8391-80859e5b56b1");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e946f4cc-fca1-439c-8f67-55584676103d");

            migrationBuilder.RenameColumn(
                name: "token",
                table: "axonscms_session",
                newName: "Token");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "axonscms_session",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Create_At", "Create_By", "Name", "NormalizedName", "RoleCode", "Update_At", "Update_By" },
                values: new object[,]
                {
                    { "39d09860-4a4a-4424-9a67-ec3dd0f67a46", null, new DateTimeOffset(new DateTime(2024, 1, 24, 19, 39, 0, 470, DateTimeKind.Unspecified).AddTicks(3121), new TimeSpan(0, 7, 0, 0, 0)), "Configure", "User", "USER", "R02", new DateTimeOffset(new DateTime(2024, 1, 24, 19, 39, 0, 470, DateTimeKind.Unspecified).AddTicks(3123), new TimeSpan(0, 7, 0, 0, 0)), "" },
                    { "85c67793-e06f-44be-8bc5-91b46baadb2f", null, new DateTimeOffset(new DateTime(2024, 1, 24, 19, 39, 0, 470, DateTimeKind.Unspecified).AddTicks(3066), new TimeSpan(0, 7, 0, 0, 0)), "Configure", "Administrator", "ADMINISTRATOR", "R01", new DateTimeOffset(new DateTime(2024, 1, 24, 19, 39, 0, 470, DateTimeKind.Unspecified).AddTicks(3110), new TimeSpan(0, 7, 0, 0, 0)), "" }
                });
        }
    }
}
