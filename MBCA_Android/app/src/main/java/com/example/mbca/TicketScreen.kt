package com.example.mbca

import android.widget.Toast
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.IconButtonDefaults
import androidx.compose.material3.Text
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
import androidx.compose.ui.draw.drawWithCache
import androidx.compose.ui.draw.drawWithContent
import androidx.compose.ui.draw.shadow
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.asAndroidBitmap
import androidx.compose.ui.graphics.graphicsLayer
import androidx.compose.ui.graphics.layer.GraphicsLayer
import androidx.compose.ui.graphics.layer.drawLayer
import androidx.compose.ui.graphics.rememberGraphicsLayer
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter
import kotlin.math.min

@Composable
fun TicketScreen(modifier: Modifier) {
    val events = remember { mutableStateListOf<Ticket>() }
    val ctx = LocalContext.current
    val scope = rememberCoroutineScope()

    LaunchedEffect(Unit) {
        val arr = HttpClient.getTickets()
        events.addAll(arr)
    }

    LazyColumn(modifier.padding(horizontal = 12.dp)) {
        item {
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
            val graphicsLayer = rememberGraphicsLayer()
            var downloading by remember { mutableStateOf(false) }
            Box(Modifier.padding(vertical = 12.dp).fillMaxWidth().drawWithContent {
                graphicsLayer.record {this@drawWithContent.drawContent()}
                drawLayer(graphicsLayer)
            }) {
                Column(
                    Modifier
                        .fillMaxWidth()
                        .shadow(4.dp, corner())
                        .clip(corner())
                        .background(Color.White)
                        .padding(12.dp)
                ) {
                    Box(Modifier.fillMaxWidth()) {
                        NetImg(
                            item.event.banners[0], item.event.title, Modifier.fillMaxWidth(),
                            ContentScale.FillWidth
                        )
                        if(!downloading) {
                            IconButton(
                                {
                                    scope.launch {
                                        downloading = true
                                        val bitmap = graphicsLayer.toImageBitmap().asAndroidBitmap()
                                        Toast.makeText(ctx, "Image captured (not downloaded yet)!", Toast.LENGTH_SHORT).show()
                                        downloading = false
                                    }
                                },
                                Modifier
                                    .align(Alignment.BottomEnd)
                                    .padding(12.dp),
                                shape = CircleShape,
                                colors = IconButtonDefaults.iconButtonColors(containerColor = Color.LightGray)
                            ) {
                                Icon(painterResource(R.drawable.download), "Download")
                            }
                        }
                    }
                    Spacer(Modifier.height(8.dp))
                    Row(Modifier.fillMaxWidth(), verticalAlignment = Alignment.CenterVertically) {
                        Text("${item.event.title} (${item.qty}pcs)", fontWeight = FontWeight.Bold)
                        Spacer(Modifier.weight(1f))
                        Text("#T%05d".format(item.id), fontWeight = FontWeight.Bold)
                    }
                    Spacer(Modifier.height(8.dp))
                    val date = item.event.date.format(DateTimeFormatter.ofPattern("dd-MM-yyyy"))
                    val startTime = item.event.startTime.format(DateTimeFormatter.ofPattern("hh:mm a"))
                    val endTime = item.event.endTime.format(DateTimeFormatter.ofPattern("hh:mm a"))
                    Text("$date ($startTime - $endTime)")
                }
            }
        }
    }
}