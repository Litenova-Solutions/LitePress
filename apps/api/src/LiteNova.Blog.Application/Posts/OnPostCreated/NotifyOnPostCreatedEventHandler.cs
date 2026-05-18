using LiteBus.Events.Abstractions;
using LiteNova.Blog.Domain.Posts.Events;

namespace LiteNova.Blog.Application.Posts.OnPostCreated;

/// <summary>Handles the <see cref="PostCreatedEvent"/> to send notifications when a post is created.</summary>
public sealed class NotifyOnPostCreatedEventHandler : IEventHandler<PostCreatedEvent>
{
    public Task HandleAsync(PostCreatedEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
