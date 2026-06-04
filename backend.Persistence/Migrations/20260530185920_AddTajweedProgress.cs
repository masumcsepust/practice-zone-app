using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTajweedProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TajweedPracticeRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LetterId = table.Column<int>(type: "integer", nullable: false),
                    Mode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Score = table.Column<double>(type: "double precision", precision: 5, scale: 2, nullable: false),
                    AccuracyScore = table.Column<double>(type: "double precision", precision: 5, scale: 2, nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    PracticedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TajweedPracticeRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TajweedPracticeRecords_ArabicLetters_LetterId",
                        column: x => x.LetterId,
                        principalTable: "ArabicLetters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TajweedPracticeRecords_LetterId_Mode",
                table: "TajweedPracticeRecords",
                columns: new[] { "LetterId", "Mode" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TajweedPracticeRecords");
        }
    }
}
