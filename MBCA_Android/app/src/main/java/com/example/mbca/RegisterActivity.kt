package com.example.mbca

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Button
import androidx.compose.material3.DropdownMenu
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
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

class RegisterActivity : ComponentActivity() {
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
                        var fullName by remember { mutableStateOf("") }
                        var username by remember { mutableStateOf("") }
                        var email by remember { mutableStateOf("") }
                        var phoneNumber by remember { mutableStateOf("") }
                        var password by remember { mutableStateOf("") }
                        val phonePrefixes = remember { mutableStateListOf<String>() }
                        var errMsg by remember { mutableStateOf("") }
                        var loading by remember { mutableStateOf(false) }
                        var phonePrefixesOpened by remember { mutableStateOf(false) }
                        var selectedPrefix by remember { mutableStateOf("") }
                        val scope = rememberCoroutineScope()
                        val ctx = LocalContext.current

                        LaunchedEffect(Unit) {
                            val arr = HttpClient.getPhonePrefixes()
                            println(arr)
                            if(arr.isNotEmpty()) {
                                phonePrefixes.addAll(arr)
                                selectedPrefix = arr[0]
                            }
                        }

                        Text(
                            "Museum Bernis Ches Ainstein",
                            fontWeight = FontWeight.Bold,
                            fontSize = typ().headlineSmall.fontSize
                        )
                        Spacer(Modifier.height(24.dp))
                        Text("Full Name")
                        OutlinedTextField(
                            fullName,
                            { fullName = it },
                            Modifier.fillMaxWidth(),
                            shape = corner()
                        )
                        Spacer(Modifier.height(12.dp))
                        Text("Email")
                        OutlinedTextField(
                            email,
                            { email = it },
                            Modifier.fillMaxWidth(),
                            shape = corner()
                        )
                        Spacer(Modifier.height(12.dp))
                        Text("Username")
                        OutlinedTextField(
                            username,
                            { username = it },
                            Modifier.fillMaxWidth(),
                            shape = corner()
                        )
                        Spacer(Modifier.height(12.dp))
                        Text("Phone Number")
                        Row(Modifier.fillMaxWidth()) {
                            Box(contentAlignment = Alignment.Center) {
                                Button(
                                    { phonePrefixesOpened = !phonePrefixesOpened },
                                    shape = corner()
                                ) {
                                    Text(selectedPrefix)
                                }
                                DropdownMenu(phonePrefixesOpened, { phonePrefixesOpened = false }) {
                                    phonePrefixes.forEach { prefix ->
                                        DropdownMenuItem({ Text(prefix) }, {
                                            selectedPrefix = prefix
                                            phonePrefixesOpened = false
                                        })
                                    }
                                }
                            }
                            OutlinedTextField(
                                phoneNumber,
                                { phoneNumber = it },
                                Modifier.weight(1f),
                                shape = corner()
                            )
                        }
                        Spacer(Modifier.height(12.dp))
                        Text("Password")
                        OutlinedTextField(
                            password,
                            { password = it },
                            Modifier.fillMaxWidth(),
                            visualTransformation = PasswordVisualTransformation()
                        )
                        Spacer(Modifier.height(12.dp))
                        ErrText(errMsg, Modifier.fillMaxWidth())
                        Column(
                            Modifier.fillMaxWidth(),
                            horizontalAlignment = Alignment.CenterHorizontally
                        ) {
                            Button({
                                if (fullName.isBlank()) {
                                    errMsg = "Full Name required"
                                    return@Button
                                }
                                if (username.isBlank()) {
                                    errMsg = "Username required"
                                    return@Button
                                }
                                if (email.isBlank()) {
                                    errMsg = "Email required"
                                    return@Button
                                }
                                if (selectedPrefix.isBlank() || phoneNumber.isBlank()) {
                                    errMsg = "Phone Number required"
                                    return@Button
                                }
                                if (password.isBlank()) {
                                    errMsg = "Password required"
                                    return@Button
                                }
                                errMsg = ""
                                scope.launch {
                                    loading = true
                                    when (val msg = HttpClient.register(fullName, email, username, selectedPrefix + phoneNumber, password)) {
                                        "ok" -> {finish()}
                                        else -> errMsg = msg
                                    }
                                    loading = false
                                }
                            }, Modifier.fillMaxWidth()) {
                                LoadingOrContent(loading) {
                                    Text("Register")
                                }
                            }
                            Spacer(Modifier.height(12.dp))
                            TextButton({finish()}) { Text("Already have an account?") }
                        }

                    }
                }
            }
        }
    }
}
