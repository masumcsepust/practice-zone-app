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
import com.example.quran_app.domain.model.Ayah
import com.example.quran_app.ui.viewmodel.AyahRecitationViewModel

private val TealDark   = Color(0xFF134E4A)
private val TealAccent = Color(0xFF0D9488)
private val TealMid    = Color(0xFF0F766E)
private val TealLight  = Color(0xFFF0FDFA)
private val TealBorder = Color(0xFF99F6E4)

@Composable
fun AyahListScreen(
    surahId:          Int,
    surahName:        String,
    viewModel:        AyahRecitationViewModel,
    onBack:           () -> Unit,
    onAyahSelected:   (ayahId: Int) -> Unit
) {
    val ayahs     by viewModel.ayahs.collectAsState()
    val isLoading by viewModel.isLoading.collectAsState()
    val error     by viewModel.error.collectAsState()

    LaunchedEffect(surahId) { viewModel.fetchAyahs(surahId) }

    Box(
        Modifier.fillMaxSize()
            .background(Brush.verticalGradient(listOf(TealDark, TealMid, TealAccent)))
    ) {
        Canvas(Modifier.fillMaxSize()) {
            drawCircle(Color.White.copy(.05f), 150.dp.toPx(), Offset(size.width * 1.1f, size.height * .08f))
            drawCircle(Color.White.copy(.04f), 120.dp.toPx(), Offset(-20.dp.toPx(), size.height * .4f))
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
                    Text(surahName, fontSize = 20.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
                    Text("আয়াত নির্বাচন করুন", fontSize = 12.sp, color = Color.White.copy(.75f))
                }
                if (ayahs.isNotEmpty()) {
                    Box(
                        Modifier
                            .background(Color.White.copy(.2f), RoundedCornerShape(20.dp))
                            .border(1.dp, Color.White.copy(.3f), RoundedCornerShape(20.dp))
                            .padding(horizontal = 12.dp, vertical = 6.dp)
                    ) {
                        Text("${ayahs.size} আয়াত", fontSize = 11.sp, color = Color.White, fontWeight = FontWeight.Bold)
                    }
                }
            }

            // ── Content ───────────────────────────────────────────────────
            when {
                isLoading -> Box(Modifier.fillMaxSize(), Alignment.Center) {
                    CircularProgressIndicator(color = Color.White, strokeWidth = 3.dp, modifier = Modifier.size(56.dp))
                }
                error != null -> Column(
                    Modifier.fillMaxSize(), Arrangement.Center, Alignment.CenterHorizontally
                ) {
                    Text("📡", fontSize = 48.sp)
                    Spacer(Modifier.height(12.dp))
                    Text(error ?: "", color = Color.White, fontSize = 14.sp, textAlign = TextAlign.Center, modifier = Modifier.padding(horizontal = 32.dp))
                    Spacer(Modifier.height(16.dp))
                    Box(
                        Modifier.clip(RoundedCornerShape(12.dp)).background(Color.White.copy(.2f)).clickable { viewModel.fetchAyahs(surahId) }.padding(horizontal = 24.dp, vertical = 10.dp)
                    ) {
                        Text("আবার চেষ্টা করুন", color = Color.White, fontWeight = FontWeight.Bold)
                    }
                }
                else -> LazyColumn(
                    Modifier.fillMaxSize(),
                    contentPadding = PaddingValues(horizontal = 16.dp, vertical = 8.dp),
                    verticalArrangement = Arrangement.spacedBy(10.dp)
                ) {
                    itemsIndexed(ayahs) { _, ayah ->
                        AyahCard(ayah, onClick = { onAyahSelected(ayah.id) })
                    }
                    item { Spacer(Modifier.height(12.dp)) }
                }
            }
        }
    }
}

@Composable
private fun AyahCard(ayah: Ayah, onClick: () -> Unit) {
    Card(
        modifier  = Modifier.fillMaxWidth().clickable { onClick() },
        shape     = RoundedCornerShape(16.dp),
        colors    = CardDefaults.cardColors(containerColor = TealLight),
        elevation = CardDefaults.cardElevation(4.dp)
    ) {
        Column(Modifier.padding(14.dp), verticalArrangement = Arrangement.spacedBy(8.dp)) {

            // Header: ayah number + recite button
            Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                Box(
                    Modifier
                        .size(36.dp)
                        .background(Brush.linearGradient(listOf(TealDark, TealAccent)), CircleShape),
                    Alignment.Center
                ) {
                    Text("${ayah.ayahNumber}", fontSize = 12.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
                }
                Box(
                    Modifier
                        .clip(RoundedCornerShape(10.dp))
                        .background(Brush.linearGradient(listOf(TealDark, TealAccent)))
                        .clickable { onClick() }
                        .padding(horizontal = 12.dp, vertical = 6.dp)
                ) {
                    Text("🎙️ তেলাওয়াত", fontSize = 11.sp, color = Color.White, fontWeight = FontWeight.Bold)
                }
            }

            // Arabic text (right-to-left, large)
            Text(
                ayah.arabicText,
                fontSize   = 22.sp,
                fontWeight = FontWeight.Bold,
                color      = TealDark,
                textAlign  = TextAlign.End,
                lineHeight = 38.sp,
                modifier   = Modifier.fillMaxWidth()
            )

            // Transliteration
            if (ayah.transliteration.isNotBlank()) {
                Text(
                    ayah.transliteration,
                    fontSize  = 11.sp,
                    color     = TealAccent,
                    fontWeight = FontWeight.Medium
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
