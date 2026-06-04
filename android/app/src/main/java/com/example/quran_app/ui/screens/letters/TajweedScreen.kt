package com.example.quran_app.ui.screens.letters

import android.Manifest
import android.content.pm.PackageManager
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.animation.AnimatedContent
import androidx.compose.animation.core.*
import androidx.compose.animation.fadeIn
import androidx.compose.animation.fadeOut
import androidx.compose.animation.togetherWith
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
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
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.hapticfeedback.HapticFeedbackType
import androidx.compose.ui.platform.LocalConfiguration
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.platform.LocalHapticFeedback
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.core.content.ContextCompat
import com.example.quran_app.domain.model.ArabicLetter
import com.example.quran_app.domain.model.PronunciationResult
import com.example.quran_app.domain.model.SpeakState
import com.example.quran_app.ui.theme.*
import com.example.quran_app.ui.viewmodel.ArabicLettersViewModel

enum class TajweedMode { HARAKAT_TANWEEN, SUKOON_SHADDAH, WORD_BUILDING }

// ── Medium green theme ────────────────────────────────────────────────────────
private val TanweenAccent = Color(0xFF16A34A)   // green-600, readable on white
private val TanweenDark   = Color(0xFF14532D)   // green-900, dark heading text

private val modeAccent = mapOf(
    TajweedMode.HARAKAT_TANWEEN to TanweenAccent,
    TajweedMode.SUKOON_SHADDAH  to TanweenAccent,
    TajweedMode.WORD_BUILDING   to TanweenAccent,
)
private val modeDark = mapOf(
    TajweedMode.HARAKAT_TANWEEN to TanweenDark,
    TajweedMode.SUKOON_SHADDAH  to TanweenDark,
    TajweedMode.WORD_BUILDING   to TanweenDark,
)

// ── Card backgrounds ───────────────────────────────────────────────────────────
private val HarakatBg    = Color(0xFFFFFBEB)
private val SukoonBg     = Color(0xFFEEF2FF)
private val SukoonBorder = Color(0xFFC7D2FE)
private val WordBg       = Color(0xFFECFDF5)
private val WordBorder   = Color(0xFFA7F3D0)

// ── Speak colours ──────────────────────────────────────────────────────────────
private val SpeakAccent    = Color(0xFF16A34A)
private val SpeakRecording = Color(0xFFEF4444)
private val SpeakSuccess   = Color(0xFF16A34A)
private val SpeakFail      = Color(0xFFDC2626)

// ─────────────────────────────────────────────────────────────────────────────
//  Root screen
// ─────────────────────────────────────────────────────────────────────────────

@Composable
fun TajweedScreen(
    viewModel:   ArabicLettersViewModel,
    onBack:      () -> Unit,
    onShowTable: () -> Unit = {},
) {
    val letters        by viewModel.letters.collectAsState()
    val selectedLetter by viewModel.selectedLetter.collectAsState()
    val totalCount     by viewModel.totalCount.collectAsState()
    val isLoadingMore  by viewModel.isLoadingMore.collectAsState()
    val isLoading      by viewModel.isLoading.collectAsState()
    val error          by viewModel.error.collectAsState()
    val speakState     by viewModel.speakState.collectAsState()

    var activeMode by remember { mutableStateOf(TajweedMode.HARAKAT_TANWEEN) }

    val context = LocalContext.current
    val permLauncher = rememberLauncherForActivityResult(
        ActivityResultContracts.RequestPermission()
    ) { granted -> if (granted) viewModel.startRecording() }

    LaunchedEffect(activeMode) { viewModel.resetSpeak() }

    // Auto-save to DB whenever an assessment result arrives
    LaunchedEffect(speakState) {
        val letter = selectedLetter
        if (speakState is SpeakState.Result && letter != null) {
            val result = (speakState as SpeakState.Result).data
            viewModel.recordTajweedProgress(
                letterId = letter.id,
                mode     = activeMode.name.lowercase(),
                result   = result
            )
        }
    }

    val selectedIndex = if (selectedLetter != null) letters.indexOf(selectedLetter) else 0
    val displayTotal  = totalCount.coerceAtLeast(letters.size)
    val isFirst       = selectedIndex <= 0
    val isLast        = letters.isNotEmpty()
                        && selectedIndex >= letters.size - 1
                        && totalCount > 0
                        && letters.size >= totalCount

    val accent = modeAccent[activeMode] ?: AppPrimary
    val dark   = modeDark[activeMode]   ?: AppDark

    val onPlayAudio: () -> Unit = { selectedLetter?.let { viewModel.playAudio(it.audioUrl) } }
    val onStart:     () -> Unit = {
        val ok = ContextCompat.checkSelfPermission(context, Manifest.permission.RECORD_AUDIO) ==
                PackageManager.PERMISSION_GRANTED
        if (ok) viewModel.startRecording() else permLauncher.launch(Manifest.permission.RECORD_AUDIO)
    }
    val onStop:  () -> Unit = { selectedLetter?.let { viewModel.stopAndAssess(it.id) } }
    val onReset: () -> Unit = { viewModel.resetSpeak() }

    Box(
        Modifier
            .fillMaxSize()
            .background(Brush.verticalGradient(listOf(AppPrimary, AppSecondary, AppAccent, AppBgLight)))
    ) {
        when {
            isLoading && letters.isEmpty() ->
                CircularProgressIndicator(
                    Modifier.align(Alignment.Center), color = Color.White, strokeWidth = 3.dp
                )

            error != null && letters.isEmpty() ->
                Column(
                    Modifier.align(Alignment.Center).padding(40.dp),
                    verticalArrangement    = Arrangement.spacedBy(16.dp),
                    horizontalAlignment    = Alignment.CenterHorizontally
                ) {
                    Text("📡", fontSize = 48.sp)
                    Text("সংযোগ হচ্ছে না", color = Color.White, fontSize = 18.sp, fontWeight = FontWeight.Bold)
                    Button(onClick = { viewModel.fetchLetters() }, colors = ButtonDefaults.buttonColors(Color.White)) {
                        Text("পুনরায় চেষ্টা", color = AppPrimary, fontWeight = FontWeight.Bold)
                    }
                }

            else -> Column(
                Modifier
                    .fillMaxSize()
                    .statusBarsPadding()
                    .navigationBarsPadding()
                    .verticalScroll(rememberScrollState())
            ) {
                Spacer(Modifier.height(8.dp))

                // ── Header ────────────────────────────────────────────────────
                Row(
                    Modifier.fillMaxWidth().padding(horizontal = 16.dp),
                    Arrangement.SpaceBetween, Alignment.CenterVertically
                ) {
                    Row(
                        verticalAlignment     = Alignment.CenterVertically,
                        horizontalArrangement = Arrangement.spacedBy(10.dp)
                    ) {
                        Box(
                            Modifier
                                .size(36.dp)
                                .background(Color.White.copy(.22f), CircleShape)
                                .border(1.dp, Color.White.copy(.3f), CircleShape)
                                .clickable { onBack() },
                            Alignment.Center
                        ) {
                            Text("◀", fontSize = 13.sp, color = Color.White, fontWeight = FontWeight.Bold)
                        }
                        Column {
                            Text("তাজওয়িদ অনুশীলন", fontSize = 18.sp, fontWeight = FontWeight.Bold, color = Color.White)
                            Text("হরকত ও স্বরচিহ্ন শিখুন",  fontSize = 12.sp,                       color = Color.White.copy(.75f))
                        }
                    }
                    Row(horizontalArrangement = Arrangement.spacedBy(8.dp), verticalAlignment = Alignment.CenterVertically) {
                        Box(
                            Modifier
                                .background(Color.White.copy(.18f), RoundedCornerShape(20.dp))
                                .padding(horizontal = 10.dp, vertical = 5.dp)
                        ) {
                            Text(
                                "${selectedIndex + 1} / $displayTotal",
                                fontSize = 11.sp, color = Color.White, fontWeight = FontWeight.Bold
                            )
                        }
                        Box(
                            Modifier
                                .background(Color.White.copy(.22f), RoundedCornerShape(20.dp))
                                .border(1.dp, Color.White.copy(.3f), RoundedCornerShape(20.dp))
                                .clickable { onShowTable() }
                                .padding(horizontal = 10.dp, vertical = 5.dp),
                            Alignment.Center
                        ) {
                            Text("📊", fontSize = 14.sp)
                        }
                    }
                }

                // ── Tab switcher ──────────────────────────────────────────────
                TajweedTabSwitcher(activeMode) { activeMode = it }

                // ── Letter rail ───────────────────────────────────────────────
                LetterRailSection(
                    letters          = letters,
                    selectedLetter   = selectedLetter,
                    selectedIndex    = selectedIndex,
                    accentColor      = accent,
                    onLetterSelected = { viewModel.selectLetter(it) },
                    isLoadingMore    = isLoadingMore,
                )

                Spacer(Modifier.height(12.dp))

                // ── Mode card (animated) ──────────────────────────────────────
                AnimatedContent(
                    targetState    = activeMode to selectedLetter,
                    transitionSpec = { fadeIn(tween(160)) togetherWith fadeOut(tween(120)) },
                    label          = "tajweed-card"
                ) { (mode, letter) ->
                    letter?.let {
                        when (mode) {
                            TajweedMode.HARAKAT_TANWEEN -> HarakatTanweenCard(it, speakState, onPlayAudio, onStart, onStop, onReset)
                            TajweedMode.SUKOON_SHADDAH  -> SukoonShaddahCard(it, speakState, onPlayAudio, onStart, onStop, onReset)
                            TajweedMode.WORD_BUILDING   -> WordBuildingCard(it, speakState, onPlayAudio, onStart, onStop, onReset)
                        }
                    }
                }

                Spacer(Modifier.height(12.dp))

                // ── Prev / Next ───────────────────────────────────────────────
                val haptic = LocalHapticFeedback.current
                Row(
                    Modifier.fillMaxWidth().padding(horizontal = 16.dp),
                    Arrangement.spacedBy(10.dp), Alignment.CenterVertically
                ) {
                    Box(
                        Modifier
                            .size(52.dp)
                            .background(if (isFirst) Color.White.copy(.35f) else Color.White.copy(.9f), CircleShape)
                            .clickable(enabled = !isFirst) {
                                haptic.performHapticFeedback(HapticFeedbackType.LongPress)
                                viewModel.selectLetter(letters[selectedIndex - 1])
                            },
                        Alignment.Center
                    ) {
                        Text("◀", fontSize = 20.sp,
                            color      = if (isFirst) Color.White.copy(.4f) else AppPrimary,
                            fontWeight = FontWeight.Bold)
                    }
                    Box(
                        Modifier
                            .weight(1f).height(52.dp)
                            .clip(RoundedCornerShape(26.dp))
                            .background(
                                if (isLast) Brush.linearGradient(listOf(Color(0xFF065F46), AppPrimary))
                                else Brush.linearGradient(listOf(dark, accent))
                            )
                            .clickable {
                                haptic.performHapticFeedback(HapticFeedbackType.LongPress)
                                if (!isLast) viewModel.selectLetter(letters[selectedIndex + 1])
                            },
                        Alignment.Center
                    ) {
                        Text(
                            if (isLast) "✅  সব বর্ণ সম্পন্ন!" else "পরবর্তী বর্ণ  ▶",
                            fontSize = 13.sp, fontWeight = FontWeight.Bold, color = Color.White
                        )
                    }
                }

                Spacer(Modifier.height(16.dp))
            }
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
//  Tab switcher
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun TajweedTabSwitcher(activeMode: TajweedMode, onSelect: (TajweedMode) -> Unit) {
    val tabs = listOf(
        TajweedMode.HARAKAT_TANWEEN to "হারাকত",
        TajweedMode.SUKOON_SHADDAH  to "সুকূন",
        TajweedMode.WORD_BUILDING   to "শব্দ গঠন",
    )
    Row(
        Modifier
            .fillMaxWidth()
            .padding(horizontal = 16.dp, vertical = 10.dp)
            .background(Color.White.copy(.20f), RoundedCornerShape(14.dp))
            .padding(4.dp),
        Arrangement.spacedBy(4.dp)
    ) {
        tabs.forEach { (mode, label) ->
            val isActive = mode == activeMode
            Box(
                Modifier
                    .weight(1f)
                    .clip(RoundedCornerShape(10.dp))
                    .background(
                        if (isActive) Brush.linearGradient(listOf(TanweenDark, TanweenAccent))
                        else Brush.linearGradient(listOf(Color.White.copy(.12f), Color.White.copy(.12f)))
                    )
                    .clickable { onSelect(mode) }
                    .padding(vertical = 9.dp),
                Alignment.Center
            ) {
                Column(horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(4.dp)) {
                    Text(
                        label,
                        fontSize   = 12.sp,
                        fontWeight = FontWeight.Bold,
                        color      = if (isActive) Color.White else Color.White.copy(.6f)
                    )
                    Box(
                        Modifier
                            .size(if (isActive) 5.dp else 3.dp)
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

// ─────────────────────────────────────────────────────────────────────────────
//  Shared speak section — embedded at the bottom of every card
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun TajweedSpeakSection(
    speakState:  SpeakState,
    onPlayAudio: () -> Unit,
    onStart:     () -> Unit,
    onStop:      () -> Unit,
    onReset:     () -> Unit,
    targetGlyph: String = "",
    targetSound: String = "",
) {
    Spacer(Modifier.height(16.dp))
    HorizontalDivider(color = Color(0xFFE5E7EB))
    Spacer(Modifier.height(12.dp))

    // "🎙️ Read: بٌ" prompt — shown when a cell is selected and not yet in result state
    if (targetGlyph.isNotEmpty() && speakState !is SpeakState.Result) {
        Box(
            Modifier
                .fillMaxWidth()
                .background(TanweenAccent.copy(.08f), RoundedCornerShape(12.dp))
                .border(1.dp, TanweenAccent.copy(.25f), RoundedCornerShape(12.dp))
                .padding(horizontal = 16.dp, vertical = 10.dp),
            Alignment.Center
        ) {
            Row(
                verticalAlignment     = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.Center
            ) {
                Text("🎙️ ", fontSize = 16.sp)
                Text("Read: ", fontSize = 14.sp, color = TanweenDark, fontWeight = FontWeight.SemiBold)
                Text(targetGlyph, fontSize = 34.sp, fontWeight = FontWeight.Bold, color = TanweenAccent)
            }
        }
        Spacer(Modifier.height(12.dp))
    }

    Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
        Text("উচ্চারণ অনুশীলন", fontSize = 13.sp, fontWeight = FontWeight.Bold, color = TanweenDark)
        Box(
            Modifier
                .background(Color(0xFFFEF3C7), RoundedCornerShape(10.dp))
                .clickable { onPlayAudio() }
                .padding(horizontal = 12.dp, vertical = 6.dp)
        ) {
            Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(5.dp)) {
                Text("🔊", fontSize = 13.sp)
                Text("শুনুন", fontSize = 12.sp, fontWeight = FontWeight.Bold, color = TanweenDark)
            }
        }
    }

    Spacer(Modifier.height(14.dp))

    when (speakState) {
        is SpeakState.Idle       -> TajweedSpeakIdle(onStart)
        is SpeakState.Recording  -> TajweedSpeakRecording(onStop)
        is SpeakState.Processing -> TajweedSpeakProcessing()
        is SpeakState.Result     -> TajweedSpeakResult(speakState.data, targetGlyph, targetSound, onReset)
        is SpeakState.Error      -> TajweedSpeakError(speakState.message, onReset)
    }
}

@Composable
private fun TajweedSpeakIdle(onStart: () -> Unit) {
    val haptic = LocalHapticFeedback.current
    Column(
        Modifier.fillMaxWidth(),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(8.dp)
    ) {
        Text("নিচে ট্যাপ করে বর্ণটি উচ্চারণ করুন", fontSize = 11.sp, color = SlateGrey, textAlign = TextAlign.Center)
        Box(
            Modifier
                .size(68.dp)
                .background(Brush.linearGradient(listOf(TanweenDark, TanweenAccent)), CircleShape)
                .clickable {
                    haptic.performHapticFeedback(HapticFeedbackType.LongPress)
                    onStart()
                },
            Alignment.Center
        ) { Text("🎙️", fontSize = 28.sp) }
        Text("ট্যাপ করে শুরু করুন", fontSize = 11.sp, color = TanweenAccent, fontWeight = FontWeight.SemiBold)
    }
}

@Composable
private fun TajweedSpeakRecording(onStop: () -> Unit) {
    val transition = rememberInfiniteTransition(label = "pulse")
    val scaleAnim by transition.animateFloat(1f, 1.2f, label = "s",
        animationSpec = infiniteRepeatable(tween(700), RepeatMode.Reverse))
    val alphaAnim by transition.animateFloat(.35f, 1f, label = "a",
        animationSpec = infiniteRepeatable(tween(700), RepeatMode.Reverse))

    Column(Modifier.fillMaxWidth(), horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(8.dp)) {
        Box(contentAlignment = Alignment.Center) {
            Box(Modifier.size(80.dp).scale(scaleAnim).background(SpeakRecording.copy(alphaAnim * .25f), CircleShape))
            Box(
                Modifier.size(60.dp).background(SpeakRecording, CircleShape).clickable { onStop() },
                Alignment.Center
            ) { Text("⏹️", fontSize = 22.sp) }
        }
        Text("রেকর্ড হচ্ছে...", fontSize = 12.sp, fontWeight = FontWeight.Bold, color = SpeakRecording)
        Text("শেষ হলে আবার ট্যাপ করুন", fontSize = 10.sp, color = SlateGrey)
    }
}

@Composable
private fun TajweedSpeakProcessing() {
    Column(
        Modifier.fillMaxWidth().padding(vertical = 8.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(8.dp)
    ) {
        CircularProgressIndicator(color = SpeakAccent, strokeWidth = 3.dp, modifier = Modifier.size(44.dp))
        Text("বিশ্লেষণ হচ্ছে...", fontSize = 13.sp, fontWeight = FontWeight.Bold, color = TanweenDark)
    }
}

@Composable
private fun TajweedSpeakResult(
    result:      PronunciationResult,
    targetGlyph: String = "",
    targetSound: String = "",
    onReset:     () -> Unit,
) {
    val isCorrect   = result.isCorrect
    val statusColor = if (isCorrect) SpeakSuccess else SpeakFail

    Column(verticalArrangement = Arrangement.spacedBy(10.dp)) {

        // ── Main result card ──────────────────────────────────────────────
        Box(
            Modifier
                .fillMaxWidth()
                .background(
                    if (isCorrect) Color(0xFFF0FDF4) else Color(0xFFFFF1F2),
                    RoundedCornerShape(12.dp)
                )
                .border(1.5.dp, statusColor, RoundedCornerShape(12.dp))
                .padding(14.dp)
        ) {
            Column(verticalArrangement = Arrangement.spacedBy(10.dp)) {

                // "🎙️ Read: بٌ" recap row
                if (targetGlyph.isNotEmpty()) {
                    Row(
                        verticalAlignment     = Alignment.CenterVertically,
                        horizontalArrangement = Arrangement.spacedBy(6.dp)
                    ) {
                        Text("🎙️", fontSize = 14.sp)
                        Text("Read:", fontSize = 12.sp, color = SlateGrey, fontWeight = FontWeight.SemiBold)
                        Text(targetGlyph, fontSize = 30.sp, fontWeight = FontWeight.Bold, color = TanweenAccent)
                    }
                    HorizontalDivider(color = Color(0xFFE5E7EB))
                }

                // Expected
                Row(
                    Modifier.fillMaxWidth(),
                    Arrangement.SpaceBetween,
                    Alignment.CenterVertically
                ) {
                    Text("Expected:", fontSize = 12.sp, color = SlateGrey, fontWeight = FontWeight.SemiBold)
                    Text(
                        targetSound.ifEmpty { result.letter },
                        fontSize = 18.sp, fontWeight = FontWeight.ExtraBold,
                        color = TanweenDark
                    )
                }

                // Your Reading
                Row(
                    Modifier.fillMaxWidth(),
                    Arrangement.SpaceBetween,
                    Alignment.CenterVertically
                ) {
                    Text("Your Reading:", fontSize = 12.sp, color = SlateGrey, fontWeight = FontWeight.SemiBold)
                    Text(
                        result.recognizedText.ifBlank { "—" },
                        fontSize = 18.sp, fontWeight = FontWeight.ExtraBold,
                        color = statusColor
                    )
                }

                HorizontalDivider(color = Color(0xFFE5E7EB))

                // ✓ Correct / ✗ Wrong
                Box(Modifier.fillMaxWidth(), Alignment.Center) {
                    Text(
                        if (isCorrect) "✓ Correct" else "✗ Wrong",
                        fontSize = 18.sp, fontWeight = FontWeight.ExtraBold,
                        color = statusColor
                    )
                }
            }
        }

        // ── Makhraj hint ──────────────────────────────────────────────────
        if (result.makhrajHint.isNotBlank()) {
            Row(
                Modifier
                    .fillMaxWidth()
                    .background(Color(0xFFEFF6FF), RoundedCornerShape(10.dp))
                    .border(1.dp, Color(0xFFBAE6FD), RoundedCornerShape(10.dp))
                    .padding(10.dp),
                horizontalArrangement = Arrangement.spacedBy(8.dp),
                verticalAlignment     = Alignment.Top
            ) {
                Text("ℹ️", fontSize = 12.sp)
                Text(result.makhrajHint, fontSize = 11.sp, color = Color(0xFF1E40AF), lineHeight = 16.sp)
            }
        }

        // ── Retry ─────────────────────────────────────────────────────────
        Box(
            Modifier
                .fillMaxWidth()
                .clip(RoundedCornerShape(12.dp))
                .background(Brush.linearGradient(listOf(TanweenDark, TanweenAccent)))
                .clickable { onReset() }
                .padding(vertical = 10.dp),
            Alignment.Center
        ) { Text("🔄 আবার চেষ্টা করুন", fontSize = 12.sp, fontWeight = FontWeight.Bold, color = Color.White) }
    }
}

@Composable
private fun TajweedSpeakError(message: String, onReset: () -> Unit) {
    Column(
        Modifier.fillMaxWidth(),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(10.dp)
    ) {
        Text("⚠️  $message", fontSize = 12.sp, color = SpeakFail, textAlign = TextAlign.Center)
        Box(
            Modifier
                .fillMaxWidth()
                .clip(RoundedCornerShape(12.dp))
                .background(Brush.linearGradient(listOf(TanweenDark, TanweenAccent)))
                .clickable { onReset() }
                .padding(vertical = 10.dp),
            Alignment.Center
        ) { Text("🔄 আবার চেষ্টা করুন", fontSize = 12.sp, fontWeight = FontWeight.Bold, color = Color.White) }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
//  Card 1 — হারাকত ও তানওয়িন (combined side-by-side table)
// ─────────────────────────────────────────────────────────────────────────────

private data class VowelRowData(
    val harakatGlyph: String,
    val harakatSound: String,
    val harakatName:  String,
    val tanweenGlyph: String,
    val tanweenSound: String,
    val tanweenName:  String,
    val color:        Color,
)

@Composable
private fun HarakatTanweenCard(
    letter:      ArabicLetter,
    speakState:  SpeakState,
    onPlayAudio: () -> Unit,
    onStart:     () -> Unit,
    onStop:      () -> Unit,
    onReset:     () -> Unit,
) {
    val isTablet   = LocalConfiguration.current.screenWidthDp >= 600
    val l          = letter.letter
    val fathaColor = Color(0xFFD97706)
    val kasraColor = Color(0xFF7C3AED)
    val dammaColor = Color(0xFF059669)

    val rows = listOf(
        VowelRowData("$lَ", "Ba",  "ফাতহা",      "$lً", "Ban", "ফাতহাতাইন", fathaColor),
        VowelRowData("$lِ", "Bi",  "কাসরা",       "$lٍ", "Bin", "কাসরাতাইন", kasraColor),
        VowelRowData("$lُ", "Bu",  "দাম্মা",      "$lٌ", "Bun", "দাম্মাতাইন", dammaColor),
    )

    // selectedRow: 0–2, selectedSide: true=harakat, false=tanween
    var selectedRow   by remember { mutableStateOf(0) }
    var isHarakatSide by remember { mutableStateOf(true) }

    val targetGlyph = if (isHarakatSide) rows[selectedRow].harakatGlyph else rows[selectedRow].tanweenGlyph
    val targetSound = if (isHarakatSide) rows[selectedRow].harakatSound  else rows[selectedRow].tanweenSound

    Card(
        modifier  = Modifier.fillMaxWidth().padding(horizontal = if (isTablet) 0.dp else 16.dp),
        shape     = RoundedCornerShape(20.dp),
        colors    = CardDefaults.cardColors(HarakatBg),
        elevation = CardDefaults.cardElevation(6.dp)
    ) {
        Column(Modifier.padding(20.dp)) {

            // ── Card header ───────────────────────────────────────────────
            Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                Column {
                    Text("হারাকত ও তানওয়িন", fontSize = 18.sp, fontWeight = FontWeight.ExtraBold, color = Color(0xFF78350F))
                    Text("ট্যাপ করে অনুশীলনের লক্ষ্য বেছে নিন", fontSize = 11.sp, color = fathaColor.copy(.8f), fontWeight = FontWeight.SemiBold)
                }
            }

            Spacer(Modifier.height(14.dp))

            // ── Table header ──────────────────────────────────────────────
            Row(Modifier.fillMaxWidth()) {
                // Harakat header
                Row(
                    Modifier.weight(1f).background(Color(0xFFFEF3C7), RoundedCornerShape(topStart = 10.dp, topEnd = 0.dp, bottomStart = 0.dp, bottomEnd = 0.dp)).padding(horizontal = 12.dp, vertical = 6.dp),
                    Arrangement.SpaceBetween
                ) {
                    Text("Harakat", fontSize = 11.sp, fontWeight = FontWeight.Bold, color = Color(0xFF92400E))
                    Text("Sound",   fontSize = 11.sp, fontWeight = FontWeight.Bold, color = Color(0xFF92400E))
                }
                Spacer(Modifier.width(4.dp))
                // Tanween header
                Row(
                    Modifier.weight(1f).background(Color(0xFFEDE9FE), RoundedCornerShape(topStart = 0.dp, topEnd = 10.dp, bottomStart = 0.dp, bottomEnd = 0.dp)).padding(horizontal = 12.dp, vertical = 6.dp),
                    Arrangement.SpaceBetween
                ) {
                    Text("Tanween", fontSize = 11.sp, fontWeight = FontWeight.Bold, color = Color(0xFF5B21B6))
                    Text("Sound",   fontSize = 11.sp, fontWeight = FontWeight.Bold, color = Color(0xFF5B21B6))
                }
            }

            // ── Table rows ────────────────────────────────────────────────
            rows.forEachIndexed { idx, row ->
                Row(
                    Modifier.fillMaxWidth().padding(top = 4.dp),
                    horizontalArrangement = Arrangement.spacedBy(4.dp)
                ) {
                    // Harakat cell
                    val harakatSel = selectedRow == idx && isHarakatSide
                    Row(
                        modifier = Modifier
                            .weight(1f)
                            .clip(RoundedCornerShape(10.dp))
                            .background(if (harakatSel) row.color.copy(.15f) else row.color.copy(.05f))
                            .border(
                                width = if (harakatSel) 2.dp else 1.dp,
                                color = if (harakatSel) row.color else row.color.copy(.2f),
                                shape = RoundedCornerShape(10.dp)
                            )
                            .clickable {
                                if (!harakatSel) onReset()
                                selectedRow = idx; isHarakatSide = true
                            }
                            .padding(horizontal = 12.dp, vertical = 10.dp),
                        horizontalArrangement = Arrangement.SpaceBetween,
                        verticalAlignment     = Alignment.CenterVertically
                    ) {
                        Text(row.harakatGlyph, fontSize = 28.sp, fontWeight = FontWeight.Bold,      color = row.color)
                        Text(row.harakatSound, fontSize = 15.sp, fontWeight = FontWeight.ExtraBold, color = Color(0xFF1E293B))
                    }

                    // Tanween cell
                    val tanweenSel = selectedRow == idx && !isHarakatSide
                    Row(
                        modifier = Modifier
                            .weight(1f)
                            .clip(RoundedCornerShape(10.dp))
                            .background(if (tanweenSel) row.color.copy(.15f) else row.color.copy(.05f))
                            .border(
                                width = if (tanweenSel) 2.dp else 1.dp,
                                color = if (tanweenSel) row.color else row.color.copy(.2f),
                                shape = RoundedCornerShape(10.dp)
                            )
                            .clickable {
                                if (!tanweenSel) onReset()
                                selectedRow = idx; isHarakatSide = false
                            }
                            .padding(horizontal = 12.dp, vertical = 10.dp),
                        horizontalArrangement = Arrangement.SpaceBetween,
                        verticalAlignment     = Alignment.CenterVertically
                    ) {
                        Text(row.tanweenGlyph, fontSize = 28.sp, fontWeight = FontWeight.Bold,      color = row.color)
                        Text(row.tanweenSound, fontSize = 15.sp, fontWeight = FontWeight.ExtraBold, color = Color(0xFF1E293B))
                    }
                }
            }

            // ── Speak section ─────────────────────────────────────────────
            TajweedSpeakSection(speakState, onPlayAudio, onStart, onStop, onReset, targetGlyph, targetSound)
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
//  Card 3 — সুকূন ও শাদ্দা
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun SukoonShaddahCard(
    letter:      ArabicLetter,
    speakState:  SpeakState,
    onPlayAudio: () -> Unit,
    onStart:     () -> Unit,
    onStop:      () -> Unit,
    onReset:     () -> Unit,
) {
    val isTablet   = LocalConfiguration.current.screenWidthDp >= 600
    val l          = letter.letter
    val indigoHigh = Color(0xFF4F46E5)
    val redHigh    = Color(0xFFDC2626)

    Card(
        modifier  = Modifier.fillMaxWidth().padding(horizontal = if (isTablet) 0.dp else 16.dp),
        shape     = RoundedCornerShape(20.dp),
        colors    = CardDefaults.cardColors(SukoonBg),
        elevation = CardDefaults.cardElevation(6.dp)
    ) {
        Column(Modifier.padding(20.dp)) {
            Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                Column {
                    Text("সুকূন ও শাদ্দা", fontSize = 18.sp, fontWeight = FontWeight.ExtraBold, color = Color(0xFF3730A3))
                    Text("বিরতি ও দ্বিগুণ উচ্চারণ", fontSize = 12.sp, color = indigoHigh.copy(.8f), fontWeight = FontWeight.SemiBold)
                }
                Box(Modifier.background(SukoonBorder, RoundedCornerShape(12.dp)).padding(horizontal = 10.dp, vertical = 5.dp)) {
                    Text("ْ  ّ", fontSize = 18.sp, color = Color(0xFF3730A3), fontWeight = FontWeight.Bold)
                }
            }
            Spacer(Modifier.height(16.dp))
            Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(10.dp)) {
                Column(
                    modifier = Modifier.weight(1f)
                        .background(indigoHigh.copy(.07f), RoundedCornerShape(14.dp))
                        .border(1.5.dp, SukoonBorder, RoundedCornerShape(14.dp))
                        .padding(14.dp),
                    horizontalAlignment = Alignment.CenterHorizontally,
                    verticalArrangement = Arrangement.spacedBy(8.dp)
                ) {
                    Text("اَ${l}ْ",             fontSize = 36.sp, fontWeight = FontWeight.Bold,      color = indigoHigh)
                    Text("সুকূন",               fontSize = 13.sp, fontWeight = FontWeight.ExtraBold, color = Color(0xFF3730A3))
                    Text("স্বরহীন — থেমে যাও", fontSize = 10.sp, color = SlateGrey, textAlign = TextAlign.Center, lineHeight = 14.sp)
                }
                Column(
                    modifier = Modifier.weight(1f)
                        .background(redHigh.copy(.07f), RoundedCornerShape(14.dp))
                        .border(1.5.dp, Color(0xFFFCA5A5), RoundedCornerShape(14.dp))
                        .padding(14.dp),
                    horizontalAlignment = Alignment.CenterHorizontally,
                    verticalArrangement = Arrangement.spacedBy(8.dp)
                ) {
                    Text("اَ${l}َّ",            fontSize = 36.sp, fontWeight = FontWeight.Bold,      color = redHigh)
                    Text("শাদ্দা",              fontSize = 13.sp, fontWeight = FontWeight.ExtraBold, color = Color(0xFF991B1B))
                    Text("দ্বিগুণ উচ্চারণ",      fontSize = 10.sp, color = SlateGrey, textAlign = TextAlign.Center, lineHeight = 14.sp)
                }
            }
            TajweedSpeakSection(speakState, onPlayAudio, onStart, onStop, onReset)
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
//  Card 3 — শব্দ গঠন
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun WordBuildingCard(
    letter:      ArabicLetter,
    speakState:  SpeakState,
    onPlayAudio: () -> Unit,
    onStart:     () -> Unit,
    onStop:      () -> Unit,
    onReset:     () -> Unit,
) {
    val isTablet = LocalConfiguration.current.screenWidthDp >= 600
    val l        = letter.initialForm

    data class WordRow(val vowel: String, val result: String, val sound: String, val color: Color)
    val rows = listOf(
        WordRow("ا", "${l}ا", "ā — (আ)", Color(0xFF2563EB)),
        WordRow("ي", "${l}ي", "ī — (ই)",  Color(0xFF7C3AED)),
        WordRow("و", "${l}و", "ū — (উ)",  Color(0xFF059669)),
    )

    Card(
        modifier  = Modifier.fillMaxWidth().padding(horizontal = if (isTablet) 0.dp else 16.dp),
        shape     = RoundedCornerShape(20.dp),
        colors    = CardDefaults.cardColors(WordBg),
        elevation = CardDefaults.cardElevation(6.dp)
    ) {
        Column(Modifier.padding(20.dp)) {
            Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                Column {
                    Text("শব্দ গঠন", fontSize = 18.sp, fontWeight = FontWeight.ExtraBold, color = Color(0xFF064E3B))
                    Text("দীর্ঘ স্বর সংযোগ", fontSize = 12.sp, color = Color(0xFF059669).copy(.8f), fontWeight = FontWeight.SemiBold)
                }
                Box(Modifier.background(WordBorder, RoundedCornerShape(12.dp)).padding(horizontal = 10.dp, vertical = 5.dp)) {
                    Text("ا  ي  و", fontSize = 16.sp, color = Color(0xFF064E3B), fontWeight = FontWeight.Bold)
                }
            }
            Spacer(Modifier.height(16.dp))
            Column(verticalArrangement = Arrangement.spacedBy(10.dp)) {
                rows.forEach { (vowel, result, sound, color) ->
                    Row(
                        modifier = Modifier.fillMaxWidth()
                            .background(color.copy(.07f), RoundedCornerShape(14.dp))
                            .border(1.dp, color.copy(.25f), RoundedCornerShape(14.dp))
                            .padding(horizontal = 16.dp, vertical = 12.dp),
                        horizontalArrangement = Arrangement.SpaceBetween,
                        verticalAlignment     = Alignment.CenterVertically
                    ) {
                        Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(10.dp)) {
                            Text(letter.letter, fontSize = 28.sp, fontWeight = FontWeight.Bold,      color = Color(0xFF1E293B))
                            Text("+",           fontSize = 16.sp, fontWeight = FontWeight.Bold,      color = SlateGrey)
                            Text(vowel,         fontSize = 28.sp, fontWeight = FontWeight.Bold,      color = color)
                        }
                        Text("=", fontSize = 20.sp, fontWeight = FontWeight.ExtraBold, color = SlateGrey)
                        Column(horizontalAlignment = Alignment.End, verticalArrangement = Arrangement.spacedBy(2.dp)) {
                            Text(result, fontSize = 36.sp, fontWeight = FontWeight.ExtraBold, color = color)
                            Text(sound,  fontSize = 10.sp, color = SlateGrey, fontWeight = FontWeight.SemiBold)
                        }
                    }
                }
            }
            TajweedSpeakSection(speakState, onPlayAudio, onStart, onStop, onReset)
        }
    }
}
