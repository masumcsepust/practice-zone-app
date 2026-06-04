using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class ArabicLetterConfiguration : IEntityTypeConfiguration<ArabicLetter>
{
    private const string T = "ـ";

    private static Guid G(int n) => new($"00000000-0000-0000-0000-{n:D12}");

    private static ArabicLetter MakeLetter(
        int order, string ch, string nameAr,
        string nameEn, string nameBn, string translit,
        string makhrajType, string makhrajEn, string makhrajBn,
        string sifaat, string exAr, string exEn, string exBn,
        bool isConnector)
    {
        return new ArabicLetter
        {
            Id                 = G(order),
            SequenceOrder      = order,
            Character          = ch,
            NameArabic         = nameAr,
            Name               = new LocalizedText { En = nameEn, Bn = nameBn },
            Transliteration    = new LocalizedText { En = translit, Bn = "" },
            MakhrajType        = makhrajType,
            MakhrajDescription = new LocalizedText { En = makhrajEn, Bn = makhrajBn },
            Sifaat             = sifaat,
            ExampleWordArabic  = exAr,
            ExampleWordMeaning = new LocalizedText { En = exEn, Bn = exBn },
            IsolatedForm       = ch,
            InitialForm        = isConnector ? $"{ch}{T}" : ch,
            MedialForm         = isConnector ? $"{T}{ch}{T}" : $"{T}{ch}",
            FinalForm          = $"{T}{ch}",
            IsConnector        = isConnector
        };
    }

    private static readonly IReadOnlyList<ArabicLetter> _letters =
    [
        MakeLetter(1,  "ا", "أَلِف",  "Alif",  "আলিফ",        "ā / ʾ",
            "Throat",             "Deepest part of the throat (Hamza); Alif itself is a vowel carrier",
            "কণ্ঠের সর্বনিম্ন স্থান — হামযার উচ্চারণস্থল",
            "Jahr,Rakhawa,Istifal,Infitah,Idhlag",
            "أَحَد", "One (Ahad)", "এক/একক", false),

        MakeLetter(2,  "ب", "بَاء",   "Ba",    "বা",           "b",
            "Lips",               "Both lips pressed together (bilabial stop)",
            "উভয় ঠোঁট একসাথে চেপে — দুই ঠোঁটের শব্দ",
            "Jahr,Shiddah,Istifal,Infitah,Idhlag,Qalqalah",
            "بَيْت", "House (Bayt)", "বাড়ি/ঘর", true),

        MakeLetter(3,  "ت", "تَاء",   "Ta",    "তা",           "t",
            "TongueTipDental",    "Tip of tongue touches the upper front teeth",
            "জিহ্বার অগ্রভাগ ওপরের সামনের দাঁতে লাগিয়ে",
            "Hams,Shiddah,Istifal,Infitah,Idhlag",
            "تَمْر", "Dates (Tamr)", "খেজুর", true),

        MakeLetter(4,  "ث", "ثَاء",   "Tha",   "ছা",           "th",
            "TongueTipInterdental","Tip of tongue lightly between the upper and lower teeth",
            "জিহ্বার অগ্রভাগ ওপর-নিচের দাঁতের মাঝখানে রেখে",
            "Hams,Rakhawa,Istifal,Infitah,Idhlag",
            "ثَوْب", "Garment (Thawb)", "পোশাক", true),

        MakeLetter(5,  "ج", "جِيم",   "Jim",   "জিম",          "j",
            "TongueFront",        "Middle of the tongue meets the hard palate",
            "জিহ্বার মধ্যভাগ শক্ত তালুর সাথে মিলিয়ে",
            "Jahr,Shiddah,Istifal,Infitah,Idhlag,Qalqalah",
            "جَبَل", "Mountain (Jabal)", "পাহাড়", true),

        MakeLetter(6,  "ح", "حَاء",   "Ha",    "হা",           "ḥ",
            "UpperThroat",        "Middle of the throat — a breathy, voiceless pharyngeal fricative",
            "কণ্ঠের মধ্যস্থান — নিঃশ্বাসের মতো শব্দ, কোনো কম্পন নেই",
            "Hams,Rakhawa,Istifal,Infitah,Idhlag",
            "حَق", "Truth (Haqq)", "সত্য/অধিকার", true),

        MakeLetter(7,  "خ", "خَاء",   "Kha",   "খা",           "kh",
            "UpperThroat",        "Upper throat (closest to mouth) — velar fricative",
            "কণ্ঠের উপরের অংশ — গার্গলিং 'খ' শব্দ",
            "Hams,Rakhawa,Isti'la,Infitah,Idhlag",
            "خَيْر", "Goodness (Khayr)", "কল্যাণ/ভালো", true),

        MakeLetter(8,  "د", "دَال",   "Dal",   "দাল",          "d",
            "TongueTipDental",    "Tip and sides of tongue against the upper front teeth",
            "জিহ্বার অগ্রভাগ ও পার্শ্ব ওপরের সামনের দাঁতে",
            "Jahr,Shiddah,Istifal,Infitah,Idhlag,Qalqalah",
            "دِين", "Religion (Deen)", "ধর্ম", false),

        MakeLetter(9,  "ذ", "ذَال",   "Dhal",  "যাল",          "dh",
            "TongueTipInterdental","Tip of tongue lightly between the teeth (voiced)",
            "জিহ্বার অগ্রভাগ দাঁতের মাঝখানে — কম্পনযুক্ত",
            "Jahr,Rakhawa,Istifal,Infitah,Idhlag",
            "ذِكْر", "Remembrance (Dhikr)", "স্মরণ/যিকর", false),

        MakeLetter(10, "ر", "رَاء",   "Ra",    "রা",           "r",
            "TongueTipTrilled",   "Tip of tongue near the upper gum ridge — trilled 'r'",
            "জিহ্বার অগ্রভাগ ওপরের মাড়ির কাছে — কম্পমান 'র'",
            "Jahr,Tawassut,Istifal,Infitah,Idhlag,Takrir",
            "رَحْمَة", "Mercy (Rahma)", "দয়া/করুণা", false),

        MakeLetter(11, "ز", "زَاي",   "Zay",   "যায়",          "z",
            "TongueTipSibilant",  "Tip of tongue near lower front teeth — voiced sibilant",
            "জিহ্বার অগ্রভাগ নিচের সামনের দাঁতের কাছে — কম্পনযুক্ত",
            "Jahr,Rakhawa,Istifal,Infitah,Idhlag,Safeer",
            "زَيْت", "Oil (Zayt)", "তেল", false),

        MakeLetter(12, "س", "سِين",   "Sin",   "সিন",          "s",
            "TongueTipSibilant",  "Tip of tongue near lower front teeth — voiceless sibilant",
            "জিহ্বার অগ্রভাগ নিচের দাঁতের কাছে — অ-কম্পনযুক্ত 'স'",
            "Hams,Rakhawa,Istifal,Infitah,Idhlag,Safeer",
            "سَلَام", "Peace (Salaam)", "শান্তি", true),

        MakeLetter(13, "ش", "شِين",   "Shin",  "শিন",          "sh",
            "TongueFront",        "Middle of tongue spread toward the hard palate — 'sh' sound",
            "জিহ্বার মধ্যভাগ শক্ত তালুর দিকে ছড়িয়ে — 'শ' শব্দ",
            "Hams,Rakhawa,Istifal,Infitah,Idhlag,Tafasshi",
            "شَمْس", "Sun (Shams)", "সূর্য", true),

        MakeLetter(14, "ص", "صَاد",   "Sad",   "সাদ",          "ṣ",
            "TongueTipSibilant",  "Tip of tongue near front teeth — heavy emphatic 'S'",
            "জিহ্বার অগ্রভাগ সামনের দাঁতের কাছে — ভারী জোরালো 'স'",
            "Hams,Rakhawa,Isti'la,Itbaq,Idhlag,Safeer",
            "صَبْر", "Patience (Sabr)", "ধৈর্য", true),

        MakeLetter(15, "ض", "ضَاد",   "Dad",   "দোয়াদ",        "ḍ",
            "TongueSide",         "One or both sides of tongue against the upper back molars",
            "জিহ্বার এক বা উভয় পার্শ্ব ওপরের পেছনের দাঁতের সাথে",
            "Jahr,Rakhawa,Isti'la,Itbaq,Idhlag",
            "ضَوْء", "Light (Daw')", "আলো", true),

        MakeLetter(16, "ط", "طَاء",   "Taa",   "তোয়া",         "ṭ",
            "TongueTipDental",    "Tip of tongue touches upper front teeth — emphatic heavy 'T'",
            "জিহ্বার অগ্রভাগ ওপরের সামনের দাঁতে — ভারী জোরালো 'ত'",
            "Jahr,Shiddah,Isti'la,Itbaq,Idhlag,Qalqalah",
            "طَرِيق", "Road (Tariq)", "রাস্তা/পথ", true),

        MakeLetter(17, "ظ", "ظَاء",   "Dhaa",  "যোয়া",         "ẓ",
            "TongueTipInterdental","Tip of tongue between teeth — heavy emphatic interdental",
            "জিহ্বার অগ্রভাগ দাঁতের মাঝে — ভারী আন্তর-দন্তীয়",
            "Jahr,Rakhawa,Isti'la,Itbaq,Idhlag",
            "ظُلْم", "Oppression (Dhulm)", "অত্যাচার", true),

        MakeLetter(18, "ع", "عَيْن",  "Ayn",   "আইন",          "ʿ",
            "MidThroat",          "Middle of the throat — voiced pharyngeal fricative",
            "কণ্ঠের মধ্যভাগ — গ্রাসনালীর কম্পনযুক্ত ঘর্ষণ শব্দ",
            "Jahr,Tawassut,Istifal,Infitah,Idhlag",
            "عِلْم", "Knowledge (Ilm)", "জ্ঞান", true),

        MakeLetter(19, "غ", "غَيْن",  "Ghayn", "গাইন",         "gh",
            "MidThroat",          "Upper throat — voiced velar fricative, a gargling 'gh' sound",
            "কণ্ঠের উপরিভাগ — কম্পনযুক্ত 'গ' জাতীয় শব্দ",
            "Jahr,Rakhawa,Isti'la,Infitah,Idhlag",
            "غَيْب", "Unseen (Ghayb)", "অদৃশ্য/গায়েব", true),

        MakeLetter(20, "ف", "فَاء",   "Fa",    "ফা",           "f",
            "Labiodental",        "Inner edge of lower lip touches tips of upper front teeth",
            "নিচের ঠোঁটের ভেতরের অংশ ওপরের সামনের দাঁতের ডগায়",
            "Hams,Rakhawa,Istifal,Infitah,Idhlag",
            "فَجْر", "Dawn (Fajr)", "ভোর/ফজর", true),

        MakeLetter(21, "ق", "قَاف",   "Qaf",   "কাফ",          "q",
            "TongueBack",         "Back of tongue touches the soft palate (uvular stop)",
            "জিহ্বার পশ্চাৎভাগ নরম তালুতে লাগিয়ে — গভীর 'ক' শব্দ",
            "Jahr,Shiddah,Isti'la,Infitah,Ismat,Qalqalah",
            "قُرْآن", "Quran (Quran)", "কুরআন", true),

        MakeLetter(22, "ك", "كَاف",   "Kaf",   "কাফ (ছোট)",    "k",
            "TongueBack",         "Back of tongue touches the hard palate — slightly forward of Qaf",
            "জিহ্বার পশ্চাৎভাগ শক্ত তালুতে — কাফের চেয়ে সামনে",
            "Hams,Shiddah,Istifal,Infitah,Ismat",
            "كِتَاب", "Book (Kitab)", "বই/কিতাব", true),

        MakeLetter(23, "ل", "لَام",   "Lam",   "লাম",          "l",
            "TongueSide",         "Tip and sides of tongue along the upper gum ridge — lateral",
            "জিহ্বার অগ্রভাগ ও পার্শ্ব ওপরের মাড়ির পাশে — পার্শ্বীয় শব্দ",
            "Jahr,Tawassut,Istifal,Infitah,Idhlag,Inhiraf",
            "لَيْل", "Night (Layl)", "রাত", true),

        MakeLetter(24, "م", "مِيم",   "Mim",   "মিম",          "m",
            "Lips",               "Both lips closed together — bilabial nasal",
            "উভয় ঠোঁট বন্ধ রেখে — নাসিক শব্দ (মুখ বন্ধ, নাক দিয়ে বাতাস)",
            "Jahr,Tawassut,Istifal,Infitah,Idhlag",
            "مَاء", "Water (Maa')", "পানি", true),

        MakeLetter(25, "ن", "نُون",   "Nun",   "নুন",          "n",
            "TongueTipAlveolar",  "Tip of tongue near the upper gum ridge — alveolar nasal",
            "জিহ্বার অগ্রভাগ ওপরের মাড়ির কাছে — অনুনাসিক শব্দ",
            "Jahr,Tawassut,Istifal,Infitah,Idhlag,Ghunnah",
            "نُور", "Light (Nur)", "আলো", true),

        MakeLetter(26, "و", "وَاو",   "Waw",   "ওয়াও",         "w / ū",
            "Lips",               "Both lips rounded and slightly apart — labio-velar semi-vowel",
            "উভয় ঠোঁট গোলাকারভাবে সামান্য ফাঁক রেখে — অর্ধ-স্বরধ্বনি",
            "Jahr,Rakhawa,Istifal,Infitah,Idhlag,Lin",
            "وَلَد", "Child (Walad)", "সন্তান", false),

        MakeLetter(27, "ه", "هَاء",   "Ha",    "হা (গোল হা)",  "h",
            "Throat",             "Deepest part of the throat — soft, breathy voiceless glottal fricative",
            "কণ্ঠের সর্বনিম্ন স্থান — শীতল নিঃশ্বাসের মতো শব্দ",
            "Hams,Rakhawa,Istifal,Infitah,Idhlag",
            "هُدَى", "Guidance (Huda)", "পথনির্দেশনা", true),

        MakeLetter(28, "ي", "يَاء",   "Ya",    "ইয়া",          "y / ī",
            "TongueFront",        "Middle of tongue rises toward the hard palate — palatal semi-vowel",
            "জিহ্বার মধ্যভাগ শক্ত তালুর দিকে উঠিয়ে — তালব্য অর্ধ-স্বর",
            "Jahr,Rakhawa,Istifal,Infitah,Idhlag,Lin",
            "يَوْم", "Day (Yawm)", "দিন", true),
    ];

    public void Configure(EntityTypeBuilder<ArabicLetter> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Character).IsRequired().HasMaxLength(10);
        builder.Property(l => l.NameArabic).IsRequired().HasMaxLength(50);
        builder.Property(l => l.MakhrajType).IsRequired().HasMaxLength(50);
        builder.Property(l => l.Sifaat).HasMaxLength(500);
        builder.Property(l => l.ExampleWordArabic).HasMaxLength(100);
        builder.Property(l => l.IsolatedForm).HasMaxLength(20);
        builder.Property(l => l.InitialForm).HasMaxLength(20);
        builder.Property(l => l.MedialForm).HasMaxLength(20);
        builder.Property(l => l.FinalForm).HasMaxLength(20);
        builder.HasIndex(l => l.SequenceOrder).IsUnique();
        builder.ToTable("ArabicLetters");

        builder.OwnsOne(l => l.Name, nb =>
        {
            nb.Property(n => n.En).HasColumnName("NameEn").HasMaxLength(50);
            nb.Property(n => n.Bn).HasColumnName("NameBn").HasMaxLength(100);
            nb.HasData(_letters.Select(l => new { ArabicLetterId = l.Id, En = l.Name.En, Bn = l.Name.Bn }));
        });

        builder.OwnsOne(l => l.Transliteration, nb =>
        {
            nb.Property(n => n.En).HasColumnName("TransliterationEn").HasMaxLength(20);
            nb.Property(n => n.Bn).HasColumnName("TransliterationBn").HasMaxLength(20);
            nb.HasData(_letters.Select(l => new { ArabicLetterId = l.Id, En = l.Transliteration.En, Bn = l.Transliteration.Bn }));
        });

        builder.OwnsOne(l => l.MakhrajDescription, nb =>
        {
            nb.Property(n => n.En).HasColumnName("MakhrajDescriptionEn").HasMaxLength(300);
            nb.Property(n => n.Bn).HasColumnName("MakhrajDescriptionBn").HasMaxLength(300);
            nb.HasData(_letters.Select(l => new { ArabicLetterId = l.Id, En = l.MakhrajDescription.En, Bn = l.MakhrajDescription.Bn }));
        });

        builder.OwnsOne(l => l.ExampleWordMeaning, nb =>
        {
            nb.Property(n => n.En).HasColumnName("ExampleWordEn").HasMaxLength(100);
            nb.Property(n => n.Bn).HasColumnName("ExampleWordBn").HasMaxLength(100);
            nb.HasData(_letters.Select(l => new { ArabicLetterId = l.Id, En = l.ExampleWordMeaning.En, Bn = l.ExampleWordMeaning.Bn }));
        });

        builder.HasData(_letters.Select(l => new ArabicLetter
        {
            Id                = l.Id,
            SequenceOrder     = l.SequenceOrder,
            Character         = l.Character,
            NameArabic        = l.NameArabic,
            MakhrajType       = l.MakhrajType,
            Sifaat            = l.Sifaat,
            ExampleWordArabic = l.ExampleWordArabic,
            IsolatedForm      = l.IsolatedForm,
            InitialForm       = l.InitialForm,
            MedialForm        = l.MedialForm,
            FinalForm         = l.FinalForm,
            IsConnector       = l.IsConnector
        }));
    }
}
