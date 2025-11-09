using TiktokClone.SharedKernel.Domain;

namespace User.Domain.Entities;

/// <summary>
/// Follow relationship entity
/// </summary>
public class Follow : BaseEntity<Guid>, IAggregateRoot
{
    public Guid FollowerId { get; private set; }
    public string FollowerUsername { get; private set; }
    public Guid FollowingId { get; private set; }
    public string FollowingUsername { get; private set; }

    // For EF Core
    private Follow() : base()
    {
        FollowerUsername = string.Empty;
        FollowingUsername = string.Empty;
    }

    private Follow(
        Guid id,
        Guid followerId,
        string followerUsername,
        Guid followingId,
        string followingUsername) : base(id)
    {
        FollowerId = followerId;
        FollowerUsername = followerUsername;
        FollowingId = followingId;
        FollowingUsername = followingUsername;
    }

    public static Follow Create(
        Guid followerId,
        string followerUsername,
        Guid followingId,
        string followingUsername)
    {
        if (followerId == Guid.Empty)
            throw new ArgumentException("Follower ID cannot be empty", nameof(followerId));

        if (followingId == Guid.Empty)
            throw new ArgumentException("Following ID cannot be empty", nameof(followingId));

        if (followerId == followingId)
            throw new ArgumentException("User cannot follow themselves");

        if (string.IsNullOrWhiteSpace(followerUsername))
            throw new ArgumentException("Follower username cannot be empty", nameof(followerUsername));

        if (string.IsNullOrWhiteSpace(followingUsername))
            throw new ArgumentException("Following username cannot be empty", nameof(followingUsername));

        return new Follow(Guid.NewGuid(), followerId, followerUsername, followingId, followingUsername);
    }
}
