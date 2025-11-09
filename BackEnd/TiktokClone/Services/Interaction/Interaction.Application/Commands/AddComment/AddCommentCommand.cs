using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Interaction.Application.Commands.AddComment;

/// <summary>
/// Command to add a comment to a video
/// </summary>
public class AddCommentCommand : IRequest<Result<Guid>>
{
    public Guid VideoId { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid? ParentCommentId { get; set; }
}
