using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Persistence.Configurations;

public class ArabicLetterConfiguration : IEntityTypeConfiguration<ArabicLetter>
{
    private const string T = "ـ"; // Arabic Tatweel

    private static ArabicLetter MakeLetter(
        int id, int order, string letter, string nameEn, string nameAr, string nameBn,
        string translit, string makhrajType, string makhrajDesc, string makhrajDescBn,
        string sifaat, string exAr, string exEn, string exBn, bool isConnector)
    {
        string isolated = letter;
        string initial  = isConnector ? $"{letter}{T}" : letter;
        string medial   = isConnector ? $"{T}{letter}{T}" : $"{T}{letter}";
        string final_   = $"{T}{letter}";
        return new ArabicLetter
        {
            Id = id, Order = order, Letter = letter,
            NameEnglish = nameEn, NameArabic = nameAr, NameBangla = nameBn,
            Transliteration = translit,
            MakhrajType = makhrajType,
            MakhrajDescription = makhrajDesc,
            MakhrajDescriptionBn = makhrajDescBn,
            Sifaat = sifaat,
            ExampleWordArabic = exAr, ExampleWord = exEn, ExampleWordBn = exBn,
            IsolatedForm = isolated, InitialForm = initial, MedialForm = medial, FinalForm = final_,
            IsConnector = isConnector
        };
    }

    public void Configure(EntityTypeBuilder<ArabicLetter> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Letter).IsRequired().HasMaxLength(10);
        builder.Property(l => l.NameEnglish).IsRequired().HasMaxLength(50);
        builder.Property(l => l.NameArabic).IsRequired().HasMaxLength(50);
        builder.Property(l => l.NameBangla).HasMaxLength(100);
        builder.Property(l => l.Transliteration).HasMaxLength(20);
        builder.Property(l => l.MakhrajType).IsRequired().HasMaxLength(50);
        builder.Property(l => l.MakhrajDescription).HasMaxLength(300);
        builder.Property(l => l.MakhrajDescriptionBn).HasMaxLength(300);
        builder.Property(l => l.Sifaat).HasMaxLength(500);
        builder.Property(l => l.ExampleWordArabic).HasMaxLength(100);
        builder.Property(l => l.ExampleWord).HasMaxLength(100);
        builder.Property(l => l.ExampleWordBn).HasMaxLength(100);
        builder.Property(l => l.IsolatedForm).HasMaxLength(20);
        builder.Property(l => l.InitialForm).HasMaxLength(20);
        builder.Property(l => l.MedialForm).HasMaxLength(20);
        builder.Property(l => l.FinalForm).HasMaxLength(20);
        builder.HasIndex(l => l.Order).IsUnique();
        builder.ToTable("ArabicLetters");

        builder.HasData(
            MakeLetter(1, 1, "ا", "Alif", "أَلِف", "আলিফ", "ā / ʾ",
                "Throat", "Deepest part of the throat (Hamza); Alif itself is a vowel carrier",
                "কণ্ঠের সর্বনিম্ন স্থান — হামযার উচ্চারণস্থল",
                "Jahr,Rakhawa,Istifal,Infitah,Idhlag",
                "أَحَد", "One (Ahad)", "এক/একক", isConnector: false),

            MakeLetter(2, 2, "ب", "Ba", "بَاء", "বা", "b",
                "Lips", "Both lips pressed together (bilabial stop)",
                "উভয় ঠোঁট একসাথে চেপে — দুই ঠোঁটের শব্দ",
                "Jahr,Shiddah,Istifal,Infitah,Idhlag,Qalqalah",
                "بَيْت", "House (Bayt)", "বাড়ি/ঘর", isConnector: true),

            MakeLetter(3, 3, "ت", "Ta", "تَاء", "তা", "t",
                "TongueTipDental", "Tip of tongue touches the upper front teeth",
                "জিহ্বার অগ্রভাগ ওপরের সামনের দাঁতে লাগিয়ে",
                "Hams,Shiddah,Istifal,Infitah,Idhlag",
                "تَمْر", "Dates (Tamr)", "খেজুর", isConnector: true),

            MakeLetter(4, 4, "ث", "Tha", "ثَاء", "ছা", "th",
                "TongueTipInterdental", "Tip of tongue lightly between the upper and lower teeth",
                "জিহ্বার অগ্রভাগ ওপর-নিচের দাঁতের মাঝখানে রেখে",
                "Hams,Rakhawa,Istifal,Infitah,Idhlag",
                "ثَوْب", "Garment (Thawb)", "পোশাক", isConnector: true),

            MakeLetter(5, 5, "ج", "Jim", "جِيم", "জিম", "j",
                "TongueFront", "Middle of the tongue meets the hard palate",
                "জিহ্বার মধ্যভাগ শক্ত তালুর সাথে মিলিয়ে",
                "Jahr,Shiddah,Istifal,Infitah,Idhlag,Qalqalah",
                "جَبَل", "Mountain (Jabal)", "পাহাড়", isConnector: true),

            MakeLetter(6, 6, "ح", "Ha", "حَاء", "হা", "ḥ",
                "UpperThroat", "Middle of the throat — a breathy, voiceless pharyngeal fricative",
                "কণ্ঠের মধ্যস্থান — নিঃশ্বাসের মতো শব্দ, কোনো কম্পন নেই",
                "Hams,Rakhawa,Istifal,Infitah,Idhlag",
                "حَق", "Truth (Haqq)", "সত্য/অধিকার", isConnector: true),

            MakeLetter(7, 7, "خ", "Kha", "خَاء", "খা", "kh",
                "UpperThroat", "Upper throat (closest to mouth) — velar fricative",
                "কণ্ঠের উপরের অংশ — গার্গলিং 'খ' শব্দ",
                "Hams,Rakhawa,Isti'la,Infitah,Idhlag",
                "خَيْر", "Goodness (Khayr)", "কল্যাণ/ভালো", isConnector: true),

            MakeLetter(8, 8, "د", "Dal", "دَال", "দাল", "d",
                "TongueTipDental", "Tip and sides of tongue against the upper front teeth",
                "জিহ্বার অগ্রভাগ ও পার্শ্ব ওপরের সামনের দাঁতে",
                "Jahr,Shiddah,Istifal,Infitah,Idhlag,Qalqalah",
                "دِين", "Religion (Deen)", "ধর্ম", isConnector: false),

            MakeLetter(9, 9, "ذ", "Dhal", "ذَال", "যাল", "dh",
                "TongueTipInterdental", "Tip of tongue lightly between the teeth (voiced)",
                "জিহ্বার অগ্রভাগ দাঁতের মাঝখানে — কম্পনযুক্ত",
                "Jahr,Rakhawa,Istifal,Infitah,Idhlag",
                "ذِكْر", "Remembrance (Dhikr)", "স্মরণ/যিকর", isConnector: false),

            MakeLetter(10, 10, "ر", "Ra", "رَاء", "রা", "r",
                "TongueTipTrilled", "Tip of tongue near the upper gum ridge — trilled 'r'",
                "জিহ্বার অগ্রভাগ ওপরের মাড়ির কাছে — কম্পমান 'র'",
                "Jahr,Tawassut,Istifal,Infitah,Idhlag,Takrir",
                "رَحْمَة", "Mercy (Rahma)", "দয়া/করুণা", isConnector: false),

            MakeLetter(11, 11, "ز", "Zay", "زَاي", "যায়", "z",
                "TongueTipSibilant", "Tip of tongue near lower front teeth — voiced sibilant",
                "জিহ্বার অগ্রভাগ নিচের সামনের দাঁতের কাছে — কম্পনযুক্ত",
                "Jahr,Rakhawa,Istifal,Infitah,Idhlag,Safeer",
                "زَيْت", "Oil (Zayt)", "তেল", isConnector: false),

            MakeLetter(12, 12, "س", "Sin", "سِين", "সিন", "s",
                "TongueTipSibilant", "Tip of tongue near lower front teeth — voiceless sibilant",
                "জিহ্বার অগ্রভাগ নিচের দাঁতের কাছে — অ-কম্পনযুক্ত 'স'",
                "Hams,Rakhawa,Istifal,Infitah,Idhlag,Safeer",
                "سَلَام", "Peace (Salaam)", "শান্তি", isConnector: true),

            MakeLetter(13, 13, "ش", "Shin", "شِين", "শিন", "sh",
                "TongueFront", "Middle of tongue spread toward the hard palate — 'sh' sound",
                "জিহ্বার মধ্যভাগ শক্ত তালুর দিকে ছড়িয়ে — 'শ' শব্দ",
                "Hams,Rakhawa,Istifal,Infitah,Idhlag,Tafasshi",
                "شَمْس", "Sun (Shams)", "সূর্য", isConnector: true),

            MakeLetter(14, 14, "ص", "Sad", "صَاد", "সাদ", "ṣ",
                "TongueTipSibilant", "Tip of tongue near front teeth — heavy emphatic 'S'",
                "জিহ্বার অগ্রভাগ সামনের দাঁতের কাছে — ভারী জোরালো 'স'",
                "Hams,Rakhawa,Isti'la,Itbaq,Idhlag,Safeer",
                "صَبْر", "Patience (Sabr)", "ধৈর্য", isConnector: true),

            MakeLetter(15, 15, "ض", "Dad", "ضَاد", "দোয়াদ", "ḍ",
                "TongueSide", "One or both sides of tongue against the upper back molars",
                "জিহ্বার এক বা উভয় পার্শ্ব ওপরের পেছনের দাঁতের সাথে",
                "Jahr,Rakhawa,Isti'la,Itbaq,Idhlag",
                "ضَوْء", "Light (Daw')", "আলো", isConnector: true),

            MakeLetter(16, 16, "ط", "Taa", "طَاء", "তোয়া", "ṭ",
                "TongueTipDental", "Tip of tongue touches upper front teeth — emphatic heavy 'T'",
                "জিহ্বার অগ্রভাগ ওপরের সামনের দাঁতে — ভারী জোরালো 'ত'",
                "Jahr,Shiddah,Isti'la,Itbaq,Idhlag,Qalqalah",
                "طَرِيق", "Road (Tariq)", "রাস্তা/পথ", isConnector: true),

            MakeLetter(17, 17, "ظ", "Dhaa", "ظَاء", "যোয়া", "ẓ",
                "TongueTipInterdental", "Tip of tongue between teeth — heavy emphatic interdental",
                "জিহ্বার অগ্রভাগ দাঁতের মাঝে — ভারী আন্তর-দন্তীয়",
                "Jahr,Rakhawa,Isti'la,Itbaq,Idhlag",
                "ظُلْم", "Oppression (Dhulm)", "অত্যাচার", isConnector: true),

            MakeLetter(18, 18, "ع", "Ayn", "عَيْن", "আইন", "ʿ",
                "MidThroat", "Middle of the throat — voiced pharyngeal fricative",
                "কণ্ঠের মধ্যভাগ — গ্রাসনালীর কম্পনযুক্ত ঘর্ষণ শব্দ",
                "Jahr,Tawassut,Istifal,Infitah,Idhlag",
                "عِلْم", "Knowledge (Ilm)", "জ্ঞান", isConnector: true),

            MakeLetter(19, 19, "غ", "Ghayn", "غَيْن", "গাইন", "gh",
                "MidThroat", "Upper throat — voiced velar fricative, a gargling 'gh' sound",
                "কণ্ঠের উপরিভাগ — কম্পনযুক্ত 'গ' জাতীয় শব্দ",
                "Jahr,Rakhawa,Isti'la,Infitah,Idhlag",
                "غَيْب", "Unseen (Ghayb)", "অদৃশ্য/গায়েব", isConnector: true),

            MakeLetter(20, 20, "ف", "Fa", "فَاء", "ফা", "f",
                "Labiodental", "Inner edge of lower lip touches tips of upper front teeth",
                "নিচের ঠোঁটের ভেতরের অংশ ওপরের সামনের দাঁতের ডগায়",
                "Hams,Rakhawa,Istifal,Infitah,Idhlag",
                "فَجْر", "Dawn (Fajr)", "ভোর/ফজর", isConnector: true),

            MakeLetter(21, 21, "ق", "Qaf", "قَاف", "কাফ", "q",
                "TongueBack", "Back of tongue touches the soft palate (uvular stop)",
                "জিহ্বার পশ্চাৎভাগ নরম তালুতে লাগিয়ে — গভীর 'ক' শব্দ",
                "Jahr,Shiddah,Isti'la,Infitah,Ismat,Qalqalah",
                "قُرْآن", "Quran (Quran)", "কুরআন", isConnector: true),

            MakeLetter(22, 22, "ك", "Kaf", "كَاف", "কাফ (ছোট)", "k",
                "TongueBack", "Back of tongue touches the hard palate — slightly forward of Qaf",
                "জিহ্বার পশ্চাৎভাগ শক্ত তালুতে — কাফের চেয়ে সামনে",
                "Hams,Shiddah,Istifal,Infitah,Ismat",
                "كِتَاب", "Book (Kitab)", "বই/কিতাব", isConnector: true),

            MakeLetter(23, 23, "ل", "Lam", "لَام", "লাম", "l",
                "TongueSide", "Tip and sides of tongue along the upper gum ridge — lateral",
                "জিহ্বার অগ্রভাগ ও পার্শ্ব ওপরের মাড়ির পাশে — পার্শ্বীয় শব্দ",
                "Jahr,Tawassut,Istifal,Infitah,Idhlag,Inhiraf",
                "لَيْل", "Night (Layl)", "রাত", isConnector: true),

            MakeLetter(24, 24, "م", "Mim", "مِيم", "মিম", "m",
                "Lips", "Both lips closed together — bilabial nasal",
                "উভয় ঠোঁট বন্ধ রেখে — নাসিক শব্দ (মুখ বন্ধ, নাক দিয়ে বাতাস)",
                "Jahr,Tawassut,Istifal,Infitah,Idhlag",
                "مَاء", "Water (Maa')", "পানি", isConnector: true),

            MakeLetter(25, 25, "ن", "Nun", "نُون", "নুন", "n",
                "TongueTipAlveolar", "Tip of tongue near the upper gum ridge — alveolar nasal",
                "জিহ্বার অগ্রভাগ ওপরের মাড়ির কাছে — অনুনাসিক শব্দ",
                "Jahr,Tawassut,Istifal,Infitah,Idhlag,Ghunnah",
                "نُور", "Light (Nur)", "আলো", isConnector: true),

            MakeLetter(26, 26, "و", "Waw", "وَاو", "ওয়াও", "w / ū",
                "Lips", "Both lips rounded and slightly apart — labio-velar semi-vowel",
                "উভয় ঠোঁট গোলাকারভাবে সামান্য ফাঁক রেখে — অর্ধ-স্বরধ্বনি",
                "Jahr,Rakhawa,Istifal,Infitah,Idhlag,Lin",
                "وَلَد", "Child (Walad)", "সন্তান", isConnector: false),

            MakeLetter(27, 27, "ه", "Ha", "هَاء", "হা (গোল হা)", "h",
                "Throat", "Deepest part of the throat — soft, breathy voiceless glottal fricative",
                "কণ্ঠের সর্বনিম্ন স্থান — শীতল নিঃশ্বাসের মতো শব্দ",
                "Hams,Rakhawa,Istifal,Infitah,Idhlag",
                "هُدَى", "Guidance (Huda)", "পথনির্দেশনা", isConnector: true),

            MakeLetter(28, 28, "ي", "Ya", "يَاء", "ইয়া", "y / ī",
                "TongueFront", "Middle of tongue rises toward the hard palate — palatal semi-vowel",
                "জিহ্বার মধ্যভাগ শক্ত তালুর দিকে উঠিয়ে — তালব্য অর্ধ-স্বর",
                "Jahr,Rakhawa,Istifal,Infitah,Idhlag,Lin",
                "يَوْم", "Day (Yawm)", "দিন", isConnector: true)
        );
    }
}
