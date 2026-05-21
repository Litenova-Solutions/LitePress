using LiteNova.Blog.Domain.Posts.Events;
using Microsoft.Extensions.Logging;

namespace LiteNova.Blog.Application.Reactions.Posts.OnPostPublished;

internal sealed class LogOnPostPublishedEventHandler : IEventHandler<PostPublished>
{
    private readonly ILogger<LogOnPostPublishedEventHandler> _logger;

    public LogOnPostPublishedEventHandler(ILogger<LogOnPostPublishedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(PostPublished @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Post {PostId} published by author {AuthorId} at {PublishedAt}",
            @event.PostId.Value,
            @event.AuthorId.Value,
            @event.PublishedAt);

        return Task.CompletedTask;
    }
}
