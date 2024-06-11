package com.github.kevdadev.mediahubandroid.agent

import com.github.kevdadev.mediahubandroid.model.MediaItem
import okhttp3.ResponseBody
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Path
import retrofit2.http.Query

interface MediaAgent {

   @GET("media/")
   suspend fun getMedia(): Response<List<MediaItem>>

   @GET("media")
   suspend fun getMedia(@Query("path") path: String? = null): Response<List<MediaItem>>

   @GET("media/thumbnail")
    suspend fun getThumbnail(@Query(value = "path") path: String): Response<ResponseBody>
}