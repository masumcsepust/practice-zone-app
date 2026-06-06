package com.example.quran_app.data.analyzer

import com.example.quran_app.data.remote.QuranApiService
import com.example.quran_app.domain.model.RecitationResult
import com.example.quran_app.domain.model.toRecitationResult
import okhttp3.MediaType.Companion.toMediaType
import okhttp3.MultipartBody
import okhttp3.RequestBody.Companion.toRequestBody

class ApiRecitationAnalyzer(private val apiService: QuranApiService) : RecitationAnalyzer {

    override suspend fun analyze(
        pcm:       ByteArray,
        surahId:   Int,
        ayahRange: IntRange,
    ): RecitationResult {
        // LetterRecorder produces OGG/OPUS on API 29+ and MP4/AAC below —
        // both are accepted by Azure Speech via AudioStreamContainerFormat.ANY
        val mimeType  = "audio/ogg"
        val body      = pcm.toRequestBody(mimeType.toMediaType())
        val audioPart = MultipartBody.Part.createFormData("audio", "recording.ogg", body)

        return apiService
            .assessAyah(surahId, ayahRange.first, audioPart)
            .toRecitationResult()
    }
}
