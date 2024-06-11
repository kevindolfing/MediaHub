package com.github.kevdadev.mediahubandroid.service


import androidx.annotation.OptIn
import androidx.media3.common.util.UnstableApi
import androidx.media3.datasource.DataSource
import androidx.media3.datasource.DefaultHttpDataSource


class VideoAuthDataSourceFactory(private val userAgent: String, private val bearerToken: String) : DataSource.Factory {
    @OptIn(UnstableApi::class)
    private fun buildHttpHeaders(bearerToken: String): MutableMap<String, String> {
        val headers = mutableMapOf<String, String>()
        headers["Authorization"] = "Bearer $bearerToken"
        return headers
    }

    @UnstableApi
    override fun createDataSource(): DataSource {
        val httpDataSourceFactory = DefaultHttpDataSource.Factory()
        httpDataSourceFactory.setUserAgent(userAgent)
        httpDataSourceFactory.setDefaultRequestProperties(buildHttpHeaders(bearerToken))
        return httpDataSourceFactory.createDataSource()
    }

}

