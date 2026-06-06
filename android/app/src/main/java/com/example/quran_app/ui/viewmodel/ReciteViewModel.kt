package com.example.quran_app.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.quran_app.data.analyzer.RecitationAnalyzer
import com.example.quran_app.data.local.SURAHS
import com.example.quran_app.data.repository.QuranRepository
import com.example.quran_app.domain.model.RecitationResult
import com.example.quran_app.domain.model.SurahDef
import com.example.quran_app.domain.model.toAyahDef
import com.example.quran_app.domain.model.toSurahDef
import com.example.quran_app.util.AudioPlayer
import com.example.quran_app.util.LetterRecorder
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch

// ── UI state ──────────────────────────────────────────────────────────────────
sealed class ReciteUiState {
    data class Idle(
        val surah:      SurahDef,
        val ayahRange:  IntRange,
        val activeAyah: Int = ayahRange.first,
    ) : ReciteUiState()

    data class LoadingAyahs(val surah: SurahDef) : ReciteUiState()

    data class Recording(
        val surah:      SurahDef,
        val ayahRange:  IntRange,
        val activeAyah: Int,
    ) : ReciteUiState()

    data class Processing(
        val surah:     SurahDef,
        val ayahRange: IntRange,
    ) : ReciteUiState()

    data class ShowFeedback(
        val surah:     SurahDef,
        val ayahRange: IntRange,
        val result:    RecitationResult,
    ) : ReciteUiState()

    data class Error(val message: String) : ReciteUiState()
}

// ── ViewModel ─────────────────────────────────────────────────────────────────
class ReciteViewModel(
    private val recorder:     LetterRecorder,
    private val analyzer:     RecitationAnalyzer,
    private val repository:   QuranRepository,
    private val audioPlayer:  AudioPlayer,
) : ViewModel() {

    private val defaultSurah = SURAHS[0]
    private val defaultRange = 1..3

    private val _state = MutableStateFlow<ReciteUiState>(
        ReciteUiState.Idle(defaultSurah, defaultRange)
    )
    val state: StateFlow<ReciteUiState> = _state.asStateFlow()

    // Surah list for the picker — starts with offline fallback, updated from API
    private val _surahList = MutableStateFlow<List<SurahDef>>(SURAHS)
    val surahList: StateFlow<List<SurahDef>> = _surahList.asStateFlow()

    val showSurahPicker = MutableStateFlow(false)

    private val _isPlayingAudio = MutableStateFlow(false)
    val isPlayingAudio: StateFlow<Boolean> = _isPlayingAudio.asStateFlow()

    init { loadRecitationSurahs() }

    // ── Surah list ────────────────────────────────────────────────────────────
    private fun loadRecitationSurahs() {
        viewModelScope.launch {
            repository.getRecitationSurahs()
                .onSuccess { surahs ->
                    if (surahs.isNotEmpty())
                        _surahList.value = surahs.map { it.toSurahDef() }
                }
            // On failure keep the offline SURAHS fallback — no-op
        }
    }

    // ── Ayah audio playback ───────────────────────────────────────────────────
    fun playAyahAudio(audioUrl: String, surahNumber: Int, ayahNumber: Int) {
        if (_isPlayingAudio.value) {
            audioPlayer.stop()
            _isPlayingAudio.value = false
            return
        }
        val url = audioUrl.ifBlank {
            "https://everyayah.com/data/Alafasy_128kbps/" +
            "${surahNumber.toString().padStart(3, '0')}" +
            "${ayahNumber.toString().padStart(3, '0')}.mp3"
        }
        _isPlayingAudio.value = true
        audioPlayer.play(url) { _isPlayingAudio.value = false }
    }

    private fun stopAudioIfPlaying() {
        if (_isPlayingAudio.value) {
            audioPlayer.stop()
            _isPlayingAudio.value = false
        }
    }

    // ── Mic toggle ────────────────────────────────────────────────────────────
    fun toggleMic() {
        when (val s = _state.value) {
            is ReciteUiState.Idle      -> startRecording(s.surah, s.ayahRange, s.activeAyah)
            is ReciteUiState.Recording -> stopAndAnalyze(s.surah, s.ayahRange)
            else                       -> Unit
        }
    }

    private fun startRecording(surah: SurahDef, range: IntRange, activeAyah: Int) {
        stopAudioIfPlaying()
        try { recorder.start() } catch (_: Exception) {}
        _state.value = ReciteUiState.Recording(surah, range, activeAyah)
    }

    private fun stopAndAnalyze(surah: SurahDef, range: IntRange) {
        val file = try { recorder.stop() } catch (_: Exception) { null }
        _state.value = ReciteUiState.Processing(surah, range)
        viewModelScope.launch {
            runCatching {
                val pcm = file?.readBytes() ?: ByteArray(0)
                analyzer.analyze(pcm, surah.id, range)
            }.onSuccess { result ->
                _state.value = ReciteUiState.ShowFeedback(surah, range, result)
            }.onFailure {
                _state.value = ReciteUiState.Error("Analysis failed — please try again")
            }
        }
    }

    // ── Navigation ────────────────────────────────────────────────────────────
    fun restart() {
        val (surah, range) = currentSurahAndRange()
        _state.value = ReciteUiState.Idle(surah, range)
    }

    fun nextAyah() {
        val (surah, range) = currentSurahAndRange()
        val newEnd   = (range.last + 1).coerceAtMost(surah.ayat.size.coerceAtLeast(1))
        val newStart = (range.first + 1).coerceAtMost(newEnd)
        _state.value = ReciteUiState.Idle(surah, newStart..newEnd)
    }

    private fun currentSurahAndRange(): Pair<SurahDef, IntRange> = when (val s = _state.value) {
        is ReciteUiState.Idle         -> s.surah to s.ayahRange
        is ReciteUiState.Recording    -> s.surah to s.ayahRange
        is ReciteUiState.Processing   -> s.surah to s.ayahRange
        is ReciteUiState.ShowFeedback -> s.surah to s.ayahRange
        is ReciteUiState.LoadingAyahs -> s.surah to defaultRange
        is ReciteUiState.Error        -> defaultSurah to defaultRange
    }

    // ── Surah picker ──────────────────────────────────────────────────────────
    fun openPicker()  { showSurahPicker.value = true }
    fun closePicker() { showSurahPicker.value = false }

    fun selectSurah(surah: SurahDef) {
        showSurahPicker.value = false
        if (surah.ayat.isNotEmpty()) {
            _state.value = ReciteUiState.Idle(surah, 1..minOf(3, surah.ayat.size))
            return
        }
        // Ayat not loaded yet — fetch from API
        _state.value = ReciteUiState.LoadingAyahs(surah)
        viewModelScope.launch {
            repository.getAyahs(surah.id)
                .onSuccess { ayahs ->
                    val loaded = surah.copy(ayat = ayahs.map { it.toAyahDef() })
                    _state.value = ReciteUiState.Idle(loaded, 1..minOf(3, loaded.ayat.size))
                }
                .onFailure {
                    // Fall back to Idle with empty ayat — verse card shows nothing
                    _state.value = ReciteUiState.Idle(surah, defaultRange)
                }
        }
    }

    // ── Clean up ──────────────────────────────────────────────────────────────
    override fun onCleared() {
        super.onCleared()
        recorder.release()
        audioPlayer.release()
    }
}
