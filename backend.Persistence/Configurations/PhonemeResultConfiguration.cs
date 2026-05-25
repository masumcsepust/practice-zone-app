using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class PhonemeResultConfiguration : IEntityTypeConfiguration<PhonemeResult>
{
    public void Configure(EntityTypeBuilder<PhonemeResult> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Phoneme).IsRequired().HasMaxLength(20);
        builder.HasIndex(p => p.WordPronunciationResultId);

        builder.ToTable("PhonemeResults");
    }
}
