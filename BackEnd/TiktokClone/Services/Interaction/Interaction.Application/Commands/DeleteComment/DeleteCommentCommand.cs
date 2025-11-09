using Interaction.Domain.Repositories;
using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Interaction.Application.Commands.DeleteComment;

public record DeleteCommentCommand : IRequest<Result<Unit>>
{
    public Guid CommentId { get; init; }
    public Guid UserId { get; init; }
}

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Result<Unit>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public DeleteCommentCommandHandler(
        ICommentRepository commentRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result<Unit>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
        if (comment == null)
        {
            return Result.Failure<Unit>("Comment not found");
        }

        // Verify ownership
        if (comment.UserId != request.UserId)
        {
            return Result.Failure<Unit>("You can only delete your own comments");
        }

        if (comment.IsDeleted)
        {
            return Result.Failure<Unit>("Comment already deleted");
        }

        comment.Delete();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Update cache: decrement comment count
        var cacheKey = $"video:comments:{comment.VideoId}";
        await _cacheService.DecrementAsync(cacheKey, cancellationToken: cancellationToken);

        return Result.Success(Unit.Value);
    }
}
