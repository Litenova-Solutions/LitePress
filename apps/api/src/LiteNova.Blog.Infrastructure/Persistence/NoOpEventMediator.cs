using LiteBus.Events.Abstractions;

namespace LiteNova.Blog.Infrastructure.Persistence;

internal sealed class NoOpEventMediator : IEventMediator
{
    public Task PublishAsync(IEvent @event, EventMediationSettings? settings = null, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task PublishAsync<TEvent>(TEvent @event, EventMediationSettings? settings = null, CancellationToken cancellationToken = default)
        where TEvent : notnull
        => Task.CompletedTask;
}
