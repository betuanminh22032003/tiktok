using MediatR;
using TiktokClone.SharedKernel.Application;
using User.Domain.Repositories;

namespace User.Application.Commands.UpdateProfile;

public record UpdateProfileCommand : IRequest<Result<Unit>>
{
    public Guid UserId { get; init; }
    public string? DisplayName { get; init; }
    public string? Bio { get; init; }
}

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<Unit>>
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileCommandHandler(
        IUserProfileRepository userProfileRepository,
        IUnitOfWork unitOfWork)
    {
        _userProfileRepository = userProfileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _userProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        
        if (profile == null)
        {
            return Result.Failure<Unit>("Profile not found");
        }

        profile.UpdateProfile(request.DisplayName, request.Bio);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}
