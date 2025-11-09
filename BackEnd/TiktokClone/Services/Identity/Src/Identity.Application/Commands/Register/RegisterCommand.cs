using MediatR;
using TiktokClone.SharedKernel.Application;

namespace Identity.Application.Commands.Register;

/// <summary>
/// Command to register a new user
/// </summary>
public class RegisterCommand : IRequest<Result<Guid>>
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
