package com.example.quran_app.data.analyzer

import com.example.quran_app.domain.model.Note
import com.example.quran_app.domain.model.NoteKind
import com.example.quran_app.domain.model.RecitationResult
import kotlinx.coroutines.delay

class FakeAnalyzer : RecitationAnalyzer {

    override suspend fun analyze(
        pcm:       ByteArray,
        surahId:   Int,
        ayahRange: IntRange,
    ): RecitationResult {
        delay(1_800) // simulate network / model latency
        // TODO: replace with real model call — POST pcm to /api/recitation/analyze
        return RecitationResult(
            overallTajweed = 88,
            lettersPct     = 94,
            pacePct        = 1.0f,
            notes = listOf(
                Note(
                    kind              = NoteKind.QALQALAH,
                    arabic            = "قْ",
                    title             = "Qalqalah on Qāf",
                    tip               = "Apply a slight bounce after ق when sukūn. Avoid prolonging the echo.",
                    isPositive        = false,
                    referenceAudioUrl = null,
                ),
                Note(
                    kind              = NoteKind.MADD,
                    arabic            = "الرَّحْمَٰنِ",
                    title             = "Madd in Ar-Raḥmān",
                    tip               = "Extend the alif for 4–5 counts (madd munfaṣil). You shortened it slightly.",
                    isPositive        = false,
                    referenceAudioUrl = null,
                ),
                Note(
                    kind              = NoteKind.GHUNNAH,
                    arabic            = "ٱلنُّون",
                    title             = "Ghunnah — well done",
                    tip               = "Your nasal resonance on nūn was clear and perfectly timed. Keep it up.",
                    isPositive        = true,
                    referenceAudioUrl = null,
                ),
            ),
        )
    }
}
