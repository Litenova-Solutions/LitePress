using Ardalis.GuardClauses;
using LiteBus.CQRS;

namespace LiteNova.Blog.Application.Posts.Commands.PublishPost;

public sealed class PublishPostCommandValidator : ICommandValidator<PublishPostCommand>
{
    public Task ValidateAsync(PublishPostCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command);
        return Task.CompletedTask;
    }
}
