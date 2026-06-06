package com.example.quran_app.ui.screens.home

import com.example.quran_app.ui.screens.leaderboard.LeaderboardScreen
import com.example.quran_app.ui.screens.profile.ProfileScreen
import com.example.quran_app.ui.screens.recite.ReciteTabContent
import com.example.quran_app.ui.viewmodel.LeaderboardViewModel
import com.example.quran_app.ui.viewmodel.ProfileViewModel
import com.example.quran_app.ui.viewmodel.ReciteViewModel

import androidx.compose.foundation.Canvas
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyRow
import androidx.compose.foundation.lazy.items
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
import androidx.compose.ui.geometry.Size
import androidx.compose.ui.graphics.*
import androidx.compose.ui.graphics.drawscope.Stroke
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.quran_app.ui.screens.letters.TanweenLessonScreen
import com.example.quran_app.ui.viewmodel.TanweenLessonViewModel

// ── Palette ───────────────────────────────────────────────────────────────────
private val AmberDark   = Color(0xFF14532D)
private val AmberMid    = Color(0xFF16A34A)
private val AmberAccent = Color(0xFF22C55E)
private val AmberLight  = Color(0xFFDCFCE7)
private val BgCream     = Color(0xFFF0FDF4)
private val TextDark    = Color(0xFF1E293B)
private val TextSlate   = Color(0xFF64748B)

// ── Screen ────────────────────────────────────────────────────────────────────

@Composable
fun HomeScreen(
    onLetterLearning:     () -> Unit,
    onTajweed:            () -> Unit,
    onNamaz:              () -> Unit,
    lessonViewModel:      TanweenLessonViewModel,
    reciteViewModel:      ReciteViewModel,
    profileViewModel:     ProfileViewModel,
    leaderboardViewModel: LeaderboardViewModel,
    onGoLogin:            () -> Unit,
    onGoRegister:         () -> Unit,
) {
    var selectedTab by remember { mutableStateOf(0) }

    Scaffold(
        containerColor = BgCream,
        bottomBar = {
            HomeBottomBar(selectedTab) { tab ->
                if (tab == 2) onNamaz() else selectedTab = tab
            }
        }
    ) { innerPadding ->
        Box(
            Modifier
                .fillMaxSize()
                .padding(bottom = innerPadding.calculateBottomPadding())
        ) {
            when (selectedTab) {
                0    -> MainHomeContent(onLetterLearning, onTajweed, onSukoonLesson = { selectedTab = 1 })
                1    -> ReciteTabContent(viewModel = reciteViewModel)
                3    -> LeaderboardScreen(viewModel = leaderboardViewModel)
                4    -> ProfileScreen(
                    viewModel    = profileViewModel,
                    onGoLogin    = onGoLogin,
                    onGoRegister = onGoRegister,
                    onBack       = { selectedTab = 0 },
                )
                else -> PlaceholderTab(label = "")
            }
        }
    }
}

// ── Main scrollable home content ──────────────────────────────────────────────

@Composable
private fun MainHomeContent(
    onLetterLearning: () -> Unit,
    onTajweed:        () -> Unit,
    onSukoonLesson:   () -> Unit,
) {
    Column(
        Modifier
            .fillMaxWidth()
            .verticalScroll(rememberScrollState())
    ) {
        AmberHeader(onLetterLearning)
        Spacer(Modifier.height(22.dp))
        LetterSection(onLetterLearning)
        Spacer(Modifier.height(20.dp))
        TajweedSection(onTajweed, onSukoonLesson)
        Spacer(Modifier.height(20.dp))
        TodayProgress()
        Spacer(Modifier.height(28.dp))
    }
}

// ── Amber header (gradient bg + profile row + hero banner) ────────────────────

@Composable
private fun AmberHeader(onLetterLearning: () -> Unit) {
    Box(
        Modifier
            .fillMaxWidth()
            .background(Brush.verticalGradient(listOf(AmberDark, AmberMid, AmberAccent)))
    ) {
        // Soft decorative circles
        Canvas(Modifier.fillMaxSize()) {
            drawCircle(Color.White.copy(.07f), 170.dp.toPx(), Offset(size.width * 1.05f, 55.dp.toPx()))
            drawCircle(Color.White.copy(.05f), 100.dp.toPx(), Offset(-22.dp.toPx(), size.height * .55f))
            drawCircle(Color.White.copy(.04f), 240.dp.toPx(), Offset(size.width * .88f, size.height * .98f))
        }

        Column(
            Modifier
                .fillMaxWidth()
                .statusBarsPadding()
                .padding(bottom = 22.dp)
        ) {
            // Profile row
            Row(
                Modifier
                    .fillMaxWidth()
                    .padding(horizontal = 20.dp, vertical = 16.dp),
                Arrangement.SpaceBetween,
                Alignment.CenterVertically
            ) {
                Row(
                    verticalAlignment = Alignment.CenterVertically,
                    horizontalArrangement = Arrangement.spacedBy(12.dp)
                ) {
                    Box(
                        Modifier
                            .size(48.dp)
                            .background(Color.White.copy(.25f), CircleShape)
                            .border(2.dp, Color.White.copy(.45f), CircleShape),
                        Alignment.Center
                    ) {
                        Text("🧑", fontSize = 26.sp)
                    }
                    Column(verticalArrangement = Arrangement.spacedBy(2.dp)) {
                        Text(
                            "আসসালামু আলাইকুম 👋",
                            fontSize = 16.sp,
                            fontWeight = FontWeight.ExtraBold,
                            color = Color.White
                        )
                        Text(
                            "আজকে নতুন কিছু শিখি 🌿",
                            fontSize = 11.sp,
                            color = Color.White.copy(.82f)
                        )
                    }
                }
                // App-grid / settings button
                Box(
                    Modifier
                        .size(42.dp)
                        .background(Color.White.copy(.18f), RoundedCornerShape(12.dp))
                        .border(1.dp, Color.White.copy(.28f), RoundedCornerShape(12.dp)),
                    Alignment.Center
                ) {
                    Text("⊞", fontSize = 20.sp, color = Color.White, fontWeight = FontWeight.Bold)
                }
            }

            // Hero banner card
            HeroBanner(onClick = onLetterLearning)
        }
    }
}

// ── Hero banner — letter learning progress ────────────────────────────────────

@Composable
private fun HeroBanner(onClick: () -> Unit) {
    Box(
        Modifier
            .fillMaxWidth()
            .padding(horizontal = 16.dp)
            .clip(RoundedCornerShape(20.dp))
            .background(Color.White.copy(.16f))
            .border(1.dp, Color.White.copy(.35f), RoundedCornerShape(20.dp))
            .clickable { onClick() }
            .padding(16.dp)
    ) {
        Row(Modifier.fillMaxWidth(), Arrangement.SpaceBetween, Alignment.CenterVertically) {

            // Left — icon + text + CTA
            Row(
                verticalAlignment = Alignment.Top,
                horizontalArrangement = Arrangement.spacedBy(14.dp),
                modifier = Modifier.weight(1f)
            ) {
                QuranBookIcon(size = 56.dp)
                Column(verticalArrangement = Arrangement.spacedBy(3.dp)) {
                    Text(
                        "বর্ণ শিক্ষা",
                        fontSize = 20.sp,
                        fontWeight = FontWeight.ExtraBold,
                        color = Color.White
                    )
                    Text(
                        "Arabic Alphabet Learning",
                        fontSize = 11.sp,
                        fontWeight = FontWeight.SemiBold,
                        color = Color.White.copy(.85f)
                    )
                    Text(
                        "আরবি হরফ পরিচয়, উচ্চারণ অনুশীলন, লেখারাজ শিখুন",
                        fontSize = 10.sp,
                        color = Color.White.copy(.75f),
                        lineHeight = 14.sp
                    )
                    Spacer(Modifier.height(10.dp))
                    // CTA
                    Box(
                        Modifier
                            .clip(RoundedCornerShape(20.dp))
                            .background(Color.White)
                            .padding(horizontal = 14.dp, vertical = 7.dp)
                    ) {
                        Row(
                            verticalAlignment = Alignment.CenterVertically,
                            horizontalArrangement = Arrangement.spacedBy(5.dp)
                        ) {
                            Text(
                                "চালিয়ে যান",
                                fontSize = 12.sp,
                                fontWeight = FontWeight.ExtraBold,
                                color = AmberMid
                            )
                            Text("→", fontSize = 13.sp, fontWeight = FontWeight.Bold, color = AmberMid)
                        }
                    }
                }
            }

            Spacer(Modifier.width(8.dp))

            // Right — progress ring
            Column(
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.spacedBy(5.dp)
            ) {
                ProgressRing(
                    progress      = 0.78f,
                    size          = 68.dp,
                    strokeWidth   = 7.dp,
                    trackColor    = Color.White.copy(.22f),
                    progressColor = Color.White
                )
                Text(
                    "অগ্রগতি",
                    fontSize = 10.sp,
                    color = Color.White.copy(.85f),
                    fontWeight = FontWeight.SemiBold
                )
            }
        }
    }
}

// ── Circular progress ring ────────────────────────────────────────────────────

@Composable
private fun ProgressRing(
    progress:      Float,
    size:          Dp,
    strokeWidth:   Dp,
    trackColor:    Color,
    progressColor: Color
) {
    Box(Modifier.size(size), Alignment.Center) {
        Canvas(Modifier.fillMaxSize()) {
            val sw      = strokeWidth.toPx()
            val inset   = sw / 2f
            val arcSize = Size(this.size.width - sw, this.size.height - sw)
            val topLeft = Offset(inset, inset)
            drawArc(trackColor,    -90f, 360f,                false, topLeft, arcSize, style = Stroke(sw, cap = StrokeCap.Round))
            if (progress > 0f)
                drawArc(progressColor, -90f, 360f * progress, false, topLeft, arcSize, style = Stroke(sw, cap = StrokeCap.Round))
        }
        Text(
            "${(progress * 100).toInt()}%",
            fontSize = 14.sp,
            fontWeight = FontWeight.ExtraBold,
            color = Color.White
        )
    }
}

// ── Quran book icon ───────────────────────────────────────────────────────────

@Composable
private fun QuranBookIcon(size: androidx.compose.ui.unit.Dp) {
    Box(
        Modifier
            .size(size)
            .background(
                Brush.verticalGradient(listOf(Color(0xFF15803D), Color(0xFF166534))),
                RoundedCornerShape(10.dp)
            )
            .border(2.dp, Color(0xFF4ADE80).copy(.5f), RoundedCornerShape(10.dp)),
        Alignment.Center
    ) {
        // Gold spine line on left edge
        Box(
            Modifier
                .width(4.dp)
                .fillMaxHeight()
                .padding(vertical = 4.dp)
                .background(Color(0xFFFBBF24).copy(.6f), RoundedCornerShape(2.dp))
                .align(Alignment.CenterStart)
        )
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(2.dp)
        ) {
            Text(
                "القرآن",
                fontSize   = (size.value * 0.22f).sp,
                fontWeight = FontWeight.Bold,
                color      = Color(0xFFFDE68A),
                textAlign  = TextAlign.Center,
            )
            Text(
                "الكريم",
                fontSize   = (size.value * 0.17f).sp,
                fontWeight = FontWeight.SemiBold,
                color      = Color(0xFFFDE68A).copy(.85f),
                textAlign  = TextAlign.Center,
            )
        }
    }
}

// ── বর্ণ শিক্ষা বিভাগ ─────────────────────────────────────────────────────────

private data class MiniCard(val emoji: String, val label: String)

@Composable
private fun LetterSection(onLetterLearning: () -> Unit) {
    val cards = listOf(
        MiniCard("📖", "বর্ণ পরিচয়\nও মুখরাজ"),
        MiniCard("🎙️", "AI উচ্চারণ\nমূল্যায়ন"),
        MiniCard("✏️", "হাতে লেখার\nঅনুশীলন"),
        MiniCard("🔄", "৪টি\nরূপভেদ")
    )

    Column(Modifier.fillMaxWidth()) {
        // Section header
        Row(
            Modifier.fillMaxWidth().padding(horizontal = 16.dp),
            Arrangement.SpaceBetween,
            Alignment.CenterVertically
        ) {
            Row(
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(8.dp)
            ) {
                Box(
                    Modifier
                        .background(AmberLight.copy(.8f), RoundedCornerShape(8.dp))
                        .padding(horizontal = 7.dp, vertical = 3.dp)
                ) {
                    Text("أ ب", fontSize = 13.sp, fontWeight = FontWeight.Bold, color = AmberDark)
                }
                Text(
                    "বর্ণ শিক্ষা বিভাগ",
                    fontSize = 16.sp,
                    fontWeight = FontWeight.ExtraBold,
                    color = TextDark
                )
            }
            Text(
                "সব দেখুন →",
                fontSize = 12.sp,
                fontWeight = FontWeight.SemiBold,
                color = AmberMid,
                modifier = Modifier.clickable { onLetterLearning() }
            )
        }

        Spacer(Modifier.height(12.dp))

        LazyRow(
            contentPadding = PaddingValues(horizontal = 16.dp),
            horizontalArrangement = Arrangement.spacedBy(10.dp)
        ) {
            items(cards) { card ->
                LetterMiniCard(card, onClick = onLetterLearning)
            }
        }
    }
}

@Composable
private fun LetterMiniCard(card: MiniCard, onClick: () -> Unit) {
    Card(
        modifier  = Modifier.width(92.dp).clickable { onClick() },
        shape     = RoundedCornerShape(16.dp),
        colors    = CardDefaults.cardColors(containerColor = Color.White),
        elevation = CardDefaults.cardElevation(4.dp)
    ) {
        Column(
            Modifier.padding(10.dp),
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(8.dp)
        ) {
            // Icon box
            Box(
                Modifier
                    .size(52.dp)
                    .background(AmberLight, RoundedCornerShape(14.dp)),
                Alignment.Center
            ) {
                Text(card.emoji, fontSize = 24.sp)
            }
            // Label
            Text(
                card.label,
                fontSize = 10.sp,
                fontWeight = FontWeight.SemiBold,
                color = TextDark,
                textAlign = TextAlign.Center,
                lineHeight = 14.sp
            )
            // Arrow button
            Box(
                Modifier
                    .size(26.dp)
                    .background(AmberMid, CircleShape),
                Alignment.Center
            ) {
                Text("→", fontSize = 12.sp, fontWeight = FontWeight.Bold, color = Color.White)
            }
        }
    }
}


// ── তাজওয়িদ বিভাগ ────────────────────────────────────────────────────────────

@Composable
private fun TajweedSection(onTajweed: () -> Unit, onSukoonLesson: () -> Unit) {
    Column(Modifier.fillMaxWidth()) {
        // Section header
        Row(
            Modifier.fillMaxWidth().padding(horizontal = 16.dp),
            Arrangement.SpaceBetween, Alignment.CenterVertically
        ) {
            Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(8.dp)) {
                Text("تجويد", fontSize = 18.sp, fontWeight = FontWeight.Bold, color = AmberMid)
                Text("তাজওয়িদ বিভাগ", fontSize = 16.sp, fontWeight = FontWeight.ExtraBold, color = TextDark)
            }
            Text(
                "শুরু করুন →",
                fontSize = 12.sp, fontWeight = FontWeight.SemiBold, color = AmberMid,
                modifier = Modifier.clickable { onTajweed() }
            )
        }

        Spacer(Modifier.height(12.dp))

        // Single banner card showing the 3 tabs inside
        Card(
            modifier  = Modifier.fillMaxWidth().padding(horizontal = 16.dp).clickable { onTajweed() },
            shape     = RoundedCornerShape(20.dp),
            colors    = CardDefaults.cardColors(Color.White),
            elevation = CardDefaults.cardElevation(4.dp)
        ) {
            Column(Modifier.padding(16.dp), verticalArrangement = Arrangement.spacedBy(12.dp)) {
                Row(
                    Modifier.fillMaxWidth(),
                    Arrangement.SpaceBetween, Alignment.CenterVertically
                ) {
                    Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(10.dp)) {
                        Box(
                            Modifier.size(44.dp).background(Color(0xFFFEF3C7), RoundedCornerShape(12.dp)),
                            Alignment.Center
                        ) { Text("تجويد", fontSize = 14.sp, fontWeight = FontWeight.Bold, color = AmberMid) }
                        Column(verticalArrangement = Arrangement.spacedBy(2.dp)) {
                            Text("তাজওয়িদ অনুশীলন", fontSize = 14.sp, fontWeight = FontWeight.ExtraBold, color = TextDark)
                            Text("হরকত ও স্বরচিহ্ন শিখুন", fontSize = 10.sp, color = TextSlate)
                        }
                    }
                    Box(
                        Modifier.size(32.dp).background(AmberMid, CircleShape),
                        Alignment.Center
                    ) { Text("→", fontSize = 14.sp, fontWeight = FontWeight.Bold, color = Color.White) }
                }

                // Tab-chip preview row
                Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(6.dp)) {
                    TajweedChip("হারাকত",   Color(0xFFDCFCE7), Color(0xFF14532D), Modifier.weight(1f))
                    TajweedChip("তানওয়িন", Color(0xFFFEF3C7), Color(0xFF78350F), Modifier.weight(1f))
                    TajweedChip("সুকূন",    Color(0xFFEEF2FF), Color(0xFF3730A3), Modifier.weight(1f), onClick = onSukoonLesson)
                    TajweedChip("শব্দ গঠন", Color(0xFFECFDF5), Color(0xFF064E3B), Modifier.weight(1f))
                }
            }
        }
    }
}

@Composable
private fun TajweedChip(
    label:    String,
    bg:       Color,
    fg:       Color,
    modifier: Modifier = Modifier,
    onClick:  (() -> Unit)? = null,
) {
    Box(
        modifier
            .background(bg, RoundedCornerShape(10.dp))
            .border(1.dp, fg.copy(.2f), RoundedCornerShape(10.dp))
            .then(if (onClick != null) Modifier.clickable { onClick() } else Modifier)
            .padding(vertical = 8.dp),
        Alignment.Center
    ) {
        Text(label, fontSize = 10.sp, fontWeight = FontWeight.ExtraBold, color = fg, textAlign = TextAlign.Center)
    }
}

// ── আজকের অগ্রগতি ─────────────────────────────────────────────────────────────

private data class StatItem(val emoji: String, val value: String, val label: String)

@Composable
private fun TodayProgress() {
    val stats = listOf(
        StatItem("🔥", "৭",    "দিন ধারাবাহিক"),
        StatItem("🎯", "৫",    "পাঠ সম্পন্ন"),
        StatItem("📖", "১২",   "মিনিট শিখেছি"),
        StatItem("⭐", "১২৫০", "পয়েন্ট অর্জন")
    )

    Column(Modifier.fillMaxWidth().padding(horizontal = 16.dp)) {
        Row(
            Modifier.fillMaxWidth(),
            Arrangement.SpaceBetween,
            Alignment.CenterVertically
        ) {
            Text(
                "আজকের অগ্রগতি",
                fontSize = 16.sp,
                fontWeight = FontWeight.ExtraBold,
                color = TextDark
            )
            Text(
                "বিস্তারিত দেখুন →",
                fontSize = 12.sp,
                fontWeight = FontWeight.SemiBold,
                color = AmberMid
            )
        }

        Spacer(Modifier.height(12.dp))

        Row(Modifier.fillMaxWidth(), Arrangement.spacedBy(8.dp)) {
            stats.forEach { stat ->
                StatCard(stat, Modifier.weight(1f))
            }
        }
    }
}

@Composable
private fun StatCard(stat: StatItem, modifier: Modifier) {
    Card(
        modifier  = modifier,
        shape     = RoundedCornerShape(14.dp),
        colors    = CardDefaults.cardColors(containerColor = Color.White),
        elevation = CardDefaults.cardElevation(3.dp)
    ) {
        Column(
            Modifier.padding(horizontal = 6.dp, vertical = 12.dp),
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.spacedBy(4.dp)
        ) {
            Text(stat.emoji, fontSize = 20.sp)
            Text(
                stat.value,
                fontSize = 17.sp,
                fontWeight = FontWeight.ExtraBold,
                color = AmberMid
            )
            Text(
                stat.label,
                fontSize = 8.sp,
                fontWeight = FontWeight.SemiBold,
                color = TextSlate,
                textAlign = TextAlign.Center,
                lineHeight = 11.sp
            )
        }
    }
}

// ── Bottom navigation bar ─────────────────────────────────────────────────────

private data class NavItem(val emoji: String, val label: String)

@Composable
private fun HomeBottomBar(selectedTab: Int, onTabSelected: (Int) -> Unit) {
    val items = listOf(
        NavItem("🏠", "হোম"),
        NavItem("🎓", "শিখুন"),
        NavItem("🕌", "নামাজ"),
        NavItem("🏆", "লিডারবোর্ড"),
        NavItem("👤", "প্রোফাইল")
    )

    Surface(
        Modifier.fillMaxWidth(),
        color          = Color.White,
        shadowElevation = 14.dp,
        tonalElevation  = 0.dp
    ) {
        Row(
            Modifier
                .fillMaxWidth()
                .navigationBarsPadding()
                .padding(horizontal = 4.dp, vertical = 6.dp),
            Arrangement.SpaceAround,
            Alignment.CenterVertically
        ) {
            items.forEachIndexed { index, item ->
                val isSelected = index == selectedTab
                Column(
                    Modifier
                        .weight(1f)
                        .clip(RoundedCornerShape(10.dp))
                        .clickable { onTabSelected(index) }
                        .padding(vertical = 6.dp),
                    horizontalAlignment = Alignment.CenterHorizontally,
                    verticalArrangement = Arrangement.spacedBy(3.dp)
                ) {
                    Text(
                        item.emoji,
                        fontSize = if (isSelected) 22.sp else 20.sp
                    )
                    Text(
                        item.label,
                        fontSize = 9.sp,
                        fontWeight = if (isSelected) FontWeight.ExtraBold else FontWeight.Normal,
                        color = if (isSelected) AmberMid else Color(0xFF94A3B8)
                    )
                    // Active dot
                    Box(
                        Modifier
                            .size(if (isSelected) 5.dp else 0.dp)
                            .background(
                                if (isSelected) AmberMid else Color.Transparent,
                                CircleShape
                            )
                    )
                }
            }
        }
    }
}

// ── Placeholder for non-home tabs ─────────────────────────────────────────────

@Composable
private fun PlaceholderTab(label: String) {
    Column(
        Modifier.fillMaxSize(),
        Arrangement.Center,
        Alignment.CenterHorizontally
    ) {
        Text("🚧", fontSize = 56.sp)
        Spacer(Modifier.height(14.dp))
        Text(
            "$label শীঘ্রই আসছে",
            fontSize = 18.sp,
            fontWeight = FontWeight.ExtraBold,
            color = TextDark
        )
        Spacer(Modifier.height(6.dp))
        Text(
            "এই বিভাগটি শীঘ্রই চালু হবে",
            fontSize = 13.sp,
            color = TextSlate,
            textAlign = TextAlign.Center
        )
    }
}
