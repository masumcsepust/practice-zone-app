using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.DisplayName      ).IsRequired().HasMaxLength(100);
        builder.Property(p => p.AvatarUrl        ).HasMaxLength(500);
        builder.Property(p => p.Username         ).HasMaxLength(50).HasDefaultValue("");
        builder.Property(p => p.TotalXp          ).HasDefaultValue(0);
        builder.Property(p => p.CurrentStreak    ).HasDefaultValue(0);
        builder.Property(p => p.TotalLessons     ).HasDefaultValue(0);
        builder.Property(p => p.CorrectAnswerRate).HasDefaultValue(0.0);
        builder.Property(p => p.CreatedAt    ).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt    ).HasDefaultValueSql("NOW()");
        builder.ToTable("UserProfiles");
    }
}
