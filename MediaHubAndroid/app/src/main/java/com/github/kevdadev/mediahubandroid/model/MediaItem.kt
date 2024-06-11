package com.github.kevdadev.mediahubandroid.model
import com.squareup.moshi.JsonClass

@JsonClass(generateAdapter = true)
data class MediaItem(
    val name: String,
    val path: String,
    val thumbnailUrl: String?,
    val type: Int,
    val children: List<MediaItem>?
)
