using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class TajweedPracticeRecordConfiguration : IEntityTypeConfiguration<TajweedPracticeRecord>
{
    public void Configure(EntityTypeBuilder<TajweedPracticeRecord> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Mode).IsRequired().HasMaxLength(30);
        builder.Property(r => r.Score).HasPrecision(5, 2);
        builder.Property(r => r.AccuracyScore).HasPrecision(5, 2);
        builder.Property(r => r.PracticedAt).HasDefaultValueSql("NOW()");

        builder.HasOne(r => r.Letter)
               .WithMany()
               .HasForeignKey(r => r.LetterId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => new { r.LetterId, r.Mode });
    }
}
