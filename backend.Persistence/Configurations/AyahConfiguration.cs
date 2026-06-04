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
        builder.HasIndex(a => a.Page);

        builder.ToTable("Ayahs");
        // Ayahs are loaded on-demand from alquran.cloud API — no seed data here.
    }
}
