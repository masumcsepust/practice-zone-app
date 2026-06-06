using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class syllable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransliterationText",
                table: "SyllableSounds",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "SyllableSounds",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"),
                column: "TransliterationText",
                value: "");

            migrationBuilder.UpdateData(
                table: "SyllableSounds",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"),
                column: "TransliterationText",
                value: "");

            migrationBuilder.UpdateData(
                table: "SyllableSounds",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000003"),
                column: "TransliterationText",
                value: "");

            migrationBuilder.UpdateData(
                table: "SyllableSounds",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000004"),
                column: "TransliterationText",
                value: "");

            migrationBuilder.UpdateData(
                table: "SyllableSounds",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000005"),
                column: "TransliterationText",
                value: "");

            migrationBuilder.UpdateData(
                table: "SyllableSounds",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000006"),
                column: "TransliterationText",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransliterationText",
                table: "SyllableSounds");
        }
    }
}
