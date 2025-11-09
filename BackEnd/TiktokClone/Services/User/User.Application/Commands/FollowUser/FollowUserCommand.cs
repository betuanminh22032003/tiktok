using MediatR;
using TiktokClone.SharedKernel.Application;
using User.Domain.Entities;
using User.Domain.Events;
using User.Domain.Repositories;

namespace User.Application.Commands.FollowUser;

public record FollowUserCommand : IRequest<Result<Unit>>
{
    public Guid FollowerId { get; init; }
    public string FollowerUsername { get; init; } = string.Empty;
    public Guid FollowingId { get; init; }
    public string FollowingUsername { get; init; } = string.Empty;
}

public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, Result<Unit>>
{
    private readonly IFollowRepository _followRepository;
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FollowUserCommandHandler(
        IFollowRepository followRepository,
        IUserProfileRepository userProfileRepository,
        IUnitOfWork unitOfWork)
    {
        _followRepository = followRepository;
        _userProfileRepository = userProfileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(FollowUserCommand request, CancellationToken cancellationToken)
    {
        // Check if already following
        var existing = await _followRepository.IsFollowingAsync(request.FollowerId, request.FollowingId, cancellationToken);
        if (existing)
        {
            return Result.Failure<Unit>("Already following this user");
        }

        try
        {
            var follow = Follow.Create(
                request.FollowerId,
                request.FollowerUsername,
                request.FollowingId,
                request.FollowingUsername);

            await _followRepository.AddAsync(follow, cancellationToken);

            // Update follower's following count
            var followerProfile = await _userProfileRepository.GetByUserIdAsync(request.FollowerId, cancellationToken);
            followerProfile?.IncrementFollowingCount();

            // Update following's followers count
            var followingProfile = await _userProfileRepository.GetByUserIdAsync(request.FollowingId, cancellationToken);
            followingProfile?.IncrementFollowersCount();

            follow.AddDomainEvent(new UserFollowedEvent(follow.Id, request.FollowerId, request.FollowingId));

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(Unit.Value);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<Unit>(ex.Message);
        }
    }
}
