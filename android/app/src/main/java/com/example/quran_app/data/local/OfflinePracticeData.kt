package com.example.quran_app.data.local

import com.example.quran_app.domain.model.PracticeItem
import com.example.quran_app.domain.model.PracticeSessionData
import com.example.quran_app.domain.model.PracticeStepResponse
import com.example.quran_app.domain.model.StepSyllable
import com.example.quran_app.domain.model.SyllableDetails
import com.example.quran_app.domain.model.SyllableInfo

private fun syllable(
    char: String, latin: String, bn: String,
    letter: String, sign: String, group: String
) = SyllableInfo(char, latin, bn, "", SyllableDetails(letter, sign, group))

private fun item(
    id: String, instruction: String, tip: String,
    target: SyllableInfo, compare: SyllableInfo? = null
) = PracticeItem(id, instruction, target, compare, tip)

private val READ_RECORD = "নিচের অংশটি পড়ুন এবং রেকর্ড করুন"
private val TANWEEN_TIP = "তানভীন হলো নামের (শব্দের) শেষে ব্যবহৃত একটি চিহ্ন। এটি সাধারণত 'ন (n)' ধ্বনি সৃষ্টি করে।"

val offlinePracticeLessons: List<PracticeSessionData> = listOf(

    // ── Lesson 1: Fatha vs Fathataan (Ba/Ta/Tha) ─────────────────────────────
    PracticeSessionData(
        lessonId      = "offline-1",
        title         = "তানভীন অনুশীলন",
        sequenceOrder = 1,
        practiceItems = listOf(
            item("o1-1", READ_RECORD, TANWEEN_TIP,
                syllable("بَ", "Ba", "বা", "বা", "ফাতহা (যবর)", "Harakat"),
                syllable("بً", "Ban", "বান", "বা", "ফাতহাতান (দুই যবর)", "Tanween")),
            item("o1-2", READ_RECORD, TANWEEN_TIP,
                syllable("تَ", "Ta", "তা", "তা", "ফাতহা (যবর)", "Harakat"),
                syllable("تً", "Tan", "তান", "তা", "ফাতহাতান (দুই যবর)", "Tanween")),
            item("o1-3", READ_RECORD, TANWEEN_TIP,
                syllable("ثَ", "Tha", "থা", "থা", "ফাতহা (যবর)", "Harakat"),
                syllable("ثً", "Than", "থান", "থা", "ফাতহাতান (দুই যবর)", "Tanween")),
            item("o1-4", READ_RECORD, TANWEEN_TIP,
                syllable("جَ", "Ja", "জা", "জিম", "ফাতহা (যবর)", "Harakat"),
                syllable("جً", "Jan", "জান", "জিম", "ফাতহাতান (দুই যবর)", "Tanween")),
            item("o1-5", READ_RECORD, TANWEEN_TIP,
                syllable("حَ", "Ha", "হা", "হা", "ফাতহা (যবর)", "Harakat"),
                syllable("حً", "Han", "হান", "হা", "ফাতহাতান (দুই যবর)", "Tanween")),
            item("o1-6", READ_RECORD, TANWEEN_TIP,
                syllable("خَ", "Kha", "খা", "খা", "ফাতহা (যবর)", "Harakat"),
                syllable("خً", "Khan", "খান", "খা", "ফাতহাতান (দুই যবর)", "Tanween")),
            item("o1-7", READ_RECORD, TANWEEN_TIP,
                syllable("دَ", "Da", "দা", "দাল", "ফাতহা (যবর)", "Harakat"),
                syllable("دً", "Dan", "দান", "দাল", "ফাতহাতান (দুই যবর)", "Tanween")),
        )
    ),

    // ── Lesson 2: Kasra vs Kasrataan ─────────────────────────────────────────
    PracticeSessionData(
        lessonId      = "offline-2",
        title         = "কাসরা বনাম তানভীন",
        sequenceOrder = 2,
        practiceItems = listOf(
            item("o2-1", READ_RECORD, "কাসরা দুটো একসাথে (কাসরাতান) শব্দের শেষে 'ন' ধ্বনি যোগ করে।",
                syllable("بِ", "Bi", "বি", "বা", "কাসরা (যের)", "Harakat"),
                syllable("بٍ", "Bin", "বিন", "বা", "কাসরাতান (দুই যের)", "Tanween")),
            item("o2-2", READ_RECORD, "কাসরা দুটো একসাথে (কাসরাতান) শব্দের শেষে 'ন' ধ্বনি যোগ করে।",
                syllable("تِ", "Ti", "তি", "তা", "কাসরা (যের)", "Harakat"),
                syllable("تٍ", "Tin", "তিন", "তা", "কাসরাতান (দুই যের)", "Tanween")),
            item("o2-3", READ_RECORD, "কাসরা দুটো একসাথে (কাসরাতান) শব্দের শেষে 'ন' ধ্বনি যোগ করে।",
                syllable("سِ", "Si", "সি", "সিন", "কাসরা (যের)", "Harakat"),
                syllable("سٍ", "Sin", "সিন", "সিন", "কাসরাতান (দুই যের)", "Tanween")),
            item("o2-4", READ_RECORD, "কাসরা দুটো একসাথে (কাসরাতান) শব্দের শেষে 'ন' ধ্বনি যোগ করে।",
                syllable("مِ", "Mi", "মি", "মিম", "কাসরা (যের)", "Harakat"),
                syllable("مٍ", "Min", "মিন", "মিম", "কাসরাতান (দুই যের)", "Tanween")),
            item("o2-5", READ_RECORD, "কাসরা দুটো একসাথে (কাসরাতান) শব্দের শেষে 'ন' ধ্বনি যোগ করে।",
                syllable("نِ", "Ni", "নি", "নুন", "কাসরা (যের)", "Harakat"),
                syllable("نٍ", "Nin", "নিন", "নুন", "কাসরাতান (দুই যের)", "Tanween")),
            item("o2-6", READ_RECORD, "কাসরা দুটো একসাথে (কাসরাতান) শব্দের শেষে 'ন' ধ্বনি যোগ করে।",
                syllable("رِ", "Ri", "রি", "রা", "কাসরা (যের)", "Harakat"),
                syllable("رٍ", "Rin", "রিন", "রা", "কাসরাতান (দুই যের)", "Tanween")),
            item("o2-7", READ_RECORD, "কাসরা দুটো একসাথে (কাসরাতান) শব্দের শেষে 'ন' ধ্বনি যোগ করে।",
                syllable("لِ", "Li", "লি", "লাম", "কাসরা (যের)", "Harakat"),
                syllable("لٍ", "Lin", "লিন", "লাম", "কাসরাতান (দুই যের)", "Tanween")),
        )
    ),

    // ── Lesson 3: Damma vs Dammataan ─────────────────────────────────────────
    PracticeSessionData(
        lessonId      = "offline-3",
        title         = "দাম্মা বনাম তানভীন",
        sequenceOrder = 3,
        practiceItems = listOf(
            item("o3-1", READ_RECORD, "দাম্মা দুটো একসাথে (দাম্মাতান) শব্দের শেষে 'ন' ধ্বনি যোগ করে।",
                syllable("بُ", "Bu", "বু", "বা", "দাম্মা (পেশ)", "Harakat"),
                syllable("بٌ", "Bun", "বুন", "বা", "দাম্মাতান (দুই পেশ)", "Tanween")),
            item("o3-2", READ_RECORD, "দাম্মা দুটো একসাথে (দাম্মাতান) শব্দের শেষে 'ন' ধ্বনি যোগ করে।",
                syllable("تُ", "Tu", "তু", "তা", "দাম্মা (পেশ)", "Harakat"),
                syllable("تٌ", "Tun", "তুন", "তা", "দাম্মাতান (দুই পেশ)", "Tanween")),
            item("o3-3", READ_RECORD, "দাম্মা দুটো একসাথে (দাম্মাতান) শব্দের শেষে 'ন' ধ্বনি যোগ করে।",
                syllable("سُ", "Su", "সু", "সিন", "দাম্মা (পেশ)", "Harakat"),
                syllable("سٌ", "Sun", "সুন", "সিন", "দাম্মাতান (দুই পেশ)", "Tanween")),
            item("o3-4", READ_RECORD, "দাম্মা দুটো একসাথে (দাম্মাতান) শব্দের শেষে 'ন' ধ্বনি যোগ করে।",
                syllable("مُ", "Mu", "মু", "মিম", "দাম্মা (পেশ)", "Harakat"),
                syllable("مٌ", "Mun", "মুন", "মিম", "দাম্মাতান (দুই পেশ)", "Tanween")),
            item("o3-5", READ_RECORD, "দাম্মা দুটো একসাথে (দাম্মাতান) শব্দের শেষে 'ন' ধ্বনি যোগ করে।",
                syllable("نُ", "Nu", "নু", "নুন", "দাম্মা (পেশ)", "Harakat"),
                syllable("نٌ", "Nun", "নুন", "নুন", "দাম্মাতান (দুই পেশ)", "Tanween")),
            item("o3-6", READ_RECORD, "দাম্মা দুটো একসাথে (দাম্মাতান) শব্দের শেষে 'ন' ধ্বনি যোগ করে।",
                syllable("رُ", "Ru", "রু", "রা", "দাম্মা (পেশ)", "Harakat"),
                syllable("رٌ", "Run", "রুন", "রা", "দাম্মাতান (দুই পেশ)", "Tanween")),
            item("o3-7", READ_RECORD, "দাম্মা দুটো একসাথে (দাম্মাতান) শব্দের শেষে 'ন' ধ্বনি যোগ করে।",
                syllable("لُ", "Lu", "লু", "লাম", "দাম্মা (পেশ)", "Harakat"),
                syllable("لٌ", "Lun", "লুন", "লাম", "দাম্মাতান (দুই পেশ)", "Tanween")),
        )
    ),

    // ── Lesson 4: Single-syllable review ─────────────────────────────────────
    PracticeSessionData(
        lessonId      = "offline-4",
        title         = "পর্যালোচনা অনুশীলন",
        sequenceOrder = 4,
        practiceItems = listOf(
            item("o4-1", "নিচের শব্দটি পড়ুন", "ফাতহা একটি ছোট তির্যক রেখা যা অক্ষরের উপরে বসে।",
                syllable("كَ", "Ka", "কা", "কাফ", "ফাতহা (যবর)", "Harakat")),
            item("o4-2", "নিচের শব্দটি পড়ুন", "ফাতহাতান দুটি ফাতহা একসাথে, যা তানভীনের ধ্বনি তৈরি করে।",
                syllable("كً", "Kan", "কান", "কাফ", "ফাতহাতান (দুই যবর)", "Tanween")),
            item("o4-3", "নিচের শব্দটি পড়ুন", "কাসরা একটি ছোট তির্যক রেখা যা অক্ষরের নিচে বসে।",
                syllable("كِ", "Ki", "কি", "কাফ", "কাসরা (যের)", "Harakat")),
            item("o4-4", "নিচের শব্দটি পড়ুন", "দাম্মা দেখতে ছোট ওয়াওর মতো, অক্ষরের উপরে বসে।",
                syllable("كُ", "Ku", "কু", "কাফ", "দাম্মা (পেশ)", "Harakat")),
            item("o4-5", READ_RECORD, TANWEEN_TIP,
                syllable("فَ", "Fa", "ফা", "ফা", "ফাতহা (যবর)", "Harakat"),
                syllable("فً", "Fan", "ফান", "ফা", "ফাতহাতান (দুই যবর)", "Tanween")),
            item("o4-6", READ_RECORD, TANWEEN_TIP,
                syllable("قَ", "Qa", "কোয়া", "কাফ", "ফাতহা (যবর)", "Harakat"),
                syllable("قً", "Qan", "কোয়ান", "কাফ", "ফাতহাতান (দুই যবর)", "Tanween")),
            item("o4-7", READ_RECORD, TANWEEN_TIP,
                syllable("كَ", "Ka", "কা", "কাফ", "ফাতহা (যবর)", "Harakat"),
                syllable("كً", "Kan", "কান", "কাফ", "ফাতহাতান (দুই যবর)", "Tanween")),
        )
    ),
)

fun offlineStep(lessonNum: Int, stepNum: Int): PracticeStepResponse? {
    val lesson      = offlinePracticeLessons.getOrNull(lessonNum - 1) ?: return null
    val items       = lesson.practiceItems
    val item        = items.getOrNull(stepNum - 1) ?: return null
    val totalLessons = offlinePracticeLessons.size
    val totalSteps  = items.size
    val totalItems  = offlinePracticeLessons.sumOf { it.practiceItems.size }
    val done        = offlinePracticeLessons.take(lessonNum - 1).sumOf { it.practiceItems.size }
    val progress    = if (totalItems == 0) 0 else (done + stepNum - 1) * 100 / totalItems

    return PracticeStepResponse(
        id           = stepNum,
        title        = lesson.title,
        lessonNum    = lessonNum,
        progress     = progress,
        tips         = item.successTip,
        left         = item.targetSyllable.toStep(item.instruction),
        right        = item.compareWithSyllable?.toStep("তুলনা করতে রেকর্ড করুন"),
        rememberText = item.successTip,
        totalLessons = totalLessons,
        totalSteps   = totalSteps,
    )
}

private fun SyllableInfo.toStep(prompt: String) = StepSyllable(
    arabic        = combinedCharacter,
    translit      = transliteration,
    bengali       = transliterationBn,
    prompt        = prompt,
    letterBengali = details.letterName,
    signName      = details.signName,
    signGroup     = details.signGroup,
    audioUrl      = audioUrl,
)
