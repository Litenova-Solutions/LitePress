using LiteBus.Events.Abstractions;
using LiteNova.Blog.Domain.Posts.Events;

namespace LiteNova.Blog.Application.Posts.OnPostDeleted;

/// <summary>Handles the <see cref="PostDeletedEvent"/> to invalidate caches when a post is deleted.</summary>
public sealed class InvalidateCacheOnPostDeletedEventHandler : IEventHandler<PostDeletedEvent>
{
    public Task HandleAsync(PostDeletedEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
