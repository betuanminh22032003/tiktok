using User.Domain.Entities;
using TiktokClone.SharedKernel.Application;

namespace User.Domain.Repositories;

public interface IUserProfileRepository : IRepository<UserProfile, Guid>
{
    Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserProfile?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UserProfile>> SearchByUsernameAsync(string searchTerm, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
}

public interface IFollowRepository : IRepository<Follow, Guid>
{
    Task<Follow?> GetFollowAsync(Guid followerId, Guid followingId, CancellationToken cancellationToken = default);
    Task<bool> IsFollowingAsync(Guid followerId, Guid followingId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Follow>> GetFollowersAsync(Guid userId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Follow>> GetFollowingAsync(Guid userId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
    Task<int> GetFollowersCountAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<int> GetFollowingCountAsync(Guid userId, CancellationToken cancellationToken = default);
}
