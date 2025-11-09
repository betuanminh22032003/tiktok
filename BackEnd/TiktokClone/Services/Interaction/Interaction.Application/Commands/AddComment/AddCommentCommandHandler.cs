using Interaction.Domain.Entities;
using Interaction.Domain.Repositories;
using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Interaction.Application.Commands.AddComment;

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Result<Guid>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public AddCommentCommandHandler(
        ICommentRepository commentRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result<Guid>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Create comment
            var comment = Comment.Create(
                request.VideoId,
                request.UserId,
                request.Username,
                request.Content,
                request.ParentCommentId
            );

            // Save to database
            await _commentRepository.AddAsync(comment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Update cache
            var cacheKey = $"video:comments:{request.VideoId}";
            await _cacheService.IncrementAsync(cacheKey, 1, cancellationToken);

            return Result.Success(comment.Id);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<Guid>(ex.Message);
        }
    }
}
