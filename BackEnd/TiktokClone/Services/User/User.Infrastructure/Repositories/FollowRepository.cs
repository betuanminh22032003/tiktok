using Microsoft.EntityFrameworkCore;
using TiktokClone.SharedKernel.Infrastructure;
using User.Domain.Entities;
using User.Domain.Repositories;
using User.Infrastructure.Persistence;

namespace User.Infrastructure.Repositories;

public class FollowRepository : Repository<Follow, Guid>, IFollowRepository
{
    private readonly UserDbContext _context;

    public FollowRepository(UserDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Follow?> GetFollowAsync(Guid followerId, Guid followingId, CancellationToken cancellationToken = default)
    {
        return await _context.Follows
            .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId, cancellationToken);
    }

    public async Task<bool> IsFollowingAsync(Guid followerId, Guid followingId, CancellationToken cancellationToken = default)
    {
        return await _context.Follows
            .AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId, cancellationToken);
    }

    public async Task<IReadOnlyList<Follow>> GetFollowersAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _context.Follows
            .Where(f => f.FollowingId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Follow>> GetFollowingAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _context.Follows
            .Where(f => f.FollowerId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetFollowersCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Follows
            .CountAsync(f => f.FollowingId == userId, cancellationToken);
    }

    public async Task<int> GetFollowingCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Follows
            .CountAsync(f => f.FollowerId == userId, cancellationToken);
    }
}
