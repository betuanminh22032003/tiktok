using Interaction.Application.DTOs;
using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Interaction.Application.Queries.GetVideoLikes;

/// <summary>
/// Query to get likes for a video
/// </summary>
public class GetVideoLikesQuery : IRequest<Result<IReadOnlyList<LikeDto>>>
{
    public Guid VideoId { get; set; }

    public GetVideoLikesQuery(Guid videoId)
    {
        VideoId = videoId;
    }
}
