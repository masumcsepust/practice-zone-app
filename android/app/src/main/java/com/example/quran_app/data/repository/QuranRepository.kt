package com.example.quran_app.data.repository

import com.example.quran_app.data.remote.QuranApiService
import com.example.quran_app.domain.model.ArabicLetter
import com.example.quran_app.domain.model.Ayah
import com.example.quran_app.domain.model.DrawingCheckRequest
import com.example.quran_app.domain.model.DrawingResult
import com.example.quran_app.domain.model.LetterForms
import com.example.quran_app.domain.model.PagedResponse
import com.example.quran_app.domain.model.PronunciationResult
import com.example.quran_app.domain.model.SaveTajweedProgressRequest
import com.example.quran_app.domain.model.Surah
import com.example.quran_app.domain.model.TajweedLetterProgress
import com.example.quran_app.domain.model.PracticeSessionData
import com.example.quran_app.domain.model.TanweenLessonData
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.MultipartBody
import okhttp3.RequestBody.Companion.asRequestBody
import java.io.File

class QuranRepository(private val apiService: QuranApiService) {
    suspend fun getArabicLetters(
        page:     Int = 1,
        pageSize: Int = 10
    ): Result<PagedResponse<ArabicLetter>> = runCatching {
        apiService.getArabicLetters(page, pageSize)
    }

    suspend fun getLetterForms(id: String): Result<LetterForms> = runCatching {
        apiService.getLetterForms(id)
    }

    suspend fun checkDrawing(id: String, imageBase64: String): Result<DrawingResult> = runCatching {
        apiService.checkDrawing(id, DrawingCheckRequest(imageBase64))
    }

    /**
     * Uploads the recorded audio file to POST /api/arabic-letters/{id}/check-pronunciation
     * and returns an AI-evaluated [PronunciationResult].
     *
     * Infers MIME type from file extension:
     *  .ogg → audio/ogg   (Android 10+ OGG/OPUS)
     *  .mp4 → audio/mp4   (Android < 10 fallback)
     *  .wav → audio/wav
     */
    suspend fun checkPronunciation(id: String, audioFile: File): Result<PronunciationResult> =
        runCatching {
            val mime = when (audioFile.extension.lowercase()) {
                "ogg"  -> "audio/ogg"
                "webm" -> "audio/webm"
                "mp4"  -> "audio/mp4"
                "wav"  -> "audio/wav"
                else   -> "audio/ogg"
            }
            val requestBody = audioFile.asRequestBody(mime.toMediaTypeOrNull())
            val part        = MultipartBody.Part.createFormData("audio", audioFile.name, requestBody)
            apiService.checkPronunciation(id, part)
        }

    suspend fun getSurahs(): Result<List<Surah>> = runCatching {
        apiService.getSurahs()
    }

    suspend fun getAyahs(surahId: Int): Result<List<Ayah>> = runCatching {
        apiService.getAyahs(surahId)
    }

    suspend fun getPage(pageNumber: Int): Result<List<Ayah>> = runCatching {
        apiService.getPage(pageNumber)
    }

    suspend fun saveTajweedProgress(
        letterId:     String,
        mode:         String,
        score:        Double,
        accuracyScore: Double,
        isCorrect:    Boolean,
    ): Result<Unit> = runCatching {
        apiService.saveTajweedProgress(
            letterId,
            SaveTajweedProgressRequest(mode, score, accuracyScore, isCorrect)
        )
    }

    suspend fun getTajweedProgress(): Result<List<TajweedLetterProgress>> = runCatching {
        apiService.getTajweedProgress()
    }

    suspend fun getTanweenLesson(letterOrder: Int): Result<TanweenLessonData> = runCatching {
        apiService.getTanweenLesson(letterOrder).data
    }

    suspend fun getPracticeLessons(): Result<List<PracticeSessionData>> = runCatching {
        apiService.getPracticeLessons().data
    }

    suspend fun getPracticeLesson(id: String): Result<PracticeSessionData> = runCatching {
        apiService.getPracticeLesson(id).data
    }
}
