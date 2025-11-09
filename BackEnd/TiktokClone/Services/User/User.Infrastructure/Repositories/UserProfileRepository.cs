using Microsoft.EntityFrameworkCore;
using TiktokClone.SharedKernel.Infrastructure;
using User.Domain.Entities;
using User.Domain.Repositories;
using User.Infrastructure.Persistence;

namespace User.Infrastructure.Repositories;

public class UserProfileRepository : Repository<UserProfile, Guid>, IUserProfileRepository
{
    private readonly UserDbContext _context;

    public UserProfileRepository(UserDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
    }

    public async Task<UserProfile?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.Username == username, cancellationToken);
    }

    public async Task<IReadOnlyList<UserProfile>> SearchByUsernameAsync(
        string searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _context.UserProfiles
            .Where(p => p.Username.Contains(searchTerm) || p.DisplayName.Contains(searchTerm))
            .OrderBy(p => p.Username)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}
