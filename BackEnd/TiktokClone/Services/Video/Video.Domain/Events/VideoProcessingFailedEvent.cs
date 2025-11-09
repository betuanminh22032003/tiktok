using TiktokClone.SharedKernel.Domain;

namespace Video.Domain.Events;

public class VideoProcessingFailedEvent : DomainEvent
{
    public Guid VideoId { get; }
    public Guid UserId { get; }
    public string Reason { get; }

    public VideoProcessingFailedEvent(Guid videoId, Guid userId, string reason)
    {
        VideoId = videoId;
        UserId = userId;
        Reason = reason;
    }
}
