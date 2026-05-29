package com.example.quran_app.ui.screens.quran

import androidx.compose.foundation.Canvas
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.itemsIndexed
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.quran_app.domain.model.Surah
import com.example.quran_app.ui.viewmodel.AyahRecitationViewModel

private val TealDark   = Color(0xFF134E4A)
private val TealAccent = Color(0xFF0D9488)
private val TealMid    = Color(0xFF0F766E)
private val TealLight  = Color(0xFFF0FDFA)
private val TealBorder = Color(0xFF99F6E4)

@Composable
fun SurahListScreen(
    viewModel:        AyahRecitationViewModel,
    onBack:           () -> Unit,
    onSurahSelected:  (surahId: Int, surahName: String) -> Unit
) {
    val surahs    by viewModel.surahs.collectAsState()
    val isLoading by viewModel.isLoading.collectAsState()
    val error     by viewModel.error.collectAsState()

    LaunchedEffect(Unit) { viewModel.fetchSurahs() }

    Box(
        Modifier.fillMaxSize()
            .background(Brush.verticalGradient(listOf(TealDark, TealMid, TealAccent)))
    ) {
        Canvas(Modifier.fillMaxSize()) {
            drawCircle(Color.White.copy(.05f), 160.dp.toPx(), Offset(size.width * 1.1f, size.height * .08f))
            drawCircle(Color.White.copy(.04f), 110.dp.toPx(), Offset(-25.dp.toPx(), size.height * .32f))
            drawCircle(Color.White.copy(.03f), 200.dp.toPx(), Offset(size.width * .9f, size.height * .75f))
        }

        Column(Modifier.fillMaxSize().statusBarsPadding().navigationBarsPadding()) {

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
                    Text("আয়াত তেলাওয়াত", fontSize = 20.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
                    Text("সূরা নির্বাচন করুন", fontSize = 12.sp, color = Color.White.copy(.75f))
                }
                if (surahs.isNotEmpty()) {
                    Box(
                        Modifier
                            .background(Color.White.copy(.2f), RoundedCornerShape(20.dp))
                            .border(1.dp, Color.White.copy(.3f), RoundedCornerShape(20.dp))
                            .padding(horizontal = 12.dp, vertical = 6.dp)
                    ) {
                        Text("📖 ${surahs.size} সূরা", fontSize = 11.sp, color = Color.White, fontWeight = FontWeight.Bold)
                    }
                }
            }

            // ── Content ───────────────────────────────────────────────────
            when {
                isLoading -> Box(Modifier.fillMaxSize(), Alignment.Center) {
                    CircularProgressIndicator(color = Color.White, strokeWidth = 3.dp, modifier = Modifier.size(56.dp))
                }
                error != null -> Column(
                    Modifier.fillMaxSize(),
                    Arrangement.Center,
                    Alignment.CenterHorizontally
                ) {
                    Text("📡", fontSize = 48.sp)
                    Spacer(Modifier.height(12.dp))
                    Text(error ?: "", color = Color.White, fontSize = 14.sp, textAlign = TextAlign.Center, modifier = Modifier.padding(horizontal = 32.dp))
                    Spacer(Modifier.height(16.dp))
                    Box(
                        Modifier.clip(RoundedCornerShape(12.dp)).background(Color.White.copy(.2f)).clickable { viewModel.fetchSurahs() }.padding(horizontal = 24.dp, vertical = 10.dp)
                    ) {
                        Text("আবার চেষ্টা করুন", color = Color.White, fontWeight = FontWeight.Bold)
                    }
                }
                else -> LazyColumn(
                    Modifier.fillMaxSize(),
                    contentPadding = PaddingValues(horizontal = 16.dp, vertical = 8.dp),
                    verticalArrangement = Arrangement.spacedBy(10.dp)
                ) {
                    itemsIndexed(surahs) { _, surah ->
                        SurahCard(
                            surah   = surah,
                            onClick = { onSurahSelected(surah.id, surah.nameBangla) }
                        )
                    }
                    item { Spacer(Modifier.height(12.dp)) }
                }
            }
        }
    }
}

@Composable
private fun SurahCard(surah: Surah, onClick: () -> Unit) {
    Card(
        modifier  = Modifier.fillMaxWidth().clickable { onClick() },
        shape     = RoundedCornerShape(16.dp),
        colors    = CardDefaults.cardColors(containerColor = TealLight),
        elevation = CardDefaults.cardElevation(4.dp)
    ) {
        Row(
            Modifier.padding(14.dp),
            Arrangement.spacedBy(14.dp),
            Alignment.CenterVertically
        ) {
            // Number badge
            Box(
                Modifier
                    .size(46.dp)
                    .background(
                        Brush.linearGradient(listOf(TealDark, TealAccent)),
                        RoundedCornerShape(14.dp)
                    ),
                Alignment.Center
            ) {
                Text(
                    "${surah.surahNumber}",
                    fontSize = 15.sp,
                    fontWeight = FontWeight.ExtraBold,
                    color = Color.White
                )
            }

            // Names
            Column(Modifier.weight(1f), verticalArrangement = Arrangement.spacedBy(2.dp)) {
                Text(surah.nameArabic,  fontSize = 18.sp, fontWeight = FontWeight.ExtraBold, color = TealDark)
                Text(surah.nameBangla, fontSize = 13.sp, fontWeight = FontWeight.SemiBold,   color = TealAccent)
                Text(surah.nameEnglish, fontSize = 11.sp, color = Color(0xFF64748B))
            }

            // Ayah count
            Column(horizontalAlignment = Alignment.CenterHorizontally) {
                Text(
                    "${surah.totalAyahs}",
                    fontSize = 20.sp,
                    fontWeight = FontWeight.ExtraBold,
                    color = TealDark
                )
                Text("আয়াত", fontSize = 9.sp, color = TealAccent, fontWeight = FontWeight.SemiBold)
            }

            Text("▶", fontSize = 14.sp, color = TealAccent, fontWeight = FontWeight.Bold)
        }
    }
}
