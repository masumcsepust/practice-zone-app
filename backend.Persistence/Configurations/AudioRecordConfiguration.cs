using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class AudioRecordConfiguration : IEntityTypeConfiguration<AudioRecord>
{
    public void Configure(EntityTypeBuilder<AudioRecord> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.FilePath).IsRequired().HasMaxLength(500);
        builder.HasIndex(a => a.StreamingSessionId);
        builder.ToTable("AudioRecords");
    }
}
