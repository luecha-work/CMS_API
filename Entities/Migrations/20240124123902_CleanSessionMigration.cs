using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class CleanSessionMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4a3d4f58-52bc-46d9-8a4f-f25f514b2215");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "cbec45d9-2dfe-48fc-9238-e1d7cd8a7402");

            migrationBuilder.AlterColumn<string>(
                name: "update_by",
                table: "axonscms_session",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying");

            migrationBuilder.AlterColumn<DateTime>(
                name: "update_at",
                table: "axonscms_session",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "platform",
                table: "axonscms_session",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "os",
                table: "axonscms_session",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "create_by",
                table: "axonscms_session",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying");

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_at",
                table: "axonscms_session",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "browser",
                table: "axonscms_session",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "axonscms_session",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Create_At", "Create_By", "Name", "NormalizedName", "RoleCode", "Update_At", "Update_By" },
                values: new object[,]
                {
                    { "39d09860-4a4a-4424-9a67-ec3dd0f67a46", null, new DateTimeOffset(new DateTime(2024, 1, 24, 19, 39, 0, 470, DateTimeKind.Unspecified).AddTicks(3121), new TimeSpan(0, 7, 0, 0, 0)), "Configure", "User", "USER", "R02", new DateTimeOffset(new DateTime(2024, 1, 24, 19, 39, 0, 470, DateTimeKind.Unspecified).AddTicks(3123), new TimeSpan(0, 7, 0, 0, 0)), "" },
                    { "85c67793-e06f-44be-8bc5-91b46baadb2f", null, new DateTimeOffset(new DateTime(2024, 1, 24, 19, 39, 0, 470, DateTimeKind.Unspecified).AddTicks(3066), new TimeSpan(0, 7, 0, 0, 0)), "Configure", "Administrator", "ADMINISTRATOR", "R01", new DateTimeOffset(new DateTime(2024, 1, 24, 19, 39, 0, 470, DateTimeKind.Unspecified).AddTicks(3110), new TimeSpan(0, 7, 0, 0, 0)), "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "39d09860-4a4a-4424-9a67-ec3dd0f67a46");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "85c67793-e06f-44be-8bc5-91b46baadb2f");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "axonscms_session");

            migrationBuilder.AlterColumn<string>(
                name: "update_by",
                table: "axonscms_session",
                type: "character varying",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "update_at",
                table: "axonscms_session",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "platform",
                table: "axonscms_session",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "os",
                table: "axonscms_session",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "create_by",
                table: "axonscms_session",
                type: "character varying",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "create_at",
                table: "axonscms_session",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "browser",
                table: "axonscms_session",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Create_At", "Create_By", "Name", "NormalizedName", "RoleCode", "Update_At", "Update_By" },
                values: new object[,]
                {
                    { "4a3d4f58-52bc-46d9-8a4f-f25f514b2215", null, new DateTimeOffset(new DateTime(2024, 1, 24, 19, 16, 20, 962, DateTimeKind.Unspecified).AddTicks(6697), new TimeSpan(0, 7, 0, 0, 0)), "Configure", "Administrator", "ADMINISTRATOR", "R01", new DateTimeOffset(new DateTime(2024, 1, 24, 19, 16, 20, 962, DateTimeKind.Unspecified).AddTicks(6749), new TimeSpan(0, 7, 0, 0, 0)), "" },
                    { "cbec45d9-2dfe-48fc-9238-e1d7cd8a7402", null, new DateTimeOffset(new DateTime(2024, 1, 24, 19, 16, 20, 962, DateTimeKind.Unspecified).AddTicks(6757), new TimeSpan(0, 7, 0, 0, 0)), "Configure", "User", "USER", "R02", new DateTimeOffset(new DateTime(2024, 1, 24, 19, 16, 20, 962, DateTimeKind.Unspecified).AddTicks(6759), new TimeSpan(0, 7, 0, 0, 0)), "" }
                });
        }
    }
}
