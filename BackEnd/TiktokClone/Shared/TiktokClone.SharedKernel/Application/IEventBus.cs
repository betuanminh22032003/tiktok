namespace TiktokClone.SharedKernel.Application;

/// <summary>
/// Interface for event bus (Redis Pub/Sub, RabbitMQ, etc.)
/// </summary>
public interface IEventBus
{
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class;
    Task SubscribeAsync<T>(Func<T, Task> handler, CancellationToken cancellationToken = default) where T : class;
}
