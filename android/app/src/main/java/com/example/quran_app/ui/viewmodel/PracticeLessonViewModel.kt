package com.example.quran_app.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.quran_app.data.repository.QuranRepository
import com.example.quran_app.domain.model.PracticeStepResponse
import com.example.quran_app.util.AudioPlayer
import com.example.quran_app.util.LetterRecorder
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import kotlin.random.Random

sealed class RecordingState {
    object Idle : RecordingState()
    data class InProgress(val isTarget: Boolean) : RecordingState()
    data class Done(val score: Int, val isTarget: Boolean) : RecordingState()
}

sealed class PracticeUiState {
    object Loading   : PracticeUiState()
    object Completed : PracticeUiState()                       // API returned nothing → all done
    data class Ready(
        val step:           PracticeStepResponse,
        val recordingState: RecordingState = RecordingState.Idle,
    ) : PracticeUiState() {
        // Only the backward guard stays on Android — step 1 of lesson 1 has no predecessor
        val isFirstItem get() = step.lessonNum == 1 && step.id == 1
    }
    data class Error(val message: String) : PracticeUiState()
}

class PracticeLessonViewModel(
    private val repository:  QuranRepository,
    private val audioPlayer: AudioPlayer,
    private val recorder:    LetterRecorder,
) : ViewModel() {

    private val _state = MutableStateFlow<PracticeUiState>(PracticeUiState.Loading)
    val state: StateFlow<PracticeUiState> = _state.asStateFlow()

    init { loadStep(1, 1) }

    fun load() = loadStep(1, 1)

    private fun loadStep(lessonNum: Int, stepNum: Int, isNavigatingForward: Boolean = false) {
        viewModelScope.launch {
            _state.value = PracticeUiState.Loading
            repository.getPracticeStep(lessonNum, stepNum)
                .onSuccess { _state.value = PracticeUiState.Ready(it) }
                .onFailure {
                    // If the next step doesn't exist and we were going forward → all lessons done
                    if (isNavigatingForward) _state.value = PracticeUiState.Completed
                    else _state.value = PracticeUiState.Error("ডেটা লোড করা যাচ্ছে না")
                }
        }
    }

    fun next() {
        val s = _state.value as? PracticeUiState.Ready ?: return
        stopRecordingIfActive()
        val step = s.step
        // Try next step in same lesson; if at last step try first step of next lesson.
        // The API (not Android) decides if that step exists — 404 → Completed state.
        val (nextLesson, nextStep) = if (step.id < step.totalSteps) {
            step.lessonNum to step.id + 1
        } else {
            step.lessonNum + 1 to 1
        }
        loadStep(nextLesson, nextStep, isNavigatingForward = true)
    }

    fun prev() {
        val s = _state.value as? PracticeUiState.Ready ?: return
        stopRecordingIfActive()
        val step = s.step
        when {
            step.id > 1        -> loadStep(step.lessonNum, step.id - 1)
            step.lessonNum > 1 -> loadStep(step.lessonNum - 1, 1)
        }
    }

    fun toggleRecording(isTarget: Boolean) {
        val s = _state.value as? PracticeUiState.Ready ?: return
        when (val rs = s.recordingState) {
            is RecordingState.Idle, is RecordingState.Done -> {
                try { recorder.start() } catch (_: Exception) { }
                _state.value = s.copy(recordingState = RecordingState.InProgress(isTarget))
            }
            is RecordingState.InProgress -> {
                if (rs.isTarget == isTarget) {
                    recorder.stop()
                    _state.value = s.copy(recordingState = RecordingState.Done(Random.nextInt(72, 97), isTarget))
                } else {
                    recorder.stop()
                    try { recorder.start() } catch (_: Exception) { }
                    _state.value = s.copy(recordingState = RecordingState.InProgress(isTarget))
                }
            }
        }
    }

    fun playAudio(url: String) {
        if (url.isBlank()) return
        val full = if (url.startsWith("http")) url else "http://localhost:5092$url"
        audioPlayer.play(full)
    }

    fun resetRecording() {
        val s = _state.value as? PracticeUiState.Ready ?: return
        _state.value = s.copy(recordingState = RecordingState.Idle)
    }

    private fun stopRecordingIfActive() {
        if ((_state.value as? PracticeUiState.Ready)?.recordingState is RecordingState.InProgress) {
            try { recorder.stop() } catch (_: Exception) { }
        }
    }

    override fun onCleared() {
        super.onCleared()
        audioPlayer.release()
        recorder.release()
    }
}
