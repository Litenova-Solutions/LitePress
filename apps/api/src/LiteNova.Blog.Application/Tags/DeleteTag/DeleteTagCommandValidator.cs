using Ardalis.GuardClauses;
using LiteBus.Commands.Abstractions;

namespace LiteNova.Blog.Application.Tags.DeleteTag;

public sealed class DeleteTagCommandValidator : ICommandValidator<DeleteTagCommand>
{
    public Task ValidateAsync(DeleteTagCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command);
        return Task.CompletedTask;
    }
}
