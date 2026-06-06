package com.example.quran_app.domain.model

import com.example.quran_app.R

data class DailyPrayer(
    val id: String,
    val nameEn: String,
    val nameAr: String,
    val subtitleEn: String,
    val rakahCount: Int,
    val isLearned: Boolean = false,
)

data class PrayerStep(
    val index: Int,
    val nameAr: String,
    val nameEn: String,
    val subtitleEn: String,
    val recitationAr: String,
    val recitationTranslit: String,
    val recitationMeaning: String,
    val repeatCount: Int,
    val postureRes: Int,
    val audioRes: Int?,  // TODO: add audio clips to res/raw/
)

val SALAH_STEPS: List<PrayerStep> = listOf(
    PrayerStep(
        index = 1,
        nameAr = "تَكْبِيرَةُ الإِحْرَام",
        nameEn = "Takbīr",
        subtitleEn = "Opening",
        recitationAr = "ٱللَّهُ أَكْبَر",
        recitationTranslit = "Allāhu Akbar",
        recitationMeaning = "Allah is the Greatest",
        repeatCount = 1,
        postureRes = R.drawable.posture_takbir,
        audioRes = null,  // TODO: add res/raw/audio_takbir
    ),
    PrayerStep(
        index = 2,
        nameAr = "ٱلْقِيَام",
        nameEn = "Qiyām",
        subtitleEn = "Standing",
        recitationAr = "بِسْمِ ٱللَّهِ ٱلرَّحْمٰنِ ٱلرَّحِيمِ — ٱلْحَمْدُ لِلَّهِ رَبِّ ٱلْعٰلَمِينَ",
        recitationTranslit = "Recite Al-Fātiḥah",
        recitationMeaning = "Opening chapter of the Qur'an",
        repeatCount = 1,
        postureRes = R.drawable.posture_qiyam,
        audioRes = null,  // TODO: add res/raw/audio_fatiha
    ),
    PrayerStep(
        index = 3,
        nameAr = "ٱلرُّكُوع",
        nameEn = "Rukūʿ",
        subtitleEn = "Bowing",
        recitationAr = "سُبْحَانَ رَبِّيَ ٱلْعَظِيم",
        recitationTranslit = "Subḥāna Rabbiyal-ʿAẓīm",
        recitationMeaning = "Glory to my Lord, the Most Great",
        repeatCount = 3,
        postureRes = R.drawable.posture_ruku,
        audioRes = null,  // TODO: add res/raw/audio_ruku
    ),
    PrayerStep(
        index = 4,
        nameAr = "ٱلِاعْتِدَال",
        nameEn = "Iʿtidāl",
        subtitleEn = "Rising",
        recitationAr = "سَمِعَ ٱللَّهُ لِمَنْ حَمِدَهُ، رَبَّنَا وَلَكَ ٱلْحَمْد",
        recitationTranslit = "Samiʿa-llāhu liman ḥamidah",
        recitationMeaning = "Allah hears whoever praises Him",
        repeatCount = 1,
        postureRes = R.drawable.posture_itidal,
        audioRes = null,  // TODO: add res/raw/audio_itidal
    ),
    PrayerStep(
        index = 5,
        nameAr = "ٱلسُّجُود",
        nameEn = "Sujūd",
        subtitleEn = "Prostration",
        recitationAr = "سُبْحَانَ رَبِّيَ ٱلأَعْلَى",
        recitationTranslit = "Subḥāna Rabbiyal-Aʿlā",
        recitationMeaning = "Glory to my Lord, the Most High",
        repeatCount = 3,
        postureRes = R.drawable.posture_sujud,
        audioRes = null,  // TODO: add res/raw/audio_sujud
    ),
    PrayerStep(
        index = 6,
        nameAr = "ٱلْجَلْسَة",
        nameEn = "Jalsa",
        subtitleEn = "Sitting",
        recitationAr = "رَبِّ اغْفِرْ لِي",
        recitationTranslit = "Rabbighfir lī",
        recitationMeaning = "My Lord, forgive me",
        repeatCount = 1,
        postureRes = R.drawable.posture_jalsa,
        audioRes = null,  // TODO: add res/raw/audio_jalsa
    ),
    PrayerStep(
        index = 7,
        nameAr = "ٱلسُّجُود ٱلثَّانِي",
        nameEn = "Second Sujūd",
        subtitleEn = "2nd Prostration",
        recitationAr = "سُبْحَانَ رَبِّيَ ٱلأَعْلَى",
        recitationTranslit = "Subḥāna Rabbiyal-Aʿlā",
        recitationMeaning = "Glory to my Lord, the Most High",
        repeatCount = 3,
        postureRes = R.drawable.posture_sujud,
        audioRes = null,  // TODO: add res/raw/audio_sujud (same as step 5)
    ),
    PrayerStep(
        index = 8,
        nameAr = "ٱلتَّشَهُّد",
        nameEn = "Tashahhud",
        subtitleEn = "Sitting testimony",
        recitationAr = "ٱلتَّحِيَّاتُ لِلَّهِ وَٱلصَّلَوَاتُ وَٱلطَّيِّبَات",
        recitationTranslit = "At-taḥiyyātu lillāh…",
        recitationMeaning = "All greetings are for Allah…",
        repeatCount = 1,
        postureRes = R.drawable.posture_tashahud,
        audioRes = null,  // TODO: add res/raw/audio_tashahud
    ),
    PrayerStep(
        index = 9,
        nameAr = "ٱلسَّلَام",
        nameEn = "Salām",
        subtitleEn = "Closing",
        recitationAr = "ٱلسَّلَامُ عَلَيْكُمْ وَرَحْمَةُ ٱللَّه",
        recitationTranslit = "As-salāmu ʿalaykum wa raḥmatullāh",
        recitationMeaning = "Peace and mercy of Allah be upon you",
        repeatCount = 1,
        postureRes = R.drawable.posture_salam,
        audioRes = null,  // TODO: add res/raw/audio_salam
    ),
)

val DAILY_PRAYERS = listOf(
    DailyPrayer("fajr",    "Fajr",    "الفجر", "Dawn · before sunrise", 2),
    DailyPrayer("dhuhr",   "Dhuhr",   "الظهر", "Midday",                4),
    DailyPrayer("asr",     "ʿAsr",    "العصر", "Afternoon",             4),
    DailyPrayer("maghrib", "Maghrib", "المغرب","After sunset",          3),
    DailyPrayer("isha",    "ʿIshā",   "العشاء","Night",                 4),
)
