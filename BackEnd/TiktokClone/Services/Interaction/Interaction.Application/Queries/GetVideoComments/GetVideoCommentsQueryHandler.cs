using Interaction.Application.DTOs;
using Interaction.Domain.Repositories;
using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Interaction.Application.Queries.GetVideoComments;

public class GetVideoCommentsQueryHandler : IRequestHandler<GetVideoCommentsQuery, Result<IReadOnlyList<CommentDto>>>
{
    private readonly ICommentRepository _commentRepository;

    public GetVideoCommentsQueryHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<Result<IReadOnlyList<CommentDto>>> Handle(GetVideoCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = await _commentRepository.GetByVideoIdAsync(request.VideoId, cancellationToken);

        var commentDtos = comments.Select(comment => new CommentDto
        {
            Id = comment.Id,
            VideoId = comment.VideoId,
            UserId = comment.UserId,
            Username = comment.Username,
            Content = comment.Content,
            ParentCommentId = comment.ParentCommentId,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt
        }).ToList();

        return Result.Success<IReadOnlyList<CommentDto>>(commentDtos);
    }
}
