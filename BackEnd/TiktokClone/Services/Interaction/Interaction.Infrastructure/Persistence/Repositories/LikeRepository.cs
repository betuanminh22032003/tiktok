using Interaction.Domain.Entities;
using Interaction.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using TiktokClone.SharedKernel.Infrastructure;

namespace Interaction.Infrastructure.Persistence.Repositories;

public class LikeRepository : Repository<Like, Guid>, ILikeRepository
{
    private readonly InteractionDbContext _context;

    public LikeRepository(InteractionDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Like?> GetByUserAndVideoAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default)
    {
        return await _context.Likes
            .FirstOrDefaultAsync(l => l.UserId == userId && l.VideoId == videoId, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default)
    {
        return await _context.Likes
            .AnyAsync(l => l.UserId == userId && l.VideoId == videoId, cancellationToken);
    }

    public async Task<int> GetCountByVideoIdAsync(Guid videoId, CancellationToken cancellationToken = default)
    {
        return await _context.Likes
            .CountAsync(l => l.VideoId == videoId, cancellationToken);
    }

    public async Task<IReadOnlyList<Like>> GetByVideoIdAsync(Guid videoId, CancellationToken cancellationToken = default)
    {
        return await _context.Likes
            .Where(l => l.VideoId == videoId)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Like>> GetByUserIdAsync(Guid userId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        return await _context.Likes
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}
