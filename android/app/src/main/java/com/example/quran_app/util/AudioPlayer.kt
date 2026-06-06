package com.example.quran_app.util

import android.content.Context
import androidx.media3.common.MediaItem
import androidx.media3.common.Player
import androidx.media3.exoplayer.ExoPlayer

class AudioPlayer(private val context: Context) {
    private var exoPlayer: ExoPlayer? = null

    fun play(url: String, onComplete: (() -> Unit)? = null) {
        exoPlayer?.release()
        exoPlayer = ExoPlayer.Builder(context).build().apply {
            if (onComplete != null) {
                addListener(object : Player.Listener {
                    override fun onPlaybackStateChanged(playbackState: Int) {
                        if (playbackState == Player.STATE_ENDED) onComplete()
                    }
                })
            }
            setMediaItem(MediaItem.fromUri(url))
            prepare()
            play()
        }
    }

    fun stop() {
        exoPlayer?.stop()
        exoPlayer?.release()
        exoPlayer = null
    }

    fun release() {
        exoPlayer?.release()
        exoPlayer = null
    }
}
