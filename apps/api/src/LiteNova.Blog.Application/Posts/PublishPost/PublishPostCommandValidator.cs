using Ardalis.GuardClauses;
using LiteBus.Commands.Abstractions;

namespace LiteNova.Blog.Application.Posts.PublishPost;

public sealed class PublishPostCommandValidator : ICommandValidator<PublishPostCommand>
{
    public Task ValidateAsync(PublishPostCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command);
        return Task.CompletedTask;
    }
}
