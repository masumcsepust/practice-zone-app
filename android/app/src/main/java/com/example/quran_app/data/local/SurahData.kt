package com.example.quran_app.data.local

import com.example.quran_app.domain.model.AyahDef
import com.example.quran_app.domain.model.SurahDef

val SURAHS: List<SurahDef> = listOf(

    SurahDef(
        id = 1,
        arabicName = "ٱلْفَاتِحَة",
        transliteratedName = "Al-Fātiḥah",
        englishName = "The Opening",
        ayat = listOf(
            AyahDef(1, "بِسْمِ ٱللَّهِ ٱلرَّحْمَٰنِ ٱلرَّحِيمِ",
                "Bismillāhi r-raḥmāni r-raḥīm"),
            AyahDef(2, "ٱلْحَمْدُ لِلَّهِ رَبِّ ٱلْعَٰلَمِينَ",
                "Al-ḥamdu lillāhi rabbi l-ʿālamīn"),
            AyahDef(3, "ٱلرَّحْمَٰنِ ٱلرَّحِيمِ",
                "Ar-raḥmāni r-raḥīm"),
        )
    ),

    SurahDef(
        id = 112,
        arabicName = "ٱلْإِخْلَاص",
        transliteratedName = "Al-Ikhlāṣ",
        englishName = "Sincerity",
        ayat = listOf(
            AyahDef(1, "قُلْ هُوَ ٱللَّهُ أَحَدٌ",
                "Qul huwa llāhu aḥad"),
            AyahDef(2, "ٱللَّهُ ٱلصَّمَدُ",
                "Allāhu ṣ-ṣamad"),
            AyahDef(3, "لَمْ يَلِدْ وَلَمْ يُولَدْ",
                "Lam yalid wa lam yūlad"),
        )
    ),

    SurahDef(
        id = 114,
        arabicName = "ٱلنَّاس",
        transliteratedName = "An-Nās",
        englishName = "Mankind",
        ayat = listOf(
            AyahDef(1, "قُلْ أَعُوذُ بِرَبِّ ٱلنَّاسِ",
                "Qul aʿūdhu bi-rabbi n-nās"),
            AyahDef(2, "مَلِكِ ٱلنَّاسِ",
                "Maliki n-nās"),
            AyahDef(3, "إِلَٰهِ ٱلنَّاسِ",
                "Ilāhi n-nās"),
        )
    ),
)
