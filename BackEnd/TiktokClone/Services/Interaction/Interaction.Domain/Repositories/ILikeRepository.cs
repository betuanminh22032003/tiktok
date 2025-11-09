using Interaction.Domain.Entities;

namespace Interaction.Domain.Repositories;

/// <summary>
/// Repository interface for Like aggregate
/// </summary>
public interface ILikeRepository
{
    Task<Like?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Like?> GetByUserAndVideoAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Like>> GetByVideoIdAsync(Guid videoId, CancellationToken cancellationToken = default);
    Task<int> GetCountByVideoIdAsync(Guid videoId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid userId, Guid videoId, CancellationToken cancellationToken = default);
    Task AddAsync(Like like, CancellationToken cancellationToken = default);
    void Remove(Like like);
}
