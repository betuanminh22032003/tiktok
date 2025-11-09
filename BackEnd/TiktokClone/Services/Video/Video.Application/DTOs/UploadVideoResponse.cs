namespace Video.Application.DTOs;

public class UploadVideoResponse
{
    public Guid VideoId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
