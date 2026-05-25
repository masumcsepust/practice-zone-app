using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class WordPronunciationResultConfiguration
    : IEntityTypeConfiguration<WordPronunciationResult>
{
    public void Configure(EntityTypeBuilder<WordPronunciationResult> builder)
    {
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Word).IsRequired().HasMaxLength(200);
        builder.Property(w => w.ErrorType).HasMaxLength(50);
        builder.HasIndex(w => w.PronunciationAssessmentResultId);

        builder.HasMany(w => w.Phonemes)
               .WithOne(p => p.Word)
               .HasForeignKey(p => p.WordPronunciationResultId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("WordPronunciationResults");
    }
}
