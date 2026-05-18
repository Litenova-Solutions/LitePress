using LiteBus.Events.Abstractions;
using LiteNova.Blog.Domain.Posts.Events;

namespace LiteNova.Blog.Application.Posts.Events.OnPostDeleted;

/// <summary>
/// Handles <see cref="PostDeletedEvent" />.
/// </summary>
public sealed class InvalidateCacheOnPostDeletedEventHandler : IEventHandler<PostDeletedEvent>
{
    public Task HandleAsync(PostDeletedEvent domainEvent, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
