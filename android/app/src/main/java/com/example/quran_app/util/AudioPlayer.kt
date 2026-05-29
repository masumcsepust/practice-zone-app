package com.example.quran_app.util

import android.content.Context
import androidx.media3.common.MediaItem
import androidx.media3.exoplayer.ExoPlayer

class AudioPlayer(private val context: Context) {
    private var exoPlayer: ExoPlayer? = null

    fun play(url: String) {
        exoPlayer?.release()
        exoPlayer = ExoPlayer.Builder(context).build().apply {
            setMediaItem(MediaItem.fromUri(url))
            prepare()
            play()
        }
    }

    fun release() {
        exoPlayer?.release()
        exoPlayer = null
    }
}
