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
import com.example.quran_app.domain.model.ConceptExample
import com.example.quran_app.domain.model.ListenCompare
import com.example.quran_app.domain.model.PronunciationResult
import com.example.quran_app.domain.model.SpeakState
import com.example.quran_app.domain.model.TanweenLessonData
import com.example.quran_app.domain.model.TanweenType
import com.example.quran_app.ui.theme.*
import com.example.quran_app.ui.viewmodel.TanweenLessonViewModel

// ── Palette ───────────────────────────────────────────────────────────────────
private val TLGreen    = Color(0xFF1B5E20)
private val TLGreenBg  = Color(0xFFE8F5E9)
private val TLPurple   = Color(0xFF6A1B9A)
private val TLPurpleBg = Color(0xFFF3E5F5)
private val TLBlue     = Color(0xFF1565C0)
private val TLBlueBg   = Color(0xFFE3F2FD)
private val TLIndigo   = Color(0xFF4527A0)
private val TLIndigoBg = Color(0xFFEDE7F6)
private val TLOrange   = Color(0xFFE65100)
private val TLOrangeBg = Color(0xFFFFF3E0)
private val TLDOrange  = Color(0xFFBF360C)
private val TLDOrangeBg= Color(0xFFFBE9E7)
private val AccentGreen = Color(0xFF2E7D32)
private val AccentDark  = Color(0xFF1B5E20)
private val PageBg      = Color(0xFFF8F9FA)

private data class VowelStyle(val fg: Color, val bg: Color, val icon: String)

private fun vowelStyle(vowelType: String): VowelStyle = when (vowelType.lowercase()) {
    "fatha"    -> VowelStyle(TLGreen,   TLGreenBg,   "◌َ")
    "fathatan" -> VowelStyle(TLPurple,  TLPurpleBg,  "◌ً")
    "kasra"    -> VowelStyle(TLBlue,    TLBlueBg,    "◌ِ")
    "kasratan" -> VowelStyle(TLIndigo,  TLIndigoBg,  "◌ٍ")
    "damma"    -> VowelStyle(TLOrange,  TLOrangeBg,  "◌ُ")
    "dammatan" -> VowelStyle(TLDOrange, TLDOrangeBg, "◌ٌ")
    else       -> VowelStyle(AppPrimary,Color(0xFFDCFCE7), "◌")
}

// ─────────────────────────────────────────────────────────────────────────────
//  Root
// ─────────────────────────────────────────────────────────────────────────────

@Composable
fun TanweenLessonScreen(viewModel: TanweenLessonViewModel) {
    val currentLesson by viewModel.currentLesson.collectAsState()
    val lessonData    by viewModel.lessonData.collectAsState()
    val isLoading     by viewModel.isLoading.collectAsState()
    val error         by viewModel.error.collectAsState()
    val speakState    by viewModel.speakState.collectAsState()
    val practiceStep  by viewModel.practiceStep.collectAsState()

    var showPractice  by remember { mutableStateOf(false) }
    var selectedRow   by remember { mutableStateOf(0) }

    val context      = LocalContext.current
    val permLauncher = rememberLauncherForActivityResult(
        ActivityResultContracts.RequestPermission()
    ) { granted -> if (granted) viewModel.startRecording() }

    LaunchedEffect(currentLesson) { showPractice = false }

    val isFirst = currentLesson <= 1
    val isLast  = currentLesson >= TanweenLessonViewModel.TOTAL_LESSONS

    val onStart: () -> Unit = {
        val ok = ContextCompat.checkSelfPermission(context, Manifest.permission.RECORD_AUDIO) ==
                PackageManager.PERMISSION_GRANTED
        if (ok) viewModel.startRecording() else permLauncher.launch(Manifest.permission.RECORD_AUDIO)
    }

    Box(Modifier.fillMaxSize().background(PageBg)) {
        when {
            isLoading -> Box(Modifier.fillMaxSize(), Alignment.Center) {
                CircularProgressIndicator(color = AccentGreen, strokeWidth = 3.dp)
            }
            error != null && lessonData == null -> ErrorState(error!!) {
                viewModel.loadLesson(currentLesson)
            }
            lessonData != null -> AnimatedContent(
                targetState    = showPractice,
                transitionSpec = {
                    if (targetState)
                        (slideInHorizontally { it } + fadeIn()) togetherWith (slideOutHorizontally { -it } + fadeOut())
                    else
                        (slideInHorizontally { -it } + fadeIn()) togetherWith (slideOutHorizontally { it } + fadeOut())
                },
                label = "lesson-switch"
            ) { isPractice ->
                if (isPractice) {
                    PracticeSessionScreen(
                        data          = lessonData!!,
                        currentLesson = currentLesson,
                        practiceStep  = practiceStep,
                        speakState    = speakState,
                        onBack        = { showPractice = false; viewModel.resetSpeak() },
                        onStepSelect  = { viewModel.setPracticeStep(it) },
                        onPlayAudio   = { url -> viewModel.playAudio(url) },
                        onStart       = onStart,
                        onStop        = { viewModel.stopAndAssess() },
                        onReset       = { viewModel.resetSpeak() },
                        onPrev        = { viewModel.prevLesson() },
                        onNext        = { viewModel.nextLesson() },
                        isFirst       = isFirst,
                        isLast        = isLast,
                    )
                } else {
                    LessonScreen(
                        data            = lessonData!!,
                        currentLesson   = currentLesson,
                        selectedRow     = selectedRow,
                        onRowSelect     = { selectedRow = it },
                        onPlayAudio     = { url -> viewModel.playAudio(url) },
                        onStartPractice = { row ->
                            viewModel.setPracticeStep(row)
                            showPractice = true
                        },
                        onPrev        = { viewModel.prevLesson() },
                        onNext        = { viewModel.nextLesson() },
                        isFirst       = isFirst,
                        isLast        = isLast,
                    )
                }
            }
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
//  Lesson screen  (left design)
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun LessonScreen(
    data:            TanweenLessonData,
    currentLesson:   Int,
    selectedRow:     Int,
    onRowSelect:     (Int) -> Unit,
    onPlayAudio:     (String) -> Unit,
    onStartPractice: (Int) -> Unit,
    onPrev:          () -> Unit,
    onNext:          () -> Unit,
    isFirst:         Boolean,
    isLast:          Boolean,
) {
    val progress = data.header.progressPercentage / 100f

    Column(
        Modifier
            .fillMaxSize()
            .verticalScroll(rememberScrollState())
    ) {
        // ── Green header ──────────────────────────────────────────────────
        Box(
            Modifier
                .fillMaxWidth()
                .background(
                    Brush.verticalGradient(listOf(AccentDark, AccentGreen)),
                    RoundedCornerShape(bottomStart = 28.dp, bottomEnd = 28.dp)
                )
        ) {
            Column(
                Modifier
                    .fillMaxWidth()
                    .statusBarsPadding()
                    .padding(horizontal = 16.dp, vertical = 14.dp)
            ) {
                Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                    Column {
                        Text(data.header.title, fontSize = 20.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
                        Text("(Lesson $currentLesson of ${data.header.totalLessons})", fontSize = 12.sp, color = Color.White.copy(.8f))
                    }
                    Box(
                        Modifier.size(38.dp)
                            .background(Color.White.copy(.2f), RoundedCornerShape(10.dp))
                            .border(1.dp, Color.White.copy(.35f), RoundedCornerShape(10.dp)),
                        Alignment.Center
                    ) { Text("📖", fontSize = 17.sp) }
                }
                Spacer(Modifier.height(12.dp))
                Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                    Box(
                        Modifier.weight(1f).height(6.dp)
                            .background(Color.White.copy(.3f), RoundedCornerShape(3.dp))
                    ) {
                        Box(Modifier.fillMaxWidth(progress).fillMaxHeight().background(Color.White, RoundedCornerShape(3.dp)))
                    }
                    Spacer(Modifier.width(10.dp))
                    Text("${data.header.progressPercentage}% Complete", fontSize = 11.sp, color = Color.White.copy(.85f), fontWeight = FontWeight.SemiBold)
                }
            }
        }

        Spacer(Modifier.height(16.dp))

        // ── Main card ─────────────────────────────────────────────────────
        Card(
            Modifier.fillMaxWidth().padding(horizontal = 16.dp),
            shape     = RoundedCornerShape(20.dp),
            colors    = CardDefaults.cardColors(Color.White),
            elevation = CardDefaults.cardElevation(3.dp)
        ) {
            Column(Modifier.padding(20.dp)) {

                // Concept section
                Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.Top) {
                    Column(Modifier.weight(1f).padding(end = 12.dp)) {
                        Text(data.concept.titleBn.ifBlank { data.concept.titleEn }, fontSize = 17.sp, fontWeight = FontWeight.ExtraBold, color = Color(0xFF1A1A2E))
                        Spacer(Modifier.height(4.dp))
                        Text(data.concept.descriptionBn.ifBlank { data.concept.descriptionEn }, fontSize = 13.sp, color = Color(0xFF475569), lineHeight = 19.sp)
                    }
                    Box(Modifier.size(40.dp).background(Color(0xFFDCFCE7), CircleShape), Alignment.Center) {
                        Text("💡", fontSize = 20.sp)
                    }
                }

                Spacer(Modifier.height(14.dp))

                // Concept example box
                ConceptExampleBox(data.concept.example, onPlayAudio)

                Spacer(Modifier.height(20.dp))

                // "Tanween Types" divider
                SectionDivider("Tanween Types")

                Spacer(Modifier.height(14.dp))

                // 2×N grid of type cells — tap any cell in a row to select it
                Column(verticalArrangement = Arrangement.spacedBy(10.dp)) {
                    data.tanweenTypes.forEachIndexed { rowIdx, t ->
                        val isRowSelected = selectedRow == rowIdx
                        Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(10.dp)) {
                            TypeCell(
                                arabic = t.baseLetter.arabic, transliteration = t.baseLetter.transliteration,
                                vowelType = t.baseLetter.vowelType, audioUrl = t.baseLetter.audioUrl,
                                isSelected = isRowSelected,
                                onSelect = { onRowSelect(rowIdx) },
                                onPlayAudio = onPlayAudio, modifier = Modifier.weight(1f)
                            )
                            TypeCell(
                                arabic = t.targetLetter.arabic, transliteration = t.targetLetter.transliteration,
                                vowelType = t.targetLetter.vowelType, audioUrl = t.targetLetter.audioUrl,
                                isSelected = isRowSelected,
                                onSelect = { onRowSelect(rowIdx) },
                                onPlayAudio = onPlayAudio, modifier = Modifier.weight(1f)
                            )
                        }
                    }
                }

                Spacer(Modifier.height(20.dp))
                HorizontalDivider(color = Color(0xFFEEEEEE))
                Spacer(Modifier.height(16.dp))

                // Lesson Practice section
                Text("Lesson Practice", fontSize = 15.sp, fontWeight = FontWeight.ExtraBold, color = Color(0xFF1A1A2E))
                Text("Overview of lesson practice", fontSize = 12.sp, color = Color(0xFF64748B))

                Spacer(Modifier.height(14.dp))

                // Start Pronunciation Practice (filled)
                Box(
                    Modifier
                        .fillMaxWidth()
                        .clip(RoundedCornerShape(28.dp))
                        .background(AccentGreen)
                        .clickable { onStartPractice(selectedRow) }
                        .padding(vertical = 14.dp),
                    Alignment.Center
                ) {
                    Text("Start Pronunciation Practice", fontSize = 14.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
                }

                Spacer(Modifier.height(10.dp))

                // Start Listening Test (outline)
                Box(
                    Modifier
                        .fillMaxWidth()
                        .clip(RoundedCornerShape(28.dp))
                        .border(2.dp, AccentGreen, RoundedCornerShape(28.dp))
                        .clickable { /* TODO: Listening test */ }
                        .padding(vertical = 13.dp),
                    Alignment.Center
                ) {
                    Text("Start Listening Test", fontSize = 14.sp, fontWeight = FontWeight.ExtraBold, color = AccentGreen)
                }
            }
        }

        Spacer(Modifier.height(16.dp))

        // ── Listen & Compare ──────────────────────────────────────────────
        if (data.listenAndCompare.isNotEmpty()) {
            Card(
                Modifier.fillMaxWidth().padding(horizontal = 16.dp),
                shape     = RoundedCornerShape(16.dp),
                colors    = CardDefaults.cardColors(Color.White),
                elevation = CardDefaults.cardElevation(2.dp)
            ) {
                Column(Modifier.padding(16.dp)) {
                    Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(10.dp)) {
                        Box(Modifier.size(34.dp).background(AccentGreen.copy(.1f), RoundedCornerShape(8.dp)), Alignment.Center) {
                            Text("🎧", fontSize = 16.sp)
                        }
                        Column {
                            Text("Listen & Compare", fontSize = 14.sp, fontWeight = FontWeight.ExtraBold, color = Color(0xFF1A1A2E))
                            Text("Listen to the difference", fontSize = 11.sp, color = Color(0xFF64748B))
                        }
                    }
                    Spacer(Modifier.height(12.dp))
                    Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(8.dp)) {
                        data.listenAndCompare.forEach { item ->
                            ListenChipCompact(item, onPlayAudio, Modifier.weight(1f))
                        }
                    }
                }
            }
            Spacer(Modifier.height(16.dp))
        }

        // ── Previous / Next ────────────────────────────────────────────────
        Row(
            Modifier.fillMaxWidth().padding(horizontal = 16.dp),
            Arrangement.SpaceBetween, Alignment.CenterVertically
        ) {
            LessonNavBtn("← Previous", isFirst, onPrev)
            LessonNavBtn("Next →", isLast, onNext)
        }

        Spacer(Modifier.height(20.dp))
    }
}

@Composable
private fun ConceptExampleBox(example: ConceptExample, onPlayAudio: (String) -> Unit) {
    Box(
        Modifier.fillMaxWidth()
            .background(Color(0xFFFEF9EC), RoundedCornerShape(16.dp))
            .border(1.dp, Color(0xFFFDE68A), RoundedCornerShape(16.dp))
            .padding(horizontal = 20.dp, vertical = 16.dp)
    ) {
        Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
            Column(horizontalAlignment = Alignment.CenterHorizontally) {
                Text(example.baseText, fontSize = 28.sp, fontWeight = FontWeight.ExtraBold, color = AccentDark)
            }
            Text("→", fontSize = 22.sp, color = Color(0xFF94A3B8), fontWeight = FontWeight.Bold)
            Column(horizontalAlignment = Alignment.CenterHorizontally) {
                Text(example.resultText, fontSize = 28.sp, fontWeight = FontWeight.ExtraBold, color = TLPurple)
                Text(example.resultType, fontSize = 11.sp, color = TLPurple, fontWeight = FontWeight.SemiBold)
            }
            Box(
                Modifier.size(42.dp)
                    .background(AccentGreen.copy(.12f), CircleShape)
                    .border(1.dp, AccentGreen.copy(.3f), CircleShape)
                    .clickable { onPlayAudio(example.audioUrl) },
                Alignment.Center
            ) { Text("🎙️", fontSize = 19.sp) }
        }
    }
}

@Composable
private fun TypeCell(
    arabic: String, transliteration: String, vowelType: String,
    audioUrl: String, isSelected: Boolean, onSelect: () -> Unit,
    onPlayAudio: (String) -> Unit, modifier: Modifier,
) {
    val style = vowelStyle(vowelType)
    Box(
        modifier
            .background(
                if (isSelected) style.fg.copy(.14f) else style.bg,
                RoundedCornerShape(16.dp)
            )
            .border(
                width = if (isSelected) 2.5.dp else 1.dp,
                color = if (isSelected) style.fg else style.fg.copy(.25f),
                shape = RoundedCornerShape(16.dp)
            )
            .clickable { onSelect() }
            .padding(12.dp)
    ) {
        Column {
            // Icon + label row
            Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(5.dp)) {
                    Box(
                        Modifier.size(20.dp).background(style.fg.copy(.15f), CircleShape),
                        Alignment.Center
                    ) { Text(style.icon, fontSize = 10.sp, color = style.fg) }
                    Text(vowelType, fontSize = 10.sp, fontWeight = FontWeight.Bold, color = style.fg)
                }
                Box(
                    Modifier.size(26.dp).background(style.fg, CircleShape).clickable { onPlayAudio(audioUrl) },
                    Alignment.Center
                ) { Text("▶", fontSize = 9.sp, color = Color.White) }
            }
            Spacer(Modifier.height(8.dp))
            // Large Arabic
            Box(Modifier.fillMaxWidth(), contentAlignment = Alignment.Center) {
                Text(arabic, fontSize = 40.sp, fontWeight = FontWeight.Bold, color = style.fg)
            }
            // Transliteration
            Box(Modifier.fillMaxWidth(), contentAlignment = Alignment.Center) {
                Text(transliteration, fontSize = 16.sp, fontWeight = FontWeight.ExtraBold, color = style.fg)
            }
            // Vowel name
            Box(Modifier.fillMaxWidth(), contentAlignment = Alignment.Center) {
                Text("($vowelType)", fontSize = 10.sp, color = style.fg.copy(.6f))
            }
        }
    }
}

@Composable
private fun ListenChipCompact(item: ListenCompare, onPlayAudio: (String) -> Unit, modifier: Modifier) {
    Box(
        modifier
            .border(1.5.dp, AccentGreen.copy(.5f), RoundedCornerShape(24.dp))
            .clickable { onPlayAudio(item.audioUrl) }
            .padding(horizontal = 8.dp, vertical = 8.dp),
        Alignment.Center
    ) {
        Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(5.dp)) {
            Text(item.label, fontSize = 11.sp, fontWeight = FontWeight.SemiBold, color = AccentGreen)
            Box(Modifier.size(18.dp).background(AccentGreen, CircleShape), Alignment.Center) {
                Text("▶", fontSize = 7.sp, color = Color.White)
            }
        }
    }
}

@Composable
private fun LessonNavBtn(label: String, disabled: Boolean, onClick: () -> Unit) {
    val fg = if (disabled) Color(0xFFCBD5E1) else Color(0xFF1A1A2E)
    val border = if (disabled) Color(0xFFE2E8F0) else Color(0xFFCBD5E1)
    Box(
        Modifier
            .border(1.5.dp, border, RoundedCornerShape(24.dp))
            .clickable(enabled = !disabled) { onClick() }
            .padding(horizontal = 20.dp, vertical = 12.dp)
    ) {
        Text(label, fontSize = 13.sp, fontWeight = FontWeight.SemiBold, color = fg)
    }
}

@Composable
private fun SectionDivider(label: String) {
    Row(Modifier.fillMaxWidth(), verticalAlignment = Alignment.CenterVertically) {
        HorizontalDivider(Modifier.weight(1f), color = Color(0xFFDDDDDD))
        Text("   $label   ", fontSize = 13.sp, fontWeight = FontWeight.ExtraBold, color = Color(0xFF555555))
        HorizontalDivider(Modifier.weight(1f), color = Color(0xFFDDDDDD))
    }
}

// ─────────────────────────────────────────────────────────────────────────────
//  Practice session screen  (right design)
// ─────────────────────────────────────────────────────────────────────────────

@Composable
private fun PracticeSessionScreen(
    data:         TanweenLessonData,
    currentLesson:Int,
    practiceStep: Int,
    speakState:   SpeakState,
    onBack:       () -> Unit,
    onStepSelect: (Int) -> Unit,
    onPlayAudio:  (String) -> Unit,
    onStart:      () -> Unit,
    onStop:       () -> Unit,
    onReset:      () -> Unit,
    onPrev:       () -> Unit,
    onNext:       () -> Unit,
    isFirst:      Boolean,
    isLast:       Boolean,
) {
    val types = data.tanweenTypes
    val current = types.getOrNull(practiceStep)
    val target  = current?.targetLetter
    val style   = target?.let { vowelStyle(it.vowelType) }
        ?: VowelStyle(AccentGreen, Color(0xFFDCFCE7), "◌")

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
            Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(12.dp)) {
                Box(
                    Modifier.size(38.dp)
                        .border(1.dp, Color(0xFFE2E8F0), CircleShape)
                        .clickable { onBack() },
                    Alignment.Center
                ) { Text("←", fontSize = 16.sp, color = Color(0xFF1A1A2E), fontWeight = FontWeight.Bold) }
                Column {
                    Text("Practice Session", fontSize = 17.sp, fontWeight = FontWeight.ExtraBold, color = Color(0xFF1A1A2E))
                    Text("Lesson $currentLesson", fontSize = 11.sp, color = Color(0xFF64748B))
                }
            }
            Text("🏆", fontSize = 22.sp)
        }

        // ── Step dots ─────────────────────────────────────────────────────
        Row(Modifier.fillMaxWidth(), Arrangement.Center, Alignment.CenterVertically) {
            repeat(types.size) { i ->
                val active = i == practiceStep
                Box(
                    Modifier
                        .padding(horizontal = 4.dp)
                        .size(if (active) 13.dp else 10.dp)
                        .then(
                            if (active)
                                Modifier.background(AccentGreen, CircleShape)
                            else
                                Modifier.border(1.5.dp, Color(0xFFCBD5E1), CircleShape)
                                    .background(Color.White, CircleShape)
                        )
                        .clickable { onStepSelect(i) }
                )
            }
        }

        Spacer(Modifier.height(16.dp))

        // ── Pronounce card ────────────────────────────────────────────────
        Card(
            Modifier.fillMaxWidth().padding(horizontal = 16.dp),
            shape     = RoundedCornerShape(20.dp),
            colors    = CardDefaults.cardColors(Color.White),
            elevation = CardDefaults.cardElevation(3.dp)
        ) {
            Column(
                Modifier.padding(20.dp),
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.spacedBy(16.dp)
            ) {
                Text(
                    "Pronounce the word:",
                    fontSize = 15.sp, fontWeight = FontWeight.Bold,
                    color = Color(0xFF1A1A2E), modifier = Modifier.fillMaxWidth()
                )

                // Letter display box
                Box(
                    Modifier.fillMaxWidth()
                        .background(Color(0xFFF4F6F8), RoundedCornerShape(16.dp))
                        .border(1.dp, Color(0xFFE2E8F0), RoundedCornerShape(16.dp))
                        .padding(20.dp)
                ) {
                    Column(Modifier.fillMaxWidth(), horizontalAlignment = Alignment.CenterHorizontally) {
                        // Audio button top-right
                        Box(Modifier.fillMaxWidth(), contentAlignment = Alignment.TopEnd) {
                            Box(
                                Modifier.size(30.dp)
                                    .background(style.fg.copy(.15f), CircleShape)
                                    .clickable { target?.let { onPlayAudio(it.audioUrl) } },
                                Alignment.Center
                            ) { Text("🔊", fontSize = 13.sp) }
                        }
                        Spacer(Modifier.height(4.dp))
                        // Large Arabic
                        Text(
                            target?.arabic ?: "—",
                            fontSize = 72.sp, fontWeight = FontWeight.Bold,
                            color = style.fg, textAlign = TextAlign.Center
                        )
                        Text("◆", fontSize = 14.sp, color = style.fg.copy(.4f))
                        Spacer(Modifier.height(8.dp))
                        Text("Expected Word", fontSize = 11.sp, color = Color(0xFF94A3B8), fontWeight = FontWeight.SemiBold)
                        Spacer(Modifier.height(4.dp))
                        Text(
                            target?.transliteration ?: "—",
                            fontSize = 32.sp, fontWeight = FontWeight.ExtraBold,
                            color = AccentGreen
                        )
                    }
                }

                // Mic states
                when (speakState) {
                    is SpeakState.Idle       -> PracticeMic(onStart)
                    is SpeakState.Recording  -> PracticeMicRecording(onStop)
                    is SpeakState.Processing -> PracticeMicProcessing()
                    else                     -> {}
                }
            }
        }

        Spacer(Modifier.height(14.dp))

        // ── Practice result ───────────────────────────────────────────────
        when (speakState) {
            is SpeakState.Result -> PracticeResult(
                result = speakState.data,
                target = target?.transliteration ?: "—",
                onReset = onReset,
                onNextStep = {
                    if (practiceStep < types.size - 1) onStepSelect(practiceStep + 1)
                },
                hasNextStep = practiceStep < types.size - 1,
            )
            is SpeakState.Error -> PracticeError(speakState.message, onReset)
            else -> {}
        }

        if (speakState is SpeakState.Result || speakState is SpeakState.Error)
            Spacer(Modifier.height(14.dp))

        // ── Bottom nav ─────────────────────────────────────────────────────
        Row(
            Modifier.fillMaxWidth().padding(horizontal = 16.dp),
            Arrangement.SpaceBetween, Alignment.CenterVertically
        ) {
            LessonNavBtn("← Previous", isFirst, onPrev)
            Box(
                Modifier.background(Color(0xFFF1F5F9), RoundedCornerShape(20.dp))
                    .padding(horizontal = 20.dp, vertical = 10.dp)
            ) {
                Text(
                    "Lesson $currentLesson / ${data.header.totalLessons}",
                    fontSize = 12.sp, fontWeight = FontWeight.ExtraBold, color = Color(0xFF1A1A2E)
                )
            }
            Box(
                Modifier.clip(RoundedCornerShape(24.dp))
                    .background(AccentGreen)
                    .clickable(enabled = !isLast) { onNext() }
                    .padding(horizontal = 20.dp, vertical = 12.dp)
            ) {
                Text(
                    if (isLast) "Done" else "Next →",
                    fontSize = 13.sp, fontWeight = FontWeight.ExtraBold, color = Color.White
                )
            }
        }

        Spacer(Modifier.height(20.dp))
    }
}

// ── Mic composables ────────────────────────────────────────────────────────────

@Composable
private fun PracticeMic(onStart: () -> Unit) {
    val haptic = LocalHapticFeedback.current
    Column(Modifier.fillMaxWidth(), horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(8.dp)) {
        Box(
            Modifier.size(72.dp).clip(CircleShape).background(AccentGreen)
                .clickable { haptic.performHapticFeedback(HapticFeedbackType.LongPress); onStart() },
            Alignment.Center
        ) { Text("🎙️", fontSize = 28.sp) }
        Text("Tap to record", fontSize = 12.sp, color = AccentGreen, fontWeight = FontWeight.SemiBold)
    }
}

@Composable
private fun PracticeMicRecording(onStop: () -> Unit) {
    val tr = rememberInfiniteTransition(label = "p")
    val sc by tr.animateFloat(1f, 1.22f, label = "s", animationSpec = infiniteRepeatable(tween(700), RepeatMode.Reverse))
    val al by tr.animateFloat(.3f, 1f, label = "a", animationSpec = infiniteRepeatable(tween(700), RepeatMode.Reverse))
    Column(Modifier.fillMaxWidth(), horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(8.dp)) {
        Box(contentAlignment = Alignment.Center) {
            Box(Modifier.size(84.dp).scale(sc).background(Color(0xFFEF4444).copy(al * .25f), CircleShape))
            Box(Modifier.size(64.dp).background(Color(0xFFEF4444), CircleShape).clickable { onStop() }, Alignment.Center) {
                Text("⏹️", fontSize = 24.sp)
            }
        }
        Text("Recording… tap to stop", fontSize = 12.sp, fontWeight = FontWeight.Bold, color = Color(0xFFEF4444))
    }
}

@Composable
private fun PracticeMicProcessing() {
    Column(Modifier.fillMaxWidth().padding(vertical = 8.dp), horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(8.dp)) {
        CircularProgressIndicator(color = AccentGreen, strokeWidth = 3.dp, modifier = Modifier.size(48.dp))
        Text("Analysing…", fontSize = 13.sp, fontWeight = FontWeight.Bold, color = AccentDark)
    }
}

// ── Practice result ────────────────────────────────────────────────────────────

@Composable
private fun PracticeResult(
    result:      PronunciationResult,
    target:      String,
    onReset:     () -> Unit,
    onNextStep:  () -> Unit,
    hasNextStep: Boolean,
) {
    val isCorrect = result.isCorrect
    val statusClr = if (isCorrect) AccentGreen else Color(0xFFDC2626)
    val accuracy  = (result.accuracyScore / 100.0).coerceIn(0.0, 1.0).toFloat()

    Card(
        Modifier.fillMaxWidth().padding(horizontal = 16.dp),
        shape     = RoundedCornerShape(20.dp),
        colors    = CardDefaults.cardColors(Color.White),
        elevation = CardDefaults.cardElevation(3.dp)
    ) {
        Column(Modifier.padding(20.dp), verticalArrangement = Arrangement.spacedBy(14.dp)) {

            Text("Your Practice Result", fontSize = 15.sp, fontWeight = FontWeight.ExtraBold, color = Color(0xFF1A1A2E))

            // Two-column result layout (matches right screenshot)
            Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(12.dp)) {
                // Left: correct/incorrect + target + you said
                Column(
                    Modifier.weight(1f)
                        .background(Color(0xFFF8FAFC), RoundedCornerShape(14.dp))
                        .border(1.dp, Color(0xFFE2E8F0), RoundedCornerShape(14.dp))
                        .padding(12.dp),
                    verticalArrangement = Arrangement.spacedBy(8.dp)
                ) {
                    Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(6.dp)) {
                        Box(Modifier.size(28.dp).background(statusClr, CircleShape), Alignment.Center) {
                            Text(if (isCorrect) "✓" else "✗", fontSize = 13.sp, color = Color.White, fontWeight = FontWeight.ExtraBold)
                        }
                        Text(if (isCorrect) "Correct\nPronunciation" else "Try\nAgain", fontSize = 11.sp, fontWeight = FontWeight.Bold, color = statusClr, lineHeight = 15.sp)
                    }
                    HorizontalDivider(color = Color(0xFFE2E8F0))
                    Column(verticalArrangement = Arrangement.spacedBy(4.dp)) {
                        Text("Target:", fontSize = 10.sp, color = Color(0xFF94A3B8))
                        Text(target, fontSize = 18.sp, fontWeight = FontWeight.ExtraBold, color = AccentGreen)
                    }
                    Column(verticalArrangement = Arrangement.spacedBy(4.dp)) {
                        Text("You Said:", fontSize = 10.sp, color = Color(0xFF94A3B8))
                        Text(result.recognizedText.ifBlank { "—" }, fontSize = 18.sp, fontWeight = FontWeight.ExtraBold, color = statusClr)
                    }
                }

                // Right: accuracy ring
                Column(
                    Modifier.weight(1f)
                        .background(Color(0xFFF8FAFC), RoundedCornerShape(14.dp))
                        .border(1.dp, Color(0xFFE2E8F0), RoundedCornerShape(14.dp))
                        .padding(12.dp),
                    horizontalAlignment = Alignment.CenterHorizontally,
                    verticalArrangement = Arrangement.spacedBy(8.dp)
                ) {
                    Text("Pronunciation\nAccuracy", fontSize = 11.sp, fontWeight = FontWeight.Bold, color = Color(0xFF1A1A2E), textAlign = TextAlign.Center, lineHeight = 16.sp)
                    AccuracyRing(accuracy, statusClr)
                    Text("Complete", fontSize = 10.sp, color = Color(0xFF94A3B8))
                    Text(if (isCorrect) "Excellent!" else "Keep trying", fontSize = 13.sp, fontWeight = FontWeight.ExtraBold, color = statusClr)
                }
            }

            // AI Tip (lavender/purple bg)
            if (result.makhrajHint.isNotBlank()) {
                Row(
                    Modifier.fillMaxWidth()
                        .background(Color(0xFFF3E5F5), RoundedCornerShape(12.dp))
                        .border(1.dp, Color(0xFFCE93D8), RoundedCornerShape(12.dp))
                        .padding(12.dp),
                    horizontalArrangement = Arrangement.spacedBy(8.dp),
                    verticalAlignment     = Alignment.Top
                ) {
                    Text("💡", fontSize = 14.sp)
                    Column {
                        Text("AI Tip:", fontSize = 11.sp, fontWeight = FontWeight.ExtraBold, color = TLPurple)
                        Text(result.makhrajHint, fontSize = 11.sp, color = Color(0xFF4A148C), lineHeight = 16.sp)
                    }
                }
            }

            // Action buttons
            Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(10.dp)) {
                Box(
                    Modifier.weight(1f).clip(RoundedCornerShape(14.dp))
                        .border(1.5.dp, AccentGreen, RoundedCornerShape(14.dp))
                        .clickable { onReset() }
                        .padding(vertical = 12.dp),
                    Alignment.Center
                ) { Text("🔄 Retry", fontSize = 13.sp, fontWeight = FontWeight.Bold, color = AccentGreen) }

                if (hasNextStep) {
                    Box(
                        Modifier.weight(1f).clip(RoundedCornerShape(14.dp))
                            .background(AccentGreen)
                            .clickable { onNextStep() }
                            .padding(vertical = 12.dp),
                        Alignment.Center
                    ) { Text("Next →", fontSize = 13.sp, fontWeight = FontWeight.Bold, color = Color.White) }
                }
            }
        }
    }
}

@Composable
private fun PracticeError(message: String, onReset: () -> Unit) {
    Column(
        Modifier.fillMaxWidth().padding(horizontal = 16.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(10.dp)
    ) {
        Text("⚠️  $message", fontSize = 12.sp, color = Color(0xFFDC2626), textAlign = TextAlign.Center)
        Box(
            Modifier.fillMaxWidth().clip(RoundedCornerShape(14.dp))
                .background(AccentGreen).clickable { onReset() }.padding(vertical = 12.dp),
            Alignment.Center
        ) { Text("🔄 Try Again", fontSize = 13.sp, fontWeight = FontWeight.Bold, color = Color.White) }
    }
}

// ── Error state ────────────────────────────────────────────────────────────────

@Composable
private fun ErrorState(message: String, onRetry: () -> Unit) {
    Column(
        Modifier.fillMaxSize(),
        Arrangement.Center, Alignment.CenterHorizontally
    ) {
        Text("📡", fontSize = 48.sp)
        Spacer(Modifier.height(12.dp))
        Text(message, fontSize = 14.sp, color = Color(0xFF475569), textAlign = TextAlign.Center, modifier = Modifier.padding(horizontal = 40.dp))
        Spacer(Modifier.height(16.dp))
        Button(onClick = onRetry, colors = ButtonDefaults.buttonColors(AccentGreen)) {
            Text("Retry", color = Color.White, fontWeight = FontWeight.Bold)
        }
    }
}

// ── Accuracy ring ──────────────────────────────────────────────────────────────

@Composable
private fun AccuracyRing(accuracy: Float, color: Color) {
    Box(Modifier.size(80.dp), Alignment.Center) {
        Canvas(Modifier.fillMaxSize()) {
            val sw  = 8.dp.toPx()
            val in_ = sw / 2f
            val arc = Size(this.size.width - sw, this.size.height - sw)
            val tl  = Offset(in_, in_)
            drawArc(Color(0xFFE2E8F0), -90f, 360f, false, tl, arc, style = Stroke(sw, cap = StrokeCap.Round))
            if (accuracy > 0f)
                drawArc(color, -90f, 360f * accuracy, false, tl, arc, style = Stroke(sw, cap = StrokeCap.Round))
        }
        Column(horizontalAlignment = Alignment.CenterHorizontally) {
            Text("${(accuracy * 100).toInt()}%", fontSize = 16.sp, fontWeight = FontWeight.ExtraBold, color = color)
        }
    }
}
