package com.example.quran_app.ui.viewmodel

import android.content.Context
import android.media.MediaPlayer
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.quran_app.domain.model.PrayerStep
import com.example.quran_app.domain.model.SALAH_STEPS
import kotlinx.coroutines.flow.MutableSharedFlow
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.SharedFlow
import kotlinx.coroutines.flow.SharingStarted
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asSharedFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.map
import kotlinx.coroutines.flow.stateIn
import kotlinx.coroutines.launch

class PostureLessonViewModel(initialIndex: Int = 1) : ViewModel() {

    private val _stepIndex = MutableStateFlow(initialIndex.coerceIn(1, SALAH_STEPS.size))
    val stepIndex: StateFlow<Int> = _stepIndex.asStateFlow()

    val currentStep: StateFlow<PrayerStep> = _stepIndex
        .map { SALAH_STEPS[it - 1] }
        .stateIn(viewModelScope, SharingStarted.Eagerly, SALAH_STEPS[_stepIndex.value - 1])

    private val _navigateBack = MutableSharedFlow<Unit>()
    val navigateBack: SharedFlow<Unit> = _navigateBack.asSharedFlow()

    private var mediaPlayer: MediaPlayer? = null

    fun next() {
        val i = _stepIndex.value
        if (i < SALAH_STEPS.size) _stepIndex.value = i + 1
    }

    fun previous() {
        val i = _stepIndex.value
        if (i > 1) _stepIndex.value = i - 1
    }

    fun playAudio(context: Context, audioRes: Int) {
        mediaPlayer?.release()
        mediaPlayer = MediaPlayer.create(context, audioRes)
        mediaPlayer?.start()
    }

    fun complete() {
        viewModelScope.launch { _navigateBack.emit(Unit) }
    }

    override fun onCleared() {
        mediaPlayer?.release()
        mediaPlayer = null
    }
}
