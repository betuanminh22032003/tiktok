using TiktokClone.SharedKernel.Domain;
using Interaction.Domain.Events;

namespace Interaction.Domain.Entities;

/// <summary>
/// Like aggregate root
/// </summary>
public class Like : BaseEntity<Guid>, IAggregateRoot
{
    public Guid VideoId { get; private set; }
    public Guid UserId { get; private set; }
    public string Username { get; private set; }

    // For EF Core
    private Like() : base() { }

    private Like(Guid id, Guid videoId, Guid userId, string username) : base(id)
    {
        VideoId = videoId;
        UserId = userId;
        Username = username;

        AddDomainEvent(new VideoLikedEvent(videoId, userId));
    }

    public static Like Create(Guid videoId, Guid userId, string username)
    {
        return new Like(Guid.NewGuid(), videoId, userId, username);
    }

    public void Remove()
    {
        AddDomainEvent(new VideoUnlikedEvent(VideoId, UserId));
    }
}
