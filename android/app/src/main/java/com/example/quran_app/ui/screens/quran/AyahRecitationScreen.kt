package com.example.quran_app.ui.screens.quran

import android.Manifest
import android.content.pm.PackageManager
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
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.StrokeCap
import androidx.compose.ui.hapticfeedback.HapticFeedbackType
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.platform.LocalHapticFeedback
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.core.content.ContextCompat
import com.example.quran_app.domain.model.Ayah
import com.example.quran_app.domain.model.RecitationResult
import com.example.quran_app.ui.viewmodel.AyahRecitationViewModel
import com.example.quran_app.ui.viewmodel.RecitationState

private val TealDark    = Color(0xFF134E4A)
private val TealAccent  = Color(0xFF0D9488)
private val TealMid     = Color(0xFF0F766E)
private val TealLight   = Color(0xFFF0FDFA)
private val TealBorder  = Color(0xFF99F6E4)
private val RecordRed   = Color(0xFFEF4444)
private val SuccessGreen = Color(0xFF16A34A)
private val FailRed     = Color(0xFFDC2626)

@Composable
fun AyahRecitationScreen(
    ayahId:    Int,
    viewModel: AyahRecitationViewModel,
    onBack:    () -> Unit
) {
    val ayahs           by viewModel.ayahs.collectAsState()
    val recitationState by viewModel.recitationState.collectAsState()

    val ayah = ayahs.find { it.id == ayahId }

    val context = LocalContext.current
    val haptic  = LocalHapticFeedback.current
    val permLauncher = rememberLauncherForActivityResult(
        ActivityResultContracts.RequestPermission()
    ) { granted ->
        if (granted) viewModel.startRecording()
    }

    DisposableEffect(Unit) {
        onDispose { viewModel.resetRecitation() }
    }

    Box(
        Modifier.fillMaxSize()
            .background(Brush.verticalGradient(listOf(TealDark, TealMid, TealAccent)))
    ) {
        Canvas(Modifier.fillMaxSize()) {
            drawCircle(Color.White.copy(.05f), 160.dp.toPx(), Offset(size.width * 1.1f, size.height * .08f))
            drawCircle(Color.White.copy(.04f), 100.dp.toPx(), Offset(-25.dp.toPx(), size.height * .5f))
        }

        Column(
            Modifier
                .fillMaxSize()
                .statusBarsPadding()
                .navigationBarsPadding()
        ) {
            // ── Top bar ───────────────────────────────────────────────────
            Row(
                Modifier.fillMaxWidth().padding(16.dp),
                Arrangement.spacedBy(12.dp),
                Alignment.CenterVertically
            ) {
                Box(
                    Modifier.size(42.dp).clip(CircleShape)
                        .background(Color.White.copy(.2f))
                        .border(1.dp, Color.White.copy(.3f), CircleShape)
                        .clickable { onBack() },
                    Alignment.Center
                ) {
                    Text("◀", fontSize = 14.sp, color = Color.White, fontWeight = FontWeight.Bold)
                }
                Column(Modifier.weight(1f)) {
                    Text("তেলাওয়াত অনুশীলন", fontSize = 18.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
                    ayah?.let {
                        Text("আয়াত ${it.ayahNumber}", fontSize = 12.sp, color = Color.White.copy(.75f))
                    }
                }
                Box(
                    Modifier
                        .background(Color.White.copy(.2f), RoundedCornerShape(20.dp))
                        .border(1.dp, Color.White.copy(.3f), RoundedCornerShape(20.dp))
                        .padding(horizontal = 10.dp, vertical = 5.dp)
                ) {
                    Text("🎙 AI মূল্যায়ন", fontSize = 10.sp, color = Color.White, fontWeight = FontWeight.Bold)
                }
            }

            // ── Scrollable body ───────────────────────────────────────────
            Column(
                Modifier
                    .weight(1f)
                    .fillMaxWidth()
                    .verticalScroll(rememberScrollState())
                    .padding(16.dp),
                verticalArrangement = Arrangement.spacedBy(14.dp)
            ) {
                // Ayah display card (always visible)
                ayah?.let { AyahCard(it) }

                // State-driven content
                AnimatedContent(
                    targetState   = recitationState,
                    transitionSpec = { fadeIn(tween(200)) togetherWith fadeOut(tween(150)) },
                    label         = "recitation-state"
                ) { state ->
                    when (state) {
                        is RecitationState.Idle -> RecitationIdlePanel(
                            onRecord = {
                                haptic.performHapticFeedback(HapticFeedbackType.LongPress)
                                val hasPermission = ContextCompat.checkSelfPermission(
                                    context, Manifest.permission.RECORD_AUDIO
                                ) == PackageManager.PERMISSION_GRANTED
                                if (hasPermission) viewModel.startRecording()
                                else permLauncher.launch(Manifest.permission.RECORD_AUDIO)
                            }
                        )
                        is RecitationState.Recording -> RecordingPanel(
                            onStop = {
                                haptic.performHapticFeedback(HapticFeedbackType.LongPress)
                                viewModel.stopAndAssess(ayahId)
                            }
                        )
                        is RecitationState.Processing -> ProcessingPanel()
                        is RecitationState.Result     -> ResultPanel(
                            result  = state.data,
                            onReset = { viewModel.resetRecitation() }
                        )
                        is RecitationState.Error -> ErrorPanel(
                            message = state.message,
                            onReset = { viewModel.resetRecitation() }
                        )
                    }
                }

                Spacer(Modifier.height(8.dp))
            }
        }
    }
}

// ── Ayah display ──────────────────────────────────────────────────────────────

@Composable
private fun AyahCard(ayah: Ayah) {
    Card(
        Modifier.fillMaxWidth(),
        RoundedCornerShape(20.dp),
        CardDefaults.cardColors(TealLight),
        CardDefaults.cardElevation(8.dp)
    ) {
        Column(Modifier.padding(20.dp), verticalArrangement = Arrangement.spacedBy(10.dp)) {
            Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                Box(
                    Modifier
                        .background(TealAccent.copy(.12f), RoundedCornerShape(10.dp))
                        .padding(horizontal = 10.dp, vertical = 4.dp)
                ) {
                    Text("আয়াত ${ayah.ayahNumber}", fontSize = 11.sp, color = TealAccent, fontWeight = FontWeight.Bold)
                }
                Text("📖", fontSize = 20.sp)
            }

            // Arabic text — large, right-aligned
            Text(
                ayah.arabicText,
                fontSize   = 26.sp,
                fontWeight = FontWeight.Bold,
                color      = TealDark,
                textAlign  = TextAlign.End,
                lineHeight = 44.sp,
                modifier   = Modifier.fillMaxWidth()
            )

            HorizontalDivider(color = TealBorder)

            // Transliteration
            if (ayah.transliteration.isNotBlank()) {
                Text(
                    ayah.transliteration,
                    fontSize   = 12.sp,
                    color      = TealAccent,
                    fontWeight = FontWeight.Medium,
                    lineHeight = 18.sp
                )
            }

            // Bangla translation
            Text(
                ayah.banglaTranslation,
                fontSize   = 12.sp,
                color      = Color(0xFF1E293B),
                lineHeight = 18.sp
            )
        }
    }
}

// ── Idle: big mic button ──────────────────────────────────────────────────────

@Composable
private fun RecitationIdlePanel(onRecord: () -> Unit) {
    Card(
        Modifier.fillMaxWidth(),
        RoundedCornerShape(20.dp),
        CardDefaults.cardColors(TealLight),
        CardDefaults.cardElevation(6.dp)
    ) {
        Column(
            Modifier.padding(24.dp).fillMaxWidth(),
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(16.dp)
        ) {
            Text("তেলাওয়াত শুরু করতে বোতামে চাপ দিন", fontSize = 13.sp, color = Color(0xFF64748B), textAlign = TextAlign.Center)
            Box(
                Modifier
                    .size(96.dp)
                    .background(Brush.linearGradient(listOf(TealDark, TealAccent)), CircleShape)
                    .clickable { onRecord() },
                Alignment.Center
            ) {
                Text("🎙️", fontSize = 40.sp)
            }
            Text("ট্যাপ করে শুরু করুন", fontSize = 12.sp, color = TealAccent, fontWeight = FontWeight.Bold)

            // Tip box
            Row(
                Modifier
                    .fillMaxWidth()
                    .background(TealAccent.copy(.07f), RoundedCornerShape(12.dp))
                    .border(1.dp, TealBorder, RoundedCornerShape(12.dp))
                    .padding(10.dp),
                Arrangement.spacedBy(8.dp),
                Alignment.Top
            ) {
                Text("💡", fontSize = 14.sp)
                Text(
                    "উপরের আয়াতটি ধীরে ধীরে পড়ুন। স্পষ্ট উচ্চারণ করুন। রেকর্ডিং শেষে ⏹️ বোতাম চাপুন।",
                    fontSize = 11.sp, color = TealDark, lineHeight = 17.sp
                )
            }
        }
    }
}

// ── Recording state ───────────────────────────────────────────────────────────

@Composable
private fun RecordingPanel(onStop: () -> Unit) {
    val infiniteTransition = rememberInfiniteTransition(label = "pulse")
    val scale by infiniteTransition.animateFloat(
        initialValue = 1f, targetValue = 1.2f, label = "scale",
        animationSpec = infiniteRepeatable(tween(700), RepeatMode.Reverse)
    )
    val alpha by infiniteTransition.animateFloat(
        initialValue = .35f, targetValue = .9f, label = "alpha",
        animationSpec = infiniteRepeatable(tween(700), RepeatMode.Reverse)
    )

    Card(
        Modifier.fillMaxWidth(),
        RoundedCornerShape(20.dp),
        CardDefaults.cardColors(TealLight),
        CardDefaults.cardElevation(6.dp)
    ) {
        Column(
            Modifier.padding(24.dp).fillMaxWidth(),
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(16.dp)
        ) {
            Text("রেকর্ড হচ্ছে...", fontSize = 16.sp, fontWeight = FontWeight.ExtraBold, color = RecordRed)
            Box(contentAlignment = Alignment.Center) {
                Box(
                    Modifier
                        .size(110.dp)
                        .scale(scale)
                        .background(RecordRed.copy(alpha * .2f), CircleShape)
                )
                Box(
                    Modifier
                        .size(84.dp)
                        .background(RecordRed, CircleShape)
                        .clickable { onStop() },
                    Alignment.Center
                ) {
                    Text("⏹️", fontSize = 30.sp)
                }
            }
            Text("তেলাওয়াত শেষ হলে বোতাম চাপুন", fontSize = 12.sp, color = Color(0xFF64748B), textAlign = TextAlign.Center)
        }
    }
}

// ── Processing ────────────────────────────────────────────────────────────────

@Composable
private fun ProcessingPanel() {
    Card(
        Modifier.fillMaxWidth(),
        RoundedCornerShape(20.dp),
        CardDefaults.cardColors(TealLight),
        CardDefaults.cardElevation(6.dp)
    ) {
        Column(
            Modifier.padding(32.dp).fillMaxWidth(),
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(12.dp)
        ) {
            CircularProgressIndicator(color = TealAccent, strokeWidth = 3.dp, modifier = Modifier.size(56.dp))
            Text("বিশ্লেষণ হচ্ছে...", fontSize = 15.sp, fontWeight = FontWeight.Bold, color = TealDark)
            Text("Azure AI আপনার তেলাওয়াত মূল্যায়ন করছে", fontSize = 11.sp, color = Color(0xFF64748B), textAlign = TextAlign.Center)
        }
    }
}

// ── Result ────────────────────────────────────────────────────────────────────

@Composable
private fun ResultPanel(result: RecitationResult, onReset: () -> Unit) {
    val overallScore = result.pronunciationScore
    val scoreColor = when {
        overallScore >= 80 -> SuccessGreen
        overallScore >= 55 -> TealAccent
        else               -> FailRed
    }
    val isGood = overallScore >= 55

    Column(verticalArrangement = Arrangement.spacedBy(12.dp)) {

        // ── Big result badge ──────────────────────────────────────────────
        Card(
            Modifier.fillMaxWidth(),
            RoundedCornerShape(20.dp),
            CardDefaults.cardColors(if (isGood) Color(0xFFF0FDF4) else Color(0xFFFFF1F2)),
            CardDefaults.cardElevation(6.dp)
        ) {
            Column(
                Modifier.padding(20.dp).fillMaxWidth(),
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.spacedBy(12.dp)
            ) {
                Text(
                    when {
                        overallScore >= 90 -> "🌟 চমৎকার তেলাওয়াত!"
                        overallScore >= 75 -> "✅ খুব ভালো!"
                        overallScore >= 55 -> "👍 মোটামুটি ঠিক"
                        else               -> "📚 আরও অনুশীলন করুন"
                    },
                    fontSize = 20.sp, fontWeight = FontWeight.ExtraBold, color = scoreColor
                )

                // Recognised text if available
                if (result.recognizedText.isNotBlank()) {
                    Column(
                        Modifier
                            .fillMaxWidth()
                            .background(Color.White, RoundedCornerShape(12.dp))
                            .border(1.dp, scoreColor.copy(.3f), RoundedCornerShape(12.dp))
                            .padding(12.dp),
                        verticalArrangement = Arrangement.spacedBy(4.dp)
                    ) {
                        Text("🗣 আপনি পড়েছেন", fontSize = 10.sp, color = Color(0xFF64748B), fontWeight = FontWeight.SemiBold)
                        Text(
                            result.recognizedText,
                            fontSize   = 18.sp,
                            fontWeight = FontWeight.Bold,
                            color      = TealDark,
                            textAlign  = TextAlign.End,
                            lineHeight = 30.sp,
                            modifier   = Modifier.fillMaxWidth()
                        )
                    }
                }
            }
        }

        // ── Score cards (2×2 grid) ────────────────────────────────────────
        Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(10.dp)) {
            ScoreCard("উচ্চারণ",  "${result.pronunciationScore.toInt()}",  scoreColor, Modifier.weight(1f))
            ScoreCard("নির্ভুলতা", "${result.accuracyScore.toInt()}",       scoreColor, Modifier.weight(1f))
        }
        Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(10.dp)) {
            ScoreCard("প্রবাহ",     "${result.fluencyScore.toInt()}",        TealAccent, Modifier.weight(1f))
            ScoreCard("সম্পূর্ণতা", "${result.completenessScore.toInt()}",   TealAccent, Modifier.weight(1f))
        }

        // ── Progress bars ─────────────────────────────────────────────────
        Card(
            Modifier.fillMaxWidth(),
            RoundedCornerShape(16.dp),
            CardDefaults.cardColors(TealLight),
            CardDefaults.cardElevation(4.dp)
        ) {
            Column(Modifier.padding(16.dp), verticalArrangement = Arrangement.spacedBy(10.dp)) {
                Text("📊 বিস্তারিত স্কোর", fontSize = 13.sp, fontWeight = FontWeight.Bold, color = TealDark)
                ScoreBar("উচ্চারণ",  result.pronunciationScore, scoreColor)
                ScoreBar("নির্ভুলতা", result.accuracyScore,       scoreColor)
                ScoreBar("প্রবাহ",     result.fluencyScore,        TealAccent)
                ScoreBar("সম্পূর্ণতা", result.completenessScore,   TealAccent)
            }
        }

        // ── Try again button ──────────────────────────────────────────────
        Box(
            Modifier
                .fillMaxWidth()
                .clip(RoundedCornerShape(14.dp))
                .background(Brush.linearGradient(listOf(TealDark, TealAccent)))
                .clickable { onReset() }
                .padding(vertical = 13.dp),
            Alignment.Center
        ) {
            Text("🔄 আবার তেলাওয়াত করুন", fontSize = 14.sp, fontWeight = FontWeight.Bold, color = Color.White)
        }
    }
}

@Composable
private fun ScoreCard(label: String, value: String, color: Color, modifier: Modifier = Modifier) {
    Column(
        modifier
            .background(color.copy(.08f), RoundedCornerShape(14.dp))
            .border(1.dp, color.copy(.25f), RoundedCornerShape(14.dp))
            .padding(12.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(4.dp)
    ) {
        Text("$value%", fontSize = 24.sp, fontWeight = FontWeight.ExtraBold, color = color)
        Text(label, fontSize = 10.sp, color = Color(0xFF64748B), fontWeight = FontWeight.SemiBold)
    }
}

@Composable
private fun ScoreBar(label: String, score: Double, color: Color) {
    val progress = (score / 100.0).coerceIn(0.0, 1.0).toFloat()
    val animatedProgress by animateFloatAsState(
        targetValue = progress,
        animationSpec = tween(800),
        label = "bar-$label"
    )
    Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(10.dp), Alignment.CenterVertically) {
        Text(label, fontSize = 11.sp, color = Color(0xFF64748B), fontWeight = FontWeight.SemiBold, modifier = Modifier.width(64.dp))
        Box(
            Modifier.weight(1f).height(8.dp).clip(RoundedCornerShape(4.dp)).background(color.copy(.15f))
        ) {
            Box(
                Modifier
                    .fillMaxHeight()
                    .fillMaxWidth(animatedProgress)
                    .background(Brush.horizontalGradient(listOf(TealDark, color)), RoundedCornerShape(4.dp))
            )
        }
        Text("${score.toInt()}%", fontSize = 11.sp, color = color, fontWeight = FontWeight.Bold, modifier = Modifier.width(36.dp), textAlign = TextAlign.End)
    }
}

// ── Error ─────────────────────────────────────────────────────────────────────

@Composable
private fun ErrorPanel(message: String, onReset: () -> Unit) {
    Card(
        Modifier.fillMaxWidth(),
        RoundedCornerShape(20.dp),
        CardDefaults.cardColors(Color(0xFFFFF1F2)),
        CardDefaults.cardElevation(4.dp)
    ) {
        Column(
            Modifier.padding(20.dp).fillMaxWidth(),
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(12.dp)
        ) {
            Text("⚠️", fontSize = 36.sp)
            Text(message, fontSize = 13.sp, color = FailRed, textAlign = TextAlign.Center, lineHeight = 18.sp)
            Box(
                Modifier
                    .clip(RoundedCornerShape(12.dp))
                    .background(Brush.linearGradient(listOf(TealDark, TealAccent)))
                    .clickable { onReset() }
                    .padding(horizontal = 24.dp, vertical = 10.dp)
            ) {
                Text("আবার চেষ্টা করুন", fontSize = 13.sp, color = Color.White, fontWeight = FontWeight.Bold)
            }
        }
    }
}
