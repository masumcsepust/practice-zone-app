package com.example.quran_app.data.analyzer

import com.example.quran_app.domain.model.RecitationResult

interface RecitationAnalyzer {
    suspend fun analyze(
        pcm:        ByteArray,
        surahId:    Int,
        ayahRange:  IntRange,
    ): RecitationResult
}
