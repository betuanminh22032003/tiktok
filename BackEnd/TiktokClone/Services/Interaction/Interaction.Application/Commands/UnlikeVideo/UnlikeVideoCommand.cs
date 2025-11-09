using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Interaction.Application.Commands.UnlikeVideo;

/// <summary>
/// Command to unlike a video
/// </summary>
public class UnlikeVideoCommand : IRequest<Result<Unit>>
{
    public Guid VideoId { get; set; }
    public Guid UserId { get; set; }
}
