namespace User.Application.DTOs;

public record UserProfileDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string? Bio { get; init; }
    public string? AvatarUrl { get; init; }
    public int FollowersCount { get; init; }
    public int FollowingCount { get; init; }
    public int VideoCount { get; init; }
    public int TotalLikes { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record FollowDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string? AvatarUrl { get; init; }
    public DateTime FollowedAt { get; init; }
}
