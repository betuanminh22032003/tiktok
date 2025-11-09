using MediatR;
using TiktokClone.SharedKernel.Application;
using User.Domain.Events;
using User.Domain.Repositories;

namespace User.Application.Commands.UnfollowUser;

public record UnfollowUserCommand : IRequest<Result<Unit>>
{
    public Guid FollowerId { get; init; }
    public Guid FollowingId { get; init; }
}

public class UnfollowUserCommandHandler : IRequestHandler<UnfollowUserCommand, Result<Unit>>
{
    private readonly IFollowRepository _followRepository;
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UnfollowUserCommandHandler(
        IFollowRepository followRepository,
        IUserProfileRepository userProfileRepository,
        IUnitOfWork unitOfWork)
    {
        _followRepository = followRepository;
        _userProfileRepository = userProfileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
    {
        var follow = await _followRepository.GetFollowAsync(request.FollowerId, request.FollowingId, cancellationToken);
        
        if (follow == null)
        {
            return Result.Failure<Unit>("Not following this user");
        }

        follow.AddDomainEvent(new UserUnfollowedEvent(request.FollowerId, request.FollowingId));

        _followRepository.Remove(follow);

        // Update follower's following count
        var followerProfile = await _userProfileRepository.GetByUserIdAsync(request.FollowerId, cancellationToken);
        followerProfile?.DecrementFollowingCount();

        // Update following's followers count
        var followingProfile = await _userProfileRepository.GetByUserIdAsync(request.FollowingId, cancellationToken);
        followingProfile?.DecrementFollowersCount();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}
