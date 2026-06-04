package com.example.quran_app.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.quran_app.data.local.TokenManager
import com.example.quran_app.data.remote.QuranApiService
import com.example.quran_app.data.repository.AuthRepository
import com.example.quran_app.domain.model.UserInfo
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch

sealed class ProfileState {
    object Loading               : ProfileState()
    object Guest                 : ProfileState()
    data class LoggedIn(val user: UserInfo) : ProfileState()
}

class ProfileViewModel(
    private val apiService:    QuranApiService,
    private val tokenManager:  TokenManager,
    private val authRepository: AuthRepository
) : ViewModel() {

    private val _state = MutableStateFlow<ProfileState>(ProfileState.Loading)
    val state: StateFlow<ProfileState> = _state.asStateFlow()

    init { load() }

    fun load() {
        if (!tokenManager.isLoggedIn) { _state.value = ProfileState.Guest; return }
        viewModelScope.launch {
            _state.value = ProfileState.Loading
            runCatching { apiService.me() }
                .onSuccess { _state.value = ProfileState.LoggedIn(it) }
                .onFailure { _state.value = ProfileState.Guest }
        }
    }

    fun logout() {
        authRepository.logout()
        _state.value = ProfileState.Guest
    }
}
