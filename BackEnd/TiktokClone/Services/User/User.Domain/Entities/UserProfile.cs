using TiktokClone.SharedKernel.Domain;
using User.Domain.Events;
using User.Domain.ValueObjects;

namespace User.Domain.Entities;

/// <summary>
/// UserProfile aggregate root - manages user profile information
/// </summary>
public class UserProfile : BaseEntity<Guid>, IAggregateRoot
{
    public Guid UserId { get; private set; } // Reference to Identity.User
    public string Username { get; private set; }
    public string DisplayName { get; private set; }
    public string? Bio { get; private set; }
    public AvatarUrl? Avatar { get; private set; }
    public int FollowersCount { get; private set; }
    public int FollowingCount { get; private set; }
    public int VideoCount { get; private set; }
    public int TotalLikes { get; private set; }

    // For EF Core
    private UserProfile() : base() 
    {
        Username = string.Empty;
        DisplayName = string.Empty;
    }

    private UserProfile(
        Guid id,
        Guid userId,
        string username,
        string displayName) : base(id)
    {
        UserId = userId;
        Username = username;
        DisplayName = displayName;
        FollowersCount = 0;
        FollowingCount = 0;
        VideoCount = 0;
        TotalLikes = 0;

        AddDomainEvent(new UserProfileCreatedEvent(id, userId, username));
    }

    public static UserProfile Create(Guid userId, string username, string? displayName = null)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty", nameof(userId));

        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty", nameof(username));

        return new UserProfile(Guid.NewGuid(), userId, username, displayName ?? username);
    }

    public void UpdateProfile(string? displayName, string? bio)
    {
        if (!string.IsNullOrWhiteSpace(displayName))
        {
            DisplayName = displayName;
        }

        Bio = bio;
        AddDomainEvent(new ProfileUpdatedEvent(Id, UserId, displayName, bio));
    }

    public void UpdateAvatar(string avatarUrl)
    {
        var oldAvatarUrl = Avatar?.Url;
        Avatar = AvatarUrl.Create(avatarUrl);
        AddDomainEvent(new AvatarChangedEvent(Id, UserId, oldAvatarUrl, avatarUrl));
    }

    public void RemoveAvatar()
    {
        var oldAvatarUrl = Avatar?.Url;
        Avatar = null;
        AddDomainEvent(new AvatarChangedEvent(Id, UserId, oldAvatarUrl, null));
    }

    public void IncrementFollowersCount()
    {
        FollowersCount++;
    }

    public void DecrementFollowersCount()
    {
        if (FollowersCount > 0)
            FollowersCount--;
    }

    public void IncrementFollowingCount()
    {
        FollowingCount++;
    }

    public void DecrementFollowingCount()
    {
        if (FollowingCount > 0)
            FollowingCount--;
    }

    public void IncrementVideoCount()
    {
        VideoCount++;
    }

    public void IncrementTotalLikes(int count = 1)
    {
        TotalLikes += count;
    }
}
