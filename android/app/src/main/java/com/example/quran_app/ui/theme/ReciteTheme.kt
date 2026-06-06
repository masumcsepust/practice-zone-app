package com.example.quran_app.ui.theme

import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.FontFamily
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.sp

// ── Spiritual / premium palette ───────────────────────────────────────────────
object Rc {
    // Backgrounds
    val Ink          = Color(0xFF11231B)
    val Parchment    = Color(0xFFF7F1E4)
    val Cream        = Color(0xFFFFFDF6)
    val Line         = Color(0xFFE7DDC8)

    // Emerald family
    val Emerald      = Color(0xFF15573F)
    val EmeraldDeep  = Color(0xFF0B3A29)
    val EmeraldSoft  = Color(0xFFE4EFE7)

    // Gold family
    val Gold         = Color(0xFFB88A36)
    val GoldBright   = Color(0xFFD8B25A)
    val GoldSoft     = Color(0xFFF1E4C4)

    // Muted / secondary text
    val Muted        = Color(0xFF7C8A80)

    // Tajweed accent colors
    val TajQalqalah  = Color(0xFFC0603F)   // warm orange-red
    val TajMadd      = Color(0xFF3F7FA6)   // cool blue
    val TajGhunnah   = Color(0xFF15573F)   // emerald (same as brand)
    val TajIdghaam   = Color(0xFF7A5C9E)   // soft violet
    val TajIkhfaa    = Color(0xFF4A7A6A)   // teal-green

    // Status
    val RecordRed    = Color(0xFFDC2626)
    val LiveGreen    = Color(0xFF4ADE80)
}

// ── Typography ────────────────────────────────────────────────────────────────
// Production: replace FontFamily.Serif → Amiri, FontFamily.Serif → Fraunces, Default → Manrope
// via ui-text-google-fonts:  Font(GoogleFont("Amiri"), provider)
object RcType {
    /** Arabic Quran text — Amiri (serif, RTL, large line height) */
    val Arabic = TextStyle(
        fontFamily  = FontFamily.Serif,
        fontWeight  = FontWeight.Normal,
        fontSize    = 30.sp,
        lineHeight  = 52.sp,
        letterSpacing = 0.sp,
    )

    /** Large Arabic display — bigger */
    val ArabicLarge = TextStyle(
        fontFamily  = FontFamily.Serif,
        fontWeight  = FontWeight.Normal,
        fontSize    = 36.sp,
        lineHeight  = 64.sp,
        letterSpacing = 0.sp,
    )

    /** Display heading — Fraunces */
    val Display = TextStyle(
        fontFamily  = FontFamily.Serif,
        fontWeight  = FontWeight.SemiBold,
        fontSize    = 22.sp,
        lineHeight  = 28.sp,
    )

    /** UI body — Manrope */
    val Body = TextStyle(
        fontFamily  = FontFamily.Default,
        fontWeight  = FontWeight.Normal,
        fontSize    = 14.sp,
        lineHeight  = 20.sp,
    )

    val BodyBold = Body.copy(fontWeight = FontWeight.Bold)
    val Label    = Body.copy(fontSize = 12.sp, lineHeight = 16.sp)
    val LabelSm  = Body.copy(fontSize = 10.sp, lineHeight = 14.sp, letterSpacing = 0.8.sp)
    val Caption  = Body.copy(fontSize = 11.sp, color = Color.Unspecified)
}
