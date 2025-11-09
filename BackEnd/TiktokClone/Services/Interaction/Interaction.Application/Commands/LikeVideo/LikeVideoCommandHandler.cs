using Interaction.Domain.Entities;
using Interaction.Domain.Repositories;
using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Interaction.Application.Commands.LikeVideo;

public class LikeVideoCommandHandler : IRequestHandler<LikeVideoCommand, Result<Guid>>
{
    private readonly ILikeRepository _likeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public LikeVideoCommandHandler(
        ILikeRepository likeRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _likeRepository = likeRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result<Guid>> Handle(LikeVideoCommand request, CancellationToken cancellationToken)
    {
        // Check if already liked
        if (await _likeRepository.ExistsAsync(request.UserId, request.VideoId, cancellationToken))
        {
            return Result.Failure<Guid>("Video already liked by user");
        }

        // Create like
        var like = Like.Create(request.VideoId, request.UserId, request.Username);

        // Save to database
        await _likeRepository.AddAsync(like, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Update cache
        var cacheKey = $"video:likes:{request.VideoId}";
        await _cacheService.IncrementAsync(cacheKey, 1, cancellationToken);

        return Result.Success(like.Id);
    }
}
