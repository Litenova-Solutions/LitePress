using Ardalis.GuardClauses;
using LiteBus.Commands.Abstractions;

namespace LiteNova.Blog.Application.Tags.CreateTag;

public sealed class CreateTagCommandValidator : ICommandValidator<CreateTagCommand>
{
    public Task ValidateAsync(CreateTagCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command);
        return Task.CompletedTask;
    }
}
