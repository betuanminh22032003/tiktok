using Microsoft.EntityFrameworkCore;
using Video.Domain.Repositories;

namespace Video.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Video aggregate
/// </summary>
public class VideoRepository : IVideoRepository
{
    private readonly VideoDbContext _context;

    public VideoRepository(VideoDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Video?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Videos
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Domain.Entities.Video>> GetFeedAsync(
        int page, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Videos
            .Where(v => v.Status == Domain.ValueObjects.VideoStatus.Ready)
            .OrderByDescending(v => v.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Domain.Entities.Video>> GetByUserIdAsync(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Videos
            .Where(v => v.UserId == userId)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Videos
            .Where(v => v.Status == Domain.ValueObjects.VideoStatus.Ready)
            .CountAsync(cancellationToken);
    }

    public async Task AddAsync(Domain.Entities.Video video, CancellationToken cancellationToken = default)
    {
        await _context.Videos.AddAsync(video, cancellationToken);
    }

    public void Update(Domain.Entities.Video video)
    {
        _context.Videos.Update(video);
    }

    public void Remove(Domain.Entities.Video video)
    {
        _context.Videos.Remove(video);
    }
}
