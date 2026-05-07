using Ardalis.GuardClauses;
using LiteBus.CQRS;

namespace LiteNova.Blog.Application.Posts.Commands.DeletePost;

public sealed class DeletePostCommandValidator : ICommandValidator<DeletePostCommand>
{
    public Task ValidateAsync(DeletePostCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command);
        return Task.CompletedTask;
    }
}
