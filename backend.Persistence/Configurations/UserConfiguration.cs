using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Email       ).IsRequired().HasMaxLength(200);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(300);
        builder.Property(u => u.Role        ).IsRequired().HasMaxLength(20).HasDefaultValue("User");
        builder.Property(u => u.CreatedAt   ).HasDefaultValueSql("NOW()");
        builder.Property(u => u.UpdatedAt   ).HasDefaultValueSql("NOW()");
        builder.HasIndex(u => u.Email).IsUnique();
        builder.ToTable("Users");

        builder.HasOne(u => u.Profile)
               .WithOne(p => p.User)
               .HasForeignKey<UserProfile>(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
