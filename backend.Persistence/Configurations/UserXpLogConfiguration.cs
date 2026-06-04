using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class UserXpLogConfiguration : IEntityTypeConfiguration<UserXpLog>
{
    public void Configure(EntityTypeBuilder<UserXpLog> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ActionType).IsRequired().HasMaxLength(50);
        builder.Property(x => x.XpEarned  ).HasDefaultValue(0);
        builder.Property(x => x.EarnedAt  ).HasDefaultValueSql("NOW()");
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.EarnedAt);
        builder.ToTable("UserXpLogs");

        builder.HasOne(x => x.User)
               .WithMany()
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Lesson)
               .WithMany()
               .HasForeignKey(x => x.LessonId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
