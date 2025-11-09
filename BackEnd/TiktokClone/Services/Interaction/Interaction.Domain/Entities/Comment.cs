using TiktokClone.SharedKernel.Domain;
using Interaction.Domain.Events;

namespace Interaction.Domain.Entities;

/// <summary>
/// Comment aggregate root
/// </summary>
public class Comment : BaseEntity<Guid>, IAggregateRoot
{
    public Guid VideoId { get; private set; }
    public Guid UserId { get; private set; }
    public string Username { get; private set; }
    public string Content { get; private set; }
    public Guid? ParentCommentId { get; private set; } // For replies
    public bool IsDeleted { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // For EF Core
    private Comment() : base() { }

    private Comment(
        Guid id,
        Guid videoId,
        Guid userId,
        string username,
        string content,
        Guid? parentCommentId) : base(id)
    {
        VideoId = videoId;
        UserId = userId;
        Username = username;
        Content = content;
        ParentCommentId = parentCommentId;

        AddDomainEvent(new CommentAddedEvent(id, videoId, userId, content));
    }

    public static Comment Create(
        Guid videoId,
        Guid userId,
        string username,
        string content,
        Guid? parentCommentId = null)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Comment content cannot be empty", nameof(content));

        if (content.Length > 500)
            throw new ArgumentException("Comment content cannot exceed 500 characters", nameof(content));

        return new Comment(Guid.NewGuid(), videoId, userId, username, content, parentCommentId);
    }

    public void UpdateContent(string newContent)
    {
        if (string.IsNullOrWhiteSpace(newContent))
            throw new ArgumentException("Comment content cannot be empty", nameof(newContent));

        if (newContent.Length > 500)
            throw new ArgumentException("Comment content cannot exceed 500 characters", nameof(newContent));

        Content = newContent;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new CommentUpdatedEvent(Id, VideoId, newContent));
    }

    public void Delete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new CommentDeletedEvent(Id, VideoId, UserId));
    }
}
