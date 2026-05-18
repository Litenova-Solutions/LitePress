using LiteBus.Events.Abstractions;
using LiteNova.Blog.Domain.Posts.Events;

namespace LiteNova.Blog.Application.Posts.Events.OnPostPublished;

/// <summary>
/// Handles <see cref="PostPublishedEvent" />.
/// </summary>
public sealed class InvalidateCacheOnPostPublishedEventHandler : IEventHandler<PostPublishedEvent>
{
    public Task HandleAsync(PostPublishedEvent domainEvent, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
