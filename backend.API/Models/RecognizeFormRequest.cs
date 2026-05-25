namespace backend.API.Models;

public class RecognizeFormRequest
{
    public int AyahId { get; set; }
    public int UserId { get; set; } = 1;
    public IFormFile AudioFile { get; set; } = null!;
}
