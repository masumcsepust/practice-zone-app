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
import androidx.compose.material.icons.filled.Check
import androidx.compose.material.icons.filled.MenuBook
import androidx.compose.material.icons.filled.Mic
import androidx.compose.material.icons.filled.NavigateBefore
import androidx.compose.material.icons.filled.NavigateNext
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.runtime.snapshots.SnapshotStateList
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.compose.ui.zIndex
import com.example.quran_app.domain.model.PracticeStepResponse
import com.example.quran_app.domain.model.StepSyllable
import com.example.quran_app.ui.theme.*
import com.example.quran_app.ui.viewmodel.PracticeUiState
import com.example.quran_app.ui.viewmodel.PracticeLessonViewModel
import com.example.quran_app.ui.viewmodel.RecordingState
import androidx.compose.material.icons.filled.VolumeUp
import kotlinx.coroutines.delay
import kotlin.random.Random

// ── Palette ───────────────────────────────────────────────────────────────────
private val Green900  = Color(0xFF064E3B)
private val Green700  = Color(0xFF047857)
private val Green600  = Color(0xFF0D713B)
private val Green400  = Color(0xFF34D399)
private val Green200  = Color(0xFFA7F3D0)

private val Blue700   = Color(0xFF1D4ED8)
private val Blue600   = Color(0xFF2563EB)
private val Blue100   = Color(0xFFDBEAFE)
private val Blue50    = Color(0xFFEFF6FF)

private val Violet700 = Color(0xFF6D28D9)
private val Violet600 = Color(0xFF7C3AED)
private val Violet100 = Color(0xFFEDE9FE)
private val Violet50  = Color(0xFFF5F3FF)

private val Amber500  = Color(0xFFF59E0B)
private val Amber100  = Color(0xFFFEF3C7)
private val Amber50   = Color(0xFFFFFBEB)

private val Red500    = Color(0xFFEF4444)
private val Red200    = Color(0xFFFECACA)

private val Slate800  = Color(0xFF1E293B)
private val Slate600  = Color(0xFF475569)
private val Slate200  = Color(0xFFE2E8F0)
private val Slate100  = Color(0xFFF1F5F9)
private val Slate50   = Color(0xFFF8FAFC)

// ── Entry point ───────────────────────────────────────────────────────────────
@Composable
fun PracticeLessonScreen(viewModel: PracticeLessonViewModel) {
    val state by viewModel.state.collectAsState()
    when (val s = state) {
        is PracticeUiState.Loading   -> LoadingView()
        is PracticeUiState.Error     -> ErrorView(s.message) { viewModel.load() }
        is PracticeUiState.Completed -> CompletedView { viewModel.load() }
        is PracticeUiState.Ready     -> ReadyContent(s, viewModel)
    }
}

@Composable private fun LoadingView() =
    Box(Modifier.fillMaxSize(), Alignment.Center) {
        CircularProgressIndicator(color = Green600, strokeWidth = 3.dp)
    }

@Composable private fun ErrorView(msg: String, onRetry: () -> Unit) =
    Box(Modifier.fillMaxSize().padding(32.dp), Alignment.Center) {
        Column(horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(16.dp)) {
            Text("⚠️", fontSize = 40.sp)
            Text(msg, color = Slate600, textAlign = TextAlign.Center, fontSize = 15.sp)
            Button(onClick = onRetry, shape = RoundedCornerShape(50),
                colors = ButtonDefaults.buttonColors(containerColor = Green600)) {
                Text("আবার চেষ্টা করুন", fontWeight = FontWeight.Bold)
            }
        }
    }

// ── Main ready content ────────────────────────────────────────────────────────
@Composable
private fun ReadyContent(s: PracticeUiState.Ready, vm: PracticeLessonViewModel) {
    val step = s.step
    val rs   = s.recordingState

    val waveHeights = remember { mutableStateListOf<Float>().also { l -> repeat(18) { l.add(.4f) } } }
    LaunchedEffect(rs) {
        if (rs is RecordingState.InProgress) {
            while (true) {
                delay(120)
                val fresh = List(18) { Random.nextFloat() * .75f + .25f }
                waveHeights.clear(); waveHeights.addAll(fresh)
            }
        }
    }

    Column(Modifier.fillMaxSize().background(Slate100)) {
        Header(step)

        Column(
            Modifier.weight(1f).verticalScroll(rememberScrollState())
                .padding(horizontal = 14.dp, vertical = 14.dp),
            verticalArrangement = Arrangement.spacedBy(12.dp)
        ) {
            InstructionRow(step.id)
            TipCard(step.tips)

            val hasLeft = step.left.arabic.isNotBlank()
            val right   = step.right?.takeIf { it.arabic.isNotBlank() }
            val hasBoth = hasLeft && right != null

            if (hasBoth)       TwoCardRow(step.left, right!!, rs, vm)
            else if (hasLeft)  SingleCard(step.left, rs, vm)

            if (rs is RecordingState.InProgress)
                WaveformOverlay(rs.isTarget, if (rs.isTarget) step.left.translit else right?.translit ?: "", waveHeights)

            if (rs is RecordingState.Done)
                ScoreCard(rs.score, rs.isTarget) { vm.resetRecording() }

            if (hasBoth) {
                Row(Modifier.fillMaxWidth(), horizontalArrangement = Arrangement.spacedBy(10.dp)) {
                    DetailsCard(step.left, isLeft = true,  Modifier.weight(1f))
                    DetailsCard(right!!,   isLeft = false, Modifier.weight(1f))
                }
                ListenRow(step.left, right!!, vm)
            } else if (hasLeft) {
                DetailsCard(step.left, isLeft = true, Modifier.fillMaxWidth())
            }

            if (step.rememberText.isNotBlank()) RememberCard(step.rememberText)
            Spacer(Modifier.height(2.dp))
        }

        PrevNextBar(s, vm)
    }
}

// ── Header ────────────────────────────────────────────────────────────────────
@Composable
private fun Header(step: PracticeStepResponse) {
    Box(
        Modifier.fillMaxWidth()
            .background(Brush.linearGradient(listOf(Green900, Green700, Green600)))
            .statusBarsPadding()
            .padding(horizontal = 18.dp, vertical = 16.dp)
    ) {
        Column(verticalArrangement = Arrangement.spacedBy(12.dp)) {
            Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                Column {
                    Text(step.title, fontSize = 20.sp, fontWeight = FontWeight.ExtraBold,
                        color = Color.White, letterSpacing = 0.3.sp)
                    Row(verticalAlignment = Alignment.CenterVertically,
                        horizontalArrangement = Arrangement.spacedBy(6.dp)) {
                        Surface(shape = RoundedCornerShape(20.dp), color = Green400.copy(.25f)) {
                            Text("Lesson ${step.lessonNum}",
                                modifier = Modifier.padding(horizontal = 8.dp, vertical = 2.dp),
                                fontSize = 11.sp, fontWeight = FontWeight.Bold, color = Green200)
                        }
                        Text("of ${step.totalLessons} lessons",
                            fontSize = 11.sp, color = Color.White.copy(.6f))
                    }
                }
                Box(
                    Modifier.size(42.dp)
                        .shadow(4.dp, RoundedCornerShape(14.dp))
                        .background(Color.White.copy(.15f), RoundedCornerShape(14.dp)),
                    Alignment.Center
                ) {
                    Icon(Icons.Default.MenuBook, null, tint = Color.White, modifier = Modifier.size(22.dp))
                }
            }

            // Progress bar
            Column(verticalArrangement = Arrangement.spacedBy(4.dp)) {
                Box(Modifier.fillMaxWidth().height(8.dp).clip(RoundedCornerShape(50))) {
                    Box(Modifier.fillMaxSize().background(Color.White.copy(.15f)))
                    Box(
                        Modifier.fillMaxHeight()
                            .fillMaxWidth((step.progress / 100f).coerceIn(0f, 1f))
                            .background(Brush.horizontalGradient(listOf(Green400, Color(0xFF6EE7B7))))
                    )
                }
                Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween) {
                    Text("অগ্রগতি", fontSize = 10.sp, color = Color.White.copy(.6f))
                    Text("${step.progress}%", fontSize = 10.sp, fontWeight = FontWeight.Bold, color = Green200)
                }
            }
        }
    }
}

// ── Instruction row ───────────────────────────────────────────────────────────
@Composable
private fun InstructionRow(stepId: Int) {
    Row(verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.spacedBy(10.dp)) {
        Box(
            Modifier.size(30.dp)
                .background(Brush.linearGradient(listOf(Green700, Green600)), CircleShape),
            Alignment.Center
        ) {
            Text("$stepId", fontSize = 14.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
        }
        Text("নিচের অংশটি পড়ুন এবং রেকর্ড করুন",
            fontSize = 15.sp, fontWeight = FontWeight.Bold, color = Slate800)
    }
}

// ── Tip card ──────────────────────────────────────────────────────────────────
@Composable
private fun TipCard(tip: String) {
    if (tip.isBlank()) return
    Row(
        Modifier.fillMaxWidth()
            .shadow(2.dp, RoundedCornerShape(14.dp))
            .clip(RoundedCornerShape(14.dp))
            .background(Color.White)
            .padding(start = 0.dp, top = 0.dp, end = 0.dp, bottom = 0.dp)
    ) {
        Box(Modifier.width(4.dp).fillMaxHeight().background(Green600))
        Row(Modifier.padding(12.dp), horizontalArrangement = Arrangement.spacedBy(10.dp)) {
            Text("💡", fontSize = 20.sp)
            Column {
                Text("সাফল্যের টিপস:", fontSize = 12.sp, fontWeight = FontWeight.ExtraBold, color = Green700)
                Spacer(Modifier.height(2.dp))
                Text(tip, fontSize = 12.sp, color = Slate600, lineHeight = 17.sp)
            }
        }
    }
}

// ── Two-card row with vs badge ────────────────────────────────────────────────
@Composable
private fun TwoCardRow(left: StepSyllable, right: StepSyllable, rs: RecordingState, vm: PracticeLessonViewModel) {
    val leftRecording  = rs is RecordingState.InProgress && rs.isTarget
    val rightRecording = rs is RecordingState.InProgress && !rs.isTarget

    Box(Modifier.fillMaxWidth()) {
        Row(Modifier.fillMaxWidth(),
            horizontalArrangement = Arrangement.spacedBy(6.dp),
            verticalAlignment = Alignment.CenterVertically) {
            SyllableCard(
                syllable    = left,
                gradient    = Brush.linearGradient(listOf(Blue50, Blue100)),
                accentColor = Blue600,
                darkColor   = Blue700,
                isRecording = leftRecording,
                modifier    = Modifier.weight(1f),
                onMicClick  = { vm.toggleRecording(true) }
            )
            Spacer(Modifier.width(32.dp))
            SyllableCard(
                syllable    = right,
                gradient    = Brush.linearGradient(listOf(Violet50, Violet100)),
                accentColor = Violet600,
                darkColor   = Violet700,
                isRecording = rightRecording,
                modifier    = Modifier.weight(1f),
                onMicClick  = { vm.toggleRecording(false) }
            )
        }
        // Floating vs badge
        Box(
            Modifier
                .size(34.dp)
                .align(Alignment.Center)
                .zIndex(2f)
                .shadow(6.dp, CircleShape)
                .background(
                    Brush.linearGradient(listOf(Blue600, Violet600)),
                    CircleShape
                ),
            Alignment.Center
        ) {
            Text("vs", fontSize = 9.sp, fontWeight = FontWeight.ExtraBold, color = Color.White,
                letterSpacing = 0.5.sp)
        }
    }
}

// ── Single card ───────────────────────────────────────────────────────────────
@Composable
private fun SingleCard(left: StepSyllable, rs: RecordingState, vm: PracticeLessonViewModel) {
    SyllableCard(
        syllable    = left,
        gradient    = Brush.linearGradient(listOf(Blue50, Blue100)),
        accentColor = Blue600,
        darkColor   = Blue700,
        isRecording = rs is RecordingState.InProgress,
        modifier    = Modifier.fillMaxWidth(),
        onMicClick  = { vm.toggleRecording(true) }
    )
}

// ── Syllable card ─────────────────────────────────────────────────────────────
@Composable
private fun SyllableCard(
    syllable: StepSyllable,
    gradient: Brush,
    accentColor: Color,
    darkColor: Color,
    isRecording: Boolean,
    modifier: Modifier,
    onMicClick: () -> Unit,
) {
    val inf   = rememberInfiniteTransition(label = "rec")
    val ring  by inf.animateFloat(0f, 1f,
        infiniteRepeatable(tween(900, easing = EaseInOut), RepeatMode.Reverse), "ring")
    val borderColor = if (isRecording) Red500 else accentColor.copy(.3f)

    Box(
        modifier
            .shadow(if (isRecording) 8.dp else 3.dp, RoundedCornerShape(20.dp))
            .clip(RoundedCornerShape(20.dp))
            .background(gradient)
            .border(1.5.dp, borderColor, RoundedCornerShape(20.dp))
    ) {
        // Watermark
        Text(
            syllable.arabic,
            fontSize = 90.sp,
            color = accentColor.copy(.06f),
            fontWeight = FontWeight.ExtraBold,
            modifier = Modifier.align(Alignment.TopEnd).offset(x = 8.dp, y = (-10).dp)
        )

        Column(
            Modifier.fillMaxWidth().padding(horizontal = 12.dp, vertical = 16.dp),
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(4.dp)
        ) {
            // Arabic
            Text(syllable.arabic, fontSize = 58.sp,
                fontWeight = FontWeight.ExtraBold, color = darkColor)

            // Transliteration
            Text(syllable.translit, fontSize = 20.sp,
                fontWeight = FontWeight.ExtraBold, color = accentColor)

            // Bengali
            if (syllable.bengali.isNotBlank()) {
                Surface(shape = RoundedCornerShape(20.dp), color = accentColor.copy(.1f)) {
                    Text("(${syllable.bengali})",
                        modifier = Modifier.padding(horizontal = 8.dp, vertical = 2.dp),
                        fontSize = 12.sp, color = darkColor, fontWeight = FontWeight.SemiBold)
                }
            }

            Spacer(Modifier.height(8.dp))

            // Mic button with pulsing ring
            Box(contentAlignment = Alignment.Center, modifier = Modifier.size(72.dp)) {
                if (isRecording) {
                    Box(
                        Modifier.size((56 + 18 * ring).dp)
                            .background(Red200.copy(0.45f * (1f - ring)), CircleShape)
                    )
                }
                Box(
                    Modifier
                        .size(54.dp)
                        .shadow(if (isRecording) 8.dp else 4.dp, CircleShape)
                        .background(
                            if (isRecording) Brush.linearGradient(listOf(Red500, Color(0xFFDC2626)))
                            else Brush.linearGradient(listOf(accentColor, darkColor)),
                            CircleShape
                        )
                        .clickable { onMicClick() },
                    Alignment.Center
                ) {
                    Icon(Icons.Default.Mic, null, tint = Color.White, modifier = Modifier.size(24.dp))
                }
            }

            if (isRecording) {
                Spacer(Modifier.height(4.dp))
                Text("রেকর্ড হচ্ছে… ট্যাপ করুন",
                    fontSize = 10.sp, color = Red500,
                    textAlign = TextAlign.Center, lineHeight = 14.sp)
            }
        }
    }
}

// ── Waveform overlay ──────────────────────────────────────────────────────────
@Composable
private fun WaveformOverlay(isTarget: Boolean, translit: String, waveHeights: SnapshotStateList<Float>) {
    val barBrush = if (isTarget)
        Brush.verticalGradient(listOf(Blue600, Blue600.copy(.4f)))
    else
        Brush.verticalGradient(listOf(Violet600, Violet600.copy(.4f)))

    val blinkA by rememberInfiniteTransition(label = "blink").animateFloat(
        .25f, 1f, infiniteRepeatable(tween(480), RepeatMode.Reverse), "a"
    )

    Column(
        Modifier.fillMaxWidth()
            .shadow(4.dp, RoundedCornerShape(16.dp))
            .clip(RoundedCornerShape(16.dp))
            .background(Brush.verticalGradient(listOf(Color(0xFF0F172A), Color(0xFF1E293B))))
            .padding(16.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(10.dp)
    ) {
        Row(verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(8.dp)) {
            Box(Modifier.size(8.dp).background(Red500.copy(blinkA), CircleShape))
            Text("উচ্চারণ রেকর্ড হচ্ছে",
                fontSize = 12.sp, fontWeight = FontWeight.Bold,
                color = Red500, letterSpacing = 0.8.sp)
        }

        Row(
            Modifier.height(52.dp).fillMaxWidth(),
            horizontalArrangement = Arrangement.Center,
            verticalAlignment = Alignment.CenterVertically
        ) {
            waveHeights.forEach { h ->
                Box(Modifier.padding(horizontal = 2.dp).width(3.dp).fillMaxHeight(h.coerceIn(.1f, 1f))
                    .background(barBrush, RoundedCornerShape(2.dp)))
            }
        }

        Text("বলুন: \"$translit\"",
            fontSize = 12.sp, color = Color(0xFF94A3B8), fontWeight = FontWeight.Medium)
    }
}

// ── Score card ────────────────────────────────────────────────────────────────
@Composable
private fun ScoreCard(score: Int, isTarget: Boolean, onRetry: () -> Unit) {
    val accent = if (isTarget) Blue600 else Violet600
    val bg     = Brush.linearGradient(
        if (isTarget) listOf(Blue50, Color(0xFFDBEAFE))
        else          listOf(Violet50, Color(0xFFDDD6FE))
    )
    val label  = when {
        score >= 92 -> "অসাধারণ!"
        score >= 80 -> "দারুণ চেষ্টা!"
        else        -> "ভালো করছেন!"
    }

    Row(
        Modifier.fillMaxWidth()
            .shadow(3.dp, RoundedCornerShape(16.dp))
            .clip(RoundedCornerShape(16.dp))
            .background(bg)
            .border(1.dp, accent.copy(.2f), RoundedCornerShape(16.dp))
            .padding(horizontal = 16.dp, vertical = 14.dp),
        verticalAlignment = Alignment.CenterVertically
    ) {
        Box(
            Modifier.size(44.dp)
                .background(accent.copy(.15f), CircleShape)
                .border(1.5.dp, accent.copy(.3f), CircleShape),
            Alignment.Center
        ) {
            Icon(Icons.Default.Check, null, tint = accent, modifier = Modifier.size(22.dp))
        }
        Spacer(Modifier.width(12.dp))
        Column(Modifier.weight(1f)) {
            Text(label, fontSize = 16.sp, fontWeight = FontWeight.ExtraBold, color = accent)
            Text("আপনার সঠিকতা পরিমাপ", fontSize = 11.sp, color = Slate600)
        }
        Column(horizontalAlignment = Alignment.End) {
            Text("$score%", fontSize = 28.sp, fontWeight = FontWeight.ExtraBold, color = accent,
                lineHeight = 30.sp)
            Surface(shape = RoundedCornerShape(20.dp), color = accent.copy(.12f)) {
                Text("মিল রয়েছে",
                    modifier = Modifier.padding(horizontal = 6.dp, vertical = 2.dp),
                    fontSize = 10.sp, fontWeight = FontWeight.Bold, color = accent)
            }
        }
        Spacer(Modifier.width(6.dp))
        IconButton(onClick = onRetry, modifier = Modifier.size(36.dp)) {
            Icon(Icons.Default.Mic, null, tint = Slate600.copy(.6f), modifier = Modifier.size(18.dp))
        }
    }
}

// ── Details card ──────────────────────────────────────────────────────────────
@Composable
private fun DetailsCard(syllable: StepSyllable, isLeft: Boolean, modifier: Modifier) {
    val accent   = if (isLeft) Blue700  else Violet700
    val stripe   = if (isLeft) Blue600  else Violet600
    val tagColor = if (isLeft) Blue100  else Violet100

    Row(
        modifier
            .shadow(2.dp, RoundedCornerShape(14.dp))
            .clip(RoundedCornerShape(14.dp))
            .background(Color.White)
    ) {
        Box(Modifier.width(4.dp).fillMaxHeight().background(stripe))
        Column(
            Modifier.padding(horizontal = 10.dp, vertical = 12.dp),
            verticalArrangement = Arrangement.spacedBy(6.dp)
        ) {
            Row(verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(5.dp)) {
                Box(Modifier.size(18.dp).background(stripe, CircleShape), Alignment.Center) {
                    Text("i", fontSize = 10.sp, color = Color.White, fontWeight = FontWeight.ExtraBold)
                }
                Text("বিস্তারিত তথ্য", fontSize = 11.sp, fontWeight = FontWeight.ExtraBold, color = accent)
            }
            HorizontalDivider(color = stripe.copy(.12f), thickness = 1.dp)
            DetailRow("অক্ষর:", syllable.letterBengali, accent)
            DetailRow("চিহ্নের নাম:", syllable.signName, accent)
            Row(verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(4.dp)) {
                Text("চিহ্নের গ্রুপ:", fontSize = 11.sp, color = accent.copy(.7f),
                    fontWeight = FontWeight.SemiBold)
                Surface(shape = RoundedCornerShape(20.dp), color = tagColor) {
                    Text(syllable.signGroup,
                        modifier = Modifier.padding(horizontal = 8.dp, vertical = 2.dp),
                        fontSize = 10.sp, fontWeight = FontWeight.Bold, color = accent)
                }
            }
        }
    }
}

@Composable
private fun DetailRow(label: String, value: String, accent: Color) {
    Row(horizontalArrangement = Arrangement.spacedBy(4.dp)) {
        Text(label, fontSize = 11.sp, color = accent.copy(.7f),
            fontWeight = FontWeight.SemiBold, modifier = Modifier.widthIn(min = 70.dp))
        Text(value, fontSize = 11.sp, color = Slate800, fontWeight = FontWeight.SemiBold,
            lineHeight = 15.sp)
    }
}

// ── Listen row ────────────────────────────────────────────────────────────────
@Composable
private fun ListenRow(left: StepSyllable, right: StepSyllable, vm: PracticeLessonViewModel) {
    Row(
        Modifier.fillMaxWidth()
            .shadow(2.dp, RoundedCornerShape(14.dp))
            .clip(RoundedCornerShape(14.dp))
            .background(Color.White)
            .padding(horizontal = 14.dp, vertical = 12.dp),
        horizontalArrangement = Arrangement.spacedBy(10.dp),
        verticalAlignment = Alignment.CenterVertically
    ) {
        Icon(Icons.Default.VolumeUp, null, tint = Slate600, modifier = Modifier.size(18.dp))
        Text("শুনুন এবং শিখুন", fontSize = 12.sp, fontWeight = FontWeight.Bold, color = Slate800)
        Spacer(Modifier.weight(1f))
        ListenChip("${left.translit} শুনুন", Blue600) { vm.playAudio(left.audioUrl) }
        ListenChip("${right.translit} শুনুন", Violet600) { vm.playAudio(right.audioUrl) }
    }
}

@Composable
private fun ListenChip(label: String, color: Color, onClick: () -> Unit) {
    Surface(onClick = onClick, shape = RoundedCornerShape(20.dp), color = color.copy(.1f)) {
        Row(
            Modifier.padding(horizontal = 10.dp, vertical = 6.dp),
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(4.dp)
        ) {
            Icon(Icons.Default.VolumeUp, null, tint = color, modifier = Modifier.size(13.dp))
            Text(label, fontSize = 11.sp, fontWeight = FontWeight.Bold, color = color)
        }
    }
}

// ── Remember card ─────────────────────────────────────────────────────────────
@Composable
private fun RememberCard(text: String) {
    Row(
        Modifier.fillMaxWidth()
            .shadow(2.dp, RoundedCornerShape(14.dp))
            .clip(RoundedCornerShape(14.dp))
            .background(Color.White)
    ) {
        Box(Modifier.width(4.dp).fillMaxHeight().background(Amber500))
        Row(Modifier.background(Amber50).padding(12.dp),
            horizontalArrangement = Arrangement.spacedBy(10.dp)) {
            Box(Modifier.size(30.dp)
                .background(Amber500, CircleShape), Alignment.Center) {
                Text("★", fontSize = 14.sp, color = Color.White, fontWeight = FontWeight.Bold)
            }
            Column {
                Text("মনে রাখবেন", fontSize = 13.sp, fontWeight = FontWeight.ExtraBold,
                    color = Color(0xFF92400E))
                Spacer(Modifier.height(2.dp))
                Text(text, fontSize = 12.sp, color = Color(0xFF78350F), lineHeight = 17.sp)
            }
        }
    }
}

// ── Prev / Next bar ───────────────────────────────────────────────────────────
@Composable
private fun PrevNextBar(s: PracticeUiState.Ready, vm: PracticeLessonViewModel) {
    Surface(
        Modifier.fillMaxWidth(),
        color          = Color.White,
        shadowElevation = 16.dp,
        tonalElevation  = 0.dp
    ) {
        Column(
            Modifier.fillMaxWidth().padding(start = 16.dp, end = 16.dp, top = 10.dp, bottom = 14.dp),
            verticalArrangement = Arrangement.spacedBy(10.dp)
        ) {
            // Step dots centered above buttons
            Row(
                Modifier.fillMaxWidth(),
                Arrangement.Center,
                Alignment.CenterVertically
            ) {
                Row(horizontalArrangement = Arrangement.spacedBy(6.dp),
                    verticalAlignment = Alignment.CenterVertically) {
                    repeat(s.step.totalSteps.coerceAtMost(8)) { i ->
                        val active = i == s.step.id - 1
                        Box(
                            Modifier.height(6.dp)
                                .animateContentSize(spring(stiffness = Spring.StiffnessMediumLow))
                                .width(if (active) 28.dp else 6.dp)
                                .background(
                                    if (active) Brush.horizontalGradient(listOf(Green600, Green400))
                                    else        Brush.horizontalGradient(listOf(Slate200, Slate200)),
                                    CircleShape
                                )
                        )
                    }
                }
            }

            // Prev / Next buttons — equal weight, side by side
            Row(
                Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.spacedBy(12.dp),
                verticalAlignment = Alignment.CenterVertically
            ) {
                // Prev — ghost style
                val prevEnabled = !s.isFirstItem
                Box(
                    Modifier
                        .weight(1f)
                        .height(48.dp)
                        .clip(RoundedCornerShape(14.dp))
                        .background(if (prevEnabled) Slate100 else Color(0xFFF1F5F9))
                        .border(1.dp, if (prevEnabled) Slate200 else Slate200, RoundedCornerShape(14.dp))
                        .clickable(enabled = prevEnabled) { vm.prev() },
                    Alignment.Center
                ) {
                    Row(verticalAlignment = Alignment.CenterVertically,
                        horizontalArrangement = Arrangement.spacedBy(4.dp)) {
                        Icon(Icons.Default.NavigateBefore, null,
                            tint = if (prevEnabled) Slate600 else Slate200,
                            modifier = Modifier.size(20.dp))
                        Text("পূর্ববর্তী",
                            fontWeight = FontWeight.Bold, fontSize = 14.sp,
                            color = if (prevEnabled) Slate600 else Slate200)
                    }
                }

                // Next — gradient fill
                Box(
                    Modifier
                        .weight(1f)
                        .height(48.dp)
                        .clip(RoundedCornerShape(14.dp))
                        .background(Brush.horizontalGradient(listOf(Green700, Green600, Green400)))
                        .clickable { vm.next() },
                    Alignment.Center
                ) {
                    Row(verticalAlignment = Alignment.CenterVertically,
                        horizontalArrangement = Arrangement.spacedBy(4.dp)) {
                        Text("পরবর্তী",
                            fontWeight = FontWeight.Bold, fontSize = 14.sp,
                            color = Color.White)
                        Icon(Icons.Default.NavigateNext, null,
                            tint = Color.White,
                            modifier = Modifier.size(20.dp))
                    }
                }
            }
        }
    }
}

// ── Completed screen ──────────────────────────────────────────────────────────
@Composable
private fun CompletedView(onRestart: () -> Unit) {
    Column(Modifier.fillMaxSize().background(Slate100)) {
        Box(
            Modifier.fillMaxWidth()
                .background(Brush.linearGradient(listOf(Green900, Green700, Green600)))
                .padding(horizontal = 18.dp, vertical = 22.dp)
        ) {
            Text("অভিনন্দন!", fontSize = 22.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
        }

        Column(
            Modifier.weight(1f).padding(32.dp),
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.Center
        ) {
            Box(
                Modifier.size(100.dp)
                    .shadow(8.dp, CircleShape)
                    .background(Brush.linearGradient(listOf(Amber100, Amber50)), CircleShape),
                Alignment.Center
            ) { Text("🏆", fontSize = 52.sp) }

            Spacer(Modifier.height(28.dp))
            Text("সব পাঠ সম্পন্ন!", fontSize = 22.sp, fontWeight = FontWeight.ExtraBold,
                color = Slate800, textAlign = TextAlign.Center)
            Spacer(Modifier.height(8.dp))
            Text("আপনি সফলভাবে সমস্ত অনুশীলন শেষ করেছেন।",
                fontSize = 14.sp, color = Slate600, textAlign = TextAlign.Center, lineHeight = 21.sp)
            Spacer(Modifier.height(36.dp))
            Button(
                onClick = onRestart, shape = RoundedCornerShape(50),
                modifier = Modifier.height(50.dp).fillMaxWidth(.65f),
                colors = ButtonDefaults.buttonColors(containerColor = Green600),
                elevation = ButtonDefaults.buttonElevation(4.dp)
            ) {
                Text("আবার শুরু করুন", fontWeight = FontWeight.Bold, fontSize = 15.sp)
            }
        }
    }
}
