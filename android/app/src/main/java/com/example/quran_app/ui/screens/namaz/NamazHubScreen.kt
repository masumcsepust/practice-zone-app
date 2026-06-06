package com.example.quran_app.ui.screens.namaz

import android.widget.Toast
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.quran_app.domain.model.DailyPrayer
import com.example.quran_app.domain.model.DAILY_PRAYERS
import com.example.quran_app.ui.theme.Rc
import com.example.quran_app.ui.theme.RcType
import com.example.quran_app.ui.viewmodel.NamazHubViewModel

@Composable
fun NamazHubScreen(
    viewModel: NamazHubViewModel,
    onBack: () -> Unit,
    onStartLesson: (step: Int) -> Unit,
) {
    val prayers by viewModel.prayers.collectAsState()
    NamazHubContent(
        prayers = prayers,
        onBack = onBack,
        onStartLesson = onStartLesson,
    )
}

@Composable
private fun NamazHubContent(
    prayers: List<DailyPrayer>,
    onBack: () -> Unit,
    onStartLesson: (step: Int) -> Unit,
) {
    val learnedCount = prayers.count { it.isLearned }
    val context = LocalContext.current

    Column(
        Modifier
            .fillMaxSize()
            .background(Rc.Parchment)
            .verticalScroll(rememberScrollState())
    ) {
        // ── Emerald header ─────────────────────────────────────────────────────
        Box(
            Modifier
                .fillMaxWidth()
                .background(
                    Brush.verticalGradient(listOf(Rc.Emerald, Rc.EmeraldDeep))
                )
        ) {
            Column(
                Modifier
                    .fillMaxWidth()
                    .statusBarsPadding()
            ) {
                // Top bar
                Row(
                    Modifier
                        .fillMaxWidth()
                        .padding(horizontal = 16.dp, vertical = 14.dp),
                    Arrangement.SpaceBetween,
                    Alignment.CenterVertically,
                ) {
                    Box(
                        Modifier
                            .size(40.dp)
                            .background(Color.White.copy(.15f), RoundedCornerShape(12.dp))
                            .border(1.dp, Color.White.copy(.2f), RoundedCornerShape(12.dp))
                            .clickable { onBack() },
                        Alignment.Center,
                    ) {
                        Text("‹", fontSize = 22.sp, color = Color.White, fontWeight = FontWeight.Bold)
                    }

                    Text(
                        "Namaz",
                        style = RcType.BodyBold.copy(color = Color.White, fontSize = 18.sp),
                    )

                    Box(
                        Modifier
                            .size(40.dp)
                            .background(Color.White.copy(.15f), RoundedCornerShape(12.dp))
                            .border(1.dp, Color.White.copy(.2f), RoundedCornerShape(12.dp)),
                        Alignment.Center,
                    ) {
                        Text("🕐", fontSize = 20.sp)
                    }
                }

                // Headline
                Column(
                    Modifier.padding(horizontal = 20.dp, vertical = 4.dp),
                    verticalArrangement = Arrangement.spacedBy(6.dp),
                ) {
                    Text(
                        "Learn to pray",
                        style = RcType.Display.copy(color = Color.White, fontSize = 28.sp),
                    )
                    Text(
                        "Master each of the five daily prayers, the right way.",
                        style = RcType.Body.copy(
                            color = Color.White.copy(.78f),
                            lineHeight = 20.sp,
                        ),
                    )
                }

                Spacer(Modifier.height(20.dp))

                // Course entry card
                Row(
                    Modifier
                        .fillMaxWidth()
                        .padding(horizontal = 16.dp)
                        .clip(RoundedCornerShape(16.dp))
                        .background(Rc.EmeraldDeep)
                        .border(1.5.dp, Rc.Gold, RoundedCornerShape(16.dp))
                        .clickable { onStartLesson(1) }
                        .padding(16.dp),
                    Arrangement.SpaceBetween,
                    Alignment.CenterVertically,
                ) {
                    Row(
                        verticalAlignment = Alignment.CenterVertically,
                        horizontalArrangement = Arrangement.spacedBy(14.dp),
                        modifier = Modifier.weight(1f),
                    ) {
                        Box(
                            Modifier
                                .size(52.dp)
                                .background(Rc.Gold.copy(.18f), CircleShape)
                                .border(1.5.dp, Rc.Gold, CircleShape),
                            Alignment.Center,
                        ) {
                            Text("🕌", fontSize = 26.sp)
                        }

                        Column(verticalArrangement = Arrangement.spacedBy(4.dp)) {
                            Text(
                                "START HERE",
                                style = RcType.LabelSm.copy(
                                    color = Rc.GoldBright,
                                    letterSpacing = 1.sp,
                                ),
                            )
                            Text(
                                "Positions of Salah",
                                style = RcType.BodyBold.copy(color = Color.White, fontSize = 16.sp),
                            )
                            Text(
                                "9 steps · ~6 min",
                                style = RcType.Label.copy(color = Color.White.copy(.62f)),
                            )
                        }
                    }

                    Box(
                        Modifier
                            .size(36.dp)
                            .background(Rc.Gold, CircleShape),
                        Alignment.Center,
                    ) {
                        Text(
                            "→",
                            fontSize = 16.sp,
                            color = Rc.EmeraldDeep,
                            fontWeight = FontWeight.Bold,
                        )
                    }
                }

                Spacer(Modifier.height(28.dp))
            }
        }

        // ── Parchment body (rounds over header bottom) ─────────────────────────
        Column(
            Modifier
                .fillMaxWidth()
                .offset(y = (-24).dp)
                .clip(RoundedCornerShape(topStart = 24.dp, topEnd = 24.dp))
                .background(Rc.Parchment)
                .padding(top = 28.dp, bottom = 32.dp),
        ) {
            // Section header
            Row(
                Modifier
                    .fillMaxWidth()
                    .padding(horizontal = 20.dp),
                Arrangement.SpaceBetween,
                Alignment.CenterVertically,
            ) {
                Text(
                    "The five prayers",
                    style = RcType.BodyBold.copy(color = Rc.Ink, fontSize = 16.sp),
                )
                Text(
                    "$learnedCount of 5 learned",
                    style = RcType.Label.copy(color = Rc.Muted),
                )
            }

            Spacer(Modifier.height(16.dp))

            prayers.forEachIndexed { index, prayer ->
                PrayerRow(
                    prayer = prayer,
                    displayIndex = index + 1,
                    onClick = {
                        Toast.makeText(context, "Coming soon", Toast.LENGTH_SHORT).show()
                    },
                )
                if (index < prayers.size - 1) {
                    Box(
                        Modifier
                            .fillMaxWidth()
                            .padding(horizontal = 20.dp)
                            .height(1.dp)
                            .background(Rc.Line),
                    )
                }
            }
        }
    }
}

// ── Prayer row ────────────────────────────────────────────────────────────────

@Composable
private fun PrayerRow(
    prayer: DailyPrayer,
    displayIndex: Int,
    onClick: () -> Unit,
) {
    Row(
        Modifier
            .fillMaxWidth()
            .clickable { onClick() }
            .padding(horizontal = 20.dp, vertical = 14.dp),
        horizontalArrangement = Arrangement.spacedBy(14.dp),
        verticalAlignment = Alignment.CenterVertically,
    ) {
        // Index tile
        Box(
            Modifier
                .size(42.dp)
                .background(
                    color = if (prayer.isLearned) Rc.Emerald else Rc.Cream,
                    shape = RoundedCornerShape(12.dp),
                )
                .border(
                    width = 1.dp,
                    color = if (prayer.isLearned) Rc.Emerald else Rc.Line,
                    shape = RoundedCornerShape(12.dp),
                ),
            Alignment.Center,
        ) {
            if (prayer.isLearned) {
                Text("✓", fontSize = 18.sp, color = Color.White, fontWeight = FontWeight.Bold)
            } else {
                Text(
                    "$displayIndex",
                    style = RcType.BodyBold.copy(color = Rc.Emerald, fontSize = 15.sp),
                )
            }
        }

        // Name block
        Column(
            Modifier.weight(1f),
            verticalArrangement = Arrangement.spacedBy(3.dp),
        ) {
            Row(
                horizontalArrangement = Arrangement.spacedBy(8.dp),
                verticalAlignment = Alignment.CenterVertically,
            ) {
                Text(
                    prayer.nameEn,
                    style = RcType.BodyBold.copy(color = Rc.Ink, fontSize = 15.sp),
                )
                Text(
                    prayer.nameAr,
                    style = RcType.Arabic.copy(color = Rc.Emerald, fontSize = 16.sp),
                )
            }
            Text(
                prayer.subtitleEn,
                style = RcType.Label.copy(color = Rc.Muted),
            )
        }

        // Rakah count
        Column(
            horizontalAlignment = Alignment.End,
            verticalArrangement = Arrangement.spacedBy(2.dp),
        ) {
            Text(
                "${prayer.rakahCount}",
                style = RcType.BodyBold.copy(color = Rc.Ink, fontSize = 16.sp),
            )
            Text(
                "RAKʿAH",
                style = RcType.LabelSm.copy(color = Rc.Muted, fontSize = 8.sp),
            )
        }
    }
}

// ── Preview ───────────────────────────────────────────────────────────────────

@Preview(showBackground = true, backgroundColor = 0xFFF7F1E4)
@Composable
private fun NamazHubPreview() {
    NamazHubContent(
        prayers = DAILY_PRAYERS,
        onBack = {},
        onStartLesson = {},
    )
}
