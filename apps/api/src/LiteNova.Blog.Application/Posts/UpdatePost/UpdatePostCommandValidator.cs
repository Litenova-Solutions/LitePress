using Ardalis.GuardClauses;
using LiteBus.Commands.Abstractions;

namespace LiteNova.Blog.Application.Posts.UpdatePost;

public sealed class UpdatePostCommandValidator : ICommandValidator<UpdatePostCommand>
{
    public Task ValidateAsync(UpdatePostCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command);
        return Task.CompletedTask;
    }
}
