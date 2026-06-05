package com.example.quran_app.domain.model

import com.google.gson.annotations.SerializedName

data class PracticeStepResponse(
    @SerializedName("id")           val id:           Int,
    @SerializedName("title")        val title:        String,
    @SerializedName("lessonNum")    val lessonNum:     Int,
    @SerializedName("progress")     val progress:      Int,
    @SerializedName("tips")         val tips:          String,
    @SerializedName("left")         val left:          StepSyllable,
    @SerializedName("right")        val right:         StepSyllable?,
    @SerializedName("rememberText") val rememberText:  String,
    @SerializedName("totalLessons") val totalLessons:  Int,
    @SerializedName("totalSteps")   val totalSteps:    Int,
)

data class StepSyllable(
    @SerializedName("arabic")        val arabic:        String,
    @SerializedName("translit")      val translit:      String,
    @SerializedName("bengali")       val bengali:       String,
    @SerializedName("prompt")        val prompt:        String,
    @SerializedName("letterBengali") val letterBengali: String,
    @SerializedName("signName")      val signName:      String,
    @SerializedName("signGroup")     val signGroup:     String,
    @SerializedName("audioUrl")      val audioUrl:      String = "",
)
