using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class PracticeItemConfiguration : IEntityTypeConfiguration<PracticeItem>
{
    private static Guid G(int n)  => new($"40000000-0000-0000-0000-{n:D12}");
    private static Guid GL(int n) => new($"30000000-0000-0000-0000-{n:D12}"); // Lesson
    private static Guid GS(int n) => new($"20000000-0000-0000-0000-{n:D12}"); // SyllableSound

    public void Configure(EntityTypeBuilder<PracticeItem> builder)
    {
        builder.HasKey(p => p.Id);
        builder.ToTable("PracticeItems");

        builder.HasOne(p => p.Lesson)
               .WithMany(l => l.PracticeItems)
               .HasForeignKey(p => p.LessonId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.TargetSyllable)
               .WithMany()
               .HasForeignKey(p => p.TargetSyllableId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.CompareWithSyllable)
               .WithMany()
               .HasForeignKey(p => p.CompareWithSyllableId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(false);

        builder.OwnsOne(p => p.Instruction, nb =>
        {
            nb.Property(n => n.En).HasColumnName("InstructionEn").HasMaxLength(300);
            nb.Property(n => n.Bn).HasColumnName("InstructionBn").HasMaxLength(500);
            nb.HasData(
                new { PracticeItemId = G(1), En = "Pronounce the letter with Fatha",  Bn = "বর্ণটি ফাতহা দিয়ে উচ্চারণ করুন" },
                new { PracticeItemId = G(2), En = "Pronounce the letter with Kasra",   Bn = "বর্ণটি কাসরা দিয়ে উচ্চারণ করুন" },
                new { PracticeItemId = G(3), En = "Pronounce the letter with Damma",   Bn = "বর্ণটি দাম্মা দিয়ে উচ্চারণ করুন" },
                new { PracticeItemId = G(4), En = "Pronounce the letter with Fatha",   Bn = "বর্ণটি ফাতহা দিয়ে উচ্চারণ করুন" },
                new { PracticeItemId = G(5), En = "Pronounce the letter with Kasra",   Bn = "বর্ণটি কাসরা দিয়ে উচ্চারণ করুন" },
                new { PracticeItemId = G(6), En = "Pronounce the letter with Damma",   Bn = "বর্ণটি দাম্মা দিয়ে উচ্চারণ করুন" },
                new { PracticeItemId = G(7), En = "Notice the difference: Ba vs Bi",   Bn = "পার্থক্য লক্ষ্য করুন: বা বনাম বি" }
            );
        });

        builder.OwnsOne(p => p.SuccessTip, nb =>
        {
            nb.Property(n => n.En).HasColumnName("SuccessTipEn").HasMaxLength(300);
            nb.Property(n => n.Bn).HasColumnName("SuccessTipBn").HasMaxLength(500);
            nb.HasData(
                new { PracticeItemId = G(1), En = "Open your mouth slightly for 'a' sound",    Bn = "'আ' শব্দের জন্য মুখ সামান্য খুলুন" },
                new { PracticeItemId = G(2), En = "Press lips and tongue down for 'i' sound",  Bn = "'ই' শব্দের জন্য ঠোঁট ও জিহ্বা নিচে রাখুন" },
                new { PracticeItemId = G(3), En = "Round your lips slightly for 'u' sound",    Bn = "'উ' শব্দের জন্য ঠোঁট সামান্য গোলাকার করুন" },
                new { PracticeItemId = G(4), En = "Open your mouth slightly for 'a' sound",    Bn = "'আ' শব্দের জন্য মুখ সামান্য খুলুন" },
                new { PracticeItemId = G(5), En = "Press lips and tongue down for 'i' sound",  Bn = "'ই' শব্দের জন্য ঠোঁট ও জিহ্বা নিচে রাখুন" },
                new { PracticeItemId = G(6), En = "Round your lips slightly for 'u' sound",    Bn = "'উ' শব্দের জন্য ঠোঁট সামান্য গোলাকার করুন" },
                new { PracticeItemId = G(7), En = "Fatha is open 'a', Kasra is closed 'i'",    Bn = "ফাতহা খোলা 'আ', কাসরা বন্ধ 'ই'" }
            );
        });

        builder.HasData(
            // Lesson 1: Ba harakat (3 items)
            new PracticeItem { Id = G(1), LessonId = GL(1), TargetSyllableId = GS(1), CompareWithSyllableId = null },
            new PracticeItem { Id = G(2), LessonId = GL(1), TargetSyllableId = GS(2), CompareWithSyllableId = null },
            new PracticeItem { Id = G(3), LessonId = GL(1), TargetSyllableId = GS(3), CompareWithSyllableId = null },
            // Lesson 2: Ta harakat (3 items)
            new PracticeItem { Id = G(4), LessonId = GL(2), TargetSyllableId = GS(4), CompareWithSyllableId = null },
            new PracticeItem { Id = G(5), LessonId = GL(2), TargetSyllableId = GS(5), CompareWithSyllableId = null },
            new PracticeItem { Id = G(6), LessonId = GL(2), TargetSyllableId = GS(6), CompareWithSyllableId = null },
            // Lesson 3: comparison (Ba Fatha vs Ba Kasra)
            new PracticeItem { Id = G(7), LessonId = GL(3), TargetSyllableId = GS(1), CompareWithSyllableId = GS(2) }
        );
    }
}
