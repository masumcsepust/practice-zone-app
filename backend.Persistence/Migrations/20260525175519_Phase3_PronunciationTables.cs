using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Phase3_PronunciationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PronunciationAssessmentResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StreamingSessionId = table.Column<int>(type: "integer", nullable: false),
                    AyahId = table.Column<int>(type: "integer", nullable: false),
                    RecognizedText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    AccuracyScore = table.Column<double>(type: "double precision", nullable: false),
                    FluencyScore = table.Column<double>(type: "double precision", nullable: false),
                    CompletenessScore = table.Column<double>(type: "double precision", nullable: false),
                    PronunciationScore = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PronunciationAssessmentResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PronunciationAssessmentResults_Ayahs_AyahId",
                        column: x => x.AyahId,
                        principalTable: "Ayahs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PronunciationAssessmentResults_StreamingSessions_StreamingS~",
                        column: x => x.StreamingSessionId,
                        principalTable: "StreamingSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordPronunciationResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PronunciationAssessmentResultId = table.Column<int>(type: "integer", nullable: false),
                    Word = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AccuracyScore = table.Column<double>(type: "double precision", nullable: false),
                    ErrorType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    Offset = table.Column<long>(type: "bigint", nullable: false),
                    Duration = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordPronunciationResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordPronunciationResults_PronunciationAssessmentResults_Pro~",
                        column: x => x.PronunciationAssessmentResultId,
                        principalTable: "PronunciationAssessmentResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhonemeResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WordPronunciationResultId = table.Column<int>(type: "integer", nullable: false),
                    Phoneme = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AccuracyScore = table.Column<double>(type: "double precision", nullable: false),
                    Duration = table.Column<long>(type: "bigint", nullable: false),
                    IsWeak = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhonemeResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhonemeResults_WordPronunciationResults_WordPronunciationRe~",
                        column: x => x.WordPronunciationResultId,
                        principalTable: "WordPronunciationResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhonemeResults_WordPronunciationResultId",
                table: "PhonemeResults",
                column: "WordPronunciationResultId");

            migrationBuilder.CreateIndex(
                name: "IX_PronunciationAssessmentResults_AyahId",
                table: "PronunciationAssessmentResults",
                column: "AyahId");

            migrationBuilder.CreateIndex(
                name: "IX_PronunciationAssessmentResults_StreamingSessionId",
                table: "PronunciationAssessmentResults",
                column: "StreamingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_WordPronunciationResults_PronunciationAssessmentResultId",
                table: "WordPronunciationResults",
                column: "PronunciationAssessmentResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhonemeResults");

            migrationBuilder.DropTable(
                name: "WordPronunciationResults");

            migrationBuilder.DropTable(
                name: "PronunciationAssessmentResults");
        }
    }
}
