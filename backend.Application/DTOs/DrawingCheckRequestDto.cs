namespace backend.Application.DTOs;

/// <summary>Request body for POST /api/arabic-letters/{id}/check-drawing</summary>
public record DrawingCheckRequestDto(string ImageBase64);
