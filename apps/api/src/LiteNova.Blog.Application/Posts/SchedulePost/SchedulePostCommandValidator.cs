using Ardalis.GuardClauses;
using LiteBus.Commands.Abstractions;

namespace LiteNova.Blog.Application.Posts.SchedulePost;

public sealed class SchedulePostCommandValidator : ICommandValidator<SchedulePostCommand>
{
    public Task ValidateAsync(SchedulePostCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command);
        return Task.CompletedTask;
    }
}
