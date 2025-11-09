using TiktokClone.SharedKernel.Domain;

namespace Video.Domain.Events;

public class VideoDeletedEvent : DomainEvent
{
    public Guid VideoId { get; }
    public Guid UserId { get; }

    public VideoDeletedEvent(Guid videoId, Guid userId)
    {
        VideoId = videoId;
        UserId = userId;
    }
}
