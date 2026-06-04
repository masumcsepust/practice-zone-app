using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDiacriticSyllableLessonPractice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiacriticSigns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Symbol = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    NameEn = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NameBn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SignGroup = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiacriticSigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TitleEn = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    TitleBn = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SequenceOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SyllableSounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LetterId = table.Column<Guid>(type: "uuid", nullable: false),
                    SignId = table.Column<Guid>(type: "uuid", nullable: false),
                    CombinedCharacter = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TransliterationEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TransliterationBn = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AudioUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyllableSounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SyllableSounds_ArabicLetters_LetterId",
                        column: x => x.LetterId,
                        principalTable: "ArabicLetters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SyllableSounds_DiacriticSigns_SignId",
                        column: x => x.SignId,
                        principalTable: "DiacriticSigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PracticeItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetSyllableId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompareWithSyllableId = table.Column<Guid>(type: "uuid", nullable: true),
                    InstructionEn = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    InstructionBn = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SuccessTipEn = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    SuccessTipBn = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PracticeItems_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PracticeItems_SyllableSounds_CompareWithSyllableId",
                        column: x => x.CompareWithSyllableId,
                        principalTable: "SyllableSounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PracticeItems_SyllableSounds_TargetSyllableId",
                        column: x => x.TargetSyllableId,
                        principalTable: "SyllableSounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiacriticSigns_Symbol",
                table: "DiacriticSigns",
                column: "Symbol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_SequenceOrder",
                table: "Lessons",
                column: "SequenceOrder",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PracticeItems_CompareWithSyllableId",
                table: "PracticeItems",
                column: "CompareWithSyllableId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticeItems_LessonId",
                table: "PracticeItems",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticeItems_TargetSyllableId",
                table: "PracticeItems",
                column: "TargetSyllableId");

            migrationBuilder.CreateIndex(
                name: "IX_SyllableSounds_LetterId_SignId",
                table: "SyllableSounds",
                columns: new[] { "LetterId", "SignId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SyllableSounds_SignId",
                table: "SyllableSounds",
                column: "SignId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PracticeItems");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "SyllableSounds");

            migrationBuilder.DropTable(
                name: "DiacriticSigns");
        }
    }
}
