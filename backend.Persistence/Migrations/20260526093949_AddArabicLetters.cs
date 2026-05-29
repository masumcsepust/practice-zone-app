using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddArabicLetters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArabicLetters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Letter = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    NameEnglish = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NameArabic = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NameBangla = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Transliteration = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    MakhrajType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MakhrajDescription = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Sifaat = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ExampleWordArabic = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ExampleWord = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArabicLetters", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ArabicLetters",
                columns: new[] { "Id", "ExampleWord", "ExampleWordArabic", "Letter", "MakhrajDescription", "MakhrajType", "NameArabic", "NameBangla", "NameEnglish", "Order", "Sifaat", "Transliteration" },
                values: new object[,]
                {
                    { 1, "One (Ahad)", "أَحَد", "ا", "Deepest part of the throat (Hamza); Alif itself is a vowel carrier", "Throat", "أَلِف", "আলিফ", "Alif", 1, "Jahr,Rakhawa,Istifal,Infitah,Idhlag", "ā / ʾ" },
                    { 2, "House (Bayt)", "بَيْت", "ب", "Both lips pressed together (bilabial stop)", "Lips", "بَاء", "বা", "Ba", 2, "Jahr,Shiddah,Istifal,Infitah,Idhlag,Qalqalah", "b" },
                    { 3, "Dates (Tamr)", "تَمْر", "ت", "Tip of tongue touches the upper front teeth", "TongueTipDental", "تَاء", "তা", "Ta", 3, "Hams,Shiddah,Istifal,Infitah,Idhlag", "t" },
                    { 4, "Garment (Thawb)", "ثَوْب", "ث", "Tip of tongue lightly between the upper and lower teeth", "TongueTipInterdental", "ثَاء", "ছা", "Tha", 4, "Hams,Rakhawa,Istifal,Infitah,Idhlag", "th" },
                    { 5, "Mountain (Jabal)", "جَبَل", "ج", "Middle of the tongue meets the hard palate", "TongueFront", "جِيم", "জিম", "Jim", 5, "Jahr,Shiddah,Istifal,Infitah,Idhlag,Qalqalah", "j" },
                    { 6, "Truth (Haqq)", "حَق", "ح", "Middle of the throat — a breathy, voiceless pharyngeal fricative", "UpperThroat", "حَاء", "হা", "Ha", 6, "Hams,Rakhawa,Istifal,Infitah,Idhlag", "ḥ" },
                    { 7, "Goodness (Khayr)", "خَيْر", "خ", "Upper throat (closest to mouth) — velar fricative, like a raspy 'kh'", "UpperThroat", "خَاء", "খা", "Kha", 7, "Hams,Rakhawa,Isti'la,Infitah,Idhlag", "kh" },
                    { 8, "Religion (Deen)", "دِين", "د", "Tip and sides of tongue against the upper front teeth", "TongueTipDental", "دَال", "দাল", "Dal", 8, "Jahr,Shiddah,Istifal,Infitah,Idhlag,Qalqalah", "d" },
                    { 9, "Remembrance (Dhikr)", "ذِكْر", "ذ", "Tip of tongue lightly between the upper and lower teeth (voiced)", "TongueTipInterdental", "ذَال", "যাল", "Dhal", 9, "Jahr,Rakhawa,Istifal,Infitah,Idhlag", "dh" },
                    { 10, "Mercy (Rahma)", "رَحْمَة", "ر", "Tip of tongue near the upper gum ridge — a trilled or tapped 'r'", "TongueTipTrilled", "رَاء", "রা", "Ra", 10, "Jahr,Tawassut,Istifal,Infitah,Idhlag,Takrir", "r" },
                    { 11, "Oil (Zayt)", "زَيْت", "ز", "Tip of tongue near the lower front teeth — voiced sibilant", "TongueTipSibilant", "زَاي", "যায়", "Zay", 11, "Jahr,Rakhawa,Istifal,Infitah,Idhlag,Safeer", "z" },
                    { 12, "Peace (Salaam)", "سَلَام", "س", "Tip of tongue near the lower front teeth — voiceless sibilant", "TongueTipSibilant", "سِين", "সিন", "Sin", 12, "Hams,Rakhawa,Istifal,Infitah,Idhlag,Safeer", "s" },
                    { 13, "Sun (Shams)", "شَمْس", "ش", "Middle of the tongue spread toward the hard palate — 'sh' sound", "TongueFront", "شِين", "শিন", "Shin", 13, "Hams,Rakhawa,Istifal,Infitah,Idhlag,Tafasshi", "sh" },
                    { 14, "Patience (Sabr)", "صَبْر", "ص", "Tip of tongue near front teeth — heavy emphatic 'S' with tongue raised", "TongueTipSibilant", "صَاد", "সাদ", "Sad", 14, "Hams,Rakhawa,Isti'la,Itbaq,Idhlag,Safeer", "ṣ" },
                    { 15, "Light (Daw')", "ضَوْء", "ض", "One or both sides of the tongue against the upper back molars", "TongueSide", "ضَاد", "দোয়াদ", "Dad", 15, "Jahr,Rakhawa,Isti'la,Itbaq,Idhlag", "ḍ" },
                    { 16, "Road (Tariq)", "طَرِيق", "ط", "Tip of tongue touches upper front teeth — emphatic heavy 'T'", "TongueTipDental", "طَاء", "তোয়া", "Taa", 16, "Jahr,Shiddah,Isti'la,Itbaq,Idhlag,Qalqalah", "ṭ" },
                    { 17, "Oppression (Dhulm)", "ظُلْم", "ظ", "Tip of tongue between teeth — heavy emphatic interdental", "TongueTipInterdental", "ظَاء", "যোয়া", "Dhaa", 17, "Jahr,Rakhawa,Isti'la,Itbaq,Idhlag", "ẓ" },
                    { 18, "Knowledge (Ilm)", "عِلْم", "ع", "Middle of the throat — voiced pharyngeal fricative, a unique Arabic sound", "MidThroat", "عَيْن", "আইন", "Ayn", 18, "Jahr,Tawassut,Istifal,Infitah,Idhlag", "ʿ" },
                    { 19, "Unseen (Ghayb)", "غَيْب", "غ", "Upper throat — voiced velar fricative, a gargling 'gh' sound", "MidThroat", "غَيْن", "গাইন", "Ghayn", 19, "Jahr,Rakhawa,Isti'la,Infitah,Idhlag", "gh" },
                    { 20, "Dawn (Fajr)", "فَجْر", "ف", "Inner edge of the lower lip touches the tips of the upper front teeth", "Labiodental", "فَاء", "ফা", "Fa", 20, "Hams,Rakhawa,Istifal,Infitah,Idhlag", "f" },
                    { 21, "Quran (Quran)", "قُرْآن", "ق", "Back of tongue touches the soft palate (uvular stop)", "TongueBack", "قَاف", "কাফ", "Qaf", 21, "Jahr,Shiddah,Isti'la,Infitah,Ismat,Qalqalah", "q" },
                    { 22, "Book (Kitab)", "كِتَاب", "ك", "Back of tongue touches the hard palate — slightly forward of Qaf", "TongueBack", "كَاف", "কাফ", "Kaf", 22, "Hams,Shiddah,Istifal,Infitah,Ismat", "k" },
                    { 23, "Night (Layl)", "لَيْل", "ل", "Tip and sides of tongue along the upper gum ridge — lateral sound", "TongueSide", "لَام", "লাম", "Lam", 23, "Jahr,Tawassut,Istifal,Infitah,Idhlag,Inhiraf", "l" },
                    { 24, "Water (Maa')", "مَاء", "م", "Both lips closed together — bilabial nasal", "Lips", "مِيم", "মিম", "Mim", 24, "Jahr,Tawassut,Istifal,Infitah,Idhlag", "m" },
                    { 25, "Light (Nur)", "نُور", "ن", "Tip of tongue near the upper gum ridge — alveolar nasal", "TongueTipAlveolar", "نُون", "নুন", "Nun", 25, "Jahr,Tawassut,Istifal,Infitah,Idhlag,Ghunnah", "n" },
                    { 26, "Child (Walad)", "وَلَد", "و", "Both lips rounded and slightly apart — labio-velar semi-vowel", "Lips", "وَاو", "ওয়াও", "Waw", 26, "Jahr,Rakhawa,Istifal,Infitah,Idhlag,Lin", "w / ū" },
                    { 27, "Guidance (Huda)", "هُدَى", "ه", "Deepest part of the throat — a soft, breathy voiceless glottal fricative", "Throat", "هَاء", "হা", "Ha", 27, "Hams,Rakhawa,Istifal,Infitah,Idhlag", "h" },
                    { 28, "Day (Yawm)", "يَوْم", "ي", "Middle of the tongue rises toward the hard palate — palatal semi-vowel", "TongueFront", "يَاء", "ইয়া", "Ya", 28, "Jahr,Rakhawa,Istifal,Infitah,Idhlag,Lin", "y / ī" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArabicLetters_Order",
                table: "ArabicLetters",
                column: "Order",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArabicLetters");
        }
    }
}
