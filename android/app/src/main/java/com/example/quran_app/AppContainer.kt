package com.example.quran_app

import android.content.Context
import com.example.quran_app.data.analyzer.ApiRecitationAnalyzer
import com.example.quran_app.data.analyzer.RecitationAnalyzer
import com.example.quran_app.data.local.TokenManager
import com.example.quran_app.data.remote.LetterPracticeWebSocketClient
import com.example.quran_app.data.remote.RetrofitClient
import com.example.quran_app.data.repository.AuthRepository
import com.example.quran_app.data.repository.QuranRepository
import com.example.quran_app.util.AudioPlayer
import com.example.quran_app.util.LetterRecorder

class AppContainer(private val context: Context) {
    val tokenManager      = TokenManager(context)
    private val apiService = RetrofitClient.apiService

    init {
        // Restore JWT into the Retrofit interceptor on app start
        RetrofitClient.setToken(tokenManager.token)
    }

    val authRepository       = AuthRepository(apiService, tokenManager)
    val quranRepository      = QuranRepository(apiService)
    val audioPlayer          = AudioPlayer(context)
    val letterRecorder       = LetterRecorder(context)
    val wsClient             = LetterPracticeWebSocketClient()
    val recitationAnalyzer: RecitationAnalyzer = ApiRecitationAnalyzer(RetrofitClient.apiService)
}
