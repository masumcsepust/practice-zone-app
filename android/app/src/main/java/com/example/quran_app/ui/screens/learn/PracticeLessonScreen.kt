package com.example.quran_app.ui.screens.learn

import androidx.compose.animation.animateContentSize
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
import androidx.compose.material.icons.filled.NavigateNext
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
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.quran_app.domain.model.PracticeStepResponse
import com.example.quran_app.domain.model.StepSyllable
import com.example.quran_app.ui.viewmodel.PracticeUiState
import com.example.quran_app.ui.viewmodel.PracticeLessonViewModel
import com.example.quran_app.ui.viewmodel.RecordingState
import kotlinx.coroutines.delay
import kotlin.random.Random

// ── Recite palette ────────────────────────────────────────────────────────────
private val BgDeep     = Color(0xFF081410)
private val BgCard     = Color(0xFF0F2018)
private val BgCardAlt  = Color(0xFF142A1E)
private val Gold       = Color(0xFFC9A84C)
private val GoldLight  = Color(0xFFE8C975)
private val GoldDim    = Color(0xFF8B6B20)
private val LiveGreen  = Color(0xFF4ADE80)
private val RecRed     = Color(0xFFEF4444)
private val White80    = Color.White.copy(.80f)
private val White50    = Color.White.copy(.50f)
private val White25    = Color.White.copy(.25f)
private val White10    = Color.White.copy(.10f)

// ── Entry ─────────────────────────────────────────────────────────────────────
@Composable
fun ReciteScreen(viewModel: PracticeLessonViewModel) {
    val state by viewModel.state.collectAsState()
    Box(Modifier.fillMaxSize().background(BgDeep)) {
        when (val s = state) {
            is PracticeUiState.Loading   -> LoadingView()
            is PracticeUiState.Error     -> ErrorView(s.message) { viewModel.load() }
            is PracticeUiState.Completed -> CompletedView { viewModel.load() }
            is PracticeUiState.Ready     -> ReadyContent(s, viewModel)
        }
    }
}

// ── Loading ───────────────────────────────────────────────────────────────────
@Composable
private fun LoadingView() =
    Box(Modifier.fillMaxSize(), Alignment.Center) {
        CircularProgressIndicator(color = Gold, strokeWidth = 2.dp, modifier = Modifier.size(36.dp))
    }

// ── Error ─────────────────────────────────────────────────────────────────────
@Composable
private fun ErrorView(msg: String, onRetry: () -> Unit) =
    Box(Modifier.fillMaxSize().padding(32.dp), Alignment.Center) {
        Column(horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(16.dp)) {
            Text("📡", fontSize = 48.sp)
            Text(msg, color = White80, textAlign = TextAlign.Center, fontSize = 15.sp)
            Surface(onClick = onRetry, shape = RoundedCornerShape(50),
                color = Gold.copy(.15f)) {
                Row(Modifier.padding(horizontal = 24.dp, vertical = 12.dp),
                    verticalAlignment = Alignment.CenterVertically,
                    horizontalArrangement = Arrangement.spacedBy(8.dp)) {
                    Icon(Icons.Default.Refresh, null, tint = Gold, modifier = Modifier.size(18.dp))
                    Text("পুনরায় চেষ্টা করুন", color = Gold, fontWeight = FontWeight.Bold, fontSize = 14.sp)
                }
            }
        }
    }

// ── Completed ─────────────────────────────────────────────────────────────────
@Composable
private fun CompletedView(onRestart: () -> Unit) =
    Box(Modifier.fillMaxSize(), Alignment.Center) {
        Column(horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(16.dp),
            modifier = Modifier.padding(32.dp)) {
            Text("🏆", fontSize = 64.sp)
            Text("সব পাঠ সম্পন্ন!", fontSize = 22.sp,
                fontWeight = FontWeight.ExtraBold, color = Gold, textAlign = TextAlign.Center)
            Text("আপনি সফলভাবে সমস্ত অনুশীলন শেষ করেছেন।",
                fontSize = 14.sp, color = White50,
                textAlign = TextAlign.Center, lineHeight = 20.sp)
            Spacer(Modifier.height(8.dp))
            Surface(onClick = onRestart, shape = RoundedCornerShape(50),
                color = Gold) {
                Text("আবার শুরু করুন",
                    modifier = Modifier.padding(horizontal = 32.dp, vertical = 14.dp),
                    fontWeight = FontWeight.Bold, fontSize = 15.sp, color = BgDeep)
            }
        }
    }

// ── Main content ──────────────────────────────────────────────────────────────
@Composable
private fun ReadyContent(s: PracticeUiState.Ready, vm: PracticeLessonViewModel) {
    val step = s.step
    val rs   = s.recordingState

    // Waveform
    val waveHeights = remember { mutableStateListOf<Float>().also { l -> repeat(20) { l.add(.3f) } } }
    LaunchedEffect(rs) {
        if (rs is RecordingState.InProgress) {
            while (true) {
                delay(110)
                val fresh = List(20) { Random.nextFloat() * .85f + .15f }
                waveHeights.clear(); waveHeights.addAll(fresh)
            }
        } else {
            waveHeights.replaceAll { .3f }
        }
    }

    val hasLeft = step.left.arabic.isNotBlank()
    val right   = step.right?.takeIf { it.arabic.isNotBlank() }

    Column(
        Modifier.fillMaxSize()
            .statusBarsPadding()
    ) {
        // Top bar
        TopBar(step, rs)

        // Scrollable body
        Column(
            Modifier.weight(1f).verticalScroll(rememberScrollState())
                .padding(horizontal = 16.dp),
            verticalArrangement = Arrangement.spacedBy(14.dp)
        ) {
            Spacer(Modifier.height(4.dp))

            // Lesson info card
            LessonInfoCard(step)

            // Arabic display card
            ArabicCard(step, hasLeft, right, rs)

            // Waveform / status
            if (rs is RecordingState.InProgress) {
                WaveformRow(rs.isTarget, waveHeights)
            } else if (rs is RecordingState.Done) {
                ScoreBadge(rs.score, rs.isTarget) { vm.resetRecording() }
            } else {
                Spacer(Modifier.height(8.dp))
            }

            // Details row (compact)
            if (hasLeft) {
                Row(Modifier.fillMaxWidth(), horizontalArrangement = Arrangement.spacedBy(10.dp)) {
                    CompactDetail(step.left, isLeft = true, Modifier.weight(1f))
                    if (right != null) CompactDetail(right, isLeft = false, Modifier.weight(1f))
                }
            }

            // Remember tip
            if (step.rememberText.isNotBlank()) TipRow(step.rememberText)

            Spacer(Modifier.height(8.dp))
        }

        // Controls
        ControlsBar(rs, s.isFirstItem, vm, right != null)

        // Prev/Next thin bar
        PrevNextBar(s, vm)
    }
}

// ── Top bar ───────────────────────────────────────────────────────────────────
@Composable
private fun TopBar(step: PracticeStepResponse, rs: RecordingState) {
    val isLive = rs is RecordingState.InProgress
    Row(
        Modifier.fillMaxWidth().padding(horizontal = 16.dp, vertical = 14.dp),
        Arrangement.SpaceBetween,
        Alignment.CenterVertically
    ) {
        Box(
            Modifier.size(36.dp).background(White10, CircleShape),
            Alignment.Center
        ) {
            Icon(Icons.Default.ArrowBackIosNew, null, tint = White80, modifier = Modifier.size(16.dp))
        }

        Text("রেকর্ড", fontSize = 18.sp, fontWeight = FontWeight.Bold, color = Color.White)

        // Live badge
        Surface(
            shape = RoundedCornerShape(20.dp),
            color = if (isLive) LiveGreen.copy(.2f) else White10
        ) {
            Row(
                Modifier.padding(horizontal = 10.dp, vertical = 5.dp),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(5.dp)
            ) {
                val blink by rememberInfiniteTransition(label = "live").animateFloat(
                    .3f, 1f, infiniteRepeatable(tween(600), RepeatMode.Reverse), "b"
                )
                Box(
                    Modifier.size(7.dp)
                        .background(
                            if (isLive) LiveGreen.copy(blink) else White25,
                            CircleShape
                        )
                )
                Text(
                    if (isLive) "Live" else "${step.lessonNum}/${step.totalLessons}",
                    fontSize = 11.sp, fontWeight = FontWeight.Bold,
                    color = if (isLive) LiveGreen else White50
                )
            }
        }
    }
}

// ── Lesson info card (white) ──────────────────────────────────────────────────
@Composable
private fun LessonInfoCard(step: PracticeStepResponse) {
    Row(
        Modifier.fillMaxWidth()
            .shadow(4.dp, RoundedCornerShape(16.dp))
            .clip(RoundedCornerShape(16.dp))
            .background(Color.White)
            .padding(14.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(12.dp)
    ) {
        // Arabic glyph icon
        Box(
            Modifier.size(52.dp)
                .background(
                    Brush.linearGradient(listOf(Color(0xFF0F2018), Color(0xFF1A3A28))),
                    RoundedCornerShape(14.dp)
                ),
            Alignment.Center
        ) {
            Text(step.left.arabic, fontSize = 26.sp, color = Gold, fontWeight = FontWeight.ExtraBold)
        }

        Column(Modifier.weight(1f)) {
            Text("LESSON ${step.lessonNum}",
                fontSize = 10.sp, fontWeight = FontWeight.ExtraBold,
                color = Gold, letterSpacing = 1.sp)
            Text(step.title, fontSize = 15.sp, fontWeight = FontWeight.Bold, color = Color(0xFF1A2E1A))
            Text("Step ${step.id} of ${step.totalSteps}",
                fontSize = 12.sp, color = Color(0xFF6B7280))
        }

        // Step progress dots
        Row(horizontalArrangement = Arrangement.spacedBy(4.dp),
            verticalAlignment = Alignment.CenterVertically) {
            repeat(step.totalSteps.coerceAtMost(6)) { i ->
                val active = i == step.id - 1
                Box(
                    Modifier.size(if (active) 8.dp else 6.dp)
                        .background(if (active) Gold else Color(0xFFD1D5DB), CircleShape)
                )
            }
        }
    }
}

// ── Arabic display card (dark gold) ──────────────────────────────────────────
@Composable
private fun ArabicCard(
    step: PracticeStepResponse,
    hasLeft: Boolean,
    right: StepSyllable?,
    rs: RecordingState,
) {
    val leftActive  = rs !is RecordingState.Done || rs.isTarget
    val rightActive = rs is RecordingState.Done && !rs.isTarget

    Box(
        Modifier.fillMaxWidth()
            .shadow(6.dp, RoundedCornerShape(20.dp))
            .clip(RoundedCornerShape(20.dp))
            .background(BgCard)
            .border(1.dp, Gold.copy(.18f), RoundedCornerShape(20.dp))
            .padding(horizontal = 20.dp, vertical = 24.dp)
    ) {
        Column(
            Modifier.fillMaxWidth(),
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(6.dp)
        ) {
            if (right != null && hasLeft) {
                // Two syllables side by side
                Row(
                    Modifier.fillMaxWidth(),
                    Arrangement.SpaceEvenly,
                    Alignment.CenterVertically
                ) {
                    // Left syllable
                    Column(
                        horizontalAlignment = Alignment.CenterHorizontally,
                        modifier = Modifier
                            .clip(RoundedCornerShape(12.dp))
                            .background(if (leftActive) Gold.copy(.08f) else Color.Transparent)
                            .padding(horizontal = 12.dp, vertical = 8.dp)
                    ) {
                        Text(step.left.arabic, fontSize = 56.sp,
                            color = Gold, fontWeight = FontWeight.ExtraBold,
                            textAlign = TextAlign.Center)
                        Text(step.left.translit, fontSize = 18.sp,
                            color = Color.White, fontWeight = FontWeight.Bold)
                        Text("(${step.left.bengali})", fontSize = 12.sp, color = White50)
                    }

                    // VS divider
                    Column(horizontalAlignment = Alignment.CenterHorizontally) {
                        Box(Modifier.height(40.dp).width(1.dp).background(Gold.copy(.2f)))
                        Text("vs", fontSize = 11.sp, color = Gold.copy(.6f),
                            fontWeight = FontWeight.Bold,
                            modifier = Modifier.padding(vertical = 4.dp))
                        Box(Modifier.height(40.dp).width(1.dp).background(Gold.copy(.2f)))
                    }

                    // Right syllable
                    Column(
                        horizontalAlignment = Alignment.CenterHorizontally,
                        modifier = Modifier
                            .clip(RoundedCornerShape(12.dp))
                            .background(if (rightActive) GoldLight.copy(.08f) else Color.Transparent)
                            .padding(horizontal = 12.dp, vertical = 8.dp)
                    ) {
                        Text(right.arabic, fontSize = 56.sp,
                            color = GoldLight, fontWeight = FontWeight.ExtraBold,
                            textAlign = TextAlign.Center)
                        Text(right.translit, fontSize = 18.sp,
                            color = Color.White, fontWeight = FontWeight.Bold)
                        Text("(${right.bengali})", fontSize = 12.sp, color = White50)
                    }
                }
            } else if (hasLeft) {
                // Single syllable centred
                Text(step.left.arabic, fontSize = 80.sp,
                    color = Gold, fontWeight = FontWeight.ExtraBold)
                Text(step.left.translit, fontSize = 24.sp,
                    color = Color.White, fontWeight = FontWeight.Bold)
                Text("(${step.left.bengali})", fontSize = 14.sp, color = White50)
            }

            Spacer(Modifier.height(10.dp))

            // Italic hint
            Text(
                step.tips.ifBlank { "স্পষ্টভাবে পড়ুন — মাইক্রোফোনে ট্যাপ করে রেকর্ড করুন" },
                fontSize = 12.sp, color = White50,
                textAlign = TextAlign.Center,
                fontStyle = FontStyle.Italic,
                lineHeight = 17.sp
            )

            Spacer(Modifier.height(10.dp))

            // Progress dashes
            Row(horizontalArrangement = Arrangement.spacedBy(8.dp),
                verticalAlignment = Alignment.CenterVertically) {
                repeat(step.totalSteps.coerceAtMost(8)) { i ->
                    val active = i == step.id - 1
                    Box(
                        Modifier.height(2.dp)
                            .width(if (active) 28.dp else 12.dp)
                            .background(
                                if (active) Gold else White25,
                                RoundedCornerShape(1.dp)
                            )
                    )
                }
            }
        }
    }
}

// ── Waveform row ──────────────────────────────────────────────────────────────
@Composable
private fun WaveformRow(isTarget: Boolean, waveHeights: SnapshotStateList<Float>) {
    val blink by rememberInfiniteTransition(label = "blink").animateFloat(
        .3f, 1f, infiniteRepeatable(tween(500), RepeatMode.Reverse), "b"
    )
    val barColor = if (isTarget) Gold else GoldLight

    Column(
        Modifier.fillMaxWidth()
            .clip(RoundedCornerShape(16.dp))
            .background(BgCardAlt)
            .padding(horizontal = 16.dp, vertical = 14.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(10.dp)
    ) {
        Row(verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(8.dp)) {
            Box(Modifier.size(8.dp).background(RecRed.copy(blink), CircleShape))
            Text("LISTENING...", fontSize = 11.sp, fontWeight = FontWeight.ExtraBold,
                color = RecRed.copy(blink), letterSpacing = 2.sp)
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
                            Brush.verticalGradient(listOf(barColor, barColor.copy(.3f))),
                            RoundedCornerShape(2.dp)
                        )
                )
            }
        }
    }
}

// ── Score badge ───────────────────────────────────────────────────────────────
@Composable
private fun ScoreBadge(score: Int, isTarget: Boolean, onRetry: () -> Unit) {
    val barColor = if (isTarget) Gold else GoldLight
    val label = when { score >= 92 -> "অসাধারণ!" ; score >= 80 -> "দারুণ!" ; else -> "ভালো!" }

    Row(
        Modifier.fillMaxWidth()
            .clip(RoundedCornerShape(14.dp))
            .background(BgCardAlt)
            .border(1.dp, barColor.copy(.25f), RoundedCornerShape(14.dp))
            .padding(horizontal = 16.dp, vertical = 12.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(12.dp)
    ) {
        Box(Modifier.size(40.dp).background(barColor.copy(.15f), CircleShape)
            .border(1.dp, barColor.copy(.3f), CircleShape), Alignment.Center) {
            Text("✓", fontSize = 18.sp, color = barColor, fontWeight = FontWeight.ExtraBold)
        }
        Column(Modifier.weight(1f)) {
            Text(label, fontSize = 15.sp, fontWeight = FontWeight.ExtraBold, color = barColor)
            Text("আপনার সঠিকতা পরিমাপ", fontSize = 11.sp, color = White50)
        }
        Text("$score%", fontSize = 28.sp, fontWeight = FontWeight.ExtraBold, color = barColor)
        IconButton(onClick = onRetry, modifier = Modifier.size(32.dp)) {
            Icon(Icons.Default.Refresh, null, tint = White50, modifier = Modifier.size(16.dp))
        }
    }
}

// ── Compact detail pill ────────────────────────────────────────────────────────
@Composable
private fun CompactDetail(syllable: StepSyllable, isLeft: Boolean, modifier: Modifier) {
    val accent = if (isLeft) Gold else GoldLight
    Column(
        modifier
            .clip(RoundedCornerShape(14.dp))
            .background(BgCard)
            .border(1.dp, accent.copy(.18f), RoundedCornerShape(14.dp))
            .padding(12.dp),
        verticalArrangement = Arrangement.spacedBy(5.dp)
    ) {
        Row(verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(5.dp)) {
            Box(Modifier.size(16.dp).background(accent.copy(.2f), CircleShape), Alignment.Center) {
                Text("i", fontSize = 9.sp, color = accent, fontWeight = FontWeight.ExtraBold)
            }
            Text("বিস্তারিত তথ্য", fontSize = 11.sp, fontWeight = FontWeight.Bold, color = accent)
        }
        HorizontalDivider(color = accent.copy(.12f))
        DetailPill("অক্ষর", syllable.letterBengali, accent)
        DetailPill("চিহ্নের নাম", syllable.signName, accent)
        Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
            Text("গ্রুপ", fontSize = 10.sp, color = White50, fontWeight = FontWeight.SemiBold)
            Surface(shape = RoundedCornerShape(20.dp), color = accent.copy(.15f)) {
                Text(syllable.signGroup,
                    modifier = Modifier.padding(horizontal = 8.dp, vertical = 3.dp),
                    fontSize = 10.sp, fontWeight = FontWeight.Bold, color = accent)
            }
        }
    }
}

@Composable
private fun DetailPill(label: String, value: String, accent: Color) {
    Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween) {
        Text(label, fontSize = 10.sp, color = White50, fontWeight = FontWeight.SemiBold)
        Text(value, fontSize = 10.sp, color = White80, fontWeight = FontWeight.Bold,
            textAlign = TextAlign.End)
    }
}

// ── Tip row ───────────────────────────────────────────────────────────────────
@Composable
private fun TipRow(text: String) {
    Row(
        Modifier.fillMaxWidth()
            .clip(RoundedCornerShape(12.dp))
            .background(Gold.copy(.07f))
            .border(1.dp, Gold.copy(.15f), RoundedCornerShape(12.dp))
            .padding(12.dp),
        horizontalArrangement = Arrangement.spacedBy(10.dp)
    ) {
        Text("⭐", fontSize = 14.sp)
        Text(text, fontSize = 12.sp, color = White50, lineHeight = 17.sp)
    }
}

// ── Controls bar ──────────────────────────────────────────────────────────────
@Composable
private fun ControlsBar(
    rs: RecordingState,
    isFirstItem: Boolean,
    vm: PracticeLessonViewModel,
    hasBoth: Boolean,
) {
    val isRecording = rs is RecordingState.InProgress
    val pulse = rememberInfiniteTransition(label = "pulse")
    val ring by pulse.animateFloat(0f, 1f,
        infiniteRepeatable(tween(900, easing = EaseInOut), RepeatMode.Reverse), "r")

    Box(
        Modifier.fillMaxWidth()
            .background(BgCard)
            .padding(horizontal = 32.dp, vertical = 20.dp),
        Alignment.Center
    ) {
        Row(
            Modifier.fillMaxWidth(),
            Arrangement.SpaceEvenly,
            Alignment.CenterVertically
        ) {
            // Reset / prev side button
            Box(
                Modifier.size(52.dp)
                    .background(White10, CircleShape)
                    .clickable { if (rs is RecordingState.Done || rs is RecordingState.InProgress) vm.resetRecording() },
                Alignment.Center
            ) {
                Icon(Icons.Default.Refresh, null, tint = White50, modifier = Modifier.size(22.dp))
            }

            // Main mic button
            Box(contentAlignment = Alignment.Center) {
                if (isRecording) {
                    Box(
                        Modifier.size((80 + 24 * ring).dp)
                            .background(RecRed.copy(.2f * (1f - ring)), CircleShape)
                    )
                    Box(
                        Modifier.size((80 + 10 * ring).dp)
                            .background(RecRed.copy(.15f * (1f - ring)), CircleShape)
                    )
                }
                Box(
                    Modifier
                        .size(80.dp)
                        .shadow(12.dp, CircleShape)
                        .background(
                            if (isRecording)
                                Brush.radialGradient(listOf(RecRed, Color(0xFFB91C1C)))
                            else
                                Brush.radialGradient(listOf(GoldLight, Gold, GoldDim)),
                            CircleShape
                        )
                        .clickable {
                            val nextTarget = when (rs) {
                                is RecordingState.Idle -> true
                                is RecordingState.Done -> if (hasBoth && rs.isTarget) false else true
                                is RecordingState.InProgress -> rs.isTarget
                            }
                            vm.toggleRecording(nextTarget)
                        },
                    Alignment.Center
                ) {
                    Icon(Icons.Default.Mic, null, tint = BgDeep, modifier = Modifier.size(36.dp))
                }
            }

            // Skip / next side button
            Box(
                Modifier.size(52.dp)
                    .background(White10, CircleShape)
                    .clickable { vm.next() },
                Alignment.Center
            ) {
                Icon(Icons.Default.NavigateNext, null, tint = White50, modifier = Modifier.size(22.dp))
            }
        }
    }
}

// ── Prev / Next thin bar ──────────────────────────────────────────────────────
@Composable
private fun PrevNextBar(s: PracticeUiState.Ready, vm: PracticeLessonViewModel) {
    Row(
        Modifier.fillMaxWidth()
            .background(BgDeep)
            .padding(horizontal = 24.dp, vertical = 10.dp),
        Arrangement.SpaceBetween,
        Alignment.CenterVertically
    ) {
        TextButton(
            onClick  = { vm.prev() },
            enabled  = !s.isFirstItem
        ) {
            Text("< পূর্ববর্তী", fontSize = 13.sp,
                color = if (s.isFirstItem) White25 else White50,
                fontWeight = FontWeight.SemiBold)
        }

        // Dots
        Row(horizontalArrangement = Arrangement.spacedBy(5.dp),
            verticalAlignment = Alignment.CenterVertically) {
            repeat(s.step.totalSteps.coerceAtMost(8)) { i ->
                val active = i == s.step.id - 1
                Box(
                    Modifier.height(4.dp)
                        .animateContentSize(spring(stiffness = Spring.StiffnessMediumLow))
                        .width(if (active) 20.dp else 4.dp)
                        .background(
                            if (active) Gold else White25,
                            CircleShape
                        )
                )
            }
        }

        TextButton(onClick = { vm.next() }) {
            Text("পরবর্তী >", fontSize = 13.sp,
                color = Gold, fontWeight = FontWeight.SemiBold)
        }
    }
}
