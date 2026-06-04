using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonConceptColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionBn",
                table: "LessonCategories",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "LessonCategories",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleBn",
                table: "LessonCategories",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "LessonCategories",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "LessonCategories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DescriptionBn", "DescriptionEn", "TitleBn", "TitleEn" },
                values: new object[] { "আরবিতে ২৮টি বর্ণ আছে। শব্দের মধ্যে অবস্থানের উপর ভিত্তি করে প্রতিটি বর্ণের আকার পরিবর্তন হয়।", "Arabic has 28 letters. Each letter changes shape based on its position in a word.", "আরবি বর্ণমালা", "Arabic Alphabet" });

            migrationBuilder.UpdateData(
                table: "LessonCategories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DescriptionBn", "DescriptionEn", "TitleBn", "TitleEn" },
                values: new object[] { "হরকত হলো ছোট স্বরচিহ্ন যা নির্ধারণ করে কীভাবে একটি বর্ণ উচ্চারণ করতে হবে।", "Harakat are short vowel marks that determine how a letter is pronounced.", "হরকত কী?", "What is Harakat?" });

            migrationBuilder.UpdateData(
                table: "LessonCategories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DescriptionBn", "DescriptionEn", "TitleBn", "TitleEn" },
                values: new object[] { "তানউইন হলো শব্দের শেষে একটি 'ন' শব্দ যোগ করা।", "Tanween is a 'n' sound added at the end of a word.", "তানউইন কী?", "What is Tanween?" });

            migrationBuilder.UpdateData(
                table: "LessonCategories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DescriptionBn", "DescriptionEn", "TitleBn", "TitleEn" },
                values: new object[] { "সুকুন মানে একটি বর্ণের কোনো স্বর নেই। এটি বর্ণের উপরে একটি ছোট বৃত্ত হিসেবে দেখা যায়।", "Sukoon means a letter has no vowel. It appears as a small circle above the letter.", "সুকুন কী?", "What is Sukoon?" });

            migrationBuilder.UpdateData(
                table: "LessonCategories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DescriptionBn", "DescriptionEn", "TitleBn", "TitleEn" },
                values: new object[] { "শাদ্দা মানে বর্ণটি উচ্চারণে দ্বিগুণ হয়।", "Shaddah means the consonant is doubled in pronunciation.", "শাদ্দা কী?", "What is Shaddah?" });

            migrationBuilder.UpdateData(
                table: "LessonCategories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DescriptionBn", "DescriptionEn", "TitleBn", "TitleEn" },
                values: new object[] { "দৈনন্দিন নামাযে ব্যবহৃত সাধারণ আরবি শব্দ পড়তে ও উচ্চারণ করতে শিখুন।", "Learn to read and pronounce common Arabic words used in daily prayers.", "শব্দ গঠন", "Word Building" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionBn",
                table: "LessonCategories");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "LessonCategories");

            migrationBuilder.DropColumn(
                name: "TitleBn",
                table: "LessonCategories");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "LessonCategories");
        }
    }
}
