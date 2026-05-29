package com.example.quran_app.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.quran_app.data.repository.QuranRepository
import com.example.quran_app.domain.model.ArabicLetter
import com.example.quran_app.domain.model.DrawingState
import com.example.quran_app.domain.model.LetterForms
import com.example.quran_app.domain.model.PronunciationResult
import com.example.quran_app.domain.model.SpeakState
import com.example.quran_app.util.AudioPlayer
import com.example.quran_app.util.LetterRecorder
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import java.io.File

class ArabicLettersViewModel(
    private val repository:  QuranRepository,
    private val audioPlayer: AudioPlayer,
    private val recorder:    LetterRecorder
) : ViewModel() {

    companion object {
        private const val PAGE_SIZE      = 100   // load all letters in one shot
        /** Start loading the next page when this many letters remain before the end. */
        private const val PREFETCH_AHEAD = 5
    }

    // ── Letters ───────────────────────────────────────────────────────────────
    private val _letters       = MutableStateFlow<List<ArabicLetter>>(emptyList())
    val letters: StateFlow<List<ArabicLetter>> = _letters.asStateFlow()

    private val _selectedLetter = MutableStateFlow<ArabicLetter?>(null)
    val selectedLetter: StateFlow<ArabicLetter?> = _selectedLetter.asStateFlow()

    private val _isLoading     = MutableStateFlow(false)
    val isLoading: StateFlow<Boolean> = _isLoading.asStateFlow()

    private val _error         = MutableStateFlow<String?>(null)
    val error: StateFlow<String?> = _error.asStateFlow()

    // ── Pagination ────────────────────────────────────────────────────────────
    /** True total letters reported by the server (across all pages). */
    private val _totalCount     = MutableStateFlow(0)
    val totalCount: StateFlow<Int> = _totalCount.asStateFlow()

    /** True when a background "load more" fetch is in progress. */
    private val _isLoadingMore  = MutableStateFlow(false)
    val isLoadingMore: StateFlow<Boolean> = _isLoadingMore.asStateFlow()

    private var currentPage     = 0       // last successfully fetched page
    private var hasNextPage     = false   // whether the server has more pages

    // ── Forms (রূপ tab) ───────────────────────────────────────────────────────
    private val _letterForms   = MutableStateFlow<LetterForms?>(null)
    val letterForms: StateFlow<LetterForms?> = _letterForms.asStateFlow()

    private val _formsLoading  = MutableStateFlow(false)
    val formsLoading: StateFlow<Boolean> = _formsLoading.asStateFlow()

    private val _formsError    = MutableStateFlow<String?>(null)
    val formsError: StateFlow<String?> = _formsError.asStateFlow()

    // ── Speak (বলুন tab) ─────────────────────────────────────────────────────
    private val _speakState    = MutableStateFlow<SpeakState>(SpeakState.Idle)
    val speakState: StateFlow<SpeakState> = _speakState.asStateFlow()

    // ── Drawing (লিখুন tab) ───────────────────────────────────────────────────
    private val _drawingState  = MutableStateFlow<DrawingState>(DrawingState.Idle)
    val drawingState: StateFlow<DrawingState> = _drawingState.asStateFlow()

    private var recordingFile: File? = null
    private val BASE_URL = "http://localhost:5092"

    init { fetchLetters() }

    // ── Public: letters ───────────────────────────────────────────────────────

    /** (Re-)loads from page 1, discarding any previously loaded pages. */
    fun fetchLetters() {
        viewModelScope.launch {
            _isLoading.value = true
            _error.value     = null
            currentPage      = 0
            hasNextPage      = false
            _letters.value   = emptyList()

            repository.getArabicLetters(page = 1, pageSize = PAGE_SIZE)
                .onSuccess { paged ->
                    _letters.value      = paged.items
                    _totalCount.value   = paged.totalCount
                    currentPage         = paged.page
                    hasNextPage         = paged.hasNext

                    if (_selectedLetter.value == null && paged.items.isNotEmpty())
                        _selectedLetter.value = paged.items.first()
                }
                .onFailure { _error.value = it.message ?: "অজানা ত্রুটি" }

            _isLoading.value = false
        }
    }

    /**
     * Loads the next page and appends its items to [letters].
     * No-op if already loading or no more pages exist.
     */
    fun loadNextPage() {
        if (!hasNextPage || _isLoadingMore.value) return
        viewModelScope.launch {
            _isLoadingMore.value = true
            val nextPage = currentPage + 1

            repository.getArabicLetters(page = nextPage, pageSize = PAGE_SIZE)
                .onSuccess { paged ->
                    _letters.value    = _letters.value + paged.items
                    _totalCount.value = paged.totalCount
                    currentPage       = paged.page
                    hasNextPage       = paged.hasNext
                }
                .onFailure {
                    // Don't update currentPage — the caller can retry
                }

            _isLoadingMore.value = false
        }
    }

    fun selectLetter(letter: ArabicLetter) {
        _selectedLetter.value = letter
        _letterForms.value    = null
        _formsError.value     = null
        resetSpeak()
        resetDrawing()

        // Pre-fetch next page when user is within PREFETCH_AHEAD of the loaded boundary
        val index = _letters.value.indexOf(letter)
        if (index >= _letters.value.size - PREFETCH_AHEAD) loadNextPage()
    }

    // ── Public: forms ─────────────────────────────────────────────────────────

    fun fetchLetterForms(id: Int) {
        if (_letterForms.value?.id == id) return
        viewModelScope.launch {
            _formsLoading.value = true
            _formsError.value   = null
            repository.getLetterForms(id)
                .onSuccess  { _letterForms.value = it }
                .onFailure  { _formsError.value  = it.message ?: "ফর্ম লোড ব্যর্থ" }
            _formsLoading.value = false
        }
    }

    // ── Public: speak ─────────────────────────────────────────────────────────

    fun startRecording() {
        _speakState.value = SpeakState.Recording
        try {
            recordingFile = recorder.start()
        } catch (e: Exception) {
            _speakState.value = SpeakState.Error("রেকর্ড শুরু করা যায়নি: ${e.message}")
        }
    }

    fun stopAndAssess(letterId: Int) {
        _speakState.value = SpeakState.Processing
        val file = recorder.stop() ?: run {
            _speakState.value = SpeakState.Error("রেকর্ড ফাইল পাওয়া যায়নি")
            return
        }
        recordingFile = file

        // HTTP multipart upload — replaces the unreliable WebSocket path
        viewModelScope.launch {
            repository.checkPronunciation(letterId, file)
                .onSuccess { result ->
                    _speakState.value = SpeakState.Result(result)
                }
                .onFailure { err ->
                    _speakState.value = SpeakState.Error(
                        err.message?.takeIf { it.isNotBlank() }
                            ?: "সার্ভারের সাথে সংযোগ বিচ্ছিন্ন হয়েছে"
                    )
                }
        }
    }

    fun resetSpeak() {
        _speakState.value = SpeakState.Idle
    }

    // ── Public: drawing ───────────────────────────────────────────────────────

    fun checkDrawing(letterId: Int, imageBase64: String) {
        _drawingState.value = DrawingState.Submitting
        viewModelScope.launch {
            repository.checkDrawing(letterId, imageBase64)
                .onSuccess { _drawingState.value = DrawingState.Result(it) }
                .onFailure { _drawingState.value = DrawingState.Error(it.message ?: "সংযোগ ব্যর্থ") }
        }
    }

    fun resetDrawing() {
        _drawingState.value = DrawingState.Idle
    }

    // ── Audio playback ────────────────────────────────────────────────────────

    fun playAudio(audioUrl: String) {
        val fullUrl = if (audioUrl.startsWith("http")) audioUrl else BASE_URL + audioUrl
        audioPlayer.play(fullUrl)
    }

    override fun onCleared() {
        super.onCleared()
        audioPlayer.release()
        recorder.release()
    }
}
