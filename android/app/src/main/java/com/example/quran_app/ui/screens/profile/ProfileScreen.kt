package com.example.quran_app.ui.screens.profile

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.*
import androidx.compose.material3.*
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import coil.compose.AsyncImage
import com.example.quran_app.domain.model.UserInfo
import com.example.quran_app.ui.theme.*
import com.example.quran_app.ui.viewmodel.ProfileState
import com.example.quran_app.ui.viewmodel.ProfileViewModel

@Composable
fun ProfileScreen(
    viewModel:    ProfileViewModel,
    onGoLogin:    () -> Unit,
    onGoRegister: () -> Unit
) {
    val state by viewModel.state.collectAsState()

    Column(
        Modifier
            .fillMaxSize()
            .verticalScroll(rememberScrollState())
            .background(Color(0xFFF8FAFC))
    ) {
        when (val s = state) {
            is ProfileState.Loading  -> Box(Modifier.fillMaxSize(), Alignment.Center) {
                CircularProgressIndicator(color = AppPrimary)
            }
            is ProfileState.Guest    -> GuestSection(onGoLogin, onGoRegister)
            is ProfileState.LoggedIn -> LoggedInSection(s.user) { viewModel.logout() }
        }
    }
}

// ── Logged-in profile ─────────────────────────────────────────────────────────

@Composable
private fun LoggedInSection(user: UserInfo, onLogout: () -> Unit) {
    // Header
    Box(
        Modifier
            .fillMaxWidth()
            .background(Brush.verticalGradient(listOf(AppDark, AppPrimary)))
            .padding(top = 48.dp, bottom = 32.dp),
        contentAlignment = Alignment.Center
    ) {
        Column(horizontalAlignment = Alignment.CenterHorizontally) {
            // Avatar
            Box(
                Modifier
                    .size(88.dp)
                    .clip(CircleShape)
                    .background(AppAccent),
                contentAlignment = Alignment.Center
            ) {
                if (user.avatarUrl.isNotBlank()) {
                    AsyncImage(
                        model              = user.avatarUrl,
                        contentDescription = "Avatar",
                        contentScale       = ContentScale.Crop,
                        modifier           = Modifier.fillMaxSize()
                    )
                } else {
                    Text(
                        user.displayName.take(1).uppercase(),
                        fontSize   = 36.sp,
                        fontWeight = FontWeight.ExtraBold,
                        color      = AppDark
                    )
                }
            }

            Spacer(Modifier.height(12.dp))
            Text(user.displayName, fontSize = 22.sp, fontWeight = FontWeight.ExtraBold, color = Color.White)
            Text(user.email, fontSize = 13.sp, color = Color.White.copy(.75f))
            Spacer(Modifier.height(8.dp))

            // Role badge
            Surface(
                shape  = RoundedCornerShape(20.dp),
                color  = if (user.role == "Admin") Color(0xFFFFD700).copy(.2f) else AppAccent.copy(.2f)
            ) {
                Text(
                    if (user.role == "Admin") "👑 Admin" else "✅ ${user.role}",
                    modifier = Modifier.padding(horizontal = 14.dp, vertical = 4.dp),
                    fontSize = 12.sp, fontWeight = FontWeight.Bold,
                    color    = if (user.role == "Admin") Color(0xFFFFD700) else AppAccent
                )
            }
        }
    }

    Spacer(Modifier.height(20.dp))

    // Stats row
    Row(Modifier.fillMaxWidth().padding(horizontal = 20.dp), horizontalArrangement = Arrangement.spacedBy(12.dp)) {
        StatCard(Modifier.weight(1f), "⚡", "${user.totalXp}", "মোট XP", Color(0xFFFFF3CD), Color(0xFFF59E0B))
        StatCard(Modifier.weight(1f), "🔥", "${user.currentStreak}", "দিনের ধারা", Color(0xFFFFEDE8), Color(0xFFEF4444))
    }

    Spacer(Modifier.height(20.dp))

    // Info card
    Card(
        Modifier.fillMaxWidth().padding(horizontal = 20.dp),
        shape  = RoundedCornerShape(16.dp),
        colors = CardDefaults.cardColors(containerColor = Color.White),
        elevation = CardDefaults.cardElevation(2.dp)
    ) {
        Column(Modifier.padding(16.dp), verticalArrangement = Arrangement.spacedBy(12.dp)) {
            Text("অ্যাকাউন্ট তথ্য", fontWeight = FontWeight.Bold, fontSize = 15.sp, color = NavyText)
            HorizontalDivider()
            InfoRow(Icons.Default.Email,  "ইমেইল",   user.email)
            InfoRow(Icons.Default.Shield, "ভূমিকা",   user.role)
            InfoRow(Icons.Default.Person, "ব্যবহারকারী ID", user.id.take(8) + "…")
        }
    }

    Spacer(Modifier.height(24.dp))

    // Logout
    Button(
        onClick  = onLogout,
        modifier = Modifier.fillMaxWidth().padding(horizontal = 20.dp).height(50.dp),
        shape    = RoundedCornerShape(12.dp),
        colors   = ButtonDefaults.buttonColors(containerColor = Color(0xFFEF4444))
    ) {
        Icon(Icons.Default.Logout, null, tint = Color.White)
        Spacer(Modifier.width(8.dp))
        Text("লগআউট", fontWeight = FontWeight.Bold, color = Color.White)
    }

    Spacer(Modifier.height(32.dp))
}

// ── Guest section ─────────────────────────────────────────────────────────────

@Composable
private fun GuestSection(onGoLogin: () -> Unit, onGoRegister: () -> Unit) {
    Column(
        Modifier.fillMaxSize().padding(28.dp),
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Spacer(Modifier.height(60.dp))

        // Illustration
        Box(
            Modifier.size(110.dp).clip(CircleShape).background(AppBgLight),
            Alignment.Center
        ) {
            Text("👤", fontSize = 52.sp)
        }

        Spacer(Modifier.height(24.dp))
        Text("লগইন করা হয়নি", fontSize = 22.sp, fontWeight = FontWeight.ExtraBold, color = NavyText)
        Spacer(Modifier.height(8.dp))
        Text(
            "অ্যাকাউন্ট তৈরি করুন বা লগইন করুন —\nআপনার অগ্রগতি সংরক্ষিত থাকবে",
            fontSize = 14.sp, color = SlateGrey,
            lineHeight = 21.sp,
            modifier = Modifier.padding(horizontal = 8.dp),
            textAlign = androidx.compose.ui.text.style.TextAlign.Center
        )

        Spacer(Modifier.height(36.dp))

        Button(
            onClick  = onGoLogin,
            modifier = Modifier.fillMaxWidth().height(50.dp),
            shape    = RoundedCornerShape(12.dp),
            colors   = ButtonDefaults.buttonColors(containerColor = AppPrimary)
        ) {
            Icon(Icons.Default.Login, null, tint = Color.White)
            Spacer(Modifier.width(8.dp))
            Text("লগইন করুন", fontWeight = FontWeight.Bold, fontSize = 16.sp)
        }

        Spacer(Modifier.height(12.dp))

        OutlinedButton(
            onClick  = onGoRegister,
            modifier = Modifier.fillMaxWidth().height(50.dp),
            shape    = RoundedCornerShape(12.dp),
            border   = ButtonDefaults.outlinedButtonBorder.copy(width = 1.5.dp),
            colors   = ButtonDefaults.outlinedButtonColors(contentColor = AppPrimary)
        ) {
            Icon(Icons.Default.PersonAdd, null)
            Spacer(Modifier.width(8.dp))
            Text("নিবন্ধন করুন", fontWeight = FontWeight.Bold, fontSize = 16.sp)
        }

        Spacer(Modifier.height(32.dp))

        Text("গেস্ট হিসেবে ব্রাউজ করতে পারবেন,\nকিন্তু অগ্রগতি সংরক্ষিত হবে না।",
            fontSize = 12.sp, color = SlateGrey.copy(.7f),
            textAlign = androidx.compose.ui.text.style.TextAlign.Center)
    }
}

// ── Helpers ───────────────────────────────────────────────────────────────────

@Composable
private fun StatCard(
    modifier: Modifier, icon: String, value: String, label: String,
    bgColor: Color, accentColor: Color
) {
    Card(
        modifier  = modifier,
        shape     = RoundedCornerShape(14.dp),
        colors    = CardDefaults.cardColors(containerColor = bgColor),
        elevation = CardDefaults.cardElevation(0.dp)
    ) {
        Column(
            Modifier.padding(16.dp),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Text(icon, fontSize = 28.sp)
            Text(value, fontSize = 24.sp, fontWeight = FontWeight.ExtraBold, color = accentColor)
            Text(label, fontSize = 12.sp, color = accentColor.copy(.7f), fontWeight = FontWeight.Medium)
        }
    }
}

@Composable
private fun InfoRow(icon: ImageVector, label: String, value: String) {
    Row(verticalAlignment = Alignment.CenterVertically) {
        Icon(icon, null, tint = AppPrimary, modifier = Modifier.size(18.dp))
        Spacer(Modifier.width(10.dp))
        Column {
            Text(label, fontSize = 11.sp, color = SlateGrey)
            Text(value, fontSize = 14.sp, fontWeight = FontWeight.Medium, color = NavyText)
        }
    }
}
