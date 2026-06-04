package com.example.quran_app.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.quran_app.data.remote.LetterPracticeWebSocketClient
import com.example.quran_app.data.repository.QuranRepository
import com.example.quran_app.domain.model.PronunciationResult
import com.example.quran_app.domain.model.SpeakState
import com.example.quran_app.domain.model.TanweenLessonData
import com.example.quran_app.util.AudioPlayer
import com.example.quran_app.util.LetterRecorder
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import java.io.File

class TanweenLessonViewModel(
    private val repository:  QuranRepository,
    private val audioPlayer: AudioPlayer,
    private val recorder:    LetterRecorder,
    private val wsClient:    LetterPracticeWebSocketClient,
) : ViewModel() {

    companion object {
        const val TOTAL_LESSONS = 28
    }

    private val _currentLesson = MutableStateFlow(1)
    val currentLesson: StateFlow<Int> = _currentLesson.asStateFlow()

    private val _lessonData = MutableStateFlow<TanweenLessonData?>(null)
    val lessonData: StateFlow<TanweenLessonData?> = _lessonData.asStateFlow()

    private val _isLoading = MutableStateFlow(false)
    val isLoading: StateFlow<Boolean> = _isLoading.asStateFlow()

    private val _error = MutableStateFlow<String?>(null)
    val error: StateFlow<String?> = _error.asStateFlow()

    private val _speakState = MutableStateFlow<SpeakState>(SpeakState.Idle)
    val speakState: StateFlow<SpeakState> = _speakState.asStateFlow()

    // Index into tanweenTypes for the current practice item
    private val _practiceStep = MutableStateFlow(0)
    val practiceStep: StateFlow<Int> = _practiceStep.asStateFlow()

    private var recordingFile: File? = null

    init { loadLesson(1) }

    fun loadLesson(letterOrder: Int) {
        val order = letterOrder.coerceIn(1, TOTAL_LESSONS)
        viewModelScope.launch {
            _isLoading.value = true
            _error.value     = null
            _currentLesson.value = order
            _practiceStep.value  = 0
            resetSpeak()
            repository.getTanweenLesson(order)
                .onSuccess { _lessonData.value = it }
                .onFailure { _error.value = it.message ?: "Failed to load lesson" }
            _isLoading.value = false
        }
    }

    fun nextLesson() {
        val next = (_currentLesson.value + 1).coerceAtMost(TOTAL_LESSONS)
        if (next != _currentLesson.value) loadLesson(next)
    }

    fun prevLesson() {
        val prev = (_currentLesson.value - 1).coerceAtLeast(1)
        if (prev != _currentLesson.value) loadLesson(prev)
    }

    fun setPracticeStep(step: Int) {
        val max = (_lessonData.value?.tanweenTypes?.size ?: 1) - 1
        _practiceStep.value = step.coerceIn(0, max)
        resetSpeak()
    }

    fun nextPracticeStep() {
        val max = (_lessonData.value?.tanweenTypes?.size ?: 1) - 1
        if (_practiceStep.value < max) {
            _practiceStep.value++
            resetSpeak()
        }
    }

    // ── Audio ─────────────────────────────────────────────────────────────────

    fun playAudio(url: String) { audioPlayer.play(url) }

    // ── Recording ─────────────────────────────────────────────────────────────

    fun startRecording() {
        _speakState.value = SpeakState.Recording
        try {
            recordingFile = recorder.start()
        } catch (e: Exception) {
            _speakState.value = SpeakState.Error("Could not start recording: ${e.message}")
        }
    }

    fun stopAndAssess() {
        _speakState.value = SpeakState.Processing
        val file = recorder.stop() ?: run {
            _speakState.value = SpeakState.Error("No recording file found")
            return
        }
        recordingFile = file

        wsClient.assess(
            letterId  = "00000000-0000-0000-0000-${_currentLesson.value.toString().padStart(12, '0')}",
            audioFile = file,
            onResult  = { result ->
                viewModelScope.launch(Dispatchers.Main) {
                    _speakState.value = SpeakState.Result(result)
                }
            },
            onError = { msg ->
                viewModelScope.launch(Dispatchers.Main) {
                    _speakState.value = SpeakState.Error(msg)
                }
            }
        )
    }

    fun resetSpeak() {
        wsClient.cancel()
        _speakState.value = SpeakState.Idle
    }

    override fun onCleared() {
        super.onCleared()
        audioPlayer.release()
        recorder.release()
        wsClient.cancel()
    }
}
