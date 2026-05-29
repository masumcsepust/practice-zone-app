package com.example.quran_app.domain.model

import com.google.gson.annotations.SerializedName

data class PagedResponse<T>(
    @SerializedName("items")       val items:       List<T>,
    @SerializedName("page")        val page:        Int,
    @SerializedName("pageSize")    val pageSize:    Int,
    @SerializedName("totalCount")  val totalCount:  Int,
    @SerializedName("totalPages")  val totalPages:  Int,
    @SerializedName("hasNext")     val hasNext:     Boolean,
    @SerializedName("hasPrevious") val hasPrevious: Boolean
)
