using LitePress.Domain.Posts.Events;
using Microsoft.Extensions.Logging;

namespace LitePress.Application.Reactions.Posts.OnPostCreated;

internal sealed class LogOnPostCreatedEventHandler : IEventHandler<PostCreated>
{
    private readonly ILogger<LogOnPostCreatedEventHandler> _logger;

    public LogOnPostCreatedEventHandler(ILogger<LogOnPostCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(PostCreated @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Post {PostId} created with slug '{Slug}' by author {AuthorId}",
            @event.PostId.Value,
            @event.Slug.Value,
            @event.AuthorId.Value);

        return Task.CompletedTask;
    }
}
