namespace backend.Application.DTOs;

public class AudioChunkDto
{
    public string ConnectionId { get; init; } = string.Empty;
    public byte[] Data { get; init; } = [];
    public int Length { get; init; }
}
