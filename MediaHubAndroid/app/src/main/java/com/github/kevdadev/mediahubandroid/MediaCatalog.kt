package com.github.kevdadev.mediahubandroid

import android.app.AlertDialog
import android.content.Intent
import android.graphics.BitmapFactory
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.Image
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.wrapContentHeight
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material.icons.filled.Person
import androidx.compose.material3.Card
import androidx.compose.material3.CenterAlignedTopAppBar
import androidx.compose.material3.DropdownMenu
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.MediumTopAppBar
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBar
import androidx.compose.material3.TopAppBarColors
import androidx.compose.material3.TopAppBarDefaults
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.ImageBitmap
import androidx.compose.ui.graphics.asImageBitmap
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import com.auth0.android.authentication.AuthenticationException
import com.auth0.android.callback.Callback
import com.auth0.android.result.Credentials
import com.github.kevdadev.mediahubandroid.agent.MediaAgent
import com.github.kevdadev.mediahubandroid.model.MediaItem
import com.github.kevdadev.mediahubandroid.service.AuthenticationService
import com.github.kevdadev.mediahubandroid.ui.theme.MediaHubAndroidTheme
import com.squareup.moshi.Moshi
import com.squareup.moshi.kotlin.reflect.KotlinJsonAdapterFactory
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.moshi.MoshiConverterFactory

class MediaCatalog() : ComponentActivity() {
    private lateinit var mediaAgent: MediaAgent
    private lateinit var authenticationService: AuthenticationService

    private val pathFlow = MutableStateFlow<String?>(null)

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        authenticationService = AuthenticationService(this)
        mediaAgent = provideMediaAgent()
        setContent {
            MediaHubAndroidTheme {
                // A surface container using the 'background' color from the theme
                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = MaterialTheme.colorScheme.background
                ) {
                    Column {
                        MediaCatalogBreadCrumb()
                        MediaCatalogComposable()
                    }
                }
            }
        }
    }

    @OptIn(ExperimentalMaterial3Api::class)
    @Composable
    fun MediaCatalogBreadCrumb() {
        val path = pathFlow.collectAsState()
        val showMenu = remember { mutableStateOf(false) }

        CenterAlignedTopAppBar(
            title = {
                Text(
                    text = path.value ?: "Media Catalog",
                    textAlign = TextAlign.Center,
                    modifier = Modifier
                        .fillMaxWidth()
                        .height(56.dp)
                        .padding(8.dp),
                )
            },
            colors = TopAppBarDefaults.topAppBarColors(
                containerColor = MaterialTheme.colorScheme.primaryContainer
            ),

            navigationIcon = {
                IconButton(onClick = { handleBackButton() }) {
                    Icon(
                        imageVector = Icons.Filled.ArrowBack,
                        contentDescription = getString(R.string.back_button)
                    )
                }
            },
            actions = {
                IconButton(onClick = { showMenu.value = !showMenu.value }) {
                    Icon(
                        imageVector = Icons.Filled.Person,
                        contentDescription = getString(R.string.profile_button)
                    )
                    DropdownMenu(
                        expanded = showMenu.value,
                        onDismissRequest = { showMenu.value = false }
                    ) {
                        DropdownMenuItem(
                            text = { Text(stringResource(R.string.log_out)) },
                            onClick = { logOut() })
                    }
                }
            }
        )
    }

    private fun logOut() {
        val logoutCallback = object : Callback<Void?, AuthenticationException> {
            override fun onSuccess(result: Void?) {
                //redirect to login screen
                val intent = Intent(this@MediaCatalog, LoginActivity::class.java)
                startActivity(intent)
            }

            override fun onFailure(error: AuthenticationException) {
                AlertDialog.Builder(this@MediaCatalog)
                    .setTitle("Logout failed")
                    .setMessage(error.message)
                    .setPositiveButton("OK", null)
                    .show()
            }
        }
        authenticationService.logout(logoutCallback)
    }

    @Composable
    @Preview
    fun PreviewMediaCatalogBreadCrumb() {
        MediaCatalogBreadCrumb()
    }

    private fun handleBackButton() {
        val path = pathFlow.value
        if (path != null) {
            val newPath = if (path.contains('/')) path.substring(0, path.lastIndexOf('/')) else null
            pathFlow.value = newPath
        }
    }

    @Composable
    fun MediaCatalogComposable() {
        var media by remember { mutableStateOf(emptyList<MediaItem>()) }
        val path = pathFlow.collectAsState()
        LaunchedEffect(path.value) {
            val response: Response<List<MediaItem>> = if (path.value != null) {
                mediaAgent.getMedia(path.value)
            } else {
                mediaAgent.getMedia()
            }
            if (response.isSuccessful) {
                media = response.body() ?: emptyList()
            } else {
                withContext(Dispatchers.Main) {
                    AlertDialog.Builder(this@MediaCatalog)
                        .setTitle("Error")
                        .setMessage("Failed to get media")
                        .setPositiveButton("OK", null)
                        .show()
                }
            }
        }

        if (media.isEmpty()) {
            Text(text = "No media found")
        } else {
            LazyColumn {
                items(media) { item ->
                    MediaCatalogItem(mediaItem = item)
                }
            }
        }
    }

    @Composable
    @Preview
    fun PreviewMediaCatalogComposable() {
        MediaCatalogComposable()
    }

    @Composable
    fun MediaCatalogItem(mediaItem: MediaItem) {
        val coroutineScope = rememberCoroutineScope()
        var image by remember { mutableStateOf<ImageBitmap?>(null) }

        LaunchedEffect(mediaItem.path) {
            coroutineScope.launch(Dispatchers.IO) {
                if (mediaItem.thumbnailUrl != null) {
                    val response = mediaAgent.getThumbnail(mediaItem.thumbnailUrl)
                    if (response.isSuccessful) {
                        val bytes = response.body()?.bytes()
                        if (bytes != null) {
                            val bitmap = BitmapFactory.decodeByteArray(bytes, 0, bytes.size)
                            image = bitmap.asImageBitmap()
                        }
                    }
                } else {
                    image = null
                }
            }
        }

        Card(
            modifier = Modifier
                .fillMaxWidth()
                .wrapContentHeight()
                .padding(8.dp)
                .clickable(onClick = {
                    when (mediaItem.type) {
                        0 -> pathFlow.value = mediaItem.path
                        1 -> {
                            val intent = Intent(this@MediaCatalog, VideoPlayer::class.java)
                            intent.putExtra("mediaItemPath", mediaItem.path)
                            startActivity(intent)
                        }
                    }
                }),
        ) {
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(8.dp)
            ) {
                if (image != null) {
                    Image(
                        bitmap = image!!,
                        contentDescription = "thumbnail",
                        alignment = Alignment.Center
                    )
                } else {
                    val resource = painterResource(id = R.drawable.default_thumbnail)
                    Image(
                        painter = resource,
                        contentDescription = "thumbnail",
                        alignment = Alignment.Center
                    )
                }
            }
            Column {
                Text(textAlign = TextAlign.Center, text = mediaItem.name)
            }
        }
    }

    @Composable
    @Preview
    fun PreviewMediaCatalogItem() {
        MediaCatalogItem(MediaItem("Attack on Titan", "title", "thumbnailUrl", 0, listOf()))
    }

    private fun provideMediaAgent(): MediaAgent {
        val moshi: Moshi = Moshi.Builder()
            .add(KotlinJsonAdapterFactory())
            .build()

        val loggingInterceptor = HttpLoggingInterceptor().apply {
            level = HttpLoggingInterceptor.Level.BODY
        }

        val okHttpClient = OkHttpClient.Builder()
            .addInterceptor { chain ->
                val request = chain.request().newBuilder()
                    .addHeader(
                        "Authorization",
                        "Bearer " + CredentialsHolder.credentials?.accessToken
                    )
                    .build()
                chain.proceed(request)
            }
            .addInterceptor(loggingInterceptor)
            .build()

        val retrofit = Retrofit.Builder()
            .baseUrl(getString(R.string.api_url))
            .client(okHttpClient)
            .addConverterFactory(MoshiConverterFactory.create(moshi))
            .build()

        return retrofit.create(MediaAgent::class.java)
    }
}