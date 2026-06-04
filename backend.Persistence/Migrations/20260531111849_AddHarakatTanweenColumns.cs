using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHarakatTanweenColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DammaExampleAr",
                table: "ArabicLetters",
                type: "character varying(100)",
                maxLength: 100,
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
                name: "KasraExampleAr",
                table: "ArabicLetters",
                type: "character varying(100)",
                maxLength: 100,
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
                name: "TanweenFathAr",
                table: "ArabicLetters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TanweenKasrAr",
                table: "ArabicLetters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "أُمَّة", "أَحَد", "إِيمَان", "", "الْحَمْد", "إِمَامٌ", "إِمَامًا", "إِمَامٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "بُرُوج", "بَيْت", "بِسْمِ اللَّه", "رَبَّنَا", "عَبْد", "كِتَابٌ", "كِتَابًا", "كِتَابٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "تُوبُوا", "تَوْبَة", "تِلْكَ", "اتَّقُوا", "أَنْتَ", "رَحْمَةٌ", "رَحْمَةً", "رَحْمَةٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "ثُمَّ", "ثَوْب", "ثِقَال", "", "حَيْثُ", "حَدِيثٌ", "حَدِيثًا", "حَدِيثٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "جُزْء", "جَنَّة", "جِهَاد", "", "أَجْر", "أَجْرٌ", "أَجْرًا", "أَجْرٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "حُكْم", "حَمْد", "حِكْمَة", "", "رَحْمَة", "فَرَحٌ", "فَرَحًا", "فَرَحٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "خُلُق", "خَلَق", "خِلَاف", "", "أَخْبَار", "خَيْرٌ", "خَيْرًا", "خَيْرٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "دُعَاء", "دَعَا", "دِين", "", "هُدْى", "مَسْجِدٌ", "مَسْجِدًا", "مَسْجِدٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "ذُنُوب", "ذَلِكَ", "ذِكْر", "", "إِذْ", "عَذَابٌ", "عَذَابًا", "عَذَابٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "رُسُل", "رَحْمَة", "رِزْق", "رَبَّنَا", "أَرْض", "نُورٌ", "نُورًا", "نُورٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "زُهُور", "زَيْت", "زِينَة", "", "أَزْوَاج", "رِزْقٌ", "رِزْقًا", "رِزْقٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "سُورَة", "سَلَام", "سِرَاج", "", "مَسْجِد", "سَلَامٌ", "سَلَامًا", "سَلَامٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "شُكْر", "شَمْس", "شِفَاء", "", "عَشْر", "عَرْشٌ", "عَرْشًا", "عَرْشٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "صُدُور", "صَلَاة", "صِرَاط", "", "أَصْحَاب", "صَبْرٌ", "صَبْرًا", "صَبْرٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "ضُعَفَاء", "ضَلَال", "ضِيَاء", "", "أَرْض", "فَضْلٌ", "فَضْلًا", "فَضْلٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "طُهُور", "طَرِيق", "طِفْل", "", "قِطْعَة", "خَطٌّ", "خَطًّا", "خَطٍّ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "ظُلْمَة", "ظَلَمَ", "ظِلّ", "", "حِفْظ", "ظُلْمٌ", "ظُلْمًا", "ظُلْمٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "عُلَمَاء", "عَمَل", "عِلْم", "", "يَعْلَم", "عِلْمٌ", "عِلْمًا", "عِلْمٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "غُرُور", "غَفَر", "غِطَاء", "", "يَغْفِر", "غَيْبٌ", "غَيْبًا", "غَيْبٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "فُؤَاد", "فَجْر", "فِطْرَة", "", "أَفْلَح", "فَجْرٌ", "فَجْرًا", "فَجْرٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "قُرْآن", "قَلْب", "قِيَامَة", "", "حَقّ", "حَقٌّ", "حَقًّا", "حَقٍّ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "كُفْر", "كَرِيم", "كِتَاب", "", "يَكْتُب", "مَلِكٌ", "مَلِكًا", "مَلِكٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "لُغَة", "لَيْل", "لِسَان", "اللَّه", "عِلْم", "لَيْلٌ", "لَيْلًا", "لَيْلٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "مُؤْمِن", "مَاء", "مِيزَان", "إِنَّمَا", "يَعْلَمُون", "عَالِمٌ", "عِلْمًا", "عِلْمٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "نُور", "نَعِيم", "نِعْمَة", "إِنَّ", "إِنْ", "نُورٌ", "نُورًا", "نُورٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "وُجُوه", "وَلَد", "وِلَادَة", "", "يَوْم", "يَوْمٌ", "يَوْمًا", "يَوْمٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "هُدَى", "هَدَى", "هِدَايَة", "", "يَهْدِي", "هَدْيٌ", "هَدْيًا", "هَدْيٍ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "DammaExampleAr", "FathaExampleAr", "KasraExampleAr", "ShaddaExampleAr", "SukunExampleAr", "TanweenDammAr", "TanweenFathAr", "TanweenKasrAr" },
                values: new object[] { "يُوسُف", "يَوْم", "يَدِيهِ", "", "خَيْر", "شَيْءٌ", "شَيْئًا", "شَيْءٍ" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DammaExampleAr",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "FathaExampleAr",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "KasraExampleAr",
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
                name: "TanweenFathAr",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "TanweenKasrAr",
                table: "ArabicLetters");
        }
    }
}
