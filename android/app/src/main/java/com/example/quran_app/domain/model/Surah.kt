package com.example.quran_app.domain.model

import com.google.gson.annotations.SerializedName

data class Surah(
    @SerializedName("Id") val id: Int,
    @SerializedName("SurahNumber") val surahNumber: Int,
    @SerializedName("NameArabic") val nameArabic: String,
    @SerializedName("NameEnglish") val nameEnglish: String,
    @SerializedName("NameBangla") val nameBangla: String,
    @SerializedName("TotalAyahs") val totalAyahs: Int
)
