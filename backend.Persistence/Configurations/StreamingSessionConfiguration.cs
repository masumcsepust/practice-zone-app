using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class StreamingSessionConfiguration : IEntityTypeConfiguration<StreamingSession>
{
    public void Configure(EntityTypeBuilder<StreamingSession> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.ConnectionId).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Status).IsRequired().HasMaxLength(50);
        builder.HasIndex(s => s.ConnectionId).IsUnique();
        builder.HasIndex(s => s.UserId);
        builder.ToTable("StreamingSessions");
    }
}
