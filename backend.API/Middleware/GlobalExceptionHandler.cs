using Microsoft.AspNetCore.Diagnostics;

namespace backend.API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken ct)
    {
        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        var (statusCode, error) = exception switch
        {
            KeyNotFoundException       => (StatusCodes.Status404NotFound,            exception.Message),
            InvalidOperationException  => (StatusCodes.Status422UnprocessableEntity, exception.Message),
            ArgumentException          => (StatusCodes.Status400BadRequest,           exception.Message),
            _                          => (StatusCodes.Status500InternalServerError,  "An unexpected error occurred.")
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(new { error }, ct);

        return true;
    }
}
