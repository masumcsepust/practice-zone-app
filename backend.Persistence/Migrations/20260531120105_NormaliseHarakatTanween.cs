using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NormaliseHarakatTanween : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DammaExampleAr",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "DammaSound",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "FathaExampleAr",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "FathaSound",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "KasraExampleAr",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "KasraSound",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "ShaddaExampleAr",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "SukunExampleAr",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "TanweenDammAr",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "TanweenDammSound",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "TanweenFathAr",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "TanweenFathSound",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "TanweenKasrAr",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "TanweenKasrSound",
                table: "ArabicLetters");

            migrationBuilder.CreateTable(
                name: "HarakatCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ArabicName = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    BanglaName = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Symbol = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DescriptionBn = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HarakatCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LetterHarakatItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LetterId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Sound = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ExampleWord = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterHarakatItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LetterHarakatItems_ArabicLetters_LetterId",
                        column: x => x.LetterId,
                        principalTable: "ArabicLetters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LetterHarakatItems_HarakatCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "HarakatCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "HarakatCategories",
                columns: new[] { "Id", "ArabicName", "BanglaName", "Description", "DescriptionBn", "DisplayOrder", "Name", "Symbol", "Type" },
                values: new object[,]
                {
                    { 1, "فَتْحَة", "ফাতহা", "Short vowel 'a' — letter opens upward", "ছোট 'আ' স্বর — বর্ণের উপরে ছোট তির্যক রেখা", 1, "Fatha", "َ", "Harakat" },
                    { 2, "كَسْرَة", "কাসরা", "Short vowel 'i' — letter bends downward", "ছোট 'ই' স্বর — বর্ণের নিচে ছোট তির্যক রেখা", 2, "Kasra", "ِ", "Harakat" },
                    { 3, "ضَمَّة", "দাম্মা", "Short vowel 'u' — small waw curl above letter", "ছোট 'উ' স্বর — বর্ণের উপরে ছোট ওয়াও চিহ্ন", 3, "Damma", "ُ", "Harakat" },
                    { 4, "سُكُون", "সুকুন", "No vowel — consonant at rest, small circle above", "কোনো স্বর নেই — বর্ণ বিশ্রামে, উপরে ছোট বৃত্ত", 4, "Sukun", "ْ", "Harakat" },
                    { 5, "شَدَّة", "শাদ্দা", "Gemination — consonant is doubled in pronunciation", "জোড়া বর্ণ — একই বর্ণ দুইবার উচ্চারণ করতে হয়", 5, "Shadda", "ّ", "Harakat" },
                    { 6, "تَنْوِين الفَتْح", "তানউইন ফাতহ (ফাতহাতান)", "Double fatha — '-an' ending (accusative indefinite noun)", "দ্বৈত ফাতহা — '-আন' সমাপ্তি (নাকিরা মানসুব)", 6, "TanweenFath", "ً", "Tanween" },
                    { 7, "تَنْوِين الكَسْر", "তানউইন কাসর (কাসরাতান)", "Double kasra — '-in' ending (genitive indefinite noun)", "দ্বৈত কাসরা — '-ইন' সমাপ্তি (নাকিরা মাজরুর)", 7, "TanweenKasr", "ٍ", "Tanween" },
                    { 8, "تَنْوِين الضَّمّ", "তানউইন দাম্ম (দাম্মাতান)", "Double damma — '-un' ending (nominative indefinite noun)", "দ্বৈত দাম্মা — '-উন' সমাপ্তি (নাকিরা মারফু)", 8, "TanweenDamm", "ٌ", "Tanween" }
                });

            migrationBuilder.InsertData(
                table: "LetterHarakatItems",
                columns: new[] { "Id", "CategoryId", "ExampleWord", "LetterId", "Sound" },
                values: new object[,]
                {
                    { 1, 1, "أَحَد", 1, "Aa" },
                    { 2, 2, "إِيمَان", 1, "Ii" },
                    { 3, 3, "أُمَّة", 1, "Uu" },
                    { 4, 4, "الْحَمْد", 1, "" },
                    { 5, 5, "", 1, "" },
                    { 6, 6, "إِمَامًا", 1, "Aan" },
                    { 7, 7, "إِمَامٍ", 1, "Iin" },
                    { 8, 8, "إِمَامٌ", 1, "Uun" },
                    { 9, 1, "بَيْت", 2, "Ba" },
                    { 10, 2, "بِسْمِ اللَّه", 2, "Bi" },
                    { 11, 3, "بُرُوج", 2, "Bu" },
                    { 12, 4, "عَبْد", 2, "" },
                    { 13, 5, "رَبَّنَا", 2, "" },
                    { 14, 6, "كِتَابًا", 2, "Ban" },
                    { 15, 7, "كِتَابٍ", 2, "Bin" },
                    { 16, 8, "كِتَابٌ", 2, "Bun" },
                    { 17, 1, "تَوْبَة", 3, "Ta" },
                    { 18, 2, "تِلْكَ", 3, "Ti" },
                    { 19, 3, "تُوبُوا", 3, "Tu" },
                    { 20, 4, "أَنْتَ", 3, "" },
                    { 21, 5, "اتَّقُوا", 3, "" },
                    { 22, 6, "رَحْمَةً", 3, "Tan" },
                    { 23, 7, "رَحْمَةٍ", 3, "Tin" },
                    { 24, 8, "رَحْمَةٌ", 3, "Tun" },
                    { 25, 1, "ثَوْب", 4, "Tha" },
                    { 26, 2, "ثِقَال", 4, "Thi" },
                    { 27, 3, "ثُمَّ", 4, "Thu" },
                    { 28, 4, "حَيْثُ", 4, "" },
                    { 29, 5, "", 4, "" },
                    { 30, 6, "حَدِيثًا", 4, "Than" },
                    { 31, 7, "حَدِيثٍ", 4, "Thin" },
                    { 32, 8, "حَدِيثٌ", 4, "Thun" },
                    { 33, 1, "جَنَّة", 5, "Ja" },
                    { 34, 2, "جِهَاد", 5, "Ji" },
                    { 35, 3, "جُزْء", 5, "Ju" },
                    { 36, 4, "أَجْر", 5, "" },
                    { 37, 5, "", 5, "" },
                    { 38, 6, "أَجْرًا", 5, "Jan" },
                    { 39, 7, "أَجْرٍ", 5, "Jin" },
                    { 40, 8, "أَجْرٌ", 5, "Jun" },
                    { 41, 1, "حَمْد", 6, "Ha" },
                    { 42, 2, "حِكْمَة", 6, "Hi" },
                    { 43, 3, "حُكْم", 6, "Hu" },
                    { 44, 4, "رَحْمَة", 6, "" },
                    { 45, 5, "", 6, "" },
                    { 46, 6, "فَرَحًا", 6, "Han" },
                    { 47, 7, "فَرَحٍ", 6, "Hin" },
                    { 48, 8, "فَرَحٌ", 6, "Hun" },
                    { 49, 1, "خَلَق", 7, "Kha" },
                    { 50, 2, "خِلَاف", 7, "Khi" },
                    { 51, 3, "خُلُق", 7, "Khu" },
                    { 52, 4, "أَخْبَار", 7, "" },
                    { 53, 5, "", 7, "" },
                    { 54, 6, "خَيْرًا", 7, "Khan" },
                    { 55, 7, "خَيْرٍ", 7, "Khin" },
                    { 56, 8, "خَيْرٌ", 7, "Khun" },
                    { 57, 1, "دَعَا", 8, "Da" },
                    { 58, 2, "دِين", 8, "Di" },
                    { 59, 3, "دُعَاء", 8, "Du" },
                    { 60, 4, "هُدْى", 8, "" },
                    { 61, 5, "", 8, "" },
                    { 62, 6, "مَسْجِدًا", 8, "Dan" },
                    { 63, 7, "مَسْجِدٍ", 8, "Din" },
                    { 64, 8, "مَسْجِدٌ", 8, "Dun" },
                    { 65, 1, "ذَلِكَ", 9, "Dha" },
                    { 66, 2, "ذِكْر", 9, "Dhi" },
                    { 67, 3, "ذُنُوب", 9, "Dhu" },
                    { 68, 4, "إِذْ", 9, "" },
                    { 69, 5, "", 9, "" },
                    { 70, 6, "عَذَابًا", 9, "Dhan" },
                    { 71, 7, "عَذَابٍ", 9, "Dhin" },
                    { 72, 8, "عَذَابٌ", 9, "Dhun" },
                    { 73, 1, "رَحْمَة", 10, "Ra" },
                    { 74, 2, "رِزْق", 10, "Ri" },
                    { 75, 3, "رُسُل", 10, "Ru" },
                    { 76, 4, "أَرْض", 10, "" },
                    { 77, 5, "رَبَّنَا", 10, "" },
                    { 78, 6, "نُورًا", 10, "Ran" },
                    { 79, 7, "نُورٍ", 10, "Rin" },
                    { 80, 8, "نُورٌ", 10, "Run" },
                    { 81, 1, "زَيْت", 11, "Za" },
                    { 82, 2, "زِينَة", 11, "Zi" },
                    { 83, 3, "زُهُور", 11, "Zu" },
                    { 84, 4, "أَزْوَاج", 11, "" },
                    { 85, 5, "", 11, "" },
                    { 86, 6, "رِزْقًا", 11, "Zan" },
                    { 87, 7, "رِزْقٍ", 11, "Zin" },
                    { 88, 8, "رِزْقٌ", 11, "Zun" },
                    { 89, 1, "سَلَام", 12, "Sa" },
                    { 90, 2, "سِرَاج", 12, "Si" },
                    { 91, 3, "سُورَة", 12, "Su" },
                    { 92, 4, "مَسْجِد", 12, "" },
                    { 93, 5, "", 12, "" },
                    { 94, 6, "سَلَامًا", 12, "San" },
                    { 95, 7, "سَلَامٍ", 12, "Sin" },
                    { 96, 8, "سَلَامٌ", 12, "Sun" },
                    { 97, 1, "شَمْس", 13, "Sha" },
                    { 98, 2, "شِفَاء", 13, "Shi" },
                    { 99, 3, "شُكْر", 13, "Shu" },
                    { 100, 4, "عَشْر", 13, "" },
                    { 101, 5, "", 13, "" },
                    { 102, 6, "عَرْشًا", 13, "Shan" },
                    { 103, 7, "عَرْشٍ", 13, "Shin" },
                    { 104, 8, "عَرْشٌ", 13, "Shun" },
                    { 105, 1, "صَلَاة", 14, "Sa" },
                    { 106, 2, "صِرَاط", 14, "Si" },
                    { 107, 3, "صُدُور", 14, "Su" },
                    { 108, 4, "أَصْحَاب", 14, "" },
                    { 109, 5, "", 14, "" },
                    { 110, 6, "صَبْرًا", 14, "San" },
                    { 111, 7, "صَبْرٍ", 14, "Sin" },
                    { 112, 8, "صَبْرٌ", 14, "Sun" },
                    { 113, 1, "ضَلَال", 15, "Da" },
                    { 114, 2, "ضِيَاء", 15, "Di" },
                    { 115, 3, "ضُعَفَاء", 15, "Du" },
                    { 116, 4, "أَرْض", 15, "" },
                    { 117, 5, "", 15, "" },
                    { 118, 6, "فَضْلًا", 15, "Dan" },
                    { 119, 7, "فَضْلٍ", 15, "Din" },
                    { 120, 8, "فَضْلٌ", 15, "Dun" },
                    { 121, 1, "طَرِيق", 16, "Ta" },
                    { 122, 2, "طِفْل", 16, "Ti" },
                    { 123, 3, "طُهُور", 16, "Tu" },
                    { 124, 4, "قِطْعَة", 16, "" },
                    { 125, 5, "", 16, "" },
                    { 126, 6, "خَطًّا", 16, "Tan" },
                    { 127, 7, "خَطٍّ", 16, "Tin" },
                    { 128, 8, "خَطٌّ", 16, "Tun" },
                    { 129, 1, "ظَلَمَ", 17, "Dha" },
                    { 130, 2, "ظِلّ", 17, "Dhi" },
                    { 131, 3, "ظُلْمَة", 17, "Dhu" },
                    { 132, 4, "حِفْظ", 17, "" },
                    { 133, 5, "", 17, "" },
                    { 134, 6, "ظُلْمًا", 17, "Dhan" },
                    { 135, 7, "ظُلْمٍ", 17, "Dhin" },
                    { 136, 8, "ظُلْمٌ", 17, "Dhun" },
                    { 137, 1, "عَمَل", 18, "'Aa" },
                    { 138, 2, "عِلْم", 18, "'Ii" },
                    { 139, 3, "عُلَمَاء", 18, "'Uu" },
                    { 140, 4, "يَعْلَم", 18, "" },
                    { 141, 5, "", 18, "" },
                    { 142, 6, "عِلْمًا", 18, "'Aan" },
                    { 143, 7, "عِلْمٍ", 18, "'Iin" },
                    { 144, 8, "عِلْمٌ", 18, "'Uun" },
                    { 145, 1, "غَفَر", 19, "Gha" },
                    { 146, 2, "غِطَاء", 19, "Ghi" },
                    { 147, 3, "غُرُور", 19, "Ghu" },
                    { 148, 4, "يَغْفِر", 19, "" },
                    { 149, 5, "", 19, "" },
                    { 150, 6, "غَيْبًا", 19, "Ghan" },
                    { 151, 7, "غَيْبٍ", 19, "Ghin" },
                    { 152, 8, "غَيْبٌ", 19, "Ghun" },
                    { 153, 1, "فَجْر", 20, "Fa" },
                    { 154, 2, "فِطْرَة", 20, "Fi" },
                    { 155, 3, "فُؤَاد", 20, "Fu" },
                    { 156, 4, "أَفْلَح", 20, "" },
                    { 157, 5, "", 20, "" },
                    { 158, 6, "فَجْرًا", 20, "Fan" },
                    { 159, 7, "فَجْرٍ", 20, "Fin" },
                    { 160, 8, "فَجْرٌ", 20, "Fun" },
                    { 161, 1, "قَلْب", 21, "Qa" },
                    { 162, 2, "قِيَامَة", 21, "Qi" },
                    { 163, 3, "قُرْآن", 21, "Qu" },
                    { 164, 4, "حَقّ", 21, "" },
                    { 165, 5, "", 21, "" },
                    { 166, 6, "حَقًّا", 21, "Qan" },
                    { 167, 7, "حَقٍّ", 21, "Qin" },
                    { 168, 8, "حَقٌّ", 21, "Qun" },
                    { 169, 1, "كَرِيم", 22, "Ka" },
                    { 170, 2, "كِتَاب", 22, "Ki" },
                    { 171, 3, "كُفْر", 22, "Ku" },
                    { 172, 4, "يَكْتُب", 22, "" },
                    { 173, 5, "", 22, "" },
                    { 174, 6, "مَلِكًا", 22, "Kan" },
                    { 175, 7, "مَلِكٍ", 22, "Kin" },
                    { 176, 8, "مَلِكٌ", 22, "Kun" },
                    { 177, 1, "لَيْل", 23, "La" },
                    { 178, 2, "لِسَان", 23, "Li" },
                    { 179, 3, "لُغَة", 23, "Lu" },
                    { 180, 4, "عِلْم", 23, "" },
                    { 181, 5, "اللَّه", 23, "" },
                    { 182, 6, "لَيْلًا", 23, "Lan" },
                    { 183, 7, "لَيْلٍ", 23, "Lin" },
                    { 184, 8, "لَيْلٌ", 23, "Lun" },
                    { 185, 1, "مَاء", 24, "Ma" },
                    { 186, 2, "مِيزَان", 24, "Mi" },
                    { 187, 3, "مُؤْمِن", 24, "Mu" },
                    { 188, 4, "يَعْلَمُون", 24, "" },
                    { 189, 5, "إِنَّمَا", 24, "" },
                    { 190, 6, "عِلْمًا", 24, "Man" },
                    { 191, 7, "عِلْمٍ", 24, "Min" },
                    { 192, 8, "عَالِمٌ", 24, "Mun" },
                    { 193, 1, "نَعِيم", 25, "Na" },
                    { 194, 2, "نِعْمَة", 25, "Ni" },
                    { 195, 3, "نُور", 25, "Nu" },
                    { 196, 4, "إِنْ", 25, "" },
                    { 197, 5, "إِنَّ", 25, "" },
                    { 198, 6, "نُورًا", 25, "Nan" },
                    { 199, 7, "نُورٍ", 25, "Nin" },
                    { 200, 8, "نُورٌ", 25, "Nun" },
                    { 201, 1, "وَلَد", 26, "Wa" },
                    { 202, 2, "وِلَادَة", 26, "Wi" },
                    { 203, 3, "وُجُوه", 26, "Wu" },
                    { 204, 4, "يَوْم", 26, "" },
                    { 205, 5, "", 26, "" },
                    { 206, 6, "يَوْمًا", 26, "Wan" },
                    { 207, 7, "يَوْمٍ", 26, "Win" },
                    { 208, 8, "يَوْمٌ", 26, "Wun" },
                    { 209, 1, "هَدَى", 27, "Ha" },
                    { 210, 2, "هِدَايَة", 27, "Hi" },
                    { 211, 3, "هُدَى", 27, "Hu" },
                    { 212, 4, "يَهْدِي", 27, "" },
                    { 213, 5, "", 27, "" },
                    { 214, 6, "هَدْيًا", 27, "Han" },
                    { 215, 7, "هَدْيٍ", 27, "Hin" },
                    { 216, 8, "هَدْيٌ", 27, "Hun" },
                    { 217, 1, "يَوْم", 28, "Ya" },
                    { 218, 2, "يَدِيهِ", 28, "Yi" },
                    { 219, 3, "يُوسُف", 28, "Yu" },
                    { 220, 4, "خَيْر", 28, "" },
                    { 221, 5, "", 28, "" },
                    { 222, 6, "شَيْئًا", 28, "Yan" },
                    { 223, 7, "شَيْءٍ", 28, "Yin" },
                    { 224, 8, "شَيْءٌ", 28, "Yun" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HarakatCategories_DisplayOrder",
                table: "HarakatCategories",
                column: "DisplayOrder",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LetterHarakatItems_CategoryId",
                table: "LetterHarakatItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterHarakatItems_LetterId_CategoryId",
                table: "LetterHarakatItems",
                columns: new[] { "LetterId", "CategoryId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LetterHarakatItems");

            migrationBuilder.DropTable(
                name: "HarakatCategories");

            migrationBuilder.AddColumn<string>(
                name: "DammaExampleAr",
                table: "ArabicLetters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DammaSound",
                table: "ArabicLetters",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FathaExampleAr",
                table: "ArabicLetters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FathaSound",
                table: "ArabicLetters",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KasraExampleAr",
                table: "ArabicLetters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KasraSound",
                table: "ArabicLetters",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShaddaExampleAr",
                table: "ArabicLetters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SukunExampleAr",
                table: "ArabicLetters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TanweenDammAr",
                table: "ArabicLetters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TanweenDammSound",
                table: "ArabicLetters",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TanweenFathAr",
                table: "ArabicLetters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TanweenFathSound",
                table: "ArabicLetters",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TanweenKasrAr",
                table: "ArabicLetters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TanweenKasrSound",
                table: "ArabicLetters",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "أُمَّة", "Uu", "أَحَد", "Aa", "إِيمَان", "Ii", "", "الْحَمْد", "إِمَامٌ", "Uun", "إِمَامًا", "Aan", "إِمَامٍ", "Iin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "بُرُوج", "Bu", "بَيْت", "Ba", "بِسْمِ اللَّه", "Bi", "رَبَّنَا", "عَبْد", "كِتَابٌ", "Bun", "كِتَابًا", "Ban", "كِتَابٍ", "Bin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "تُوبُوا", "Tu", "تَوْبَة", "Ta", "تِلْكَ", "Ti", "اتَّقُوا", "أَنْتَ", "رَحْمَةٌ", "Tun", "رَحْمَةً", "Tan", "رَحْمَةٍ", "Tin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "ثُمَّ", "Thu", "ثَوْب", "Tha", "ثِقَال", "Thi", "", "حَيْثُ", "حَدِيثٌ", "Thun", "حَدِيثًا", "Than", "حَدِيثٍ", "Thin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "جُزْء", "Ju", "جَنَّة", "Ja", "جِهَاد", "Ji", "", "أَجْر", "أَجْرٌ", "Jun", "أَجْرًا", "Jan", "أَجْرٍ", "Jin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "حُكْم", "Hu", "حَمْد", "Ha", "حِكْمَة", "Hi", "", "رَحْمَة", "فَرَحٌ", "Hun", "فَرَحًا", "Han", "فَرَحٍ", "Hin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "خُلُق", "Khu", "خَلَق", "Kha", "خِلَاف", "Khi", "", "أَخْبَار", "خَيْرٌ", "Khun", "خَيْرًا", "Khan", "خَيْرٍ", "Khin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "دُعَاء", "Du", "دَعَا", "Da", "دِين", "Di", "", "هُدْى", "مَسْجِدٌ", "Dun", "مَسْجِدًا", "Dan", "مَسْجِدٍ", "Din" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "ذُنُوب", "Dhu", "ذَلِكَ", "Dha", "ذِكْر", "Dhi", "", "إِذْ", "عَذَابٌ", "Dhun", "عَذَابًا", "Dhan", "عَذَابٍ", "Dhin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "رُسُل", "Ru", "رَحْمَة", "Ra", "رِزْق", "Ri", "رَبَّنَا", "أَرْض", "نُورٌ", "Run", "نُورًا", "Ran", "نُورٍ", "Rin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "زُهُور", "Zu", "زَيْت", "Za", "زِينَة", "Zi", "", "أَزْوَاج", "رِزْقٌ", "Zun", "رِزْقًا", "Zan", "رِزْقٍ", "Zin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "سُورَة", "Su", "سَلَام", "Sa", "سِرَاج", "Si", "", "مَسْجِد", "سَلَامٌ", "Sun", "سَلَامًا", "San", "سَلَامٍ", "Sin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "شُكْر", "Shu", "شَمْس", "Sha", "شِفَاء", "Shi", "", "عَشْر", "عَرْشٌ", "Shun", "عَرْشًا", "Shan", "عَرْشٍ", "Shin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "صُدُور", "Su", "صَلَاة", "Sa", "صِرَاط", "Si", "", "أَصْحَاب", "صَبْرٌ", "Sun", "صَبْرًا", "San", "صَبْرٍ", "Sin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "ضُعَفَاء", "Du", "ضَلَال", "Da", "ضِيَاء", "Di", "", "أَرْض", "فَضْلٌ", "Dun", "فَضْلًا", "Dan", "فَضْلٍ", "Din" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "طُهُور", "Tu", "طَرِيق", "Ta", "طِفْل", "Ti", "", "قِطْعَة", "خَطٌّ", "Tun", "خَطًّا", "Tan", "خَطٍّ", "Tin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "ظُلْمَة", "Dhu", "ظَلَمَ", "Dha", "ظِلّ", "Dhi", "", "حِفْظ", "ظُلْمٌ", "Dhun", "ظُلْمًا", "Dhan", "ظُلْمٍ", "Dhin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "عُلَمَاء", "'Uu", "عَمَل", "'Aa", "عِلْم", "'Ii", "", "يَعْلَم", "عِلْمٌ", "'Uun", "عِلْمًا", "'Aan", "عِلْمٍ", "'Iin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "غُرُور", "Ghu", "غَفَر", "Gha", "غِطَاء", "Ghi", "", "يَغْفِر", "غَيْبٌ", "Ghun", "غَيْبًا", "Ghan", "غَيْبٍ", "Ghin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "فُؤَاد", "Fu", "فَجْر", "Fa", "فِطْرَة", "Fi", "", "أَفْلَح", "فَجْرٌ", "Fun", "فَجْرًا", "Fan", "فَجْرٍ", "Fin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "قُرْآن", "Qu", "قَلْب", "Qa", "قِيَامَة", "Qi", "", "حَقّ", "حَقٌّ", "Qun", "حَقًّا", "Qan", "حَقٍّ", "Qin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "كُفْر", "Ku", "كَرِيم", "Ka", "كِتَاب", "Ki", "", "يَكْتُب", "مَلِكٌ", "Kun", "مَلِكًا", "Kan", "مَلِكٍ", "Kin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "لُغَة", "Lu", "لَيْل", "La", "لِسَان", "Li", "اللَّه", "عِلْم", "لَيْلٌ", "Lun", "لَيْلًا", "Lan", "لَيْلٍ", "Lin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "مُؤْمِن", "Mu", "مَاء", "Ma", "مِيزَان", "Mi", "إِنَّمَا", "يَعْلَمُون", "عَالِمٌ", "Mun", "عِلْمًا", "Man", "عِلْمٍ", "Min" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "نُور", "Nu", "نَعِيم", "Na", "نِعْمَة", "Ni", "إِنَّ", "إِنْ", "نُورٌ", "Nun", "نُورًا", "Nan", "نُورٍ", "Nin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "وُجُوه", "Wu", "وَلَد", "Wa", "وِلَادَة", "Wi", "", "يَوْم", "يَوْمٌ", "Wun", "يَوْمًا", "Wan", "يَوْمٍ", "Win" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "هُدَى", "Hu", "هَدَى", "Ha", "هِدَايَة", "Hi", "", "يَهْدِي", "هَدْيٌ", "Hun", "هَدْيًا", "Han", "هَدْيٍ", "Hin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "DammaExampleAr", "DammaSound", "FathaExampleAr", "FathaSound", "KasraExampleAr", "KasraSound", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenDammSound", "TanweenFathAr", "TanweenFathSound", "TanweenKasrAr", "TanweenKasrSound" },
                values: new object[] { "يُوسُف", "Yu", "يَوْم", "Ya", "يَدِيهِ", "Yi", "", "خَيْر", "شَيْءٌ", "Yun", "شَيْئًا", "Yan", "شَيْءٍ", "Yin" });
        }
    }
}
