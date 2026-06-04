package com.example.quran_app.domain.model

import com.google.gson.annotations.SerializedName

data class PracticeSessionResponse(
    @SerializedName("status") val status: String,
    @SerializedName("data")   val data:   PracticeSessionData
)

data class PracticeListResponse(
    @SerializedName("status") val status: String,
    @SerializedName("data")   val data:   List<PracticeSessionData>
)

data class PracticeSessionData(
    @SerializedName("lesson_id")      val lessonId:      String,
    @SerializedName("title")          val title:         String,
    @SerializedName("sequence_order") val sequenceOrder: Int,
    @SerializedName("practice_items") val practiceItems: List<PracticeItem>
)

data class PracticeItem(
    @SerializedName("id")                    val id:                  String,
    @SerializedName("instruction")           val instruction:         String,
    @SerializedName("target_syllable")       val targetSyllable:      SyllableInfo,
    @SerializedName("compare_with_syllable") val compareWithSyllable: SyllableInfo?,
    @SerializedName("success_tip")           val successTip:          String
)

data class SyllableInfo(
    @SerializedName("combined_character")  val combinedCharacter:  String,
    @SerializedName("transliteration")     val transliteration:    String,
    @SerializedName("transliteration_bn")  val transliterationBn:  String,
    @SerializedName("audio_url")           val audioUrl:           String,
    @SerializedName("details")             val details:            SyllableDetails
)

data class SyllableDetails(
    @SerializedName("letter_name") val letterName: String,
    @SerializedName("sign_name")   val signName:   String,
    @SerializedName("sign_group")  val signGroup:  String
)
