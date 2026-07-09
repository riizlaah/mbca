package com.example.mbca

import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.material3.Typography
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.Dp
import androidx.compose.ui.unit.dp


@Composable
fun typ(): Typography {
    return MaterialTheme.typography
}

@Composable
fun ErrText(errMsg: String, modifier: Modifier = Modifier) {
    if(errMsg.isNotEmpty()) Text(errMsg, modifier.padding(vertical = 12.dp), color = Color.Red, textAlign = TextAlign.Center)
}

@Composable
fun LoadingOrContent(loading: Boolean, content: @Composable () -> Unit) {
    if(loading) CircularProgressIndicator(Modifier.size(24.dp), color = Color.White)
    else content()
}

fun corner(size: Dp = 12.dp): RoundedCornerShape {
    return RoundedCornerShape(size)
}