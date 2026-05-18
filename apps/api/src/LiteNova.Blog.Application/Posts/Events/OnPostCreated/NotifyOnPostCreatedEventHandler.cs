using LiteBus.CQRS;
using LiteNova.Blog.Domain.Posts.Events;

namespace LiteNova.Blog.Application.Posts.Events.OnPostCreated;

public sealed class NotifyOnPostCreatedEventHandler : IDomainEventHandler<PostCreatedEvent>
{
    public Task HandleAsync(PostCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
