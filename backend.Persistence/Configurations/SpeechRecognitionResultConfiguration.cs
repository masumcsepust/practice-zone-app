using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class SpeechRecognitionResultConfiguration : IEntityTypeConfiguration<SpeechRecognitionResult>
{
    public void Configure(EntityTypeBuilder<SpeechRecognitionResult> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.RecognizedText).IsRequired().HasMaxLength(2000);
        builder.Property(r => r.ResultType).IsRequired().HasMaxLength(20);
        builder.Property(r => r.RecognizedAt).IsRequired();

        builder.HasOne(r => r.StreamingSession)
            .WithMany()
            .HasForeignKey(r => r.StreamingSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => r.StreamingSessionId);
        builder.ToTable("SpeechRecognitionResults");
    }
}
