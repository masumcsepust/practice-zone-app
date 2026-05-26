using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class TajweedRuleConfiguration : IEntityTypeConfiguration<TajweedRule>
{
    public void Configure(EntityTypeBuilder<TajweedRule> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.RuleType).IsRequired().HasMaxLength(50);
        builder.Property(r => r.ArabicName).HasMaxLength(100);
        builder.Property(r => r.Description).HasMaxLength(500);

        builder.HasData(
            new TajweedRule { Id = 1, RuleType = "Madd",     ArabicName = "مد",    Description = "Elongation of vowel letters ا و ي for a minimum of 2 harakats." },
            new TajweedRule { Id = 2, RuleType = "Ghunnah",  ArabicName = "غنة",   Description = "Nasalization of ن or م with shaddah, held for 2 harakats." },
            new TajweedRule { Id = 3, RuleType = "Qalqalah", ArabicName = "قلقلة", Description = "Echoing bounce on ق ط ب ج د when carrying sukoon or at word end." },
            new TajweedRule { Id = 4, RuleType = "Ikhfa",    ArabicName = "إخفاء", Description = "Partial concealment of noon sakinah/tanween before 15 letters." },
            new TajweedRule { Id = 5, RuleType = "Idgham",   ArabicName = "إدغام", Description = "Merging of noon sakinah/tanween into the following يرملون letter." }
        );
    }
}
