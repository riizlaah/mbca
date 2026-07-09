package com.example.mbca

import androidx.compose.animation.animateColor
import androidx.compose.animation.core.RepeatMode
import androidx.compose.animation.core.infiniteRepeatable
import androidx.compose.animation.core.rememberInfiniteTransition
import androidx.compose.animation.core.tween
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.material3.Typography
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.getValue
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.ImageBitmap
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp


class ImageLoader {
    val caches = mutableMapOf<String, ImageBitmap>()

    suspend fun loadImg(path: String): ImageBitmap? {
        caches[path]?.let { return it }
        val img = HttpClient.fetchImg(path)
        if (img != null) caches[path] = img
        return img
    }

    fun hasCache(path: String): Boolean {
        return caches.contains(path)
    }
}

@Composable
fun NetImg(
    path: String,
    contentDescription: String,
    modifier: Modifier = Modifier,
    contentScale: ContentScale = ContentScale.Fit
) {
    val imgLoader = remember { ImageLoader() }
    var img by remember { mutableStateOf<ImageBitmap?>(null) }
    var loading by remember { mutableStateOf(false) }
    var isError by remember { mutableStateOf(false) }

    LaunchedEffect(path) {
        if (!imgLoader.hasCache(path)) loading = true
        img = imgLoader.loadImg(path)
        if (img == null) isError = true
        loading = false
    }

    when {
        loading -> {
            val trans = rememberInfiniteTransition()
            val color by trans.animateColor(
                Color.Gray, Color.LightGray, infiniteRepeatable(
                    tween(500),
                    RepeatMode.Reverse
                )
            )
            Box(modifier.background(color))
        }
        isError -> {
            Box(modifier, contentAlignment = Alignment.Center) {
                Text("Failed to load image")
            }
        }
        img != null -> {
            Image(img!!, contentDescription, modifier, contentScale = contentScale)
        }
    }
}

@Composable
fun typ(): Typography {
    return MaterialTheme.typography
}

@Composable
fun ErrText(errMsg: String, modifier: Modifier = Modifier) {
    if (errMsg.isNotEmpty()) Text(
        errMsg,
        modifier.padding(vertical = 12.dp),
        color = Color.Red,
        textAlign = TextAlign.Center
    )
}

@Composable
fun LoadingOrContent(loading: Boolean, content: @Composable () -> Unit) {
    if (loading) CircularProgressIndicator(Modifier.size(24.dp), color = Color.White)
    else content()
}

fun corner(size: Dp = 12.dp): RoundedCornerShape {
    return RoundedCornerShape(size)
}

fun corner(size: Int): RoundedCornerShape {
    return RoundedCornerShape(size)
}