using TiktokClone.SharedKernel.Domain;

namespace Identity.Domain.Events;

public class UserLoggedInEvent : DomainEvent
{
    public Guid UserId { get; }
    public string Username { get; }

    public UserLoggedInEvent(Guid userId, string username)
    {
        UserId = userId;
        Username = username;
    }
}
