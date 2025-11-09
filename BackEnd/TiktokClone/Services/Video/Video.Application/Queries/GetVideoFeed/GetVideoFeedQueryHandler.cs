using MediatR;
using TiktokClone.SharedKernel.Application;
using Video.Application.DTOs;
using Video.Domain.Repositories;

namespace Video.Application.Queries.GetVideoFeed;

public class GetVideoFeedQueryHandler : IRequestHandler<GetVideoFeedQuery, Result<PagedResult<VideoDto>>>
{
    private readonly IVideoRepository _videoRepository;

    public GetVideoFeedQueryHandler(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    public async Task<Result<PagedResult<VideoDto>>> Handle(GetVideoFeedQuery request, CancellationToken cancellationToken)
    {
        var videos = await _videoRepository.GetFeedAsync(request.Page, request.PageSize, cancellationToken);
        var totalCount = await _videoRepository.GetTotalCountAsync(cancellationToken);

        var videoDtos = videos.Select(video => new VideoDto
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
        }).ToList();

        var pagedResult = new PagedResult<VideoDto>(
            videoDtos,
            request.Page,
            request.PageSize,
            totalCount
        );

        return Result.Success(pagedResult);
    }
}
