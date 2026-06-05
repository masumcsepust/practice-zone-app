package com.example.quran_app.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.quran_app.data.remote.QuranApiService
import com.example.quran_app.domain.model.LeaderboardResponse
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch

sealed class LeaderboardState {
    object Loading : LeaderboardState()
    data class Success(val data: LeaderboardResponse, val period: String) : LeaderboardState()
    data class Error(val message: String) : LeaderboardState()
}

class LeaderboardViewModel(
    private val apiService: QuranApiService
) : ViewModel() {

    private val _state = MutableStateFlow<LeaderboardState>(LeaderboardState.Loading)
    val state: StateFlow<LeaderboardState> = _state.asStateFlow()

    init { load("all") }

    fun load(period: String) {
        viewModelScope.launch {
            _state.value = LeaderboardState.Loading
            runCatching { apiService.getLeaderboard(period) }
                .onSuccess { _state.value = LeaderboardState.Success(it, period) }
                .onFailure { _state.value = LeaderboardState.Error(it.message ?: "Unknown error") }
        }
    }
}
