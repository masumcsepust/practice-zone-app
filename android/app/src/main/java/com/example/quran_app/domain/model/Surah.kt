package com.example.quran_app.domain.model

import com.google.gson.annotations.SerializedName

data class Surah(
    @SerializedName("id") val id: Int,
    @SerializedName("surahNumber") val surahNumber: Int,
    @SerializedName("nameArabic") val nameArabic: String,
    @SerializedName("nameEnglish") val nameEnglish: String,
    @SerializedName("nameBangla") val nameBangla: String,
    @SerializedName("totalAyahs") val totalAyahs: Int
)
