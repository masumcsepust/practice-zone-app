package com.example.quran_app.ui.navigation

sealed class Screen(val route: String) {
    object ArabicLetters : Screen("arabic_letters")
    object Surahs : Screen("surahs")
    object Ayahs : Screen("ayahs/{surahId}") {
        fun createRoute(surahId: Int) = "ayahs/$surahId"
    }
}
