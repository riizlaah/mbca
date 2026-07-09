package com.example.mbca

import android.content.Intent
import android.os.Bundle
import android.widget.Space
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.material3.Button
import androidx.compose.material3.Icon
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import com.example.mbca.ui.theme.HttpClient
import com.example.mbca.ui.theme.MBCATheme
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import kotlin.time.Duration.Companion.milliseconds

class OTPVerificationActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            MBCATheme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    Column(
                        Modifier
                            .fillMaxSize()
                            .padding(innerPadding)
                            .padding(24.dp)
                    ) {
                        var code by remember { mutableStateOf("") }
                        var errMsg by remember { mutableStateOf("") }
                        var loading by remember { mutableStateOf(false) }
                        val scope = rememberCoroutineScope()
                        var secondLeft by remember { mutableIntStateOf(30) }
                        var timerRunning by remember { mutableStateOf(false) }
                        val ctx = LocalContext.current

                        LaunchedEffect(timerRunning, secondLeft) {
                            if (timerRunning && secondLeft > 0) {
                                delay(1000.milliseconds)
                                secondLeft--
                            } else if (secondLeft == 0) {
                                timerRunning = false
                                secondLeft = 30
                            }
                        }

                        TextButton({ finish() }) {
                            Icon(painterResource(R.drawable.arr_back), "Back")
                            Spacer(Modifier.width(8.dp))
                            Text("Back", fontWeight = FontWeight.Bold)
                        }
                        Column(
                            Modifier.weight(1f),
                            verticalArrangement = Arrangement.Center,
                            horizontalAlignment = Alignment.CenterHorizontally
                        ) {
                            Text(
                                "Verify Your Account",
                                Modifier.fillMaxWidth(),
                                fontWeight = FontWeight.Bold,
                                fontSize = typ().titleLarge.fontSize
                            )
                            Spacer(Modifier.height(8.dp))
                            OutlinedTextField(
                                code,
                                { code = it },
                            )
                            Spacer(Modifier.height(24.dp))
                            Button({
                                if (code.isBlank()) {
                                    errMsg = "Code required"
                                    return@Button
                                }
                                errMsg = ""
                                scope.launch {
                                    loading = true
                                    when (val msg = HttpClient.verifyOTP(code)) {
                                        "ok" -> {
                                            val int = Intent(ctx, HomeActivity::class.java).apply {
                                                flags =
                                                    Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                                            }
                                            ctx.startActivity(int)
                                        }

                                        else -> errMsg = msg
                                    }
                                    loading = false
                                }
                            }, Modifier.fillMaxWidth(), shape = corner()) {
                                LoadingOrContent(loading) {
                                    Text("Verify")
                                }
                            }
                            Spacer(Modifier.height(12.dp))
                            ErrText(errMsg, Modifier.fillMaxWidth())
                            Spacer(Modifier.height(48.dp))
                            Text("Code not received?", fontWeight = FontWeight.Bold)
                            TextButton(
                                {
                                    errMsg = ""
                                    scope.launch {
                                        when (val msg = HttpClient.newOTP()) {
                                            "ok" -> {
                                                timerRunning = true
                                            }

                                            else -> {
                                                errMsg = msg
                                            }
                                        }
                                    }
                                },
                                enabled = !timerRunning
                            ) { Text(if (timerRunning) "Code Sent. (cooldown ${secondLeft}s)" else "Resend code") }

                        }

                    }
                }
            }
        }
    }
}
