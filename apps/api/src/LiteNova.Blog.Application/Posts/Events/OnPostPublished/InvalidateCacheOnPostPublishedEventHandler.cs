using LiteBus.CQRS;
using LiteNova.Blog.Domain.Posts.Events;

namespace LiteNova.Blog.Application.Posts.Events.OnPostPublished;

public sealed class InvalidateCacheOnPostPublishedEventHandler : IDomainEventHandler<PostPublishedEvent>
{
    public Task HandleAsync(PostPublishedEvent domainEvent, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
