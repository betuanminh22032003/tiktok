using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Interaction.Application.Commands.LikeVideo;

/// <summary>
/// Command to like a video
/// </summary>
public class LikeVideoCommand : IRequest<Result<Guid>>
{
    public Guid VideoId { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
}
