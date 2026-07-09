package com.example.mbca

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.PrimaryTabRow
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Tab
import androidx.compose.material3.Text
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.rememberGraphicsLayer
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.unit.dp
import com.example.mbca.ui.theme.MBCATheme

class HomeActivity : ComponentActivity() {
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
                    ) {
                        var selectedIdx by remember { mutableIntStateOf(0) }
                        val tabs = listOf("Event", "Tickets")
                        val ctx = LocalContext.current

                        if(selectedIdx == 0) {
                            EventScreen(Modifier.weight(1f))
                        } else {
                            TicketScreen(Modifier.weight(1f))
                        }
                        PrimaryTabRow(selectedIdx, Modifier.fillMaxWidth()) {
                            tabs.forEachIndexed { idx, tab ->
                                Tab(selectedIdx == idx, {selectedIdx = idx}, Modifier.padding(12.dp)) {
                                    Text(tab)
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
