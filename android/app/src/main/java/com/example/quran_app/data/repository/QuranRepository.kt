package com.example.quran_app.data.repository

import com.example.quran_app.data.remote.QuranApiService
import com.example.quran_app.domain.model.ArabicLetter
import com.example.quran_app.domain.model.Ayah
import com.example.quran_app.domain.model.DrawingCheckRequest
import com.example.quran_app.domain.model.DrawingResult
import com.example.quran_app.domain.model.LetterForms
import com.example.quran_app.domain.model.PagedResponse
import com.example.quran_app.domain.model.PronunciationResult
import com.example.quran_app.domain.model.Surah
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

    suspend fun getLetterForms(id: Int): Result<LetterForms> = runCatching {
        apiService.getLetterForms(id)
    }

    suspend fun checkDrawing(id: Int, imageBase64: String): Result<DrawingResult> = runCatching {
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
    suspend fun checkPronunciation(id: Int, audioFile: File): Result<PronunciationResult> =
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
}
