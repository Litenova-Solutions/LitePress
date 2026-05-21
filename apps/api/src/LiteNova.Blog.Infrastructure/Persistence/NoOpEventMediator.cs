using LiteBus.Events.Abstractions;

namespace LiteNova.Blog.Infrastructure.Persistence;

internal sealed class NoOpEventMediator : IEventPublisher
{
    public Task PublishAsync(IEvent @event, EventMediationSettings? settings = null, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    Task IEventMediator.PublishAsync<TEvent>(TEvent @event, EventMediationSettings? settings, CancellationToken cancellationToken)
        => Task.CompletedTask;
}
