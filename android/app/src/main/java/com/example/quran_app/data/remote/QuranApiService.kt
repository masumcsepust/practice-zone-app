package com.example.quran_app.data.remote

import com.example.quran_app.domain.model.ArabicLetter
import com.example.quran_app.domain.model.AuthResponse
import com.example.quran_app.domain.model.LeaderboardResponse
import com.example.quran_app.domain.model.PracticeStepResponse
import com.example.quran_app.domain.model.LoginRequest
import com.example.quran_app.domain.model.RegisterRequest
import com.example.quran_app.domain.model.TanweenLessonResponse
import com.example.quran_app.domain.model.Ayah
import com.example.quran_app.domain.model.DrawingCheckRequest
import com.example.quran_app.domain.model.DrawingResult
import com.example.quran_app.domain.model.LetterForms
import com.example.quran_app.domain.model.PagedResponse
import com.example.quran_app.domain.model.UserInfo
import com.example.quran_app.domain.model.SaveTajweedProgressRequest
import com.example.quran_app.domain.model.Surah
import com.example.quran_app.domain.model.TajweedLetterProgress
import com.example.quran_app.domain.model.AyahAssessmentResult
import okhttp3.MultipartBody
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.Multipart
import retrofit2.http.POST
import retrofit2.http.Part
import retrofit2.http.Path
import retrofit2.http.Query

interface QuranApiService {

    // ── Auth ─────────────────────────────────────────────────────────────────
    @POST("api/auth/register")
    suspend fun register(@Body body: RegisterRequest): AuthResponse

    @POST("api/auth/login")
    suspend fun login(@Body body: LoginRequest): AuthResponse

    @GET("api/auth/me")
    suspend fun me(): UserInfo

    // ── Letters ───────────────────────────────────────────────────────────────
    @GET("api/arabic-letters")
    suspend fun getArabicLetters(
        @Query("page")     page:     Int = 1,
        @Query("pageSize") pageSize: Int = 10
    ): PagedResponse<ArabicLetter>

    @GET("api/arabic-letters/{id}/forms")
    suspend fun getLetterForms(@Path("id") id: String): LetterForms

    @POST("api/arabic-letters/{id}/check-drawing")
    suspend fun checkDrawing(
        @Path("id") id: String,
        @Body body: DrawingCheckRequest
    ): DrawingResult

    @GET("api/quran/surahs")
    suspend fun getSurahs(): List<Surah>

    @GET("api/quran/surahs/recitation")
    suspend fun getRecitationSurahs(): List<Surah>

    @GET("api/quran/surahs/{surahId}/ayahs")
    suspend fun getAyahs(@Path("surahId") surahId: Int): List<Ayah>

    @Multipart
    @POST("api/quran/surahs/{surahId}/ayahs/{ayahNumber}/assess")
    suspend fun assessAyah(
        @Path("surahId")    surahId:    Int,
        @Path("ayahNumber") ayahNumber: Int,
        @Part audio: MultipartBody.Part,
    ): AyahAssessmentResult

    @GET("api/quran/pages/{pageNumber}")
    suspend fun getPage(@Path("pageNumber") pageNumber: Int): List<Ayah>

    @POST("api/arabic-letters/{id}/tajweed-progress")
    suspend fun saveTajweedProgress(
        @Path("id") id: String,
        @Body body: SaveTajweedProgressRequest
    )

    @GET("api/arabic-letters/tajweed-progress")
    suspend fun getTajweedProgress(): List<TajweedLetterProgress>

    @GET("api/lessons/tanween/{letterOrder}")
    suspend fun getTanweenLesson(@Path("letterOrder") letterOrder: Int): TanweenLessonResponse

    // ── Practice lessons ──────────────────────────────────────────────────────
    @GET("api/practice/lessons/{lessonNum}/step/{stepNum}")
    suspend fun getPracticeStep(
        @Path("lessonNum") lessonNum: Int,
        @Path("stepNum")   stepNum:   Int,
    ): PracticeStepResponse

    // ── Leaderboard ───────────────────────────────────────────────────────────
    @GET("api/leaderboard")
    suspend fun getLeaderboard(@Query("period") period: String = "all"): LeaderboardResponse
}
