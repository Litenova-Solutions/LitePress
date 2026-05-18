using LiteBus.CQRS;
using LiteNova.Blog.Domain.Posts.Events;

namespace LiteNova.Blog.Application.Posts.Events.OnPostDeleted;

public sealed class InvalidateCacheOnPostDeletedEventHandler : IDomainEventHandler<PostDeletedEvent>
{
    public Task HandleAsync(PostDeletedEvent domainEvent, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
