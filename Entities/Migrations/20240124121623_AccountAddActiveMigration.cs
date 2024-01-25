using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AccountAddActiveMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "0a31701d-c0a1-44c2-a572-96e3febd6127");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "a38297bd-0132-4fd6-b29f-e963ddaa92c3");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Accounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "axonscms_session",
                columns: table => new
                {
                    session_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    account_id = table.Column<string>(type: "text", nullable: false),
                    login_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    platform = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    os = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    browser = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    login_ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    create_by = table.Column<string>(type: "character varying", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_by = table.Column<string>(type: "character varying", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    issued_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expiration_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    session_status = table.Column<string>(type: "character varying", nullable: false, defaultValueSql: "'A'::bpchar", comment: "B (Blocked): Session ยังไม่ได้ใช้งาน\r\nA (Active): Session กำลังใช้งานอยู่\r\nE (Expired): Session หมดอายุแล้ว")
                },
                constraints: table =>
                {
                    table.PrimaryKey("cms_session_pk", x => x.session_id);
                });

            migrationBuilder.CreateTable(
                name: "block_bruteforce",
                columns: table => new
                {
                    block_force_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    username = table.Column<string>(type: "character varying", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    status = table.Column<string>(type: "character varying", nullable: false, defaultValueSql: "'A'::character varying", comment: "L (Locked): ถูกล็อก\r\nU (UnLock): ไม่ล็อก"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    locked_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    un_lock_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("block_bruteforce_pk", x => x.block_force_id);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Create_At", "Create_By", "Name", "NormalizedName", "RoleCode", "Update_At", "Update_By" },
                values: new object[,]
                {
                    { "4a3d4f58-52bc-46d9-8a4f-f25f514b2215", null, new DateTimeOffset(new DateTime(2024, 1, 24, 19, 16, 20, 962, DateTimeKind.Unspecified).AddTicks(6697), new TimeSpan(0, 7, 0, 0, 0)), "Configure", "Administrator", "ADMINISTRATOR", "R01", new DateTimeOffset(new DateTime(2024, 1, 24, 19, 16, 20, 962, DateTimeKind.Unspecified).AddTicks(6749), new TimeSpan(0, 7, 0, 0, 0)), "" },
                    { "cbec45d9-2dfe-48fc-9238-e1d7cd8a7402", null, new DateTimeOffset(new DateTime(2024, 1, 24, 19, 16, 20, 962, DateTimeKind.Unspecified).AddTicks(6757), new TimeSpan(0, 7, 0, 0, 0)), "Configure", "User", "USER", "R02", new DateTimeOffset(new DateTime(2024, 1, 24, 19, 16, 20, 962, DateTimeKind.Unspecified).AddTicks(6759), new TimeSpan(0, 7, 0, 0, 0)), "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "axonscms_session");

            migrationBuilder.DropTable(
                name: "block_bruteforce");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4a3d4f58-52bc-46d9-8a4f-f25f514b2215");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "cbec45d9-2dfe-48fc-9238-e1d7cd8a7402");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Accounts");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Create_At", "Create_By", "Name", "NormalizedName", "RoleCode", "Update_At", "Update_By" },
                values: new object[,]
                {
                    { "0a31701d-c0a1-44c2-a572-96e3febd6127", null, new DateTimeOffset(new DateTime(2024, 1, 22, 15, 58, 43, 709, DateTimeKind.Unspecified).AddTicks(8651), new TimeSpan(0, 7, 0, 0, 0)), "Configure", "Administrator", "ADMINISTRATOR", "R01", new DateTimeOffset(new DateTime(2024, 1, 22, 15, 58, 43, 709, DateTimeKind.Unspecified).AddTicks(8698), new TimeSpan(0, 7, 0, 0, 0)), "" },
                    { "a38297bd-0132-4fd6-b29f-e963ddaa92c3", null, new DateTimeOffset(new DateTime(2024, 1, 22, 15, 58, 43, 709, DateTimeKind.Unspecified).AddTicks(8704), new TimeSpan(0, 7, 0, 0, 0)), "Configure", "User", "USER", "R02", new DateTimeOffset(new DateTime(2024, 1, 22, 15, 58, 43, 709, DateTimeKind.Unspecified).AddTicks(8705), new TimeSpan(0, 7, 0, 0, 0)), "" }
                });
        }
    }
}
