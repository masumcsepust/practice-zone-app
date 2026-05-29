package com.example.quran_app.data.remote

import com.example.quran_app.domain.model.RecitationResult
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.Response
import okhttp3.WebSocket
import okhttp3.WebSocketListener
import okio.ByteString.Companion.toByteString
import org.json.JSONObject
import java.io.File
import java.util.concurrent.TimeUnit

/**
 * Connects to ws://localhost:5092/ws?ayahId={id},
 * streams the recorded audio file as binary chunks, sends "end-of-audio",
 * then delivers the pronunciation result as a [RecitationResult].
 *
 * Protocol mirrors LetterPracticeWebSocketClient: binary chunks → text "end-of-audio"
 * The QuranRecitationWebSocketHandler on the server calls CloseInputAsync on "end-of-audio",
 * which signals Azure STT end-of-stream → fires Recognized → server sends pronunciation JSON.
 */
class AyahRecitationWebSocketClient {

    private val client = OkHttpClient.Builder()
        .readTimeout(60, TimeUnit.SECONDS)
        .build()

    private var activeSocket: WebSocket? = null

    fun assess(
        ayahId:   Int,
        audioFile: File,
        onResult: (RecitationResult) -> Unit,
        onError:  (String) -> Unit
    ) {
        val request = Request.Builder()
            .url("ws://localhost:5092/ws?ayahId=$ayahId")
            .build()

        var resultReceived = false

        activeSocket = client.newWebSocket(request, object : WebSocketListener() {

            override fun onOpen(webSocket: WebSocket, response: Response) {
                val bytes     = audioFile.readBytes()
                val chunkSize = 4_096
                var offset    = 0
                while (offset < bytes.size) {
                    val end   = minOf(offset + chunkSize, bytes.size)
                    webSocket.send(bytes.copyOfRange(offset, end).toByteString())
                    offset = end
                }
                val mimeType = when (audioFile.extension.lowercase()) {
                    "ogg"  -> "audio/ogg"
                    "mp4"  -> "audio/mp4"
                    "webm" -> "audio/webm"
                    else   -> "audio/ogg"
                }
                webSocket.send("mime:$mimeType|end-of-audio")
            }

            override fun onMessage(webSocket: WebSocket, text: String) {
                try {
                    val json = JSONObject(text)
                    when {
                        json.has("pronunciationScore") -> {
                            resultReceived = true
                            val result = RecitationResult(
                                recognizedText     = json.optString("recognizedText", ""),
                                pronunciationScore = json.optDouble("pronunciationScore", 0.0),
                                accuracyScore      = json.optDouble("accuracyScore", 0.0),
                                fluencyScore       = json.optDouble("fluencyScore", 0.0),
                                completenessScore  = json.optDouble("completenessScore", 0.0)
                            )
                            onResult(result)
                            webSocket.close(1000, "done")
                        }
                        json.optString("type") == "error" -> {
                            resultReceived = true
                            onError(json.optString("message", "সার্ভার ত্রুটি"))
                            webSocket.close(1000, "error")
                        }
                    }
                } catch (e: Exception) {
                    onError("ফলাফল পার্স করা যায়নি: ${e.message}")
                }
            }

            override fun onClosed(webSocket: WebSocket, code: Int, reason: String) {
                if (!resultReceived) {
                    onError("কোনো কথা শোনা যায়নি। দয়া করে আবার চেষ্টা করুন।")
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
