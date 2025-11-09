using TiktokClone.SharedKernel.Domain;

namespace Interaction.Domain.Events;

public class CommentUpdatedEvent : DomainEvent
{
    public Guid CommentId { get; }
    public Guid VideoId { get; }
    public string NewContent { get; }

    public CommentUpdatedEvent(Guid commentId, Guid videoId, string newContent)
    {
        CommentId = commentId;
        VideoId = videoId;
        NewContent = newContent;
    }
}
