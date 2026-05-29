package com.example.quran_app.ui.screens.home

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
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.quran_app.ui.theme.*

// ── Letter Learning palette (Amber) ──────────────────────────────────────────
private val LetterDark   = Color(0xFF92400E)
private val LetterAccent = Color(0xFFD97706)
private val LetterLight  = Color(0xFFFFFBEB)

// ── Ayah Recitation palette (Teal) ───────────────────────────────────────────
private val AyahDark     = Color(0xFF134E4A)
private val AyahAccent   = Color(0xFF0D9488)
private val AyahLight    = Color(0xFFF0FDFA)

@Composable
fun HomeScreen(
    onLetterLearning: () -> Unit,
    onAyahRecitation: () -> Unit
) {
    Box(
        Modifier.fillMaxSize()
            .background(Brush.verticalGradient(listOf(AppPrimary, AppSecondary, AppAccent, AppBgLight)))
    ) {
        // Soft decorative circles
        Canvas(Modifier.fillMaxSize()) {
            drawCircle(Color.White.copy(.07f), 160.dp.toPx(), Offset(size.width * 1.1f, size.height * .08f))
            drawCircle(Color.White.copy(.05f), 110.dp.toPx(), Offset(-30.dp.toPx(), size.height * .28f))
            drawCircle(Color.White.copy(.04f), 180.dp.toPx(), Offset(size.width * .85f, size.height * .75f))
            drawCircle(Color.White.copy(.03f),  90.dp.toPx(), Offset(size.width * .2f,  size.height * .65f))
        }

        Column(
            Modifier
                .fillMaxSize()
                .statusBarsPadding()
                .navigationBarsPadding()
                .verticalScroll(rememberScrollState()),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Spacer(Modifier.height(28.dp))

            // ── App header ────────────────────────────────────────────────
            Column(
                Modifier.fillMaxWidth().padding(horizontal = 24.dp),
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.spacedBy(6.dp)
            ) {
                // Arabic calligraphy badge
                Box(
                    Modifier
                        .background(Color.White.copy(.2f), RoundedCornerShape(20.dp))
                        .border(1.dp, Color.White.copy(.35f), RoundedCornerShape(20.dp))
                        .padding(horizontal = 20.dp, vertical = 8.dp)
                ) {
                    Text(
                        "بِسْمِ اللَّهِ الرَّحْمَٰنِ الرَّحِيمِ",
                        fontSize = 18.sp,
                        fontWeight = FontWeight.Bold,
                        color = Color.White,
                        textAlign = TextAlign.Center
                    )
                }
                Spacer(Modifier.height(4.dp))
                Text(
                    "কোরআন শিক্ষা",
                    fontSize = 28.sp,
                    fontWeight = FontWeight.ExtraBold,
                    color = Color.White
                )
                Text(
                    "আপনার কোরআন শিক্ষার আধুনিক সাথী",
                    fontSize = 13.sp,
                    color = Color.White.copy(.82f),
                    textAlign = TextAlign.Center
                )
            }

            Spacer(Modifier.height(36.dp))

            // ── Section heading ───────────────────────────────────────────
            Row(
                Modifier.fillMaxWidth().padding(horizontal = 20.dp),
                Arrangement.SpaceBetween,
                Alignment.CenterVertically
            ) {
                Text(
                    "কী শিখতে চান?",
                    fontSize = 15.sp,
                    fontWeight = FontWeight.Bold,
                    color = Color.White
                )
                Box(
                    Modifier
                        .background(Color.White.copy(.18f), RoundedCornerShape(20.dp))
                        .padding(horizontal = 10.dp, vertical = 4.dp)
                ) {
                    Text("২ বিভাগ", fontSize = 10.sp, color = Color.White, fontWeight = FontWeight.SemiBold)
                }
            }

            Spacer(Modifier.height(14.dp))

            // ── Letter Learning card ──────────────────────────────────────
            FeatureCard(
                emoji        = "🔤",
                badge        = "২৮টি বর্ণ",
                title        = "বর্ণ শিক্ষা",
                subtitle     = "Arabic Alphabet Learning",
                description  = "আরবি হরফ পরিচয়, উচ্চারণ অনুশীলন, লেখার পদ্ধতি ও মাখরাজ শিখুন",
                features     = listOf(
                    "📖 বর্ণ পরিচয় ও মাখরাজ",
                    "🎙️ AI উচ্চারণ মূল্যায়ন",
                    "✏️ হাতে লেখার অনুশীলন",
                    "🔄 ৪টি রূপভেদ"
                ),
                darkColor    = LetterDark,
                accentColor  = LetterAccent,
                lightColor   = LetterLight,
                onClick      = onLetterLearning,
                modifier     = Modifier.padding(horizontal = 16.dp)
            )

            Spacer(Modifier.height(16.dp))

            // ── Ayah Recitation card ──────────────────────────────────────
            FeatureCard(
                emoji        = "📖",
                badge        = "১১৪ সূরা",
                title        = "আয়াত তেলাওয়াত",
                subtitle     = "Quran Recitation Practice",
                description  = "কোরআনের যেকোনো সূরার আয়াত তেলাওয়াত করুন এবং Azure AI দিয়ে উচ্চারণ যাচাই করুন",
                features     = listOf(
                    "📜 সকল সূরার তালিকা",
                    "🤖 AI উচ্চারণ বিশ্লেষণ",
                    "📊 বিস্তারিত স্কোর কার্ড",
                    "🌍 বাংলা অনুবাদ সহ"
                ),
                darkColor    = AyahDark,
                accentColor  = AyahAccent,
                lightColor   = AyahLight,
                onClick      = onAyahRecitation,
                modifier     = Modifier.padding(horizontal = 16.dp)
            )

            Spacer(Modifier.height(32.dp))

            // ── Footer ────────────────────────────────────────────────────
            Column(
                Modifier.fillMaxWidth().padding(horizontal = 24.dp),
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.spacedBy(6.dp)
            ) {
                HorizontalDivider(color = Color.White.copy(.2f))
                Spacer(Modifier.height(4.dp))
                Text(
                    "🕌 ইসলামিক শিক্ষার আধুনিক প্রযুক্তি",
                    fontSize = 12.sp,
                    color = Color.White.copy(.65f),
                    textAlign = TextAlign.Center
                )
                Text(
                    "Powered by Azure AI · Speech SDK · Pronunciation Assessment",
                    fontSize = 10.sp,
                    color = Color.White.copy(.42f),
                    textAlign = TextAlign.Center
                )
            }

            Spacer(Modifier.height(28.dp))
        }
    }
}

// ── Feature card ──────────────────────────────────────────────────────────────

@Composable
private fun FeatureCard(
    emoji:       String,
    badge:       String,
    title:       String,
    subtitle:    String,
    description: String,
    features:    List<String>,
    darkColor:   Color,
    accentColor: Color,
    lightColor:  Color,
    onClick:     () -> Unit,
    modifier:    Modifier = Modifier
) {
    Card(
        modifier  = modifier.fillMaxWidth().clickable { onClick() },
        shape     = RoundedCornerShape(24.dp),
        colors    = CardDefaults.cardColors(containerColor = lightColor),
        elevation = CardDefaults.cardElevation(10.dp)
    ) {
        Column(Modifier.padding(20.dp)) {

            // Header row: icon + title/subtitle + badge
            Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {
                Row(
                    verticalAlignment = Alignment.CenterVertically,
                    horizontalArrangement = Arrangement.spacedBy(14.dp)
                ) {
                    Box(
                        Modifier
                            .size(58.dp)
                            .background(
                                Brush.linearGradient(listOf(darkColor, accentColor)),
                                RoundedCornerShape(18.dp)
                            ),
                        Alignment.Center
                    ) {
                        Text(emoji, fontSize = 26.sp)
                    }
                    Column(verticalArrangement = Arrangement.spacedBy(2.dp)) {
                        Text(title,    fontSize = 21.sp, fontWeight = FontWeight.ExtraBold, color = darkColor)
                        Text(subtitle, fontSize = 11.sp, fontWeight = FontWeight.SemiBold,  color = accentColor)
                    }
                }
                Box(
                    Modifier
                        .background(accentColor.copy(.13f), RoundedCornerShape(20.dp))
                        .padding(horizontal = 10.dp, vertical = 5.dp)
                ) {
                    Text(badge, fontSize = 10.sp, color = accentColor, fontWeight = FontWeight.Bold)
                }
            }

            Spacer(Modifier.height(14.dp))

            // Description
            Text(
                description,
                fontSize   = 13.sp,
                color      = Color(0xFF1E293B),
                lineHeight = 20.sp
            )

            Spacer(Modifier.height(14.dp))

            // Feature chips — 2 per row
            Column(verticalArrangement = Arrangement.spacedBy(7.dp)) {
                features.chunked(2).forEach { row ->
                    Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(8.dp)) {
                        row.forEach { feat ->
                            Text(
                                feat,
                                modifier = Modifier
                                    .weight(1f)
                                    .background(accentColor.copy(.07f), RoundedCornerShape(9.dp))
                                    .padding(horizontal = 8.dp, vertical = 6.dp),
                                fontSize   = 11.sp,
                                color      = darkColor,
                                fontWeight = FontWeight.SemiBold
                            )
                        }
                        if (row.size == 1) Spacer(Modifier.weight(1f))
                    }
                }
            }

            Spacer(Modifier.height(18.dp))

            // CTA button
            Box(
                Modifier
                    .fillMaxWidth()
                    .clip(RoundedCornerShape(14.dp))
                    .background(Brush.linearGradient(listOf(darkColor, accentColor)))
                    .clickable { onClick() }
                    .padding(vertical = 13.dp),
                Alignment.Center
            ) {
                Row(
                    verticalAlignment = Alignment.CenterVertically,
                    horizontalArrangement = Arrangement.spacedBy(8.dp)
                ) {
                    Text("শুরু করুন", fontSize = 14.sp, fontWeight = FontWeight.Bold, color = Color.White)
                    Text("→", fontSize = 16.sp, fontWeight = FontWeight.Bold, color = Color.White)
                }
            }
        }
    }
}
