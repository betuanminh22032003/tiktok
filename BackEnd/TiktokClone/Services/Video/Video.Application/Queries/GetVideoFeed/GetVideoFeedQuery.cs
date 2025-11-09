using MediatR;
using TiktokClone.SharedKernel.Application;
using Video.Application.DTOs;

namespace Video.Application.Queries.GetVideoFeed;

/// <summary>
/// Query to get paginated video feed
/// </summary>
public class GetVideoFeedQuery : IRequest<Result<PagedResult<VideoDto>>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public GetVideoFeedQuery(int page, int pageSize)
    {
        Page = page > 0 ? page : 1;
        PageSize = pageSize > 0 && pageSize <= 50 ? pageSize : 10;
    }
}
