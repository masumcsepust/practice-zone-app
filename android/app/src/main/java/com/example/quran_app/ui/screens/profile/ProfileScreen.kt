package com.example.quran_app.ui.screens.profile

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.*
import androidx.compose.material.icons.automirrored.filled.Login
import androidx.compose.material.icons.automirrored.filled.Logout
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.Path
import androidx.compose.ui.graphics.StrokeCap
import androidx.compose.ui.graphics.drawscope.Stroke
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import coil.compose.AsyncImage
import com.example.quran_app.domain.model.UserInfo
import com.example.quran_app.ui.theme.*
import com.example.quran_app.ui.viewmodel.ProfileState
import com.example.quran_app.ui.viewmodel.ProfileViewModel
import androidx.compose.foundation.Canvas
import kotlin.math.cos
import kotlin.math.sin

// ── Palette ───────────────────────────────────────────────────────────────────
private val HeaderDark   = Color(0xFF14532D)
private val HeaderMid    = Color(0xFF166534)
private val GreenPrimary = Color(0xFF16A34A)
private val GreenAccent  = Color(0xFF22C55E)
private val GreenLight   = Color(0xFFDCFCE7)
private val TextDark     = Color(0xFF1E293B)
private val TextGrey     = Color(0xFF64748B)
private val CardWhite    = Color(0xFFFFFFFF)
private val PageBg       = Color(0xFFF1F5F9)

// ── Dummy fallback values shown when API fields are zero/empty ─────────────────
private val DEMO_USER = UserInfo(
    id                = "demo",
    email             = "mehedi@example.com",
    role              = "User",
    displayName       = "Mehedi",
    avatarUrl         = "",
    username          = "@mehedi_123",
    totalXp           = 3200,
    currentStreak     = 17,
    totalLessons      = 28,
    correctAnswerRate = 0.89f,
    level             = 12,
    nextLevelXp       = 4000,
    memberSince       = "১২ মার্চ, ২০২৪",
)

@Composable
fun ProfileScreen(
    viewModel:    ProfileViewModel,
    onGoLogin:    () -> Unit,
    onGoRegister: () -> Unit,
    onBack:       (() -> Unit)? = null,
) {
    val state by viewModel.state.collectAsState()

    when (val s = state) {
        is ProfileState.Loading  -> Box(
            Modifier.fillMaxSize().background(PageBg),
            Alignment.Center
        ) { CircularProgressIndicator(color = GreenPrimary) }

        is ProfileState.Guest    -> GuestSection(onGoLogin, onGoRegister)

        is ProfileState.LoggedIn -> {
            // Merge real data with demo defaults for zero-value fields
            val raw  = s.user
            val user = raw.copy(
                username          = raw.username.orEmpty().ifBlank { "@${raw.displayName.orEmpty().lowercase().replace(" ", "_")}" },
                totalXp           = if (raw.totalXp == 0) DEMO_USER.totalXp else raw.totalXp,
                currentStreak     = if (raw.currentStreak == 0) DEMO_USER.currentStreak else raw.currentStreak,
                totalLessons      = if (raw.totalLessons == 0) DEMO_USER.totalLessons else raw.totalLessons,
                correctAnswerRate = if (raw.correctAnswerRate == 0f) DEMO_USER.correctAnswerRate else raw.correctAnswerRate,
                level             = if (raw.level <= 1 && raw.totalXp == 0) DEMO_USER.level else raw.level,
                nextLevelXp       = if (raw.nextLevelXp <= 400 && raw.totalXp == 0) DEMO_USER.nextLevelXp else raw.nextLevelXp,
                memberSince       = raw.memberSince.orEmpty().ifBlank { DEMO_USER.memberSince },
                avatarUrl         = raw.avatarUrl.orEmpty(),
                displayName       = raw.displayName.orEmpty().ifBlank { DEMO_USER.displayName },
            )
            LoggedInSection(user, onBack) { viewModel.logout() }
        }
    }
}

// ── Logged-in profile ─────────────────────────────────────────────────────────

@Composable
private fun LoggedInSection(user: UserInfo, onBack: (() -> Unit)?, onLogout: () -> Unit) {
    Column(
        Modifier
            .fillMaxSize()
            .background(PageBg)
            .verticalScroll(rememberScrollState())
    ) {
        // ── Green header ───────────────────────────────────────────────────
        Box(
            Modifier
                .fillMaxWidth()
                .background(Brush.verticalGradient(listOf(HeaderDark, HeaderMid)))
        ) {
            // Decorative circles
            Canvas(Modifier.fillMaxSize().height(200.dp)) {
                drawCircle(Color.White.copy(.05f), 160.dp.toPx(), center = androidx.compose.ui.geometry.Offset(size.width * 1.1f, 40.dp.toPx()))
                drawCircle(Color.White.copy(.04f), 90.dp.toPx(),  center = androidx.compose.ui.geometry.Offset(-10.dp.toPx(), size.height * .6f))
            }

            Column(
                Modifier
                    .fillMaxWidth()
                    .statusBarsPadding()
                    .padding(bottom = 24.dp)
            ) {
                // Top bar: back + title + settings
                Row(
                    Modifier
                        .fillMaxWidth()
                        .padding(horizontal = 16.dp, vertical = 16.dp),
                    Arrangement.SpaceBetween,
                    Alignment.CenterVertically
                ) {
                    // Back button
                    Box(
                        Modifier
                            .size(38.dp)
                            .background(Color.White.copy(.15f), CircleShape)
                            .border(1.dp, Color.White.copy(.3f), CircleShape)
                            .clickable { onBack?.invoke() },
                        Alignment.Center
                    ) {
                        Text("←", fontSize = 18.sp, color = Color.White, fontWeight = FontWeight.Bold)
                    }

                    Column(horizontalAlignment = Alignment.CenterHorizontally) {
                        Text(
                            "প্রোফাইল",
                            fontSize = 18.sp, fontWeight = FontWeight.ExtraBold,
                            color = Color.White
                        )
                        Row(
                            verticalAlignment = Alignment.CenterVertically,
                            horizontalArrangement = Arrangement.spacedBy(4.dp)
                        ) {
                            Text("🌿", fontSize = 11.sp)
                            Text(
                                "শিখি, অনুশীলন করি, উন্নতি করি",
                                fontSize = 11.sp, color = Color.White.copy(.75f)
                            )
                            Text("🌿", fontSize = 11.sp)
                        }
                    }

                    // Settings button
                    Box(
                        Modifier
                            .size(38.dp)
                            .background(Color.White.copy(.15f), CircleShape)
                            .border(1.dp, Color.White.copy(.3f), CircleShape),
                        Alignment.Center
                    ) {
                        Icon(
                            Icons.Default.Settings, null,
                            tint = Color.White, modifier = Modifier.size(18.dp)
                        )
                    }
                }

                // ── Profile card (overlaps the header) ────────────────────
                Card(
                    Modifier
                        .fillMaxWidth()
                        .padding(horizontal = 16.dp),
                    shape  = RoundedCornerShape(20.dp),
                    colors = CardDefaults.cardColors(containerColor = CardWhite),
                    elevation = CardDefaults.cardElevation(8.dp)
                ) {
                    Column(Modifier.padding(16.dp)) {
                        Row(
                            Modifier.fillMaxWidth(),
                            verticalAlignment = Alignment.CenterVertically,
                            horizontalArrangement = Arrangement.SpaceBetween
                        ) {
                            // Avatar
                            Box(Modifier.size(72.dp)) {
                                Box(
                                    Modifier
                                        .fillMaxSize()
                                        .clip(CircleShape)
                                        .background(GreenLight)
                                        .border(2.dp, GreenPrimary, CircleShape),
                                    Alignment.Center
                                ) {
                                    if (user.avatarUrl.orEmpty().isNotBlank()) {
                                        AsyncImage(
                                            model              = user.avatarUrl,
                                            contentDescription = "Avatar",
                                            contentScale       = ContentScale.Crop,
                                            modifier           = Modifier.fillMaxSize()
                                        )
                                    } else {
                                        Text(
                                            user.displayName.orEmpty().take(1).ifEmpty { "?" }.uppercase(),
                                            fontSize   = 28.sp,
                                            fontWeight = FontWeight.ExtraBold,
                                            color      = GreenPrimary
                                        )
                                    }
                                }
                                // Edit pencil
                                Box(
                                    Modifier
                                        .size(22.dp)
                                        .background(GreenPrimary, CircleShape)
                                        .align(Alignment.BottomEnd),
                                    Alignment.Center
                                ) {
                                    Icon(
                                        Icons.Default.Edit, null,
                                        tint = Color.White, modifier = Modifier.size(12.dp)
                                    )
                                }
                            }

                            Spacer(Modifier.width(12.dp))

                            // Name + badge + date
                            Column(Modifier.weight(1f), verticalArrangement = Arrangement.spacedBy(4.dp)) {
                                Text(
                                    "You (${user.displayName})",
                                    fontSize = 16.sp, fontWeight = FontWeight.ExtraBold,
                                    color = TextDark
                                )
                                Text(
                                    user.username,
                                    fontSize = 12.sp, color = TextGrey
                                )
                                // Role badge
                                Surface(
                                    shape = RoundedCornerShape(20.dp),
                                    color = GreenLight
                                ) {
                                    Row(
                                        Modifier.padding(horizontal = 8.dp, vertical = 3.dp),
                                        verticalAlignment = Alignment.CenterVertically,
                                        horizontalArrangement = Arrangement.spacedBy(4.dp)
                                    ) {
                                        Icon(
                                            Icons.Default.Shield, null,
                                            tint = GreenPrimary, modifier = Modifier.size(12.dp)
                                        )
                                        Text(
                                            if (user.role == "Admin") "অ্যাডমিন" else "নিয়মিত শিক্ষার্থী",
                                            fontSize = 11.sp, fontWeight = FontWeight.Bold,
                                            color = GreenPrimary
                                        )
                                    }
                                }
                                // Join date
                                Row(
                                    verticalAlignment = Alignment.CenterVertically,
                                    horizontalArrangement = Arrangement.spacedBy(4.dp)
                                ) {
                                    Icon(
                                        Icons.Default.CalendarToday, null,
                                        tint = TextGrey, modifier = Modifier.size(12.dp)
                                    )
                                    Text(
                                        "সদস্য হয়েছেন: ${user.memberSince}",
                                        fontSize = 11.sp, color = TextGrey
                                    )
                                }
                            }

                            // Level hexagon badge
                            LevelBadge(level = user.level)
                        }

                        Spacer(Modifier.height(16.dp))

                        // XP progress bar
                        val xpProgress = (user.totalXp.toFloat() / user.nextLevelXp).coerceIn(0f, 1f)
                        Row(
                            Modifier.fillMaxWidth(),
                            Arrangement.SpaceBetween,
                            Alignment.CenterVertically
                        ) {
                            Text(
                                "${user.totalXp.formatBn()} XP",
                                fontSize = 13.sp, fontWeight = FontWeight.ExtraBold,
                                color = GreenPrimary
                            )
                            Text(
                                "পরবর্তী লেভেল: ${user.nextLevelXp.formatBn()} XP",
                                fontSize = 11.sp, color = TextGrey
                            )
                        }
                        Spacer(Modifier.height(6.dp))
                        Box(
                            Modifier
                                .fillMaxWidth()
                                .height(8.dp)
                                .clip(RoundedCornerShape(4.dp))
                                .background(Color(0xFFE2E8F0))
                        ) {
                            Box(
                                Modifier
                                    .fillMaxWidth(xpProgress)
                                    .fillMaxHeight()
                                    .background(
                                        Brush.horizontalGradient(listOf(GreenPrimary, GreenAccent)),
                                        RoundedCornerShape(4.dp)
                                    )
                            )
                        }
                    }
                }
            }
        }

        Spacer(Modifier.height(16.dp))

        // ── Stats row ──────────────────────────────────────────────────────
        Card(
            Modifier
                .fillMaxWidth()
                .padding(horizontal = 16.dp),
            shape  = RoundedCornerShape(16.dp),
            colors = CardDefaults.cardColors(containerColor = CardWhite),
            elevation = CardDefaults.cardElevation(2.dp)
        ) {
            Row(
                Modifier
                    .fillMaxWidth()
                    .padding(vertical = 16.dp),
                Arrangement.SpaceEvenly
            ) {
                StatBox("📚", user.totalXp.formatBn(), "মোট XP",       Color(0xFF14B8A6))
                VerticalDivider(Modifier.height(48.dp))
                StatBox("🔥", "${user.currentStreak} দিন", "স্ট্রিক",  Color(0xFFEF4444))
                VerticalDivider(Modifier.height(48.dp))
                StatBox("🏆", user.totalLessons.toString(), "মোট পাঠ", Color(0xFF6366F1))
                VerticalDivider(Modifier.height(48.dp))
                StatBox("🎯", "${(user.correctAnswerRate * 100).toInt()}%", "সঠিক উত্তর", Color(0xFFF97316))
            }
        }

        Spacer(Modifier.height(16.dp))

        // ── Achievements ───────────────────────────────────────────────────
        Card(
            Modifier
                .fillMaxWidth()
                .padding(horizontal = 16.dp),
            shape  = RoundedCornerShape(16.dp),
            colors = CardDefaults.cardColors(containerColor = CardWhite),
            elevation = CardDefaults.cardElevation(2.dp)
        ) {
            Column(Modifier.padding(16.dp)) {
                Row(
                    Modifier.fillMaxWidth(),
                    Arrangement.SpaceBetween,
                    Alignment.CenterVertically
                ) {
                    Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(6.dp)) {
                        Text("🏅", fontSize = 18.sp)
                        Text(
                            "অর্জনসমূহ",
                            fontSize = 15.sp, fontWeight = FontWeight.ExtraBold,
                            color = TextDark
                        )
                    }
                    Text(
                        "সব দেখুন >",
                        fontSize = 12.sp, fontWeight = FontWeight.SemiBold,
                        color = GreenPrimary
                    )
                }

                Spacer(Modifier.height(12.dp))

                Row(
                    Modifier.fillMaxWidth(),
                    Arrangement.spacedBy(8.dp)
                ) {
                    AchievementBadge("📖", "প্রথম পাঠ সম্পন্ন", Color(0xFF16A34A), Modifier.weight(1f))
                    AchievementBadge("🔥", "৭ দিনের স্ট্রিক",  Color(0xFF7C3AED), Modifier.weight(1f))
                    AchievementBadge("🎙️", "উচ্চারণ অনুশীলন",  Color(0xFF2563EB), Modifier.weight(1f))
                    AchievementBadge("🎯", "১০০% সঠিক",        Color(0xFFD97706), Modifier.weight(1f))
                }
            }
        }

        Spacer(Modifier.height(16.dp))

        // ── Menu ───────────────────────────────────────────────────────────
        Card(
            Modifier
                .fillMaxWidth()
                .padding(horizontal = 16.dp),
            shape  = RoundedCornerShape(16.dp),
            colors = CardDefaults.cardColors(containerColor = CardWhite),
            elevation = CardDefaults.cardElevation(2.dp)
        ) {
            Column {
                MenuRow(Icons.Default.Person,       Color(0xFF2563EB), "ব্যক্তিগত তথ্য")
                HorizontalDivider(Modifier.padding(horizontal = 16.dp), thickness = 0.5.dp, color = Color(0xFFE2E8F0))
                MenuRow(Icons.Default.WorkspacePremium, Color(0xFFD97706), "সাবস্ক্রিপশন")
                HorizontalDivider(Modifier.padding(horizontal = 16.dp), thickness = 0.5.dp, color = Color(0xFFE2E8F0))
                MenuRow(Icons.Default.BarChart,     Color(0xFF2563EB), "আমার পারফরম্যান্স")
                HorizontalDivider(Modifier.padding(horizontal = 16.dp), thickness = 0.5.dp, color = Color(0xFFE2E8F0))
                MenuRow(Icons.Default.Bookmark,     Color(0xFF7C3AED), "সেভ করা পাঠ")
                HorizontalDivider(Modifier.padding(horizontal = 16.dp), thickness = 0.5.dp, color = Color(0xFFE2E8F0))
                MenuRow(Icons.Default.Settings,     Color(0xFF64748B), "সেটিংস")
            }
        }

        Spacer(Modifier.height(16.dp))

        // Logout
        OutlinedButton(
            onClick  = onLogout,
            modifier = Modifier
                .fillMaxWidth()
                .padding(horizontal = 16.dp)
                .height(48.dp),
            shape    = RoundedCornerShape(12.dp),
            border   = ButtonDefaults.outlinedButtonBorder(enabled = true).copy(width = 1.dp),
            colors   = ButtonDefaults.outlinedButtonColors(contentColor = Color(0xFFEF4444))
        ) {
            Icon(Icons.AutoMirrored.Filled.Logout, null, tint = Color(0xFFEF4444))
            Spacer(Modifier.width(8.dp))
            Text("লগআউট", fontWeight = FontWeight.Bold)
        }

        Spacer(Modifier.height(32.dp))
    }
}

// ── Hexagonal level badge ─────────────────────────────────────────────────────

@Composable
private fun LevelBadge(level: Int) {
    Box(Modifier.size(72.dp), Alignment.Center) {
        Canvas(Modifier.fillMaxSize()) {
            val cx      = size.width / 2f
            val cy      = size.height / 2f
            val radius  = size.minDimension / 2f - 4.dp.toPx()
            val path    = Path()
            for (i in 0..5) {
                val angle = Math.PI / 3 * i - Math.PI / 6
                val x     = (cx + radius * cos(angle)).toFloat()
                val y     = (cy + radius * sin(angle)).toFloat()
                if (i == 0) path.moveTo(x, y) else path.lineTo(x, y)
            }
            path.close()
            drawPath(path, color = Color(0xFF16A34A))
            drawPath(path, color = Color(0xFF22C55E), style = Stroke(3.dp.toPx(), cap = StrokeCap.Round))
        }
        Column(horizontalAlignment = Alignment.CenterHorizontally) {
            Text("⭐", fontSize = 14.sp)
            Text("লেভেল", fontSize = 8.sp, color = Color.White, fontWeight = FontWeight.SemiBold)
            Text(
                level.toString(),
                fontSize = 18.sp, fontWeight = FontWeight.ExtraBold,
                color = Color.White
            )
        }
    }
}

// ── Stat box ──────────────────────────────────────────────────────────────────

@Composable
private fun StatBox(emoji: String, value: String, label: String, color: Color) {
    Column(horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(2.dp)) {
        Text(emoji, fontSize = 20.sp)
        Text(value,  fontSize = 16.sp, fontWeight = FontWeight.ExtraBold, color = color)
        Text(label,  fontSize = 10.sp, color = TextGrey, textAlign = TextAlign.Center)
    }
}

// ── Achievement badge ─────────────────────────────────────────────────────────

@Composable
private fun AchievementBadge(emoji: String, label: String, color: Color, modifier: Modifier) {
    Column(
        modifier              = modifier,
        horizontalAlignment   = Alignment.CenterHorizontally,
        verticalArrangement   = Arrangement.spacedBy(6.dp)
    ) {
        // Hexagonal badge shape
        Box(Modifier.size(56.dp), Alignment.Center) {
            Canvas(Modifier.fillMaxSize()) {
                val cx     = size.width / 2f
                val cy     = size.height / 2f
                val radius = size.minDimension / 2f - 2.dp.toPx()
                val path   = Path()
                for (i in 0..5) {
                    val angle = Math.PI / 3 * i - Math.PI / 6
                    val x     = (cx + radius * cos(angle)).toFloat()
                    val y     = (cy + radius * sin(angle)).toFloat()
                    if (i == 0) path.moveTo(x, y) else path.lineTo(x, y)
                }
                path.close()
                drawPath(path, color = color)
                drawPath(path, color = Color.White.copy(.2f), style = Stroke(2.dp.toPx()))
            }
            Text(emoji, fontSize = 22.sp)
        }
        Text(
            label, fontSize = 9.sp, fontWeight = FontWeight.SemiBold,
            color = TextDark, textAlign = TextAlign.Center, lineHeight = 12.sp
        )
        Text(
            "অজিত", fontSize = 9.sp, color = GreenPrimary,
            fontWeight = FontWeight.Bold
        )
    }
}

// ── Menu row ──────────────────────────────────────────────────────────────────

@Composable
private fun MenuRow(icon: ImageVector, iconColor: Color, label: String) {
    Row(
        Modifier
            .fillMaxWidth()
            .clickable { }
            .padding(horizontal = 16.dp, vertical = 14.dp),
        verticalAlignment = Alignment.CenterVertically
    ) {
        Box(
            Modifier
                .size(36.dp)
                .background(iconColor.copy(.1f), RoundedCornerShape(10.dp)),
            Alignment.Center
        ) {
            Icon(icon, null, tint = iconColor, modifier = Modifier.size(18.dp))
        }
        Spacer(Modifier.width(12.dp))
        Text(
            label,
            Modifier.weight(1f),
            fontSize = 14.sp, fontWeight = FontWeight.Medium,
            color = TextDark
        )
        Icon(
            Icons.Default.ChevronRight, null,
            tint = Color(0xFFCBD5E1), modifier = Modifier.size(20.dp)
        )
    }
}

// ── Guest section ─────────────────────────────────────────────────────────────

@Composable
private fun GuestSection(onGoLogin: () -> Unit, onGoRegister: () -> Unit) {
    Column(
        Modifier
            .fillMaxSize()
            .background(PageBg)
            .padding(28.dp),
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Spacer(Modifier.height(60.dp))

        Box(
            Modifier.size(110.dp).clip(CircleShape).background(GreenLight),
            Alignment.Center
        ) { Text("👤", fontSize = 52.sp) }

        Spacer(Modifier.height(24.dp))
        Text("লগইন করা হয়নি", fontSize = 22.sp, fontWeight = FontWeight.ExtraBold, color = TextDark)
        Spacer(Modifier.height(8.dp))
        Text(
            "অ্যাকাউন্ট তৈরি করুন বা লগইন করুন —\nআপনার অগ্রগতি সংরক্ষিত থাকবে",
            fontSize = 14.sp, color = TextGrey, lineHeight = 21.sp,
            modifier = Modifier.padding(horizontal = 8.dp),
            textAlign = TextAlign.Center
        )

        Spacer(Modifier.height(36.dp))

        Button(
            onClick  = onGoLogin,
            modifier = Modifier.fillMaxWidth().height(50.dp),
            shape    = RoundedCornerShape(12.dp),
            colors   = ButtonDefaults.buttonColors(containerColor = GreenPrimary)
        ) {
            Icon(Icons.AutoMirrored.Filled.Login, null, tint = Color.White)
            Spacer(Modifier.width(8.dp))
            Text("লগইন করুন", fontWeight = FontWeight.Bold, fontSize = 16.sp)
        }

        Spacer(Modifier.height(12.dp))

        OutlinedButton(
            onClick  = onGoRegister,
            modifier = Modifier.fillMaxWidth().height(50.dp),
            shape    = RoundedCornerShape(12.dp),
            colors   = ButtonDefaults.outlinedButtonColors(contentColor = GreenPrimary)
        ) {
            Icon(Icons.Default.PersonAdd, null)
            Spacer(Modifier.width(8.dp))
            Text("নিবন্ধন করুন", fontWeight = FontWeight.Bold, fontSize = 16.sp)
        }
    }
}

// ── Helpers ───────────────────────────────────────────────────────────────────

private fun Int.formatBn(): String = when {
    this >= 1000 -> "${this / 1000},${"%03d".format(this % 1000)}"
    else         -> this.toString()
}
