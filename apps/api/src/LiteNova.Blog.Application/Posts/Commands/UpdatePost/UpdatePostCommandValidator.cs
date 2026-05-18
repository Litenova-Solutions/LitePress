using Ardalis.GuardClauses;
using LiteBus.CQRS;

namespace LiteNova.Blog.Application.Posts.Commands.UpdatePost;

public sealed class UpdatePostCommandValidator : ICommandValidator<UpdatePostCommand>
{
    public Task ValidateAsync(UpdatePostCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command);
        return Task.CompletedTask;
    }
}
