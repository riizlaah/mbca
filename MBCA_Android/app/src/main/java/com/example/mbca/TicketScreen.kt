package com.example.mbca

import android.content.ContentValues
import android.graphics.Bitmap
import android.media.MediaScannerConnection
import android.os.Build
import android.os.Environment
import android.provider.MediaStore
import android.util.Log
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
import androidx.compose.material3.Button
import androidx.compose.material3.DatePickerDialog
import androidx.compose.material3.DateRangePicker
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.IconButtonDefaults
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.material3.getSelectedEndDate
import androidx.compose.material3.getSelectedStartDate
import androidx.compose.material3.rememberDateRangePickerState
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
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import kotlinx.coroutines.launch
import java.io.File
import java.io.FileOutputStream
import java.time.LocalDate
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter
import kotlin.math.min

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun TicketScreen(modifier: Modifier) {
    val tickets = remember { mutableStateListOf<Ticket>() }
    var openDialog by remember { mutableStateOf(false) }
    val dateRangePicker = rememberDateRangePickerState()
    val ctx = LocalContext.current
    val scope = rememberCoroutineScope()
    var selectedRange by remember { mutableStateOf<Pair<LocalDate, LocalDate>?>(null) }

    LaunchedEffect(Unit) {
        val arr = HttpClient.getTickets()
        tickets.addAll(arr)
    }

    LaunchedEffect(selectedRange) {
        if(selectedRange != null) {
            println("Yeah")
            val range = selectedRange!!
            val filter = HttpClient.getTickets().filter { range.first < it.event.date && it.event.date < range.second }
            println(filter)
            tickets.clear()
            tickets.addAll(filter)
        }
    }

    if (openDialog) {
        DatePickerDialog(
            { openDialog = false },
            { TextButton({
                val start = dateRangePicker.getSelectedStartDate()
                val end = dateRangePicker.getSelectedEndDate()
                if(start == null || end == null) return@TextButton
                selectedRange = Pair(start, end)
                println(selectedRange)
                openDialog = false
            }) { Text("OK") } },
            dismissButton = { TextButton({
                openDialog = false
            }) { Text("Cancel") } }) {
            DateRangePicker(
                dateRangePicker,
                title = { Text("Select date range") },
                showModeToggle = false,
                modifier = Modifier.fillMaxWidth()
            )
        }
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
            Spacer(Modifier.height(12.dp))
            OutlinedButton({
                openDialog = true
            }, shape = corner()) {
                val fmt = DateTimeFormatter.ofPattern("yyyy-MM-dd")
                val text = if(selectedRange != null) "${selectedRange!!.first.format(fmt)} - ${selectedRange!!.second.format(fmt)}" else "Filter Date"
                Text(text)
            }
            Spacer(Modifier.height(24.dp))
            if(tickets.isEmpty()) {
                Text("No Ticket found", Modifier.fillMaxWidth(), textAlign = TextAlign.Center, color = Color.Gray)
            }
        }
        items(tickets) { item ->
            val graphicsLayer = rememberGraphicsLayer()
            var downloading by remember { mutableStateOf(false) }
            Box(
                Modifier
                    .padding(vertical = 12.dp)
                    .fillMaxWidth()
                    .drawWithContent {
                        graphicsLayer.record { this@drawWithContent.drawContent() }
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
                            item.event.banners[0],
                            item.event.title,
                            Modifier.fillMaxWidth(),
                            ContentScale.FillWidth
                        )
                        if (!downloading) {
                            IconButton(
                                {
                                    scope.launch {
                                        downloading = true
                                        val bitmap = graphicsLayer.toImageBitmap().asAndroidBitmap()
                                        val filename = "Card_${System.currentTimeMillis()}.png"
                                        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.Q) {
                                            // for android 10 or greater
                                            val contentVal = ContentValues().apply {
                                                put(MediaStore.MediaColumns.DISPLAY_NAME, filename)
                                                put(MediaStore.MediaColumns.MIME_TYPE, "image/png")
                                                put(
                                                    MediaStore.MediaColumns.RELATIVE_PATH,
                                                    Environment.DIRECTORY_PICTURES + "/Tickets"
                                                )
                                            }
                                            val imgUri = ctx.contentResolver.insert(
                                                MediaStore.Images.Media.EXTERNAL_CONTENT_URI,
                                                contentVal
                                            )
                                            imgUri?.let { uri ->
                                                ctx.contentResolver.openOutputStream(uri)?.use {
                                                    bitmap.compress(
                                                        Bitmap.CompressFormat.PNG, 100, it
                                                    )
                                                }
                                                Toast.makeText(
                                                    ctx,
                                                    "Ticket downloaded successfully",
                                                    Toast.LENGTH_SHORT
                                                ).show()
                                            }

                                        } else {
                                            // for android 9 or lower
                                            val imgDir = File(
                                                Environment.getExternalStoragePublicDirectory(
                                                    Environment.DIRECTORY_PICTURES
                                                ), "Tickets"
                                            )
                                            if (!imgDir.exists()) {
                                                imgDir.mkdirs()
                                            }
                                            val imgFile = File(imgDir, filename)
                                            try {
                                                FileOutputStream(imgFile).use {
                                                    bitmap.compress(
                                                        Bitmap.CompressFormat.PNG, 100, it
                                                    )
                                                }
                                                MediaScannerConnection.scanFile(
                                                    ctx,
                                                    arrayOf(imgFile.absolutePath),
                                                    arrayOf("image/png")
                                                ) { path, uri ->
                                                    Log.d(
                                                        "MediaScanner",
                                                        "File $path ready to show in the galery"
                                                    )
                                                }
                                            } catch (e: Exception) {
                                                e.printStackTrace()
                                                Toast.makeText(
                                                    ctx,
                                                    "Failed to download image",
                                                    Toast.LENGTH_SHORT
                                                ).show()
                                            }
                                        }
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
                    val startTime =
                        item.event.startTime.format(DateTimeFormatter.ofPattern("hh:mm a"))
                    val endTime = item.event.endTime.format(DateTimeFormatter.ofPattern("hh:mm a"))
                    Text("$date ($startTime - $endTime)")
                }
            }
        }
    }
}