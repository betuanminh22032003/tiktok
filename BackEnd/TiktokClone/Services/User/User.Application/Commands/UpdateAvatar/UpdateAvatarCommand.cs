using MediatR;
using TiktokClone.SharedKernel.Application;
using User.Domain.Repositories;

namespace User.Application.Commands.UpdateAvatar;

public record UpdateAvatarCommand : IRequest<Result<Unit>>
{
    public Guid UserId { get; init; }
    public string AvatarUrl { get; init; } = string.Empty;
}

public class UpdateAvatarCommandHandler : IRequestHandler<UpdateAvatarCommand, Result<Unit>>
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAvatarCommandHandler(
        IUserProfileRepository userProfileRepository,
        IUnitOfWork unitOfWork)
    {
        _userProfileRepository = userProfileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
    {
        var profile = await _userProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        
        if (profile == null)
        {
            return Result.Failure<Unit>("Profile not found");
        }

        try
        {
            profile.UpdateAvatar(request.AvatarUrl);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<Unit>(ex.Message);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}
