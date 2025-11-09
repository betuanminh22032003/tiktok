using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User.Application.Commands.CreateProfile;
using User.Application.Commands.FollowUser;
using User.Application.Commands.UnfollowUser;
using User.Application.Commands.UpdateAvatar;
using User.Application.Commands.UpdateProfile;
using User.Application.Queries.GetFollowers;
using User.Application.Queries.GetProfile;

namespace User.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("profile")]
    [Authorize]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? Ok(new { profileId = result.Value })
            : BadRequest(new { error = result.Error });
    }

    [HttpGet("profile/{userId:guid}")]
    public async Task<IActionResult> GetProfile(Guid userId)
    {
        var query = new GetUserProfileQuery(userId);
        var result = await _mediator.Send(query);
        
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { error = result.Error });
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new UpdateProfileCommand
        {
            UserId = userId,
            DisplayName = request.DisplayName,
            Bio = request.Bio
        };

        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? Ok(new { message = "Profile updated successfully" })
            : BadRequest(new { error = result.Error });
    }

    [HttpPost("avatar")]
    [Authorize]
    public async Task<IActionResult> UpdateAvatar([FromBody] UpdateAvatarRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new UpdateAvatarCommand
        {
            UserId = userId,
            AvatarUrl = request.AvatarUrl
        };

        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? Ok(new { message = "Avatar updated successfully" })
            : BadRequest(new { error = result.Error });
    }

    [HttpPost("follow/{followingId:guid}")]
    [Authorize]
    public async Task<IActionResult> FollowUser(Guid followingId, [FromBody] FollowUserRequest request)
    {
        var followerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var followerUsername = User.FindFirstValue(ClaimTypes.Name)!;

        var command = new FollowUserCommand
        {
            FollowerId = followerId,
            FollowerUsername = followerUsername,
            FollowingId = followingId,
            FollowingUsername = request.FollowingUsername
        };

        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? Ok(new { message = "Successfully followed user" })
            : BadRequest(new { error = result.Error });
    }

    [HttpDelete("follow/{followingId:guid}")]
    [Authorize]
    public async Task<IActionResult> UnfollowUser(Guid followingId)
    {
        var followerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new UnfollowUserCommand
        {
            FollowerId = followerId,
            FollowingId = followingId
        };

        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? Ok(new { message = "Successfully unfollowed user" })
            : BadRequest(new { error = result.Error });
    }

    [HttpGet("{userId:guid}/followers")]
    public async Task<IActionResult> GetFollowers(Guid userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = new GetFollowersQuery(userId, page, pageSize);
        var result = await _mediator.Send(query);
        
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(new { error = result.Error });
    }

    [HttpGet("{userId:guid}/following")]
    public async Task<IActionResult> GetFollowing(Guid userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = new User.Application.Queries.GetFollowing.GetFollowingQuery(userId, page, pageSize);
        var result = await _mediator.Send(query);
        
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(new { error = result.Error });
    }
}

public record UpdateProfileRequest(string? DisplayName, string? Bio);
public record UpdateAvatarRequest(string AvatarUrl);
public record FollowUserRequest(string FollowingUsername);
