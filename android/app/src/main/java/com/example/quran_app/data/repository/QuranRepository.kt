package com.example.quran_app.data.repository

import com.example.quran_app.data.remote.QuranApiService
import com.example.quran_app.domain.model.ArabicLetter
import com.example.quran_app.domain.model.Ayah
import com.example.quran_app.domain.model.DrawingCheckRequest
import com.example.quran_app.domain.model.DrawingResult
import com.example.quran_app.domain.model.LetterForms
import com.example.quran_app.domain.model.PagedResponse
import com.example.quran_app.domain.model.SaveTajweedProgressRequest
import com.example.quran_app.domain.model.Surah
import com.example.quran_app.domain.model.TajweedLetterProgress
import com.example.quran_app.data.local.offlineStep
import com.example.quran_app.domain.model.PracticeStepResponse
import com.example.quran_app.domain.model.TanweenLessonData

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

    suspend fun getPracticeStep(lessonNum: Int, stepNum: Int): Result<PracticeStepResponse> =
        runCatching { apiService.getPracticeStep(lessonNum, stepNum) }
            .recoverCatching {
                offlineStep(lessonNum, stepNum)
                    ?: throw NoSuchElementException("offline: no step $lessonNum/$stepNum")
            }
}
