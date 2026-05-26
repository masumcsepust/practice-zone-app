using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Phase4_TajweedRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TajweedRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RuleType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ArabicName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TajweedRules", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TajweedRules",
                columns: new[] { "Id", "ArabicName", "Description", "RuleType" },
                values: new object[,]
                {
                    { 1, "مد", "Elongation of vowel letters ا و ي for a minimum of 2 harakats.", "Madd" },
                    { 2, "غنة", "Nasalization of ن or م with shaddah, held for 2 harakats.", "Ghunnah" },
                    { 3, "قلقلة", "Echoing bounce on ق ط ب ج د when carrying sukoon or at word end.", "Qalqalah" },
                    { 4, "إخفاء", "Partial concealment of noon sakinah/tanween before 15 letters.", "Ikhfa" },
                    { 5, "إدغام", "Merging of noon sakinah/tanween into the following يرملون letter.", "Idgham" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TajweedRules");
        }
    }
}
