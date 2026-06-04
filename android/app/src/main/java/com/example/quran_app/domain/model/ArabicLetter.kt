package com.example.quran_app.domain.model

import com.google.gson.annotations.SerializedName

data class ArabicLetter(
    @SerializedName("id")                  val id: String,
    @SerializedName("order")               val order: Int,
    @SerializedName("letter")              val letter: String,
    @SerializedName("nameEnglish")         val nameEnglish: String,
    @SerializedName("nameArabic")          val nameArabic: String,
    @SerializedName("nameBangla")          val nameBangla: String,
    @SerializedName("transliteration")     val transliteration: String,
    @SerializedName("makhrajType")         val makhrajType: String,
    @SerializedName("makhrajDescription")  val makhrajDescription: String,
    @SerializedName("makhrajDescriptionBn")val makhrajDescriptionBn: String,
    @SerializedName("sifaat")              val sifaat: List<String>,
    @SerializedName("exampleWordArabic")   val exampleWordArabic: String,
    @SerializedName("exampleWord")         val exampleWord: String,
    @SerializedName("exampleWordBn")       val exampleWordBn: String,
    @SerializedName("isolatedForm")        val isolatedForm: String,
    @SerializedName("initialForm")         val initialForm: String,
    @SerializedName("medialForm")          val medialForm: String,
    @SerializedName("finalForm")           val finalForm: String,
    @SerializedName("isConnector")         val isConnector: Boolean,
    @SerializedName("audioUrl")            val audioUrl: String
)
