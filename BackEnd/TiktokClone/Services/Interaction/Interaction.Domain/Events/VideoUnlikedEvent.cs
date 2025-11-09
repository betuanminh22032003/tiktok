using TiktokClone.SharedKernel.Domain;

namespace Interaction.Domain.Events;

public class VideoUnlikedEvent : DomainEvent
{
    public Guid VideoId { get; }
    public Guid UserId { get; }

    public VideoUnlikedEvent(Guid videoId, Guid userId)
    {
        VideoId = videoId;
        UserId = userId;
    }
}
