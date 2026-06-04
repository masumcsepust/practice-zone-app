using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class DiacriticSignConfiguration : IEntityTypeConfiguration<DiacriticSign>
{
    private static Guid G(int n) => new($"10000000-0000-0000-0000-{n:D12}");

    public void Configure(EntityTypeBuilder<DiacriticSign> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Symbol   ).IsRequired().HasMaxLength(10);
        builder.Property(d => d.SignGroup).IsRequired().HasMaxLength(30);
        builder.HasIndex(d => d.Symbol).IsUnique();
        builder.ToTable("DiacriticSigns");

        builder.OwnsOne(d => d.Name, nb =>
        {
            nb.Property(n => n.En).HasColumnName("NameEn").HasMaxLength(50);
            nb.Property(n => n.Bn).HasColumnName("NameBn").HasMaxLength(100);
            nb.HasData(
                new { DiacriticSignId = G(1), En = "Fatha",        Bn = "ফাতহা"       },
                new { DiacriticSignId = G(2), En = "Kasra",        Bn = "কাসরা"       },
                new { DiacriticSignId = G(3), En = "Damma",        Bn = "দাম্মা"      },
                new { DiacriticSignId = G(4), En = "Sukun",        Bn = "সুকুন"       },
                new { DiacriticSignId = G(5), En = "Shaddah",      Bn = "শাদ্দাহ"    },
                new { DiacriticSignId = G(6), En = "Tanween Fath", Bn = "তানউইন ফাতহ" },
                new { DiacriticSignId = G(7), En = "Tanween Kasr", Bn = "তানউইন কাসর" },
                new { DiacriticSignId = G(8), En = "Tanween Damm", Bn = "তানউইন দাম্ম" }
            );
        });

        builder.HasData(
            new DiacriticSign { Id = G(1), Symbol = "َ", SignGroup = "Harakat" },
            new DiacriticSign { Id = G(2), Symbol = "ِ", SignGroup = "Harakat" },
            new DiacriticSign { Id = G(3), Symbol = "ُ", SignGroup = "Harakat" },
            new DiacriticSign { Id = G(4), Symbol = "ْ", SignGroup = "Harakat" },
            new DiacriticSign { Id = G(5), Symbol = "ّ", SignGroup = "Shaddah" },
            new DiacriticSign { Id = G(6), Symbol = "ً", SignGroup = "Tanween" },
            new DiacriticSign { Id = G(7), Symbol = "ٍ", SignGroup = "Tanween" },
            new DiacriticSign { Id = G(8), Symbol = "ٌ", SignGroup = "Tanween" }
        );
    }
}
