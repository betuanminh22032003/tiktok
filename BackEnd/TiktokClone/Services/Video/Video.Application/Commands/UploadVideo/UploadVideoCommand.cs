using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Video.Application.Commands.UploadVideo;

/// <summary>
/// Command to upload a new video
/// </summary>
public class UploadVideoCommand : IRequest<Result<Guid>>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public int DurationSeconds { get; set; }
    public long FileSizeBytes { get; set; }
    public string Format { get; set; } = string.Empty;
}
