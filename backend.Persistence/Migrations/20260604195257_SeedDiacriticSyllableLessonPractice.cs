using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedDiacriticSyllableLessonPractice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DiacriticSigns",
                columns: new[] { "Id", "SignGroup", "Symbol", "NameBn", "NameEn" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "Harakat", "َ", "ফাতহা", "Fatha" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "Harakat", "ِ", "কাসরা", "Kasra" },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "Harakat", "ُ", "দাম্মা", "Damma" },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "Harakat", "ْ", "সুকুন", "Sukun" },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "Shaddah", "ّ", "শাদ্দাহ", "Shaddah" },
                    { new Guid("10000000-0000-0000-0000-000000000006"), "Tanween", "ً", "তানউইন ফাতহ", "Tanween Fath" },
                    { new Guid("10000000-0000-0000-0000-000000000007"), "Tanween", "ٍ", "তানউইন কাসর", "Tanween Kasr" },
                    { new Guid("10000000-0000-0000-0000-000000000008"), "Tanween", "ٌ", "তানউইন দাম্ম", "Tanween Damm" }
                });

            migrationBuilder.InsertData(
                table: "Lessons",
                columns: new[] { "Id", "SequenceOrder", "TitleBn", "TitleEn" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), 1, "বা-এর হরকত অনুশীলন", "Ba with Harakat" },
                    { new Guid("30000000-0000-0000-0000-000000000002"), 2, "তা-এর হরকত অনুশীলন", "Ta with Harakat" },
                    { new Guid("30000000-0000-0000-0000-000000000003"), 3, "বা ফাতহা বনাম বা কাসরা তুলনা", "Ba vs Ba — Fatha vs Kasra" }
                });

            migrationBuilder.InsertData(
                table: "SyllableSounds",
                columns: new[] { "Id", "AudioUrl", "CombinedCharacter", "LetterId", "SignId", "TransliterationBn", "TransliterationEn" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), "", "بَ", new Guid("00000000-0000-0000-0000-000000000002"), new Guid("10000000-0000-0000-0000-000000000001"), "বা", "Ba" },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "", "بِ", new Guid("00000000-0000-0000-0000-000000000002"), new Guid("10000000-0000-0000-0000-000000000002"), "বি", "Bi" },
                    { new Guid("20000000-0000-0000-0000-000000000003"), "", "بُ", new Guid("00000000-0000-0000-0000-000000000002"), new Guid("10000000-0000-0000-0000-000000000003"), "বু", "Bu" },
                    { new Guid("20000000-0000-0000-0000-000000000004"), "", "تَ", new Guid("00000000-0000-0000-0000-000000000003"), new Guid("10000000-0000-0000-0000-000000000001"), "তা", "Ta" },
                    { new Guid("20000000-0000-0000-0000-000000000005"), "", "تِ", new Guid("00000000-0000-0000-0000-000000000003"), new Guid("10000000-0000-0000-0000-000000000002"), "তি", "Ti" },
                    { new Guid("20000000-0000-0000-0000-000000000006"), "", "تُ", new Guid("00000000-0000-0000-0000-000000000003"), new Guid("10000000-0000-0000-0000-000000000003"), "তু", "Tu" }
                });

            migrationBuilder.InsertData(
                table: "PracticeItems",
                columns: new[] { "Id", "CompareWithSyllableId", "LessonId", "TargetSyllableId", "InstructionBn", "InstructionEn", "SuccessTipBn", "SuccessTipEn" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000001"), null, new Guid("30000000-0000-0000-0000-000000000001"), new Guid("20000000-0000-0000-0000-000000000001"), "বর্ণটি ফাতহা দিয়ে উচ্চারণ করুন", "Pronounce the letter with Fatha", "'আ' শব্দের জন্য মুখ সামান্য খুলুন", "Open your mouth slightly for 'a' sound" },
                    { new Guid("40000000-0000-0000-0000-000000000002"), null, new Guid("30000000-0000-0000-0000-000000000001"), new Guid("20000000-0000-0000-0000-000000000002"), "বর্ণটি কাসরা দিয়ে উচ্চারণ করুন", "Pronounce the letter with Kasra", "'ই' শব্দের জন্য ঠোঁট ও জিহ্বা নিচে রাখুন", "Press lips and tongue down for 'i' sound" },
                    { new Guid("40000000-0000-0000-0000-000000000003"), null, new Guid("30000000-0000-0000-0000-000000000001"), new Guid("20000000-0000-0000-0000-000000000003"), "বর্ণটি দাম্মা দিয়ে উচ্চারণ করুন", "Pronounce the letter with Damma", "'উ' শব্দের জন্য ঠোঁট সামান্য গোলাকার করুন", "Round your lips slightly for 'u' sound" },
                    { new Guid("40000000-0000-0000-0000-000000000004"), null, new Guid("30000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000004"), "বর্ণটি ফাতহা দিয়ে উচ্চারণ করুন", "Pronounce the letter with Fatha", "'আ' শব্দের জন্য মুখ সামান্য খুলুন", "Open your mouth slightly for 'a' sound" },
                    { new Guid("40000000-0000-0000-0000-000000000005"), null, new Guid("30000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000005"), "বর্ণটি কাসরা দিয়ে উচ্চারণ করুন", "Pronounce the letter with Kasra", "'ই' শব্দের জন্য ঠোঁট ও জিহ্বা নিচে রাখুন", "Press lips and tongue down for 'i' sound" },
                    { new Guid("40000000-0000-0000-0000-000000000006"), null, new Guid("30000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000006"), "বর্ণটি দাম্মা দিয়ে উচ্চারণ করুন", "Pronounce the letter with Damma", "'উ' শব্দের জন্য ঠোঁট সামান্য গোলাকার করুন", "Round your lips slightly for 'u' sound" },
                    { new Guid("40000000-0000-0000-0000-000000000007"), new Guid("20000000-0000-0000-0000-000000000002"), new Guid("30000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000001"), "পার্থক্য লক্ষ্য করুন: বা বনাম বি", "Notice the difference: Ba vs Bi", "ফাতহা খোলা 'আ', কাসরা বন্ধ 'ই'", "Fatha is open 'a', Kasra is closed 'i'" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DiacriticSigns",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "DiacriticSigns",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "DiacriticSigns",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "DiacriticSigns",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "DiacriticSigns",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "PracticeItems",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "PracticeItems",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "PracticeItems",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "PracticeItems",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "PracticeItems",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "PracticeItems",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "PracticeItems",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "SyllableSounds",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "SyllableSounds",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "SyllableSounds",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "SyllableSounds",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "SyllableSounds",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "SyllableSounds",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "DiacriticSigns",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "DiacriticSigns",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "DiacriticSigns",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"));
        }
    }
}
