package com.example.quran_app

import android.content.Context
import com.example.quran_app.data.remote.AyahRecitationWebSocketClient
import com.example.quran_app.data.remote.LetterPracticeWebSocketClient
import com.example.quran_app.data.remote.RetrofitClient
import com.example.quran_app.data.repository.QuranRepository
import com.example.quran_app.util.AudioPlayer
import com.example.quran_app.util.LetterRecorder

class AppContainer(private val context: Context) {
    private val apiService    = RetrofitClient.apiService
    val quranRepository       = QuranRepository(apiService)
    val audioPlayer           = AudioPlayer(context)
    val letterRecorder        = LetterRecorder(context)
    val wsClient              = LetterPracticeWebSocketClient()
    val ayahRecorder          = LetterRecorder(context)
    val ayahWsClient          = AyahRecitationWebSocketClient()
}
