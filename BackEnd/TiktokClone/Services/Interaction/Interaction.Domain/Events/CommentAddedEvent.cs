using TiktokClone.SharedKernel.Domain;

namespace Interaction.Domain.Events;

public class CommentAddedEvent : DomainEvent
{
    public Guid CommentId { get; }
    public Guid VideoId { get; }
    public Guid UserId { get; }
    public string Content { get; }

    public CommentAddedEvent(Guid commentId, Guid videoId, Guid userId, string content)
    {
        CommentId = commentId;
        VideoId = videoId;
        UserId = userId;
        Content = content;
    }
}
