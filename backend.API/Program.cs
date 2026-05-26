using backend.API.Middleware;
using backend.API.WebSockets;
using backend.Application;
using backend.Infrastructure;
using backend.Persistence;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Quran Recitation API",
        Version     = "v1",
        Description = "Phase 1: REST recitation correction — Phase 2: real-time WebSocket streaming"
    });
});

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence(builder.Configuration);

// Singleton: stateless handler, dependencies are singletons or resolved via IServiceScopeFactory
builder.Services.AddSingleton<QuranRecitationWebSocketHandler>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors("Angular");

// Must be registered before any middleware that needs WebSocket support
app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(30)
});

// Phase 2: real-time streaming endpoint
app.Map("/ws/recitation", async context =>
{
    var handler = context.RequestServices
        .GetRequiredService<QuranRecitationWebSocketHandler>();
    await handler.HandleAsync(context);
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Quran Recitation API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
