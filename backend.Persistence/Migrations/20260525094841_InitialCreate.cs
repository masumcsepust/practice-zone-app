using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Surahs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SurahNumber = table.Column<int>(type: "integer", nullable: false),
                    NameArabic = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NameEnglish = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NameBangla = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TotalAyahs = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surahs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ayahs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SurahId = table.Column<int>(type: "integer", nullable: false),
                    AyahNumber = table.Column<int>(type: "integer", nullable: false),
                    ArabicText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    NormalizedArabicText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    EnglishTranslation = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    BanglaTranslation = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    Transliteration = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ayahs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ayahs_Surahs_SurahId",
                        column: x => x.SurahId,
                        principalTable: "Surahs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecitationSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    AyahId = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RecognizedText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    NormalizedRecognizedText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    OverallScore = table.Column<int>(type: "integer", nullable: false),
                    AudioFilePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecitationSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecitationSessions_Ayahs_AyahId",
                        column: x => x.AyahId,
                        principalTable: "Ayahs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Surahs",
                columns: new[] { "Id", "NameArabic", "NameBangla", "NameEnglish", "SurahNumber", "TotalAyahs" },
                values: new object[] { 1, "الفاتحة", "সূচনা", "The Opening", 1, 7 });

            migrationBuilder.InsertData(
                table: "Ayahs",
                columns: new[] { "Id", "ArabicText", "AyahNumber", "BanglaTranslation", "EnglishTranslation", "NormalizedArabicText", "SurahId", "Transliteration" },
                values: new object[,]
                {
                    { 1, "بِسْمِ اللَّهِ الرَّحْمَٰنِ الرَّحِيمِ", 1, "পরম করুণাময় অতি দয়ালু আল্লাহর নামে।", "In the name of Allah, the Most Gracious, the Most Merciful.", "بسم الله الرحمن الرحيم", 1, "Bismillahi r-rahmani r-raheem" },
                    { 2, "الْحَمْدُ لِلَّهِ رَبِّ الْعَالَمِينَ", 2, "সমস্ত প্রশংসা আল্লাহর জন্য, যিনি সমগ্র জগতের প্রতিপালক।", "All praise is due to Allah, Lord of all the worlds.", "الحمد لله رب العالمين", 1, "Alhamdu lillahi rabbi l-'alamin" },
                    { 3, "الرَّحْمَٰنِ الرَّحِيمِ", 3, "পরম করুণাময়, অতি দয়ালু।", "The Most Gracious, the Most Merciful.", "الرحمن الرحيم", 1, "Ar-rahmani r-raheem" },
                    { 4, "مَالِكِ يَوْمِ الدِّينِ", 4, "বিচার দিনের মালিক।", "Master of the Day of Judgment.", "مالك يوم الدين", 1, "Maliki yawmi d-deen" },
                    { 5, "إِيَّاكَ نَعْبُدُ وَإِيَّاكَ نَسْتَعِينُ", 5, "আমরা কেবল তোমারই ইবাদত করি এবং কেবল তোমারই সাহায্য চাই।", "You alone we worship, and You alone we ask for help.", "اياك نعبد واياك نستعين", 1, "Iyyaka na'budu wa-iyyaka nasta'een" },
                    { 6, "اهْدِنَا الصِّرَاطَ الْمُسْتَقِيمَ", 6, "আমাদের সরল পথ দেখাও।", "Guide us to the straight path.", "اهدنا الصراط المستقيم", 1, "Ihdina s-sirata l-mustaqeem" },
                    { 7, "صِرَاطَ الَّذِينَ أَنْعَمْتَ عَلَيْهِمْ غَيْرِ الْمَغْضُوبِ عَلَيْهِمْ وَلَا الضَّالِّينَ", 7, "তাদের পথ, যাদের তুমি নেয়ামত দিয়েছ; তাদের পথ নয় যাদের উপর তোমার ক্রোধ আছে এবং যারা পথভ্রষ্ট।", "The path of those upon whom You have bestowed favor, not of those who have evoked Your anger or of those who are astray.", "صراط الذين انعمت عليهم غير المغضوب عليهم ولا الضالين", 1, "Sirata l-ladhina an'amta 'alayhim ghayri l-maghdubi 'alayhim wa-la d-dalleen" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ayahs_SurahId_AyahNumber",
                table: "Ayahs",
                columns: new[] { "SurahId", "AyahNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecitationSessions_AyahId",
                table: "RecitationSessions",
                column: "AyahId");

            migrationBuilder.CreateIndex(
                name: "IX_RecitationSessions_StartedAt",
                table: "RecitationSessions",
                column: "StartedAt");

            migrationBuilder.CreateIndex(
                name: "IX_RecitationSessions_UserId",
                table: "RecitationSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Surahs_SurahNumber",
                table: "Surahs",
                column: "SurahNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecitationSessions");

            migrationBuilder.DropTable(
                name: "Ayahs");

            migrationBuilder.DropTable(
                name: "Surahs");
        }
    }
}
