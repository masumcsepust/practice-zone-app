package com.example.quran_app.ui.viewmodel

import android.content.SharedPreferences
import androidx.lifecycle.ViewModel
import com.example.quran_app.domain.model.DailyPrayer
import com.example.quran_app.domain.model.DAILY_PRAYERS
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update

class NamazHubViewModel(private val prefs: SharedPreferences) : ViewModel() {

    private val _prayers = MutableStateFlow(loadPrayers())
    val prayers: StateFlow<List<DailyPrayer>> = _prayers.asStateFlow()

    private fun loadPrayers(): List<DailyPrayer> =
        DAILY_PRAYERS.map { prayer ->
            prayer.copy(isLearned = prefs.getBoolean("namaz_learned_${prayer.id}", false))
        }

    fun markLearned(id: String) {
        prefs.edit().putBoolean("namaz_learned_$id", true).apply()
        _prayers.update { list ->
            list.map { if (it.id == id) it.copy(isLearned = true) else it }
        }
    }
}
