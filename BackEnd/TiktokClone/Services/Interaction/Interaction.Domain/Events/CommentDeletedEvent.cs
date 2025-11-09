using TiktokClone.SharedKernel.Domain;

namespace Interaction.Domain.Events;

public class CommentDeletedEvent : DomainEvent
{
    public Guid CommentId { get; }
    public Guid VideoId { get; }
    public Guid UserId { get; }

    public CommentDeletedEvent(Guid commentId, Guid videoId, Guid userId)
    {
        CommentId = commentId;
        VideoId = videoId;
        UserId = userId;
    }
}
