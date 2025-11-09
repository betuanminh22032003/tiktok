using MediatR;
using TiktokClone.SharedKernel.Application;
using Video.Domain.Repositories;

namespace Video.Application.Commands.IncrementViewCount;

public class IncrementViewCountCommandHandler : IRequestHandler<IncrementViewCountCommand, Result<Unit>>
{
    private readonly IVideoRepository _videoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public IncrementViewCountCommandHandler(
        IVideoRepository videoRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _videoRepository = videoRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result<Unit>> Handle(IncrementViewCountCommand request, CancellationToken cancellationToken)
    {
        var video = await _videoRepository.GetByIdAsync(request.VideoId, cancellationToken);

        if (video == null)
        {
            return Result.Failure<Unit>("Video not found");
        }

        // Increment in cache (real-time)
        var cacheKey = $"video:views:{request.VideoId}";
        await _cacheService.IncrementAsync(cacheKey, 1, cancellationToken);

        // Increment in database
        video.IncrementViewCount();
        _videoRepository.Update(video);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}
