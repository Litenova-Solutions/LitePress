using LiteBus.Events.Abstractions;
using LiteNova.Blog.Domain.Posts.Events;

namespace LiteNova.Blog.Application.Posts.Events.OnPostCreated;

/// <summary>
/// Handles <see cref="PostCreatedEvent" />.
/// </summary>
public sealed class NotifyOnPostCreatedEventHandler : IEventHandler<PostCreatedEvent>
{
    public Task HandleAsync(PostCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
