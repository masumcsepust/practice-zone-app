using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class LessonEntityConfiguration : IEntityTypeConfiguration<Lesson>
{
    private static Guid G(int n) => new($"30000000-0000-0000-0000-{n:D12}");

    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(l => l.Id);
        builder.HasIndex(l => l.SequenceOrder).IsUnique();
        builder.ToTable("Lessons");

        builder.OwnsOne(l => l.Title, nb =>
        {
            nb.Property(n => n.En).HasColumnName("TitleEn").HasMaxLength(150);
            nb.Property(n => n.Bn).HasColumnName("TitleBn").HasMaxLength(200);
            nb.HasData(
                new { LessonId = G(1), En = "Ba with Harakat",  Bn = "বা-এর হরকত অনুশীলন" },
                new { LessonId = G(2), En = "Ta with Harakat",  Bn = "তা-এর হরকত অনুশীলন" },
                new { LessonId = G(3), En = "Ba vs Ba — Fatha vs Kasra", Bn = "বা ফাতহা বনাম বা কাসরা তুলনা" }
            );
        });

        builder.HasData(
            new Lesson { Id = G(1), SequenceOrder = 1 },
            new Lesson { Id = G(2), SequenceOrder = 2 },
            new Lesson { Id = G(3), SequenceOrder = 3 }
        );
    }
}
