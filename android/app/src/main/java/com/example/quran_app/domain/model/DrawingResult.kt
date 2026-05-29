package com.example.quran_app.domain.model

import com.google.gson.annotations.SerializedName

data class DrawingResult(
    @SerializedName("isCorrect")  val isCorrect:  Boolean,
    @SerializedName("score")      val score:      Int,
    @SerializedName("feedback")   val feedback:   String,
    @SerializedName("feedbackBn") val feedbackBn: String,
    @SerializedName("hint")       val hint:       String,
    @SerializedName("hintBn")     val hintBn:     String
)

sealed class DrawingState {
    object Idle       : DrawingState()
    object Submitting : DrawingState()
    data class Result(val data: DrawingResult) : DrawingState()
    data class Error(val message: String)      : DrawingState()
}

/** Request body for POST /api/arabic-letters/{id}/check-drawing */
data class DrawingCheckRequest(
    @SerializedName("imageBase64") val imageBase64: String
)
