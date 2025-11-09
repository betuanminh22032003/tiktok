using MediatR;
using TiktokClone.SharedKernel.Application;
using User.Application.DTOs;
using User.Domain.Repositories;

namespace User.Application.Queries.GetFollowers;

public record GetFollowersQuery(Guid UserId, int Page = 1, int PageSize = 20) : IRequest<Result<List<FollowDto>>>;

public class GetFollowersQueryHandler : IRequestHandler<GetFollowersQuery, Result<List<FollowDto>>>
{
    private readonly IFollowRepository _followRepository;
    private readonly IUserProfileRepository _userProfileRepository;

    public GetFollowersQueryHandler(
        IFollowRepository followRepository,
        IUserProfileRepository userProfileRepository)
    {
        _followRepository = followRepository;
        _userProfileRepository = userProfileRepository;
    }

    public async Task<Result<List<FollowDto>>> Handle(GetFollowersQuery request, CancellationToken cancellationToken)
    {
        var follows = await _followRepository.GetFollowersAsync(request.UserId, request.Page, request.PageSize, cancellationToken);

        var dtos = new List<FollowDto>();
        foreach (var follow in follows)
        {
            var followerProfile = await _userProfileRepository.GetByUserIdAsync(follow.FollowerId, cancellationToken);
            dtos.Add(new FollowDto
            {
                Id = follow.Id,
                UserId = follow.FollowerId,
                Username = follow.FollowerUsername,
                AvatarUrl = followerProfile?.Avatar?.Url,
                FollowedAt = follow.CreatedAt
            });
        }

        return Result.Success(dtos);
    }
}
