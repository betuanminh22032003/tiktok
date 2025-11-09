using MediatR;
using TiktokClone.SharedKernel.Application;
using User.Application.DTOs;
using User.Domain.Repositories;

namespace User.Application.Queries.GetFollowing;

public record GetFollowingQuery(Guid UserId, int Page = 1, int PageSize = 20) : IRequest<Result<List<FollowDto>>>;

public class GetFollowingQueryHandler : IRequestHandler<GetFollowingQuery, Result<List<FollowDto>>>
{
    private readonly IFollowRepository _followRepository;
    private readonly IUserProfileRepository _userProfileRepository;

    public GetFollowingQueryHandler(
        IFollowRepository followRepository,
        IUserProfileRepository userProfileRepository)
    {
        _followRepository = followRepository;
        _userProfileRepository = userProfileRepository;
    }

    public async Task<Result<List<FollowDto>>> Handle(GetFollowingQuery request, CancellationToken cancellationToken)
    {
        var follows = await _followRepository.GetFollowingAsync(request.UserId, request.Page, request.PageSize, cancellationToken);

        var dtos = new List<FollowDto>();
        foreach (var follow in follows)
        {
            var followingProfile = await _userProfileRepository.GetByUserIdAsync(follow.FollowingId, cancellationToken);
            dtos.Add(new FollowDto
            {
                Id = follow.Id,
                UserId = follow.FollowingId,
                Username = follow.FollowingUsername,
                AvatarUrl = followingProfile?.Avatar?.Url,
                FollowedAt = follow.CreatedAt
            });
        }

        return Result.Success(dtos);
    }
}
