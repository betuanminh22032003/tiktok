using Interaction.Application.DTOs;
using Interaction.Domain.Repositories;
using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Interaction.Application.Queries.GetVideoLikes;

public class GetVideoLikesQueryHandler : IRequestHandler<GetVideoLikesQuery, Result<IReadOnlyList<LikeDto>>>
{
    private readonly ILikeRepository _likeRepository;

    public GetVideoLikesQueryHandler(ILikeRepository likeRepository)
    {
        _likeRepository = likeRepository;
    }

    public async Task<Result<IReadOnlyList<LikeDto>>> Handle(GetVideoLikesQuery request, CancellationToken cancellationToken)
    {
        var likes = await _likeRepository.GetByVideoIdAsync(request.VideoId, cancellationToken);

        var likeDtos = likes.Select(like => new LikeDto
        {
            Id = like.Id,
            VideoId = like.VideoId,
            UserId = like.UserId,
            Username = like.Username,
            CreatedAt = like.CreatedAt
        }).ToList();

        return Result.Success<IReadOnlyList<LikeDto>>(likeDtos);
    }
}
