using MediatR;
using TiktokClone.SharedKernel.Application;
using Video.Application.DTOs;

namespace Video.Application.Queries.GetVideoById;

/// <summary>
/// Query to get video by ID
/// </summary>
public class GetVideoByIdQuery : IRequest<Result<VideoDto>>
{
    public Guid VideoId { get; set; }

    public GetVideoByIdQuery(Guid videoId)
    {
        VideoId = videoId;
    }
}
