using TiktokClone.SharedKernel.Domain;

namespace Identity.Domain.Events;

/// <summary>
/// Domain event raised when a user registers
/// </summary>
public class UserRegisteredEvent : DomainEvent
{
    public Guid UserId { get; }
    public string Email { get; }
    public string Username { get; }

    public UserRegisteredEvent(Guid userId, string email, string username)
    {
        UserId = userId;
        Email = email;
        Username = username;
    }
}
