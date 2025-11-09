namespace TiktokClone.SharedKernel.Domain;

/// <summary>
/// Marker interface for aggregate roots in DDD
/// </summary>
public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
