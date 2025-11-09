using MediatR;
using TiktokClone.SharedKernel.Application;
using Video.Application.DTOs;
using Video.Domain.Repositories;

namespace Video.Application.Queries.GetVideoById;

public class GetVideoByIdQueryHandler : IRequestHandler<GetVideoByIdQuery, Result<VideoDto>>
{
    private readonly IVideoRepository _videoRepository;

    public GetVideoByIdQueryHandler(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    public async Task<Result<VideoDto>> Handle(GetVideoByIdQuery request, CancellationToken cancellationToken)
    {
        var video = await _videoRepository.GetByIdAsync(request.VideoId, cancellationToken);

        if (video == null)
        {
            return Result.Failure<VideoDto>("Video not found");
        }

        var dto = new VideoDto
        {
            Id = video.Id,
            Title = video.Title,
            Description = video.Description,
            VideoUrl = video.VideoUrl.Value,
            ThumbnailUrl = video.ThumbnailUrl,
            UserId = video.UserId,
            Username = video.Username,
            DurationSeconds = video.Duration.TotalSeconds,
            Status = video.Status.ToString(),
            ViewCount = video.ViewCount,
            LikeCount = video.LikeCount,
            CommentCount = video.CommentCount,
            ShareCount = video.ShareCount,
            FileSizeBytes = video.Metadata.FileSizeBytes,
            Format = video.Metadata.Format,
            CreatedAt = video.CreatedAt,
            UpdatedAt = video.UpdatedAt
        };

        return Result.Success(dto);
    }
}
