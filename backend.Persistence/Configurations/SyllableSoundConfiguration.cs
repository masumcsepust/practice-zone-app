using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class SyllableSoundConfiguration : IEntityTypeConfiguration<SyllableSound>
{
    private static Guid G(int n)  => new($"20000000-0000-0000-0000-{n:D12}");
    private static Guid GL(int n) => new($"00000000-0000-0000-0000-{n:D12}"); // ArabicLetter
    private static Guid GS(int n) => new($"10000000-0000-0000-0000-{n:D12}"); // DiacriticSign

    public void Configure(EntityTypeBuilder<SyllableSound> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.CombinedCharacter).IsRequired().HasMaxLength(20);
        builder.Property(s => s.AudioUrl         ).HasMaxLength(300);
        builder.HasIndex(s => new { s.LetterId, s.SignId }).IsUnique();
        builder.ToTable("SyllableSounds");

        builder.HasOne(s => s.Letter)
               .WithMany()
               .HasForeignKey(s => s.LetterId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Sign)
               .WithMany()
               .HasForeignKey(s => s.SignId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(s => s.Transliteration, nb =>
        {
            nb.Property(n => n.En).HasColumnName("TransliterationEn").HasMaxLength(100);
            nb.Property(n => n.Bn).HasColumnName("TransliterationBn").HasMaxLength(200);
            nb.HasData(
                new { SyllableSoundId = G(1), En = "Ba", Bn = "বা"  },
                new { SyllableSoundId = G(2), En = "Bi", Bn = "বি"  },
                new { SyllableSoundId = G(3), En = "Bu", Bn = "বু"  },
                new { SyllableSoundId = G(4), En = "Ta", Bn = "তা"  },
                new { SyllableSoundId = G(5), En = "Ti", Bn = "তি"  },
                new { SyllableSoundId = G(6), En = "Tu", Bn = "তু"  }
            );
        });

        builder.HasData(
            new SyllableSound { Id = G(1), LetterId = GL(2),  SignId = GS(1), CombinedCharacter = "بَ", AudioUrl = "" },
            new SyllableSound { Id = G(2), LetterId = GL(2),  SignId = GS(2), CombinedCharacter = "بِ", AudioUrl = "" },
            new SyllableSound { Id = G(3), LetterId = GL(2),  SignId = GS(3), CombinedCharacter = "بُ", AudioUrl = "" },
            new SyllableSound { Id = G(4), LetterId = GL(3),  SignId = GS(1), CombinedCharacter = "تَ", AudioUrl = "" },
            new SyllableSound { Id = G(5), LetterId = GL(3),  SignId = GS(2), CombinedCharacter = "تِ", AudioUrl = "" },
            new SyllableSound { Id = G(6), LetterId = GL(3),  SignId = GS(3), CombinedCharacter = "تُ", AudioUrl = "" }
        );
    }
}
