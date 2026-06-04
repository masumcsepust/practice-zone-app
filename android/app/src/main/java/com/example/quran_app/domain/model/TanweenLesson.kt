package com.example.quran_app.domain.model

import com.google.gson.annotations.SerializedName

data class TanweenLessonResponse(
    @SerializedName("status") val status: String,
    @SerializedName("data")   val data:   TanweenLessonData
)

data class TanweenLessonData(
    @SerializedName("header")          val header:          LessonHeader,
    @SerializedName("concept")         val concept:         LessonConcept,
    @SerializedName("tanweenTypes")    val tanweenTypes:    List<TanweenType>,
    @SerializedName("listenAndCompare")val listenAndCompare:List<ListenCompare>
)

data class LessonHeader(
    @SerializedName("title")              val title:              String,
    @SerializedName("currentLesson")      val currentLesson:      Int,
    @SerializedName("totalLessons")       val totalLessons:       Int,
    @SerializedName("progressPercentage") val progressPercentage: Int
)

data class LessonConcept(
    @SerializedName("titleEn")       val titleEn:       String,
    @SerializedName("titleBn")       val titleBn:       String,
    @SerializedName("descriptionEn") val descriptionEn: String,
    @SerializedName("descriptionBn") val descriptionBn: String,
    @SerializedName("example")       val example:       ConceptExample
)

data class ConceptExample(
    @SerializedName("baseText")   val baseText:   String,
    @SerializedName("resultText") val resultText: String,
    @SerializedName("resultType") val resultType: String,
    @SerializedName("audioUrl")   val audioUrl:   String
)

data class TanweenType(
    @SerializedName("id")           val id:           Int,
    @SerializedName("baseLetter")   val baseLetter:   TanweenLetter,
    @SerializedName("targetLetter") val targetLetter: TanweenLetter
)

data class TanweenLetter(
    @SerializedName("arabic")           val arabic:           String,
    @SerializedName("transliteration")  val transliteration:  String,
    @SerializedName("vowelType")        val vowelType:        String,
    @SerializedName("audioUrl")         val audioUrl:         String
)

data class ListenCompare(
    @SerializedName("label")    val label:    String,
    @SerializedName("audioUrl") val audioUrl: String
)
