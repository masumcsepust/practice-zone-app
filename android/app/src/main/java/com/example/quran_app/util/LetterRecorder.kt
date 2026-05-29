package com.example.quran_app.util

import android.content.Context
import android.media.MediaRecorder
import android.os.Build
import java.io.File

/**
 * Thin wrapper around [MediaRecorder] that records mic audio to a temp file.
 *
 * On API 29+ (Android 10+) records as OGG/OPUS — the format Azure Speech SDK
 * accepts via AudioStreamContainerFormat.ANY.
 * On API < 29 falls back to MPEG-4/AAC_ADTS (Azure also accepts this).
 */
class LetterRecorder(private val context: Context) {

    private var recorder: MediaRecorder? = null
    private var outputFile: File? = null

    /** Start recording. Returns the [File] the audio is being written to. */
    fun start(): File {
        val ext  = if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.Q) "ogg" else "mp4"
        val file = File(context.cacheDir, "letter_rec.$ext").also { outputFile = it }

        recorder = if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.S) {
            MediaRecorder(context)
        } else {
            @Suppress("DEPRECATION")
            MediaRecorder()
        }

        recorder!!.apply {
            setAudioSource(MediaRecorder.AudioSource.MIC)
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.Q) {
                setOutputFormat(MediaRecorder.OutputFormat.OGG)
                setAudioEncoder(MediaRecorder.AudioEncoder.OPUS)
            } else {
                setOutputFormat(MediaRecorder.OutputFormat.MPEG_4)
                setAudioEncoder(MediaRecorder.AudioEncoder.AAC)
            }
            setAudioSamplingRate(16_000)
            setAudioChannels(1)
            setOutputFile(file.absolutePath)
            prepare()
            start()
        }
        return file
    }

    /**
     * Stop recording. Returns the recorded [File], or null on error.
     * Always call [release] afterwards if you don't call this.
     */
    fun stop(): File? = try {
        recorder?.stop()
        release()
        outputFile
    } catch (e: Exception) {
        release()
        null
    }

    fun release() {
        recorder?.release()
        recorder = null
    }
}
