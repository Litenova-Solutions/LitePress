using Ardalis.GuardClauses;
using LiteBus.Commands.Abstractions;

namespace LiteNova.Blog.Application.Posts.DeletePost;

public sealed class DeletePostCommandValidator : ICommandValidator<DeletePostCommand>
{
    public Task ValidateAsync(DeletePostCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command);
        return Task.CompletedTask;
    }
}
