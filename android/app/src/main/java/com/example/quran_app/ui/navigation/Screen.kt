package com.example.quran_app.ui.navigation

sealed class Screen(val route: String) {
    object Home           : Screen("home")
    object ArabicLetters  : Screen("arabic_letters")
    object SurahList      : Screen("surah_list")
    object AyahList       : Screen("ayah_list/{surahId}/{surahName}") {
        fun createRoute(surahId: Int, surahName: String) =
            "ayah_list/$surahId/${surahName.encodeUrl()}"
    }
    object AyahRecitation : Screen("ayah_recitation/{ayahId}") {
        fun createRoute(ayahId: Int) = "ayah_recitation/$ayahId"
    }
}

private fun String.encodeUrl() = java.net.URLEncoder.encode(this, "UTF-8")
