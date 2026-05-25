using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class SurahConfiguration : IEntityTypeConfiguration<Surah>
{
    public void Configure(EntityTypeBuilder<Surah> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.NameArabic).IsRequired().HasMaxLength(200);
        builder.Property(s => s.NameEnglish).IsRequired().HasMaxLength(200);
        builder.Property(s => s.NameBangla).IsRequired().HasMaxLength(200);

        builder.HasIndex(s => s.SurahNumber).IsUnique();

        builder.ToTable("Surahs");

        builder.HasData(new Surah
        {
            Id = 1,
            SurahNumber = 1,
            NameArabic = "الفاتحة",
            NameEnglish = "The Opening",
            NameBangla = "সূচনা",
            TotalAyahs = 7
        });
    }
}
