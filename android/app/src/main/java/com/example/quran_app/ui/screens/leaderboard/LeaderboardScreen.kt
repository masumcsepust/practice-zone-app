package com.example.quran_app.ui.screens.leaderboard

import androidx.compose.foundation.Canvas
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.graphics.*
import androidx.compose.ui.graphics.drawscope.Stroke
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.quran_app.domain.model.LeaderboardEntry
import com.example.quran_app.domain.model.LeaderboardMyPosition
import com.example.quran_app.ui.viewmodel.LeaderboardState
import com.example.quran_app.ui.viewmodel.LeaderboardViewModel

// ── Palette ───────────────────────────────────────────────────────────────────
private val GreenDark   = Color(0xFF14532D)
private val GreenMid    = Color(0xFF16A34A)
private val GreenAccent = Color(0xFF22C55E)
private val GreenLight  = Color(0xFFDCFCE7)
private val BgCream     = Color(0xFFF0FDF4)
private val TextDark    = Color(0xFF1E293B)
private val TextSlate   = Color(0xFF64748B)

private val GoldBg      = Color(0xFFFEF9C3)
private val GoldBorder  = Color(0xFFFBBF24)
private val SilverBg    = Color(0xFFF8FAFC)
private val SilverBorder = Color(0xFFCBD5E1)
private val BronzeBg    = Color(0xFFFFF7ED)
private val BronzeBorder = Color(0xFFF97316)

@Composable
fun LeaderboardScreen(viewModel: LeaderboardViewModel) {
    val state by viewModel.state.collectAsState()

    Box(
        Modifier
            .fillMaxSize()
            .background(BgCream)
    ) {
        Column(Modifier.fillMaxSize()) {
            LeaderboardHeader()

            when (val s = state) {
                is LeaderboardState.Loading -> {
                    Box(Modifier.weight(1f).fillMaxWidth(), Alignment.Center) {
                        CircularProgressIndicator(color = GreenMid)
                    }
                }

                is LeaderboardState.Error -> {
                    Box(Modifier.weight(1f).fillMaxWidth(), Alignment.Center) {
                        Column(horizontalAlignment = Alignment.CenterHorizontally) {
                            Text("⚠️", fontSize = 40.sp)
                            Spacer(Modifier.height(8.dp))
                            Text("লোড করতে সমস্যা হয়েছে", fontWeight = FontWeight.Bold, color = TextDark)
                            Spacer(Modifier.height(8.dp))
                            Button(
                                onClick = { viewModel.load("all") },
                                colors  = ButtonDefaults.buttonColors(containerColor = GreenMid)
                            ) { Text("আবার চেষ্টা করুন") }
                        }
                    }
                }

                is LeaderboardState.Success -> {
                    PeriodTabRow(s.period) { viewModel.load(it) }

                    Box(Modifier.weight(1f)) {
                        LazyColumn(
                            contentPadding        = PaddingValues(bottom = if (s.data.myPosition != null) 120.dp else 16.dp),
                            verticalArrangement   = Arrangement.spacedBy(0.dp),
                            modifier              = Modifier.fillMaxSize()
                        ) {
                            // White rounded card wrapping entire list
                            item {
                                Spacer(Modifier.height(8.dp))
                                Box(
                                    Modifier
                                        .fillMaxWidth()
                                        .padding(horizontal = 12.dp)
                                        .clip(RoundedCornerShape(topStart = 20.dp, topEnd = 20.dp))
                                        .background(Color.White)
                                ) {
                                    Column {
                                        s.data.entries.take(3).forEachIndexed { idx, entry ->
                                            TopThreeRow(entry, idx + 1)
                                            if (idx < 2) Divider(color = Color(0xFFF1F5F9), thickness = 1.dp)
                                        }
                                    }
                                }
                            }

                            // Ranks 4+
                            items(s.data.entries.drop(3)) { entry ->
                                Box(
                                    Modifier
                                        .fillMaxWidth()
                                        .padding(horizontal = 12.dp)
                                        .background(Color.White)
                                ) {
                                    RegularRankRow(entry)
                                    Divider(
                                        Modifier.align(Alignment.BottomCenter).padding(horizontal = 16.dp),
                                        color = Color(0xFFF1F5F9),
                                        thickness = 1.dp
                                    )
                                }
                            }

                            // Close the rounded card at the bottom
                            item {
                                Box(
                                    Modifier
                                        .fillMaxWidth()
                                        .padding(horizontal = 12.dp)
                                        .clip(RoundedCornerShape(bottomStart = 20.dp, bottomEnd = 20.dp))
                                        .background(Color.White)
                                        .height(12.dp)
                                )
                            }
                        }

                        // Sticky "My Position" card at the bottom
                        s.data.myPosition?.let { pos ->
                            MyPositionCard(
                                pos,
                                Modifier.align(Alignment.BottomCenter)
                            )
                        }
                    }
                }
            }
        }
    }
}

// ── Header ─────────────────────────────────────────────────────────────────────

@Composable
private fun LeaderboardHeader() {
    Box(
        Modifier
            .fillMaxWidth()
            .background(Brush.verticalGradient(listOf(GreenDark, GreenMid)))
    ) {
        Canvas(Modifier.fillMaxWidth().height(140.dp)) {
            drawCircle(Color.White.copy(.07f), 160.dp.toPx(), Offset(size.width * 1.1f, 40.dp.toPx()))
            drawCircle(Color.White.copy(.05f), 90.dp.toPx(),  Offset(-20.dp.toPx(), size.height * .6f))
        }
        Column(
            Modifier
                .fillMaxWidth()
                .statusBarsPadding()
                .padding(bottom = 20.dp),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Spacer(Modifier.height(12.dp))
            // Title row
            Box(Modifier.fillMaxWidth().padding(horizontal = 16.dp)) {
                // Trophy button (right)
                Box(
                    Modifier
                        .size(40.dp)
                        .background(Color.White.copy(.18f), CircleShape)
                        .border(1.dp, Color.White.copy(.3f), CircleShape)
                        .align(Alignment.CenterEnd),
                    Alignment.Center
                ) { Text("🏆", fontSize = 20.sp) }

                // Centered title
                Column(
                    Modifier.align(Alignment.Center),
                    horizontalAlignment = Alignment.CenterHorizontally
                ) {
                    Text(
                        "লিডারবোর্ড",
                        fontSize    = 24.sp,
                        fontWeight  = FontWeight.ExtraBold,
                        color       = Color.White
                    )
                }
            }
            Spacer(Modifier.height(6.dp))
            Row(
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(6.dp)
            ) {
                Text("🌿", fontSize = 14.sp)
                Text(
                    "সব সময়ের সেরা শিক্ষার্থীরা",
                    fontSize   = 12.sp,
                    color      = Color.White.copy(.85f),
                    fontWeight = FontWeight.SemiBold
                )
                Text("🌿", fontSize = 14.sp)
            }
        }
    }
}

// ── Period tab row ─────────────────────────────────────────────────────────────

@Composable
private fun PeriodTabRow(activePeriod: String, onSelect: (String) -> Unit) {
    val tabs = listOf(
        Triple("all",   "🌐", "সব সময়"),
        Triple("week",  "📅", "সপ্তাহ"),
        Triple("month", "📊", "মাস")
    )

    Surface(
        Modifier.fillMaxWidth(),
        color           = Color.White,
        shadowElevation = 2.dp
    ) {
        Row(
            Modifier
                .fillMaxWidth()
                .padding(horizontal = 12.dp, vertical = 10.dp),
            horizontalArrangement = Arrangement.spacedBy(8.dp)
        ) {
            tabs.forEach { (period, icon, label) ->
                val active = period == activePeriod
                Box(
                    Modifier
                        .weight(1f)
                        .clip(RoundedCornerShape(50.dp))
                        .background(if (active) GreenDark else Color.Transparent)
                        .border(
                            1.dp,
                            if (active) Color.Transparent else Color(0xFFE2E8F0),
                            RoundedCornerShape(50.dp)
                        )
                        .clickable { onSelect(period) }
                        .padding(vertical = 9.dp),
                    Alignment.Center
                ) {
                    Row(
                        verticalAlignment = Alignment.CenterVertically,
                        horizontalArrangement = Arrangement.spacedBy(5.dp)
                    ) {
                        Text(icon, fontSize = 13.sp)
                        Text(
                            label,
                            fontSize   = 12.sp,
                            fontWeight = if (active) FontWeight.ExtraBold else FontWeight.Normal,
                            color      = if (active) Color.White else TextSlate
                        )
                    }
                }
            }
        }
    }
}

// ── Top 3 row ──────────────────────────────────────────────────────────────────

@Composable
private fun TopThreeRow(entry: LeaderboardEntry, rank: Int) {
    val (bg, border, medalEmoji) = when (rank) {
        1    -> Triple(GoldBg,   GoldBorder,   "🥇")
        2    -> Triple(SilverBg, SilverBorder, "🥈")
        else -> Triple(BronzeBg, BronzeBorder, "🥉")
    }

    Box(
        Modifier
            .fillMaxWidth()
            .background(bg)
            .border(
                width  = if (rank == 1) 1.5.dp else 0.dp,
                color  = border,
                shape  = RoundedCornerShape(0.dp)
            )
            .padding(horizontal = 16.dp, vertical = 14.dp)
    ) {
        Row(
            Modifier.fillMaxWidth(),
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.SpaceBetween
        ) {
            Row(
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(12.dp)
            ) {
                // Medal badge
                Text(medalEmoji, fontSize = 32.sp)

                // Avatar circle
                AvatarCircle(size = 46.dp, isTop3 = true)

                // Name
                Text(
                    entry.displayName,
                    fontSize   = if (rank == 1) 18.sp else 16.sp,
                    fontWeight = FontWeight.ExtraBold,
                    color      = TextDark
                )
            }

            // XP badge
            Row(
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(4.dp)
            ) {
                Text("⭐", fontSize = 16.sp)
                Text(
                    "${formatXp(entry.xp)} XP",
                    fontSize   = 14.sp,
                    fontWeight = FontWeight.ExtraBold,
                    color      = Color(0xFFB45309)
                )
            }
        }
    }
}

// ── Regular rank row ───────────────────────────────────────────────────────────

@Composable
private fun RegularRankRow(entry: LeaderboardEntry) {
    Row(
        Modifier
            .fillMaxWidth()
            .padding(horizontal = 16.dp, vertical = 12.dp),
        verticalAlignment = Alignment.CenterVertically,
        horizontalArrangement = Arrangement.SpaceBetween
    ) {
        Row(
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(14.dp)
        ) {
            // Rank number
            Text(
                "${entry.rank}",
                fontSize   = 15.sp,
                fontWeight = FontWeight.Bold,
                color      = TextSlate,
                modifier   = Modifier.width(24.dp),
                textAlign  = TextAlign.Center
            )

            AvatarCircle(size = 40.dp, isTop3 = false)

            Text(
                entry.displayName,
                fontSize   = 15.sp,
                fontWeight = FontWeight.SemiBold,
                color      = TextDark
            )
        }

        Row(
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.spacedBy(4.dp)
        ) {
            Text("⭐", fontSize = 14.sp)
            Text(
                "${formatXp(entry.xp)} XP",
                fontSize   = 13.sp,
                fontWeight = FontWeight.Bold,
                color      = Color(0xFFB45309)
            )
        }
    }
}

// ── My position sticky card ────────────────────────────────────────────────────

@Composable
private fun MyPositionCard(pos: LeaderboardMyPosition, modifier: Modifier = Modifier) {
    Surface(
        modifier        = modifier.fillMaxWidth(),
        color           = Color.White,
        shadowElevation = 12.dp
    ) {
        Column(
            Modifier
                .fillMaxWidth()
                .navigationBarsPadding()
                .padding(horizontal = 16.dp, vertical = 12.dp)
        ) {
            Row(
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(6.dp)
            ) {
                Text("👤", fontSize = 13.sp)
                Text(
                    "আপনার অবস্থান",
                    fontSize   = 13.sp,
                    fontWeight = FontWeight.ExtraBold,
                    color      = TextDark
                )
            }

            Spacer(Modifier.height(10.dp))

            Row(
                Modifier.fillMaxWidth(),
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.SpaceBetween
            ) {
                Row(
                    verticalAlignment = Alignment.CenterVertically,
                    horizontalArrangement = Arrangement.spacedBy(10.dp)
                ) {
                    // Rank number
                    Text(
                        "${pos.rank}",
                        fontSize   = 28.sp,
                        fontWeight = FontWeight.ExtraBold,
                        color      = GreenMid
                    )

                    AvatarCircle(size = 44.dp, isTop3 = false)

                    Column(verticalArrangement = Arrangement.spacedBy(2.dp)) {
                        Text(
                            pos.displayName,
                            fontSize   = 14.sp,
                            fontWeight = FontWeight.ExtraBold,
                            color      = TextDark
                        )
                        Row(
                            verticalAlignment = Alignment.CenterVertically,
                            horizontalArrangement = Arrangement.spacedBy(3.dp)
                        ) {
                            Text("⭐", fontSize = 12.sp)
                            Text(
                                "${formatXp(pos.xp)} XP",
                                fontSize   = 12.sp,
                                fontWeight = FontWeight.Bold,
                                color      = Color(0xFFB45309)
                            )
                        }
                    }
                }

                // Next rank info
                if (pos.xpToNextRank > 0) {
                    Column(horizontalAlignment = Alignment.End) {
                        Text(
                            "পরবর্তী র্যাংকের জন্য প্রয়োজন",
                            fontSize = 9.sp,
                            color    = TextSlate
                        )
                        Text(
                            "${pos.xpToNextRank} XP",
                            fontSize   = 14.sp,
                            fontWeight = FontWeight.ExtraBold,
                            color      = GreenMid
                        )
                        Spacer(Modifier.height(4.dp))
                        // Progress bar (visual only — shows partial fill)
                        val fraction = (pos.xp.toFloat() / (pos.xp + pos.xpToNextRank)).coerceIn(0f, 1f)
                        XpProgressBar(fraction, Modifier.width(100.dp))
                    }
                }
            }

            Spacer(Modifier.height(8.dp))

            // Info footer
            Row(
                verticalAlignment = Alignment.CenterVertically,
                horizontalArrangement = Arrangement.spacedBy(4.dp)
            ) {
                Box(
                    Modifier
                        .size(16.dp)
                        .background(Color(0xFFEFF6FF), CircleShape),
                    Alignment.Center
                ) { Text("ℹ", fontSize = 9.sp, color = Color(0xFF3B82F6)) }
                Text(
                    "র্যাংকে প্রতি ৫০ XP পর পর আপডেট হয়।",
                    fontSize = 10.sp,
                    color    = TextSlate
                )
            }
        }
    }
}

// ── XP Progress bar ────────────────────────────────────────────────────────────

@Composable
private fun XpProgressBar(fraction: Float, modifier: Modifier = Modifier) {
    Box(
        modifier
            .height(6.dp)
            .clip(RoundedCornerShape(3.dp))
            .background(Color(0xFFE2E8F0))
    ) {
        Box(
            Modifier
                .fillMaxHeight()
                .fillMaxWidth(fraction)
                .background(
                    Brush.horizontalGradient(listOf(GreenMid, GreenAccent)),
                    RoundedCornerShape(3.dp)
                )
        )
    }
}

// ── Avatar circle ──────────────────────────────────────────────────────────────

@Composable
private fun AvatarCircle(size: androidx.compose.ui.unit.Dp, isTop3: Boolean) {
    Box(
        Modifier
            .size(size)
            .background(Color(0xFFE2E8F0), CircleShape)
            .border(
                width  = if (isTop3) 2.dp else 1.dp,
                color  = if (isTop3) GreenAccent.copy(.5f) else Color(0xFFCBD5E1),
                shape  = CircleShape
            ),
        Alignment.Center
    ) {
        Text("👤", fontSize = (size.value * 0.5f).sp)
    }
}

// ── Helpers ────────────────────────────────────────────────────────────────────

private fun formatXp(xp: Int): String =
    if (xp >= 1000) "${xp / 1000},${(xp % 1000).toString().padStart(3, '0')}"
    else xp.toString()
