using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class RecitationSessionConfiguration : IEntityTypeConfiguration<RecitationSession>
{
    public void Configure(EntityTypeBuilder<RecitationSession> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Status).IsRequired().HasMaxLength(50);
        builder.Property(r => r.RecognizedText).IsRequired().HasMaxLength(2000);
        builder.Property(r => r.NormalizedRecognizedText).IsRequired().HasMaxLength(2000);
        builder.Property(r => r.StartedAt).IsRequired();
        builder.Property(r => r.AudioFilePath).IsRequired().HasMaxLength(500);

        builder.HasOne(r => r.Ayah)
            .WithMany()
            .HasForeignKey(r => r.AyahId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => r.UserId);
        builder.HasIndex(r => r.AyahId);
        builder.HasIndex(r => r.StartedAt);

        builder.ToTable("RecitationSessions");
    }
}
