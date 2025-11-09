using Identity.Application.DTOs;
using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Identity.Application.Commands.Login;

/// <summary>
/// Command to login a user
/// </summary>
public class LoginCommand : IRequest<Result<LoginResponse>>
{
    public string EmailOrUsername { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
