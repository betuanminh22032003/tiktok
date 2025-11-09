using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Video.Application.Commands.IncrementViewCount;

/// <summary>
/// Command to increment video view count
/// </summary>
public class IncrementViewCountCommand : IRequest<Result<Unit>>
{
    public Guid VideoId { get; set; }

    public IncrementViewCountCommand(Guid videoId)
    {
        VideoId = videoId;
    }
}
