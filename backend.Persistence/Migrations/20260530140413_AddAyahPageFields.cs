using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAyahPageFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HizbQuarter",
                table: "Ayahs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Juz",
                table: "Ayahs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Manzil",
                table: "Ayahs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Page",
                table: "Ayahs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ruku",
                table: "Ayahs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Sajda",
                table: "Ayahs",
                type: "boolean",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ayahs_Page",
                table: "Ayahs",
                column: "Page");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ayahs_Page",
                table: "Ayahs");

            migrationBuilder.DropColumn(
                name: "HizbQuarter",
                table: "Ayahs");

            migrationBuilder.DropColumn(
                name: "Juz",
                table: "Ayahs");

            migrationBuilder.DropColumn(
                name: "Manzil",
                table: "Ayahs");

            migrationBuilder.DropColumn(
                name: "Page",
                table: "Ayahs");

            migrationBuilder.DropColumn(
                name: "Ruku",
                table: "Ayahs");

            migrationBuilder.DropColumn(
                name: "Sajda",
                table: "Ayahs");
        }
    }
}
