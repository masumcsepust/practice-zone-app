package com.example.quran_app.ui.screens.auth

import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.KeyboardActions
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Email
import androidx.compose.material.icons.filled.Lock
import androidx.compose.material.icons.filled.Person
import androidx.compose.material.icons.filled.Visibility
import androidx.compose.material.icons.filled.VisibilityOff
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.focus.FocusDirection
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalFocusManager
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.ImeAction
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.input.VisualTransformation
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.quran_app.ui.theme.*
import com.example.quran_app.ui.viewmodel.AuthState
import com.example.quran_app.ui.viewmodel.AuthViewModel

@Composable
private fun authFieldColors() = OutlinedTextFieldDefaults.colors(
    focusedBorderColor   = AppPrimary,
    unfocusedBorderColor = Color(0xFFCBD5E1),
    focusedLabelColor    = AppPrimary
)

@Composable
fun RegisterScreen(
    viewModel:         AuthViewModel,
    onRegisterSuccess: () -> Unit,
    onGoLogin:         () -> Unit
) {
    val state = viewModel.state.collectAsState().value
    val focus = LocalFocusManager.current

    var displayName by remember { mutableStateOf("") }
    var email       by remember { mutableStateOf("") }
    var password    by remember { mutableStateOf("") }
    var showPass    by remember { mutableStateOf(false) }

    LaunchedEffect(state) {
        if (state is AuthState.Success) {
            viewModel.resetState()
            onRegisterSuccess()
        }
    }

    Box(
        modifier = Modifier
            .fillMaxSize()
            .background(Brush.verticalGradient(listOf(AppDark, AppPrimary, AppSecondary)))
    ) {
        Column(
            modifier = Modifier
                .fillMaxSize()
                .verticalScroll(rememberScrollState())
                .padding(horizontal = 28.dp, vertical = 48.dp),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Text("القرآن", fontSize = 48.sp, fontWeight = FontWeight.Bold, color = Color.White)
            Text("নতুন অ্যাকাউন্ট তৈরি করুন", fontSize = 15.sp, color = Color.White.copy(.8f))
            Spacer(Modifier.height(32.dp))

            Card(
                shape  = RoundedCornerShape(20.dp),
                colors = CardDefaults.cardColors(containerColor = GlassWhite),
                elevation = CardDefaults.cardElevation(8.dp)
            ) {
                Column(Modifier.padding(24.dp)) {
                    Text("নিবন্ধন করুন", fontSize = 22.sp, fontWeight = FontWeight.ExtraBold, color = NavyText)
                    Text("আপনার তথ্য দিয়ে শুরু করুন", fontSize = 13.sp, color = SlateGrey)
                    Spacer(Modifier.height(20.dp))

                    // Display name
                    OutlinedTextField(
                        value         = displayName,
                        onValueChange = { displayName = it },
                        label         = { Text("নাম") },
                        leadingIcon   = { Icon(Icons.Default.Person, null, tint = AppPrimary) },
                        singleLine    = true,
                        modifier      = Modifier.fillMaxWidth(),
                        keyboardOptions = KeyboardOptions(imeAction = ImeAction.Next),
                        keyboardActions = KeyboardActions(onNext = { focus.moveFocus(FocusDirection.Down) }),
                        colors = authFieldColors()
                    )
                    Spacer(Modifier.height(12.dp))

                    // Email
                    OutlinedTextField(
                        value         = email,
                        onValueChange = { email = it },
                        label         = { Text("ইমেইল") },
                        leadingIcon   = { Icon(Icons.Default.Email, null, tint = AppPrimary) },
                        singleLine    = true,
                        modifier      = Modifier.fillMaxWidth(),
                        keyboardOptions = KeyboardOptions(
                            keyboardType = KeyboardType.Email,
                            imeAction    = ImeAction.Next),
                        keyboardActions = KeyboardActions(onNext = { focus.moveFocus(FocusDirection.Down) }),
                        colors = authFieldColors()
                    )
                    Spacer(Modifier.height(12.dp))

                    // Password
                    OutlinedTextField(
                        value         = password,
                        onValueChange = { password = it },
                        label         = { Text("পাসওয়ার্ড (কমপক্ষে ৬ অক্ষর)") },
                        leadingIcon   = { Icon(Icons.Default.Lock, null, tint = AppPrimary) },
                        trailingIcon  = {
                            IconButton(onClick = { showPass = !showPass }) {
                                Icon(
                                    if (showPass) Icons.Default.VisibilityOff else Icons.Default.Visibility,
                                    null, tint = SlateGrey)
                            }
                        },
                        visualTransformation = if (showPass) VisualTransformation.None else PasswordVisualTransformation(),
                        singleLine    = true,
                        modifier      = Modifier.fillMaxWidth(),
                        keyboardOptions = KeyboardOptions(
                            keyboardType = KeyboardType.Password,
                            imeAction    = ImeAction.Done),
                        keyboardActions = KeyboardActions(onDone = {
                            focus.clearFocus()
                            viewModel.register(email, password, displayName)
                        }),
                        colors = authFieldColors()
                    )
                    Spacer(Modifier.height(8.dp))

                    // Error
                    if (state is AuthState.Error) {
                        Text(
                            state.message,
                            color     = MaterialTheme.colorScheme.error,
                            fontSize  = 13.sp,
                            modifier  = Modifier.fillMaxWidth(),
                            textAlign = TextAlign.Center
                        )
                        Spacer(Modifier.height(6.dp))
                    }

                    // Register button
                    Button(
                        onClick  = { focus.clearFocus(); viewModel.register(email, password, displayName) },
                        enabled  = state !is AuthState.Loading,
                        modifier = Modifier.fillMaxWidth().height(50.dp),
                        shape    = RoundedCornerShape(12.dp),
                        colors   = ButtonDefaults.buttonColors(containerColor = AppPrimary)
                    ) {
                        if (state is AuthState.Loading)
                            CircularProgressIndicator(Modifier.size(22.dp), color = Color.White, strokeWidth = 2.dp)
                        else
                            Text("নিবন্ধন করুন", fontSize = 16.sp, fontWeight = FontWeight.Bold)
                    }
                }
            }

            Spacer(Modifier.height(20.dp))

            Row(verticalAlignment = Alignment.CenterVertically) {
                Text("ইতিমধ্যে অ্যাকাউন্ট আছে? ", color = Color.White.copy(.85f), fontSize = 14.sp)
                Text(
                    "লগইন করুন",
                    color      = AppAccent,
                    fontSize   = 14.sp,
                    fontWeight = FontWeight.Bold,
                    modifier   = Modifier.clickable { viewModel.resetState(); onGoLogin() }
                )
            }
        }
    }
}
