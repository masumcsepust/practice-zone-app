package com.example.quran_app.ui.screens.letters

import android.Manifest
import android.content.pm.PackageManager
import android.graphics.Bitmap
import android.util.Base64
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.animation.AnimatedContent
import androidx.compose.animation.core.*
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.animation.togetherWith
import androidx.compose.foundation.Canvas
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.gestures.detectDragGestures
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyRow
import androidx.compose.foundation.lazy.itemsIndexed
import androidx.compose.foundation.lazy.rememberLazyListState
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.scale
import androidx.compose.ui.geometry.CornerRadius
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.geometry.Size
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.Path
import androidx.compose.ui.graphics.drawscope.Stroke
import androidx.compose.ui.graphics.nativeCanvas
import androidx.compose.ui.graphics.toArgb
import androidx.compose.ui.hapticfeedback.HapticFeedbackType
import androidx.compose.ui.input.pointer.pointerInput
import androidx.compose.foundation.lazy.grid.GridCells
import androidx.compose.foundation.lazy.grid.GridItemSpan
import androidx.compose.foundation.lazy.grid.LazyVerticalGrid
import androidx.compose.foundation.lazy.grid.itemsIndexed
import androidx.compose.foundation.lazy.grid.rememberLazyGridState
import androidx.compose.ui.platform.LocalConfiguration
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.platform.LocalDensity
import androidx.compose.ui.platform.LocalHapticFeedback
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.core.content.ContextCompat
import com.example.quran_app.domain.model.ArabicLetter
import com.example.quran_app.domain.model.DrawingResult
import com.example.quran_app.domain.model.DrawingState
import com.example.quran_app.domain.model.LetterForms
import com.example.quran_app.domain.model.PronunciationResult
import com.example.quran_app.domain.model.SpeakState
import com.example.quran_app.ui.theme.*
import com.example.quran_app.ui.viewmodel.ArabicLettersViewModel
import java.io.ByteArrayOutputStream

// ── Unified green theme (বলুন color used everywhere) ─────────────────────
private val SpeakAccent      = Color(0xFF16A34A)   // green-600 — readable on white
private val SpeakDark        = Color(0xFF14532D)   // green-900 — dark heading text
private val SpeakLight       = Color(0xFFF0FDF4)
private val SpeakBorder      = Color(0xFFBBF7D0)
private val SpeakRecording   = Color(0xFFEF4444)
private val SpeakSuccess     = Color(0xFF16A34A)
private val SpeakFail        = Color(0xFFDC2626)

// রূপ panel — same green theme
private val FormsAccent      = SpeakAccent
private val FormsDark        = SpeakDark
private val FormsLight       = SpeakLight
private val FormsBorder      = SpeakBorder
private val FormsBadgeBg     = Color(0xFFD1FAE5)
private val FormsCardBg      = Color(0xFFECFDF5)

// লিখুন panel — same amber theme
private val WriteAccent      = SpeakAccent
private val WriteDark        = SpeakDark
private val WriteLight       = SpeakLight
private val WriteBorder      = SpeakBorder
private val WriteSuccess     = SpeakSuccess
private val WriteFail        = SpeakFail

// ─────────────────────────────────────────────────────────────────────────────
//  Root screen
// ─────────────────────────────────────────────────────────────────────────────

@Composable
fun ArabicLettersScreen(viewModel: ArabicLettersViewModel, onBack: () -> Unit = {}) {
    val letters         by viewModel.letters.collectAsState()
    val selectedLetter  by viewModel.selectedLetter.collectAsState()
    val isLoading       by viewModel.isLoading.collectAsState()
    val error           by viewModel.error.collectAsState()
    val letterForms     by viewModel.letterForms.collectAsState()
    val formsLoading    by viewModel.formsLoading.collectAsState()
    val formsError      by viewModel.formsError.collectAsState()
    val speakState      by viewModel.speakState.collectAsState()
    val drawingState    by viewModel.drawingState.collectAsState()
    val totalCount      by viewModel.totalCount.collectAsState()
    val isLoadingMore   by viewModel.isLoadingMore.collectAsState()

    var activeTab by remember { mutableStateOf("পাঠ") }
    val selectedIndex  = if (selectedLetter != null) letters.indexOf(selectedLetter) else 0
    val isFirst        = selectedIndex <= 0
    // isLast uses the server total so "all done" only shows after every page is loaded + navigated
    val isLast         = letters.isNotEmpty()
                         && selectedIndex >= letters.size - 1
                         && totalCount > 0
                         && letters.size >= totalCount

    LaunchedEffect(activeTab, selectedLetter) {
        if (activeTab == "রূপ")   selectedLetter?.let { viewModel.fetchLetterForms(it.id) }
        if (activeTab != "বলুন")  viewModel.resetSpeak()
        if (activeTab != "লিখুন") viewModel.resetDrawing()
    }

    Box(
        modifier = Modifier.fillMaxSize()
            .background(Brush.verticalGradient(listOf(AppPrimary, AppSecondary, AppAccent, AppBgLight)))
    ) {
        DecorativeCircles()

        when {
            isLoading && letters.isEmpty() ->
                CircularProgressIndicator(Modifier.align(Alignment.Center), color = Color.White, strokeWidth = 3.dp)

            error != null && letters.isEmpty() ->
                ErrorState(error ?: "", { viewModel.fetchLetters() }, Modifier.align(Alignment.Center))

            else -> if (LocalConfiguration.current.screenWidthDp >= 600) {
                TabletLayout(
                    vm             = viewModel,
                    letters        = letters,
                    selectedLetter = selectedLetter,
                    selectedIndex  = selectedIndex,
                    isFirst        = isFirst,
                    isLast         = isLast,
                    activeTab      = activeTab,
                    onTabSelect    = { activeTab = it },
                    letterForms    = letterForms,
                    formsLoading   = formsLoading,
                    formsError     = formsError,
                    speakState     = speakState,
                    drawingState   = drawingState,
                    totalCount     = totalCount,
                    isLoadingMore  = isLoadingMore,
                    onBack         = onBack,
                )
            } else {
                PhoneLayout(
                    vm             = viewModel,
                    letters        = letters,
                    selectedLetter = selectedLetter,
                    selectedIndex  = selectedIndex,
                    isFirst        = isFirst,
                    isLast         = isLast,
                    activeTab      = activeTab,
                    onTabSelect    = { activeTab = it },
                    letterForms    = letterForms,
                    formsLoading   = formsLoading,
                    formsError     = formsError,
                    speakState     = speakState,
                    drawingState   = drawingState,
                    totalCount     = totalCount,
                    isLoadingMore  = isLoadingMore,
                    onBack         = onBack,
                )
            }
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
//  বলুন  —  Speak / Record panel
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun SpeakPanel(
    letter:    ArabicLetter?,
    speakState: SpeakState,
    onStart:   () -> Unit,
    onStop:    () -> Unit,
    onReset:   () -> Unit
) {
    val context = LocalContext.current
    val isTablet = LocalConfiguration.current.screenWidthDp >= 600
    
    val permLauncher = rememberLauncherForActivityResult(
        ActivityResultContracts.RequestPermission()
    ) { granted ->
        if (granted) onStart()
    }

    Card(
        modifier  = Modifier.fillMaxWidth().padding(horizontal = if (isTablet) 0.dp else 16.dp),
        shape     = RoundedCornerShape(24.dp),
        colors    = CardDefaults.cardColors(containerColor = SpeakLight),
        elevation = CardDefaults.cardElevation(8.dp)
    ) {
        Column(
            modifier            = Modifier.padding(24.dp),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            // ── Header ───────────────────────────────────────────────────
            Row(
                Modifier.fillMaxWidth(),
                Arrangement.SpaceBetween,
                Alignment.CenterVertically
            ) {
                Column {
                    Text("উচ্চারণ পরীক্ষা", fontSize = 18.sp, fontWeight = FontWeight.ExtraBold, color = SpeakDark)
                    letter?.let {
                        Text("${it.letter}  ·  ${it.nameEnglish}",
                            fontSize = 14.sp, color = SpeakAccent.copy(.8f), fontWeight = FontWeight.SemiBold)
                    }
                }
                Box(
                    Modifier.background(Color(0xFFFEF3C7), RoundedCornerShape(20.dp)).padding(horizontal=12.dp, vertical=6.dp)
                ) { Text("🎙️ বলুন", fontSize = 11.sp, fontWeight = FontWeight.Bold, color = SpeakDark) }
            }

            Spacer(Modifier.height(24.dp))

            if (isTablet && (speakState is SpeakState.Idle || speakState is SpeakState.Recording || speakState is SpeakState.Processing)) {
                Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(24.dp)) {
                    // Column 1: Target Info
                    Column(Modifier.weight(0.4f), horizontalAlignment = Alignment.CenterHorizontally) {
                        Text("সঠিক উচ্চারণ লক্ষ্য করুন", fontSize = 14.sp, color = SpeakDark, fontWeight = FontWeight.SemiBold)
                        Spacer(Modifier.height(16.dp))
                        Box(
                            Modifier
                                .fillMaxWidth()
                                .aspectRatio(1f)
                                .background(Color.White, RoundedCornerShape(20.dp))
                                .border(1.5.dp, SpeakBorder, RoundedCornerShape(20.dp)),
                            Alignment.Center
                        ) {
                            Column(horizontalAlignment = Alignment.CenterHorizontally) {
                                Text(letter?.letter ?: "", fontSize = 90.sp, fontWeight = FontWeight.Bold, color = SpeakDark)
                                Text(letter?.nameBangla ?: "", fontSize = 18.sp, color = SpeakAccent, fontWeight = FontWeight.Bold)
                            }
                        }
                        Spacer(Modifier.height(16.dp))
                        Text("উপরের বর্ণটি দেখে সঠিক উচ্চারণের চেষ্টা করুন", fontSize = 12.sp, color = SlateGrey, textAlign = TextAlign.Center)
                    }
                    
                    // Column 2: User Action
                    Column(Modifier.weight(0.6f), horizontalAlignment = Alignment.CenterHorizontally) {
                        when (speakState) {
                            is SpeakState.Idle -> SpeakIdleContent(onRecord = {
                                val hasPermission = ContextCompat.checkSelfPermission(
                                    context, Manifest.permission.RECORD_AUDIO
                                ) == PackageManager.PERMISSION_GRANTED
                                if (hasPermission) onStart() else permLauncher.launch(Manifest.permission.RECORD_AUDIO)
                            })
                            is SpeakState.Recording -> SpeakRecording(onStop = onStop)
                            is SpeakState.Processing -> SpeakProcessing()
                            else -> {}
                        }
                    }
                }
            } else {
                when (speakState) {
                    is SpeakState.Idle       -> SpeakIdle(letter, onRecord = {
                        val hasPermission = ContextCompat.checkSelfPermission(
                            context, Manifest.permission.RECORD_AUDIO
                        ) == PackageManager.PERMISSION_GRANTED
                        if (hasPermission) onStart() else permLauncher.launch(Manifest.permission.RECORD_AUDIO)
                    })
                    is SpeakState.Recording  -> SpeakRecording(onStop = onStop)
                    is SpeakState.Processing -> SpeakProcessing()
                    is SpeakState.Result     -> SpeakResult(speakState.data, onReset)
                    is SpeakState.Error      -> SpeakError(speakState.message, onReset)
                }
            }
        }
    }
}

@Composable
private fun SpeakIdleContent(onRecord: () -> Unit) {
    val haptic = LocalHapticFeedback.current
    Column(horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(16.dp)) {
        Spacer(Modifier.height(20.dp))
        Text(
            "নিচের বোতামে চাপ দিয়ে এখন বর্ণটি জোরে বলুন",
            fontSize = 14.sp, color = SlateGrey, textAlign = TextAlign.Center
        )
        // Mic button with haptic
        Box(
            modifier = Modifier
                .size(100.dp)
                .background(Brush.linearGradient(listOf(SpeakDark, SpeakAccent)), CircleShape)
                .clickable {
                    haptic.performHapticFeedback(HapticFeedbackType.LongPress)
                    onRecord()
                },
            contentAlignment = Alignment.Center
        ) {
            Text("🎙️", fontSize = 42.sp)
        }
        Text("ট্যাপ করে শুরু করুন", fontSize = 13.sp, color = SpeakAccent, fontWeight = FontWeight.Bold)
    }
}

// ── Idle state: show letter + big mic button ──────────────────────────────────
@Composable
private fun SpeakIdle(letter: ArabicLetter?, onRecord: () -> Unit) {
    val haptic = LocalHapticFeedback.current
    Column(horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(12.dp)) {
        // Compact letter display card
        letter?.let {
            Box(
                Modifier
                    .fillMaxWidth()
                    .background(Color.White, RoundedCornerShape(14.dp))
                    .border(1.5.dp, SpeakBorder, RoundedCornerShape(14.dp))
                    .padding(vertical = 12.dp, horizontal = 16.dp),
                contentAlignment = Alignment.Center
            ) {
                Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                    Column(verticalArrangement = Arrangement.spacedBy(2.dp)) {
                        Text(it.nameEnglish, fontSize = 16.sp, fontWeight = FontWeight.ExtraBold, color = SpeakDark)
                        Text(it.transliteration, fontSize = 12.sp, color = SlateGrey)
                        Text(it.nameBangla, fontSize = 11.sp, color = SpeakAccent, fontWeight = FontWeight.SemiBold)
                    }
                    Text(it.letter, fontSize = 64.sp, fontWeight = FontWeight.Bold, color = SpeakDark)
                }
            }
        }
        Text(
            "নিচের বোতামে চাপ দিয়ে এখন বর্ণটি জোরে বলুন",
            fontSize = 12.sp, color = SlateGrey, textAlign = TextAlign.Center
        )
        // Mic button with haptic
        Box(
            modifier = Modifier
                .size(84.dp)
                .background(Brush.linearGradient(listOf(SpeakDark, SpeakAccent)), CircleShape)
                .clickable {
                    haptic.performHapticFeedback(HapticFeedbackType.LongPress)
                    onRecord()
                },
            contentAlignment = Alignment.Center
        ) {
            Text("🎙️", fontSize = 36.sp)
        }
        Text("ট্যাপ করে শুরু করুন", fontSize = 11.sp, color = SpeakAccent, fontWeight = FontWeight.SemiBold)
    }
}

// ── Recording state: pulsing red mic + stop button ───────────────────────────
@Composable
private fun SpeakRecording(onStop: () -> Unit) {
    val infiniteTransition = rememberInfiniteTransition(label = "pulse")
    val scale by infiniteTransition.animateFloat(
        initialValue = 1f, targetValue = 1.18f, label = "scale",
        animationSpec = infiniteRepeatable(tween(700), RepeatMode.Reverse)
    )
    val alpha by infiniteTransition.animateFloat(
        initialValue = .4f, targetValue = 1f, label = "alpha",
        animationSpec = infiniteRepeatable(tween(700), RepeatMode.Reverse)
    )

    Column(horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(16.dp)) {
        Box(contentAlignment = Alignment.Center) {
            // Outer pulse ring
            Box(
                Modifier
                    .size(100.dp)
                    .scale(scale)
                    .background(SpeakRecording.copy(alpha = alpha * .25f), CircleShape)
            )
            // Inner mic button
            Box(
                Modifier
                    .size(80.dp)
                    .background(SpeakRecording, CircleShape)
                    .clickable { onStop() },
                contentAlignment = Alignment.Center
            ) {
                Text("⏹️", fontSize = 28.sp)
            }
        }
        Text("রেকর্ড হচ্ছে...", fontSize = 14.sp, fontWeight = FontWeight.Bold, color = SpeakRecording)
        Text("উচ্চারণ শেষ হলে বোতামটি আবার ট্যাপ করুন", fontSize = 11.sp, color = SlateGrey, textAlign = TextAlign.Center)
    }
}

// ── Processing state: spinner ────────────────────────────────────────────────
@Composable
private fun SpeakProcessing() {
    Column(horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(12.dp),
        modifier = Modifier.padding(vertical = 24.dp)
    ) {
        CircularProgressIndicator(color = SpeakAccent, strokeWidth = 3.dp, modifier = Modifier.size(56.dp))
        Text("বিশ্লেষণ হচ্ছে...", fontSize = 14.sp, fontWeight = FontWeight.Bold, color = SpeakDark)
        Text("আপনার উচ্চারণ যাচাই করা হচ্ছে", fontSize = 11.sp, color = SlateGrey)
    }
}

// ── Result state ─────────────────────────────────────────────────────────────
@Composable
private fun SpeakResult(result: PronunciationResult, onReset: () -> Unit) {
    val isCorrect = result.isCorrect
    val scoreColor = when {
        result.score >= 85 -> SpeakSuccess
        result.score >= 60 -> SpeakAccent
        else               -> SpeakFail
    }

    Column(verticalArrangement = Arrangement.spacedBy(12.dp)) {

        // ── Big result badge ──────────────────────────────────────────────
        Box(
            Modifier
                .fillMaxWidth()
                .background(
                    if (isCorrect) Color(0xFFF0FDF4) else Color(0xFFFFF1F2),
                    RoundedCornerShape(14.dp)
                )
                .border(2.dp, if (isCorrect) SpeakSuccess else SpeakFail, RoundedCornerShape(14.dp))
                .padding(16.dp),
            contentAlignment = Alignment.Center
        ) {
            Column(horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(8.dp)) {
                Text(if (isCorrect) "✅ সঠিক!" else "❌ ভুল", fontSize = 20.sp, fontWeight = FontWeight.ExtraBold,
                    color = if (isCorrect) SpeakSuccess else SpeakFail)

                // Wrong-letter comparison: heard → expected
                if (!isCorrect && result.recognizedText.isNotBlank() && result.letter.isNotBlank()) {
                    Row(
                        horizontalArrangement = Arrangement.spacedBy(16.dp),
                        verticalAlignment = Alignment.CenterVertically
                    ) {
                        Column(horizontalAlignment = Alignment.CenterHorizontally) {
                            Text("আপনি বললেন", fontSize = 9.sp, color = SlateGrey, fontWeight = FontWeight.SemiBold)
                            Text(result.recognizedText, fontSize = 32.sp, fontWeight = FontWeight.Bold,
                                color = SpeakFail, fontFamily = androidx.compose.ui.text.font.FontFamily.Default)
                        }
                        Text("→", fontSize = 18.sp, color = SlateGrey, fontWeight = FontWeight.Bold)
                        Column(horizontalAlignment = Alignment.CenterHorizontally) {
                            Text("হওয়া উচিত", fontSize = 9.sp, color = SlateGrey, fontWeight = FontWeight.SemiBold)
                            Text(result.letter, fontSize = 32.sp, fontWeight = FontWeight.Bold,
                                color = SpeakSuccess, fontFamily = androidx.compose.ui.text.font.FontFamily.Default)
                            Text(result.letterName, fontSize = 10.sp, color = SpeakSuccess)
                        }
                    }
                } else if (isCorrect) {
                    Text(result.recognizedText.ifBlank { "(কিছু শোনা যায়নি)" },
                        fontSize = 13.sp, color = SlateGrey, textAlign = TextAlign.Center)
                } else {
                    Text("(কিছু শোনা যায়নি)", fontSize = 13.sp, color = SlateGrey, textAlign = TextAlign.Center)
                }
            }
        }

        // ── Score row ─────────────────────────────────────────────────────
        Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(8.dp)) {
            ScorePill("উচ্চারণ", "${result.score.toInt()}%", scoreColor, Modifier.weight(1f))
            ScorePill("নির্ভুলতা", "${result.accuracyScore.toInt()}%", scoreColor, Modifier.weight(1f))
        }

        // ── Feedback ──────────────────────────────────────────────────────
        Column(
            Modifier
                .fillMaxWidth()
                .background(Color.White, RoundedCornerShape(12.dp))
                .border(1.dp, Color(0xFFE2E8F0), RoundedCornerShape(12.dp))
                .padding(12.dp),
            verticalArrangement = Arrangement.spacedBy(6.dp)
        ) {
            Text("💬 মতামত", fontSize = 11.sp, fontWeight = FontWeight.Bold, color = NavyText)
            Text(result.feedback, fontSize = 12.sp, color = NavyText, lineHeight = 18.sp)
        }

        // ── Makhraj hint ──────────────────────────────────────────────────
        if (result.makhrajHint.isNotBlank()) {
            Row(
                Modifier
                    .fillMaxWidth()
                    .background(SkyBlueAlert, RoundedCornerShape(12.dp))
                    .border(1.dp, Color(0xFFBAE6FD), RoundedCornerShape(12.dp))
                    .padding(10.dp),
                horizontalArrangement = Arrangement.spacedBy(8.dp),
                verticalAlignment = Alignment.Top
            ) {
                Text("ℹ️", fontSize = 14.sp)
                Text(result.makhrajHint, fontSize = 11.sp, fontWeight = FontWeight.SemiBold,
                    color = SkyBlueDark, lineHeight = 16.sp)
            }
        }

        // ── Try again ─────────────────────────────────────────────────────
        Box(
            modifier = Modifier
                .fillMaxWidth()
                .clip(RoundedCornerShape(14.dp))
                .background(Brush.linearGradient(listOf(SpeakDark, SpeakAccent)))
                .clickable { onReset() }
                .padding(vertical = 12.dp),
            contentAlignment = Alignment.Center
        ) {
            Text("🔄 আবার চেষ্টা করুন", fontSize = 13.sp, fontWeight = FontWeight.Bold, color = Color.White)
        }
    }
}

@Composable
private fun ScorePill(label: String, value: String, color: Color, modifier: Modifier = Modifier) {
    Column(
        modifier = modifier
            .background(color.copy(.08f), RoundedCornerShape(12.dp))
            .border(1.dp, color.copy(.3f), RoundedCornerShape(12.dp))
            .padding(10.dp),
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Text(value, fontSize = 22.sp, fontWeight = FontWeight.ExtraBold, color = color)
        Text(label, fontSize = 10.sp, color = SlateGrey, fontWeight = FontWeight.SemiBold)
    }
}

// ── Error state ───────────────────────────────────────────────────────────────
@Composable
private fun SpeakError(message: String, onReset: () -> Unit) {
    Column(
        Modifier.padding(vertical = 12.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(12.dp)
    ) {
        Text("⚠️ $message", fontSize = 12.sp, color = SpeakFail, textAlign = TextAlign.Center)
        Box(
            Modifier.clip(RoundedCornerShape(12.dp)).background(SpeakAccent).clickable { onReset() }.padding(horizontal = 24.dp, vertical = 10.dp)
        ) {
            Text("আবার চেষ্টা করুন", fontSize = 12.sp, color = Color.White, fontWeight = FontWeight.Bold)
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
//  লিখুন  —  Drawing / Writing panel
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun LikhunPanel(
    letter:       ArabicLetter?,
    drawingState: DrawingState,
    onSubmit:     (String) -> Unit,   // base64 PNG
    onReset:      () -> Unit
) {
    val isTablet = LocalConfiguration.current.screenWidthDp >= 600
    // Track all strokes: each stroke is a list of Offsets
    var strokes     by remember { mutableStateOf(listOf<List<Offset>>()) }
    var currentStroke by remember { mutableStateOf(listOf<Offset>()) }
    val hasStrokes = strokes.isNotEmpty()

    // Reset canvas when drawingState goes back to Idle (e.g. after "try again")
    LaunchedEffect(drawingState) {
        if (drawingState is DrawingState.Idle) {
            strokes       = emptyList()
            currentStroke = emptyList()
        }
    }

    // Canvas size in px (captured on first layout)
    var canvasPx by remember { mutableStateOf(Pair(0, 0)) }   // width, height

    Card(
        modifier  = Modifier.fillMaxWidth().padding(horizontal = if (isTablet) 0.dp else 16.dp),
        shape     = RoundedCornerShape(24.dp),
        colors    = CardDefaults.cardColors(containerColor = WriteLight),
        elevation = CardDefaults.cardElevation(8.dp)
    ) {
        Column(
            modifier            = Modifier.padding(24.dp),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            // ── Header ───────────────────────────────────────────────────
            Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                Column {
                    Text("হাতে লেখা পরীক্ষা", fontSize = 18.sp, fontWeight = FontWeight.ExtraBold, color = WriteDark)
                    letter?.let {
                        Text("${it.letter}  ·  ${it.nameEnglish}",
                            fontSize = 14.sp, color = WriteAccent.copy(.8f), fontWeight = FontWeight.SemiBold)
                    }
                }
                Box(
                    Modifier
                        .clip(RoundedCornerShape(12.dp))
                        .background(WriteAccent.copy(.12f))
                        .padding(horizontal = 12.dp, vertical = 6.dp)
                ) {
                    Text("✏️ লিখুন", fontSize = 12.sp, color = WriteAccent, fontWeight = FontWeight.Bold)
                }
            }

            Spacer(Modifier.height(24.dp))

            when (drawingState) {
                is DrawingState.Idle -> {
                    if (isTablet) {
                        Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(24.dp)) {
                            // Reference Column
                            Column(Modifier.weight(0.4f), horizontalAlignment = Alignment.CenterHorizontally) {
                                Text("বর্ণটি অনুসরণ করুন", fontSize = 14.sp, color = WriteDark, fontWeight = FontWeight.SemiBold)
                                Spacer(Modifier.height(16.dp))
                                Box(
                                    Modifier
                                        .fillMaxWidth()
                                        .aspectRatio(1f)
                                        .background(Color.White, RoundedCornerShape(20.dp))
                                        .border(1.dp, WriteBorder, RoundedCornerShape(20.dp)),
                                    Alignment.Center
                                ) {
                                    Text(letter?.letter ?: "", fontSize = 120.sp, color = WriteAccent.copy(0.15f))
                                    Text(letter?.letter ?: "", fontSize = 80.sp, fontWeight = FontWeight.Bold, color = WriteDark)
                                }
                                Spacer(Modifier.height(16.dp))
                                Text("নিচের ক্যানভাসে এই বর্ণটি আঁকুন", fontSize = 12.sp, color = SlateGrey, textAlign = TextAlign.Center)
                            }
                            
                            // Drawing Column
                            Column(Modifier.weight(0.6f)) {
                                DrawingCanvas(
                                    strokes        = strokes,
                                    currentStroke  = currentStroke,
                                    letter         = letter,
                                    onStrokeStart  = { offset -> currentStroke = listOf(offset) },
                                    onStrokeDrag   = { offset -> currentStroke = currentStroke + offset },
                                    onStrokeEnd    = { strokes = strokes + listOf(currentStroke); currentStroke = emptyList() },
                                    onSizeCapture  = { w, h -> canvasPx = Pair(w, h) }
                                )
                                Spacer(Modifier.height(16.dp))
                                DrawingActions(
                                    hasStrokes = hasStrokes,
                                    onClear    = { strokes = emptyList(); currentStroke = emptyList() },
                                    onSubmit   = {
                                        val b64 = strokesToBitmap(strokes, canvasPx.first, canvasPx.second)
                                        onSubmit(b64)
                                    }
                                )
                            }
                        }
                    } else {
                        DrawingCanvas(
                            strokes        = strokes,
                            currentStroke  = currentStroke,
                            letter         = letter,
                            onStrokeStart  = { offset -> currentStroke = listOf(offset) },
                            onStrokeDrag   = { offset -> currentStroke = currentStroke + offset },
                            onStrokeEnd    = { strokes = strokes + listOf(currentStroke); currentStroke = emptyList() },
                            onSizeCapture  = { w, h -> canvasPx = Pair(w, h) }
                        )
                        Spacer(Modifier.height(12.dp))
                        DrawingActions(
                            hasStrokes = hasStrokes,
                            onClear    = { strokes = emptyList(); currentStroke = emptyList() },
                            onSubmit   = {
                                val b64 = strokesToBitmap(strokes, canvasPx.first, canvasPx.second)
                                onSubmit(b64)
                            }
                        )
                    }
                }

                is DrawingState.Submitting -> {
                    if (isTablet) {
                        Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(24.dp)) {
                             Box(Modifier.weight(0.4f).aspectRatio(1f), Alignment.Center) {
                                 DrawingCanvasStatic(strokes = strokes, letter = letter)
                             }
                             Column(Modifier.weight(0.6f), Arrangement.Center, Alignment.CenterHorizontally) {
                                CircularProgressIndicator(color = WriteAccent, strokeWidth = 4.dp)
                                Spacer(Modifier.height(16.dp))
                                Text("AI বিশ্লেষণ করছে...", fontSize = 16.sp, color = WriteDark, fontWeight = FontWeight.Bold)
                                Text("আপনার লেখা যাচাই হচ্ছে", fontSize = 13.sp, color = WriteAccent.copy(.7f))
                             }
                        }
                    } else {
                        DrawingCanvasStatic(strokes = strokes, letter = letter)
                        Spacer(Modifier.height(16.dp))
                        CircularProgressIndicator(color = WriteAccent, strokeWidth = 3.dp)
                        Spacer(Modifier.height(8.dp))
                        Text("AI বিশ্লেষণ করছে...", fontSize = 13.sp, color = WriteDark, fontWeight = FontWeight.SemiBold)
                        Text("আপনার লেখা যাচাই হচ্ছে", fontSize = 11.sp, color = WriteAccent.copy(.7f))
                    }
                    Spacer(Modifier.height(12.dp))
                }

                is DrawingState.Result -> {
                    DrawingResultView(result = drawingState.data, onRetry = onReset)
                }

                is DrawingState.Error -> {
                    Spacer(Modifier.height(8.dp))
                    Text("⚠️ ${drawingState.message}",
                        fontSize = 14.sp, color = WriteFail, textAlign = TextAlign.Center,
                        modifier = Modifier.padding(horizontal = 8.dp))
                    Spacer(Modifier.height(16.dp))
                    Box(Modifier.clip(RoundedCornerShape(12.dp)).background(WriteAccent).clickable { onReset() }.padding(horizontal = 24.dp, vertical = 10.dp)) {
                        Text("আবার চেষ্টা করুন", fontSize = 14.sp, color = Color.White, fontWeight = FontWeight.Bold)
                    }
                }
                else -> {}
            }
        }
    }
}

// ── Drawing canvas (interactive) ─────────────────────────────────────────────

@Composable
private fun DrawingCanvas(
    strokes:       List<List<Offset>>,
    currentStroke: List<Offset>,
    letter:        ArabicLetter?,
    onStrokeStart: (Offset) -> Unit,
    onStrokeDrag:  (Offset) -> Unit,
    onStrokeEnd:   () -> Unit,
    onSizeCapture: (Int, Int) -> Unit
) {
    val density = LocalDensity.current
    val canvasH = 260.dp
    val canvasPx = with(density) { canvasH.toPx().toInt() }

    Box(
        Modifier
            .fillMaxWidth()
            .height(canvasH)
            .clip(RoundedCornerShape(16.dp))
            .border(2.dp, WriteBorder, RoundedCornerShape(16.dp))
    ) {
        Canvas(
            modifier = Modifier
                .fillMaxSize()
                .background(Color.White)
                .pointerInput(Unit) {
                    // Capture actual px size once
                    onSizeCapture(size.width, size.height)
                    detectDragGestures(
                        onDragStart = { onStrokeStart(it) },
                        onDrag      = { change, _ -> onStrokeDrag(change.position) },
                        onDragEnd   = { onStrokeEnd() }
                    )
                }
        ) {
            // Ghost guide letter
            if (strokes.isEmpty() && currentStroke.isEmpty() && letter != null) {
                drawContext.canvas.nativeCanvas.apply {
                    val paint = android.graphics.Paint().apply {
                        color     = android.graphics.Color.argb(72, 37, 99, 235)
                        textSize  = size.height * 0.55f
                        textAlign = android.graphics.Paint.Align.CENTER
                        isAntiAlias = true
                    }
                    drawText(letter.letter, size.width / 2f, size.height * 0.72f, paint)
                }
            }

            // Committed strokes
            strokes.forEach { stroke -> drawStroke(stroke, WriteAccent.copy(.9f)) }
            // Live stroke
            drawStroke(currentStroke, WriteAccent)
        }

        // Corner hint
        if (strokes.isEmpty() && currentStroke.isEmpty()) {
            Text(
                "এখানে আঁকুন",
                modifier = Modifier.align(Alignment.BottomCenter).padding(bottom = 8.dp),
                fontSize = 11.sp, color = WriteAccent.copy(.5f), fontWeight = FontWeight.Medium
            )
        }
    }
}

/** Static (non-interactive) canvas for Submitting state preview. */
@Composable
private fun DrawingCanvasStatic(strokes: List<List<Offset>>, letter: ArabicLetter?) {
    Box(
        Modifier
            .fillMaxWidth()
            .height(180.dp)
            .clip(RoundedCornerShape(16.dp))
            .border(2.dp, WriteBorder, RoundedCornerShape(16.dp))
    ) {
        Canvas(Modifier.fillMaxSize().background(Color.White)) {
            strokes.forEach { drawStroke(it, WriteAccent.copy(.7f)) }
        }
    }
}

private fun androidx.compose.ui.graphics.drawscope.DrawScope.drawStroke(
    stroke: List<Offset>,
    color:  Color
) {
    if (stroke.size < 2) {
        if (stroke.size == 1)
            drawCircle(color, radius = 5f, center = stroke[0])
        return
    }
    val path = Path().apply {
        moveTo(stroke[0].x, stroke[0].y)
        stroke.drop(1).forEach { lineTo(it.x, it.y) }
    }
    drawPath(path, color, style = Stroke(width = 8f, cap = androidx.compose.ui.graphics.StrokeCap.Round,
        join = androidx.compose.ui.graphics.StrokeJoin.Round))
}

// ── Action buttons ────────────────────────────────────────────────────────────

@Composable
private fun DrawingActions(hasStrokes: Boolean, onClear: () -> Unit, onSubmit: () -> Unit) {
    Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(10.dp), Alignment.CenterVertically) {
        // Clear button
        OutlinedButton(
            onClick  = onClear,
            enabled  = hasStrokes,
            modifier = Modifier.weight(1f),
            shape    = RoundedCornerShape(14.dp),
            colors   = ButtonDefaults.outlinedButtonColors(contentColor = WriteDark),
            border   = androidx.compose.foundation.BorderStroke(1.5.dp, if (hasStrokes) WriteBorder else Color.LightGray)
        ) {
            Text("🗑 মুছুন", fontWeight = FontWeight.SemiBold, fontSize = 13.sp)
        }

        // Submit button
        Box(
            Modifier
                .weight(2f)
                .clip(RoundedCornerShape(14.dp))
                .background(
                    if (hasStrokes) Brush.linearGradient(listOf(WriteDark, WriteAccent))
                    else Brush.linearGradient(listOf(Color.LightGray, Color.LightGray))
                )
                .clickable(enabled = hasStrokes, onClick = onSubmit)
                .padding(vertical = 12.dp),
            contentAlignment = Alignment.Center
        ) {
            Text("🤖 AI দিয়ে যাচাই করুন", color = Color.White,
                fontWeight = FontWeight.Bold, fontSize = 13.sp)
        }
    }
}

// ── Result view ───────────────────────────────────────────────────────────────

@Composable
private fun DrawingResultView(result: DrawingResult, onRetry: () -> Unit) {
    val isCorrect = result.isCorrect
    val badgeBg   = if (isCorrect) Color(0xFFDCFCE7) else Color(0xFFFFE4E6)
    val badgeFg   = if (isCorrect) WriteSuccess else WriteFail

    // ── Result badge ─────────────────────────────────────────────────────
    Box(
        Modifier
            .fillMaxWidth()
            .clip(RoundedCornerShape(14.dp))
            .background(badgeBg)
            .padding(vertical = 14.dp),
        contentAlignment = Alignment.Center
    ) {
        Column(horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(4.dp)) {
            Text(if (isCorrect) "✅ সঠিক!" else "❌ আরও চেষ্টা করুন",
                fontSize = 18.sp, fontWeight = FontWeight.ExtraBold, color = badgeFg)
        }
    }

    Spacer(Modifier.height(10.dp))

    // ── Score pill ────────────────────────────────────────────────────────
    Box(
        Modifier
            .clip(RoundedCornerShape(20.dp))
            .background(WriteAccent)
            .padding(horizontal = 28.dp, vertical = 8.dp)
    ) {
        Text("${result.score}% মিল", color = Color.White,
            fontWeight = FontWeight.ExtraBold, fontSize = 15.sp)
    }

    Spacer(Modifier.height(12.dp))

    // ── Feedback ──────────────────────────────────────────────────────────
    Card(
        Modifier.fillMaxWidth(),
        shape  = RoundedCornerShape(14.dp),
        colors = CardDefaults.cardColors(Color.White),
        elevation = CardDefaults.cardElevation(2.dp)
    ) {
        Column(Modifier.padding(14.dp), verticalArrangement = Arrangement.spacedBy(6.dp)) {
            Text("🗒 মতামত", fontSize = 11.sp, fontWeight = FontWeight.Bold, color = WriteDark)
            Text(result.feedbackBn, fontSize = 13.sp, color = Color(0xFF1E293B))
        }
    }

    // ── Hint ──────────────────────────────────────────────────────────────
    if (result.hintBn.isNotBlank()) {
        Spacer(Modifier.height(8.dp))
        Card(
            Modifier.fillMaxWidth(),
            shape  = RoundedCornerShape(14.dp),
            colors = CardDefaults.cardColors(Color(0xFFEFF6FF)),
            elevation = CardDefaults.cardElevation(0.dp)
        ) {
            Row(Modifier.padding(12.dp), horizontalArrangement = Arrangement.spacedBy(8.dp)) {
                Text("💡", fontSize = 16.sp)
                Text(result.hintBn, fontSize = 12.sp, color = WriteDark, lineHeight = 18.sp)
            }
        }
    }

    Spacer(Modifier.height(14.dp))

    // ── Retry ─────────────────────────────────────────────────────────────
    Box(
        Modifier
            .fillMaxWidth()
            .clip(RoundedCornerShape(14.dp))
            .background(Brush.linearGradient(listOf(WriteDark, WriteAccent)))
            .clickable(onClick = onRetry)
            .padding(vertical = 13.dp),
        contentAlignment = Alignment.Center
    ) {
        Text("✏ আবার আঁকুন", color = Color.White,
            fontWeight = FontWeight.Bold, fontSize = 14.sp)
    }
    Spacer(Modifier.height(4.dp))
}

// ── Bitmap helper ─────────────────────────────────────────────────────────────

/**
 * Render [strokes] onto a 512×512 white Bitmap (scaled from [srcW]×[srcH] canvas)
 * and return it as a Base64-encoded PNG string.
 */
private fun strokesToBitmap(strokes: List<List<Offset>>, srcW: Int, srcH: Int): String {
    val bmpSize = 512
    val scaleX  = if (srcW > 0) bmpSize.toFloat() / srcW else 1f
    val scaleY  = if (srcH > 0) bmpSize.toFloat() / srcH else 1f

    val bitmap  = Bitmap.createBitmap(bmpSize, bmpSize, Bitmap.Config.ARGB_8888)
    val canvas  = android.graphics.Canvas(bitmap)
    canvas.drawColor(android.graphics.Color.WHITE)

    val paint = android.graphics.Paint().apply {
        color       = android.graphics.Color.BLACK
        strokeWidth = 8f * ((scaleX + scaleY) / 2f)
        strokeCap   = android.graphics.Paint.Cap.ROUND
        strokeJoin  = android.graphics.Paint.Join.ROUND
        style       = android.graphics.Paint.Style.STROKE
        isAntiAlias = true
    }

    strokes.forEach { stroke ->
        if (stroke.isEmpty()) return@forEach
        val path = android.graphics.Path()
        stroke.forEachIndexed { i, pt ->
            val x = pt.x * scaleX
            val y = pt.y * scaleY
            if (i == 0) path.moveTo(x, y) else path.lineTo(x, y)
        }
        canvas.drawPath(path, paint)
    }

    val stream = ByteArrayOutputStream()
    bitmap.compress(Bitmap.CompressFormat.PNG, 100, stream)
    return Base64.encodeToString(stream.toByteArray(), Base64.NO_WRAP)
}

// ─────────────────────────────────────────────────────────────────────────────
//  রূপ  —  Forms panel
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun FormsPanel(
    letter:    ArabicLetter?,
    forms:     LetterForms?,
    isLoading: Boolean,
    error:     String?,
    onRetry:   () -> Unit
) {
    val isTablet = LocalConfiguration.current.screenWidthDp >= 600
    
    Card(
        modifier  = Modifier.fillMaxWidth().padding(horizontal = if (isTablet) 0.dp else 16.dp),
        shape     = RoundedCornerShape(20.dp),
        colors    = CardDefaults.cardColors(containerColor = FormsCardBg),
        elevation = CardDefaults.cardElevation(8.dp)
    ) {
        Column(modifier = Modifier.padding(24.dp)) {
            Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                Column {
                    Text("বর্ণের রূপভেদ", fontSize = 18.sp, fontWeight = FontWeight.ExtraBold, color = FormsDark)
                    letter?.let { Text("${it.letter}  ·  ${it.nameEnglish}", fontSize = 14.sp, color = FormsAccent.copy(.8f), fontWeight = FontWeight.SemiBold) }
                }
                forms?.let {
                    val (lbl, bg, fg) = if (it.isConnector) Triple("উভয় দিকে যুক্ত হয় ✓", FormsBadgeBg, FormsAccent)
                                        else Triple("পরের বর্ণে যুক্ত হয় না ✗", Color(0xFFFEF2F2), Color(0xFFDC2626))
                    Box(Modifier.background(bg, RoundedCornerShape(20.dp)).padding(horizontal = 12.dp, vertical = 6.dp)) {
                        Text(lbl, fontSize = 11.sp, fontWeight = FontWeight.Bold, color = fg)
                    }
                }
            }
            
            Spacer(Modifier.height(24.dp))
            
            when {
                isLoading -> Box(Modifier.fillMaxWidth().height(200.dp), Alignment.Center) {
                    CircularProgressIndicator(color = FormsAccent, strokeWidth = 3.dp)
                }
                error != null -> Column(Modifier.fillMaxWidth().padding(16.dp), Arrangement.spacedBy(12.dp), Alignment.CenterHorizontally) {
                    Text("⚠️ $error", fontSize = 14.sp, color = Color(0xFF92400E), textAlign = TextAlign.Center)
                    Box(Modifier.clip(RoundedCornerShape(12.dp)).background(FormsAccent).clickable { onRetry() }.padding(horizontal = 24.dp, vertical = 10.dp)) {
                        Text("আবার চেষ্টা করুন", fontSize = 14.sp, color = Color.White, fontWeight = FontWeight.Bold)
                    }
                }
                forms != null -> {
                    if (isTablet) {
                        Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(24.dp)) {
                            Column(Modifier.weight(1f)) {
                                Text("অবস্থান অনুযায়ী রূপ", fontSize = 14.sp, fontWeight = FontWeight.Bold, color = FormsDark)
                                Spacer(Modifier.height(12.dp))
                                FormsGrid(forms)
                            }
                            Column(Modifier.weight(1f)) {
                                Text("সংযোগ প্রবাহ", fontSize = 14.sp, fontWeight = FontWeight.Bold, color = FormsDark)
                                Spacer(Modifier.height(12.dp))
                                FormsConnectionDiagram(forms)
                                Spacer(Modifier.height(16.dp))
                                InfoChip(
                                    label = "বর্ণের ধরণ",
                                    value = if (forms.isConnector) "এটি একটি সংযোগকারী বর্ণ" else "এটি একটি অ-সংযোগকারী বর্ণ",
                                    modifier = Modifier.fillMaxWidth()
                                )
                            }
                        }
                    } else {
                        FormsGrid(forms)
                        Spacer(Modifier.height(14.dp))
                        FormsConnectionDiagram(forms)
                    }
                }
                else -> Box(Modifier.fillMaxWidth().height(100.dp), Alignment.Center) {
                    Text("একটি বর্ণ নির্বাচন করুন", color = SlateGrey, fontSize = 14.sp)
                }
            }
        }
    }
}

@Composable
private fun FormsGrid(forms: LetterForms) {
    val cells = listOf(
        Triple("বিচ্ছিন্ন", forms.isolatedForm, "isolated"),
        Triple("প্রাথমিক",  forms.initialForm,  "initial"),
        Triple("মধ্যবর্তী", forms.medialForm,   "medial"),
        Triple("চূড়ান্ত",  forms.finalForm,    "final")
    )
    Column(verticalArrangement = Arrangement.spacedBy(10.dp)) {
        cells.chunked(2).forEach { row ->
            Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(10.dp)) {
                row.forEach { (label, glyph, pos) -> FormCell(label, glyph, pos, Modifier.weight(1f)) }
                if (row.size == 1) Spacer(Modifier.weight(1f))
            }
        }
    }
}

@Composable
private fun FormCell(label: String, glyph: String, position: String, modifier: Modifier = Modifier) {
    val (bg, border, icon) = when (position) {
        "isolated" -> Triple(Color(0xFFF5F3FF), Color(0xFFDDD6FE), "○")
        "initial"  -> Triple(Color(0xFFEFFFF7), Color(0xFFBBF7D0), "→")
        "medial"   -> Triple(Color(0xFFFFF7ED), Color(0xFFFED7AA), "↔")
        else       -> Triple(Color(0xFFFFF1F2), Color(0xFFFFCDD2), "←")
    }
    val accent = when (position) { "isolated" -> FormsAccent; "initial" -> AppPrimary; "medial" -> Color(0xFFD97706); else -> Color(0xFFDC2626) }
    Column(
        modifier = modifier.background(bg, RoundedCornerShape(14.dp)).border(1.5.dp, border, RoundedCornerShape(14.dp)).padding(12.dp),
        horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(6.dp)
    ) {
        Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
            Text(label, fontSize = 10.sp, fontWeight = FontWeight.Bold, color = accent)
            Text(icon,  fontSize = 12.sp, color = accent)
        }
        Text(glyph, fontSize = 42.sp, fontWeight = FontWeight.Bold, color = accent, textAlign = TextAlign.Center, modifier = Modifier.fillMaxWidth())
    }
}

@Composable
private fun FormsConnectionDiagram(forms: LetterForms) {
    Box(Modifier.fillMaxWidth().background(FormsLight, RoundedCornerShape(12.dp)).border(1.dp, FormsBorder, RoundedCornerShape(12.dp)).padding(12.dp)) {
        Column(verticalArrangement = Arrangement.spacedBy(6.dp)) {
            Text("সংযোগ প্রবাহ", fontSize = 10.sp, fontWeight = FontWeight.Bold, color = FormsAccent)
            Row(Modifier.fillMaxWidth(), Arrangement.SpaceEvenly, Alignment.CenterVertically) {
                DiagramNode(forms.isolatedForm, "বিচ্ছিন্ন")
                Text("→", fontSize = 14.sp, color = FormsBorder, fontWeight = FontWeight.Bold)
                DiagramNode(forms.initialForm,  "প্রাথমিক")
                Text("→", fontSize = 14.sp, color = FormsBorder, fontWeight = FontWeight.Bold)
                DiagramNode(forms.medialForm,   "মধ্যবর্তী")
                Text("→", fontSize = 14.sp, color = FormsBorder, fontWeight = FontWeight.Bold)
                DiagramNode(forms.finalForm,    "চূড়ান্ত")
            }
        }
    }
}

@Composable
private fun DiagramNode(glyph: String, label: String) {
    Column(horizontalAlignment = Alignment.CenterHorizontally) {
        Text(glyph, fontSize = 20.sp, fontWeight = FontWeight.Bold, color = FormsDark)
        Text(label, fontSize = 8.sp, color = SlateGrey, fontWeight = FontWeight.SemiBold)
    }
}

// ─────────────────────────────────────────────────────────────────────────────
//  Shared composables
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun DecorativeCircles() {
    Canvas(Modifier.fillMaxSize()) {
        drawCircle(Color.White.copy(.06f), 110.dp.toPx(), Offset(size.width + 50.dp.toPx(), 60.dp.toPx()))
        drawCircle(Color.White.copy(.05f), 130.dp.toPx(), Offset(-30.dp.toPx(), 180.dp.toPx()))
        drawCircle(Color.White.copy(.04f),  90.dp.toPx(), Offset(size.width / 2,  300.dp.toPx()))
        drawCircle(Color.White.copy(.03f), 160.dp.toPx(), Offset(size.width - 20.dp.toPx(), size.height * .6f))
    }
}

@Composable
private fun ErrorState(message: String, onRetry: () -> Unit, modifier: Modifier = Modifier) {
    Column(modifier.padding(40.dp), Arrangement.spacedBy(16.dp), Alignment.CenterHorizontally) {
        Text("📡", fontSize = 56.sp)
        Text("সংযোগ হচ্ছে না", color = Color.White, fontSize = 20.sp, fontWeight = FontWeight.ExtraBold, textAlign = TextAlign.Center)
        Text(
            "সার্ভারের সাথে যোগাযোগ করা যাচ্ছে না।\nইন্টারনেট সংযোগ দেখে আবার চেষ্টা করুন।",
            color = Color.White.copy(.8f), fontSize = 13.sp, textAlign = TextAlign.Center, lineHeight = 20.sp
        )
        Button(
            onClick = onRetry,
            colors  = ButtonDefaults.buttonColors(Color.White),
            shape   = RoundedCornerShape(24.dp),
            modifier = Modifier.padding(top = 4.dp)
        ) {
            Text("🔄  পুনরায় চেষ্টা করুন", color = AppPrimary, fontWeight = FontWeight.Bold, fontSize = 14.sp)
        }
    }
}

@Composable
private fun GreetingRow(onBack: () -> Unit = {}) {
    Row(Modifier.fillMaxWidth().padding(horizontal = 16.dp), Arrangement.SpaceBetween, Alignment.CenterVertically) {
        Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(10.dp)) {
            Box(
                Modifier.size(36.dp)
                    .background(Color.White.copy(.22f), androidx.compose.foundation.shape.CircleShape)
                    .border(1.dp, Color.White.copy(.3f), androidx.compose.foundation.shape.CircleShape)
                    .clickable { onBack() },
                Alignment.Center
            ) {
                Text("◀", fontSize = 13.sp, color = Color.White, fontWeight = FontWeight.Bold)
            }
            Column {
                Text("বর্ণ শিক্ষা", fontSize = 18.sp, fontWeight = FontWeight.Bold, color = Color.White)
                Text("আরবি হরফ শিখুন", fontSize = 12.sp, color = Color.White.copy(.75f))
            }
        }
        Box(Modifier.size(42.dp).background(Brush.linearGradient(listOf(Color(0xFFFBBF24), Color(0xFFF59E0B))), CircleShape), Alignment.Center) {
            Text("🧑", fontSize = 22.sp)
        }
    }
}

@Composable
private fun StatsGrid(learned: Int, total: Int) {
    val level = ((learned.toFloat() / total.coerceAtLeast(1)) * 10).toInt().coerceAtLeast(1)
    val score = learned * 60
    Row(Modifier.fillMaxWidth().padding(horizontal = 16.dp), Arrangement.spacedBy(10.dp)) {
        StatCard(
            icon    = "📖",
            value   = "$learned",
            label   = "শেখা বর্ণ",
            sub     = "/ $total",
            accent  = Color(0xFF10B981),
            modifier = Modifier.weight(1f)
        )
        StatCard(
            icon    = "⭐",
            value   = "$score",
            label   = "মোট স্কোর",
            sub     = "pts",
            accent  = Color(0xFFF59E0B),
            modifier = Modifier.weight(1f)
        )
        StatCard(
            icon    = "🏆",
            value   = "$level",
            label   = "স্তর",
            sub     = "/ 10",
            accent  = Color(0xFF8B5CF6),
            modifier = Modifier.weight(1f)
        )
    }
}

@Composable
private fun StatCard(
    icon:     String,
    value:    String,
    label:    String,
    sub:      String,
    accent:   Color,
    modifier: Modifier = Modifier
) {
    Card(
        modifier  = modifier,
        shape     = RoundedCornerShape(16.dp),
        colors    = CardDefaults.cardColors(containerColor = Color.White),
        elevation = CardDefaults.cardElevation(4.dp)
    ) {
        Column(Modifier.padding(horizontal = 10.dp, vertical = 10.dp)) {
            // ── Left: name/label  |  Right: icon + number ──────────────
            Row(
                Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceBetween,
                verticalAlignment     = Alignment.CenterVertically
            ) {
                // Left — label on top, sub below
                Column(verticalArrangement = Arrangement.spacedBy(2.dp)) {
                    Text(
                        label,
                        fontSize   = 10.sp,
                        fontWeight = FontWeight.Bold,
                        color      = Color(0xFF334155)
                    )
                    Text(
                        sub,
                        fontSize  = 9.sp,
                        color     = SlateGrey,
                        fontWeight = FontWeight.Medium
                    )
                }
                // Right — icon bubble above, number below
                Column(
                    horizontalAlignment = Alignment.CenterHorizontally,
                    verticalArrangement = Arrangement.spacedBy(3.dp)
                ) {
                    Box(
                        Modifier
                            .size(28.dp)
                            .background(accent.copy(.13f), CircleShape),
                        Alignment.Center
                    ) {
                        Text(icon, fontSize = 14.sp)
                    }
                    Text(
                        value,
                        fontSize   = 18.sp,
                        fontWeight = FontWeight.ExtraBold,
                        color      = accent,
                        lineHeight = 20.sp
                    )
                }
            }
            Spacer(Modifier.height(8.dp))
            // Bottom accent bar
            Box(
                Modifier
                    .fillMaxWidth()
                    .height(3.dp)
                    .clip(RoundedCornerShape(2.dp))
                    .background(accent.copy(.35f))
            )
        }
    }
}

@Composable
private fun TabSwitcher(tabs: List<String>, activeTab: String, onTabSelect: (String) -> Unit) {
    Row(
        Modifier.fillMaxWidth().padding(horizontal = 16.dp, vertical = 10.dp)
            .background(Color.White.copy(.20f), RoundedCornerShape(14.dp)).padding(4.dp),
        Arrangement.spacedBy(4.dp)
    ) {
        val activeBrush   = Brush.linearGradient(listOf(SpeakDark, SpeakAccent))
        val inactiveBrush = Brush.linearGradient(listOf(SpeakDark.copy(.30f), SpeakAccent.copy(.30f)))
        tabs.forEach { tab ->
            val isActive = tab == activeTab
            val tabBrush = if (isActive) activeBrush else inactiveBrush
            Box(
                Modifier.weight(1f).clip(RoundedCornerShape(10.dp))
                    .background(tabBrush)
                    .clickable { onTabSelect(tab) }.padding(vertical = 8.dp),
                Alignment.Center
            ) {
                Column(horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(4.dp)) {
                    Text(tab, fontSize = 13.sp, fontWeight = FontWeight.Bold,
                        color = if (isActive) Color.White else Color.White.copy(.6f))
                    Box(
                        Modifier.size(if (isActive) 5.dp else 3.dp)
                            .background(
                                if (isActive) Color.White else Color.White.copy(.3f),
                                CircleShape
                            )
                    )
                }
            }
        }
    }
}

@Composable
private fun LessonCard(letter: ArabicLetter, onPlayAudio: () -> Unit) {
    Card(Modifier.fillMaxWidth().padding(horizontal = 16.dp), RoundedCornerShape(20.dp),
        CardDefaults.cardColors(Color.White), CardDefaults.cardElevation(8.dp)) {
        Column(Modifier.padding(16.dp)) {
            Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                Column(Modifier.weight(1f)) {
                    Text("বর্ণ পরিচয়", fontSize = 11.sp, color = SlateGrey, fontWeight = FontWeight.SemiBold)
                    Text(
                        "${letter.letter}  ${letter.nameArabic}  —  ${letter.nameEnglish}",
                        fontSize = 14.sp, fontWeight = FontWeight.ExtraBold, color = NavyText,
                        maxLines = 1, overflow = TextOverflow.Ellipsis
                    )
                }
                Spacer(Modifier.width(8.dp))
                Box(Modifier.clip(RoundedCornerShape(20.dp)).background(Brush.linearGradient(listOf(AppPrimary, AppSecondary))).clickable { onPlayAudio() }.padding(horizontal = 12.dp, vertical = 8.dp)) {
                    Text("🔊 শুনুন", fontSize = 12.sp, fontWeight = FontWeight.Bold, color = Color.White)
                }
            }
            Spacer(Modifier.height(14.dp))
            Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(12.dp), Alignment.CenterVertically) {
                // Letter showcase box instead of hardcoded image
                Box(
                    Modifier.width(110.dp).height(100.dp)
                        .background(Brush.linearGradient(listOf(AppPrimary, AppSecondary)), RoundedCornerShape(14.dp)),
                    Alignment.Center
                ) {
                    Column(horizontalAlignment = Alignment.CenterHorizontally) {
                        Text(letter.letter, fontSize = 44.sp, fontWeight = FontWeight.Bold, color = Color.White)
                        Text(letter.transliteration, fontSize = 10.sp, color = Color.White.copy(.8f), fontWeight = FontWeight.SemiBold)
                    }
                }
                Box(Modifier.weight(1f).height(100.dp).background(Brush.linearGradient(listOf(Color(0xFFF8FAFC), Color(0xFFF1F5F9))), RoundedCornerShape(14.dp)).border(1.5.dp, Color(0xFFE2E8F0), RoundedCornerShape(14.dp)).padding(12.dp)) {
                    Column(Modifier.fillMaxSize(), verticalArrangement = Arrangement.Center) {
                        Text(letter.exampleWordArabic, fontSize = 28.sp, fontWeight = FontWeight.Bold, color = AppPrimary, textAlign = TextAlign.End, modifier = Modifier.fillMaxWidth())
                        Spacer(Modifier.height(4.dp))
                        Text(letter.exampleWordBn,  fontSize = 11.sp, fontWeight = FontWeight.Bold, color = AppPrimary)
                        Text("অর্থ: ${letter.exampleWord}", fontSize = 10.sp, color = SlateGrey)
                    }
                }
            }
            Spacer(Modifier.height(12.dp))
            Row(Modifier.fillMaxWidth().background(SkyBlueAlert, RoundedCornerShape(12.dp)).border(1.dp, Color(0xFFBAE6FD), RoundedCornerShape(12.dp)).padding(10.dp), Arrangement.spacedBy(8.dp), Alignment.Top) {
                Text("ℹ️", fontSize = 16.sp)
                Text(letter.makhrajDescriptionBn, fontSize = 11.sp, fontWeight = FontWeight.SemiBold, color = SkyBlueDark, lineHeight = 16.sp)
            }
        }
    }
}

@Composable
private fun ProgressSection(current: Int, total: Int) {
    val progress = if (total > 0) current.toFloat() / total else 0f
    val pct      = (progress * 100).toInt()
    Card(Modifier.fillMaxWidth().padding(horizontal = 16.dp), RoundedCornerShape(20.dp), CardDefaults.cardColors(Color.White), CardDefaults.cardElevation(4.dp)) {
        Column(Modifier.padding(14.dp)) {
            Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                Text("📖 অগ্রগতি", fontSize = 13.sp, fontWeight = FontWeight.Bold, color = NavyText)
                Text("$pct%", fontSize = 13.sp, fontWeight = FontWeight.ExtraBold, color = AppPrimary)
            }
            Spacer(Modifier.height(10.dp))
            Canvas(Modifier.fillMaxWidth().height(20.dp)) {
                val trackH = 10.dp.toPx(); val trackY = size.height / 2
                val fillW  = (size.width * progress).coerceIn(0f, size.width); val thumbR = 8.dp.toPx()
                drawRoundRect(Color(0xFFE2E8F0), Offset(0f, trackY - trackH/2), Size(size.width, trackH), CornerRadius(trackH/2))
                if (fillW > 0f) drawRoundRect(Brush.horizontalGradient(listOf(AppPrimary, AppSecondary, AppAccent), 0f, size.width), Offset(0f, trackY - trackH/2), Size(fillW, trackH), CornerRadius(trackH/2))
                val tx = fillW.coerceIn(thumbR, size.width - thumbR)
                drawCircle(Color.White, thumbR, Offset(tx, trackY))
                drawCircle(AppSecondary, thumbR, Offset(tx, trackY), style = Stroke(3.dp.toPx()))
            }
            Spacer(Modifier.height(6.dp))
            Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween) {
                Text("শুরু",                                   fontSize = 10.sp, color = SlateGrey, fontWeight = FontWeight.SemiBold)
                Text("$current / $total বর্ণ সম্পন্ন",        fontSize = 10.sp, color = SlateGrey, fontWeight = FontWeight.SemiBold)
                Text("${total}টি বর্ণ",                       fontSize = 10.sp, color = SlateGrey, fontWeight = FontWeight.SemiBold)
            }
        }
    }
}

@Composable
internal fun LetterRailSection(
    letters:          List<ArabicLetter>,
    selectedLetter:   ArabicLetter?,
    selectedIndex:    Int,
    accentColor:      Color,
    onLetterSelected: (ArabicLetter) -> Unit,
    isLoadingMore:    Boolean = false,
) {
    val pageSize   = 3
    var page by remember { mutableStateOf(0) }

    // Auto-follow selected letter (driven by Next / Back buttons)
    LaunchedEffect(selectedIndex) {
        if (selectedIndex >= 0) page = selectedIndex / pageSize
    }

    val totalPages  = if (letters.isEmpty()) 1 else (letters.size + pageSize - 1) / pageSize
    val pageLetters = letters.drop(page * pageSize).take(pageSize)
    val from        = if (letters.isEmpty()) 0 else page * pageSize + 1
    val to          = minOf((page + 1) * pageSize, letters.size)

    Card(
        modifier  = Modifier.fillMaxWidth().padding(horizontal = 16.dp),
        shape     = RoundedCornerShape(20.dp),
        colors    = CardDefaults.cardColors(containerColor = Color.White.copy(.14f)),
        elevation = CardDefaults.cardElevation(0.dp)
    ) {
        Column(Modifier.padding(horizontal = 14.dp, vertical = 12.dp)) {

            // ── Header ──────────────────────────────────────────────────
            Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                Text(
                    "আরবি বর্ণমালা",
                    fontSize = 13.sp, fontWeight = FontWeight.ExtraBold, color = Color.White
                )
                Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(5.dp)) {
                    if (isLoadingMore) {
                        CircularProgressIndicator(color = Color.White.copy(.7f), strokeWidth = 1.5.dp, modifier = Modifier.size(11.dp))
                    }
                    if (letters.isNotEmpty()) {
                        Box(
                            Modifier
                                .background(Color.White.copy(.2f), RoundedCornerShape(20.dp))
                                .padding(horizontal = 8.dp, vertical = 3.dp)
                        ) {
                            Text(
                                "$from–$to / ${letters.size}",
                                fontSize = 10.sp, fontWeight = FontWeight.Bold, color = Color.White
                            )
                        }
                    }
                }
            }

            Spacer(Modifier.height(12.dp))

            // ── Paginator row: ◀  [cell] [cell] [cell]  ▶ ───────────────
            Row(
                Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.spacedBy(8.dp),
                verticalAlignment     = Alignment.CenterVertically
            ) {
                // ◀ Prev
                Box(
                    Modifier
                        .size(34.dp)
                        .clip(CircleShape)
                        .background(if (page > 0) Color.White.copy(.28f) else Color.White.copy(.07f))
                        .clickable(enabled = page > 0) { page-- },
                    Alignment.Center
                ) {
                    Text("◀", fontSize = 12.sp, fontWeight = FontWeight.Bold,
                        color = if (page > 0) Color.White else Color.White.copy(.2f))
                }

                // Letter cells — vertical layout: big letter on top, name below
                pageLetters.forEach { letter ->
                    val isSel = selectedLetter?.id == letter.id
                    Column(
                        Modifier
                            .weight(1f)
                            .height(78.dp)
                            .clip(RoundedCornerShape(14.dp))
                            .background(
                                if (isSel) accentColor.copy(.18f)
                                else Color.White.copy(.93f)
                            )
                            .border(2.dp,
                                if (isSel) accentColor else Color.Transparent,
                                RoundedCornerShape(14.dp)
                            )
                            .clickable { onLetterSelected(letter) },
                        verticalArrangement   = Arrangement.Center,
                        horizontalAlignment   = Alignment.CenterHorizontally
                    ) {
                        // Arabic letter — big, top
                        Text(
                            letter.letter,
                            fontSize   = 30.sp,
                            fontWeight = FontWeight.Bold,
                            color      = if (isSel) accentColor else Color(0xFF1E293B)
                        )
                        // Bangla name — small, below
                        Text(
                            letter.nameBangla.take(6),
                            fontSize   = 9.sp,
                            fontWeight = FontWeight.SemiBold,
                            color      = if (isSel) accentColor else SlateGrey,
                            maxLines   = 1,
                            overflow   = TextOverflow.Clip,
                            textAlign  = TextAlign.Center,
                            modifier   = Modifier.padding(horizontal = 4.dp)
                        )
                    }
                }

                // Empty slot placeholder (last page may have < 3 letters)
                repeat((pageSize - pageLetters.size).coerceAtLeast(0)) {
                    Spacer(Modifier.weight(1f))
                }

                // ▶ Next
                val canNext = page < totalPages - 1
                Box(
                    Modifier
                        .size(34.dp)
                        .clip(CircleShape)
                        .background(if (canNext) Color.White.copy(.28f) else Color.White.copy(.07f))
                        .clickable(enabled = canNext) { page++ },
                    Alignment.Center
                ) {
                    Text("▶", fontSize = 12.sp, fontWeight = FontWeight.Bold,
                        color = if (canNext) Color.White else Color.White.copy(.2f))
                }
            }

            // ── Page dots ────────────────────────────────────────────────
            if (totalPages > 1) {
                Spacer(Modifier.height(10.dp))
                Row(Modifier.fillMaxWidth(), Arrangement.Center, Alignment.CenterVertically) {
                    val show = totalPages.coerceAtMost(10)
                    repeat(show) { i ->
                        Box(
                            Modifier
                                .padding(horizontal = 3.dp)
                                .size(if (i == page) 8.dp else 5.dp)
                                .background(
                                    if (i == page) accentColor else Color.White.copy(.3f),
                                    CircleShape
                                )
                        )
                    }
                    if (totalPages > 10)
                        Text(" +${totalPages - 10}", fontSize = 9.sp, color = Color.White.copy(.45f))
                }
            }
        }
    }
}

@Composable
private fun BottomActions(
    accentBrush:    Brush,
    isFirst:        Boolean,
    isLast:         Boolean,
    onBack:         () -> Unit,
    onNext:         () -> Unit,
    outerPadding:   androidx.compose.foundation.layout.PaddingValues =
        androidx.compose.foundation.layout.PaddingValues(horizontal = 16.dp)
) {
    val haptic = LocalHapticFeedback.current
    Row(Modifier.fillMaxWidth().padding(outerPadding), Arrangement.spacedBy(10.dp), Alignment.CenterVertically) {
        // Back button — grey + disabled when on first letter
        Box(
            Modifier
                .size(52.dp)
                .background(
                    if (isFirst) Color.White.copy(.35f) else Color.White.copy(.9f),
                    CircleShape
                )
                .clickable(enabled = !isFirst) {
                    haptic.performHapticFeedback(HapticFeedbackType.LongPress)
                    onBack()
                },
            Alignment.Center
        ) {
            Text("◀", fontSize = 20.sp,
                color = if (isFirst) Color.White.copy(.4f) else AppPrimary,
                fontWeight = FontWeight.Bold)
        }
        // Next / Finish button
        Box(
            Modifier
                .weight(1f)
                .height(52.dp)
                .clip(RoundedCornerShape(26.dp))
                .background(if (isLast) Brush.linearGradient(listOf(Color(0xFF065F46), AppPrimary)) else accentBrush)
                .clickable {
                    haptic.performHapticFeedback(HapticFeedbackType.LongPress)
                    onNext()
                },
            Alignment.Center
        ) {
            Text(
                if (isLast) "✅  সব বর্ণ সম্পন্ন!" else "পরবর্তী বর্ণ শিখুন  ▶",
                fontSize = 13.sp, fontWeight = FontWeight.Bold, color = Color.White
            )
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
//  Phone single-column layout  (screen width < 600dp)
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun PhoneLayout(
    vm:             ArabicLettersViewModel,
    letters:        List<ArabicLetter>,
    selectedLetter: ArabicLetter?,
    selectedIndex:  Int,
    isFirst:        Boolean,
    isLast:         Boolean,
    activeTab:      String,
    onTabSelect:    (String) -> Unit,
    letterForms:    LetterForms?,
    formsLoading:   Boolean,
    formsError:     String?,
    speakState:     SpeakState,
    drawingState:   DrawingState,
    totalCount:     Int = 0,
    isLoadingMore:  Boolean = false,
    onBack:         () -> Unit = {},
) {
    // Use server total when available; fall back to local list size while first page is loading
    val displayTotal = totalCount.coerceAtLeast(letters.size)
    Column(
        modifier = Modifier
            .fillMaxSize()
            .statusBarsPadding()
            .navigationBarsPadding()
            .verticalScroll(rememberScrollState())
    ) {
        Spacer(Modifier.height(8.dp))
        GreetingRow(onBack = onBack)
        Spacer(Modifier.height(8.dp))
        StatsGrid(learned = (selectedIndex + 1).coerceAtLeast(1), total = displayTotal)
        Spacer(Modifier.height(4.dp))
        TabSwitcher(
            tabs        = listOf("পাঠ", "রূপ", "বলুন", "লিখুন"),
            activeTab   = activeTab,
            onTabSelect = onTabSelect
        )
        AnimatedContent(
            targetState = activeTab to selectedLetter,
            transitionSpec = { fadeIn(tween(180)) togetherWith fadeOut(tween(120)) },
            label = "tab-content"
        ) { (tab, letter) ->
            when (tab) {
                "রূপ"   -> FormsPanel(letter, letterForms, formsLoading, formsError,
                              onRetry = { letter?.let { vm.fetchLetterForms(it.id) } })
                "বলুন"  -> SpeakPanel(
                               letter     = letter,
                               speakState = speakState,
                               onStart    = { vm.startRecording() },
                               onStop     = { letter?.let { vm.stopAndAssess(it.id) } },
                               onReset    = { vm.resetSpeak() }
                            )
                "লিখুন" -> LikhunPanel(
                               letter       = letter,
                               drawingState = drawingState,
                               onSubmit     = { b64 -> letter?.let { vm.checkDrawing(it.id, b64) } },
                               onReset      = { vm.resetDrawing() }
                            )
                else    -> letter?.let { LessonCard(it) { vm.playAudio(it.audioUrl) } }
            }
        }
        Spacer(Modifier.height(4.dp))
        ProgressSection(current = selectedIndex + 1, total = displayTotal)
        Spacer(Modifier.height(8.dp))
        // ── Letter paginator — card design, below progress ───────────────
        LetterRailSection(
            letters          = letters,
            selectedLetter   = selectedLetter,
            selectedIndex    = selectedIndex,
            accentColor      = when (activeTab) {
                "রূপ" -> FormsAccent; "বলুন" -> SpeakAccent; "লিখুন" -> WriteAccent; else -> AppPrimary
            },
            onLetterSelected = { vm.selectLetter(it) },
            isLoadingMore    = isLoadingMore,
        )
        Spacer(Modifier.height(8.dp))
        BottomActions(
            accentBrush = when (activeTab) {
                "রূপ"   -> Brush.linearGradient(listOf(FormsDark,  FormsAccent))
                "বলুন"  -> Brush.linearGradient(listOf(SpeakDark,  SpeakAccent))
                "লিখুন" -> Brush.linearGradient(listOf(WriteDark,  WriteAccent))
                else    -> Brush.linearGradient(listOf(AppDark, AppPrimary, AppSecondary))
            },
            isFirst = isFirst,
            isLast  = isLast,
            onBack  = { if (!isFirst) vm.selectLetter(letters[selectedIndex - 1]) },
            onNext  = { if (!isLast)  vm.selectLetter(letters[selectedIndex + 1]) }
        )
        Spacer(Modifier.height(16.dp))
    }
}

// ─────────────────────────────────────────────────────────────────────────────
//  Tablet two-pane layout  (screen width ≥ 600dp)
//
//  LEFT  (300dp fixed) : greeting · stats · progress · letter grid · nav
//  RIGHT (remaining)   : tab switcher · tab content (scrollable)
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun TabletLayout(
    vm:             ArabicLettersViewModel,
    letters:        List<ArabicLetter>,
    selectedLetter: ArabicLetter?,
    selectedIndex:  Int,
    isFirst:        Boolean,
    isLast:         Boolean,
    activeTab:      String,
    onTabSelect:    (String) -> Unit,
    letterForms:    LetterForms?,
    formsLoading:   Boolean,
    formsError:     String?,
    speakState:     SpeakState,
    drawingState:   DrawingState,
    totalCount:     Int = 0,
    isLoadingMore:  Boolean = false,
    onBack:         () -> Unit = {},
) {
    val displayTotal = totalCount.coerceAtLeast(letters.size)
    val accentColor = when (activeTab) {
        "রূপ" -> FormsAccent; "বলুন" -> SpeakAccent; "লিখুন" -> WriteAccent; else -> AppPrimary
    }
    val accentBrush = when (activeTab) {
        "রূপ"   -> Brush.linearGradient(listOf(FormsDark,  FormsAccent))
        "বলুন"  -> Brush.linearGradient(listOf(SpeakDark,  SpeakAccent))
        "লিখুন" -> Brush.linearGradient(listOf(WriteDark,  WriteAccent))
        else    -> Brush.linearGradient(listOf(AppDark, AppPrimary, AppSecondary))
    }

    Row(
        Modifier
            .fillMaxSize()
            .statusBarsPadding()
            .navigationBarsPadding()
    ) {
        // ── LEFT SIDEBAR (Master) ──────────────────────────────────────────
        Surface(
            modifier = Modifier
                .width(320.dp)
                .fillMaxHeight(),
            color = Color.Black.copy(0.05f), // Subtle contrast from main bg
            shape = RoundedCornerShape(topEnd = 24.dp, bottomEnd = 24.dp)
        ) {
            Column(
                Modifier
                    .fillMaxSize()
                    .padding(16.dp)
            ) {
                GreetingRow(onBack = onBack)
                Spacer(Modifier.height(16.dp))

                // Sidebar Dashboard
                Column(
                    Modifier
                        .fillMaxWidth()
                        .background(Color.White.copy(0.1f), RoundedCornerShape(16.dp))
                        .padding(12.dp)
                ) {
                    StatsGrid(learned = (selectedIndex + 1).coerceAtLeast(1), total = displayTotal)
                    Spacer(Modifier.height(12.dp))
                    ProgressSection(current = selectedIndex + 1, total = displayTotal)
                }
                
                Spacer(Modifier.height(20.dp))
                Text(
                    "আরবি বর্ণমালা",
                    fontSize = 14.sp, fontWeight = FontWeight.Bold, color = Color.White,
                    modifier = Modifier.padding(start = 4.dp, bottom = 12.dp)
                )
                
                // Scrollable Letter List
                TabletLetterGrid(
                    letters        = letters,
                    selectedLetter = selectedLetter,
                    selectedIndex  = selectedIndex,
                    accentColor    = accentColor,
                    onLetterSelect = { vm.selectLetter(it) },
                    modifier       = Modifier.weight(1f),
                    isLoadingMore  = isLoadingMore,
                )
                
                Spacer(Modifier.height(16.dp))
                
                // Pinned Navigation at Sidebar Bottom
                BottomActions(
                    accentBrush  = accentBrush,
                    isFirst      = isFirst,
                    isLast       = isLast,
                    onBack       = { if (!isFirst) vm.selectLetter(letters[selectedIndex - 1]) },
                    onNext       = { if (!isLast)  vm.selectLetter(letters[selectedIndex + 1]) },
                    outerPadding = PaddingValues(0.dp)
                )
            }
        }

        // ── RIGHT CONTENT (Detail) ─────────────────────────────────────────
        Column(
            Modifier
                .weight(1f)
                .fillMaxHeight()
                .padding(horizontal = 24.dp, vertical = 16.dp)
        ) {
            // Content Header - Selected Letter Summary
            selectedLetter?.let { letter ->
                TabletDetailHeader(
                    letter      = letter,
                    accentColor = accentColor,
                    onPlay      = { vm.playAudio(letter.audioUrl) }
                )
                Spacer(Modifier.height(20.dp))
            }

            // Tab Switcher - More prominent for tablet
            TabSwitcher(
                tabs        = listOf("পাঠ", "রূপ", "বলুন", "লিখুন"),
                activeTab   = activeTab,
                onTabSelect = onTabSelect
            )
            
            Spacer(Modifier.height(24.dp))
            
            // Content Area
            Box(Modifier.weight(1f).fillMaxWidth()) {
                AnimatedContent(
                    targetState = activeTab to selectedLetter,
                    transitionSpec = { fadeIn(tween(200)) togetherWith fadeOut(tween(150)) },
                    label = "tab-content-tablet"
                ) { (tab, letter) ->
                    Column(
                        Modifier
                            .fillMaxSize()
                            .verticalScroll(rememberScrollState())
                    ) {
                        when (tab) {
                            "রূপ"   -> FormsPanel(letter, letterForms, formsLoading, formsError,
                                          onRetry = { letter?.let { vm.fetchLetterForms(it.id) } })
                            "বলুন"  -> SpeakPanel(
                                           letter     = letter,
                                           speakState = speakState,
                                           onStart    = { vm.startRecording() },
                                           onStop     = { letter?.let { vm.stopAndAssess(it.id) } },
                                           onReset    = { vm.resetSpeak() }
                                        )
                            "লিখুন" -> LikhunPanel(
                                           letter       = letter,
                                           drawingState = drawingState,
                                           onSubmit     = { b64 -> letter?.let { vm.checkDrawing(it.id, b64) } },
                                           onReset      = { vm.resetDrawing() }
                                        )
                            else    -> letter?.let { TabletLessonCard(it) { vm.playAudio(it.audioUrl) } }
                        }
                        // Add some padding at the bottom for better scrolling feel
                        Spacer(Modifier.height(40.dp))
                    }
                }
            }
        }
    }
}

@Composable
private fun TabletDetailHeader(
    letter: ArabicLetter,
    accentColor: Color,
    onPlay: () -> Unit
) {
    Row(
        Modifier
            .fillMaxWidth()
            .background(Color.White.copy(0.12f), RoundedCornerShape(20.dp))
            .border(1.dp, Color.White.copy(0.2f), RoundedCornerShape(20.dp))
            .padding(16.dp),
        verticalAlignment = Alignment.CenterVertically
    ) {
        // Current Letter Badge
        Box(
            Modifier
                .size(64.dp)
                .background(Color.White, CircleShape),
            Alignment.Center
        ) {
            Text(letter.letter, fontSize = 32.sp, fontWeight = FontWeight.Bold, color = AppPrimary)
        }
        
        Spacer(Modifier.width(20.dp))
        
        Column(Modifier.weight(1f)) {
            Text(
                text = "${letter.nameArabic} (${letter.nameEnglish})",
                fontSize = 20.sp,
                fontWeight = FontWeight.ExtraBold,
                color = Color.White
            )
            Text(
                text = "উচ্চারণ: ${letter.transliteration}",
                fontSize = 14.sp,
                color = Color.White.copy(0.8f)
            )
        }
        
        // Quick Play Button
        IconButton(
            onClick = onPlay,
            modifier = Modifier
                .size(56.dp)
                .background(accentColor, CircleShape)
        ) {
            Text("🔊", fontSize = 24.sp)
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
//  Tablet letter grid  (3 columns, larger cards for tablet)
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun TabletLetterGrid(
    letters:        List<ArabicLetter>,
    selectedLetter: ArabicLetter?,
    selectedIndex:  Int,
    accentColor:    Color,
    onLetterSelect: (ArabicLetter) -> Unit,
    modifier:       Modifier = Modifier,
    isLoadingMore:  Boolean  = false,
) {
    val gridState = rememberLazyGridState()
    LaunchedEffect(selectedIndex) {
        if (selectedIndex >= 0 && letters.isNotEmpty())
            gridState.animateScrollToItem(selectedIndex)
    }
    LazyVerticalGrid(
        columns               = GridCells.Fixed(3), // 3 columns look cleaner in sidebar
        state                 = gridState,
        modifier              = modifier,
        contentPadding        = PaddingValues(vertical = 4.dp),
        horizontalArrangement = Arrangement.spacedBy(10.dp),
        verticalArrangement   = Arrangement.spacedBy(10.dp)
    ) {
        itemsIndexed(letters) { _, letter ->
            val isSel = selectedLetter?.id == letter.id
            Column(
                Modifier
                    .aspectRatio(0.9f)
                    .clip(RoundedCornerShape(14.dp))
                    .background(if (isSel) Color.White else Color.White.copy(0.15f))
                    .border(2.dp, if (isSel) Color.White else Color.Transparent, RoundedCornerShape(14.dp))
                    .clickable { onLetterSelect(letter) },
                Arrangement.Center, Alignment.CenterHorizontally
            ) {
                Text(
                    letter.letter, fontSize = 28.sp, fontWeight = FontWeight.Bold,
                    color = if (isSel) AppPrimary else Color.White
                )
                Text(
                    letter.nameBangla, fontSize = 10.sp,
                    color = if (isSel) AppPrimary.copy(0.7f) else Color.White.copy(0.7f),
                    fontWeight = FontWeight.SemiBold, textAlign = TextAlign.Center,
                    maxLines = 1, overflow = TextOverflow.Ellipsis
                )
            }
        }
        if (isLoadingMore) {
            item(span = { GridItemSpan(maxLineSpan) }) {
                Box(Modifier.fillMaxWidth().padding(8.dp), Alignment.Center) {
                    CircularProgressIndicator(color = Color.White, strokeWidth = 2.dp, modifier = Modifier.size(24.dp))
                }
            }
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
//  Tablet lesson card  (Grid layout — more expansive use of space)
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun TabletLessonCard(letter: ArabicLetter, onPlayAudio: () -> Unit) {
    Column(verticalArrangement = Arrangement.spacedBy(20.dp)) {
        // Row 1: Letter Detail and Example Word
        Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(20.dp)) {
            // Showcase Card
            Card(
                Modifier.weight(0.4f).aspectRatio(1f),
                RoundedCornerShape(24.dp),
                CardDefaults.cardColors(Color.White),
                CardDefaults.cardElevation(4.dp)
            ) {
                Box(Modifier.fillMaxSize(), Alignment.Center) {
                    Column(horizontalAlignment = Alignment.CenterHorizontally) {
                        Text(letter.letter, fontSize = 100.sp, fontWeight = FontWeight.Bold, color = AppPrimary)
                        Text(letter.nameArabic, fontSize = 24.sp, color = AppSecondary, fontWeight = FontWeight.SemiBold)
                    }
                }
            }
            
            // Info and Example Word Card
            Card(
                Modifier.weight(0.6f),
                RoundedCornerShape(24.dp),
                CardDefaults.cardColors(Color.White),
                CardDefaults.cardElevation(4.dp)
            ) {
                Column(Modifier.padding(24.dp), verticalArrangement = Arrangement.spacedBy(16.dp)) {
                    Text("উদাহরণ (Example)", fontSize = 14.sp, color = SlateGrey, fontWeight = FontWeight.Bold)
                    
                    Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                        Column(Modifier.weight(1f)) {
                            Text(letter.exampleWordArabic, fontSize = 48.sp, fontWeight = FontWeight.Bold, color = AppPrimary)
                            Text(letter.exampleWordBn, fontSize = 20.sp, fontWeight = FontWeight.ExtraBold, color = AppSecondary)
                            Text("অর্থ: ${letter.exampleWord}", fontSize = 14.sp, color = SlateGrey)
                        }
                    }
                    
                    HorizontalDivider(color = Color.Black.copy(0.05f))
                    
                    Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(12.dp)) {
                        InfoChip(label = "বাংলা নাম", value = letter.nameBangla, Modifier.weight(1f))
                        InfoChip(label = "মাখরাজ স্থান", value = letter.makhrajType, Modifier.weight(1f))
                    }
                }
            }
        }
        
        // Row 2: Detailed Makhraj Description
        Card(
            Modifier.fillMaxWidth(),
            RoundedCornerShape(24.dp),
            CardDefaults.cardColors(Color.White),
            CardDefaults.cardElevation(4.dp)
        ) {
            Row(
                Modifier.padding(24.dp),
                Arrangement.spacedBy(16.dp), Alignment.Top
            ) {
                Box(
                    Modifier
                        .size(48.dp)
                        .background(SkyBlueAlert, CircleShape),
                    Alignment.Center
                ) {
                    Text("💡", fontSize = 20.sp)
                }
                Column {
                    Text("মাখরাজ বর্ণনা", fontSize = 16.sp, fontWeight = FontWeight.ExtraBold, color = NavyText)
                    Spacer(Modifier.height(8.dp))
                    Text(
                        letter.makhrajDescriptionBn,
                        fontSize = 15.sp,
                        color = SlateGrey,
                        lineHeight = 24.sp
                    )
                }
            }
        }
    }
}

@Composable
private fun InfoChip(label: String, value: String, modifier: Modifier = Modifier) {
    Column(
        modifier
            .background(Color(0xFFF8FAFC), RoundedCornerShape(10.dp))
            .border(1.dp, Color(0xFFE2E8F0), RoundedCornerShape(10.dp))
            .padding(horizontal = 10.dp, vertical = 8.dp),
        verticalArrangement = Arrangement.spacedBy(2.dp)
    ) {
        Text(label, fontSize = 9.sp, color = SlateGrey, fontWeight = FontWeight.SemiBold)
        Text(value, fontSize = 13.sp, color = NavyText, fontWeight = FontWeight.Bold, maxLines = 1, overflow = TextOverflow.Ellipsis)
    }
}

