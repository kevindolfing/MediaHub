package com.github.kevdadev.mediahubandroid.service

import android.app.Activity
import android.app.AlertDialog
import androidx.core.content.ContextCompat.getString
import com.auth0.android.Auth0
import com.auth0.android.authentication.AuthenticationAPIClient
import com.auth0.android.authentication.AuthenticationException
import com.auth0.android.authentication.storage.CredentialsManager
import com.auth0.android.authentication.storage.CredentialsManagerException
import com.auth0.android.authentication.storage.SharedPreferencesStorage
import com.auth0.android.callback.Callback
import com.auth0.android.provider.WebAuthProvider
import com.auth0.android.result.Credentials
import com.github.kevdadev.mediahubandroid.CredentialsHolder
import com.github.kevdadev.mediahubandroid.R

class AuthenticationService(val context: Activity) {
    private val account: Auth0 = Auth0(
        getString(context, R.string.com_auth0_client_id),
        getString(context, R.string.com_auth0_domain)
    )
    private val authentication = AuthenticationAPIClient(account)
    private val storage = SharedPreferencesStorage(context)
    private val manager = CredentialsManager(authentication, storage)

    fun authenticate(callback: Callback<Credentials, CredentialsManagerException>) {
        val authentication = AuthenticationAPIClient(account)
        val storage = SharedPreferencesStorage(context)
        val manager = CredentialsManager(authentication, storage)

        if (manager.hasValidCredentials()) {
            manager.getCredentials(object : Callback<Credentials, CredentialsManagerException> {
                override fun onSuccess(credentials: Credentials) {
                    CredentialsHolder.credentials = credentials
                    callback.onSuccess(credentials)
                }

                override fun onFailure(error: CredentialsManagerException) {
                    callback.onFailure(error)
                }
            })
        }
    }

    fun openLogin(callback: Callback<Credentials, AuthenticationException>) {
        WebAuthProvider.login(account)
            .withScheme(getString(context, R.string.com_auth0_scheme))
            .withScope("openid profile email")
            .withAudience(getString(context, R.string.com_auth0_audience))
            // Launch the authentication passing the callback where the results will be received
            .start(context, object : Callback<Credentials, AuthenticationException> {
                // Called when there is an authentication failure
                override fun onFailure(error: AuthenticationException) {
                    callback.onFailure(error)
                    //alert
                    AlertDialog.Builder(context)
                        .setTitle("Login failed")
                        .setMessage(error.message)
                        .setPositiveButton("OK", null)
                        .show()
                }

                // Called when authentication completed successfully
                override fun onSuccess(result: Credentials) {
                    //store credentials
                    manager.saveCredentials(result)
                    CredentialsHolder.credentials = result

                    callback.onSuccess(result)
                }
            })
    }

    fun logout(callback : Callback<Void?, AuthenticationException>) {
        val logoutCallback = object : Callback<Void?, AuthenticationException> {
            override fun onSuccess(result: Void?) {
                manager.clearCredentials()
                callback.onSuccess(result)
            }

            override fun onFailure(error: AuthenticationException) {
                callback.onFailure(error)
            }
        }
        WebAuthProvider.logout(account)
            .withScheme(getString(context, R.string.com_auth0_scheme))
            .start(context, logoutCallback)
    }
}