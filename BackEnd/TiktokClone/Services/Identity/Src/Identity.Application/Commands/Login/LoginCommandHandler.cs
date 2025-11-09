using Identity.Application.DTOs;
using Identity.Domain.Repositories;
using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Identity.Application.Commands.Login;

/// <summary>
/// Handler for LoginCommand
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Find user by email or username
        var user = request.EmailOrUsername.Contains('@')
            ? await _userRepository.GetByEmailAsync(request.EmailOrUsername, cancellationToken)
            : await _userRepository.GetByUsernameAsync(request.EmailOrUsername, cancellationToken);

        if (user == null)
        {
            return Result.Failure<LoginResponse>("Invalid credentials");
        }

        // Verify password
        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Result.Failure<LoginResponse>("Invalid credentials");
        }

        // Check if user is active
        if (!user.IsActive)
        {
            return Result.Failure<LoginResponse>("Account is deactivated");
        }

        // Record login
        user.RecordLogin();

        // Generate tokens
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user.Id, user.Username, user.Email.Value, user.Role.ToString());
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        user.SetRefreshToken(refreshToken, refreshTokenExpiry);

        // Save changes
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new LoginResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email.Value,
            Role = user.Role.ToString(),
            AccessToken = accessToken
        });
    }
}
