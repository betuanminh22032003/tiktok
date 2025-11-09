
using Interaction.Domain.Repositories;
using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Interaction.Application.Commands.UpdateComment;

public record UpdateCommentCommand : IRequest<Result<Unit>>
{
    public Guid CommentId { get; init; }
    public Guid UserId { get; init; }
    public string Content { get; init; } = string.Empty;
}

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Result<Unit>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCommentCommandHandler(
        ICommentRepository commentRepository,
        IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
        if (comment == null)
        {
            return Result.Failure<Unit>("Comment not found");
        }

        // Verify ownership
        if (comment.UserId != request.UserId)
        {
            return Result.Failure<Unit>("You can only update your own comments");
        }

        if (comment.IsDeleted)
        {
            return Result.Failure<Unit>("Cannot update deleted comment");
        }

        comment.UpdateContent(request.Content);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}
