using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppForGame.Migrations
{
    /// <inheritdoc />
    public partial class remaketimestamptodatetime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "time",
                table: "userlog_in");

            migrationBuilder.DropColumn(
                name: "time",
                table: "log_gameover");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "userlog_in",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "userlog_in",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "SerialNumbers",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "serial_number",
                table: "SerialNumbers",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "log_gameover",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "log_gameover",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "userlog_in");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "log_gameover");

            migrationBuilder.UpdateData(
                table: "userlog_in",
                keyColumn: "user_id",
                keyValue: null,
                column: "user_id",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "userlog_in",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "time",
                table: "userlog_in",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "SerialNumbers",
                keyColumn: "user_id",
                keyValue: null,
                column: "user_id",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "SerialNumbers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "SerialNumbers",
                keyColumn: "serial_number",
                keyValue: null,
                column: "serial_number",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "serial_number",
                table: "SerialNumbers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "log_gameover",
                keyColumn: "user_id",
                keyValue: null,
                column: "user_id",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "log_gameover",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "time",
                table: "log_gameover",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
