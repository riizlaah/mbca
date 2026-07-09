package com.example.mbca

import android.content.Intent
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Button
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.getValue
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.unit.dp
import com.example.mbca.ui.theme.MBCATheme
import kotlinx.coroutines.launch

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        HttpClient.prefs = getSharedPreferences("prefs", MODE_PRIVATE)
        HttpClient.loadToken()
        setContent {
            MBCATheme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    Column(
                        Modifier
                            .fillMaxSize()
                            .padding(innerPadding)
                            .padding(24.dp)
                    ) {
                        var usernameOrEmail by remember { mutableStateOf("") }
                        var password by remember { mutableStateOf("") }
                        var errMsg by remember { mutableStateOf("") }
                        var loading by remember { mutableStateOf(false) }
                        val scope = rememberCoroutineScope()
                        val ctx = LocalContext.current

                        LaunchedEffect(Unit) {
                            HttpClient.loadToken()
                            if (HttpClient.me()) {
                                if (!(HttpClient.profile?.isActivated ?: false)) {
                                    val int = Intent(ctx, OTPVerificationActivity::class.java)
                                    ctx.startActivity(int)
                                } else {
                                    val int = Intent(ctx, HomeActivity::class.java).apply {
                                        flags =
                                            Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                                    }
                                    ctx.startActivity(int)
                                }
                            }
                        }

                        Text(
                            "Museum Bernis Ches Ainstein",
                            fontWeight = FontWeight.Bold,
                            fontSize = typ().headlineSmall.fontSize
                        )
                        Spacer(Modifier.height(24.dp))
                        Text("Username/Email")
                        OutlinedTextField(
                            usernameOrEmail,
                            { usernameOrEmail = it },
                            Modifier.fillMaxWidth(),
                            shape = corner()
                        )
                        Spacer(Modifier.height(12.dp))
                        Text("Password")
                        OutlinedTextField(
                            password,
                            { password = it },
                            Modifier.fillMaxWidth(),
                            shape = corner(),
                            visualTransformation = PasswordVisualTransformation()
                        )
                        Spacer(Modifier.height(12.dp))
                        ErrText(errMsg, Modifier.fillMaxWidth())
                        Column(
                            Modifier.fillMaxWidth(),
                            horizontalAlignment = Alignment.CenterHorizontally
                        ) {
                            Button({
                                if (usernameOrEmail.isBlank()) {
                                    errMsg = "Username/Email required"
                                    return@Button
                                }
                                if (password.isBlank()) {
                                    errMsg = "Password required"
                                    return@Button
                                }
                                errMsg = ""
                                scope.launch {
                                    loading = true
                                    when (val msg = HttpClient.login(usernameOrEmail, password)) {
                                        "ok" -> {
                                            HttpClient.me()
                                            if (!(HttpClient.profile?.isActivated ?: false)) {
                                                val int =
                                                    Intent(ctx, OTPVerificationActivity::class.java)
                                                ctx.startActivity(int)
                                            } else {
                                                val int =
                                                    Intent(ctx, HomeActivity::class.java).apply {
                                                        flags =
                                                            Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                                                    }
                                                ctx.startActivity(int)
                                            }
                                        }

                                        else -> errMsg = msg
                                    }
                                    loading = false
                                }
                            }, Modifier.fillMaxWidth(), shape = corner()) {
                                LoadingOrContent(loading) {
                                    Text("Login")
                                }
                            }
                            Spacer(Modifier.height(12.dp))
                            TextButton({
                                val int = Intent(ctx, RegisterActivity::class.java)
                                ctx.startActivity(int)
                            }) { Text("Don't have an account?") }
                        }

                    }
                }
            }
        }
    }
}
