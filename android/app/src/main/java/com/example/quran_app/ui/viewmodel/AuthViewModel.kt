package com.example.quran_app.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.quran_app.data.repository.AuthRepository
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch

sealed class AuthState {
    object Idle       : AuthState()
    object Loading    : AuthState()
    object Success    : AuthState()
    data class Error(val message: String) : AuthState()
}

class AuthViewModel(private val authRepository: AuthRepository) : ViewModel() {

    private val _state = MutableStateFlow<AuthState>(AuthState.Idle)
    val state: StateFlow<AuthState> = _state.asStateFlow()

    fun login(email: String, password: String) {
        if (email.isBlank() || password.isBlank()) {
            _state.value = AuthState.Error("ইমেইল ও পাসওয়ার্ড প্রয়োজন")
            return
        }
        viewModelScope.launch {
            _state.value = AuthState.Loading
            authRepository.login(email.trim(), password)
                .onSuccess { _state.value = AuthState.Success }
                .onFailure { _state.value = AuthState.Error(friendlyError(it.message)) }
        }
    }

    fun register(email: String, password: String, displayName: String) {
        if (email.isBlank() || password.isBlank() || displayName.isBlank()) {
            _state.value = AuthState.Error("সব তথ্য পূরণ করুন")
            return
        }
        if (password.length < 6) {
            _state.value = AuthState.Error("পাসওয়ার্ড কমপক্ষে ৬ অক্ষরের হতে হবে")
            return
        }
        viewModelScope.launch {
            _state.value = AuthState.Loading
            authRepository.register(email.trim(), password, displayName.trim())
                .onSuccess { _state.value = AuthState.Success }
                .onFailure { _state.value = AuthState.Error(friendlyError(it.message)) }
        }
    }

    fun resetState() { _state.value = AuthState.Idle }

    private fun friendlyError(msg: String?): String = when {
        msg?.contains("409") == true || msg?.contains("Conflict") == true ->
            "এই ইমেইল ইতিমধ্যে নিবন্ধিত"
        msg?.contains("401") == true || msg?.contains("Unauthorized") == true ->
            "ইমেইল বা পাসওয়ার্ড ভুল"
        msg?.contains("Unable to resolve") == true || msg?.contains("connect") == true ->
            "সার্ভারে সংযোগ করা যাচ্ছে না"
        else -> "কিছু একটা ভুল হয়েছে, আবার চেষ্টা করুন"
    }
}
