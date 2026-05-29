package com.example.quran_app.data.remote

import com.example.quran_app.domain.model.PronunciationResult
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.Response
import okhttp3.WebSocket
import okhttp3.WebSocketListener
import okio.ByteString
import okio.ByteString.Companion.toByteString
import org.json.JSONObject
import java.io.File
import java.util.concurrent.TimeUnit

/**
 * Connects to  ws://localhost:5092/ws/letter?letterId={id},
 * streams the recorded audio file as binary chunks, sends "end-of-audio",
 * then delivers the `letter-result` JSON as a [PronunciationResult].
 */
class LetterPracticeWebSocketClient {

    private val client = OkHttpClient.Builder()
        .readTimeout(30, TimeUnit.SECONDS)
        .build()

    private var activeSocket: WebSocket? = null

    fun assess(
        letterId:  Int,
        audioFile: File,
        onResult:  (PronunciationResult) -> Unit,
        onError:   (String) -> Unit
    ) {
        val request = Request.Builder()
            .url("ws://localhost:5092/ws/letter?letterId=$letterId")
            .build()

        activeSocket = client.newWebSocket(request, object : WebSocketListener() {

            override fun onOpen(webSocket: WebSocket, response: Response) {
                // Stream the file in 4 KB binary chunks
                val bytes = audioFile.readBytes()
                val chunkSize = 4_096
                var offset = 0
                while (offset < bytes.size) {
                    val end   = minOf(offset + chunkSize, bytes.size)
                    val chunk = bytes.copyOfRange(offset, end).toByteString()
                    webSocket.send(chunk)
                    offset = end
                }
                // Signal end of audio — include mime type so backend uses the right decoder
                val mimeType = when (audioFile.extension.lowercase()) {
                    "ogg"  -> "audio/ogg"
                    "mp4"  -> "audio/mpeg"
                    else   -> "audio/ogg"
                }
                webSocket.send("mime:$mimeType|end-of-audio")
            }

            override fun onMessage(webSocket: WebSocket, text: String) {
                try {
                    val json = JSONObject(text)
                    when (json.getString("type")) {
                        "letter-result" -> {
                            val result = PronunciationResult(
                                isCorrect      = json.getBoolean("isCorrect"),
                                score          = json.getDouble("pronunciationScore"),
                                accuracyScore  = json.getDouble("accuracyScore"),
                                recognizedText = json.optString("recognizedText", ""),
                                feedback       = json.optString("feedback", ""),
                                makhrajHint    = json.optString("makhrajHint", ""),
                                letter         = json.optString("letter", ""),
                                letterName     = json.optString("letterName", "")
                            )
                            onResult(result)
                            webSocket.close(1000, "done")
                        }
                        "error" -> {
                            onError(json.optString("message", "সার্ভার ত্রুটি"))
                            webSocket.close(1000, "error")
                        }
                    }
                } catch (e: Exception) {
                    onError("ফলাফল পার্স করা যায়নি: ${e.message}")
                }
            }

            override fun onClosing(webSocket: WebSocket, code: Int, reason: String) {
                webSocket.close(1000, null)
            }

            override fun onFailure(webSocket: WebSocket, t: Throwable, response: Response?) {
                val msg = t.message?.takeIf { it.isNotBlank() }
                    ?: "সার্ভারের সাথে সংযোগ বিচ্ছিন্ন হয়েছে"
                onError(msg)
            }
        })
    }

    fun cancel() {
        activeSocket?.cancel()
        activeSocket = null
    }
}
