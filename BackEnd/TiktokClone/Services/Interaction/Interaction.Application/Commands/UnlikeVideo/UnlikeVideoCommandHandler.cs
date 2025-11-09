using Interaction.Domain.Repositories;
using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Interaction.Application.Commands.UnlikeVideo;

public class UnlikeVideoCommandHandler : IRequestHandler<UnlikeVideoCommand, Result<Unit>>
{
    private readonly ILikeRepository _likeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public UnlikeVideoCommandHandler(
        ILikeRepository likeRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _likeRepository = likeRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result<Unit>> Handle(UnlikeVideoCommand request, CancellationToken cancellationToken)
    {
        var like = await _likeRepository.GetByUserAndVideoAsync(request.UserId, request.VideoId, cancellationToken);

        if (like == null)
        {
            return Result.Failure<Unit>("Like not found");
        }

        // Remove like
        like.Remove();
        _likeRepository.Remove(like);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Update cache
        var cacheKey = $"video:likes:{request.VideoId}";
        await _cacheService.DecrementAsync(cacheKey, 1, cancellationToken);

        return Result.Success(Unit.Value);
    }
}
