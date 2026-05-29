package com.example.quran_app.domain.model

import com.google.gson.annotations.SerializedName

data class Ayah(
    @SerializedName("Id") val id: Int,
    @SerializedName("SurahId") val surahId: Int,
    @SerializedName("AyahNumber") val ayahNumber: Int,
    @SerializedName("ArabicText") val arabicText: String,
    @SerializedName("EnglishTranslation") val englishTranslation: String,
    @SerializedName("BanglaTranslation") val banglaTranslation: String,
    @SerializedName("Transliteration") val transliteration: String
)
