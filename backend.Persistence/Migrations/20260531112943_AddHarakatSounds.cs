using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHarakatSounds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DammaSound",
                table: "ArabicLetters",
                type: "character varying(20)",
                maxLength: 20,
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
                name: "KasraSound",
                table: "ArabicLetters",
                type: "character varying(20)",
                maxLength: 20,
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
                name: "TanweenFathSound",
                table: "ArabicLetters",
                type: "character varying(20)",
                maxLength: 20,
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
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Uu", "Aa", "Ii", "Uun", "Aan", "Iin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Bu", "Ba", "Bi", "Bun", "Ban", "Bin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Tu", "Ta", "Ti", "Tun", "Tan", "Tin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Thu", "Tha", "Thi", "Thun", "Than", "Thin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Ju", "Ja", "Ji", "Jun", "Jan", "Jin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Hu", "Ha", "Hi", "Hun", "Han", "Hin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Khu", "Kha", "Khi", "Khun", "Khan", "Khin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Du", "Da", "Di", "Dun", "Dan", "Din" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Dhu", "Dha", "Dhi", "Dhun", "Dhan", "Dhin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Ru", "Ra", "Ri", "Run", "Ran", "Rin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Zu", "Za", "Zi", "Zun", "Zan", "Zin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Su", "Sa", "Si", "Sun", "San", "Sin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Shu", "Sha", "Shi", "Shun", "Shan", "Shin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Su", "Sa", "Si", "Sun", "San", "Sin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Du", "Da", "Di", "Dun", "Dan", "Din" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Tu", "Ta", "Ti", "Tun", "Tan", "Tin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Dhu", "Dha", "Dhi", "Dhun", "Dhan", "Dhin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "'Uu", "'Aa", "'Ii", "'Uun", "'Aan", "'Iin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Ghu", "Gha", "Ghi", "Ghun", "Ghan", "Ghin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Fu", "Fa", "Fi", "Fun", "Fan", "Fin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Qu", "Qa", "Qi", "Qun", "Qan", "Qin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Ku", "Ka", "Ki", "Kun", "Kan", "Kin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Lu", "La", "Li", "Lun", "Lan", "Lin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Mu", "Ma", "Mi", "Mun", "Man", "Min" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Nu", "Na", "Ni", "Nun", "Nan", "Nin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Wu", "Wa", "Wi", "Wun", "Wan", "Win" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Hu", "Ha", "Hi", "Hun", "Han", "Hin" });

            migrationBuilder.UpdateData(
                table: "ArabicLetters",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "DammaSound", "FathaSound", "KasraSound", "TanweenDammSound", "TanweenFathSound", "TanweenKasrSound" },
                values: new object[] { "Yu", "Ya", "Yi", "Yun", "Yan", "Yin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DammaSound",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "FathaSound",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "KasraSound",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "TanweenDammSound",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "TanweenFathSound",
                table: "ArabicLetters");

            migrationBuilder.DropColumn(
                name: "TanweenKasrSound",
                table: "ArabicLetters");
        }
    }
}
