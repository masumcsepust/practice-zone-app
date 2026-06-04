package com.example.quran_app.data.repository

import com.example.quran_app.data.local.TokenManager
import com.example.quran_app.data.remote.QuranApiService
import com.example.quran_app.data.remote.RetrofitClient
import com.example.quran_app.domain.model.AuthResponse
import com.example.quran_app.domain.model.LoginRequest
import com.example.quran_app.domain.model.RegisterRequest

class AuthRepository(
    private val apiService:   QuranApiService,
    private val tokenManager: TokenManager
) {
    suspend fun register(email: String, password: String, displayName: String): Result<AuthResponse> =
        runCatching {
            val response = apiService.register(RegisterRequest(email, password, displayName))
            saveSession(response)
            response
        }

    suspend fun login(email: String, password: String): Result<AuthResponse> =
        runCatching {
            val response = apiService.login(LoginRequest(email, password))
            saveSession(response)
            response
        }

    fun logout() {
        tokenManager.clear()
        RetrofitClient.setToken(null)
    }

    val isLoggedIn get() = tokenManager.isLoggedIn
    val displayName get() = tokenManager.displayName

    private fun saveSession(response: AuthResponse) {
        tokenManager.save(response.token, response.userId, response.displayName)
        RetrofitClient.setToken(response.token)
    }
}
