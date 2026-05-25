using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class PronunciationAssessmentResultConfiguration
    : IEntityTypeConfiguration<PronunciationAssessmentResult>
{
    public void Configure(EntityTypeBuilder<PronunciationAssessmentResult> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.RecognizedText).HasMaxLength(2000);
        builder.HasIndex(p => p.StreamingSessionId);
        builder.HasIndex(p => p.AyahId);

        builder.HasOne(p => p.StreamingSession)
               .WithMany()
               .HasForeignKey(p => p.StreamingSessionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Ayah)
               .WithMany()
               .HasForeignKey(p => p.AyahId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Words)
               .WithOne(w => w.Assessment)
               .HasForeignKey(w => w.PronunciationAssessmentResultId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("PronunciationAssessmentResults");
    }
}
