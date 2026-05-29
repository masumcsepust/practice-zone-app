package com.example.quran_app

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.ui.Modifier
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavType
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import androidx.navigation.navArgument
import com.example.quran_app.ui.navigation.Screen
import com.example.quran_app.ui.screens.home.HomeScreen
import com.example.quran_app.ui.screens.letters.ArabicLettersScreen
import com.example.quran_app.ui.screens.quran.AyahListScreen
import com.example.quran_app.ui.screens.quran.AyahRecitationScreen
import com.example.quran_app.ui.screens.quran.SurahListScreen
import com.example.quran_app.ui.theme.QuranappTheme
import com.example.quran_app.ui.viewmodel.ArabicLettersViewModel
import com.example.quran_app.ui.viewmodel.AyahRecitationViewModel
import java.net.URLDecoder

class MainActivity : ComponentActivity() {
    private lateinit var appContainer: AppContainer

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        appContainer = AppContainer(applicationContext)
        enableEdgeToEdge()
        setContent {
            QuranappTheme {
                val navController = rememberNavController()

                // Shared ViewModel scoped to this Activity — persists across all Quran screens
                val ayahViewModel: AyahRecitationViewModel = viewModel(
                    factory = object : ViewModelProvider.Factory {
                        @Suppress("UNCHECKED_CAST")
                        override fun <T : ViewModel> create(modelClass: Class<T>): T =
                            AyahRecitationViewModel(
                                appContainer.quranRepository,
                                appContainer.ayahRecorder,
                                appContainer.ayahWsClient
                            ) as T
                    }
                )

                NavHost(
                    navController    = navController,
                    startDestination = Screen.Home.route,
                    modifier         = Modifier.fillMaxSize()
                ) {

                    // ── Home ─────────────────────────────────────────────
                    composable(Screen.Home.route) {
                        HomeScreen(
                            onLetterLearning = { navController.navigate(Screen.ArabicLetters.route) },
                            onAyahRecitation = { navController.navigate(Screen.SurahList.route) }
                        )
                    }

                    // ── Arabic Letters ────────────────────────────────────
                    composable(Screen.ArabicLetters.route) {
                        val lettersViewModel: ArabicLettersViewModel = viewModel(
                            factory = object : ViewModelProvider.Factory {
                                @Suppress("UNCHECKED_CAST")
                                override fun <T : ViewModel> create(modelClass: Class<T>): T =
                                    ArabicLettersViewModel(
                                        appContainer.quranRepository,
                                        appContainer.audioPlayer,
                                        appContainer.letterRecorder,
                                        appContainer.wsClient
                                    ) as T
                            }
                        )
                        ArabicLettersScreen(
                            viewModel = lettersViewModel,
                            onBack    = { navController.popBackStack() }
                        )
                    }

                    // ── Surah list ────────────────────────────────────────
                    composable(Screen.SurahList.route) {
                        SurahListScreen(
                            viewModel       = ayahViewModel,
                            onBack          = { navController.popBackStack() },
                            onSurahSelected = { surahId, surahName ->
                                navController.navigate(Screen.AyahList.createRoute(surahId, surahName))
                            }
                        )
                    }

                    // ── Ayah list ─────────────────────────────────────────
                    composable(
                        route     = Screen.AyahList.route,
                        arguments = listOf(
                            navArgument("surahId")   { type = NavType.IntType },
                            navArgument("surahName") { type = NavType.StringType }
                        )
                    ) { backStackEntry ->
                        val surahId   = backStackEntry.arguments?.getInt("surahId") ?: return@composable
                        val surahName = URLDecoder.decode(
                            backStackEntry.arguments?.getString("surahName") ?: "", "UTF-8"
                        )
                        AyahListScreen(
                            surahId        = surahId,
                            surahName      = surahName,
                            viewModel      = ayahViewModel,
                            onBack         = { navController.popBackStack() },
                            onAyahSelected = { ayahId ->
                                navController.navigate(Screen.AyahRecitation.createRoute(ayahId))
                            }
                        )
                    }

                    // ── Ayah recitation ───────────────────────────────────
                    composable(
                        route     = Screen.AyahRecitation.route,
                        arguments = listOf(
                            navArgument("ayahId") { type = NavType.IntType }
                        )
                    ) { backStackEntry ->
                        val ayahId = backStackEntry.arguments?.getInt("ayahId") ?: return@composable
                        AyahRecitationScreen(
                            ayahId    = ayahId,
                            viewModel = ayahViewModel,
                            onBack    = { navController.popBackStack() }
                        )
                    }
                }
            }
        }
    }
}
