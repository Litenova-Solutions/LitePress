using Ardalis.GuardClauses;
using LiteBus.CQRS;

namespace LiteNova.Blog.Application.Tags.Commands.CreateTag;

public sealed class CreateTagCommandValidator : ICommandValidator<CreateTagCommand>
{
    public Task ValidateAsync(CreateTagCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command);
        return Task.CompletedTask;
    }
}
