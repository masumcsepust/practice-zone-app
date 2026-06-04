using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAllSurahs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ayahs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ayahs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ayahs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Ayahs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Ayahs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Ayahs",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Ayahs",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "NameArabic", "NameBangla", "NameEnglish" },
                values: new object[] { "ٱلْفَاتِحَةِ", "The Opening", "Al-Faatiha" });

            migrationBuilder.InsertData(
                table: "Surahs",
                columns: new[] { "Id", "NameArabic", "NameBangla", "NameEnglish", "SurahNumber", "TotalAyahs" },
                values: new object[,]
                {
                    { 2, "البَقَرَةِ", "The Cow", "Al-Baqara", 2, 286 },
                    { 3, "آلِ عِمۡرَانَ", "The Family of Imraan", "Aal-i-Imraan", 3, 200 },
                    { 4, "النِّسَاءِ", "The Women", "An-Nisaa", 4, 176 },
                    { 5, "المَائـِدَةِ", "The Table", "Al-Maaida", 5, 120 },
                    { 6, "الأَنۡعَامِ", "The Cattle", "Al-An'aam", 6, 165 },
                    { 7, "الأَعۡرَافِ", "The Heights", "Al-A'raaf", 7, 206 },
                    { 8, "الأَنفَالِ", "The Spoils of War", "Al-Anfaal", 8, 75 },
                    { 9, "التَّوۡبَةِ", "The Repentance", "At-Tawba", 9, 129 },
                    { 10, "يُونُسَ", "Jonas", "Yunus", 10, 109 },
                    { 11, "هُودٍ", "Hud", "Hud", 11, 123 },
                    { 12, "يُوسُفَ", "Joseph", "Yusuf", 12, 111 },
                    { 13, "الرَّعۡدِ", "The Thunder", "Ar-Ra'd", 13, 43 },
                    { 14, "إِبۡرَاهِيمَ", "Abraham", "Ibrahim", 14, 52 },
                    { 15, "الحِجۡرِ", "The Rock", "Al-Hijr", 15, 99 },
                    { 16, "النَّحۡلِ", "The Bee", "An-Nahl", 16, 128 },
                    { 17, "الإِسۡرَاءِ", "The Night Journey", "Al-Israa", 17, 111 },
                    { 18, "الكَهۡفِ", "The Cave", "Al-Kahf", 18, 110 },
                    { 19, "مَرۡيَمَ", "Mary", "Maryam", 19, 98 },
                    { 20, "طه", "Taa-Haa", "Taa-Haa", 20, 135 },
                    { 21, "الأَنبِيَاءِ", "The Prophets", "Al-Anbiyaa", 21, 112 },
                    { 22, "الحَجِّ", "The Pilgrimage", "Al-Hajj", 22, 78 },
                    { 23, "المُؤۡمِنُونَ", "The Believers", "Al-Muminoon", 23, 118 },
                    { 24, "النُّورِ", "The Light", "An-Noor", 24, 64 },
                    { 25, "الفُرۡقَانِ", "The Criterion", "Al-Furqaan", 25, 77 },
                    { 26, "الشُّعَرَاءِ", "The Poets", "Ash-Shu'araa", 26, 227 },
                    { 27, "النَّمۡلِ", "The Ant", "An-Naml", 27, 93 },
                    { 28, "القَصَصِ", "The Stories", "Al-Qasas", 28, 88 },
                    { 29, "العَنكَبُوتِ", "The Spider", "Al-Ankaboot", 29, 69 },
                    { 30, "الرُّومِ", "The Romans", "Ar-Room", 30, 60 },
                    { 31, "لُقۡمَانَ", "Luqman", "Luqman", 31, 34 },
                    { 32, "السَّجۡدَةِ", "The Prostration", "As-Sajda", 32, 30 },
                    { 33, "الأَحۡزَابِ", "The Clans", "Al-Ahzaab", 33, 73 },
                    { 34, "سَبَإٍ", "Sheba", "Saba", 34, 54 },
                    { 35, "فَاطِرٍ", "The Originator", "Faatir", 35, 45 },
                    { 36, "يسٓ", "Yaseen", "Yaseen", 36, 83 },
                    { 37, "الصَّافَّاتِ", "Those drawn up in Ranks", "As-Saaffaat", 37, 182 },
                    { 38, "صٓ", "The letter Saad", "Saad", 38, 88 },
                    { 39, "الزُّمَرِ", "The Groups", "Az-Zumar", 39, 75 },
                    { 40, "غَافِرٍ", "The Forgiver", "Ghafir", 40, 85 },
                    { 41, "فُصِّلَتۡ", "Explained in detail", "Fussilat", 41, 54 },
                    { 42, "الشُّورَىٰ", "Consultation", "Ash-Shura", 42, 53 },
                    { 43, "الزُّخۡرُفِ", "Ornaments of gold", "Az-Zukhruf", 43, 89 },
                    { 44, "الدُّخَانِ", "The Smoke", "Ad-Dukhaan", 44, 59 },
                    { 45, "الجَاثِيَةِ", "Crouching", "Al-Jaathiya", 45, 37 },
                    { 46, "الأَحۡقَافِ", "The Dunes", "Al-Ahqaf", 46, 35 },
                    { 47, "مُحَمَّدٍ", "Muhammad", "Muhammad", 47, 38 },
                    { 48, "الفَتۡحِ", "The Victory", "Al-Fath", 48, 29 },
                    { 49, "الحُجُرَاتِ", "The Inner Apartments", "Al-Hujuraat", 49, 18 },
                    { 50, "قٓ", "The letter Qaaf", "Qaaf", 50, 45 },
                    { 51, "الذَّارِيَاتِ", "The Winnowing Winds", "Adh-Dhaariyat", 51, 60 },
                    { 52, "الطُّورِ", "The Mount", "At-Tur", 52, 49 },
                    { 53, "النَّجۡمِ", "The Star", "An-Najm", 53, 62 },
                    { 54, "القَمَرِ", "The Moon", "Al-Qamar", 54, 55 },
                    { 55, "الرَّحۡمَٰن", "The Beneficent", "Ar-Rahmaan", 55, 78 },
                    { 56, "الوَاقِعَةِ", "The Inevitable", "Al-Waaqia", 56, 96 },
                    { 57, "الحَدِيدِ", "The Iron", "Al-Hadid", 57, 29 },
                    { 58, "المُجَادلَةِ", "The Pleading Woman", "Al-Mujaadila", 58, 22 },
                    { 59, "الحَشۡرِ", "The Exile", "Al-Hashr", 59, 24 },
                    { 60, "المُمۡتَحنَةِ", "She that is to be examined", "Al-Mumtahana", 60, 13 },
                    { 61, "الصَّفِّ", "The Ranks", "As-Saff", 61, 14 },
                    { 62, "الجُمُعَةِ", "Friday", "Al-Jumu'a", 62, 11 },
                    { 63, "المُنَافِقُونَ", "The Hypocrites", "Al-Munaafiqoon", 63, 11 },
                    { 64, "التَّغَابُنِ", "Mutual Disillusion", "At-Taghaabun", 64, 18 },
                    { 65, "الطَّلَاقِ", "Divorce", "At-Talaaq", 65, 12 },
                    { 66, "التَّحۡرِيمِ", "The Prohibition", "At-Tahrim", 66, 12 },
                    { 67, "المُلۡكِ", "The Sovereignty", "Al-Mulk", 67, 30 },
                    { 68, "القَلَمِ", "The Pen", "Al-Qalam", 68, 52 },
                    { 69, "الحَاقَّةِ", "The Reality", "Al-Haaqqa", 69, 52 },
                    { 70, "المَعَارِجِ", "The Ascending Stairways", "Al-Ma'aarij", 70, 44 },
                    { 71, "نُوحٍ", "Noah", "Nooh", 71, 28 },
                    { 72, "الجِنِّ", "The Jinn", "Al-Jinn", 72, 28 },
                    { 73, "المُزَّمِّلِ", "The Enshrouded One", "Al-Muzzammil", 73, 20 },
                    { 74, "المُدَّثِّرِ", "The Cloaked One", "Al-Muddaththir", 74, 56 },
                    { 75, "القِيَامَةِ", "The Resurrection", "Al-Qiyaama", 75, 40 },
                    { 76, "الإِنسَانِ", "Man", "Al-Insaan", 76, 31 },
                    { 77, "المُرۡسَلَاتِ", "The Emissaries", "Al-Mursalaat", 77, 50 },
                    { 78, "النَّبَإِ", "The Announcement", "An-Naba", 78, 40 },
                    { 79, "النَّازِعَاتِ", "Those who drag forth", "An-Naazi'aat", 79, 46 },
                    { 80, "عَبَسَ", "He frowned", "Abasa", 80, 42 },
                    { 81, "التَّكۡوِيرِ", "The Overthrowing", "At-Takwir", 81, 29 },
                    { 82, "الانفِطَارِ", "The Cleaving", "Al-Infitaar", 82, 19 },
                    { 83, "المُطَفِّفِينَ", "Defrauding", "Al-Mutaffifin", 83, 36 },
                    { 84, "الانشِقَاقِ", "The Splitting Open", "Al-Inshiqaaq", 84, 25 },
                    { 85, "البُرُوجِ", "The Constellations", "Al-Burooj", 85, 22 },
                    { 86, "الطَّارِقِ", "The Morning Star", "At-Taariq", 86, 17 },
                    { 87, "الأَعۡلَىٰ", "The Most High", "Al-A'laa", 87, 19 },
                    { 88, "الغَاشِيَةِ", "The Overwhelming", "Al-Ghaashiya", 88, 26 },
                    { 89, "الفَجۡرِ", "The Dawn", "Al-Fajr", 89, 30 },
                    { 90, "البَلَدِ", "The City", "Al-Balad", 90, 20 },
                    { 91, "الشَّمۡسِ", "The Sun", "Ash-Shams", 91, 15 },
                    { 92, "اللَّيۡلِ", "The Night", "Al-Lail", 92, 21 },
                    { 93, "الضُّحَىٰ", "The Morning Hours", "Ad-Dhuhaa", 93, 11 },
                    { 94, "الشَّرۡحِ", "The Consolation", "Ash-Sharh", 94, 8 },
                    { 95, "التِّينِ", "The Fig", "At-Tin", 95, 8 },
                    { 96, "العَلَقِ", "The Clot", "Al-Alaq", 96, 19 },
                    { 97, "القَدۡرِ", "The Power, Fate", "Al-Qadr", 97, 5 },
                    { 98, "البَيِّنَةِ", "The Evidence", "Al-Bayyina", 98, 8 },
                    { 99, "الزَّلۡزَلَةِ", "The Earthquake", "Az-Zalzala", 99, 8 },
                    { 100, "العَادِيَاتِ", "The Chargers", "Al-Aadiyaat", 100, 11 },
                    { 101, "القَارِعَةِ", "The Calamity", "Al-Qaari'a", 101, 11 },
                    { 102, "التَّكَاثُرِ", "Competition", "At-Takaathur", 102, 8 },
                    { 103, "العَصۡرِ", "The Declining Day", "Al-Asr", 103, 3 },
                    { 104, "الهُمَزَةِ", "The Traducer", "Al-Humaza", 104, 9 },
                    { 105, "الفِيلِ", "The Elephant", "Al-Fil", 105, 5 },
                    { 106, "قُرَيۡشٍ", "Quraysh", "Quraish", 106, 4 },
                    { 107, "المَاعُونِ", "Almsgiving", "Al-Maa'un", 107, 7 },
                    { 108, "الكَوۡثَرِ", "Abundance", "Al-Kawthar", 108, 3 },
                    { 109, "الكَافِرُونَ", "The Disbelievers", "Al-Kaafiroon", 109, 6 },
                    { 110, "النَّصۡرِ", "Divine Support", "An-Nasr", 110, 3 },
                    { 111, "المَسَدِ", "The Palm Fibre", "Al-Masad", 111, 5 },
                    { 112, "الإِخۡلَاصِ", "Sincerity", "Al-Ikhlaas", 112, 4 },
                    { 113, "الفَلَقِ", "The Dawn", "Al-Falaq", 113, 5 },
                    { 114, "النَّاسِ", "Mankind", "An-Naas", 114, 6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.InsertData(
                table: "Ayahs",
                columns: new[] { "Id", "ArabicText", "AyahNumber", "BanglaTranslation", "EnglishTranslation", "NormalizedArabicText", "SurahId", "Transliteration" },
                values: new object[,]
                {
                    { 1, "بِسْمِ اللَّهِ الرَّحْمَٰنِ الرَّحِيمِ", 1, "পরম করুণাময় অতি দয়ালু আল্লাহর নামে।", "In the name of Allah, the Most Gracious, the Most Merciful.", "بسم الله الرحمن الرحيم", 1, "Bismillahi r-rahmani r-raheem" },
                    { 2, "الْحَمْدُ لِلَّهِ رَبِّ الْعَالَمِينَ", 2, "সমস্ত প্রশংসা আল্লাহর জন্য, যিনি সমগ্র জগতের প্রতিপালক।", "All praise is due to Allah, Lord of all the worlds.", "الحمد لله رب العالمين", 1, "Alhamdu lillahi rabbi l-'alamin" },
                    { 3, "الرَّحْمَٰنِ الرَّحِيمِ", 3, "পরম করুণাময়, অতি দয়ালু।", "The Most Gracious, the Most Merciful.", "الرحمن الرحيم", 1, "Ar-rahmani r-raheem" },
                    { 4, "مَالِكِ يَوْمِ الدِّينِ", 4, "বিচার দিনের মালিক।", "Master of the Day of Judgment.", "مالك يوم الدين", 1, "Maliki yawmi d-deen" },
                    { 5, "إِيَّاكَ نَعْبُدُ وَإِيَّاكَ نَسْتَعِينُ", 5, "আমরা কেবল তোমারই ইবাদত করি এবং কেবল তোমারই সাহায্য চাই।", "You alone we worship, and You alone we ask for help.", "اياك نعبد واياك نستعين", 1, "Iyyaka na'budu wa-iyyaka nasta'een" },
                    { 6, "اهْدِنَا الصِّرَاطَ الْمُسْتَقِيمَ", 6, "আমাদের সরল পথ দেখাও।", "Guide us to the straight path.", "اهدنا الصراط المستقيم", 1, "Ihdina s-sirata l-mustaqeem" },
                    { 7, "صِرَاطَ الَّذِينَ أَنْعَمْتَ عَلَيْهِمْ غَيْرِ الْمَغْضُوبِ عَلَيْهِمْ وَلَا الضَّالِّينَ", 7, "তাদের পথ, যাদের তুমি নেয়ামত দিয়েছ; তাদের পথ নয় যাদের উপর তোমার ক্রোধ আছে এবং যারা পথভ্রষ্ট।", "The path of those upon whom You have bestowed favor, not of those who have evoked Your anger or of those who are astray.", "صراط الذين انعمت عليهم غير المغضوب عليهم ولا الضالين", 1, "Sirata l-ladhina an'amta 'alayhim ghayri l-maghdubi 'alayhim wa-la d-dalleen" }
                });

            migrationBuilder.UpdateData(
                table: "Surahs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "NameArabic", "NameBangla", "NameEnglish" },
                values: new object[] { "الفاتحة", "সূচনা", "The Opening" });
        }
    }
}
