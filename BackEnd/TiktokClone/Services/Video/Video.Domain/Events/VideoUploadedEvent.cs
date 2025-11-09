using TiktokClone.SharedKernel.Domain;

namespace Video.Domain.Events;

public class VideoUploadedEvent : DomainEvent
{
    public Guid VideoId { get; }
    public Guid UserId { get; }
    public string Title { get; }

    public VideoUploadedEvent(Guid videoId, Guid userId, string title)
    {
        VideoId = videoId;
        UserId = userId;
        Title = title;
    }
}
