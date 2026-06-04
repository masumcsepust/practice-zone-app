using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TajweedImplement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionBn",
                table: "LessonItems",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "LessonItems",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "আলিফ — আরবি বর্ণমালার ১ম বর্ণ", "Alif — first letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "বা — আরবি বর্ণমালার ২য় বর্ণ", "Ba — second letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "তা — আরবি বর্ণমালার ৩য় বর্ণ", "Ta — third letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "ছা — আরবি বর্ণমালার ৪র্থ বর্ণ", "Tha — fourth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "জিম — আরবি বর্ণমালার ৫ম বর্ণ", "Jim — fifth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "হা — আরবি বর্ণমালার ৬ষ্ঠ বর্ণ", "Ha — sixth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "খা — আরবি বর্ণমালার ৭ম বর্ণ", "Kha — seventh letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "দাল — আরবি বর্ণমালার ৮ম বর্ণ", "Dal — eighth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "যাল — আরবি বর্ণমালার ৯ম বর্ণ", "Dhal — ninth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "রা — আরবি বর্ণমালার ১০ম বর্ণ", "Ra — tenth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "যায় — আরবি বর্ণমালার ১১তম বর্ণ", "Zay — eleventh letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "সিন — আরবি বর্ণমালার ১২তম বর্ণ", "Sin — twelfth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "শিন — আরবি বর্ণমালার ১৩তম বর্ণ", "Shin — thirteenth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "সাদ — আরবি বর্ণমালার ১৪তম বর্ণ", "Sad — fourteenth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "দোয়াদ — আরবি বর্ণমালার ১৫তম বর্ণ", "Dad — fifteenth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "তোয়া — আরবি বর্ণমালার ১৬তম বর্ণ", "Taa — sixteenth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "যোয়া — আরবি বর্ণমালার ১৭তম বর্ণ", "Dhaa — seventeenth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "আইন — আরবি বর্ণমালার ১৮তম বর্ণ", "Ayn — eighteenth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "গাইন — আরবি বর্ণমালার ১৯তম বর্ণ", "Ghayn — nineteenth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "ফা — আরবি বর্ণমালার ২০তম বর্ণ", "Fa — twentieth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "কাফ — আরবি বর্ণমালার ২১তম বর্ণ", "Qaf — twenty-first letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "কাফ — আরবি বর্ণমালার ২২তম বর্ণ", "Kaf — twenty-second letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "লাম — আরবি বর্ণমালার ২৩তম বর্ণ", "Lam — twenty-third letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "মিম — আরবি বর্ণমালার ২৪তম বর্ণ", "Mim — twenty-fourth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "নুন — আরবি বর্ণমালার ২৫তম বর্ণ", "Nun — twenty-fifth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "ওয়াও — আরবি বর্ণমালার ২৬তম বর্ণ", "Waw — twenty-sixth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "হা — আরবি বর্ণমালার ২৭তম বর্ণ", "Ha — twenty-seventh letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "ইয়া — আরবি বর্ণমালার ২৮তম বর্ণ", "Ya — twenty-eighth letter of the Arabic alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Aa — আলিফ সহ ফাতহা (ছোট আ)", "Aa — Alif with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ii — আলিফ সহ কাসরা (ছোট ই)", "Ii — Alif with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Uu — আলিফ সহ দাম্মা (ছোট উ)", "Uu — Alif with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ba — বা সহ ফাতহা (ছোট আ)", "Ba — Ba with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Bi — বা সহ কাসরা (ছোট ই)", "Bi — Ba with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Bu — বা সহ দাম্মা (ছোট উ)", "Bu — Ba with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ta — তা সহ ফাতহা (ছোট আ)", "Ta — Ta with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ti — তা সহ কাসরা (ছোট ই)", "Ti — Ta with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Tu — তা সহ দাম্মা (ছোট উ)", "Tu — Ta with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Tha — ছা সহ ফাতহা (ছোট আ)", "Tha — Tha with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Thi — ছা সহ কাসরা (ছোট ই)", "Thi — Tha with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Thu — ছা সহ দাম্মা (ছোট উ)", "Thu — Tha with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ja — জিম সহ ফাতহা (ছোট আ)", "Ja — Jim with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ji — জিম সহ কাসরা (ছোট ই)", "Ji — Jim with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ju — জিম সহ দাম্মা (ছোট উ)", "Ju — Jim with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ha — হা সহ ফাতহা (ছোট আ)", "Ha — Ha with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Hi — হা সহ কাসরা (ছোট ই)", "Hi — Ha with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Hu — হা সহ দাম্মা (ছোট উ)", "Hu — Ha with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Kha — খা সহ ফাতহা (ছোট আ)", "Kha — Kha with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Khi — খা সহ কাসরা (ছোট ই)", "Khi — Kha with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Khu — খা সহ দাম্মা (ছোট উ)", "Khu — Kha with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Da — দাল সহ ফাতহা (ছোট আ)", "Da — Dal with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Di — দাল সহ কাসরা (ছোট ই)", "Di — Dal with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Du — দাল সহ দাম্মা (ছোট উ)", "Du — Dal with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dha — যাল সহ ফাতহা (ছোট আ)", "Dha — Dhal with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dhi — যাল সহ কাসরা (ছোট ই)", "Dhi — Dhal with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dhu — যাল সহ দাম্মা (ছোট উ)", "Dhu — Dhal with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ra — রা সহ ফাতহা (ছোট আ)", "Ra — Ra with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ri — রা সহ কাসরা (ছোট ই)", "Ri — Ra with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ru — রা সহ দাম্মা (ছোট উ)", "Ru — Ra with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Za — যায় সহ ফাতহা (ছোট আ)", "Za — Zay with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Zi — যায় সহ কাসরা (ছোট ই)", "Zi — Zay with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Zu — যায় সহ দাম্মা (ছোট উ)", "Zu — Zay with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Sa — সিন সহ ফাতহা (ছোট আ)", "Sa — Sin with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Si — সিন সহ কাসরা (ছোট ই)", "Si — Sin with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Su — সিন সহ দাম্মা (ছোট উ)", "Su — Sin with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Sha — শিন সহ ফাতহা (ছোট আ)", "Sha — Shin with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Shi — শিন সহ কাসরা (ছোট ই)", "Shi — Shin with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Shu — শিন সহ দাম্মা (ছোট উ)", "Shu — Shin with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Sa — সাদ সহ ফাতহা (ছোট আ)", "Sa — Sad with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Si — সাদ সহ কাসরা (ছোট ই)", "Si — Sad with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Su — সাদ সহ দাম্মা (ছোট উ)", "Su — Sad with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Da — দোয়াদ সহ ফাতহা (ছোট আ)", "Da — Dad with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Di — দোয়াদ সহ কাসরা (ছোট ই)", "Di — Dad with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Du — দোয়াদ সহ দাম্মা (ছোট উ)", "Du — Dad with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ta — তোয়া সহ ফাতহা (ছোট আ)", "Ta — Taa with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ti — তোয়া সহ কাসরা (ছোট ই)", "Ti — Taa with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Tu — তোয়া সহ দাম্মা (ছোট উ)", "Tu — Taa with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dha — যোয়া সহ ফাতহা (ছোট আ)", "Dha — Dhaa with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dhi — যোয়া সহ কাসরা (ছোট ই)", "Dhi — Dhaa with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dhu — যোয়া সহ দাম্মা (ছোট উ)", "Dhu — Dhaa with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "'Aa — আইন সহ ফাতহা (ছোট আ)", "'Aa — Ayn with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "'Ii — আইন সহ কাসরা (ছোট ই)", "'Ii — Ayn with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "'Uu — আইন সহ দাম্মা (ছোট উ)", "'Uu — Ayn with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Gha — গাইন সহ ফাতহা (ছোট আ)", "Gha — Ghayn with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ghi — গাইন সহ কাসরা (ছোট ই)", "Ghi — Ghayn with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 85,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ghu — গাইন সহ দাম্মা (ছোট উ)", "Ghu — Ghayn with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Fa — ফা সহ ফাতহা (ছোট আ)", "Fa — Fa with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Fi — ফা সহ কাসরা (ছোট ই)", "Fi — Fa with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Fu — ফা সহ দাম্মা (ছোট উ)", "Fu — Fa with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Qa — কাফ সহ ফাতহা (ছোট আ)", "Qa — Qaf with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Qi — কাফ সহ কাসরা (ছোট ই)", "Qi — Qaf with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Qu — কাফ সহ দাম্মা (ছোট উ)", "Qu — Qaf with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ka — কাফ সহ ফাতহা (ছোট আ)", "Ka — Kaf with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ki — কাফ সহ কাসরা (ছোট ই)", "Ki — Kaf with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ku — কাফ সহ দাম্মা (ছোট উ)", "Ku — Kaf with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "La — লাম সহ ফাতহা (ছোট আ)", "La — Lam with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 96,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Li — লাম সহ কাসরা (ছোট ই)", "Li — Lam with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 97,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Lu — লাম সহ দাম্মা (ছোট উ)", "Lu — Lam with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 98,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ma — মিম সহ ফাতহা (ছোট আ)", "Ma — Mim with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Mi — মিম সহ কাসরা (ছোট ই)", "Mi — Mim with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Mu — মিম সহ দাম্মা (ছোট উ)", "Mu — Mim with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Na — নুন সহ ফাতহা (ছোট আ)", "Na — Nun with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ni — নুন সহ কাসরা (ছোট ই)", "Ni — Nun with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Nu — নুন সহ দাম্মা (ছোট উ)", "Nu — Nun with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Wa — ওয়াও সহ ফাতহা (ছোট আ)", "Wa — Waw with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Wi — ওয়াও সহ কাসরা (ছোট ই)", "Wi — Waw with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Wu — ওয়াও সহ দাম্মা (ছোট উ)", "Wu — Waw with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 107,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ha — হা সহ ফাতহা (ছোট আ)", "Ha — Ha with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 108,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Hi — হা সহ কাসরা (ছোট ই)", "Hi — Ha with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 109,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Hu — হা সহ দাম্মা (ছোট উ)", "Hu — Ha with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 110,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ya — ইয়া সহ ফাতহা (ছোট আ)", "Ya — Ya with Fatha (short 'a')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 111,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Yi — ইয়া সহ কাসরা (ছোট ই)", "Yi — Ya with Kasra (short 'i')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 112,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Yu — ইয়া সহ দাম্মা (ছোট উ)", "Yu — Ya with Damma (short 'u')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 113,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Aan — আলিফ সহ তানউইন ফাতহ (-আন)", "Aan — Alif with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 114,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Iin — আলিফ সহ তানউইন কাসর (-ইন)", "Iin — Alif with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 115,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Uun — আলিফ সহ তানউইন দাম্ম (-উন)", "Uun — Alif with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 116,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ban — বা সহ তানউইন ফাতহ (-আন)", "Ban — Ba with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 117,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Bin — বা সহ তানউইন কাসর (-ইন)", "Bin — Ba with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 118,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Bun — বা সহ তানউইন দাম্ম (-উন)", "Bun — Ba with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 119,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Tan — তা সহ তানউইন ফাতহ (-আন)", "Tan — Ta with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 120,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Tin — তা সহ তানউইন কাসর (-ইন)", "Tin — Ta with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 121,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Tun — তা সহ তানউইন দাম্ম (-উন)", "Tun — Ta with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 122,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Than — ছা সহ তানউইন ফাতহ (-আন)", "Than — Tha with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 123,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Thin — ছা সহ তানউইন কাসর (-ইন)", "Thin — Tha with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 124,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Thun — ছা সহ তানউইন দাম্ম (-উন)", "Thun — Tha with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 125,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Jan — জিম সহ তানউইন ফাতহ (-আন)", "Jan — Jim with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 126,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Jin — জিম সহ তানউইন কাসর (-ইন)", "Jin — Jim with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 127,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Jun — জিম সহ তানউইন দাম্ম (-উন)", "Jun — Jim with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 128,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Han — হা সহ তানউইন ফাতহ (-আন)", "Han — Ha with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 129,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Hin — হা সহ তানউইন কাসর (-ইন)", "Hin — Ha with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 130,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Hun — হা সহ তানউইন দাম্ম (-উন)", "Hun — Ha with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 131,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Khan — খা সহ তানউইন ফাতহ (-আন)", "Khan — Kha with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 132,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Khin — খা সহ তানউইন কাসর (-ইন)", "Khin — Kha with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 133,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Khun — খা সহ তানউইন দাম্ম (-উন)", "Khun — Kha with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 134,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dan — দাল সহ তানউইন ফাতহ (-আন)", "Dan — Dal with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 135,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Din — দাল সহ তানউইন কাসর (-ইন)", "Din — Dal with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 136,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dun — দাল সহ তানউইন দাম্ম (-উন)", "Dun — Dal with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 137,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dhan — যাল সহ তানউইন ফাতহ (-আন)", "Dhan — Dhal with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 138,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dhin — যাল সহ তানউইন কাসর (-ইন)", "Dhin — Dhal with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 139,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dhun — যাল সহ তানউইন দাম্ম (-উন)", "Dhun — Dhal with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 140,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ran — রা সহ তানউইন ফাতহ (-আন)", "Ran — Ra with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 141,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Rin — রা সহ তানউইন কাসর (-ইন)", "Rin — Ra with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 142,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Run — রা সহ তানউইন দাম্ম (-উন)", "Run — Ra with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 143,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Zan — যায় সহ তানউইন ফাতহ (-আন)", "Zan — Zay with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 144,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Zin — যায় সহ তানউইন কাসর (-ইন)", "Zin — Zay with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 145,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Zun — যায় সহ তানউইন দাম্ম (-উন)", "Zun — Zay with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 146,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "San — সিন সহ তানউইন ফাতহ (-আন)", "San — Sin with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 147,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Sin — সিন সহ তানউইন কাসর (-ইন)", "Sin — Sin with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 148,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Sun — সিন সহ তানউইন দাম্ম (-উন)", "Sun — Sin with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 149,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Shan — শিন সহ তানউইন ফাতহ (-আন)", "Shan — Shin with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 150,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Shin — শিন সহ তানউইন কাসর (-ইন)", "Shin — Shin with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 151,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Shun — শিন সহ তানউইন দাম্ম (-উন)", "Shun — Shin with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 152,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "San — সাদ সহ তানউইন ফাতহ (-আন)", "San — Sad with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 153,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Sin — সাদ সহ তানউইন কাসর (-ইন)", "Sin — Sad with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 154,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Sun — সাদ সহ তানউইন দাম্ম (-উন)", "Sun — Sad with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 155,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dan — দোয়াদ সহ তানউইন ফাতহ (-আন)", "Dan — Dad with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 156,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Din — দোয়াদ সহ তানউইন কাসর (-ইন)", "Din — Dad with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 157,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dun — দোয়াদ সহ তানউইন দাম্ম (-উন)", "Dun — Dad with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 158,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Tan — তোয়া সহ তানউইন ফাতহ (-আন)", "Tan — Taa with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 159,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Tin — তোয়া সহ তানউইন কাসর (-ইন)", "Tin — Taa with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 160,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Tun — তোয়া সহ তানউইন দাম্ম (-উন)", "Tun — Taa with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 161,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dhan — যোয়া সহ তানউইন ফাতহ (-আন)", "Dhan — Dhaa with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 162,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dhin — যোয়া সহ তানউইন কাসর (-ইন)", "Dhin — Dhaa with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 163,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dhun — যোয়া সহ তানউইন দাম্ম (-উন)", "Dhun — Dhaa with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 164,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "'Aan — আইন সহ তানউইন ফাতহ (-আন)", "'Aan — Ayn with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 165,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "'Iin — আইন সহ তানউইন কাসর (-ইন)", "'Iin — Ayn with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 166,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "'Uun — আইন সহ তানউইন দাম্ম (-উন)", "'Uun — Ayn with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 167,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ghan — গাইন সহ তানউইন ফাতহ (-আন)", "Ghan — Ghayn with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 168,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ghin — গাইন সহ তানউইন কাসর (-ইন)", "Ghin — Ghayn with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 169,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Ghun — গাইন সহ তানউইন দাম্ম (-উন)", "Ghun — Ghayn with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 170,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Fan — ফা সহ তানউইন ফাতহ (-আন)", "Fan — Fa with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 171,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Fin — ফা সহ তানউইন কাসর (-ইন)", "Fin — Fa with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 172,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Fun — ফা সহ তানউইন দাম্ম (-উন)", "Fun — Fa with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 173,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Qan — কাফ সহ তানউইন ফাতহ (-আন)", "Qan — Qaf with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 174,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Qin — কাফ সহ তানউইন কাসর (-ইন)", "Qin — Qaf with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 175,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Qun — কাফ সহ তানউইন দাম্ম (-উন)", "Qun — Qaf with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 176,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Kan — কাফ সহ তানউইন ফাতহ (-আন)", "Kan — Kaf with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 177,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Kin — কাফ সহ তানউইন কাসর (-ইন)", "Kin — Kaf with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 178,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Kun — কাফ সহ তানউইন দাম্ম (-উন)", "Kun — Kaf with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 179,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Lan — লাম সহ তানউইন ফাতহ (-আন)", "Lan — Lam with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 180,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Lin — লাম সহ তানউইন কাসর (-ইন)", "Lin — Lam with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 181,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Lun — লাম সহ তানউইন দাম্ম (-উন)", "Lun — Lam with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 182,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Man — মিম সহ তানউইন ফাতহ (-আন)", "Man — Mim with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 183,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Min — মিম সহ তানউইন কাসর (-ইন)", "Min — Mim with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 184,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Mun — মিম সহ তানউইন দাম্ম (-উন)", "Mun — Mim with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 185,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Nan — নুন সহ তানউইন ফাতহ (-আন)", "Nan — Nun with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 186,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Nin — নুন সহ তানউইন কাসর (-ইন)", "Nin — Nun with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 187,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Nun — নুন সহ তানউইন দাম্ম (-উন)", "Nun — Nun with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 188,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Wan — ওয়াও সহ তানউইন ফাতহ (-আন)", "Wan — Waw with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 189,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Win — ওয়াও সহ তানউইন কাসর (-ইন)", "Win — Waw with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 190,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Wun — ওয়াও সহ তানউইন দাম্ম (-উন)", "Wun — Waw with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 191,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Han — হা সহ তানউইন ফাতহ (-আন)", "Han — Ha with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 192,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Hin — হা সহ তানউইন কাসর (-ইন)", "Hin — Ha with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 193,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Hun — হা সহ তানউইন দাম্ম (-উন)", "Hun — Ha with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 194,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Yan — ইয়া সহ তানউইন ফাতহ (-আন)", "Yan — Ya with Tanween Fath (-an)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 195,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Yin — ইয়া সহ তানউইন কাসর (-ইন)", "Yin — Ya with Tanween Kasr (-in)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 196,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Yun — ইয়া সহ তানউইন দাম্ম (-উন)", "Yun — Ya with Tanween Damm (-un)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 197,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "A — আলিফ সহ সুকুন (কোনো স্বর নেই)", "A — Alif with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 198,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "B — বা সহ সুকুন (কোনো স্বর নেই)", "B — Ba with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 199,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "T — তা সহ সুকুন (কোনো স্বর নেই)", "T — Ta with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 200,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Th — ছা সহ সুকুন (কোনো স্বর নেই)", "Th — Tha with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 201,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "J — জিম সহ সুকুন (কোনো স্বর নেই)", "J — Jim with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 202,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "H — হা সহ সুকুন (কোনো স্বর নেই)", "H — Ha with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 203,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Kh — খা সহ সুকুন (কোনো স্বর নেই)", "Kh — Kha with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 204,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "D — দাল সহ সুকুন (কোনো স্বর নেই)", "D — Dal with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 205,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dh — যাল সহ সুকুন (কোনো স্বর নেই)", "Dh — Dhal with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 206,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "R — রা সহ সুকুন (কোনো স্বর নেই)", "R — Ra with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 207,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Z — যায় সহ সুকুন (কোনো স্বর নেই)", "Z — Zay with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 208,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "S — সিন সহ সুকুন (কোনো স্বর নেই)", "S — Sin with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 209,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Sh — শিন সহ সুকুন (কোনো স্বর নেই)", "Sh — Shin with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 210,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "S — সাদ সহ সুকুন (কোনো স্বর নেই)", "S — Sad with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 211,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "D — দোয়াদ সহ সুকুন (কোনো স্বর নেই)", "D — Dad with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 212,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "T — তোয়া সহ সুকুন (কোনো স্বর নেই)", "T — Taa with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 213,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Dh — যোয়া সহ সুকুন (কোনো স্বর নেই)", "Dh — Dhaa with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 214,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "' — আইন সহ সুকুন (কোনো স্বর নেই)", "' — Ayn with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 215,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Gh — গাইন সহ সুকুন (কোনো স্বর নেই)", "Gh — Ghayn with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 216,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "F — ফা সহ সুকুন (কোনো স্বর নেই)", "F — Fa with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 217,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Q — কাফ সহ সুকুন (কোনো স্বর নেই)", "Q — Qaf with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 218,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "K — কাফ সহ সুকুন (কোনো স্বর নেই)", "K — Kaf with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 219,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "L — লাম সহ সুকুন (কোনো স্বর নেই)", "L — Lam with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 220,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "M — মিম সহ সুকুন (কোনো স্বর নেই)", "M — Mim with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 221,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "N — নুন সহ সুকুন (কোনো স্বর নেই)", "N — Nun with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 222,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "W — ওয়াও সহ সুকুন (কোনো স্বর নেই)", "W — Waw with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 223,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "H — হা সহ সুকুন (কোনো স্বর নেই)", "H — Ha with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 224,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "Y — ইয়া সহ সুকুন (কোনো স্বর নেই)", "Y — Ya with Sukoon (no vowel)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 225,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "AA — আলিফ সহ শাদ্দা (দ্বিগুণ)", "AA — Alif with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 226,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "BB — বা সহ শাদ্দা (দ্বিগুণ)", "BB — Ba with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 227,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "TT — তা সহ শাদ্দা (দ্বিগুণ)", "TT — Ta with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 228,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "ThTh — ছা সহ শাদ্দা (দ্বিগুণ)", "ThTh — Tha with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 229,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "JJ — জিম সহ শাদ্দা (দ্বিগুণ)", "JJ — Jim with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 230,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "HH — হা সহ শাদ্দা (দ্বিগুণ)", "HH — Ha with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 231,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "KhKh — খা সহ শাদ্দা (দ্বিগুণ)", "KhKh — Kha with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 232,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "DD — দাল সহ শাদ্দা (দ্বিগুণ)", "DD — Dal with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 233,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "DhDh — যাল সহ শাদ্দা (দ্বিগুণ)", "DhDh — Dhal with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 234,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "RR — রা সহ শাদ্দা (দ্বিগুণ)", "RR — Ra with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 235,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "ZZ — যায় সহ শাদ্দা (দ্বিগুণ)", "ZZ — Zay with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 236,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "SS — সিন সহ শাদ্দা (দ্বিগুণ)", "SS — Sin with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 237,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "ShSh — শিন সহ শাদ্দা (দ্বিগুণ)", "ShSh — Shin with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 238,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "SS — সাদ সহ শাদ্দা (দ্বিগুণ)", "SS — Sad with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 239,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "DD — দোয়াদ সহ শাদ্দা (দ্বিগুণ)", "DD — Dad with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 240,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "TT — তোয়া সহ শাদ্দা (দ্বিগুণ)", "TT — Taa with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 241,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "DhDh — যোয়া সহ শাদ্দা (দ্বিগুণ)", "DhDh — Dhaa with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 242,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "'' — আইন সহ শাদ্দা (দ্বিগুণ)", "'' — Ayn with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 243,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "GhGh — গাইন সহ শাদ্দা (দ্বিগুণ)", "GhGh — Ghayn with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 244,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "FF — ফা সহ শাদ্দা (দ্বিগুণ)", "FF — Fa with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 245,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "QQ — কাফ সহ শাদ্দা (দ্বিগুণ)", "QQ — Qaf with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 246,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "KK — কাফ সহ শাদ্দা (দ্বিগুণ)", "KK — Kaf with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 247,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "LL — লাম সহ শাদ্দা (দ্বিগুণ)", "LL — Lam with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 248,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "MM — মিম সহ শাদ্দা (দ্বিগুণ)", "MM — Mim with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 249,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "NN — নুন সহ শাদ্দা (দ্বিগুণ)", "NN — Nun with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 250,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "WW — ওয়াও সহ শাদ্দা (দ্বিগুণ)", "WW — Waw with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 251,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "HH — হা সহ শাদ্দা (দ্বিগুণ)", "HH — Ha with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 252,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "YY — ইয়া সহ শাদ্দা (দ্বিগুণ)", "YY — Ya with Shaddah (doubled)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 253,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "সাধারণ আরবি শব্দ — বাড়ি", "Common Arabic word — House (Bayt)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 254,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "সাধারণ আরবি শব্দ — বই", "Common Arabic word — Book (Kitab)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 255,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "সাধারণ আরবি শব্দ — কলম", "Common Arabic word — Pen (Qalam)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 256,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "সাধারণ আরবি শব্দ — পানি", "Common Arabic word — Water (Maa')" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 257,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "সাধারণ আরবি শব্দ — আলো", "Common Arabic word — Light (Nur)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 258,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "সাধারণ আরবি শব্দ — মা", "Common Arabic word — Mother (Umm)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 259,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "সাধারণ আরবি শব্দ — বাবা", "Common Arabic word — Father (Ab)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 260,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "সাধারণ আরবি শব্দ — সূর্য", "Common Arabic word — Sun (Shams)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 261,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "সাধারণ আরবি শব্দ — চাঁদ", "Common Arabic word — Moon (Qamar)" });

            migrationBuilder.UpdateData(
                table: "LessonItems",
                keyColumn: "Id",
                keyValue: 262,
                columns: new[] { "DescriptionBn", "DescriptionEn" },
                values: new object[] { "সাধারণ আরবি শব্দ — রহমত", "Common Arabic word — Mercy (Rahma)" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionBn",
                table: "LessonItems");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "LessonItems");
        }
    }
}
