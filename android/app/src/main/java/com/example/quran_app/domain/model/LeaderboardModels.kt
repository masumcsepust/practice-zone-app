package com.example.quran_app.domain.model

import com.google.gson.annotations.SerializedName

data class LeaderboardEntry(
    @SerializedName("rank")        val rank:        Int,
    @SerializedName("displayName") val displayName: String,
    @SerializedName("avatarUrl")   val avatarUrl:   String,
    @SerializedName("xp")         val xp:          Int
)

data class LeaderboardMyPosition(
    @SerializedName("rank")        val rank:        Int,
    @SerializedName("displayName") val displayName: String,
    @SerializedName("avatarUrl")   val avatarUrl:   String,
    @SerializedName("xp")         val xp:          Int,
    @SerializedName("xpToNextRank") val xpToNextRank: Int
)

data class LeaderboardResponse(
    @SerializedName("entries")    val entries:    List<LeaderboardEntry>,
    @SerializedName("myPosition") val myPosition: LeaderboardMyPosition?
)
