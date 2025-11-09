using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Video.Application.Commands.IncrementViewCount;
using Video.Application.Commands.UploadVideo;
using Video.Application.DTOs;
using Video.Application.Queries.GetVideoById;
using Video.Application.Queries.GetVideoFeed;

namespace Video.Web.Controllers;

/// <summary>
/// Video controller for managing video operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VideosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<VideosController> _logger;

    public VideosController(IMediator mediator, ILogger<VideosController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get paginated video feed
    /// </summary>
    [HttpGet("feed")]
    [ProducesResponseType(typeof(TiktokClone.SharedKernel.Application.PagedResult<VideoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFeed([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetVideoFeedQuery(page, pageSize);
        var result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Get video by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VideoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetVideoByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            return NotFound(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Upload a new video
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(UploadVideoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Upload([FromBody] UploadVideoCommand command)
    {
        // Get user info from JWT token
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var usernameClaim = User.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        // Set user info in command
        command.UserId = userId;
        command.Username = usernameClaim ?? "Unknown";

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        var videoId = result.Value!;
        var videoResult = await _mediator.Send(new GetVideoByIdQuery(videoId));

        if (videoResult.IsFailure)
        {
            return BadRequest(new { error = videoResult.Error });
        }

        var video = videoResult.Value!;

        return CreatedAtAction(
            nameof(GetById),
            new { id = videoId },
            new UploadVideoResponse
            {
                VideoId = video.Id,
                Title = video.Title,
                VideoUrl = video.VideoUrl,
                Status = video.Status
            });
    }

    /// <summary>
    /// Increment video view count
    /// </summary>
    [HttpPost("{id}/view")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> IncrementView(Guid id)
    {
        var command = new IncrementViewCountCommand(id);
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return NotFound(new { error = result.Error });
        }

        return Ok(new { message = "View count incremented" });
    }
}
