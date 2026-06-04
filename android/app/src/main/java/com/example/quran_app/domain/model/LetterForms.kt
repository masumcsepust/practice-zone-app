package com.example.quran_app.domain.model

import com.google.gson.annotations.SerializedName

/** Returned by GET /api/arabic-letters/{id}/forms */
data class LetterForms(
    @SerializedName("id")           val id:           String,
    @SerializedName("order")        val order:        Int,
    @SerializedName("letter")       val letter:       String,
    @SerializedName("nameEnglish")  val nameEnglish:  String,
    @SerializedName("isolatedForm") val isolatedForm: String,
    @SerializedName("initialForm")  val initialForm:  String,
    @SerializedName("medialForm")   val medialForm:   String,
    @SerializedName("finalForm")    val finalForm:    String,
    @SerializedName("isConnector")  val isConnector:  Boolean
)
