using Interaction.Domain.Entities;
using Interaction.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using TiktokClone.SharedKernel.Infrastructure;

namespace Interaction.Infrastructure.Persistence.Repositories;

public class CommentRepository : Repository<Comment, Guid>, ICommentRepository
{
    private readonly InteractionDbContext _context;

    public CommentRepository(InteractionDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Comment>> GetByVideoIdAsync(Guid videoId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Where(c => c.VideoId == videoId && !c.IsDeleted)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountByVideoIdAsync(Guid videoId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .CountAsync(c => c.VideoId == videoId && !c.IsDeleted, cancellationToken);
    }

    public async Task<List<Comment>> GetRepliesAsync(Guid parentCommentId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Where(c => c.ParentCommentId == parentCommentId && !c.IsDeleted)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Comment>> GetByUserIdAsync(Guid userId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Where(c => c.UserId == userId && !c.IsDeleted)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasRepliesAsync(Guid commentId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .AnyAsync(c => c.ParentCommentId == commentId && !c.IsDeleted, cancellationToken);
    }
}
