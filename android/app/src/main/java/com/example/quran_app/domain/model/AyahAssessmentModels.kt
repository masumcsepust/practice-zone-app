package com.example.quran_app.domain.model

import com.google.gson.annotations.SerializedName

data class AyahAssessmentResult(
    @SerializedName("overallTajweed") val overallTajweed: Int,
    @SerializedName("lettersPct")     val lettersPct:     Int,
    @SerializedName("pacePct")        val pacePct:        Float,
    @SerializedName("recognizedText") val recognizedText: String,
    @SerializedName("notes")          val notes:          List<AyahNote>,
)

data class AyahNote(
    @SerializedName("kind")              val kind:              String,
    @SerializedName("arabic")            val arabic:            String,
    @SerializedName("title")             val title:             String,
    @SerializedName("tip")               val tip:               String,
    @SerializedName("isPositive")        val isPositive:        Boolean,
    @SerializedName("referenceAudioUrl") val referenceAudioUrl: String?,
)

fun AyahAssessmentResult.toRecitationResult() = RecitationResult(
    overallTajweed = overallTajweed,
    lettersPct     = lettersPct,
    pacePct        = pacePct,
    notes = notes.mapNotNull { n ->
        runCatching {
            Note(
                kind              = NoteKind.valueOf(n.kind.uppercase()),
                arabic            = n.arabic,
                title             = n.title,
                tip               = n.tip,
                isPositive        = n.isPositive,
                referenceAudioUrl = n.referenceAudioUrl,
            )
        }.getOrNull()
    }
)
