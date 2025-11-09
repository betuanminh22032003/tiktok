using TiktokClone.SharedKernel.Domain;

namespace Video.Domain.Events;

public class VideoReadyEvent : DomainEvent
{
    public Guid VideoId { get; }
    public Guid UserId { get; }

    public VideoReadyEvent(Guid videoId, Guid userId)
    {
        VideoId = videoId;
        UserId = userId;
    }
}
