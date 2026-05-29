package com.example.quran_app.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.quran_app.data.remote.AyahRecitationWebSocketClient
import com.example.quran_app.data.repository.QuranRepository
import com.example.quran_app.domain.model.Ayah
import com.example.quran_app.domain.model.RecitationResult
import com.example.quran_app.domain.model.Surah
import com.example.quran_app.util.LetterRecorder
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import java.io.File

sealed class RecitationState {
    object Idle       : RecitationState()
    object Recording  : RecitationState()
    object Processing : RecitationState()
    data class Result(val data: RecitationResult) : RecitationState()
    data class Error(val message: String)         : RecitationState()
}

class AyahRecitationViewModel(
    private val repository: QuranRepository,
    private val recorder:   LetterRecorder,
    private val wsClient:   AyahRecitationWebSocketClient
) : ViewModel() {

    private val _surahs    = MutableStateFlow<List<Surah>>(emptyList())
    val surahs = _surahs.asStateFlow()

    private val _ayahs     = MutableStateFlow<List<Ayah>>(emptyList())
    val ayahs = _ayahs.asStateFlow()

    private val _isLoading = MutableStateFlow(false)
    val isLoading = _isLoading.asStateFlow()

    private val _error     = MutableStateFlow<String?>(null)
    val error = _error.asStateFlow()

    private val _recitationState = MutableStateFlow<RecitationState>(RecitationState.Idle)
    val recitationState = _recitationState.asStateFlow()

    private var recordingFile: File? = null

    fun fetchSurahs() {
        if (_surahs.value.isNotEmpty()) return
        viewModelScope.launch {
            _isLoading.value = true
            _error.value     = null
            repository.getSurahs()
                .onSuccess { _surahs.value = it }
                .onFailure { _error.value = it.message ?: "সূরা লোড ব্যর্থ হয়েছে" }
            _isLoading.value = false
        }
    }

    fun fetchAyahs(surahId: Int) {
        viewModelScope.launch {
            _isLoading.value = true
            _error.value     = null
            _ayahs.value     = emptyList()
            repository.getAyahs(surahId)
                .onSuccess { _ayahs.value = it }
                .onFailure { _error.value = it.message ?: "আয়াত লোড ব্যর্থ হয়েছে" }
            _isLoading.value = false
        }
    }

    fun startRecording() {
        recordingFile = recorder.start()
        _recitationState.value = RecitationState.Recording
    }

    fun stopAndAssess(ayahId: Int) {
        val file = recorder.stop() ?: run {
            _recitationState.value = RecitationState.Error("রেকর্ডিং ব্যর্থ হয়েছে")
            return
        }
        recordingFile = file
        _recitationState.value = RecitationState.Processing

        wsClient.assess(
            ayahId    = ayahId,
            audioFile = file,
            onResult  = { result ->
                _recitationState.value = RecitationState.Result(result)
            },
            onError   = { msg ->
                _recitationState.value = RecitationState.Error(msg)
            }
        )
    }

    fun resetRecitation() {
        wsClient.cancel()
        recorder.release()
        _recitationState.value = RecitationState.Idle
    }

    override fun onCleared() {
        super.onCleared()
        wsClient.cancel()
        recorder.release()
    }
}
