package com.example.quran_app.domain.model

/** Parsed from the backend's  `letter-result`  WebSocket message. */
data class PronunciationResult(
    val isCorrect:      Boolean,
    val score:          Double,   // overall pronunciation score 0-100
    val accuracyScore:  Double,   // phoneme-level accuracy 0-100
    val recognizedText: String,   // what Azure heard
    val feedback:       String,   // English feedback from backend
    val makhrajHint:    String,   // Bengali makhraj hint
    val letter:         String = "",  // target Arabic letter character (e.g. "ا")
    val letterName:     String = ""   // target letter Arabic name (e.g. "أَلِف")
)

/** UI state machine for the বলুন (Speak) tab. */
sealed class SpeakState {
    object Idle       : SpeakState()
    object Recording  : SpeakState()
    object Processing : SpeakState()
    data class Result(val data: PronunciationResult) : SpeakState()
    data class Error(val message: String)            : SpeakState()
}
