package com.example.quran_app.ui.screens.learn

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.MenuBook
import androidx.compose.material.icons.filled.Mic
import androidx.compose.material.icons.filled.NavigateBefore
import androidx.compose.material.icons.filled.NavigateNext
import androidx.compose.material.icons.filled.VolumeUp
import androidx.compose.material3.*
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.quran_app.domain.model.SyllableInfo
import com.example.quran_app.ui.theme.*
import com.example.quran_app.ui.viewmodel.PracticeUiState
import com.example.quran_app.ui.viewmodel.PracticeLessonViewModel

// ── Colour tokens ─────────────────────────────────────────────────────────────
private val TargetBg      = Color(0xFFEEF2FF)
private val TargetBorder  = Color(0xFFC7D2FE)
private val TargetMic     = Color(0xFF1E3A8A)
private val CompareBg     = Color(0xFFF5F0FF)
private val CompareBorder = Color(0xFFDDD6FE)
private val CompareMic    = Color(0xFF6D28D9)
private val TipBg         = Color(0xFFF0FFF4)
private val TipBorder     = Color(0xFFBBF7D0)
private val RememberBg    = Color(0xFFFEF9C3)
private val RememberBorder= Color(0xFFFDE68A)
private val DetailsBg     = Color(0xFFF1F5FF)

@Composable
fun PracticeLessonScreen(viewModel: PracticeLessonViewModel) {
    val state by viewModel.state.collectAsState()

    when (val s = state) {
        is PracticeUiState.Loading -> Box(Modifier.fillMaxSize(), Alignment.Center) {
            CircularProgressIndicator(color = AppPrimary)
        }
        is PracticeUiState.Error   -> Box(Modifier.fillMaxSize(), Alignment.Center) {
            Column(horizontalAlignment = Alignment.CenterHorizontally) {
                Text(s.message, color = SlateGrey, textAlign = TextAlign.Center)
                Spacer(Modifier.height(16.dp))
                Button(onClick = { viewModel.load() }, colors = ButtonDefaults.buttonColors(AppPrimary)) {
                    Text("আবার চেষ্টা করুন")
                }
            }
        }
        is PracticeUiState.Ready   -> ReadyContent(s, viewModel)
    }
}

@Composable
private fun ReadyContent(s: PracticeUiState.Ready, vm: PracticeLessonViewModel) {
    val lesson = s.lesson
    val item   = s.item

    // Global lesson position across all lessons
    val globalItem  = s.lessons.take(s.lessonIndex).sumOf { it.practiceItems.size } + s.itemIndex + 1
    val globalTotal = s.lessons.sumOf { it.practiceItems.size }
    val progress    = globalItem.toFloat() / globalTotal.coerceAtLeast(1)

    Column(Modifier.fillMaxSize()) {

        // ── Green header ──────────────────────────────────────────────────────
        Column(
            Modifier
                .fillMaxWidth()
                .background(Brush.verticalGradient(listOf(AppDark, AppPrimary)))
                .padding(horizontal = 20.dp, vertical = 16.dp)
        ) {
            Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.Top) {
                Column(Modifier.weight(1f)) {
                    Text(lesson.title, fontSize = 22.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
                    Text(
                        "(Lesson ${s.lessonIndex + 1} of ${s.totalLessons.coerceAtLeast(28)})",
                        fontSize = 13.sp, color = Color.White.copy(.75f)
                    )
                }
                Icon(Icons.Default.MenuBook, null, tint = Color.White,
                    modifier = Modifier.size(36.dp))
            }
            Spacer(Modifier.height(10.dp))
            // Progress bar
            Row(Modifier.fillMaxWidth(), verticalAlignment = Alignment.CenterVertically) {
                Box(Modifier.weight(1f).height(7.dp).clip(RoundedCornerShape(50))) {
                    Box(Modifier.fillMaxSize().background(Color.White.copy(.25f)))
                    Box(Modifier.fillMaxHeight().fillMaxWidth(progress)
                        .background(Brush.horizontalGradient(listOf(AppAccent, Color(0xFF4ADE80)))))
                }
                Spacer(Modifier.width(10.dp))
                Text("${(progress * 100).toInt()}% Complete",
                    fontSize = 11.sp, fontWeight = FontWeight.Bold, color = Color.White)
            }
        }

        // ── Scrollable body ───────────────────────────────────────────────────
        Column(
            Modifier
                .weight(1f)
                .verticalScroll(rememberScrollState())
                .background(Color(0xFFF8FAFC))
                .padding(16.dp),
            verticalArrangement = Arrangement.spacedBy(14.dp)
        ) {
            // Step + instruction
            Row(verticalAlignment = Alignment.Top, horizontalArrangement = Arrangement.spacedBy(12.dp)) {
                Box(
                    Modifier.size(34.dp).background(AppPrimary, CircleShape),
                    Alignment.Center
                ) {
                    Text("${s.itemIndex + 1}", fontSize = 15.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
                }
                Text(item.instruction, fontSize = 16.sp, fontWeight = FontWeight.Bold,
                    color = NavyText, modifier = Modifier.padding(top = 6.dp))
            }

            // Success tip
            if (item.successTip.isNotBlank()) {
                Row(
                    Modifier.fillMaxWidth()
                        .clip(RoundedCornerShape(10.dp))
                        .background(TipBg)
                        .border(1.dp, TipBorder, RoundedCornerShape(10.dp))
                        .padding(12.dp),
                    horizontalArrangement = Arrangement.spacedBy(10.dp)
                ) {
                    Text("💡", fontSize = 18.sp)
                    Column {
                        Text("সাফল্যের টিপস:", fontSize = 13.sp, fontWeight = FontWeight.Bold, color = AppDark)
                        Text(item.successTip, fontSize = 13.sp, color = Color(0xFF166534), lineHeight = 19.sp)
                    }
                }
            }

            // Syllable cards
            val compare = item.compareWithSyllable
            if (compare != null) {
                // Side-by-side with vs
                Row(
                    Modifier.fillMaxWidth(),
                    horizontalArrangement = Arrangement.spacedBy(8.dp),
                    verticalAlignment = Alignment.CenterVertically
                ) {
                    SyllableCard(
                        syllable    = item.targetSyllable,
                        bg          = TargetBg,
                        border      = TargetBorder,
                        micColor    = TargetMic,
                        subtitle    = "আপনার উচ্চারণ রেকর্ড করুন",
                        modifier    = Modifier.weight(1f),
                        onPlay      = { vm.playAudio(item.targetSyllable.audioUrl) }
                    )
                    Box(
                        Modifier.size(36.dp).background(Color.White, CircleShape)
                            .border(1.dp, Color(0xFFE2E8F0), CircleShape),
                        Alignment.Center
                    ) {
                        Text("vs", fontSize = 11.sp, fontWeight = FontWeight.Bold, color = SlateGrey)
                    }
                    SyllableCard(
                        syllable    = compare,
                        bg          = CompareBg,
                        border      = CompareBorder,
                        micColor    = CompareMic,
                        subtitle    = "তুলনা করতে রেকর্ড করুন",
                        modifier    = Modifier.weight(1f),
                        onPlay      = { vm.playAudio(compare.audioUrl) }
                    )
                }

                // Details side by side
                Row(Modifier.fillMaxWidth(), horizontalArrangement = Arrangement.spacedBy(8.dp)) {
                    DetailsCard(item.targetSyllable, Modifier.weight(1f))
                    DetailsCard(compare,             Modifier.weight(1f))
                }

                // Listen & Learn
                ListenSection(item.targetSyllable, compare, vm)

            } else {
                // Single syllable
                SyllableCard(
                    syllable    = item.targetSyllable,
                    bg          = TargetBg,
                    border      = TargetBorder,
                    micColor    = TargetMic,
                    subtitle    = "আপনার উচ্চারণ রেকর্ড করুন",
                    modifier    = Modifier.fillMaxWidth(),
                    onPlay      = { vm.playAudio(item.targetSyllable.audioUrl) }
                )
                DetailsCard(item.targetSyllable, Modifier.fillMaxWidth())
            }

            // Remember tip
            if (item.successTip.isNotBlank()) {
                Row(
                    Modifier.fillMaxWidth()
                        .clip(RoundedCornerShape(10.dp))
                        .background(RememberBg)
                        .border(1.dp, RememberBorder, RoundedCornerShape(10.dp))
                        .padding(12.dp),
                    horizontalArrangement = Arrangement.spacedBy(10.dp)
                ) {
                    Text("⭐", fontSize = 18.sp)
                    Column {
                        Text("মেনে রাখবেন", fontSize = 13.sp, fontWeight = FontWeight.Bold, color = Color(0xFF92400E))
                        Text(item.successTip, fontSize = 13.sp, color = Color(0xFF78350F), lineHeight = 19.sp)
                    }
                }
            }

            Spacer(Modifier.height(4.dp))
        }

        // ── Bottom navigation ─────────────────────────────────────────────────
        Surface(color = Color.White, shadowElevation = 8.dp) {
            Row(
                Modifier.fillMaxWidth().padding(horizontal = 16.dp, vertical = 12.dp)
                    .navigationBarsPadding(),
                Arrangement.SpaceBetween
            ) {
                OutlinedButton(
                    onClick  = { vm.prev() },
                    enabled  = !s.isFirstItem,
                    shape    = RoundedCornerShape(12.dp),
                    modifier = Modifier.height(48.dp),
                    colors   = ButtonDefaults.outlinedButtonColors(contentColor = SlateGrey)
                ) {
                    Icon(Icons.Default.NavigateBefore, null)
                    Spacer(Modifier.width(4.dp))
                    Text("পূর্ববর্তী", fontWeight = FontWeight.Bold)
                }

                Button(
                    onClick  = { vm.next() },
                    enabled  = !s.isLastItem,
                    shape    = RoundedCornerShape(12.dp),
                    modifier = Modifier.height(48.dp),
                    colors   = ButtonDefaults.buttonColors(containerColor = AppDark)
                ) {
                    Text("পরবর্তী", fontWeight = FontWeight.Bold)
                    Spacer(Modifier.width(4.dp))
                    Icon(Icons.Default.NavigateNext, null)
                }
            }
        }
    }
}

// ── Syllable card ─────────────────────────────────────────────────────────────
@Composable
private fun SyllableCard(
    syllable: SyllableInfo,
    bg: Color, border: Color, micColor: Color,
    subtitle: String, modifier: Modifier,
    onPlay: () -> Unit
) {
    Column(
        modifier
            .clip(RoundedCornerShape(14.dp))
            .background(bg)
            .border(1.dp, border, RoundedCornerShape(14.dp))
            .padding(12.dp),
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Text(syllable.combinedCharacter, fontSize = 42.sp, fontWeight = FontWeight.Bold,
            color = if (micColor == TargetMic) TargetMic else CompareMic)
        Spacer(Modifier.height(4.dp))
        Text(syllable.transliteration, fontSize = 20.sp, fontWeight = FontWeight.ExtraBold,
            color = if (micColor == TargetMic) TargetMic else CompareMic)
        if (syllable.transliterationBn.isNotBlank()) {
            Text("(${syllable.transliterationBn})", fontSize = 13.sp, color = SlateGrey)
        }
        Spacer(Modifier.height(10.dp))
        Box(
            Modifier.size(44.dp).background(micColor, CircleShape),
            Alignment.Center
        ) {
            Icon(Icons.Default.Mic, null, tint = Color.White, modifier = Modifier.size(22.dp))
        }
        Spacer(Modifier.height(6.dp))
        Text(subtitle, fontSize = 11.sp, color = SlateGrey, textAlign = TextAlign.Center,
            lineHeight = 15.sp)
    }
}

// ── Details card ──────────────────────────────────────────────────────────────
@Composable
private fun DetailsCard(syllable: SyllableInfo, modifier: Modifier) {
    Column(
        modifier
            .clip(RoundedCornerShape(10.dp))
            .background(DetailsBg)
            .border(1.dp, Color(0xFFDBEAFE), RoundedCornerShape(10.dp))
            .padding(10.dp),
        verticalArrangement = Arrangement.spacedBy(4.dp)
    ) {
        Row(verticalAlignment = Alignment.CenterVertically) {
            Box(
                Modifier.size(18.dp).background(AppPrimary, CircleShape),
                Alignment.Center
            ) { Text("ℹ", fontSize = 10.sp, color = Color.White, fontWeight = FontWeight.Bold) }
            Spacer(Modifier.width(6.dp))
            Text("বিস্তারিত তথ্য", fontSize = 11.sp, fontWeight = FontWeight.Bold, color = NavyText)
        }
        HorizontalDivider(color = Color(0xFFBFDBFE), thickness = 0.5.dp)
        DetailRow("অক্ষর:",      syllable.details.letterName)
        DetailRow("চিহ্নের নাম:", syllable.details.signName)
        Row(verticalAlignment = Alignment.CenterVertically) {
            Text("চিহ্নের গ্রুপ:", fontSize = 11.sp, color = SlateGrey, fontWeight = FontWeight.Medium)
            Spacer(Modifier.width(4.dp))
            Surface(
                shape  = RoundedCornerShape(20.dp),
                color  = if (syllable.details.signGroup == "Tanween")
                             Color(0xFFEDE9FE) else Color(0xFFDCFCE7)
            ) {
                Text(
                    syllable.details.signGroup,
                    modifier = Modifier.padding(horizontal = 8.dp, vertical = 2.dp),
                    fontSize = 10.sp, fontWeight = FontWeight.Bold,
                    color    = if (syllable.details.signGroup == "Tanween")
                                   Color(0xFF5B21B6) else Color(0xFF166534)
                )
            }
        }
    }
}

@Composable
private fun DetailRow(label: String, value: String) {
    Row {
        Text(label, fontSize = 11.sp, color = SlateGrey, fontWeight = FontWeight.Medium,
            modifier = Modifier.width(80.dp))
        Text(value, fontSize = 12.sp, color = NavyText, fontWeight = FontWeight.SemiBold)
    }
}

// ── Listen section ────────────────────────────────────────────────────────────
@Composable
private fun ListenSection(target: SyllableInfo, compare: SyllableInfo, vm: PracticeLessonViewModel) {
    Column(
        Modifier.fillMaxWidth()
            .clip(RoundedCornerShape(12.dp))
            .background(Color.White)
            .border(1.dp, Color(0xFFE2E8F0), RoundedCornerShape(12.dp))
            .padding(12.dp),
        verticalArrangement = Arrangement.spacedBy(10.dp)
    ) {
        Row(verticalAlignment = Alignment.CenterVertically) {
            Text("🎧", fontSize = 18.sp)
            Spacer(Modifier.width(8.dp))
            Text("শুনুন এবং শিখুন", fontSize = 14.sp, fontWeight = FontWeight.ExtraBold, color = NavyText)
        }
        Row(Modifier.fillMaxWidth(), horizontalArrangement = Arrangement.spacedBy(8.dp)) {
            OutlinedButton(
                onClick  = { vm.playAudio(target.audioUrl) },
                modifier = Modifier.weight(1f),
                shape    = RoundedCornerShape(10.dp),
                colors   = ButtonDefaults.outlinedButtonColors(contentColor = TargetMic),
                border   = ButtonDefaults.outlinedButtonBorder.copy(width = 1.dp)
            ) {
                Icon(Icons.Default.VolumeUp, null, modifier = Modifier.size(16.dp))
                Spacer(Modifier.width(4.dp))
                Text("${target.transliteration} শুনুন", fontSize = 13.sp, fontWeight = FontWeight.Bold)
            }
            OutlinedButton(
                onClick  = { vm.playAudio(compare.audioUrl) },
                modifier = Modifier.weight(1f),
                shape    = RoundedCornerShape(10.dp),
                colors   = ButtonDefaults.outlinedButtonColors(contentColor = CompareMic),
                border   = ButtonDefaults.outlinedButtonBorder.copy(width = 1.dp)
            ) {
                Icon(Icons.Default.VolumeUp, null, modifier = Modifier.size(16.dp))
                Spacer(Modifier.width(4.dp))
                Text("${compare.transliteration} শুনুন", fontSize = 13.sp, fontWeight = FontWeight.Bold)
            }
        }
    }
}
