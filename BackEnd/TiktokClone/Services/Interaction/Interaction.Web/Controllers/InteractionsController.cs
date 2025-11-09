using Interaction.Application.Commands.LikeVideo;
using Interaction.Application.Commands.UnlikeVideo;
using Interaction.Application.Commands.AddComment;
using Interaction.Application.Commands.UpdateComment;
using Interaction.Application.Commands.DeleteComment;
using Interaction.Application.Queries.GetVideoLikes;
using Interaction.Application.Queries.GetVideoComments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Interaction.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InteractionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<InteractionsController> _logger;

    public InteractionsController(IMediator mediator, ILogger<InteractionsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Like a video
    /// </summary>
    [HttpPost("likes")]
    [Authorize]
    public async Task<IActionResult> LikeVideo([FromBody] LikeVideoRequest request)
    {
        var userId = GetUserId();
        var username = GetUsername();

        var command = new LikeVideoCommand
        {
            VideoId = request.VideoId,
            UserId = userId,
            Username = username
        };

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(new { message = "Video liked successfully", likeId = result.Value });
        }

        return BadRequest(new { error = result.Error });
    }

    /// <summary>
    /// Unlike a video
    /// </summary>
    [HttpDelete("likes/{videoId:guid}")]
    [Authorize]
    public async Task<IActionResult> UnlikeVideo(Guid videoId)
    {
        var userId = GetUserId();

        var command = new UnlikeVideoCommand
        {
            VideoId = videoId,
            UserId = userId
        };

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(new { message = "Video unliked successfully" });
        }

        return BadRequest(new { error = result.Error });
    }

    /// <summary>
    /// Get all likes for a video
    /// </summary>
    [HttpGet("videos/{videoId:guid}/likes")]
    public async Task<IActionResult> GetVideoLikes(Guid videoId)
    {
        var query = new GetVideoLikesQuery(videoId);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(new { error = result.Error });
    }

    /// <summary>
    /// Add a comment to a video
    /// </summary>
    [HttpPost("comments")]
    [Authorize]
    public async Task<IActionResult> AddComment([FromBody] AddCommentRequest request)
    {
        var userId = GetUserId();
        var username = GetUsername();

        var command = new AddCommentCommand
        {
            VideoId = request.VideoId,
            UserId = userId,
            Username = username,
            Content = request.Content,
            ParentCommentId = request.ParentCommentId
        };

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(new { message = "Comment added successfully", commentId = result.Value });
        }

        return BadRequest(new { error = result.Error });
    }

    /// <summary>
    /// Update a comment
    /// </summary>
    [HttpPut("comments/{commentId:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateComment(Guid commentId, [FromBody] UpdateCommentRequest request)
    {
        var userId = GetUserId();

        var command = new UpdateCommentCommand
        {
            CommentId = commentId,
            UserId = userId,
            Content = request.Content
        };

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(new { message = "Comment updated successfully" });
        }

        return BadRequest(new { error = result.Error });
    }

    /// <summary>
    /// Delete a comment (soft delete)
    /// </summary>
    [HttpDelete("comments/{commentId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        var userId = GetUserId();

        var command = new DeleteCommentCommand
        {
            CommentId = commentId,
            UserId = userId
        };

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(new { message = "Comment deleted successfully" });
        }

        return BadRequest(new { error = result.Error });
    }

    /// <summary>
    /// Get all comments for a video
    /// </summary>
    [HttpGet("videos/{videoId:guid}/comments")]
    public async Task<IActionResult> GetVideoComments(Guid videoId)
    {
        var query = new GetVideoCommentsQuery(videoId);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(new { error = result.Error });
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }
        return userId;
    }

    private string GetUsername()
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        if (string.IsNullOrEmpty(username))
        {
            throw new UnauthorizedAccessException("Username not found in token");
        }
        return username;
    }
}

// Request models
public record LikeVideoRequest(Guid VideoId);
public record AddCommentRequest(Guid VideoId, string Content, Guid? ParentCommentId);
public record UpdateCommentRequest(string Content);
