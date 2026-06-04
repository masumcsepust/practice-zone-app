package com.example.quran_app.domain.model

import com.google.gson.annotations.SerializedName

data class SaveTajweedProgressRequest(
    val mode:         String,
    val score:        Double,
    val accuracyScore: Double,
    val isCorrect:    Boolean,
)

data class TajweedModeStat(
    val bestScore:   Double,
    val attempts:    Int,
    val lastCorrect: Boolean,
)

data class TajweedLetterProgress(
    val letterId:      String,
    val letter:        String,
    val nameBangla:    String,
    val harakat:       TajweedModeStat?,
    val tanween:       TajweedModeStat?,
    val sukoonShaddah: TajweedModeStat?,
    val wordBuilding:  TajweedModeStat?,
)
