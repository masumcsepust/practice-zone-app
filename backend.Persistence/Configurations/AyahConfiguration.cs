using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class AyahConfiguration : IEntityTypeConfiguration<Ayah>
{
    public void Configure(EntityTypeBuilder<Ayah> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.ArabicText).IsRequired().HasMaxLength(2000);
        builder.Property(a => a.NormalizedArabicText).IsRequired().HasMaxLength(2000);
        builder.Property(a => a.EnglishTranslation).HasMaxLength(5000);
        builder.Property(a => a.BanglaTranslation).HasMaxLength(5000);
        builder.Property(a => a.Transliteration).HasMaxLength(2000);

        builder.HasOne(a => a.Surah)
            .WithMany()
            .HasForeignKey(a => a.SurahId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => new { a.SurahId, a.AyahNumber }).IsUnique();

        builder.ToTable("Ayahs");

        // Seed: Surah Al-Fatiha (1:1–1:7)
        builder.HasData(
            new Ayah
            {
                Id = 1, SurahId = 1, AyahNumber = 1,
                ArabicText = "بِسْمِ اللَّهِ الرَّحْمَٰنِ الرَّحِيمِ",
                NormalizedArabicText = "بسم الله الرحمن الرحيم",
                EnglishTranslation = "In the name of Allah, the Most Gracious, the Most Merciful.",
                BanglaTranslation = "পরম করুণাময় অতি দয়ালু আল্লাহর নামে।",
                Transliteration = "Bismillahi r-rahmani r-raheem"
            },
            new Ayah
            {
                Id = 2, SurahId = 1, AyahNumber = 2,
                ArabicText = "الْحَمْدُ لِلَّهِ رَبِّ الْعَالَمِينَ",
                NormalizedArabicText = "الحمد لله رب العالمين",
                EnglishTranslation = "All praise is due to Allah, Lord of all the worlds.",
                BanglaTranslation = "সমস্ত প্রশংসা আল্লাহর জন্য, যিনি সমগ্র জগতের প্রতিপালক।",
                Transliteration = "Alhamdu lillahi rabbi l-'alamin"
            },
            new Ayah
            {
                Id = 3, SurahId = 1, AyahNumber = 3,
                ArabicText = "الرَّحْمَٰنِ الرَّحِيمِ",
                NormalizedArabicText = "الرحمن الرحيم",
                EnglishTranslation = "The Most Gracious, the Most Merciful.",
                BanglaTranslation = "পরম করুণাময়, অতি দয়ালু।",
                Transliteration = "Ar-rahmani r-raheem"
            },
            new Ayah
            {
                Id = 4, SurahId = 1, AyahNumber = 4,
                ArabicText = "مَالِكِ يَوْمِ الدِّينِ",
                NormalizedArabicText = "مالك يوم الدين",
                EnglishTranslation = "Master of the Day of Judgment.",
                BanglaTranslation = "বিচার দিনের মালিক।",
                Transliteration = "Maliki yawmi d-deen"
            },
            new Ayah
            {
                Id = 5, SurahId = 1, AyahNumber = 5,
                ArabicText = "إِيَّاكَ نَعْبُدُ وَإِيَّاكَ نَسْتَعِينُ",
                NormalizedArabicText = "اياك نعبد واياك نستعين",
                EnglishTranslation = "You alone we worship, and You alone we ask for help.",
                BanglaTranslation = "আমরা কেবল তোমারই ইবাদত করি এবং কেবল তোমারই সাহায্য চাই।",
                Transliteration = "Iyyaka na'budu wa-iyyaka nasta'een"
            },
            new Ayah
            {
                Id = 6, SurahId = 1, AyahNumber = 6,
                ArabicText = "اهْدِنَا الصِّرَاطَ الْمُسْتَقِيمَ",
                NormalizedArabicText = "اهدنا الصراط المستقيم",
                EnglishTranslation = "Guide us to the straight path.",
                BanglaTranslation = "আমাদের সরল পথ দেখাও।",
                Transliteration = "Ihdina s-sirata l-mustaqeem"
            },
            new Ayah
            {
                Id = 7, SurahId = 1, AyahNumber = 7,
                ArabicText = "صِرَاطَ الَّذِينَ أَنْعَمْتَ عَلَيْهِمْ غَيْرِ الْمَغْضُوبِ عَلَيْهِمْ وَلَا الضَّالِّينَ",
                NormalizedArabicText = "صراط الذين انعمت عليهم غير المغضوب عليهم ولا الضالين",
                EnglishTranslation = "The path of those upon whom You have bestowed favor, not of those who have evoked Your anger or of those who are astray.",
                BanglaTranslation = "তাদের পথ, যাদের তুমি নেয়ামত দিয়েছ; তাদের পথ নয় যাদের উপর তোমার ক্রোধ আছে এবং যারা পথভ্রষ্ট।",
                Transliteration = "Sirata l-ladhina an'amta 'alayhim ghayri l-maghdubi 'alayhim wa-la d-dalleen"
            }
        );
    }
}
