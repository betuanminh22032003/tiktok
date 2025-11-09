using TiktokClone.SharedKernel.Domain;

namespace User.Domain.Events;

public class UserProfileCreatedEvent : DomainEvent
{
    public Guid ProfileId { get; }
    public Guid UserId { get; }
    public string Username { get; }

    public UserProfileCreatedEvent(Guid profileId, Guid userId, string username)
    {
        ProfileId = profileId;
        UserId = userId;
        Username = username;
    }
}

public class ProfileUpdatedEvent : DomainEvent
{
    public Guid ProfileId { get; }
    public Guid UserId { get; }
    public string? DisplayName { get; }
    public string? Bio { get; }

    public ProfileUpdatedEvent(Guid profileId, Guid userId, string? displayName, string? bio)
    {
        ProfileId = profileId;
        UserId = userId;
        DisplayName = displayName;
        Bio = bio;
    }
}

public class AvatarChangedEvent : DomainEvent
{
    public Guid ProfileId { get; }
    public Guid UserId { get; }
    public string? OldAvatarUrl { get; }
    public string? NewAvatarUrl { get; }

    public AvatarChangedEvent(Guid profileId, Guid userId, string? oldAvatarUrl, string? newAvatarUrl)
    {
        ProfileId = profileId;
        UserId = userId;
        OldAvatarUrl = oldAvatarUrl;
        NewAvatarUrl = newAvatarUrl;
    }
}

public class UserFollowedEvent : DomainEvent
{
    public Guid FollowId { get; }
    public Guid FollowerId { get; }
    public Guid FollowingId { get; }

    public UserFollowedEvent(Guid followId, Guid followerId, Guid followingId)
    {
        FollowId = followId;
        FollowerId = followerId;
        FollowingId = followingId;
    }
}

public class UserUnfollowedEvent : DomainEvent
{
    public Guid FollowerId { get; }
    public Guid FollowingId { get; }

    public UserUnfollowedEvent(Guid followerId, Guid followingId)
    {
        FollowerId = followerId;
        FollowingId = followingId;
    }
}
