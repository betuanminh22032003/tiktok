using Identity.Domain.ValueObjects;
using TiktokClone.SharedKernel.Domain;

namespace Identity.Domain.Events;

public class UserRoleChangedEvent : DomainEvent
{
    public Guid UserId { get; }
    public UserRole OldRole { get; }
    public UserRole NewRole { get; }

    public UserRoleChangedEvent(Guid userId, UserRole oldRole, UserRole newRole)
    {
        UserId = userId;
        OldRole = oldRole;
        NewRole = newRole;
    }
}
