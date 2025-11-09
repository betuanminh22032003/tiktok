using MediatR;
using TiktokClone.SharedKernel.Application;
using Video.Domain.Repositories;

namespace Video.Application.Commands.UploadVideo;

/// <summary>
/// Handler for UploadVideoCommand
/// </summary>
public class UploadVideoCommandHandler : IRequestHandler<UploadVideoCommand, Result<Guid>>
{
    private readonly IVideoRepository _videoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UploadVideoCommandHandler(
        IVideoRepository videoRepository,
        IUnitOfWork unitOfWork)
    {
        _videoRepository = videoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(UploadVideoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Create video entity
            var video = Domain.Entities.Video.Create(
                title: request.Title,
                description: request.Description,
                videoUrl: request.VideoUrl,
                thumbnailUrl: request.ThumbnailUrl,
                userId: request.UserId,
                username: request.Username,
                durationSeconds: request.DurationSeconds,
                fileSizeBytes: request.FileSizeBytes,
                format: request.Format
            );

            // Save to database
            await _videoRepository.AddAsync(video, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(video.Id);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<Guid>(ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>($"Failed to upload video: {ex.Message}");
        }
    }
}
