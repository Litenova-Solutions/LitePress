using Ardalis.GuardClauses;
using LiteBus.CQRS;

namespace LiteNova.Blog.Application.Posts.Commands.CreatePost;

public sealed class CreatePostCommandValidator : ICommandValidator<CreatePostCommand>
{
    public Task ValidateAsync(CreatePostCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command);
        return Task.CompletedTask;
    }
}
