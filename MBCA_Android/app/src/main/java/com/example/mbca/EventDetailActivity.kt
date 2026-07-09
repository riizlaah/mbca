package com.example.mbca

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.heightIn
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.LazyRow
import androidx.compose.foundation.lazy.grid.GridCells
import androidx.compose.foundation.lazy.grid.LazyVerticalGrid
import androidx.compose.foundation.lazy.grid.items
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material3.Button
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.IconButtonDefaults
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.PrimaryTabRow
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Tab
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.Dialog
import com.example.mbca.ui.theme.MBCATheme
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import java.time.format.DateTimeFormatter
import kotlin.math.max
import kotlin.math.min
import kotlin.time.Duration.Companion.milliseconds

class EventDetailActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            MBCATheme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    LazyColumn(
                        Modifier
                            .fillMaxSize()
                            .padding(innerPadding)
                            .padding(18.dp)
                    ) {
                        item {
                            var event by remember { mutableStateOf<Event?>(null) }
                            var promoCheckErrMsg by remember { mutableStateOf("") }
                            var errMsg by remember { mutableStateOf("") }
                            var loading by remember { mutableStateOf(false) }
                            var promoCodeState by remember { mutableStateOf("") }
                            var quantity by remember { mutableStateOf("1") }
                            var code by remember { mutableStateOf("") }
                            var imgIdx by remember { mutableIntStateOf(0) }
                            var showDialog by remember { mutableStateOf(false) }
                            var selectedExhibit by remember { mutableStateOf<Exhibit?>(null) }
                            val scope = rememberCoroutineScope()
                            val ctx = LocalContext.current

                            LaunchedEffect(Unit) {
                                event = HttpClient.getEventDetail(intent.getIntExtra("id", 0))
                            }

                            LaunchedEffect(code) {
                                promoCheckErrMsg = ""
                                if (code.isEmpty()) {
                                    promoCodeState = "none"
                                    return@LaunchedEffect
                                }
                                delay(500.milliseconds)
                                promoCodeState = "checking"
                                val msg = HttpClient.checkPromoCode(code)
                                if (msg == "ok") {
                                    promoCheckErrMsg = ""
                                    promoCodeState = "valid"
                                } else {
                                    println(msg)
                                    promoCheckErrMsg = msg
                                    promoCodeState = "invalid"
                                }
                            }

                            TextButton({ finish() }) {
                                Icon(painterResource(R.drawable.arr_back), "Back")
                                Spacer(Modifier.width(8.dp))
                                Text("Back", fontWeight = FontWeight.Bold)
                            }
                            if (event == null) return@item

                            if (showDialog && selectedExhibit != null) {
                                Dialog({ showDialog = false }) {
                                    Column(
                                        Modifier
                                            .fillMaxWidth()
                                            .clip(corner())
                                            .background(Color.White)
                                            .padding(12.dp)
                                    ) {
                                        val ex = selectedExhibit!!
                                        NetImg(ex.image, ex.name, Modifier.fillMaxWidth())
                                        Spacer(Modifier.height(12.dp))
                                        LazyRow(Modifier.fillMaxWidth()) {
                                            items(ex.tags) { tag ->
                                                Text(
                                                    tag,
                                                    Modifier
                                                        .border(2.dp, Color.Gray, corner(50))
                                                        .padding(12.dp),
                                                    textAlign = TextAlign.Center
                                                )
                                            }
                                        }
                                        Spacer(Modifier.height(12.dp))
                                        Text(
                                            ex.name,
                                            fontWeight = FontWeight.Bold,
                                            fontSize = typ().titleLarge.fontSize
                                        )
                                        Text("Artist : ${ex.artist}")
                                        Text("Category : ${ex.categoryName}")
                                        Text("Time Period : ${ex.timePeriod}")
                                    }
                                }
                            }
                            val item = event!!
                            Spacer(Modifier.height(24.dp))
                            Text(
                                item.title,
                                fontWeight = FontWeight.Bold,
                                fontSize = typ().displaySmall.fontSize
                            )
                            val date = item.date.format(DateTimeFormatter.ofPattern("dd-MM-yyyy"))
                            val startTime =
                                item.startTime.format(DateTimeFormatter.ofPattern("hh:mm a"))
                            val endTime =
                                item.endTime.format(DateTimeFormatter.ofPattern("hh:mm a"))
                            Text("Date : $date")
                            Text("Time : $startTime - $endTime")
                            Text("Price : $${item.price}/person", fontWeight = FontWeight.Bold)
                            Spacer(Modifier.height(12.dp))
                            Box(
                                Modifier
                                    .fillMaxWidth()
                                    .background(Color.Transparent)
                            ) {
                                NetImg(
                                    item.banners[imgIdx], item.title, Modifier.fillMaxWidth(),
                                    ContentScale.FillWidth
                                )
                                IconButton(
                                    { imgIdx = max(0, imgIdx - 1) },
                                    Modifier
                                        .align(Alignment.CenterStart)
                                        .padding(start = 8.dp),
                                    shape = CircleShape,
                                    colors = IconButtonDefaults.iconButtonColors(containerColor = Color.White)
                                ) {
                                    Icon(painterResource(R.drawable.arr_back), "Previous")
                                }
                                IconButton(
                                    { imgIdx = min(item.banners.size - 1, imgIdx + 1) },
                                    Modifier
                                        .align(Alignment.CenterEnd)
                                        .padding(start = 8.dp),
                                    shape = CircleShape,
                                    colors = IconButtonDefaults.iconButtonColors(containerColor = Color.White)
                                ) {
                                    Icon(painterResource(R.drawable.arr_next), "Next")
                                }
                            }
                            LazyVerticalGrid(
                                GridCells.Fixed(3),
                                Modifier
                                    .fillMaxWidth()
                                    .heightIn(128.dp, 512.dp)
                                    .padding(vertical = 24.dp)
                            ) {
                                items(item.exhibits) { ex ->
                                    NetImg(ex.image, ex.name, Modifier.fillMaxWidth().clickable(onClick = {
                                        selectedExhibit = ex
                                        showDialog = true
                                    }))
                                }
                            }
                            Spacer(Modifier.height(12.dp))
                            Row(Modifier.fillMaxWidth()) {
                                Column(Modifier.weight(1f)) {
                                    Text("Quantity")
                                    OutlinedTextField(
                                        quantity,
                                        { quantity = it },
                                        Modifier.fillMaxWidth()
                                    )
                                }
                                Spacer(Modifier.width(12.dp))
                                Column(Modifier.weight(1f)) {
                                    Text("Promo Code")
                                    OutlinedTextField(code, { code = it }, Modifier.fillMaxWidth())
                                    ErrText(promoCheckErrMsg)
                                }
                            }
                            Spacer(Modifier.height(12.dp))
                            Button({
                                val qty = quantity.toIntOrNull() ?: 0
                                if (qty < 1) {
                                    errMsg = "Quantity must be greater than zero"
                                    return@Button
                                }
                                scope.launch {
                                    loading = true
                                    when (val msg = HttpClient.purchaseTicket(qty, item.id, code)) {
                                        "ok" -> {
                                            finish()
                                        }
                                        else -> errMsg = msg
                                    }
                                    loading = false
                                }
                            }, Modifier.fillMaxWidth(), shape = corner(), enabled = promoCheckErrMsg == "") {
                                LoadingOrContent(loading) {
                                    val qty = quantity.toIntOrNull() ?: 1
                                    Text("Buy $${item.price * qty}")
                                }
                            }
                            Spacer(Modifier.height(24.dp))
                        }
                    }
                }
            }
        }
    }
}
