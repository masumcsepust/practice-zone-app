package com.example.quran_app.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.quran_app.data.repository.QuranRepository
import com.example.quran_app.domain.model.PracticeItem
import com.example.quran_app.domain.model.PracticeSessionData
import com.example.quran_app.util.AudioPlayer
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch

sealed class PracticeUiState {
    object Loading : PracticeUiState()
    data class Ready(
        val lessons:       List<PracticeSessionData>,
        val lessonIndex:   Int,
        val itemIndex:     Int,
    ) : PracticeUiState() {
        val lesson     get() = lessons[lessonIndex]
        val item       get() = lesson.practiceItems[itemIndex]
        val totalItems get() = lesson.practiceItems.size
        val totalLessons get() = lessons.size
        val isFirstItem  get() = itemIndex == 0 && lessonIndex == 0
        val isLastItem   get() = itemIndex == totalItems - 1 && lessonIndex == lessons.lastIndex
    }
    data class Error(val message: String) : PracticeUiState()
}

class PracticeLessonViewModel(
    private val repository:  QuranRepository,
    private val audioPlayer: AudioPlayer
) : ViewModel() {

    private val _state = MutableStateFlow<PracticeUiState>(PracticeUiState.Loading)
    val state: StateFlow<PracticeUiState> = _state.asStateFlow()

    init { load() }

    fun load() {
        viewModelScope.launch {
            _state.value = PracticeUiState.Loading
            repository.getPracticeLessons()
                .onSuccess { lessons ->
                    if (lessons.isEmpty()) {
                        _state.value = PracticeUiState.Error("কোনো পাঠ পাওয়া যায়নি")
                    } else {
                        _state.value = PracticeUiState.Ready(lessons, 0, 0)
                    }
                }
                .onFailure {
                    _state.value = PracticeUiState.Error("সার্ভারে সংযোগ করা যাচ্ছে না")
                }
        }
    }

    fun next() {
        val s = _state.value as? PracticeUiState.Ready ?: return
        val newItemIdx   = s.itemIndex + 1
        val newLessonIdx = s.lessonIndex
        if (newItemIdx < s.totalItems) {
            _state.value = s.copy(itemIndex = newItemIdx)
        } else if (newLessonIdx + 1 < s.totalLessons) {
            _state.value = s.copy(lessonIndex = newLessonIdx + 1, itemIndex = 0)
        }
    }

    fun prev() {
        val s = _state.value as? PracticeUiState.Ready ?: return
        val newItemIdx = s.itemIndex - 1
        if (newItemIdx >= 0) {
            _state.value = s.copy(itemIndex = newItemIdx)
        } else if (s.lessonIndex > 0) {
            val prevLesson = s.lessons[s.lessonIndex - 1]
            _state.value = s.copy(
                lessonIndex = s.lessonIndex - 1,
                itemIndex   = prevLesson.practiceItems.lastIndex
            )
        }
    }

    fun playAudio(url: String) {
        if (url.isBlank()) return
        val full = if (url.startsWith("http")) url else "http://localhost:5092$url"
        audioPlayer.play(full)
    }

    override fun onCleared() {
        super.onCleared()
        audioPlayer.release()
    }
}
