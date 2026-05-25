using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class WebSocketConnectionConfiguration : IEntityTypeConfiguration<WebSocketConnection>
{
    public void Configure(EntityTypeBuilder<WebSocketConnection> builder)
    {
        builder.HasKey(w => w.Id);
        builder.Property(w => w.ConnectionId).IsRequired().HasMaxLength(100);
        builder.Property(w => w.IpAddress).HasMaxLength(50);
        builder.HasIndex(w => w.ConnectionId);
        builder.HasIndex(w => w.UserId);
        builder.ToTable("WebSocketConnections");
    }
}
