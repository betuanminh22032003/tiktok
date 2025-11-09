using TiktokClone.SharedKernel.Domain;
using Video.Domain.Events;
using Video.Domain.ValueObjects;

namespace Video.Domain.Entities;

/// <summary>
/// Video aggregate root - represents a uploaded video
/// </summary>
public class Video : BaseEntity<Guid>, IAggregateRoot
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public VideoUrl VideoUrl { get; private set; }
    public string? ThumbnailUrl { get; private set; }
    public Guid UserId { get; private set; } // Owner
    public string Username { get; private set; }
    public VideoDuration Duration { get; private set; }
    public VideoStatus Status { get; private set; }
    
    // Statistics
    public long ViewCount { get; private set; }
    public long LikeCount { get; private set; }
    public long CommentCount { get; private set; }
    public long ShareCount { get; private set; }
    
    // Metadata
    public VideoMetadata Metadata { get; private set; }

    // For EF Core
    private Video() : base() { }

    private Video(
        Guid id,
        string title,
        string description,
        VideoUrl videoUrl,
        string? thumbnailUrl,
        Guid userId,
        string username,
        VideoDuration duration,
        VideoMetadata metadata) : base(id)
    {
        Title = title;
        Description = description;
        VideoUrl = videoUrl;
        ThumbnailUrl = thumbnailUrl;
        UserId = userId;
        Username = username;
        Duration = duration;
        Status = VideoStatus.Processing;
        Metadata = metadata;
        
        ViewCount = 0;
        LikeCount = 0;
        CommentCount = 0;
        ShareCount = 0;

        AddDomainEvent(new VideoUploadedEvent(Id, UserId, title));
    }

    public static Video Create(
        string title,
        string description,
        string videoUrl,
        string? thumbnailUrl,
        Guid userId,
        string username,
        int durationSeconds,
        long fileSizeBytes,
        string format)
    {
        var videoUrlVO = VideoUrl.Create(videoUrl);
        var duration = VideoDuration.FromSeconds(durationSeconds);
        var metadata = VideoMetadata.Create(fileSizeBytes, format);

        return new Video(
            Guid.NewGuid(),
            title,
            description,
            videoUrlVO,
            thumbnailUrl,
            userId,
            username,
            duration,
            metadata
        );
    }

    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Title cannot be empty");

        Title = newTitle;
        SetUpdatedAt();
    }

    public void UpdateDescription(string newDescription)
    {
        Description = newDescription ?? string.Empty;
        SetUpdatedAt();
    }

    public void MarkAsReady()
    {
        if (Status != VideoStatus.Processing)
            throw new InvalidOperationException("Video is not in processing state");

        Status = VideoStatus.Ready;
        SetUpdatedAt();
        AddDomainEvent(new VideoReadyEvent(Id, UserId));
    }

    public void MarkAsFailed(string reason)
    {
        Status = VideoStatus.Failed;
        SetUpdatedAt();
        AddDomainEvent(new VideoProcessingFailedEvent(Id, UserId, reason));
    }

    public void IncrementViewCount()
    {
        ViewCount++;
        SetUpdatedAt();
    }

    public void IncrementLikeCount()
    {
        LikeCount++;
        SetUpdatedAt();
    }

    public void DecrementLikeCount()
    {
        if (LikeCount > 0)
        {
            LikeCount--;
            SetUpdatedAt();
        }
    }

    public void IncrementCommentCount()
    {
        CommentCount++;
        SetUpdatedAt();
    }

    public void DecrementCommentCount()
    {
        if (CommentCount > 0)
        {
            CommentCount--;
            SetUpdatedAt();
        }
    }

    public void IncrementShareCount()
    {
        ShareCount++;
        SetUpdatedAt();
    }

    public void Delete()
    {
        AddDomainEvent(new VideoDeletedEvent(Id, UserId));
    }
}
