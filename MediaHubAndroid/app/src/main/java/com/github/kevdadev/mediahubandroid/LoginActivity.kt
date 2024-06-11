package com.github.kevdadev.mediahubandroid

import android.app.AlertDialog
import android.content.Intent
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.Button
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.tooling.preview.Preview
import com.auth0.android.authentication.AuthenticationException
import com.auth0.android.authentication.storage.CredentialsManagerException
import com.auth0.android.callback.Callback
import com.auth0.android.result.Credentials
import com.github.kevdadev.mediahubandroid.service.AuthenticationService
import com.github.kevdadev.mediahubandroid.ui.theme.MediaHubAndroidTheme

class LoginActivity : ComponentActivity() {
    private lateinit var authenticationService: AuthenticationService
    override fun onResume() {
        super.onResume()
        val callBack = object : Callback<Credentials, CredentialsManagerException> {
            override fun onFailure(exception: CredentialsManagerException) {
                AlertDialog.Builder(this@LoginActivity)
                    .setTitle("Login failed")
                    .setMessage(exception.message)
                    .setPositiveButton("OK", null)
                    .show()
            }

            override fun onSuccess(credentials: Credentials) {
                CredentialsHolder.credentials = credentials
                val intent = Intent(this@LoginActivity, MediaCatalog::class.java)
                startActivity(intent)
            }
        }
        authenticationService.authenticate(callBack)

    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            MediaHubAndroidTheme {
                // A surface container using the 'background' color from the theme
                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = MaterialTheme.colorScheme.background
                ) {
                    Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                        LoginButton()
                    }
                }
            }
        }

        authenticationService = AuthenticationService(this)
            }

    @Composable
    fun LoginButton() {
        //green button with text login that logs in
        Button(
            onClick = { loginWithBrowser() }) {
            Text("Login")
        }
    }

    private fun loginWithBrowser() {
        val callback = object : Callback<Credentials, AuthenticationException> {
            override fun onFailure(exception: AuthenticationException) {
                AlertDialog.Builder(this@LoginActivity)
                    .setTitle("Login failed")
                    .setMessage(exception.message)
                    .setPositiveButton("OK", null)
                    .show()
            }

            override fun onSuccess(credentials: Credentials) {
                val intent = Intent(this@LoginActivity, MediaCatalog::class.java)
                startActivity(intent)
            }
        }
        authenticationService.openLogin(callback)
    }

    @Composable
    @Preview
    fun PreviewLoginButton() {

        MediaHubAndroidTheme {
            Surface {
                Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                    LoginButton()
                }
            }
        }
    }
}