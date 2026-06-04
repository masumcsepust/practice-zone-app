package com.example.quran_app.ui.navigation

sealed class Screen(val route: String) {
    object Login          : Screen("login")
    object Register       : Screen("register")
    object Home           : Screen("home")
    object ArabicLetters  : Screen("arabic_letters")
    object Tajweed        : Screen("tajweed")
    object TajweedProgress: Screen("tajweed_progress")
    object SukoonLesson   : Screen("sukoon_lesson")
}
