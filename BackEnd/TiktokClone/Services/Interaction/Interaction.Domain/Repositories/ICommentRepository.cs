using Interaction.Domain.Entities;

namespace Interaction.Domain.Repositories;

/// <summary>
/// Repository interface for Comment aggregate
/// </summary>
public interface ICommentRepository
{
    Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Comment>> GetByVideoIdAsync(Guid videoId, CancellationToken cancellationToken = default);
    Task<int> GetCountByVideoIdAsync(Guid videoId, CancellationToken cancellationToken = default);
    Task AddAsync(Comment comment, CancellationToken cancellationToken = default);
    void Update(Comment comment);
    void Remove(Comment comment);
}
