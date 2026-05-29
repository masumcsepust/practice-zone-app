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
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.example.quran_app.ui.navigation.Screen
import com.example.quran_app.ui.screens.letters.ArabicLettersScreen
import com.example.quran_app.ui.theme.QuranappTheme
import com.example.quran_app.ui.viewmodel.ArabicLettersViewModel

class MainActivity : ComponentActivity() {
    private lateinit var appContainer: AppContainer

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        appContainer = AppContainer(applicationContext)
        enableEdgeToEdge()
        setContent {
            QuranappTheme {
                // No Scaffold here — ArabicLettersScreen owns the full-bleed gradient
                // and calls statusBarsPadding() / navigationBarsPadding() itself.
                val navController = rememberNavController()
                NavHost(
                    navController     = navController,
                    startDestination  = Screen.ArabicLetters.route,
                    modifier          = Modifier.fillMaxSize()
                ) {
                    composable(Screen.ArabicLetters.route) {
                        val viewModel: ArabicLettersViewModel = viewModel(
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
                        ArabicLettersScreen(viewModel)
                    }
                }
            }
        }
    }
}
