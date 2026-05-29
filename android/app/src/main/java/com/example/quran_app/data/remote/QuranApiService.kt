package com.example.quran_app.data.remote

import com.example.quran_app.domain.model.ArabicLetter
import com.example.quran_app.domain.model.Ayah
import com.example.quran_app.domain.model.DrawingCheckRequest
import com.example.quran_app.domain.model.DrawingResult
import com.example.quran_app.domain.model.LetterForms
import com.example.quran_app.domain.model.PagedResponse
import com.example.quran_app.domain.model.PronunciationResult
import com.example.quran_app.domain.model.Surah
import okhttp3.MultipartBody
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.Multipart
import retrofit2.http.POST
import retrofit2.http.Part
import retrofit2.http.Path
import retrofit2.http.Query

interface QuranApiService {
    @GET("api/arabic-letters")
    suspend fun getArabicLetters(
        @Query("page")     page:     Int = 1,
        @Query("pageSize") pageSize: Int = 10
    ): PagedResponse<ArabicLetter>

    @GET("api/arabic-letters/{id}")
    suspend fun getArabicLetter(@Path("id") id: Int): ArabicLetter

    @GET("api/arabic-letters/{id}/forms")
    suspend fun getLetterForms(@Path("id") id: Int): LetterForms

    @POST("api/arabic-letters/{id}/check-drawing")
    suspend fun checkDrawing(
        @Path("id") id: Int,
        @Body body: DrawingCheckRequest
    ): DrawingResult

    /** One-shot HTTP pronunciation check — replaces broken WebSocket path. */
    @Multipart
    @POST("api/arabic-letters/{id}/check-pronunciation")
    suspend fun checkPronunciation(
        @Path("id") id: Int,
        @Part audio: MultipartBody.Part
    ): PronunciationResult

    @GET("api/quran/surahs")
    suspend fun getSurahs(): List<Surah>

    @GET("api/quran/surahs/{surahId}/ayahs")
    suspend fun getAyahs(@Path("surahId") surahId: Int): List<Ayah>
}
