using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewArabicLetterEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 28);

            // Drop FKs and change column types using raw SQL (int→uuid cannot auto-cast in PostgreSQL)
            migrationBuilder.Sql(@"
                ALTER TABLE ""TajweedPracticeRecords"" DROP CONSTRAINT IF EXISTS ""FK_TajweedPracticeRecords_ArabicLetters_LetterId"";
                ALTER TABLE ""LetterHarakatItems"" DROP CONSTRAINT IF EXISTS ""FK_LetterHarakatItems_ArabicLetters_LetterId"";
                TRUNCATE TABLE ""LetterHarakatItems"";
                TRUNCATE TABLE ""TajweedPracticeRecords"";
                ALTER TABLE ""TajweedPracticeRecords"" ALTER COLUMN ""LetterId"" TYPE uuid USING NULL::uuid;
                ALTER TABLE ""LetterHarakatItems"" ALTER COLUMN ""LetterId"" TYPE uuid USING NULL::uuid;
                ALTER TABLE ""ArabicLetters"" DROP CONSTRAINT ""PK_ArabicLetters"";
                ALTER TABLE ""ArabicLetters"" ALTER COLUMN ""Id"" DROP IDENTITY IF EXISTS;
                ALTER TABLE ""ArabicLetters"" ALTER COLUMN ""Id"" TYPE uuid USING NULL::uuid;
                ALTER TABLE ""ArabicLetters"" ADD CONSTRAINT ""PK_ArabicLetters"" PRIMARY KEY (""Id"");
            ");

            migrationBuilder.RenameColumn(
                name: "Transliteration",
                table: "ArabicLetters",
                newName: "TransliterationEn");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "ArabicLetters",
                newName: "SequenceOrder");

            migrationBuilder.RenameColumn(
                name: "NameEnglish",
                table: "ArabicLetters",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "NameBangla",
                table: "ArabicLetters",
                newName: "NameBn");

            migrationBuilder.RenameColumn(
                name: "MakhrajDescription",
                table: "ArabicLetters",
                newName: "MakhrajDescriptionEn");

            migrationBuilder.RenameColumn(
                name: "Letter",
                table: "ArabicLetters",
                newName: "Character");

            migrationBuilder.RenameColumn(
                name: "ExampleWord",
                table: "ArabicLetters",
                newName: "ExampleWordEn");

            migrationBuilder.RenameIndex(
                name: "IX_ArabicLetters_Order",
                table: "ArabicLetters",
                newName: "IX_ArabicLetters_SequenceOrder");

            migrationBuilder.AddColumn<string>(
                name: "TransliterationBn",
                table: "ArabicLetters",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "ArabicLetters",
                columns: new[] { "Id", "Character", "ExampleWordArabic", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajType", "MedialForm", "NameArabic", "SequenceOrder", "Sifaat", "ExampleWordBn", "ExampleWordEn", "MakhrajDescriptionBn", "MakhrajDescriptionEn", "NameBn", "NameEn", "TransliterationBn", "TransliterationEn" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "ا", "أَحَد", "ـا", "ا", false, "ا", "Throat", "ـا", "أَلِف", 1, "Jahr,Rakhawa,Istifal,Infitah,Idhlag", "এক/একক", "One (Ahad)", "কণ্ঠের সর্বনিম্ন স্থান — হামযার উচ্চারণস্থল", "Deepest part of the throat (Hamza); Alif itself is a vowel carrier", "আলিফ", "Alif", "", "ā / ʾ" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "ب", "بَيْت", "ـب", "بـ", true, "ب", "Lips", "ـبـ", "بَاء", 2, "Jahr,Shiddah,Istifal,Infitah,Idhlag,Qalqalah", "বাড়ি/ঘর", "House (Bayt)", "উভয় ঠোঁট একসাথে চেপে — দুই ঠোঁটের শব্দ", "Both lips pressed together (bilabial stop)", "বা", "Ba", "", "b" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "ت", "تَمْر", "ـت", "تـ", true, "ت", "TongueTipDental", "ـتـ", "تَاء", 3, "Hams,Shiddah,Istifal,Infitah,Idhlag", "খেজুর", "Dates (Tamr)", "জিহ্বার অগ্রভাগ ওপরের সামনের দাঁতে লাগিয়ে", "Tip of tongue touches the upper front teeth", "তা", "Ta", "", "t" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "ث", "ثَوْب", "ـث", "ثـ", true, "ث", "TongueTipInterdental", "ـثـ", "ثَاء", 4, "Hams,Rakhawa,Istifal,Infitah,Idhlag", "পোশাক", "Garment (Thawb)", "জিহ্বার অগ্রভাগ ওপর-নিচের দাঁতের মাঝখানে রেখে", "Tip of tongue lightly between the upper and lower teeth", "ছা", "Tha", "", "th" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), "ج", "جَبَل", "ـج", "جـ", true, "ج", "TongueFront", "ـجـ", "جِيم", 5, "Jahr,Shiddah,Istifal,Infitah,Idhlag,Qalqalah", "পাহাড়", "Mountain (Jabal)", "জিহ্বার মধ্যভাগ শক্ত তালুর সাথে মিলিয়ে", "Middle of the tongue meets the hard palate", "জিম", "Jim", "", "j" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), "ح", "حَق", "ـح", "حـ", true, "ح", "UpperThroat", "ـحـ", "حَاء", 6, "Hams,Rakhawa,Istifal,Infitah,Idhlag", "সত্য/অধিকার", "Truth (Haqq)", "কণ্ঠের মধ্যস্থান — নিঃশ্বাসের মতো শব্দ, কোনো কম্পন নেই", "Middle of the throat — a breathy, voiceless pharyngeal fricative", "হা", "Ha", "", "ḥ" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), "خ", "خَيْر", "ـخ", "خـ", true, "خ", "UpperThroat", "ـخـ", "خَاء", 7, "Hams,Rakhawa,Isti'la,Infitah,Idhlag", "কল্যাণ/ভালো", "Goodness (Khayr)", "কণ্ঠের উপরের অংশ — গার্গলিং 'খ' শব্দ", "Upper throat (closest to mouth) — velar fricative", "খা", "Kha", "", "kh" },
                    { new Guid("00000000-0000-0000-0000-000000000008"), "د", "دِين", "ـد", "د", false, "د", "TongueTipDental", "ـد", "دَال", 8, "Jahr,Shiddah,Istifal,Infitah,Idhlag,Qalqalah", "ধর্ম", "Religion (Deen)", "জিহ্বার অগ্রভাগ ও পার্শ্ব ওপরের সামনের দাঁতে", "Tip and sides of tongue against the upper front teeth", "দাল", "Dal", "", "d" },
                    { new Guid("00000000-0000-0000-0000-000000000009"), "ذ", "ذِكْر", "ـذ", "ذ", false, "ذ", "TongueTipInterdental", "ـذ", "ذَال", 9, "Jahr,Rakhawa,Istifal,Infitah,Idhlag", "স্মরণ/যিকর", "Remembrance (Dhikr)", "জিহ্বার অগ্রভাগ দাঁতের মাঝখানে — কম্পনযুক্ত", "Tip of tongue lightly between the teeth (voiced)", "যাল", "Dhal", "", "dh" },
                    { new Guid("00000000-0000-0000-0000-000000000010"), "ر", "رَحْمَة", "ـر", "ر", false, "ر", "TongueTipTrilled", "ـر", "رَاء", 10, "Jahr,Tawassut,Istifal,Infitah,Idhlag,Takrir", "দয়া/করুণা", "Mercy (Rahma)", "জিহ্বার অগ্রভাগ ওপরের মাড়ির কাছে — কম্পমান 'র'", "Tip of tongue near the upper gum ridge — trilled 'r'", "রা", "Ra", "", "r" },
                    { new Guid("00000000-0000-0000-0000-000000000011"), "ز", "زَيْت", "ـز", "ز", false, "ز", "TongueTipSibilant", "ـز", "زَاي", 11, "Jahr,Rakhawa,Istifal,Infitah,Idhlag,Safeer", "তেল", "Oil (Zayt)", "জিহ্বার অগ্রভাগ নিচের সামনের দাঁতের কাছে — কম্পনযুক্ত", "Tip of tongue near lower front teeth — voiced sibilant", "যায়", "Zay", "", "z" },
                    { new Guid("00000000-0000-0000-0000-000000000012"), "س", "سَلَام", "ـس", "سـ", true, "س", "TongueTipSibilant", "ـسـ", "سِين", 12, "Hams,Rakhawa,Istifal,Infitah,Idhlag,Safeer", "শান্তি", "Peace (Salaam)", "জিহ্বার অগ্রভাগ নিচের দাঁতের কাছে — অ-কম্পনযুক্ত 'স'", "Tip of tongue near lower front teeth — voiceless sibilant", "সিন", "Sin", "", "s" },
                    { new Guid("00000000-0000-0000-0000-000000000013"), "ش", "شَمْس", "ـش", "شـ", true, "ش", "TongueFront", "ـشـ", "شِين", 13, "Hams,Rakhawa,Istifal,Infitah,Idhlag,Tafasshi", "সূর্য", "Sun (Shams)", "জিহ্বার মধ্যভাগ শক্ত তালুর দিকে ছড়িয়ে — 'শ' শব্দ", "Middle of tongue spread toward the hard palate — 'sh' sound", "শিন", "Shin", "", "sh" },
                    { new Guid("00000000-0000-0000-0000-000000000014"), "ص", "صَبْر", "ـص", "صـ", true, "ص", "TongueTipSibilant", "ـصـ", "صَاد", 14, "Hams,Rakhawa,Isti'la,Itbaq,Idhlag,Safeer", "ধৈর্য", "Patience (Sabr)", "জিহ্বার অগ্রভাগ সামনের দাঁতের কাছে — ভারী জোরালো 'স'", "Tip of tongue near front teeth — heavy emphatic 'S'", "সাদ", "Sad", "", "ṣ" },
                    { new Guid("00000000-0000-0000-0000-000000000015"), "ض", "ضَوْء", "ـض", "ضـ", true, "ض", "TongueSide", "ـضـ", "ضَاد", 15, "Jahr,Rakhawa,Isti'la,Itbaq,Idhlag", "আলো", "Light (Daw')", "জিহ্বার এক বা উভয় পার্শ্ব ওপরের পেছনের দাঁতের সাথে", "One or both sides of tongue against the upper back molars", "দোয়াদ", "Dad", "", "ḍ" },
                    { new Guid("00000000-0000-0000-0000-000000000016"), "ط", "طَرِيق", "ـط", "طـ", true, "ط", "TongueTipDental", "ـطـ", "طَاء", 16, "Jahr,Shiddah,Isti'la,Itbaq,Idhlag,Qalqalah", "রাস্তা/পথ", "Road (Tariq)", "জিহ্বার অগ্রভাগ ওপরের সামনের দাঁতে — ভারী জোরালো 'ত'", "Tip of tongue touches upper front teeth — emphatic heavy 'T'", "তোয়া", "Taa", "", "ṭ" },
                    { new Guid("00000000-0000-0000-0000-000000000017"), "ظ", "ظُلْم", "ـظ", "ظـ", true, "ظ", "TongueTipInterdental", "ـظـ", "ظَاء", 17, "Jahr,Rakhawa,Isti'la,Itbaq,Idhlag", "অত্যাচার", "Oppression (Dhulm)", "জিহ্বার অগ্রভাগ দাঁতের মাঝে — ভারী আন্তর-দন্তীয়", "Tip of tongue between teeth — heavy emphatic interdental", "যোয়া", "Dhaa", "", "ẓ" },
                    { new Guid("00000000-0000-0000-0000-000000000018"), "ع", "عِلْم", "ـع", "عـ", true, "ع", "MidThroat", "ـعـ", "عَيْن", 18, "Jahr,Tawassut,Istifal,Infitah,Idhlag", "জ্ঞান", "Knowledge (Ilm)", "কণ্ঠের মধ্যভাগ — গ্রাসনালীর কম্পনযুক্ত ঘর্ষণ শব্দ", "Middle of the throat — voiced pharyngeal fricative", "আইন", "Ayn", "", "ʿ" },
                    { new Guid("00000000-0000-0000-0000-000000000019"), "غ", "غَيْب", "ـغ", "غـ", true, "غ", "MidThroat", "ـغـ", "غَيْن", 19, "Jahr,Rakhawa,Isti'la,Infitah,Idhlag", "অদৃশ্য/গায়েব", "Unseen (Ghayb)", "কণ্ঠের উপরিভাগ — কম্পনযুক্ত 'গ' জাতীয় শব্দ", "Upper throat — voiced velar fricative, a gargling 'gh' sound", "গাইন", "Ghayn", "", "gh" },
                    { new Guid("00000000-0000-0000-0000-000000000020"), "ف", "فَجْر", "ـف", "فـ", true, "ف", "Labiodental", "ـفـ", "فَاء", 20, "Hams,Rakhawa,Istifal,Infitah,Idhlag", "ভোর/ফজর", "Dawn (Fajr)", "নিচের ঠোঁটের ভেতরের অংশ ওপরের সামনের দাঁতের ডগায়", "Inner edge of lower lip touches tips of upper front teeth", "ফা", "Fa", "", "f" },
                    { new Guid("00000000-0000-0000-0000-000000000021"), "ق", "قُرْآن", "ـق", "قـ", true, "ق", "TongueBack", "ـقـ", "قَاف", 21, "Jahr,Shiddah,Isti'la,Infitah,Ismat,Qalqalah", "কুরআন", "Quran (Quran)", "জিহ্বার পশ্চাৎভাগ নরম তালুতে লাগিয়ে — গভীর 'ক' শব্দ", "Back of tongue touches the soft palate (uvular stop)", "কাফ", "Qaf", "", "q" },
                    { new Guid("00000000-0000-0000-0000-000000000022"), "ك", "كِتَاب", "ـك", "كـ", true, "ك", "TongueBack", "ـكـ", "كَاف", 22, "Hams,Shiddah,Istifal,Infitah,Ismat", "বই/কিতাব", "Book (Kitab)", "জিহ্বার পশ্চাৎভাগ শক্ত তালুতে — কাফের চেয়ে সামনে", "Back of tongue touches the hard palate — slightly forward of Qaf", "কাফ (ছোট)", "Kaf", "", "k" },
                    { new Guid("00000000-0000-0000-0000-000000000023"), "ل", "لَيْل", "ـل", "لـ", true, "ل", "TongueSide", "ـلـ", "لَام", 23, "Jahr,Tawassut,Istifal,Infitah,Idhlag,Inhiraf", "রাত", "Night (Layl)", "জিহ্বার অগ্রভাগ ও পার্শ্ব ওপরের মাড়ির পাশে — পার্শ্বীয় শব্দ", "Tip and sides of tongue along the upper gum ridge — lateral", "লাম", "Lam", "", "l" },
                    { new Guid("00000000-0000-0000-0000-000000000024"), "م", "مَاء", "ـم", "مـ", true, "م", "Lips", "ـمـ", "مِيم", 24, "Jahr,Tawassut,Istifal,Infitah,Idhlag", "পানি", "Water (Maa')", "উভয় ঠোঁট বন্ধ রেখে — নাসিক শব্দ (মুখ বন্ধ, নাক দিয়ে বাতাস)", "Both lips closed together — bilabial nasal", "মিম", "Mim", "", "m" },
                    { new Guid("00000000-0000-0000-0000-000000000025"), "ن", "نُور", "ـن", "نـ", true, "ن", "TongueTipAlveolar", "ـنـ", "نُون", 25, "Jahr,Tawassut,Istifal,Infitah,Idhlag,Ghunnah", "আলো", "Light (Nur)", "জিহ্বার অগ্রভাগ ওপরের মাড়ির কাছে — অনুনাসিক শব্দ", "Tip of tongue near the upper gum ridge — alveolar nasal", "নুন", "Nun", "", "n" },
                    { new Guid("00000000-0000-0000-0000-000000000026"), "و", "وَلَد", "ـو", "و", false, "و", "Lips", "ـو", "وَاو", 26, "Jahr,Rakhawa,Istifal,Infitah,Idhlag,Lin", "সন্তান", "Child (Walad)", "উভয় ঠোঁট গোলাকারভাবে সামান্য ফাঁক রেখে — অর্ধ-স্বরধ্বনি", "Both lips rounded and slightly apart — labio-velar semi-vowel", "ওয়াও", "Waw", "", "w / ū" },
                    { new Guid("00000000-0000-0000-0000-000000000027"), "ه", "هُدَى", "ـه", "هـ", true, "ه", "Throat", "ـهـ", "هَاء", 27, "Hams,Rakhawa,Istifal,Infitah,Idhlag", "পথনির্দেশনা", "Guidance (Huda)", "কণ্ঠের সর্বনিম্ন স্থান — শীতল নিঃশ্বাসের মতো শব্দ", "Deepest part of the throat — soft, breathy voiceless glottal fricative", "হা (গোল হা)", "Ha", "", "h" },
                    { new Guid("00000000-0000-0000-0000-000000000028"), "ي", "يَوْم", "ـي", "يـ", true, "ي", "TongueFront", "ـيـ", "يَاء", 28, "Jahr,Rakhawa,Istifal,Infitah,Idhlag,Lin", "দিন", "Day (Yawm)", "জিহ্বার মধ্যভাগ শক্ত তালুর দিকে উঠিয়ে — তালব্য অর্ধ-স্বর", "Middle of tongue rises toward the hard palate — palatal semi-vowel", "ইয়া", "Ya", "", "y / ī" }
                });

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 10,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 11,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 12,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 13,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 14,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 15,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 16,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 17,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 18,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 19,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 20,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 21,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 22,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 23,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 24,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 25,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 26,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 27,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 28,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 29,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 30,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 31,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 32,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 33,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000005"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 34,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000005"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 35,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000005"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 36,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000005"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 37,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000005"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 38,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000005"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 39,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000005"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 40,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000005"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 41,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000006"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 42,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000006"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 43,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000006"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 44,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000006"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 45,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000006"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 46,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000006"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 47,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000006"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 48,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000006"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 49,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000007"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 50,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000007"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 51,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000007"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 52,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000007"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 53,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000007"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 54,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000007"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 55,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000007"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 56,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000007"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 57,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000008"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 58,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000008"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 59,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000008"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 60,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000008"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 61,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000008"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 62,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000008"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 63,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000008"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 64,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000008"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 65,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000009"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 66,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000009"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 67,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000009"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 68,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000009"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 69,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000009"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 70,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000009"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 71,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000009"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 72,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000009"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 73,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 74,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 75,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 76,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 77,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 78,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 79,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 80,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 81,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000011"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 82,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000011"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 83,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000011"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 84,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000011"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 85,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000011"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 86,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000011"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 87,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000011"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 88,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000011"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 89,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000012"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 90,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000012"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 91,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000012"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 92,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000012"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 93,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000012"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 94,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000012"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 95,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000012"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 96,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000012"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 97,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000013"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 98,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000013"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 99,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000013"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 100,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000013"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 101,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000013"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 102,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000013"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 103,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000013"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 104,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000013"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 105,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000014"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 106,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000014"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 107,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000014"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 108,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000014"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 109,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000014"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 110,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000014"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 111,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000014"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 112,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000014"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 113,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000015"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 114,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000015"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 115,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000015"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 116,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000015"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 117,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000015"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 118,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000015"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 119,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000015"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 120,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000015"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 121,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000016"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 122,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000016"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 123,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000016"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 124,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000016"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 125,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000016"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 126,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000016"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 127,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000016"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 128,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000016"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 129,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000017"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 130,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000017"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 131,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000017"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 132,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000017"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 133,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000017"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 134,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000017"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 135,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000017"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 136,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000017"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 137,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000018"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 138,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000018"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 139,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000018"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 140,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000018"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 141,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000018"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 142,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000018"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 143,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000018"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 144,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000018"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 145,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000019"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 146,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000019"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 147,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000019"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 148,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000019"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 149,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000019"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 150,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000019"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 151,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000019"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 152,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000019"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 153,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000020"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 154,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000020"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 155,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000020"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 156,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000020"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 157,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000020"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 158,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000020"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 159,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000020"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 160,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000020"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 161,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000021"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 162,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000021"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 163,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000021"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 164,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000021"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 165,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000021"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 166,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000021"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 167,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000021"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 168,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000021"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 169,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000022"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 170,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000022"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 171,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000022"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 172,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000022"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 173,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000022"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 174,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000022"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 175,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000022"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 176,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000022"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 177,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000023"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 178,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000023"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 179,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000023"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 180,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000023"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 181,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000023"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 182,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000023"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 183,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000023"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 184,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000023"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 185,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000024"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 186,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000024"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 187,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000024"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 188,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000024"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 189,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000024"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 190,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000024"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 191,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000024"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 192,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000024"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 193,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000025"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 194,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000025"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 195,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000025"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 196,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000025"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 197,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000025"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 198,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000025"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 199,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000025"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 200,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000025"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 201,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000026"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 202,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000026"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 203,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000026"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 204,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000026"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 205,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000026"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 206,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000026"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 207,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000026"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 208,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000026"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 209,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000027"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 210,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000027"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 211,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000027"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 212,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000027"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 213,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000027"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 214,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000027"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 215,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000027"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 216,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000027"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 217,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000028"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 218,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000028"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 219,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000028"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 220,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000028"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 221,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000028"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 222,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000028"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 223,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000028"));

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 224,
                column: "LetterId",
                value: new Guid("00000000-0000-0000-0000-000000000028"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000026"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000027"));

            migrationBuilder.DeleteData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000028"));

            migrationBuilder.DropColumn(
                name: "TransliterationBn",
                table: "ArabicLetters");

            migrationBuilder.RenameColumn(
                name: "TransliterationEn",
                table: "ArabicLetters",
                newName: "Transliteration");

            migrationBuilder.RenameColumn(
                name: "SequenceOrder",
                table: "ArabicLetters",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "ArabicLetters",
                newName: "NameEnglish");

            migrationBuilder.RenameColumn(
                name: "NameBn",
                table: "ArabicLetters",
                newName: "NameBangla");

            migrationBuilder.RenameColumn(
                name: "MakhrajDescriptionEn",
                table: "ArabicLetters",
                newName: "MakhrajDescription");

            migrationBuilder.RenameColumn(
                name: "ExampleWordEn",
                table: "ArabicLetters",
                newName: "ExampleWord");

            migrationBuilder.RenameColumn(
                name: "Character",
                table: "ArabicLetters",
                newName: "Letter");

            migrationBuilder.RenameIndex(
                name: "IX_ArabicLetters_SequenceOrder",
                table: "ArabicLetters",
                newName: "IX_ArabicLetters_Order");

            migrationBuilder.AlterColumn<int>(
                name: "LetterId",
                table: "TajweedPracticeRecords",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "LetterId",
                table: "LetterHarakatItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ArabicLetters",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.InsertData(
                table: "ArabicLetters",
                columns: new[] { "Id", "ExampleWord", "ExampleWordArabic", "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "Letter", "MakhrajDescription", "MakhrajDescriptionBn", "MakhrajType", "MedialForm", "NameArabic", "NameBangla", "NameEnglish", "Order", "Sifaat", "Transliteration" },
                values: new object[,]
                {
                    { 1, "One (Ahad)", "أَحَد", "এক/একক", "ـا", "ا", false, "ا", "ا", "Deepest part of the throat (Hamza); Alif itself is a vowel carrier", "কণ্ঠের সর্বনিম্ন স্থান — হামযার উচ্চারণস্থল", "Throat", "ـا", "أَلِف", "আলিফ", "Alif", 1, "Jahr,Rakhawa,Istifal,Infitah,Idhlag", "ā / ʾ" },
                    { 2, "House (Bayt)", "بَيْت", "বাড়ি/ঘর", "ـب", "بـ", true, "ب", "ب", "Both lips pressed together (bilabial stop)", "উভয় ঠোঁট একসাথে চেপে — দুই ঠোঁটের শব্দ", "Lips", "ـبـ", "بَاء", "বা", "Ba", 2, "Jahr,Shiddah,Istifal,Infitah,Idhlag,Qalqalah", "b" },
                    { 3, "Dates (Tamr)", "تَمْر", "খেজুর", "ـت", "تـ", true, "ت", "ت", "Tip of tongue touches the upper front teeth", "জিহ্বার অগ্রভাগ ওপরের সামনের দাঁতে লাগিয়ে", "TongueTipDental", "ـتـ", "تَاء", "তা", "Ta", 3, "Hams,Shiddah,Istifal,Infitah,Idhlag", "t" },
                    { 4, "Garment (Thawb)", "ثَوْب", "পোশাক", "ـث", "ثـ", true, "ث", "ث", "Tip of tongue lightly between the upper and lower teeth", "জিহ্বার অগ্রভাগ ওপর-নিচের দাঁতের মাঝখানে রেখে", "TongueTipInterdental", "ـثـ", "ثَاء", "ছা", "Tha", 4, "Hams,Rakhawa,Istifal,Infitah,Idhlag", "th" },
                    { 5, "Mountain (Jabal)", "جَبَل", "পাহাড়", "ـج", "جـ", true, "ج", "ج", "Middle of the tongue meets the hard palate", "জিহ্বার মধ্যভাগ শক্ত তালুর সাথে মিলিয়ে", "TongueFront", "ـجـ", "جِيم", "জিম", "Jim", 5, "Jahr,Shiddah,Istifal,Infitah,Idhlag,Qalqalah", "j" },
                    { 6, "Truth (Haqq)", "حَق", "সত্য/অধিকার", "ـح", "حـ", true, "ح", "ح", "Middle of the throat — a breathy, voiceless pharyngeal fricative", "কণ্ঠের মধ্যস্থান — নিঃশ্বাসের মতো শব্দ, কোনো কম্পন নেই", "UpperThroat", "ـحـ", "حَاء", "হা", "Ha", 6, "Hams,Rakhawa,Istifal,Infitah,Idhlag", "ḥ" },
                    { 7, "Goodness (Khayr)", "خَيْر", "কল্যাণ/ভালো", "ـخ", "خـ", true, "خ", "خ", "Upper throat (closest to mouth) — velar fricative", "কণ্ঠের উপরের অংশ — গার্গলিং 'খ' শব্দ", "UpperThroat", "ـخـ", "خَاء", "খা", "Kha", 7, "Hams,Rakhawa,Isti'la,Infitah,Idhlag", "kh" },
                    { 8, "Religion (Deen)", "دِين", "ধর্ম", "ـد", "د", false, "د", "د", "Tip and sides of tongue against the upper front teeth", "জিহ্বার অগ্রভাগ ও পার্শ্ব ওপরের সামনের দাঁতে", "TongueTipDental", "ـد", "دَال", "দাল", "Dal", 8, "Jahr,Shiddah,Istifal,Infitah,Idhlag,Qalqalah", "d" },
                    { 9, "Remembrance (Dhikr)", "ذِكْر", "স্মরণ/যিকর", "ـذ", "ذ", false, "ذ", "ذ", "Tip of tongue lightly between the teeth (voiced)", "জিহ্বার অগ্রভাগ দাঁতের মাঝখানে — কম্পনযুক্ত", "TongueTipInterdental", "ـذ", "ذَال", "যাল", "Dhal", 9, "Jahr,Rakhawa,Istifal,Infitah,Idhlag", "dh" },
                    { 10, "Mercy (Rahma)", "رَحْمَة", "দয়া/করুণা", "ـر", "ر", false, "ر", "ر", "Tip of tongue near the upper gum ridge — trilled 'r'", "জিহ্বার অগ্রভাগ ওপরের মাড়ির কাছে — কম্পমান 'র'", "TongueTipTrilled", "ـر", "رَاء", "রা", "Ra", 10, "Jahr,Tawassut,Istifal,Infitah,Idhlag,Takrir", "r" },
                    { 11, "Oil (Zayt)", "زَيْت", "তেল", "ـز", "ز", false, "ز", "ز", "Tip of tongue near lower front teeth — voiced sibilant", "জিহ্বার অগ্রভাগ নিচের সামনের দাঁতের কাছে — কম্পনযুক্ত", "TongueTipSibilant", "ـز", "زَاي", "যায়", "Zay", 11, "Jahr,Rakhawa,Istifal,Infitah,Idhlag,Safeer", "z" },
                    { 12, "Peace (Salaam)", "سَلَام", "শান্তি", "ـس", "سـ", true, "س", "س", "Tip of tongue near lower front teeth — voiceless sibilant", "জিহ্বার অগ্রভাগ নিচের দাঁতের কাছে — অ-কম্পনযুক্ত 'স'", "TongueTipSibilant", "ـسـ", "سِين", "সিন", "Sin", 12, "Hams,Rakhawa,Istifal,Infitah,Idhlag,Safeer", "s" },
                    { 13, "Sun (Shams)", "شَمْس", "সূর্য", "ـش", "شـ", true, "ش", "ش", "Middle of tongue spread toward the hard palate — 'sh' sound", "জিহ্বার মধ্যভাগ শক্ত তালুর দিকে ছড়িয়ে — 'শ' শব্দ", "TongueFront", "ـشـ", "شِين", "শিন", "Shin", 13, "Hams,Rakhawa,Istifal,Infitah,Idhlag,Tafasshi", "sh" },
                    { 14, "Patience (Sabr)", "صَبْر", "ধৈর্য", "ـص", "صـ", true, "ص", "ص", "Tip of tongue near front teeth — heavy emphatic 'S'", "জিহ্বার অগ্রভাগ সামনের দাঁতের কাছে — ভারী জোরালো 'স'", "TongueTipSibilant", "ـصـ", "صَاد", "সাদ", "Sad", 14, "Hams,Rakhawa,Isti'la,Itbaq,Idhlag,Safeer", "ṣ" },
                    { 15, "Light (Daw')", "ضَوْء", "আলো", "ـض", "ضـ", true, "ض", "ض", "One or both sides of tongue against the upper back molars", "জিহ্বার এক বা উভয় পার্শ্ব ওপরের পেছনের দাঁতের সাথে", "TongueSide", "ـضـ", "ضَاد", "দোয়াদ", "Dad", 15, "Jahr,Rakhawa,Isti'la,Itbaq,Idhlag", "ḍ" },
                    { 16, "Road (Tariq)", "طَرِيق", "রাস্তা/পথ", "ـط", "طـ", true, "ط", "ط", "Tip of tongue touches upper front teeth — emphatic heavy 'T'", "জিহ্বার অগ্রভাগ ওপরের সামনের দাঁতে — ভারী জোরালো 'ত'", "TongueTipDental", "ـطـ", "طَاء", "তোয়া", "Taa", 16, "Jahr,Shiddah,Isti'la,Itbaq,Idhlag,Qalqalah", "ṭ" },
                    { 17, "Oppression (Dhulm)", "ظُلْم", "অত্যাচার", "ـظ", "ظـ", true, "ظ", "ظ", "Tip of tongue between teeth — heavy emphatic interdental", "জিহ্বার অগ্রভাগ দাঁতের মাঝে — ভারী আন্তর-দন্তীয়", "TongueTipInterdental", "ـظـ", "ظَاء", "যোয়া", "Dhaa", 17, "Jahr,Rakhawa,Isti'la,Itbaq,Idhlag", "ẓ" },
                    { 18, "Knowledge (Ilm)", "عِلْم", "জ্ঞান", "ـع", "عـ", true, "ع", "ع", "Middle of the throat — voiced pharyngeal fricative", "কণ্ঠের মধ্যভাগ — গ্রাসনালীর কম্পনযুক্ত ঘর্ষণ শব্দ", "MidThroat", "ـعـ", "عَيْن", "আইন", "Ayn", 18, "Jahr,Tawassut,Istifal,Infitah,Idhlag", "ʿ" },
                    { 19, "Unseen (Ghayb)", "غَيْب", "অদৃশ্য/গায়েব", "ـغ", "غـ", true, "غ", "غ", "Upper throat — voiced velar fricative, a gargling 'gh' sound", "কণ্ঠের উপরিভাগ — কম্পনযুক্ত 'গ' জাতীয় শব্দ", "MidThroat", "ـغـ", "غَيْن", "গাইন", "Ghayn", 19, "Jahr,Rakhawa,Isti'la,Infitah,Idhlag", "gh" },
                    { 20, "Dawn (Fajr)", "فَجْر", "ভোর/ফজর", "ـف", "فـ", true, "ف", "ف", "Inner edge of lower lip touches tips of upper front teeth", "নিচের ঠোঁটের ভেতরের অংশ ওপরের সামনের দাঁতের ডগায়", "Labiodental", "ـفـ", "فَاء", "ফা", "Fa", 20, "Hams,Rakhawa,Istifal,Infitah,Idhlag", "f" },
                    { 21, "Quran (Quran)", "قُرْآن", "কুরআন", "ـق", "قـ", true, "ق", "ق", "Back of tongue touches the soft palate (uvular stop)", "জিহ্বার পশ্চাৎভাগ নরম তালুতে লাগিয়ে — গভীর 'ক' শব্দ", "TongueBack", "ـقـ", "قَاف", "কাফ", "Qaf", 21, "Jahr,Shiddah,Isti'la,Infitah,Ismat,Qalqalah", "q" },
                    { 22, "Book (Kitab)", "كِتَاب", "বই/কিতাব", "ـك", "كـ", true, "ك", "ك", "Back of tongue touches the hard palate — slightly forward of Qaf", "জিহ্বার পশ্চাৎভাগ শক্ত তালুতে — কাফের চেয়ে সামনে", "TongueBack", "ـكـ", "كَاف", "কাফ (ছোট)", "Kaf", 22, "Hams,Shiddah,Istifal,Infitah,Ismat", "k" },
                    { 23, "Night (Layl)", "لَيْل", "রাত", "ـل", "لـ", true, "ل", "ل", "Tip and sides of tongue along the upper gum ridge — lateral", "জিহ্বার অগ্রভাগ ও পার্শ্ব ওপরের মাড়ির পাশে — পার্শ্বীয় শব্দ", "TongueSide", "ـلـ", "لَام", "লাম", "Lam", 23, "Jahr,Tawassut,Istifal,Infitah,Idhlag,Inhiraf", "l" },
                    { 24, "Water (Maa')", "مَاء", "পানি", "ـم", "مـ", true, "م", "م", "Both lips closed together — bilabial nasal", "উভয় ঠোঁট বন্ধ রেখে — নাসিক শব্দ (মুখ বন্ধ, নাক দিয়ে বাতাস)", "Lips", "ـمـ", "مِيم", "মিম", "Mim", 24, "Jahr,Tawassut,Istifal,Infitah,Idhlag", "m" },
                    { 25, "Light (Nur)", "نُور", "আলো", "ـن", "نـ", true, "ن", "ن", "Tip of tongue near the upper gum ridge — alveolar nasal", "জিহ্বার অগ্রভাগ ওপরের মাড়ির কাছে — অনুনাসিক শব্দ", "TongueTipAlveolar", "ـنـ", "نُون", "নুন", "Nun", 25, "Jahr,Tawassut,Istifal,Infitah,Idhlag,Ghunnah", "n" },
                    { 26, "Child (Walad)", "وَلَد", "সন্তান", "ـو", "و", false, "و", "و", "Both lips rounded and slightly apart — labio-velar semi-vowel", "উভয় ঠোঁট গোলাকারভাবে সামান্য ফাঁক রেখে — অর্ধ-স্বরধ্বনি", "Lips", "ـو", "وَاو", "ওয়াও", "Waw", 26, "Jahr,Rakhawa,Istifal,Infitah,Idhlag,Lin", "w / ū" },
                    { 27, "Guidance (Huda)", "هُدَى", "পথনির্দেশনা", "ـه", "هـ", true, "ه", "ه", "Deepest part of the throat — soft, breathy voiceless glottal fricative", "কণ্ঠের সর্বনিম্ন স্থান — শীতল নিঃশ্বাসের মতো শব্দ", "Throat", "ـهـ", "هَاء", "হা (গোল হা)", "Ha", 27, "Hams,Rakhawa,Istifal,Infitah,Idhlag", "h" },
                    { 28, "Day (Yawm)", "يَوْم", "দিন", "ـي", "يـ", true, "ي", "ي", "Middle of tongue rises toward the hard palate — palatal semi-vowel", "জিহ্বার মধ্যভাগ শক্ত তালুর দিকে উঠিয়ে — তালব্য অর্ধ-স্বর", "TongueFront", "ـيـ", "يَاء", "ইয়া", "Ya", 28, "Jahr,Rakhawa,Istifal,Infitah,Idhlag,Lin", "y / ī" }
                });

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "LetterId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "LetterId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "LetterId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "LetterId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "LetterId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "LetterId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "LetterId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "LetterId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "LetterId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 10,
                column: "LetterId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 11,
                column: "LetterId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 12,
                column: "LetterId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 13,
                column: "LetterId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 14,
                column: "LetterId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 15,
                column: "LetterId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 16,
                column: "LetterId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 17,
                column: "LetterId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 18,
                column: "LetterId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 19,
                column: "LetterId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 20,
                column: "LetterId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 21,
                column: "LetterId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 22,
                column: "LetterId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 23,
                column: "LetterId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 24,
                column: "LetterId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 25,
                column: "LetterId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 26,
                column: "LetterId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 27,
                column: "LetterId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 28,
                column: "LetterId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 29,
                column: "LetterId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 30,
                column: "LetterId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 31,
                column: "LetterId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 32,
                column: "LetterId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 33,
                column: "LetterId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 34,
                column: "LetterId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 35,
                column: "LetterId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 36,
                column: "LetterId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 37,
                column: "LetterId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 38,
                column: "LetterId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 39,
                column: "LetterId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 40,
                column: "LetterId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 41,
                column: "LetterId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 42,
                column: "LetterId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 43,
                column: "LetterId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 44,
                column: "LetterId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 45,
                column: "LetterId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 46,
                column: "LetterId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 47,
                column: "LetterId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 48,
                column: "LetterId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 49,
                column: "LetterId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 50,
                column: "LetterId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 51,
                column: "LetterId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 52,
                column: "LetterId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 53,
                column: "LetterId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 54,
                column: "LetterId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 55,
                column: "LetterId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 56,
                column: "LetterId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 57,
                column: "LetterId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 58,
                column: "LetterId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 59,
                column: "LetterId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 60,
                column: "LetterId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 61,
                column: "LetterId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 62,
                column: "LetterId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 63,
                column: "LetterId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 64,
                column: "LetterId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 65,
                column: "LetterId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 66,
                column: "LetterId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 67,
                column: "LetterId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 68,
                column: "LetterId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 69,
                column: "LetterId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 70,
                column: "LetterId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 71,
                column: "LetterId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 72,
                column: "LetterId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 73,
                column: "LetterId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 74,
                column: "LetterId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 75,
                column: "LetterId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 76,
                column: "LetterId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 77,
                column: "LetterId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 78,
                column: "LetterId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 79,
                column: "LetterId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 80,
                column: "LetterId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 81,
                column: "LetterId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 82,
                column: "LetterId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 83,
                column: "LetterId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 84,
                column: "LetterId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 85,
                column: "LetterId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 86,
                column: "LetterId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 87,
                column: "LetterId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 88,
                column: "LetterId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 89,
                column: "LetterId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 90,
                column: "LetterId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 91,
                column: "LetterId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 92,
                column: "LetterId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 93,
                column: "LetterId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 94,
                column: "LetterId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 95,
                column: "LetterId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 96,
                column: "LetterId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 97,
                column: "LetterId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 98,
                column: "LetterId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 99,
                column: "LetterId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 100,
                column: "LetterId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 101,
                column: "LetterId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 102,
                column: "LetterId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 103,
                column: "LetterId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 104,
                column: "LetterId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 105,
                column: "LetterId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 106,
                column: "LetterId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 107,
                column: "LetterId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 108,
                column: "LetterId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 109,
                column: "LetterId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 110,
                column: "LetterId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 111,
                column: "LetterId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 112,
                column: "LetterId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 113,
                column: "LetterId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 114,
                column: "LetterId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 115,
                column: "LetterId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 116,
                column: "LetterId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 117,
                column: "LetterId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 118,
                column: "LetterId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 119,
                column: "LetterId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 120,
                column: "LetterId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 121,
                column: "LetterId",
                value: 16);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 122,
                column: "LetterId",
                value: 16);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 123,
                column: "LetterId",
                value: 16);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 124,
                column: "LetterId",
                value: 16);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 125,
                column: "LetterId",
                value: 16);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 126,
                column: "LetterId",
                value: 16);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 127,
                column: "LetterId",
                value: 16);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 128,
                column: "LetterId",
                value: 16);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 129,
                column: "LetterId",
                value: 17);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 130,
                column: "LetterId",
                value: 17);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 131,
                column: "LetterId",
                value: 17);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 132,
                column: "LetterId",
                value: 17);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 133,
                column: "LetterId",
                value: 17);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 134,
                column: "LetterId",
                value: 17);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 135,
                column: "LetterId",
                value: 17);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 136,
                column: "LetterId",
                value: 17);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 137,
                column: "LetterId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 138,
                column: "LetterId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 139,
                column: "LetterId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 140,
                column: "LetterId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 141,
                column: "LetterId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 142,
                column: "LetterId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 143,
                column: "LetterId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 144,
                column: "LetterId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 145,
                column: "LetterId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 146,
                column: "LetterId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 147,
                column: "LetterId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 148,
                column: "LetterId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 149,
                column: "LetterId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 150,
                column: "LetterId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 151,
                column: "LetterId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 152,
                column: "LetterId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 153,
                column: "LetterId",
                value: 20);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 154,
                column: "LetterId",
                value: 20);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 155,
                column: "LetterId",
                value: 20);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 156,
                column: "LetterId",
                value: 20);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 157,
                column: "LetterId",
                value: 20);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 158,
                column: "LetterId",
                value: 20);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 159,
                column: "LetterId",
                value: 20);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 160,
                column: "LetterId",
                value: 20);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 161,
                column: "LetterId",
                value: 21);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 162,
                column: "LetterId",
                value: 21);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 163,
                column: "LetterId",
                value: 21);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 164,
                column: "LetterId",
                value: 21);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 165,
                column: "LetterId",
                value: 21);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 166,
                column: "LetterId",
                value: 21);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 167,
                column: "LetterId",
                value: 21);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 168,
                column: "LetterId",
                value: 21);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 169,
                column: "LetterId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 170,
                column: "LetterId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 171,
                column: "LetterId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 172,
                column: "LetterId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 173,
                column: "LetterId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 174,
                column: "LetterId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 175,
                column: "LetterId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 176,
                column: "LetterId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 177,
                column: "LetterId",
                value: 23);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 178,
                column: "LetterId",
                value: 23);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 179,
                column: "LetterId",
                value: 23);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 180,
                column: "LetterId",
                value: 23);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 181,
                column: "LetterId",
                value: 23);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 182,
                column: "LetterId",
                value: 23);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 183,
                column: "LetterId",
                value: 23);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 184,
                column: "LetterId",
                value: 23);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 185,
                column: "LetterId",
                value: 24);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 186,
                column: "LetterId",
                value: 24);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 187,
                column: "LetterId",
                value: 24);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 188,
                column: "LetterId",
                value: 24);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 189,
                column: "LetterId",
                value: 24);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 190,
                column: "LetterId",
                value: 24);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 191,
                column: "LetterId",
                value: 24);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 192,
                column: "LetterId",
                value: 24);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 193,
                column: "LetterId",
                value: 25);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 194,
                column: "LetterId",
                value: 25);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 195,
                column: "LetterId",
                value: 25);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 196,
                column: "LetterId",
                value: 25);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 197,
                column: "LetterId",
                value: 25);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 198,
                column: "LetterId",
                value: 25);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 199,
                column: "LetterId",
                value: 25);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 200,
                column: "LetterId",
                value: 25);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 201,
                column: "LetterId",
                value: 26);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 202,
                column: "LetterId",
                value: 26);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 203,
                column: "LetterId",
                value: 26);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 204,
                column: "LetterId",
                value: 26);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 205,
                column: "LetterId",
                value: 26);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 206,
                column: "LetterId",
                value: 26);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 207,
                column: "LetterId",
                value: 26);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 208,
                column: "LetterId",
                value: 26);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 209,
                column: "LetterId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 210,
                column: "LetterId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 211,
                column: "LetterId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 212,
                column: "LetterId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 213,
                column: "LetterId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 214,
                column: "LetterId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 215,
                column: "LetterId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 216,
                column: "LetterId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 217,
                column: "LetterId",
                value: 28);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 218,
                column: "LetterId",
                value: 28);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 219,
                column: "LetterId",
                value: 28);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 220,
                column: "LetterId",
                value: 28);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 221,
                column: "LetterId",
                value: 28);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 222,
                column: "LetterId",
                value: 28);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 223,
                column: "LetterId",
                value: 28);

            migrationBuilder.UpdateData(
                table: "LetterHarakatItems",
                keyColumn: "Id",
                keyValue: 224,
                column: "LetterId",
                value: 28);
        }
    }
}
