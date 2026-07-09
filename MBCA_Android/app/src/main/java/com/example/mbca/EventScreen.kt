package com.example.mbca

import android.content.Intent
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.Icon
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.getValue
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter

@Composable
fun EventScreen(modifier: Modifier) {
    val events = remember { mutableStateListOf<Event>() }
    val scope = rememberCoroutineScope()
    val ctx = LocalContext.current

    LaunchedEffect(Unit) {
        val arr = HttpClient.getEvents()
        println(arr)
        events.addAll(arr)
    }

    LazyColumn(modifier.padding(horizontal = 12.dp)) {
        item {
            Spacer(Modifier.height(12.dp))
            OutlinedButton({
                scope.launch {
                    HttpClient.token = ""
                    HttpClient.saveToken()
                    HttpClient.profile = null
                    val int = Intent(ctx, MainActivity::class.java).apply {
                        flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                    }
                    ctx.startActivity(int)
                }
            }, shape = corner()) {
                Icon(painterResource(R.drawable.arr_back), "Back")
                Spacer(Modifier.width(4.dp))
                Text("Log Out", fontWeight = FontWeight.Bold)
            }
            Spacer(Modifier.height(24.dp))
            Text(
                "Upcoming Event",
                fontWeight = FontWeight.Bold,
                fontSize = typ().headlineSmall.fontSize
            )
            Text("The magical events are waiting for you")
            Spacer(Modifier.height(24.dp))
        }
        items(events) { item ->
            Column(
                Modifier
                    .padding(vertical = 12.dp)
                    .fillMaxWidth()
                    .shadow(4.dp, corner())
                    .clip(corner())
                    .background(Color.White)
                    .padding(12.dp)
                    .clickable(onClick = {
                        val int = Intent(ctx, EventDetailActivity::class.java).apply { putExtra("id", item.id) }
                        ctx.startActivity(int)
                    })
            ) {
                NetImg(item.banners[0], item.title, Modifier.fillMaxWidth(), ContentScale.FillWidth)
                Spacer(Modifier.height(8.dp))
                Row(Modifier.fillMaxWidth(), verticalAlignment = Alignment.CenterVertically) {
                    Text(item.title, fontWeight = FontWeight.Bold)
                    Spacer(Modifier.weight(1f))
                    Text("$${item.price}/person", fontWeight = FontWeight.Bold)
                }
                Spacer(Modifier.height(8.dp))
                val date = item.date.format(DateTimeFormatter.ofPattern("dd-MM-yyyy"))
                val startTime = item.startTime.format(DateTimeFormatter.ofPattern("hh:mm a"))
                val endTime = item.endTime.format(DateTimeFormatter.ofPattern("hh:mm a"))
                Text("$date ($startTime - $endTime)")
            }
        }
    }
}