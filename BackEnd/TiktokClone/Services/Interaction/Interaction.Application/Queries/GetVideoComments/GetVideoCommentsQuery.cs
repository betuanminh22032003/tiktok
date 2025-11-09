using Interaction.Application.DTOs;
using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Interaction.Application.Queries.GetVideoComments;

/// <summary>
/// Query to get comments for a video
/// </summary>
public class GetVideoCommentsQuery : IRequest<Result<IReadOnlyList<CommentDto>>>
{
    public Guid VideoId { get; set; }

    public GetVideoCommentsQuery(Guid videoId)
    {
        VideoId = videoId;
    }
}
