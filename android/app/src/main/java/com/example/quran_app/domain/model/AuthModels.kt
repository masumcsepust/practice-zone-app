package com.example.quran_app.domain.model

import com.google.gson.annotations.SerializedName

data class RegisterRequest(
    val email:       String,
    val password:    String,
    val displayName: String
)

data class LoginRequest(
    val email:    String,
    val password: String
)

data class AuthResponse(
    @SerializedName("token")         val token:         String,
    @SerializedName("userId")        val userId:        String,
    @SerializedName("displayName")   val displayName:   String,
    @SerializedName("email")         val email:         String,
    @SerializedName("role")          val role:          String,
    @SerializedName("totalXp")       val totalXp:       Int,
    @SerializedName("currentStreak") val currentStreak: Int
)

data class UserInfo(
    @SerializedName("id")            val id:            String,
    @SerializedName("email")         val email:         String,
    @SerializedName("role")          val role:          String,
    @SerializedName("displayName")   val displayName:   String,
    @SerializedName("avatarUrl")     val avatarUrl:     String,
    @SerializedName("totalXp")       val totalXp:       Int,
    @SerializedName("currentStreak") val currentStreak: Int
)
