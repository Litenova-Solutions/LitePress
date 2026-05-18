using Ardalis.GuardClauses;
using LiteBus.CQRS;

namespace LiteNova.Blog.Application.Posts.Commands.SchedulePost;

public sealed class SchedulePostCommandValidator : ICommandValidator<SchedulePostCommand>
{
    public Task ValidateAsync(SchedulePostCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command);
        return Task.CompletedTask;
    }
}
