package com.example.quran_app.ui.screens.letters

import android.Manifest
import android.content.pm.PackageManager
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.animation.*
import androidx.compose.animation.core.*
import androidx.compose.foundation.Canvas
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
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.geometry.Size
import androidx.compose.ui.graphics.*
import androidx.compose.ui.graphics.drawscope.Stroke
import androidx.compose.ui.hapticfeedback.HapticFeedbackType
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

// ── Palette (Tanween Learning design language) ────────────────────────────────
private val SkGreen   = Color(0xFF16A34A)
private val SkDark    = Color(0xFF14532D)
private val SkIndigo  = Color(0xFF4F46E5)
private val SkRed     = Color(0xFFE11D48)
private val SkAmber   = Color(0xFFD97706)
private val PageBg    = Color(0xFFF8FAFC)

private enum class PStep { SUKOON, SHADDAH }

// ─────────────────────────────────────────────────────────────────────────────
//  Root
// ─────────────────────────────────────────────────────────────────────────────

@Composable
fun SukoonLessonScreen(
    viewModel: ArabicLettersViewModel,
    onBack:    () -> Unit,
) {
    val letters        by viewModel.letters.collectAsState()
    val selectedLetter by viewModel.selectedLetter.collectAsState()
    val totalCount     by viewModel.totalCount.collectAsState()
    val isLoading      by viewModel.isLoading.collectAsState()
    val error          by viewModel.error.collectAsState()
    val speakState     by viewModel.speakState.collectAsState()

    var showPractice by remember { mutableStateOf(false) }
    var practiceStep by remember { mutableStateOf(PStep.SUKOON) }

    val context      = LocalContext.current
    val permLauncher = rememberLauncherForActivityResult(
        ActivityResultContracts.RequestPermission()
    ) { granted -> if (granted) viewModel.startRecording() }

    LaunchedEffect(speakState) {
        val letter = selectedLetter
        if (speakState is SpeakState.Result && letter != null) {
            viewModel.recordTajweedProgress(
                letterId = letter.id,
                mode     = "sukoon_shaddah",
                result   = (speakState as SpeakState.Result).data
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

    val haptic = LocalHapticFeedback.current

    val onStart: () -> Unit = {
        val ok = ContextCompat.checkSelfPermission(context, Manifest.permission.RECORD_AUDIO) ==
                PackageManager.PERMISSION_GRANTED
        if (ok) viewModel.startRecording() else permLauncher.launch(Manifest.permission.RECORD_AUDIO)
    }
    val onStop:      () -> Unit = { selectedLetter?.let { viewModel.stopAndAssess(it.id) } }
    val onReset:     () -> Unit = { viewModel.resetSpeak() }
    val onPlayAudio: () -> Unit = { selectedLetter?.let { viewModel.playAudio(it.audioUrl) } }
    val onPrev:      () -> Unit = {
        haptic.performHapticFeedback(HapticFeedbackType.LongPress)
        if (!isFirst) viewModel.selectLetter(letters[selectedIndex - 1])
    }
    val onNext: () -> Unit = {
        haptic.performHapticFeedback(HapticFeedbackType.LongPress)
        if (!isLast) viewModel.selectLetter(letters[selectedIndex + 1])
    }

    Box(Modifier.fillMaxSize().background(PageBg)) {
        when {
            isLoading && letters.isEmpty() ->
                CircularProgressIndicator(
                    Modifier.align(Alignment.Center), color = SkGreen, strokeWidth = 3.dp
                )
            error != null && letters.isEmpty() ->
                Column(
                    Modifier.align(Alignment.Center).padding(40.dp),
                    verticalArrangement    = Arrangement.spacedBy(16.dp),
                    horizontalAlignment    = Alignment.CenterHorizontally
                ) {
                    Text("📡", fontSize = 48.sp)
                    Text("সংযোগ হচ্ছে না", color = SkDark, fontSize = 18.sp, fontWeight = FontWeight.Bold)
                    Button(onClick = { viewModel.fetchLetters() },
                        colors = ButtonDefaults.buttonColors(SkGreen)) {
                        Text("পুনরায় চেষ্টা", color = Color.White, fontWeight = FontWeight.Bold)
                    }
                }
            else -> AnimatedContent(
                targetState    = showPractice,
                transitionSpec = {
                    if (targetState)
                        (slideInHorizontally { it } + fadeIn()) togetherWith
                        (slideOutHorizontally { -it } + fadeOut())
                    else
                        (slideInHorizontally { -it } + fadeIn()) togetherWith
                        (slideOutHorizontally { it } + fadeOut())
                },
                label = "sukoon-switch"
            ) { isPractice ->
                if (isPractice) {
                    PracticeView(
                        letter        = selectedLetter,
                        selectedIndex = selectedIndex,
                        displayTotal  = displayTotal,
                        step          = practiceStep,
                        speakState    = speakState,
                        onBack        = { showPractice = false; onReset() },
                        onStepSelect  = { practiceStep = it; onReset() },
                        onPlayAudio   = onPlayAudio,
                        onStart       = onStart,
                        onStop        = onStop,
                        onReset       = onReset,
                        onPrev        = onPrev,
                        onNext        = onNext,
                        isFirst       = isFirst,
                        isLast        = isLast,
                    )
                } else {
                    LessonView(
                        letter        = selectedLetter,
                        selectedIndex = selectedIndex,
                        displayTotal  = displayTotal,
                        onBack        = onBack,
                        onPractice    = { showPractice = true },
                        onPlayAudio   = onPlayAudio,
                        onPrev        = onPrev,
                        onNext        = onNext,
                        isFirst       = isFirst,
                        isLast        = isLast,
                    )
                }
            }
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
//  Lesson view
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun LessonView(
    letter:        ArabicLetter?,
    selectedIndex: Int,
    displayTotal:  Int,
    onBack:        () -> Unit,
    onPractice:    () -> Unit,
    onPlayAudio:   () -> Unit,
    onPrev:        () -> Unit,
    onNext:        () -> Unit,
    isFirst:       Boolean,
    isLast:        Boolean,
) {
    val l        = letter?.letter ?: "ب"
    val translit = letter?.transliteration?.firstOrNull()?.toString() ?: "B"
    val progress = if (displayTotal > 0) (selectedIndex + 1).toFloat() / displayTotal else 0f

    Column(
        Modifier
            .fillMaxSize()
            .verticalScroll(rememberScrollState())
    ) {

        // ── Green gradient header ─────────────────────────────────────────
        Box(
            Modifier
                .fillMaxWidth()
                .background(
                    Brush.verticalGradient(listOf(SkDark, SkGreen)),
                    RoundedCornerShape(bottomStart = 24.dp, bottomEnd = 24.dp)
                )
        ) {
            Column(
                Modifier
                    .fillMaxWidth()
                    .statusBarsPadding()
                    .padding(horizontal = 16.dp, vertical = 12.dp)
                    .padding(bottom = 8.dp)
            ) {
                Row(
                    Modifier.fillMaxWidth(),
                    Arrangement.SpaceBetween, Alignment.CenterVertically
                ) {
                    Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(10.dp)) {
                        Box(
                            Modifier.size(36.dp)
                                .background(Color.White.copy(.22f), CircleShape)
                                .border(1.dp, Color.White.copy(.3f), CircleShape)
                                .clickable { onBack() },
                            Alignment.Center
                        ) { Text("←", fontSize = 16.sp, color = Color.White, fontWeight = FontWeight.Bold) }
                        Column {
                            Text("Sukoon Learning", fontSize = 17.sp, fontWeight = FontWeight.Bold, color = Color.White)
                            Text("Lesson ${selectedIndex + 1} of $displayTotal", fontSize = 11.sp, color = Color.White.copy(.75f))
                        }
                    }
                    Box(
                        Modifier.size(36.dp)
                            .background(Color.White.copy(.22f), RoundedCornerShape(10.dp))
                            .border(1.dp, Color.White.copy(.3f), RoundedCornerShape(10.dp)),
                        Alignment.Center
                    ) { Text("📖", fontSize = 16.sp) }
                }
                Spacer(Modifier.height(12.dp))
                Row(Modifier.fillMaxWidth(), Arrangement.End) {
                    Text("${(progress * 100).toInt()}% Complete", fontSize = 11.sp, color = Color.White.copy(.85f), fontWeight = FontWeight.SemiBold)
                }
                Spacer(Modifier.height(4.dp))
                Box(
                    Modifier.fillMaxWidth().height(6.dp)
                        .background(Color.White.copy(.3f), RoundedCornerShape(3.dp))
                ) {
                    Box(
                        Modifier.fillMaxWidth(progress).fillMaxHeight()
                            .background(Color.White, RoundedCornerShape(3.dp))
                    )
                }
            }
        }

        Spacer(Modifier.height(16.dp))

        // ── Main card ─────────────────────────────────────────────────────
        Card(
            Modifier.fillMaxWidth().padding(horizontal = 16.dp),
            shape     = RoundedCornerShape(20.dp),
            colors    = CardDefaults.cardColors(Color.White),
            elevation = CardDefaults.cardElevation(4.dp)
        ) {
            Column(Modifier.padding(20.dp)) {

                // "What is Sukoon?" section
                Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.Top) {
                    Column(Modifier.weight(1f).padding(end = 12.dp)) {
                        Text("What is Sukoon?", fontSize = 17.sp, fontWeight = FontWeight.ExtraBold, color = SkDark)
                        Spacer(Modifier.height(4.dp))
                        Text(
                            "Sukoon (ْ◌) is a mark placed on a letter that has no vowel. It makes the letter silent — only the consonant sound is pronounced.",
                            fontSize = 13.sp, color = Color(0xFF475569), lineHeight = 19.sp
                        )
                    }
                    Box(Modifier.size(38.dp).background(Color(0xFFDCFCE7), CircleShape), Alignment.Center) {
                        Text("💡", fontSize = 18.sp)
                    }
                }

                Spacer(Modifier.height(14.dp))

                // Ba (Fatha) → Ba (Sukoon) example
                Box(
                    Modifier.fillMaxWidth()
                        .background(Color(0xFFFEF9EC), RoundedCornerShape(14.dp))
                        .border(1.dp, Color(0xFFFDE68A), RoundedCornerShape(14.dp))
                        .padding(horizontal = 16.dp, vertical = 14.dp)
                ) {
                    Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                        Column(horizontalAlignment = Alignment.CenterHorizontally) {
                            Text("${l}َ", fontSize = 36.sp, fontWeight = FontWeight.Bold, color = SkAmber)
                            Text("${translit}a", fontSize = 14.sp, fontWeight = FontWeight.ExtraBold, color = SkAmber)
                        }
                        Text("→", fontSize = 22.sp, color = Color(0xFF94A3B8), fontWeight = FontWeight.Bold)
                        Column(horizontalAlignment = Alignment.CenterHorizontally) {
                            Text("${l}ْ", fontSize = 36.sp, fontWeight = FontWeight.Bold, color = SkIndigo)
                            Text("Sukoon", fontSize = 11.sp, fontWeight = FontWeight.SemiBold, color = SkIndigo)
                        }
                        Box(
                            Modifier.size(40.dp)
                                .background(SkGreen.copy(.12f), CircleShape)
                                .border(1.dp, SkGreen.copy(.3f), CircleShape)
                                .clickable { onPlayAudio() },
                            Alignment.Center
                        ) { Text("🎙️", fontSize = 18.sp) }
                    }
                }

                Spacer(Modifier.height(18.dp))

                // "Sukoon Types" divider
                Row(Modifier.fillMaxWidth(), verticalAlignment = Alignment.CenterVertically) {
                    HorizontalDivider(Modifier.weight(1f), color = Color(0xFFE2E8F0))
                    Text("   Sukoon Types   ", fontSize = 12.sp, fontWeight = FontWeight.ExtraBold, color = Color(0xFF64748B))
                    HorizontalDivider(Modifier.weight(1f), color = Color(0xFFE2E8F0))
                }

                Spacer(Modifier.height(14.dp))

                // Types grid
                Column(verticalArrangement = Arrangement.spacedBy(10.dp)) {
                    TypeRow(
                        leftGlyph  = "${l}َ",  leftSound  = "${translit}a",  leftName  = "Fatha",  leftColor  = SkAmber,
                        rightGlyph = "${l}ْ",  rightSound = translit,        rightName = "Sukoon", rightColor = SkIndigo,
                        onPlay = { onPlayAudio() }
                    )
                    TypeRow(
                        leftGlyph  = "${l}َ",   leftSound  = "${translit}a",         leftName  = "Fatha",  leftColor  = SkAmber,
                        rightGlyph = "${l}َّ",  rightSound = "${translit}a${translit}", rightName = "Shaddah", rightColor = SkRed,
                        onPlay = { onPlayAudio() }
                    )
                }

                Spacer(Modifier.height(18.dp))
                HorizontalDivider(color = Color(0xFFE2E8F0))
                Spacer(Modifier.height(14.dp))

                // Listen & Compare
                Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(10.dp)) {
                    Box(Modifier.size(36.dp).background(SkGreen.copy(.12f), RoundedCornerShape(10.dp)), Alignment.Center) {
                        Text("🎧", fontSize = 18.sp)
                    }
                    Column {
                        Text("Listen & Compare", fontSize = 14.sp, fontWeight = FontWeight.ExtraBold, color = SkDark)
                        Text("Listen to the difference", fontSize = 11.sp, color = Color(0xFF64748B))
                    }
                }

                Spacer(Modifier.height(12.dp))

                Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(8.dp)) {
                    CompareChip("${translit}a vs ${translit}", SkIndigo, { onPlayAudio() }, Modifier.weight(1f))
                    CompareChip("${translit}a vs ${translit}a${translit}", SkRed, { onPlayAudio() }, Modifier.weight(1f))
                }
            }
        }

        Spacer(Modifier.height(16.dp))

        // ── Previous / Lessons / Next ─────────────────────────────────────
        Row(
            Modifier.fillMaxWidth().padding(horizontal = 16.dp),
            Arrangement.SpaceBetween, Alignment.CenterVertically
        ) {
            OutlineNavBtn("← Previous", enabled = !isFirst, onClick = onPrev)
            Column(horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(4.dp)) {
                Box(
                    Modifier.size(52.dp).clip(CircleShape)
                        .background(SkGreen)
                        .clickable { onPractice() },
                    Alignment.Center
                ) { Text("📖", fontSize = 22.sp) }
                Text("Lessons", fontSize = 10.sp, color = SkDark, fontWeight = FontWeight.SemiBold)
            }
            OutlineNavBtn("Next →", enabled = !isLast, onClick = onNext)
        }

        Spacer(Modifier.height(20.dp))
    }
}

// ── Shared sub-composables for lesson ─────────────────────────────────────────

@Composable
private fun TypeRow(
    leftGlyph: String, leftSound: String, leftName: String, leftColor: Color,
    rightGlyph: String, rightSound: String, rightName: String, rightColor: Color,
    onPlay: () -> Unit,
) {
    Row(Modifier.fillMaxWidth(), verticalAlignment = Alignment.CenterVertically) {
        TypeCell(leftGlyph, leftSound, leftName, leftColor, onPlay, Modifier.weight(1f))
        Text("→", fontSize = 20.sp, color = Color(0xFF94A3B8), fontWeight = FontWeight.Bold,
            modifier = Modifier.padding(horizontal = 6.dp))
        TypeCell(rightGlyph, rightSound, rightName, rightColor, onPlay, Modifier.weight(1f))
    }
}

@Composable
private fun TypeCell(
    glyph: String, sound: String, name: String, color: Color,
    onPlay: () -> Unit, modifier: Modifier,
) {
    Box(
        modifier
            .background(color.copy(.08f), RoundedCornerShape(14.dp))
            .border(1.dp, color.copy(.3f), RoundedCornerShape(14.dp))
            .padding(10.dp)
    ) {
        Column {
            Box(Modifier.fillMaxWidth(), contentAlignment = Alignment.TopEnd) {
                Box(
                    Modifier.size(26.dp).background(color.copy(.15f), CircleShape).clickable { onPlay() },
                    Alignment.Center
                ) { Text("🔊", fontSize = 11.sp) }
            }
            Spacer(Modifier.height(4.dp))
            Box(Modifier.fillMaxWidth(), contentAlignment = Alignment.Center) {
                Text(glyph, fontSize = 38.sp, fontWeight = FontWeight.Bold, color = color, textAlign = TextAlign.Center)
            }
            Box(Modifier.fillMaxWidth(), contentAlignment = Alignment.Center) {
                Text(sound, fontSize = 15.sp, fontWeight = FontWeight.ExtraBold, color = color)
            }
            Box(Modifier.fillMaxWidth(), contentAlignment = Alignment.Center) {
                Text("($name)", fontSize = 10.sp, color = Color(0xFF94A3B8))
            }
        }
    }
}

@Composable
private fun CompareChip(label: String, color: Color, onClick: () -> Unit, modifier: Modifier) {
    Box(
        modifier
            .border(1.5.dp, color.copy(.5f), RoundedCornerShape(24.dp))
            .clickable { onClick() }
            .padding(horizontal = 12.dp, vertical = 9.dp),
        Alignment.Center
    ) {
        Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(6.dp)) {
            Text(label, fontSize = 13.sp, fontWeight = FontWeight.SemiBold, color = color)
            Box(Modifier.size(22.dp).background(color, CircleShape), Alignment.Center) {
                Text("▶", fontSize = 8.sp, color = Color.White)
            }
        }
    }
}

@Composable
private fun OutlineNavBtn(label: String, enabled: Boolean, onClick: () -> Unit) {
    val fg = if (enabled) SkDark else Color(0xFFCBD5E1)
    val border = if (enabled) Color(0xFFCBD5E1) else Color(0xFFE2E8F0)
    Box(
        Modifier
            .border(1.5.dp, border, RoundedCornerShape(24.dp))
            .clickable(enabled = enabled) { onClick() }
            .padding(horizontal = 16.dp, vertical = 12.dp)
    ) {
        Text(label, fontSize = 12.sp, fontWeight = FontWeight.SemiBold, color = fg)
    }
}

// ─────────────────────────────────────────────────────────────────────────────
//  Practice view
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun PracticeView(
    letter:        ArabicLetter?,
    selectedIndex: Int,
    displayTotal:  Int,
    step:          PStep,
    speakState:    SpeakState,
    onBack:        () -> Unit,
    onStepSelect:  (PStep) -> Unit,
    onPlayAudio:   () -> Unit,
    onStart:       () -> Unit,
    onStop:        () -> Unit,
    onReset:       () -> Unit,
    onPrev:        () -> Unit,
    onNext:        () -> Unit,
    isFirst:       Boolean,
    isLast:        Boolean,
) {
    val l        = letter?.letter ?: "ب"
    val translit = letter?.transliteration?.firstOrNull()?.toString() ?: "B"
    val isSukoon = step == PStep.SUKOON

    val glyph         = if (isSukoon) "${l}ْ" else "${l}َّ"
    val expectedSound = if (isSukoon) translit else "${translit}a${translit}"
    val accentColor   = if (isSukoon) SkIndigo else SkRed

    Column(
        Modifier
            .fillMaxSize()
            .background(Color.White)
            .verticalScroll(rememberScrollState())
    ) {
        Spacer(Modifier.statusBarsPadding())

        // ── White header ──────────────────────────────────────────────────
        Row(
            Modifier.fillMaxWidth().padding(horizontal = 16.dp, vertical = 10.dp),
            Arrangement.SpaceBetween, Alignment.CenterVertically
        ) {
            Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(10.dp)) {
                Box(
                    Modifier.size(36.dp)
                        .border(1.dp, Color(0xFFE2E8F0), CircleShape)
                        .clickable { onBack() },
                    Alignment.Center
                ) { Text("←", fontSize = 16.sp, color = SkDark, fontWeight = FontWeight.Bold) }
                Column {
                    Text("Practice Sukoon", fontSize = 16.sp, fontWeight = FontWeight.ExtraBold, color = SkDark)
                    Text("Read and get feedback", fontSize = 11.sp, color = Color(0xFF64748B))
                }
            }
            Text("🏆", fontSize = 22.sp)
        }

        // ── Step dots ─────────────────────────────────────────────────────
        Row(Modifier.fillMaxWidth(), Arrangement.Center, Alignment.CenterVertically) {
            PStep.entries.forEach { s ->
                val active = s == step
                Box(
                    Modifier
                        .padding(horizontal = 4.dp)
                        .size(if (active) 13.dp else 10.dp)
                        .then(
                            if (active)
                                Modifier.background(SkGreen, CircleShape)
                            else
                                Modifier.border(1.5.dp, Color(0xFFCBD5E1), CircleShape)
                                    .background(Color.White, CircleShape)
                        )
                        .clickable { onStepSelect(s) }
                )
            }
        }

        Spacer(Modifier.height(14.dp))

        // ── "Your Turn" card ──────────────────────────────────────────────
        Card(
            Modifier.fillMaxWidth().padding(horizontal = 16.dp),
            shape     = RoundedCornerShape(20.dp),
            colors    = CardDefaults.cardColors(Color.White),
            elevation = CardDefaults.cardElevation(3.dp)
        ) {
            Column(
                Modifier.padding(20.dp),
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.spacedBy(14.dp)
            ) {
                Row(
                    Modifier.fillMaxWidth(),
                    verticalAlignment = Alignment.CenterVertically,
                    horizontalArrangement = Arrangement.spacedBy(10.dp)
                ) {
                    Box(
                        Modifier.size(36.dp).background(SkGreen.copy(.12f), CircleShape),
                        Alignment.Center
                    ) { Text("🎙️", fontSize = 16.sp) }
                    Column {
                        Text("Your Turn", fontSize = 15.sp, fontWeight = FontWeight.ExtraBold, color = SkDark)
                        Text("Read the following", fontSize = 11.sp, color = Color(0xFF64748B))
                    }
                }

                // Letter display box
                Box(
                    Modifier
                        .fillMaxWidth()
                        .background(Color(0xFFF8FAFF), RoundedCornerShape(16.dp))
                        .border(1.dp, Color(0xFFE2E8F0), RoundedCornerShape(16.dp))
                        .padding(20.dp)
                ) {
                    Column(Modifier.fillMaxWidth(), horizontalAlignment = Alignment.CenterHorizontally) {
                        Box(Modifier.fillMaxWidth(), contentAlignment = Alignment.TopEnd) {
                            Box(
                                Modifier.size(30.dp)
                                    .background(accentColor.copy(.12f), CircleShape)
                                    .clickable { onPlayAudio() },
                                Alignment.Center
                            ) { Text("🔊", fontSize = 14.sp) }
                        }
                        Spacer(Modifier.height(4.dp))
                        Text(glyph, fontSize = 72.sp, fontWeight = FontWeight.Bold, color = accentColor, textAlign = TextAlign.Center)
                        Text("◆", fontSize = 14.sp, color = accentColor.copy(.4f))
                        Spacer(Modifier.height(8.dp))
                        Text("Expected Sound", fontSize = 11.sp, color = Color(0xFF94A3B8), fontWeight = FontWeight.SemiBold)
                        Spacer(Modifier.height(4.dp))
                        Text(expectedSound, fontSize = 28.sp, fontWeight = FontWeight.ExtraBold, color = accentColor)
                    }
                }

                // Mic widget
                when (speakState) {
                    is SpeakState.Idle       -> MicIdle(onStart)
                    is SpeakState.Recording  -> MicRecording(onStop)
                    is SpeakState.Processing -> MicProcessing()
                    else                     -> {}
                }
            }
        }

        Spacer(Modifier.height(12.dp))

        // ── Result / error ────────────────────────────────────────────────
        when (speakState) {
            is SpeakState.Result -> ResultCard(speakState.data, expectedSound, isSukoon, onReset)
            is SpeakState.Error  -> ErrorCard(speakState.message, onReset)
            else                 -> {}
        }

        if (speakState is SpeakState.Result || speakState is SpeakState.Error) {
            Spacer(Modifier.height(12.dp))
        }

        // ── Bottom nav ─────────────────────────────────────────────────────
        Row(
            Modifier.fillMaxWidth().padding(horizontal = 16.dp),
            Arrangement.SpaceBetween, Alignment.CenterVertically
        ) {
            OutlineNavBtn("← Previous", enabled = !isFirst, onClick = onPrev)
            Box(
                Modifier
                    .background(Color(0xFFF1F5F9), RoundedCornerShape(20.dp))
                    .padding(horizontal = 18.dp, vertical = 10.dp),
                Alignment.Center
            ) {
                Text(
                    "${selectedIndex + 1} / $displayTotal",
                    fontSize = 13.sp, fontWeight = FontWeight.ExtraBold, color = SkDark
                )
            }
            Box(
                Modifier
                    .clip(RoundedCornerShape(24.dp))
                    .background(if (isLast) SkDark else SkGreen)
                    .clickable(enabled = !isLast) { onNext() }
                    .padding(horizontal = 18.dp, vertical = 12.dp)
            ) {
                Text("Next →", fontSize = 12.sp, fontWeight = FontWeight.Bold, color = Color.White)
            }
        }

        Spacer(Modifier.height(20.dp))
    }
}

// ── Mic states ────────────────────────────────────────────────────────────────

@Composable
private fun MicIdle(onStart: () -> Unit) {
    val haptic = LocalHapticFeedback.current
    Column(Modifier.fillMaxWidth(), horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(8.dp)) {
        Box(
            Modifier.size(72.dp).clip(CircleShape)
                .background(SkGreen)
                .clickable { haptic.performHapticFeedback(HapticFeedbackType.LongPress); onStart() },
            Alignment.Center
        ) { Text("🎙️", fontSize = 28.sp) }
        Text("Tap the mic and read", fontSize = 12.sp, color = SkGreen, fontWeight = FontWeight.SemiBold)
    }
}

@Composable
private fun MicRecording(onStop: () -> Unit) {
    val transition = rememberInfiniteTransition(label = "pulse")
    val scaleAnim by transition.animateFloat(1f, 1.25f, label = "s",
        animationSpec = infiniteRepeatable(tween(700), RepeatMode.Reverse))
    val alphaAnim by transition.animateFloat(.3f, 1f, label = "a",
        animationSpec = infiniteRepeatable(tween(700), RepeatMode.Reverse))
    Column(Modifier.fillMaxWidth(), horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(8.dp)) {
        Box(contentAlignment = Alignment.Center) {
            Box(Modifier.size(86.dp).scale(scaleAnim).background(Color(0xFFEF4444).copy(alphaAnim * .25f), CircleShape))
            Box(Modifier.size(64.dp).background(Color(0xFFEF4444), CircleShape).clickable { onStop() }, Alignment.Center) {
                Text("⏹️", fontSize = 24.sp)
            }
        }
        Text("Recording...", fontSize = 12.sp, fontWeight = FontWeight.Bold, color = Color(0xFFEF4444))
        Text("Tap again to stop", fontSize = 10.sp, color = Color(0xFF64748B))
    }
}

@Composable
private fun MicProcessing() {
    Column(Modifier.fillMaxWidth().padding(vertical = 8.dp), horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(8.dp)) {
        CircularProgressIndicator(color = SkGreen, strokeWidth = 3.dp, modifier = Modifier.size(48.dp))
        Text("Analysing...", fontSize = 13.sp, fontWeight = FontWeight.Bold, color = SkDark)
    }
}

// ── Result card ────────────────────────────────────────────────────────────────

@Composable
private fun ResultCard(
    result:        PronunciationResult,
    expectedSound: String,
    isSukoon:      Boolean,
    onReset:       () -> Unit,
) {
    val isCorrect  = result.isCorrect
    val statusClr  = if (isCorrect) SkGreen else Color(0xFFDC2626)
    val accentClr  = if (isSukoon) SkIndigo else SkRed
    val accuracy   = (result.accuracyScore / 100.0).coerceIn(0.0, 1.0).toFloat()

    Column(Modifier.padding(horizontal = 16.dp), verticalArrangement = Arrangement.spacedBy(12.dp)) {
        Text("Your Result", fontSize = 15.sp, fontWeight = FontWeight.ExtraBold, color = SkDark)

        // Excellent / Wrong banner
        Row(
            Modifier
                .fillMaxWidth()
                .background(if (isCorrect) Color(0xFFF0FDF4) else Color(0xFFFFF1F2), RoundedCornerShape(14.dp))
                .border(1.5.dp, statusClr, RoundedCornerShape(14.dp))
                .padding(14.dp),
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(12.dp)
        ) {
            Box(Modifier.size(36.dp).background(statusClr, CircleShape), Alignment.Center) {
                Text(if (isCorrect) "✓" else "✗", fontSize = 16.sp, color = Color.White, fontWeight = FontWeight.ExtraBold)
            }
            Column {
                Text(if (isCorrect) "Excellent!" else "Try again", fontSize = 14.sp, fontWeight = FontWeight.ExtraBold, color = statusClr)
                Text(if (isCorrect) "You pronounced it correctly" else "There was an issue", fontSize = 11.sp, color = Color(0xFF64748B))
            }
        }

        // Expected | You Said
        Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(10.dp)) {
            Column(
                Modifier.weight(1f)
                    .background(Color(0xFFF8FAFC), RoundedCornerShape(12.dp))
                    .border(1.dp, Color(0xFFE2E8F0), RoundedCornerShape(12.dp))
                    .padding(12.dp),
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.spacedBy(4.dp)
            ) {
                Text("Expected", fontSize = 11.sp, color = Color(0xFF94A3B8), fontWeight = FontWeight.SemiBold)
                Text(expectedSound, fontSize = 20.sp, fontWeight = FontWeight.ExtraBold, color = accentClr)
            }
            Column(
                Modifier.weight(1f)
                    .background(Color(0xFFF8FAFC), RoundedCornerShape(12.dp))
                    .border(1.dp, Color(0xFFE2E8F0), RoundedCornerShape(12.dp))
                    .padding(12.dp),
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.spacedBy(4.dp)
            ) {
                Text("You Said", fontSize = 11.sp, color = Color(0xFF94A3B8), fontWeight = FontWeight.SemiBold)
                Text(result.recognizedText.ifBlank { "—" }, fontSize = 20.sp, fontWeight = FontWeight.ExtraBold, color = statusClr)
            }
        }

        // Accuracy ring (centred)
        Column(Modifier.fillMaxWidth(), horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(6.dp)) {
            Text("Accuracy", fontSize = 11.sp, color = Color(0xFF94A3B8), fontWeight = FontWeight.SemiBold)
            AccuracyRing(accuracy, statusClr)
        }

        // Tip
        Row(
            Modifier
                .fillMaxWidth()
                .background(Color(0xFFFFFBEB), RoundedCornerShape(12.dp))
                .border(1.dp, Color(0xFFFDE68A), RoundedCornerShape(12.dp))
                .padding(12.dp),
            horizontalArrangement = Arrangement.spacedBy(8.dp),
            verticalAlignment     = Alignment.Top
        ) {
            Text("💡", fontSize = 14.sp)
            Text(
                if (isSukoon)
                    "Sukoon (ْ◌) stops the vowel — only the consonant sound is heard."
                else
                    "Shaddah (ّ◌) doubles the consonant — first silent (sukoon), then with a vowel.",
                fontSize = 12.sp, color = Color(0xFF78350F), lineHeight = 17.sp
            )
        }

        // Retry
        Box(
            Modifier.fillMaxWidth().clip(RoundedCornerShape(14.dp))
                .background(SkGreen)
                .clickable { onReset() }
                .padding(vertical = 12.dp),
            Alignment.Center
        ) { Text("🔄 Try Again", fontSize = 13.sp, fontWeight = FontWeight.Bold, color = Color.White) }
    }
}

@Composable
private fun ErrorCard(message: String, onReset: () -> Unit) {
    Column(
        Modifier.fillMaxWidth().padding(horizontal = 16.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(10.dp)
    ) {
        Text("⚠️  $message", fontSize = 12.sp, color = Color(0xFFDC2626), textAlign = TextAlign.Center)
        Box(
            Modifier.fillMaxWidth().clip(RoundedCornerShape(14.dp))
                .background(SkGreen).clickable { onReset() }.padding(vertical = 12.dp),
            Alignment.Center
        ) { Text("🔄 Try Again", fontSize = 13.sp, fontWeight = FontWeight.Bold, color = Color.White) }
    }
}

// ── Accuracy ring ──────────────────────────────────────────────────────────────

@Composable
private fun AccuracyRing(accuracy: Float, color: Color) {
    Box(Modifier.size(80.dp), Alignment.Center) {
        Canvas(Modifier.fillMaxSize()) {
            val sw    = 8.dp.toPx()
            val inset = sw / 2f
            val arc   = Size(this.size.width - sw, this.size.height - sw)
            val tl    = Offset(inset, inset)
            drawArc(Color(0xFFE2E8F0), -90f, 360f, false, tl, arc, style = Stroke(sw, cap = StrokeCap.Round))
            if (accuracy > 0f)
                drawArc(color, -90f, 360f * accuracy, false, tl, arc, style = Stroke(sw, cap = StrokeCap.Round))
        }
        Text("${(accuracy * 100).toInt()}%", fontSize = 18.sp, fontWeight = FontWeight.ExtraBold, color = color)
    }
}
