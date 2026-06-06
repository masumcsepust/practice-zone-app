package com.example.quran_app.ui.screens.namaz

import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.SnackbarHost
import androidx.compose.material3.SnackbarHostState
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontStyle
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.compose.material3.Text
import com.example.quran_app.domain.model.PrayerStep
import com.example.quran_app.domain.model.SALAH_STEPS
import com.example.quran_app.ui.theme.Rc
import com.example.quran_app.ui.theme.RcType
import com.example.quran_app.ui.viewmodel.PostureLessonViewModel
import kotlinx.coroutines.launch

@Composable
fun PostureLessonScreen(
    viewModel: PostureLessonViewModel,
    onBack: () -> Unit,
    onComplete: () -> Unit,
) {
    val stepIndex by viewModel.stepIndex.collectAsState()
    val step by viewModel.currentStep.collectAsState()
    val context = LocalContext.current
    val snackbarHostState = remember { SnackbarHostState() }
    val scope = rememberCoroutineScope()

    LaunchedEffect(Unit) {
        viewModel.navigateBack.collect { onComplete() }
    }

    PostureLessonContent(
        step = step,
        stepIndex = stepIndex,
        totalSteps = SALAH_STEPS.size,
        snackbarHostState = snackbarHostState,
        onBackStep = { if (stepIndex > 1) viewModel.previous() else onBack() },
        onNext = viewModel::next,
        onComplete = viewModel::complete,
        onListenTap = { audioRes ->
            if (audioRes != null) {
                viewModel.playAudio(context, audioRes)
            } else {
                scope.launch { snackbarHostState.showSnackbar("Audio coming soon") }
            }
        },
    )
}

@Composable
private fun PostureLessonContent(
    step: PrayerStep,
    stepIndex: Int,
    totalSteps: Int,
    snackbarHostState: SnackbarHostState = remember { SnackbarHostState() },
    onBackStep: () -> Unit,
    onNext: () -> Unit,
    onComplete: () -> Unit,
    onListenTap: (audioRes: Int?) -> Unit,
) {
    Box(Modifier.fillMaxSize()) {
        Column(
            Modifier
                .fillMaxSize()
                .background(Rc.EmeraldDeep)
        ) {
            // ── Top bar ────────────────────────────────────────────────────────
            Column(
                Modifier
                    .fillMaxWidth()
                    .statusBarsPadding()
                    .padding(horizontal = 16.dp, vertical = 12.dp),
                verticalArrangement = Arrangement.spacedBy(12.dp),
            ) {
                Row(
                    Modifier.fillMaxWidth(),
                    Arrangement.SpaceBetween,
                    Alignment.CenterVertically,
                ) {
                    Box(
                        Modifier
                            .size(40.dp)
                            .background(Color.White.copy(.12f), RoundedCornerShape(12.dp))
                            .clickable { onBackStep() },
                        Alignment.Center,
                    ) {
                        Text("‹", fontSize = 22.sp, color = Color.White, fontWeight = FontWeight.Bold)
                    }

                    Text(
                        "Positions of Salah",
                        style = RcType.BodyBold.copy(color = Color.White, fontSize = 15.sp),
                    )

                    Spacer(Modifier.size(40.dp))
                }

                // Step progress strip — filled gold up to current, bright for current
                Row(
                    Modifier.fillMaxWidth(),
                    Arrangement.spacedBy(4.dp),
                    Alignment.CenterVertically,
                ) {
                    repeat(totalSteps) { i ->
                        val idx = i + 1
                        Box(
                            Modifier
                                .weight(1f)
                                .height(4.dp)
                                .clip(RoundedCornerShape(2.dp))
                                .background(
                                    when {
                                        idx < stepIndex -> Rc.Gold
                                        idx == stepIndex -> Rc.GoldBright
                                        else -> Color.White.copy(.22f)
                                    }
                                )
                        )
                    }
                }
            }

            // ── Posture stage (dark radial bg) ─────────────────────────────────
            Box(
                Modifier
                    .fillMaxWidth()
                    .height(216.dp)
                    .background(
                        Brush.radialGradient(
                            colors = listOf(Color(0xFF1E4D38), Color(0xFF0B1E14)),
                        )
                    ),
                Alignment.Center,
            ) {
                Column(
                    horizontalAlignment = Alignment.CenterHorizontally,
                    verticalArrangement = Arrangement.spacedBy(8.dp),
                ) {
                    Box(
                        Modifier
                            .size(118.dp)
                            .background(Color.White.copy(.06f), CircleShape)
                            .border(2.dp, Rc.Gold, CircleShape),
                        Alignment.Center,
                    ) {
                        Image(
                            painter = painterResource(id = step.postureRes),
                            contentDescription = step.nameEn,
                            modifier = Modifier.size(76.dp),
                        )
                    }

                    Text(
                        step.nameAr,
                        style = RcType.Arabic.copy(color = Rc.GoldBright, fontSize = 24.sp),
                    )

                    Text(
                        "${step.nameEn} — ${step.subtitleEn}",
                        style = RcType.Display.copy(color = Color.White, fontSize = 17.sp),
                    )

                    Text(
                        "STEP $stepIndex OF $totalSteps",
                        style = RcType.LabelSm.copy(color = Rc.Muted, letterSpacing = 1.sp),
                    )
                }
            }

            // ── Parchment body ─────────────────────────────────────────────────
            Column(
                Modifier
                    .fillMaxWidth()
                    .weight(1f)
                    .clip(RoundedCornerShape(topStart = 20.dp, topEnd = 20.dp))
                    .background(Rc.Parchment)
                    .verticalScroll(rememberScrollState())
                    .padding(20.dp),
                verticalArrangement = Arrangement.spacedBy(14.dp),
            ) {
                // Recitation card — gold left-border, cream bg
                Row(
                    Modifier
                        .fillMaxWidth()
                        .clip(RoundedCornerShape(14.dp))
                        .background(Rc.Cream)
                        .height(IntrinsicSize.Min),
                ) {
                    Box(
                        Modifier
                            .width(4.dp)
                            .fillMaxHeight()
                            .background(Rc.Gold)
                    )
                    Column(
                        Modifier.padding(
                            start = 14.dp, end = 12.dp,
                            top = 14.dp, bottom = 14.dp,
                        ),
                        verticalArrangement = Arrangement.spacedBy(8.dp),
                    ) {
                        Text(
                            "RECITE WHILE ${step.subtitleEn.uppercase()}",
                            style = RcType.LabelSm.copy(color = Rc.Gold, letterSpacing = 1.sp),
                        )

                        Text(
                            step.recitationAr,
                            style = RcType.Arabic.copy(color = Rc.Ink, fontSize = 22.sp),
                            textAlign = TextAlign.End,
                            modifier = Modifier.fillMaxWidth(),
                        )

                        Text(
                            step.recitationTranslit,
                            style = RcType.Display.copy(
                                color = Rc.Emerald,
                                fontSize = 14.sp,
                                fontStyle = FontStyle.Italic,
                            ),
                        )

                        Text(
                            step.recitationMeaning,
                            style = RcType.Caption.copy(color = Rc.Muted, fontSize = 11.sp),
                        )

                        if (step.repeatCount > 1) {
                            Box(
                                Modifier
                                    .background(Rc.GoldSoft, RoundedCornerShape(20.dp))
                                    .padding(horizontal = 12.dp, vertical = 5.dp),
                            ) {
                                Text(
                                    "Repeat ×${step.repeatCount}",
                                    style = RcType.LabelSm.copy(color = Rc.Gold),
                                )
                            }
                        }
                    }
                }

                // Listen button — full-width outline
                Row(
                    Modifier
                        .fillMaxWidth()
                        .clip(RoundedCornerShape(14.dp))
                        .border(1.5.dp, Rc.Emerald, RoundedCornerShape(14.dp))
                        .clickable { onListenTap(step.audioRes) }
                        .padding(14.dp),
                    Arrangement.Center,
                    Alignment.CenterVertically,
                ) {
                    Box(
                        Modifier
                            .size(32.dp)
                            .background(Rc.Emerald, CircleShape),
                        Alignment.Center,
                    ) {
                        Text("▶", fontSize = 12.sp, color = Color.White)
                    }
                    Spacer(Modifier.width(10.dp))
                    Text(
                        "Listen & follow",
                        style = RcType.BodyBold.copy(color = Rc.Emerald),
                    )
                }
            }

            // ── Bottom navigation ──────────────────────────────────────────────
            Row(
                Modifier
                    .fillMaxWidth()
                    .background(Rc.Parchment)
                    .navigationBarsPadding()
                    .padding(horizontal = 16.dp, vertical = 12.dp),
                Arrangement.spacedBy(12.dp),
                Alignment.CenterVertically,
            ) {
                Box(
                    Modifier
                        .width(56.dp)
                        .height(52.dp)
                        .clip(RoundedCornerShape(14.dp))
                        .border(1.5.dp, Rc.Line, RoundedCornerShape(14.dp))
                        .clickable { onBackStep() },
                    Alignment.Center,
                ) {
                    Text("‹", fontSize = 20.sp, color = Rc.Ink, fontWeight = FontWeight.Bold)
                }

                val isLastStep = stepIndex == totalSteps
                val nextLabel = if (isLastStep) "Complete ✓"
                else "Next — ${SALAH_STEPS[stepIndex].nameEn}"

                Box(
                    Modifier
                        .weight(1f)
                        .height(52.dp)
                        .clip(RoundedCornerShape(14.dp))
                        .background(Rc.Gold)
                        .clickable { if (isLastStep) onComplete() else onNext() },
                    Alignment.Center,
                ) {
                    Text(
                        nextLabel,
                        style = RcType.BodyBold.copy(color = Rc.EmeraldDeep, fontSize = 15.sp),
                    )
                }
            }
        }

        SnackbarHost(
            hostState = snackbarHostState,
            modifier = Modifier.align(Alignment.BottomCenter),
        )
    }
}

// ── Preview ───────────────────────────────────────────────────────────────────

@Preview(showBackground = true)
@Composable
private fun PostureLessonPreview() {
    PostureLessonContent(
        step = SALAH_STEPS[2],
        stepIndex = 3,
        totalSteps = SALAH_STEPS.size,
        onBackStep = {},
        onNext = {},
        onComplete = {},
        onListenTap = {},
    )
}
