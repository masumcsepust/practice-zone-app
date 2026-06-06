package com.example.quran_app.ui.screens.recite

import androidx.compose.animation.core.*
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowBackIosNew
import androidx.compose.material.icons.filled.Mic
import androidx.compose.material.icons.filled.Refresh
import androidx.compose.material.icons.filled.Stop
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.runtime.snapshots.SnapshotStateList
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontStyle
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.text.style.TextDirection
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.quran_app.data.local.SURAHS
import com.example.quran_app.domain.model.SurahDef
import com.example.quran_app.ui.theme.Rc
import com.example.quran_app.ui.theme.RcType
import com.example.quran_app.ui.viewmodel.ReciteUiState
import com.example.quran_app.ui.viewmodel.ReciteViewModel
import kotlinx.coroutines.delay
import kotlin.random.Random
import androidx.compose.foundation.Canvas
import androidx.compose.foundation.BorderStroke
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.geometry.Size
import androidx.compose.ui.graphics.StrokeCap
import androidx.compose.ui.graphics.drawscope.Stroke
import androidx.compose.material.icons.filled.PlayArrow
import androidx.compose.ui.text.font.FontWeight
import com.example.quran_app.domain.model.Note
import com.example.quran_app.domain.model.NoteKind
import com.example.quran_app.domain.model.RecitationResult

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun ReciteTabContent(viewModel: ReciteViewModel) {
    val state      by viewModel.state.collectAsState()
    val showPicker by viewModel.showSurahPicker.collectAsState()
    val surahList  by viewModel.surahList.collectAsState()

    when (val s = state) {
        is ReciteUiState.ShowFeedback ->
            FeedbackScreen(s.surah, s.ayahRange, s.result, viewModel)
        else ->
            ReciteScreen(state, viewModel)
    }

    if (showPicker) {
        SurahPickerSheet(
            surahList = surahList,
            onSelect  = viewModel::selectSurah,
            onDismiss = viewModel::closePicker
        )
    }
}

// ── Recite screen ─────────────────────────────────────────────────────────────
@Composable
private fun ReciteScreen(state: ReciteUiState, vm: ReciteViewModel) {
    val isRecording    = state is ReciteUiState.Recording
    val isProcessing   = state is ReciteUiState.Processing
    val isLoadingAyahs = state is ReciteUiState.LoadingAyahs

    val surah = when (state) {
        is ReciteUiState.Idle         -> state.surah
        is ReciteUiState.Recording    -> state.surah
        is ReciteUiState.Processing   -> state.surah
        is ReciteUiState.LoadingAyahs -> state.surah
        else                          -> SURAHS[0]
    }
    val ayahRange = when (state) {
        is ReciteUiState.Idle         -> state.ayahRange
        is ReciteUiState.Recording    -> state.ayahRange
        is ReciteUiState.Processing   -> state.ayahRange
        is ReciteUiState.LoadingAyahs -> 1..1
        else                          -> 1..3
    }
    val activeAyah = when (state) {
        is ReciteUiState.Idle      -> state.activeAyah
        is ReciteUiState.Recording -> state.activeAyah
        else                       -> ayahRange.first
    }

    // Waveform heights
    val waveHeights = remember { mutableStateListOf<Float>().also { l -> repeat(24) { l.add(.25f) } } }
    LaunchedEffect(isRecording) {
        if (isRecording) {
            while (true) {
                delay(100)
                val fresh = List(24) { Random.nextFloat() * .8f + .2f }
                waveHeights.clear(); waveHeights.addAll(fresh)
            }
        } else {
            waveHeights.replaceAll { .25f }
        }
    }

    Box(
        Modifier
            .fillMaxSize()
            .background(Rc.Parchment)
    ) {
        Column(
            Modifier
                .fillMaxSize()
                .statusBarsPadding()
        ) {
            // Top bar
            ReciteTopBar(isRecording)

            Column(
                Modifier
                    .weight(1f)
                    .verticalScroll(rememberScrollState())
                    .padding(horizontal = 16.dp),
                verticalArrangement = Arrangement.spacedBy(14.dp)
            ) {
                Spacer(Modifier.height(4.dp))

                // Surah picker card
                SurahInfoCard(surah, ayahRange) { vm.openPicker() }

                // Verse display card
                val isPlayingAudio by vm.isPlayingAudio.collectAsState()
                val ayahAudioUrl = surah.ayat.find { it.number == activeAyah }?.audioUrl ?: ""
                VerseCard(
                    surah          = surah,
                    ayahRange      = ayahRange,
                    activeAyah     = activeAyah,
                    isPlayingAudio = isPlayingAudio,
                    onListenTap    = { vm.playAyahAudio(ayahAudioUrl, surah.id, activeAyah) },
                )

                // Waveform / processing / loading
                when {
                    isLoadingAyahs -> LoadingAyahsCard()
                    isProcessing   -> ProcessingCard()
                    isRecording    -> WaveformCard(waveHeights)
                    else           -> IdleHintCard()
                }

                Spacer(Modifier.height(8.dp))
            }

            // Controls
            ControlsBar(isRecording, isProcessing || isLoadingAyahs, vm)
        }
    }
}

// ── Top bar ───────────────────────────────────────────────────────────────────
@Composable
private fun ReciteTopBar(isRecording: Boolean) {
    Row(
        Modifier
            .fillMaxWidth()
            .padding(horizontal = 16.dp, vertical = 14.dp),
        Arrangement.SpaceBetween,
        Alignment.CenterVertically
    ) {
        Box(
            Modifier
                .size(36.dp)
                .background(Rc.Line, CircleShape),
            Alignment.Center
        ) {
            Icon(Icons.Default.ArrowBackIosNew, null,
                tint = Rc.Ink, modifier = Modifier.size(16.dp))
        }

        Text("Recite", style = RcType.Display, color = Rc.Ink)

        // Live pill
        val blink by rememberInfiniteTransition(label = "live").animateFloat(
            .4f, 1f, infiniteRepeatable(tween(600), RepeatMode.Reverse), "b"
        )
        Surface(
            shape = RoundedCornerShape(20.dp),
            color = if (isRecording) Rc.Gold.copy(.15f) else Rc.EmeraldSoft,
        ) {
            Row(
                Modifier.padding(horizontal = 12.dp, vertical = 6.dp),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(5.dp)
            ) {
                Box(
                    Modifier.size(7.dp).background(
                        if (isRecording) Rc.Gold.copy(blink) else Rc.Muted.copy(.5f),
                        CircleShape
                    )
                )
                Text(
                    if (isRecording) "Live" else "Ready",
                    style = RcType.LabelSm,
                    color = if (isRecording) Rc.Gold else Rc.Muted
                )
            }
        }
    }
}

// ── Surah info card ───────────────────────────────────────────────────────────
@Composable
private fun SurahInfoCard(surah: SurahDef, ayahRange: IntRange, onChangeTap: () -> Unit) {
    Row(
        Modifier
            .fillMaxWidth()
            .shadow(3.dp, RoundedCornerShape(16.dp))
            .clip(RoundedCornerShape(16.dp))
            .background(Rc.Cream)
            .border(1.dp, Rc.Line, RoundedCornerShape(16.dp))
            .padding(14.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(12.dp)
    ) {
        // Arabic glyph tile
        Box(
            Modifier
                .size(50.dp)
                .background(
                    Brush.linearGradient(listOf(Rc.EmeraldDeep, Rc.Emerald)),
                    RoundedCornerShape(14.dp)
                ),
            Alignment.Center
        ) {
            Text(surah.arabicName.take(2), style = RcType.Arabic.copy(fontSize = 18.sp, lineHeight = 24.sp),
                color = Rc.GoldBright, textAlign = TextAlign.Center)
        }

        Column(Modifier.weight(1f)) {
            Text(
                "Surah ${surah.id}",
                style = RcType.LabelSm,
                color = Rc.Gold
            )
            Text(surah.transliteratedName, style = RcType.BodyBold, color = Rc.Ink)
            Text(
                "${surah.englishName}  ·  Āyāt ${ayahRange.first}–${ayahRange.last}",
                style = RcType.Label,
                color = Rc.Muted
            )
        }

        Surface(
            onClick = onChangeTap,
            shape = RoundedCornerShape(20.dp),
            color = Rc.GoldSoft
        ) {
            Text("Change",
                modifier = Modifier.padding(horizontal = 12.dp, vertical = 6.dp),
                style = RcType.BodyBold, color = Rc.Gold)
        }
    }
}

// ── Verse display card ────────────────────────────────────────────────────────
@Composable
private fun VerseCard(
    surah:          SurahDef,
    ayahRange:      IntRange,
    activeAyah:     Int,
    isPlayingAudio: Boolean   = false,
    onListenTap:    () -> Unit = {},
) {
    val ayat = surah.ayat.filter { it.number in ayahRange }
    val currentAyah = ayat.find { it.number == activeAyah } ?: ayat.firstOrNull()

    Column(
        Modifier
            .fillMaxWidth()
            .shadow(4.dp, RoundedCornerShape(20.dp))
            .clip(RoundedCornerShape(20.dp))
            .background(Rc.Cream)
            .border(1.dp, Rc.Line, RoundedCornerShape(20.dp))
            .padding(horizontal = 20.dp, vertical = 24.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(10.dp)
    ) {
        // Progress dashes
        Row(horizontalArrangement = Arrangement.spacedBy(8.dp),
            verticalAlignment = Alignment.CenterVertically) {
            ayat.forEach { ayah ->
                val done = ayah.number < activeAyah
                val active = ayah.number == activeAyah
                Box(
                    Modifier
                        .height(3.dp)
                        .width(if (active) 28.dp else 16.dp)
                        .background(
                            when { done -> Rc.Emerald; active -> Rc.Gold; else -> Rc.Line },
                            RoundedCornerShape(2.dp)
                        )
                )
            }
        }

        Spacer(Modifier.height(8.dp))

        // Arabic text (RTL, large serif)
        if (currentAyah != null) {
            Text(
                currentAyah.arabic,
                style = RcType.ArabicLarge.copy(
                    textDirection = TextDirection.Rtl,
                    color = Rc.Ink,
                ),
                textAlign = TextAlign.Center,
                modifier = Modifier.fillMaxWidth()
            )

            Spacer(Modifier.height(6.dp))

            // Transliteration hint
            Text(
                currentAyah.transliteration,
                style = RcType.Caption.copy(fontStyle = FontStyle.Italic),
                color = Rc.Muted,
                textAlign = TextAlign.Center
            )
        }

        Spacer(Modifier.height(8.dp))

        // Listen first button — shown only when ayah content is available
        if (currentAyah != null) {
            Row(
                Modifier
                    .fillMaxWidth()
                    .clip(RoundedCornerShape(10.dp))
                    .background(if (isPlayingAudio) Rc.Gold.copy(.12f) else Rc.GoldSoft)
                    .border(1.dp, Rc.Gold.copy(.35f), RoundedCornerShape(10.dp))
                    .clickable { onListenTap() }
                    .padding(horizontal = 14.dp, vertical = 10.dp),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.Center
            ) {
                if (isPlayingAudio) {
                    CircularProgressIndicator(
                        modifier    = Modifier.size(14.dp),
                        color       = Rc.Gold,
                        strokeWidth = 1.5.dp
                    )
                    Spacer(Modifier.width(8.dp))
                    Text("Playing… tap to stop", style = RcType.BodyBold, color = Rc.Gold)
                } else {
                    Icon(
                        Icons.Default.PlayArrow,
                        contentDescription = "Listen",
                        tint     = Rc.Gold,
                        modifier = Modifier.size(16.dp)
                    )
                    Spacer(Modifier.width(6.dp))
                    Text("Listen first", style = RcType.BodyBold, color = Rc.Gold)
                }
            }
        }

        Spacer(Modifier.height(4.dp))

        // Ayah number badge
        Surface(shape = CircleShape, color = Rc.EmeraldSoft) {
            Text(
                "﴿${activeAyah}﴾",
                modifier = Modifier.padding(horizontal = 10.dp, vertical = 3.dp),
                style = RcType.LabelSm,
                color = Rc.Emerald
            )
        }
    }
}

// ── Idle hint ─────────────────────────────────────────────────────────────────
@Composable
private fun IdleHintCard() {
    Row(
        Modifier
            .fillMaxWidth()
            .clip(RoundedCornerShape(14.dp))
            .background(Rc.EmeraldSoft)
            .border(1.dp, Rc.Emerald.copy(.2f), RoundedCornerShape(14.dp))
            .padding(horizontal = 16.dp, vertical = 12.dp),
        horizontalArrangement = Arrangement.spacedBy(10.dp),
        verticalAlignment = Alignment.CenterVertically
    ) {
        Text("🎙️", fontSize = 20.sp)
        Text(
            "Tap the mic to begin reciting aloud — the AI follows word by word.",
            style = RcType.Body,
            color = Rc.EmeraldDeep,
            lineHeight = 20.sp
        )
    }
}

// ── Waveform card ─────────────────────────────────────────────────────────────
@Composable
private fun WaveformCard(waveHeights: SnapshotStateList<Float>) {
    val blink by rememberInfiniteTransition(label = "blink").animateFloat(
        .35f, 1f, infiniteRepeatable(tween(480), RepeatMode.Reverse), "b"
    )
    Column(
        Modifier
            .fillMaxWidth()
            .clip(RoundedCornerShape(16.dp))
            .background(Rc.Ink)
            .padding(horizontal = 16.dp, vertical = 16.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(10.dp)
    ) {
        Row(verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(7.dp)) {
            Box(Modifier.size(8.dp).background(Rc.GoldBright.copy(blink), CircleShape))
            Text("Listening…",
                style = RcType.BodyBold.copy(letterSpacing = 0.8.sp),
                color = Rc.GoldBright.copy(blink))
        }

        Row(
            Modifier.fillMaxWidth().height(56.dp),
            Arrangement.Center,
            Alignment.CenterVertically
        ) {
            waveHeights.forEach { h ->
                Box(
                    Modifier.padding(horizontal = 1.5.dp).width(3.dp)
                        .fillMaxHeight(h.coerceIn(.08f, 1f))
                        .background(
                            Brush.verticalGradient(listOf(Rc.GoldBright, Rc.Gold.copy(.4f))),
                            RoundedCornerShape(2.dp)
                        )
                )
            }
        }
    }
}

// ── Loading ayahs card ────────────────────────────────────────────────────────
@Composable
private fun LoadingAyahsCard() {
    Row(
        Modifier
            .fillMaxWidth()
            .clip(RoundedCornerShape(14.dp))
            .background(Rc.EmeraldSoft)
            .border(1.dp, Rc.Emerald.copy(.2f), RoundedCornerShape(14.dp))
            .padding(horizontal = 16.dp, vertical = 14.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(12.dp)
    ) {
        CircularProgressIndicator(
            modifier = Modifier.size(20.dp),
            color    = Rc.Emerald,
            strokeWidth = 2.dp
        )
        Text("Loading āyāt…", style = RcType.Body, color = Rc.EmeraldDeep)
    }
}

// ── Processing card ───────────────────────────────────────────────────────────
@Composable
private fun ProcessingCard() {
    Row(
        Modifier
            .fillMaxWidth()
            .clip(RoundedCornerShape(14.dp))
            .background(Rc.GoldSoft)
            .padding(horizontal = 16.dp, vertical = 14.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(12.dp)
    ) {
        CircularProgressIndicator(
            modifier = Modifier.size(20.dp),
            color = Rc.Gold,
            strokeWidth = 2.dp
        )
        Text("Analysing your recitation…", style = RcType.Body, color = Rc.Gold)
    }
}

// ── Controls bar ──────────────────────────────────────────────────────────────
@Composable
private fun ControlsBar(isRecording: Boolean, isProcessing: Boolean, vm: ReciteViewModel) {
    val pulse = rememberInfiniteTransition(label = "pulse")
    val ring by pulse.animateFloat(0f, 1f,
        infiniteRepeatable(tween(900, easing = EaseInOut), RepeatMode.Reverse), "r")

    Surface(
        Modifier.fillMaxWidth(),
        color = Rc.Cream,
        shadowElevation = 12.dp,
        tonalElevation  = 0.dp
    ) {
        Column(
            Modifier.padding(start = 24.dp, top = 20.dp, end = 24.dp, bottom = 28.dp),
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(6.dp)
        ) {
            Row(
                Modifier.fillMaxWidth(),
                Arrangement.SpaceEvenly,
                Alignment.CenterVertically
            ) {
                // Restart
                Box(
                    Modifier
                        .size(52.dp)
                        .background(Rc.EmeraldSoft, CircleShape)
                        .border(1.dp, Rc.Emerald.copy(.2f), CircleShape)
                        .clickable(enabled = !isProcessing) { vm.restart() },
                    Alignment.Center
                ) {
                    Icon(Icons.Default.Refresh, null,
                        tint = Rc.Emerald, modifier = Modifier.size(22.dp))
                }

                // Central mic
                Box(contentAlignment = Alignment.Center) {
                    if (isRecording) {
                        Box(Modifier.size((84 + 22 * ring).dp)
                            .background(Rc.Gold.copy(.12f * (1f - ring)), CircleShape))
                        Box(Modifier.size((84 + 10 * ring).dp)
                            .background(Rc.Gold.copy(.08f * (1f - ring)), CircleShape))
                    }
                    Box(
                        Modifier
                            .size(80.dp)
                            .shadow(if (isRecording) 16.dp else 6.dp, CircleShape)
                            .background(
                                if (isRecording)
                                    Brush.radialGradient(listOf(Rc.GoldBright, Rc.Gold))
                                else
                                    Brush.radialGradient(listOf(Rc.Emerald, Rc.EmeraldDeep)),
                                CircleShape
                            )
                            .clickable(enabled = !isProcessing) { vm.toggleMic() },
                        Alignment.Center
                    ) {
                        Icon(
                            if (isRecording) Icons.Default.Stop else Icons.Default.Mic,
                            null, tint = Color.White,
                            modifier = Modifier.size(34.dp)
                        )
                    }
                }

                // Stop / done
                Box(
                    Modifier
                        .size(52.dp)
                        .background(Rc.GoldSoft, CircleShape)
                        .border(1.dp, Rc.Gold.copy(.3f), CircleShape)
                        .clickable(enabled = isRecording) {
                            if (isRecording) vm.toggleMic()
                        },
                    Alignment.Center
                ) {
                    Icon(Icons.Default.Stop, null,
                        tint = if (isRecording) Rc.Gold else Rc.Muted.copy(.4f),
                        modifier = Modifier.size(22.dp))
                }
            }

            Text(
                when {
                    isProcessing -> "Analysing…"
                    isRecording  -> "Tap stop or the mic to finish"
                    else         -> "Tap the mic to start recording"
                },
                style = RcType.Label,
                color = Rc.Muted,
                textAlign = TextAlign.Center
            )
        }
    }
}

// ── Surah picker bottom sheet ─────────────────────────────────────────────────
@OptIn(ExperimentalMaterial3Api::class)
@Composable
private fun SurahPickerSheet(
    surahList: List<SurahDef>,
    onSelect:  (SurahDef) -> Unit,
    onDismiss: () -> Unit,
) {
    ModalBottomSheet(
        onDismissRequest = onDismiss,
        containerColor   = Rc.Cream,
        tonalElevation   = 0.dp,
    ) {
        Column(
            Modifier.fillMaxWidth().padding(horizontal = 16.dp).padding(bottom = 24.dp),
            verticalArrangement = Arrangement.spacedBy(10.dp)
        ) {
            Text("Choose a Surah", style = RcType.Display, color = Rc.Ink,
                modifier = Modifier.padding(bottom = 4.dp))

            HorizontalDivider(color = Rc.Line)

            surahList.forEach { surah ->
                Row(
                    Modifier
                        .fillMaxWidth()
                        .clip(RoundedCornerShape(14.dp))
                        .background(Rc.Parchment)
                        .border(1.dp, Rc.Line, RoundedCornerShape(14.dp))
                        .clickable { onSelect(surah) }
                        .padding(14.dp),
                    verticalAlignment = Alignment.CenterVertically,
                    horizontalArrangement = Arrangement.spacedBy(12.dp)
                ) {
                    // Number badge
                    Box(
                        Modifier
                            .size(40.dp)
                            .background(
                                Brush.linearGradient(listOf(Rc.EmeraldDeep, Rc.Emerald)),
                                CircleShape
                            ),
                        Alignment.Center
                    ) {
                        Text("${surah.id}", style = RcType.BodyBold, color = Rc.GoldBright)
                    }
                    Column(Modifier.weight(1f)) {
                        Text(surah.transliteratedName, style = RcType.BodyBold, color = Rc.Ink)
                        Text("${surah.englishName} · ${surah.ayat.size} āyāt",
                            style = RcType.Label, color = Rc.Muted)
                    }
                    Text(surah.arabicName, style = RcType.Arabic.copy(fontSize = 18.sp, lineHeight = 24.sp),
                        color = Rc.Gold)
                }
            }
        }
    }
}

// ── Note kind → tajweed accent color ─────────────────────────────────────────
private fun NoteKind.accentColor(): Color = when (this) {
    NoteKind.QALQALAH -> Rc.TajQalqalah
    NoteKind.MADD     -> Rc.TajMadd
    NoteKind.GHUNNAH  -> Rc.TajGhunnah
    NoteKind.IDGHAAM  -> Rc.TajIdghaam
    NoteKind.IKHFAA   -> Rc.TajIkhfaa
    NoteKind.POSITIVE -> Rc.Emerald
}

// ─────────────────────────────────────────────────────────────────────────────
// FEEDBACK SCREEN
// ─────────────────────────────────────────────────────────────────────────────

@Composable
fun FeedbackScreen(
    surah:     SurahDef,
    ayahRange: IntRange,
    result:    RecitationResult,
    vm:        ReciteViewModel,
) {
    Box(
        Modifier
            .fillMaxSize()
            .background(Rc.Parchment)
    ) {
        Column(
            Modifier
                .fillMaxSize()
                .statusBarsPadding()
        ) {
            FeedbackHeader(surah, ayahRange, result, vm)

            Column(
                Modifier
                    .weight(1f)
                    .verticalScroll(rememberScrollState())
                    .padding(horizontal = 16.dp),
                verticalArrangement = Arrangement.spacedBy(14.dp)
            ) {
                Spacer(Modifier.height(4.dp))
                StatStrip(result)
                FeedbackNotesList(result.notes)
                Spacer(Modifier.height(8.dp))
            }

            FeedbackCtaBar(vm)
        }
    }
}

// ── Emerald header ────────────────────────────────────────────────────────────
@Composable
private fun FeedbackHeader(
    surah:     SurahDef,
    ayahRange: IntRange,
    result:    RecitationResult,
    vm:        ReciteViewModel,
) {
    val areasToPolish = result.notes.count { !it.isPositive }
    val headline = when {
        result.overallTajweed >= 90 -> "Mā shā' Allāh — excellent!"
        result.overallTajweed >= 80 -> "Mā shā' Allāh — well recited!"
        result.overallTajweed >= 70 -> "Jayyid — good effort!"
        else                        -> "Keep practicing — you're improving!"
    }
    val subline = buildString {
        append("${surah.transliteratedName} · āyāt ${ayahRange.first}–${ayahRange.last}")
        if (areasToPolish > 0) append(" · $areasToPolish ${if (areasToPolish == 1) "area" else "areas"} to polish")
        else append(" · no issues found")
    }

    Box(
        Modifier
            .fillMaxWidth()
            .background(Brush.verticalGradient(listOf(Rc.EmeraldDeep, Rc.Emerald)))
    ) {
        Canvas(Modifier.fillMaxWidth().height(260.dp)) {
            drawCircle(Color.White.copy(.05f), 180.dp.toPx(), Offset(size.width * 1.1f, 44.dp.toPx()))
            drawCircle(Color.White.copy(.04f), 100.dp.toPx(), Offset(-24.dp.toPx(), size.height * .72f))
            drawCircle(Color.White.copy(.03f), 260.dp.toPx(), Offset(size.width * .88f, size.height))
        }

        Column(
            Modifier
                .fillMaxWidth()
                .padding(horizontal = 16.dp)
                .padding(top = 14.dp, bottom = 28.dp),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            // Back chevron + title
            Row(
                Modifier.fillMaxWidth(),
                Arrangement.SpaceBetween,
                Alignment.CenterVertically
            ) {
                Box(
                    Modifier
                        .size(36.dp)
                        .background(Color.White.copy(.15f), CircleShape)
                        .clickable { vm.restart() },
                    Alignment.Center
                ) {
                    Icon(
                        Icons.Default.ArrowBackIosNew,
                        contentDescription = "Back",
                        tint = Color.White,
                        modifier = Modifier.size(16.dp)
                    )
                }
                Text("Your recitation", style = RcType.Display, color = Color.White)
                Spacer(Modifier.size(36.dp))
            }

            Spacer(Modifier.height(20.dp))

            ScoreRing(result.overallTajweed)

            Spacer(Modifier.height(14.dp))

            Text(
                headline,
                style = RcType.Display,
                color = Color.White,
                textAlign = TextAlign.Center
            )
            Spacer(Modifier.height(4.dp))
            Text(
                subline,
                style = RcType.Label,
                color = Color.White.copy(.75f),
                textAlign = TextAlign.Center
            )
        }
    }
}

// ── Score ring (gold arc on emerald) ─────────────────────────────────────────
@Composable
private fun ScoreRing(score: Int) {
    Box(Modifier.size(130.dp), Alignment.Center) {
        Canvas(Modifier.fillMaxSize()) {
            val sw      = 10.dp.toPx()
            val inset   = sw / 2f
            val topLeft = Offset(inset, inset)
            val arcSize = Size(this.size.width - sw, this.size.height - sw)
            drawArc(
                color      = Color.White.copy(.2f),
                startAngle = -90f,
                sweepAngle = 360f,
                useCenter  = false,
                topLeft    = topLeft,
                size       = arcSize,
                style      = Stroke(sw, cap = StrokeCap.Round)
            )
            drawArc(
                color      = Rc.GoldBright,
                startAngle = -90f,
                sweepAngle = 360f * (score / 100f),
                useCenter  = false,
                topLeft    = topLeft,
                size       = arcSize,
                style      = Stroke(sw, cap = StrokeCap.Round)
            )
        }
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(2.dp)
        ) {
            Text(
                "$score",
                style = RcType.Display.copy(fontSize = 36.sp, fontWeight = FontWeight.SemiBold),
                color = Color.White
            )
            Text("Tajweed", style = RcType.LabelSm, color = Color.White.copy(.75f))
        }
    }
}

// ── Stat strip (3 cells) ──────────────────────────────────────────────────────
@Composable
private fun StatStrip(result: RecitationResult) {
    Row(
        Modifier
            .fillMaxWidth()
            .shadow(3.dp, RoundedCornerShape(16.dp))
            .clip(RoundedCornerShape(16.dp))
            .background(Rc.Cream)
            .border(1.dp, Rc.Line, RoundedCornerShape(16.dp))
            .padding(horizontal = 8.dp, vertical = 16.dp),
        Arrangement.SpaceEvenly,
        Alignment.CenterVertically
    ) {
        StatCell("Letters",  "${result.lettersPct}%")
        Box(Modifier.width(1.dp).height(36.dp).background(Rc.Line))
        StatCell("Tajweed",  "${result.overallTajweed}%")
        Box(Modifier.width(1.dp).height(36.dp).background(Rc.Line))
        StatCell("Pace",     "${"%.1f".format(result.pacePct)}×")
    }
}

@Composable
private fun StatCell(label: String, value: String) {
    Column(
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(3.dp)
    ) {
        Text(value, style = RcType.Display.copy(fontSize = 20.sp), color = Rc.Ink)
        Text(label, style = RcType.LabelSm, color = Rc.Muted)
    }
}

// ── Corrections / feedback list ───────────────────────────────────────────────
@Composable
private fun FeedbackNotesList(notes: List<Note>) {
    Column(verticalArrangement = Arrangement.spacedBy(4.dp)) {
        Text(
            "Feedback",
            style = RcType.Display,
            color = Rc.Ink,
            modifier = Modifier.padding(bottom = 4.dp)
        )
        notes.forEach { note -> NoteCard(note) }
    }
}

@Composable
private fun NoteCard(note: Note) {
    val cardBg    = if (note.isPositive) Rc.EmeraldSoft else Rc.Cream
    val borderCol = if (note.isPositive) Rc.Emerald.copy(.3f) else Rc.Line
    val dotColor  = note.kind.accentColor()

    Row(
        Modifier
            .fillMaxWidth()
            .shadow(if (note.isPositive) 0.dp else 2.dp, RoundedCornerShape(16.dp))
            .clip(RoundedCornerShape(16.dp))
            .background(cardBg)
            .border(1.dp, borderCol, RoundedCornerShape(16.dp))
            .padding(14.dp),
        horizontalArrangement = Arrangement.spacedBy(12.dp),
        verticalAlignment     = Alignment.Top
    ) {
        Box(
            Modifier
                .padding(top = 4.dp)
                .size(10.dp)
                .background(dotColor, CircleShape)
        )

        Column(Modifier.weight(1f), verticalArrangement = Arrangement.spacedBy(5.dp)) {
            Row(
                verticalAlignment     = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(8.dp)
            ) {
                Text(
                    note.title,
                    style    = RcType.BodyBold,
                    color    = if (note.isPositive) Rc.EmeraldDeep else Rc.Ink,
                    modifier = Modifier.weight(1f, fill = false)
                )
                Text(
                    note.arabic,
                    style = RcType.Arabic.copy(fontSize = 16.sp, lineHeight = 22.sp),
                    color = dotColor
                )
            }

            Text(note.tip, style = RcType.Body, color = Rc.Muted, lineHeight = 19.sp)

            if (!note.isPositive) {
                Row(
                    Modifier
                        .clip(RoundedCornerShape(8.dp))
                        .background(Rc.GoldSoft)
                        .clickable { /* TODO: play note.referenceAudioUrl */ }
                        .padding(horizontal = 8.dp, vertical = 5.dp),
                    verticalAlignment     = Alignment.CenterVertically,
                    horizontalArrangement = Arrangement.spacedBy(4.dp)
                ) {
                    Icon(
                        Icons.Default.PlayArrow,
                        contentDescription = "Hear reference",
                        tint               = Rc.Gold,
                        modifier           = Modifier.size(14.dp)
                    )
                    Text("Hear reference", style = RcType.LabelSm, color = Rc.Gold)
                }
            }
        }
    }
}

// ── CTA row (Try again / Next āyah) ──────────────────────────────────────────
@Composable
private fun FeedbackCtaBar(vm: ReciteViewModel) {
    Surface(
        Modifier.fillMaxWidth(),
        color           = Rc.Cream,
        shadowElevation = 12.dp,
        tonalElevation  = 0.dp
    ) {
        Row(
            Modifier
                .fillMaxWidth()
                .padding(horizontal = 24.dp, vertical = 16.dp),
            horizontalArrangement = Arrangement.spacedBy(12.dp),
            verticalAlignment     = Alignment.CenterVertically
        ) {
            Button(
                onClick  = { vm.restart() },
                modifier = Modifier
                    .weight(1f)
                    .height(50.dp),
                colors   = ButtonDefaults.buttonColors(
                    containerColor = Rc.Gold,
                    contentColor   = Color.White
                ),
                shape = RoundedCornerShape(14.dp)
            ) {
                Text("Try again", style = RcType.BodyBold)
            }

            OutlinedButton(
                onClick  = { vm.nextAyah() },
                modifier = Modifier
                    .weight(1f)
                    .height(50.dp),
                colors   = ButtonDefaults.outlinedButtonColors(contentColor = Rc.Emerald),
                border   = BorderStroke(1.5.dp, Rc.Emerald),
                shape    = RoundedCornerShape(14.dp)
            ) {
                Text("Next āyah", style = RcType.BodyBold, color = Rc.Emerald)
            }
        }
    }
}
