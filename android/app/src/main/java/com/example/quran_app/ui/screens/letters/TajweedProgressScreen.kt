package com.example.quran_app.ui.screens.letters

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.horizontalScroll
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.itemsIndexed
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.quran_app.domain.model.TajweedLetterProgress
import com.example.quran_app.domain.model.TajweedModeStat
import com.example.quran_app.ui.theme.*
import com.example.quran_app.ui.viewmodel.ArabicLettersViewModel

private val AmberAccent = Color(0xFF16A34A)
private val AmberDark   = Color(0xFF14532D)
private val PassColor   = Color(0xFF16A34A)
private val FailColor   = Color(0xFFDC2626)
private val MidColor    = Color(0xFFD97706)

// Column header labels + column widths (table scrolls horizontally)
private val ColumnDefs = listOf(
    "বর্ণ"      to 64.dp,
    "হারাকত"    to 80.dp,
    "তানওয়িন"  to 80.dp,
    "সুকূন"     to 80.dp,
    "শব্দ গঠন"  to 80.dp,
)

@Composable
fun TajweedProgressScreen(
    viewModel: ArabicLettersViewModel,
    onBack:    () -> Unit,
) {
    val rows    by viewModel.tajweedProgress.collectAsState()
    val loading by viewModel.tajweedProgressLoading.collectAsState()

    LaunchedEffect(Unit) { viewModel.fetchTajweedProgress() }

    Box(
        Modifier
            .fillMaxSize()
            .background(Brush.verticalGradient(listOf(AppPrimary, AppSecondary, AppAccent, AppBgLight)))
    ) {
        Column(
            Modifier
                .fillMaxSize()
                .statusBarsPadding()
                .navigationBarsPadding()
        ) {
            // ── Header ────────────────────────────────────────────────────────
            Row(
                Modifier
                    .fillMaxWidth()
                    .padding(horizontal = 16.dp, vertical = 12.dp),
                Arrangement.spacedBy(10.dp),
                Alignment.CenterVertically
            ) {
                Box(
                    Modifier
                        .size(36.dp)
                        .background(Color.White.copy(.22f), CircleShape)
                        .border(1.dp, Color.White.copy(.3f), CircleShape)
                        .clickable { onBack() },
                    Alignment.Center
                ) { Text("◀", fontSize = 13.sp, color = Color.White, fontWeight = FontWeight.Bold) }

                Column {
                    Text("তাজওয়িদ অগ্রগতি", fontSize = 18.sp, fontWeight = FontWeight.Bold, color = Color.White)
                    Text("প্রতি বর্ণের সেরা স্কোর", fontSize = 12.sp, color = Color.White.copy(.75f))
                }
            }

            // ── Legend ────────────────────────────────────────────────────────
            Row(
                Modifier
                    .padding(horizontal = 16.dp)
                    .background(Color.White.copy(.12f), RoundedCornerShape(10.dp))
                    .padding(horizontal = 12.dp, vertical = 8.dp),
                Arrangement.spacedBy(14.dp),
                Alignment.CenterVertically
            ) {
                LegendDot(PassColor, "≥85 সঠিক")
                LegendDot(MidColor,  "60–84")
                LegendDot(FailColor, "<60")
                LegendDot(Color.White.copy(.3f), "অনুশীলন হয়নি")
            }

            Spacer(Modifier.height(10.dp))

            // ── Table ─────────────────────────────────────────────────────────
            Card(
                Modifier
                    .fillMaxSize()
                    .padding(horizontal = 12.dp),
                shape     = RoundedCornerShape(topStart = 20.dp, topEnd = 20.dp),
                colors    = CardDefaults.cardColors(Color.White),
                elevation = CardDefaults.cardElevation(8.dp)
            ) {
                if (loading && rows.isEmpty()) {
                    Box(Modifier.fillMaxSize(), Alignment.Center) {
                        CircularProgressIndicator(color = AmberAccent)
                    }
                } else if (rows.isEmpty()) {
                    Box(Modifier.fillMaxSize(), Alignment.Center) {
                        Column(horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(10.dp)) {
                            Text("📋", fontSize = 48.sp)
                            Text("এখনো কোনো অনুশীলন নেই", fontSize = 16.sp, fontWeight = FontWeight.Bold, color = AmberDark)
                            Text("তাজওয়িদ স্ক্রিনে গিয়ে অনুশীলন শুরু করুন", fontSize = 12.sp, color = SlateGrey, textAlign = TextAlign.Center)
                        }
                    }
                } else {
                    val hScroll = rememberScrollState()
                    LazyColumn {
                        item {
                            Row(Modifier.horizontalScroll(hScroll)) { TableHeader() }
                            HorizontalDivider(color = Color(0xFFE5E7EB), thickness = 1.5.dp)
                        }
                        itemsIndexed(rows) { index, row ->
                            Row(Modifier.horizontalScroll(hScroll)) { TableRow(row, isEven = index % 2 == 0) }
                            if (index < rows.lastIndex)
                                HorizontalDivider(color = Color(0xFFF3F4F6))
                        }
                        item { Spacer(Modifier.height(24.dp)) }
                    }
                }
            }
        }
    }
}

@Composable
private fun TableHeader() {
    Row(
        Modifier
            .background(Color(0xFFDCFCE7))
            .padding(vertical = 10.dp, horizontal = 8.dp),
        verticalAlignment = Alignment.CenterVertically
    ) {
        ColumnDefs.forEach { (label, width) ->
            Box(Modifier.width(width), Alignment.Center) {
                Text(label, fontSize = 11.sp, fontWeight = FontWeight.ExtraBold, color = AmberDark, textAlign = TextAlign.Center)
            }
        }
    }
}

@Composable
private fun TableRow(row: TajweedLetterProgress, isEven: Boolean) {
    Row(
        Modifier
            .background(if (isEven) Color.White else Color(0xFFF0FDF4))
            .padding(vertical = 8.dp, horizontal = 8.dp),
        verticalAlignment = Alignment.CenterVertically
    ) {
        Box(Modifier.width(64.dp), Alignment.Center) {
            Column(horizontalAlignment = Alignment.CenterHorizontally) {
                Text(row.letter, fontSize = 22.sp, fontWeight = FontWeight.Bold, color = AmberDark)
                Text(row.nameBangla, fontSize = 9.sp, color = SlateGrey, textAlign = TextAlign.Center)
            }
        }
        ScoreCell(row.harakat,       Modifier.width(80.dp))
        ScoreCell(row.tanween,       Modifier.width(80.dp))
        ScoreCell(row.sukoonShaddah, Modifier.width(80.dp))
        ScoreCell(row.wordBuilding,  Modifier.width(80.dp))
    }
}

@Composable
private fun ScoreCell(stat: TajweedModeStat?, modifier: Modifier = Modifier) {
    Box(modifier, Alignment.Center) {
        if (stat == null) {
            Box(
                Modifier
                    .size(36.dp)
                    .background(Color(0xFFF3F4F6), CircleShape)
                    .border(1.dp, Color(0xFFE5E7EB), CircleShape),
                Alignment.Center
            ) { Text("—", fontSize = 12.sp, color = Color(0xFFD1D5DB)) }
        } else {
            val color = when {
                stat.bestScore >= 85 -> PassColor
                stat.bestScore >= 60 -> MidColor
                else                 -> FailColor
            }
            Column(horizontalAlignment = Alignment.CenterHorizontally, verticalArrangement = Arrangement.spacedBy(2.dp)) {
                Box(
                    Modifier
                        .size(40.dp)
                        .background(color.copy(.12f), CircleShape)
                        .border(1.5.dp, color.copy(.4f), CircleShape),
                    Alignment.Center
                ) {
                    Text(
                        "${stat.bestScore.toInt()}%",
                        fontSize   = 10.sp,
                        fontWeight = FontWeight.ExtraBold,
                        color      = color
                    )
                }
                Text(
                    "${stat.attempts}×",
                    fontSize = 9.sp,
                    color    = SlateGrey,
                )
            }
        }
    }
}

@Composable
private fun LegendDot(color: Color, label: String) {
    Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(4.dp)) {
        Box(Modifier.size(8.dp).background(color, CircleShape))
        Text(label, fontSize = 10.sp, color = Color.White.copy(.8f))
    }
}
