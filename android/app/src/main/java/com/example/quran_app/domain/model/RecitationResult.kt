package com.example.quran_app.domain.model

data class RecitationResult(
    val recognizedText:    String,
    val pronunciationScore: Double,
    val accuracyScore:     Double,
    val fluencyScore:      Double,
    val completenessScore: Double
)
