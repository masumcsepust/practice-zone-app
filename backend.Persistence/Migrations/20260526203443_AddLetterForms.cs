using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLetterForms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExampleWordBn",
                table: "ArabicLetters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FinalForm",
                table: "ArabicLetters",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InitialForm",
                table: "ArabicLetters",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsConnector",
                table: "ArabicLetters",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IsolatedForm",
                table: "ArabicLetters",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MakhrajDescriptionBn",
                table: "ArabicLetters",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MedialForm",
                table: "ArabicLetters",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "এক/একক", "ـا", "ا", false, "ا", "কণ্ঠের সর্বনিম্ন স্থান — হামযার উচ্চারণস্থল", "ـا" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "বাড়ি/ঘর", "ـب", "بـ", true, "ب", "উভয় ঠোঁট একসাথে চেপে — দুই ঠোঁটের শব্দ", "ـبـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "খেজুর", "ـت", "تـ", true, "ت", "জিহ্বার অগ্রভাগ ওপরের সামনের দাঁতে লাগিয়ে", "ـتـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "পোশাক", "ـث", "ثـ", true, "ث", "জিহ্বার অগ্রভাগ ওপর-নিচের দাঁতের মাঝখানে রেখে", "ـثـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "পাহাড়", "ـج", "جـ", true, "ج", "জিহ্বার মধ্যভাগ শক্ত তালুর সাথে মিলিয়ে", "ـجـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "সত্য/অধিকার", "ـح", "حـ", true, "ح", "কণ্ঠের মধ্যস্থান — নিঃশ্বাসের মতো শব্দ, কোনো কম্পন নেই", "ـحـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescription", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "কল্যাণ/ভালো", "ـخ", "خـ", true, "خ", "Upper throat (closest to mouth) — velar fricative", "কণ্ঠের উপরের অংশ — গার্গলিং 'খ' শব্দ", "ـخـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "ধর্ম", "ـد", "د", false, "د", "জিহ্বার অগ্রভাগ ও পার্শ্ব ওপরের সামনের দাঁতে", "ـد" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescription", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "স্মরণ/যিকর", "ـذ", "ذ", false, "ذ", "Tip of tongue lightly between the teeth (voiced)", "জিহ্বার অগ্রভাগ দাঁতের মাঝখানে — কম্পনযুক্ত", "ـذ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescription", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "দয়া/করুণা", "ـر", "ر", false, "ر", "Tip of tongue near the upper gum ridge — trilled 'r'", "জিহ্বার অগ্রভাগ ওপরের মাড়ির কাছে — কম্পমান 'র'", "ـر" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescription", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "তেল", "ـز", "ز", false, "ز", "Tip of tongue near lower front teeth — voiced sibilant", "জিহ্বার অগ্রভাগ নিচের সামনের দাঁতের কাছে — কম্পনযুক্ত", "ـز" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescription", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "শান্তি", "ـس", "سـ", true, "س", "Tip of tongue near lower front teeth — voiceless sibilant", "জিহ্বার অগ্রভাগ নিচের দাঁতের কাছে — অ-কম্পনযুক্ত 'স'", "ـسـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescription", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "সূর্য", "ـش", "شـ", true, "ش", "Middle of tongue spread toward the hard palate — 'sh' sound", "জিহ্বার মধ্যভাগ শক্ত তালুর দিকে ছড়িয়ে — 'শ' শব্দ", "ـشـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescription", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "ধৈর্য", "ـص", "صـ", true, "ص", "Tip of tongue near front teeth — heavy emphatic 'S'", "জিহ্বার অগ্রভাগ সামনের দাঁতের কাছে — ভারী জোরালো 'স'", "ـصـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescription", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "আলো", "ـض", "ضـ", true, "ض", "One or both sides of tongue against the upper back molars", "জিহ্বার এক বা উভয় পার্শ্ব ওপরের পেছনের দাঁতের সাথে", "ـضـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "রাস্তা/পথ", "ـط", "طـ", true, "ط", "জিহ্বার অগ্রভাগ ওপরের সামনের দাঁতে — ভারী জোরালো 'ত'", "ـطـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "অত্যাচার", "ـظ", "ظـ", true, "ظ", "জিহ্বার অগ্রভাগ দাঁতের মাঝে — ভারী আন্তর-দন্তীয়", "ـظـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescription", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "জ্ঞান", "ـع", "عـ", true, "ع", "Middle of the throat — voiced pharyngeal fricative", "কণ্ঠের মধ্যভাগ — গ্রাসনালীর কম্পনযুক্ত ঘর্ষণ শব্দ", "ـعـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "অদৃশ্য/গায়েব", "ـغ", "غـ", true, "غ", "কণ্ঠের উপরিভাগ — কম্পনযুক্ত 'গ' জাতীয় শব্দ", "ـغـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescription", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "ভোর/ফজর", "ـف", "فـ", true, "ف", "Inner edge of lower lip touches tips of upper front teeth", "নিচের ঠোঁটের ভেতরের অংশ ওপরের সামনের দাঁতের ডগায়", "ـفـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "কুরআন", "ـق", "قـ", true, "ق", "জিহ্বার পশ্চাৎভাগ নরম তালুতে লাগিয়ে — গভীর 'ক' শব্দ", "ـقـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescriptionBn", "MedialForm", "NameBangla" },
                values: new object[] { "বই/কিতাব", "ـك", "كـ", true, "ك", "জিহ্বার পশ্চাৎভাগ শক্ত তালুতে — কাফের চেয়ে সামনে", "ـكـ", "কাফ (ছোট)" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescription", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "রাত", "ـل", "لـ", true, "ل", "Tip and sides of tongue along the upper gum ridge — lateral", "জিহ্বার অগ্রভাগ ও পার্শ্ব ওপরের মাড়ির পাশে — পার্শ্বীয় শব্দ", "ـلـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "পানি", "ـم", "مـ", true, "م", "উভয় ঠোঁট বন্ধ রেখে — নাসিক শব্দ (মুখ বন্ধ, নাক দিয়ে বাতাস)", "ـمـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "আলো", "ـن", "نـ", true, "ن", "জিহ্বার অগ্রভাগ ওপরের মাড়ির কাছে — অনুনাসিক শব্দ", "ـنـ" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "সন্তান", "ـو", "و", false, "و", "উভয় ঠোঁট গোলাকারভাবে সামান্য ফাঁক রেখে — অর্ধ-স্বরধ্বনি", "ـو" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescription", "MakhrajDescriptionBn", "MedialForm", "NameBangla" },
                values: new object[] { "পথনির্দেশনা", "ـه", "هـ", true, "ه", "Deepest part of the throat — soft, breathy voiceless glottal fricative", "কণ্ঠের সর্বনিম্ন স্থান — শীতল নিঃশ্বাসের মতো শব্দ", "ـهـ", "হা (গোল হা)" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "ExampleWordBn", "FinalForm", "InitialForm", "IsConnector", "IsolatedForm", "MakhrajDescription", "MakhrajDescriptionBn", "MedialForm" },
                values: new object[] { "দিন", "ـي", "يـ", true, "ي", "Middle of tongue rises toward the hard palate — palatal semi-vowel", "জিহ্বার মধ্যভাগ শক্ত তালুর দিকে উঠিয়ে — তালব্য অর্ধ-স্বর", "ـيـ" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExampleWordBn",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "FinalForm",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "InitialForm",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "IsConnector",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "IsolatedForm",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "MakhrajDescriptionBn",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "MedialForm",
                table: "ArabicLetters");

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 7,
                column: "MakhrajDescription",
                value: "Upper throat (closest to mouth) — velar fricative, like a raspy 'kh'");

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 9,
                column: "MakhrajDescription",
                value: "Tip of tongue lightly between the upper and lower teeth (voiced)");

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 10,
                column: "MakhrajDescription",
                value: "Tip of tongue near the upper gum ridge — a trilled or tapped 'r'");

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 11,
                column: "MakhrajDescription",
                value: "Tip of tongue near the lower front teeth — voiced sibilant");

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 12,
                column: "MakhrajDescription",
                value: "Tip of tongue near the lower front teeth — voiceless sibilant");

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 13,
                column: "MakhrajDescription",
                value: "Middle of the tongue spread toward the hard palate — 'sh' sound");

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 14,
                column: "MakhrajDescription",
                value: "Tip of tongue near front teeth — heavy emphatic 'S' with tongue raised");

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 15,
                column: "MakhrajDescription",
                value: "One or both sides of the tongue against the upper back molars");

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 18,
                column: "MakhrajDescription",
                value: "Middle of the throat — voiced pharyngeal fricative, a unique Arabic sound");

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 20,
                column: "MakhrajDescription",
                value: "Inner edge of the lower lip touches the tips of the upper front teeth");

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 22,
                column: "NameBangla",
                value: "কাফ");

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 23,
                column: "MakhrajDescription",
                value: "Tip and sides of tongue along the upper gum ridge — lateral sound");

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "MakhrajDescription", "NameBangla" },
                values: new object[] { "Deepest part of the throat — a soft, breathy voiceless glottal fricative", "হা" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 28,
                column: "MakhrajDescription",
                value: "Middle of the tongue rises toward the hard palate — palatal semi-vowel");
        }
    }
}
