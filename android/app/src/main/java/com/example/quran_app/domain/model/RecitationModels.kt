package com.example.quran_app.domain.model

// ── Note kind ─────────────────────────────────────────────────────────────────
enum class NoteKind { QALQALAH, MADD, GHUNNAH, IDGHAAM, IKHFAA, POSITIVE }

// ── API → local model mappers ─────────────────────────────────────────────────
fun Surah.toSurahDef(ayat: List<Ayah> = emptyList()) = SurahDef(
    id                 = surahNumber,
    arabicName         = nameArabic,
    transliteratedName = nameEnglish,            // Al-Quran Cloud englishName = transliteration
    englishName        = nameBangla.ifEmpty { nameEnglish }, // nameBangla stores englishNameTranslation
    ayat               = ayat.map { it.toAyahDef() },
)

fun Ayah.toAyahDef() = AyahDef(
    number          = ayahNumber,
    arabic          = arabicText,
    transliteration = transliteration.ifEmpty { englishTranslation },
    audioUrl        = audioUrl,
)

// ── Core result models ────────────────────────────────────────────────────────
data class RecitationResult(
    val overallTajweed: Int,
    val lettersPct:     Int,
    val pacePct:        Float,
    val notes:          List<Note>,
)

data class Note(
    val kind:              NoteKind,
    val arabic:            String,
    val title:             String,
    val tip:               String,
    val isPositive:        Boolean,
    val referenceAudioUrl: String?,
)

// ── Surah / Ayah definitions ──────────────────────────────────────────────────
data class SurahDef(
    val id:                 Int,
    val arabicName:         String,
    val transliteratedName: String,
    val englishName:        String,
    val ayat:               List<AyahDef>,
)

data class AyahDef(
    val number:          Int,
    val arabic:          String,
    val transliteration: String,
    val audioUrl:        String = "",
)
