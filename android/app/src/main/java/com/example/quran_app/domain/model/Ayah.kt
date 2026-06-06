package com.example.quran_app.domain.model

import com.google.gson.annotations.SerializedName

data class Ayah(
    @SerializedName("id") val id: Int,
    @SerializedName("surahId") val surahId: Int,
    @SerializedName("ayahNumber") val ayahNumber: Int,
    @SerializedName("arabicText") val arabicText: String,
    @SerializedName("englishTranslation") val englishTranslation: String,
    @SerializedName("banglaTranslation") val banglaTranslation: String,
    @SerializedName("transliteration") val transliteration: String,
    @SerializedName("page") val page: Int?,
    @SerializedName("juz") val juz: Int?,
    @SerializedName("manzil") val manzil: Int?,
    @SerializedName("ruku") val ruku: Int?,
    @SerializedName("hizbQuarter") val hizbQuarter: Int?,
    @SerializedName("sajda") val sajda: Boolean?,
    @SerializedName("audioUrl") val audioUrl: String = "",
)
