package com.example.quran_app

import android.content.Context
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
import com.example.quran_app.ui.screens.auth.LoginScreen
import com.example.quran_app.ui.screens.auth.RegisterScreen
import com.example.quran_app.ui.screens.home.HomeScreen
import com.example.quran_app.ui.screens.letters.ArabicLettersScreen
import com.example.quran_app.ui.screens.letters.SukoonLessonScreen
import com.example.quran_app.ui.screens.letters.TajweedProgressScreen
import com.example.quran_app.ui.screens.letters.TajweedScreen
import com.example.quran_app.ui.screens.namaz.NamazHubScreen
import com.example.quran_app.ui.screens.namaz.PostureLessonScreen
import com.example.quran_app.ui.theme.QuranappTheme
import com.example.quran_app.ui.viewmodel.ArabicLettersViewModel
import com.example.quran_app.ui.viewmodel.AuthViewModel
import com.example.quran_app.data.remote.RetrofitClient
import com.example.quran_app.ui.viewmodel.LeaderboardViewModel
import com.example.quran_app.ui.viewmodel.NamazHubViewModel
import com.example.quran_app.ui.viewmodel.PostureLessonViewModel
import com.example.quran_app.ui.viewmodel.ProfileViewModel
import com.example.quran_app.ui.viewmodel.ReciteViewModel
import com.example.quran_app.ui.viewmodel.TanweenLessonViewModel

class MainActivity : ComponentActivity() {
    private lateinit var appContainer: AppContainer

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        appContainer = AppContainer(applicationContext)
        enableEdgeToEdge()
        setContent {
            QuranappTheme {
                val navController = rememberNavController()

                NavHost(
                    navController    = navController,
                    startDestination = Screen.Home.route,   // always start on Home
                    modifier         = Modifier.fillMaxSize()
                ) {

                    // ── Home (with Profile tab wired) ─────────────────────────
                    composable(Screen.Home.route) {
                        val lessonVm: TanweenLessonViewModel = viewModel(
                            key = "tanween_home", factory = tanweenVmFactory())
                        val profileVm: ProfileViewModel = viewModel(
                            key = "profile", factory = profileVmFactory())
                        val reciteVm: ReciteViewModel = viewModel(
                            key = "recite", factory = reciteVmFactory())
                        val leaderboardVm: LeaderboardViewModel = viewModel(
                            key = "leaderboard", factory = leaderboardVmFactory())

                        // Reload profile when returning from login
                        val refreshProfile = it.savedStateHandle.get<Boolean>("refresh_profile")
                        if (refreshProfile == true) {
                            it.savedStateHandle.remove<Boolean>("refresh_profile")
                            profileVm.load()
                        }

                        HomeScreen(
                            onLetterLearning     = { navController.navigate(Screen.ArabicLetters.route) },
                            onTajweed            = { navController.navigate(Screen.Tajweed.route) },
                            onNamaz              = { navController.navigate(Screen.NamazHub.route) },
                            lessonViewModel      = lessonVm,
                            reciteViewModel      = reciteVm,
                            profileViewModel     = profileVm,
                            leaderboardViewModel = leaderboardVm,
                            onGoLogin            = { navController.navigate(Screen.Login.route) },
                            onGoRegister         = { navController.navigate(Screen.Register.route) },
                        )
                    }

                    // ── Auth (optional — accessed from Profile tab) ────────────
                    composable(Screen.Login.route) {
                        val authVm: AuthViewModel = viewModel(factory = authVmFactory())
                        LoginScreen(
                            viewModel      = authVm,
                            onLoginSuccess = {
                                // Reload profile then go back to Home
                                navController.previousBackStackEntry
                                    ?.savedStateHandle
                                    ?.set("refresh_profile", true)
                                navController.popBackStack()
                            },
                            onGoRegister = { navController.navigate(Screen.Register.route) },
                            onSkip       = { navController.popBackStack() }
                        )
                    }

                    composable(Screen.Register.route) {
                        val authVm: AuthViewModel = viewModel(factory = authVmFactory())
                        RegisterScreen(
                            viewModel         = authVm,
                            onRegisterSuccess = {
                                navController.popBackStack(Screen.Home.route, inclusive = false)
                            },
                            onGoLogin = { navController.popBackStack() }
                        )
                    }

                    // ── Letter learning ───────────────────────────────────────
                    composable(Screen.ArabicLetters.route) {
                        ArabicLettersScreen(
                            viewModel = viewModel(factory = lettersVmFactory()),
                            onBack    = { navController.popBackStack() }
                        )
                    }

                    composable(Screen.Tajweed.route) {
                        TajweedScreen(
                            viewModel   = viewModel(factory = lettersVmFactory()),
                            onBack      = { navController.popBackStack() },
                            onShowTable = { navController.navigate(Screen.TajweedProgress.route) }
                        )
                    }

                    composable(Screen.TajweedProgress.route) {
                        TajweedProgressScreen(
                            viewModel = viewModel(key = "tajweed", factory = lettersVmFactory()),
                            onBack    = { navController.popBackStack() }
                        )
                    }

                    composable(Screen.SukoonLesson.route) {
                        SukoonLessonScreen(
                            viewModel = viewModel(factory = lettersVmFactory()),
                            onBack    = { navController.popBackStack() }
                        )
                    }

                    // ── Namaz ─────────────────────────────────────────────────
                    composable(Screen.NamazHub.route) {
                        val namazVm: NamazHubViewModel = viewModel(
                            key = "namaz_hub", factory = namazHubVmFactory())
                        NamazHubScreen(
                            viewModel     = namazVm,
                            onBack        = { navController.popBackStack() },
                            onStartLesson = { step ->
                                navController.navigate(Screen.PostureLesson.route(step))
                            },
                        )
                    }

                    composable(
                        route     = Screen.PostureLesson.route,
                        arguments = listOf(navArgument("step") {
                            type         = NavType.IntType
                            defaultValue = 1
                        }),
                    ) { backStack ->
                        val step = backStack.arguments?.getInt("step") ?: 1
                        val postureVm: PostureLessonViewModel = viewModel(
                            key = "posture_$step", factory = postureLessonVmFactory(step))
                        PostureLessonScreen(
                            viewModel  = postureVm,
                            onBack     = { navController.popBackStack() },
                            onComplete = { navController.popBackStack(Screen.NamazHub.route, inclusive = false) },
                        )
                    }
                }
            }
        }
    }

    // ── ViewModel factories ────────────────────────────────────────────────────

    private fun reciteVmFactory() = object : ViewModelProvider.Factory {
        @Suppress("UNCHECKED_CAST")
        override fun <T : ViewModel> create(c: Class<T>): T =
            ReciteViewModel(
                recorder    = appContainer.letterRecorder,
                analyzer    = appContainer.recitationAnalyzer,
                repository  = appContainer.quranRepository,
                audioPlayer = appContainer.audioPlayer,
            ) as T
    }

    private fun authVmFactory() = object : ViewModelProvider.Factory {
        @Suppress("UNCHECKED_CAST")
        override fun <T : ViewModel> create(c: Class<T>): T =
            AuthViewModel(appContainer.authRepository) as T
    }

    private fun profileVmFactory() = object : ViewModelProvider.Factory {
        @Suppress("UNCHECKED_CAST")
        override fun <T : ViewModel> create(c: Class<T>): T =
            ProfileViewModel(
                RetrofitClient.apiService,
                appContainer.tokenManager,
                appContainer.authRepository
            ) as T
    }

    private fun lettersVmFactory() = object : ViewModelProvider.Factory {
        @Suppress("UNCHECKED_CAST")
        override fun <T : ViewModel> create(c: Class<T>): T =
            ArabicLettersViewModel(
                appContainer.quranRepository,
                appContainer.audioPlayer,
                appContainer.letterRecorder,
                appContainer.wsClient
            ) as T
    }

    private fun tanweenVmFactory() = object : ViewModelProvider.Factory {
        @Suppress("UNCHECKED_CAST")
        override fun <T : ViewModel> create(c: Class<T>): T =
            TanweenLessonViewModel(
                appContainer.quranRepository,
                appContainer.audioPlayer,
                appContainer.letterRecorder,
                appContainer.wsClient
            ) as T
    }

    private fun leaderboardVmFactory() = object : ViewModelProvider.Factory {
        @Suppress("UNCHECKED_CAST")
        override fun <T : ViewModel> create(c: Class<T>): T =
            LeaderboardViewModel(RetrofitClient.apiService) as T
    }

    private fun namazHubVmFactory() = object : ViewModelProvider.Factory {
        @Suppress("UNCHECKED_CAST")
        override fun <T : ViewModel> create(c: Class<T>): T =
            NamazHubViewModel(
                applicationContext.getSharedPreferences("namaz_prefs", Context.MODE_PRIVATE)
            ) as T
    }

    private fun postureLessonVmFactory(step: Int) = object : ViewModelProvider.Factory {
        @Suppress("UNCHECKED_CAST")
        override fun <T : ViewModel> create(c: Class<T>): T =
            PostureLessonViewModel(step) as T
    }
}
