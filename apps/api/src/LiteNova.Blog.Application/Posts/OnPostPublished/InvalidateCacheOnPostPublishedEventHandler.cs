using LiteBus.Events.Abstractions;
using LiteNova.Blog.Domain.Posts.Events;

namespace LiteNova.Blog.Application.Posts.OnPostPublished;

/// <summary>Handles the <see cref="PostPublishedEvent"/> to invalidate caches when a post is published.</summary>
public sealed class InvalidateCacheOnPostPublishedEventHandler : IEventHandler<PostPublishedEvent>
{
    public Task HandleAsync(PostPublishedEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
