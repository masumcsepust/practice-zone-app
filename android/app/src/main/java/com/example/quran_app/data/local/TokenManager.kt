package com.example.quran_app.data.local

import android.content.Context

class TokenManager(context: Context) {
    private val prefs = context.getSharedPreferences("auth_prefs", Context.MODE_PRIVATE)

    var token: String?
        get() = prefs.getString(KEY_TOKEN, null)
        set(value) = prefs.edit().putString(KEY_TOKEN, value).apply()

    var userId: String?
        get() = prefs.getString(KEY_USER_ID, null)
        set(value) = prefs.edit().putString(KEY_USER_ID, value).apply()

    var displayName: String?
        get() = prefs.getString(KEY_DISPLAY_NAME, null)
        set(value) = prefs.edit().putString(KEY_DISPLAY_NAME, value).apply()

    val isLoggedIn get() = token != null

    fun save(token: String, userId: String, displayName: String) {
        prefs.edit()
            .putString(KEY_TOKEN, token)
            .putString(KEY_USER_ID, userId)
            .putString(KEY_DISPLAY_NAME, displayName)
            .apply()
    }

    fun clear() = prefs.edit().clear().apply()

    companion object {
        private const val KEY_TOKEN        = "jwt_token"
        private const val KEY_USER_ID      = "user_id"
        private const val KEY_DISPLAY_NAME = "display_name"
    }
}
