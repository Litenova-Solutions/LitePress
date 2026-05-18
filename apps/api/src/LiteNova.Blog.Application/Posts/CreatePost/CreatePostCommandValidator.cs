using Ardalis.GuardClauses;
using LiteBus.Commands.Abstractions;

namespace LiteNova.Blog.Application.Posts.CreatePost;

public sealed class CreatePostCommandValidator : ICommandValidator<CreatePostCommand>
{
    public Task ValidateAsync(CreatePostCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command);
        return Task.CompletedTask;
    }
}
