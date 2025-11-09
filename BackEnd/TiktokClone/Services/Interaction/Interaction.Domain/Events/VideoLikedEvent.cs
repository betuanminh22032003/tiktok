using TiktokClone.SharedKernel.Domain;

namespace Interaction.Domain.Events;

public class VideoLikedEvent : DomainEvent
{
    public Guid VideoId { get; }
    public Guid UserId { get; }

    public VideoLikedEvent(Guid videoId, Guid userId)
    {
        VideoId = videoId;
        UserId = userId;
    }
}
