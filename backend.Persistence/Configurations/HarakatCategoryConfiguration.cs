using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class HarakatCategoryConfiguration : IEntityTypeConfiguration<HarakatCategory>
{
    public void Configure(EntityTypeBuilder<HarakatCategory> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name         ).IsRequired().HasMaxLength(30);
        builder.Property(c => c.ArabicName   ).HasMaxLength(60);
        builder.Property(c => c.BanglaName   ).HasMaxLength(60);
        builder.Property(c => c.Symbol       ).HasMaxLength(10);
        builder.Property(c => c.Type         ).IsRequired().HasMaxLength(10); // "Harakat"|"Tanween"
        builder.Property(c => c.Description  ).HasMaxLength(200);
        builder.Property(c => c.DescriptionBn).HasMaxLength(200);
        builder.HasIndex(c => c.DisplayOrder).IsUnique();
        builder.ToTable("HarakatCategories");

        builder.HasData(
            new HarakatCategory
            {
                Id = 1, Name = "Fatha", ArabicName = "فَتْحَة", BanglaName = "ফাতহা",
                Symbol = "َ", Type = "Harakat", DisplayOrder = 1,
                Description   = "Short vowel 'a' — letter opens upward",
                DescriptionBn = "ছোট 'আ' স্বর — বর্ণের উপরে ছোট তির্যক রেখা"
            },
            new HarakatCategory
            {
                Id = 2, Name = "Kasra", ArabicName = "كَسْرَة", BanglaName = "কাসরা",
                Symbol = "ِ", Type = "Harakat", DisplayOrder = 2,
                Description   = "Short vowel 'i' — letter bends downward",
                DescriptionBn = "ছোট 'ই' স্বর — বর্ণের নিচে ছোট তির্যক রেখা"
            },
            new HarakatCategory
            {
                Id = 3, Name = "Damma", ArabicName = "ضَمَّة", BanglaName = "দাম্মা",
                Symbol = "ُ", Type = "Harakat", DisplayOrder = 3,
                Description   = "Short vowel 'u' — small waw curl above letter",
                DescriptionBn = "ছোট 'উ' স্বর — বর্ণের উপরে ছোট ওয়াও চিহ্ন"
            },
            new HarakatCategory
            {
                Id = 4, Name = "Sukun", ArabicName = "سُكُون", BanglaName = "সুকুন",
                Symbol = "ْ", Type = "Harakat", DisplayOrder = 4,
                Description   = "No vowel — consonant at rest, small circle above",
                DescriptionBn = "কোনো স্বর নেই — বর্ণ বিশ্রামে, উপরে ছোট বৃত্ত"
            },
            new HarakatCategory
            {
                Id = 5, Name = "Shadda", ArabicName = "شَدَّة", BanglaName = "শাদ্দা",
                Symbol = "ّ", Type = "Harakat", DisplayOrder = 5,
                Description   = "Gemination — consonant is doubled in pronunciation",
                DescriptionBn = "জোড়া বর্ণ — একই বর্ণ দুইবার উচ্চারণ করতে হয়"
            },
            new HarakatCategory
            {
                Id = 6, Name = "TanweenFath", ArabicName = "تَنْوِين الفَتْح", BanglaName = "তানউইন ফাতহ (ফাতহাতান)",
                Symbol = "ً", Type = "Tanween", DisplayOrder = 6,
                Description   = "Double fatha — '-an' ending (accusative indefinite noun)",
                DescriptionBn = "দ্বৈত ফাতহা — '-আন' সমাপ্তি (নাকিরা মানসুব)"
            },
            new HarakatCategory
            {
                Id = 7, Name = "TanweenKasr", ArabicName = "تَنْوِين الكَسْر", BanglaName = "তানউইন কাসর (কাসরাতান)",
                Symbol = "ٍ", Type = "Tanween", DisplayOrder = 7,
                Description   = "Double kasra — '-in' ending (genitive indefinite noun)",
                DescriptionBn = "দ্বৈত কাসরা — '-ইন' সমাপ্তি (নাকিরা মাজরুর)"
            },
            new HarakatCategory
            {
                Id = 8, Name = "TanweenDamm", ArabicName = "تَنْوِين الضَّمّ", BanglaName = "তানউইন দাম্ম (দাম্মাতান)",
                Symbol = "ٌ", Type = "Tanween", DisplayOrder = 8,
                Description   = "Double damma — '-un' ending (nominative indefinite noun)",
                DescriptionBn = "দ্বৈত দাম্মা — '-উন' সমাপ্তি (নাকিরা মারফু)"
            }
        );
    }
}
