package com.example.mbca.ui.theme

import android.content.SharedPreferences
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.getValue
import androidx.compose.runtime.setValue
import androidx.core.content.edit
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import org.json.JSONArray
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

data class HttpReq(
    val url: String,
    val method: String = "GET",
    val body: String = "",
    val headers: Map<String, String> = emptyMap(),
    val timeout: Int = 10000
)

data class HttpRes(
    val code: Int,
    val body: String? = null,
    val bytes: ByteArray? = null,
    val error: String? = null
)

data class Profile(
    val fullName: String,
    val username: String,
    val email: String,
    val phoneNumber: String,
    val role: String,
    val isActivated: Boolean
)

object HttpClient {
    val addr = "http://10.0.2.2:5000/"
    var token = ""
    var profile by mutableStateOf<Profile?>(null)
    lateinit var prefs: SharedPreferences

    fun loadToken() {
        token = prefs.getString("token", "") ?: ""
    }

    fun saveToken() {
        prefs.edit(true) {
            putString("token", token)
        }
    }

    fun send(req: HttpReq, getByte: Boolean = false): HttpRes {
        val conn = URL(req.url).openConnection() as HttpURLConnection
        return try {
            conn.run {
                requestMethod = req.method
                readTimeout = req.timeout
                connectTimeout = req.timeout
                req.headers.forEach { k, v -> setRequestProperty(k, v) }
                if(req.body.isNotEmpty() && req.method in listOf("POST", "PUT", "PATCH")) {
                    getOutputStream().buffered().use { it.write(req.body.toByteArray()) }
                }

                connect()
                val code = responseCode
                val body = if(getByte) null else {
                    if(code in 200..299) {
                        getInputStream().bufferedReader().use { it.readText() }
                    } else {
                        errorStream?.bufferedReader()?.use { it.readText() }
                    }
                }
                val bytes = if(!getByte) null else {
                    if(code in 200..299) {
                        getInputStream().buffered().use { it.readBytes() }
                    } else {
                        errorStream?.buffered()?.use { it.readBytes() }
                    }
                }
                HttpRes(code, body, bytes)
            }
        } catch (e: Exception) {
            e.printStackTrace()
            HttpRes(-1, error = e.message ?: "Network error")
        } finally {
            conn.disconnect()
        }
    }

    suspend fun jsonReq(route: String, method: String = "GET", body: String = "", errMsg: String = "Error", onParsingJSON: JSONObject.() -> Unit): String {
        val headers = if(token.isEmpty()) mapOf("content-type" to "application/json") else mapOf("content-type" to "application/json", "authorization" to "Bearer $token")
        val res = withContext(Dispatchers.IO) {
            send(HttpReq("${addr}mbca-api/v1/$route", method, body, headers))
        }
        if(res.body == null) return errMsg
        return try {
            val json = JSONObject(res.body)
            if(res.code != 200) json.optString("message", errMsg)
            else {
                json.run(onParsingJSON)
                "ok"
            }
        } catch (e: Exception) {
            e.printStackTrace()
            errMsg
        }
    }

    suspend fun login(usernameOrEmail: String, password: String): String {
        return jsonReq("users/login", "POST", """{
  "usernameOrEmail": "$usernameOrEmail",
  "password": "$password"
}""", "Login Failed") {
            token = getJSONObject("data").getString("token")
            println(token)
            saveToken()
        }
    }

    suspend fun me(): Boolean {
        val msg = jsonReq("users/me", "GET", errMsg = "Profile failed to fetch") {
            profile = getJSONObject("data").run {
                Profile(
                    getString("fullName"),
                    getString("username"),
                    getString("email"),
                    getString("phoneNumber"),
                    getString("role"),
                    getBoolean("isActivated"),
                )
            }
        }
        return msg == "ok"
    }

    suspend fun register(fullName: String, email: String, username: String, phoneNumber: String, password: String): String {
        return jsonReq("users/register", "POST", """{
  "username": "$username",
  "fullName": "$fullName",
  "email": "$email",
  "phoneNumber": "$phoneNumber",
  "password": "$password"
}""", "Register Failed") {}
    }

    suspend fun newOTP(): String {
        return jsonReq("otp/new", "POST", errMsg = "Request failed") {}
    }

    suspend fun verifyOTP(code: String): String {
        return jsonReq("otp/verify", "POST", """{
  "code": "$code"
}""","OTP verification failed") {
            token = getJSONObject("data").getString("newToken")
            saveToken()
        }
    }

    suspend fun getPhonePrefixes(): List<String> {
        val arr = mutableListOf<String>()
        jsonReq("phonePrefixes") {
            getJSONArray("data").run {
                for(i in 0 until length()) {
                    arr.add(getString(i))
                }
            }
        }
        return arr
    }
}

fun <T> JSONArray.mapToData(transform: JSONObject.() -> T): List<T> {
    val arr = mutableListOf<T>()
    for(i in 0 until length()) {
        arr.add(getJSONObject(i).run(transform))
    }
    return  arr
}