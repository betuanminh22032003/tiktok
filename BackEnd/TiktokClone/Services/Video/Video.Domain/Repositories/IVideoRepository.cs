using Video.Domain.Entities;

namespace Video.Domain.Repositories;

/// <summary>
/// Repository interface for Video aggregate
/// </summary>
public interface IVideoRepository
{
    Task<Entities.Video?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entities.Video>> GetFeedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entities.Video>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Entities.Video video, CancellationToken cancellationToken = default);
    void Update(Entities.Video video);
    void Remove(Entities.Video video);
}
