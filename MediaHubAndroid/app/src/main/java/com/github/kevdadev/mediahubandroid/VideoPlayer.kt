package com.github.kevdadev.mediahubandroid

import android.content.res.Configuration
import android.os.Bundle
import android.view.View
import android.view.WindowManager
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.annotation.OptIn
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.remember
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.viewinterop.AndroidView
import androidx.media3.common.MediaItem
import androidx.media3.common.util.UnstableApi
import androidx.media3.exoplayer.ExoPlayer
import androidx.media3.exoplayer.source.DefaultMediaSourceFactory
import androidx.media3.exoplayer.source.MediaSourceFactory
import androidx.media3.ui.PlayerControlView
import androidx.media3.ui.PlayerView
import com.github.kevdadev.mediahubandroid.service.VideoAuthDataSourceFactory
import com.github.kevdadev.mediahubandroid.ui.theme.MediaHubAndroidTheme
import kotlinx.coroutines.flow.MutableStateFlow
import retrofit2.http.Path

class VideoPlayer : ComponentActivity() {

    lateinit var player: ExoPlayer

    val mediaItemPathFlow = MutableStateFlow("")

    @OptIn(UnstableApi::class)
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        mediaItemPathFlow.value = intent.getStringExtra("mediaItemPath")
            ?: throw IllegalArgumentException("Media item path not provided")

        val userAgent = "MediaHubAndroid"
        val bearerToken = CredentialsHolder.credentials?.accessToken
            ?: throw IllegalArgumentException("No access token found")
        val httpDataSourceFactory = VideoAuthDataSourceFactory(userAgent, bearerToken)
        val mediaSourceFactory = DefaultMediaSourceFactory(httpDataSourceFactory)
        player = ExoPlayer.Builder(this@VideoPlayer)
            .setMediaSourceFactory(mediaSourceFactory)
            .build()

        val uri = "${getString(R.string.api_url)}media/file?path=${mediaItemPathFlow.value}"
        val mediaItem = MediaItem.fromUri(uri)
        player.setMediaItem(mediaItem)
        player.playWhenReady = true
        setContent {
            MediaHubAndroidTheme {
                // A surface container using the 'background' color from the theme
                Player()
            }
        }
    }

    override fun onConfigurationChanged(newConfig: Configuration) {
        super.onConfigurationChanged(newConfig)

        // Check the orientation of the new configuration
        if (newConfig.orientation == Configuration.ORIENTATION_LANDSCAPE) {
            window.setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN)
        } else if (newConfig.orientation == Configuration.ORIENTATION_PORTRAIT){
            window.setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN)
        }
    }

    override fun onWindowFocusChanged(hasFocus: Boolean) {
        super.onWindowFocusChanged(hasFocus)
        if (hasFocus) {
            hideSystemUI()
        }
    }

    private fun hideSystemUI() {
        window.decorView.systemUiVisibility = (View.SYSTEM_UI_FLAG_IMMERSIVE
                // Set the content to appear under the system bars so that the
                // content doesn't resize when the system bars hide and show.
                or View.SYSTEM_UI_FLAG_LAYOUT_STABLE
                or View.SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION
                or View.SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN
                // Hide the nav bar and status bar
                or View.SYSTEM_UI_FLAG_HIDE_NAVIGATION
                or View.SYSTEM_UI_FLAG_FULLSCREEN)
    }

    override fun onDestroy() {
        super.onDestroy()
        player.release() // Release the player when it's no longer needed
    }

    @OptIn(UnstableApi::class)
    @Composable
    fun Player() {
        val playerView = remember {
            val playerView = PlayerView(this)
            playerView.setShowSubtitleButton(true)
            playerView.setShowBuffering(PlayerView.SHOW_BUFFERING_WHEN_PLAYING)
            playerView
        }

        Row(modifier = Modifier.fillMaxSize().background(Color.Black), verticalAlignment = androidx.compose.ui.Alignment.CenterVertically) {
            AndroidView(
                factory = { playerView },
                modifier = Modifier.fillMaxWidth(),
            ) { _ ->
                playerView.player = player
            }
        }
    }
}