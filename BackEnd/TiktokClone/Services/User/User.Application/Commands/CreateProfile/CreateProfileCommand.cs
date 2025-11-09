using MediatR;
using TiktokClone.SharedKernel.Application;
using User.Domain.Entities;
using User.Domain.Repositories;

namespace User.Application.Commands.CreateProfile;

public record CreateProfileCommand : IRequest<Result<Guid>>
{
    public Guid UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string? DisplayName { get; init; }
}

public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, Result<Guid>>
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProfileCommandHandler(
        IUserProfileRepository userProfileRepository,
        IUnitOfWork unitOfWork)
    {
        _userProfileRepository = userProfileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        // Check if profile already exists
        var existing = await _userProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (existing != null)
        {
            return Result.Failure<Guid>("Profile already exists for this user");
        }

        var profile = UserProfile.Create(request.UserId, request.Username, request.DisplayName);
        
        await _userProfileRepository.AddAsync(profile, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(profile.Id);
    }
}
