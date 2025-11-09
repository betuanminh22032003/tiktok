using MediatR;
using TiktokClone.SharedKernel.Application;
using User.Application.DTOs;
using User.Domain.Repositories;

namespace User.Application.Queries.GetProfile;

public record GetUserProfileQuery(Guid UserId) : IRequest<Result<UserProfileDto>>;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Result<UserProfileDto>>
{
    private readonly IUserProfileRepository _userProfileRepository;

    public GetUserProfileQueryHandler(IUserProfileRepository userProfileRepository)
    {
        _userProfileRepository = userProfileRepository;
    }

    public async Task<Result<UserProfileDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await _userProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        
        if (profile == null)
        {
            return Result.Failure<UserProfileDto>($"Profile not found for user ID: {request.UserId}");
        }

        var dto = new UserProfileDto
        {
            Id = profile.Id,
            UserId = profile.UserId,
            Username = profile.Username,
            DisplayName = profile.DisplayName,
            Bio = profile.Bio,
            AvatarUrl = profile.Avatar?.Url,
            FollowersCount = profile.FollowersCount,
            FollowingCount = profile.FollowingCount,
            VideoCount = profile.VideoCount,
            TotalLikes = profile.TotalLikes,
            CreatedAt = profile.CreatedAt
        };

        return Result.Success(dto);
    }
}
