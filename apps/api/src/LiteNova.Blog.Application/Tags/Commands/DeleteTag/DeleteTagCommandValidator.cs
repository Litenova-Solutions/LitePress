using Ardalis.GuardClauses;
using LiteBus.CQRS;

namespace LiteNova.Blog.Application.Tags.Commands.DeleteTag;

public sealed class DeleteTagCommandValidator : ICommandValidator<DeleteTagCommand>
{
    public Task ValidateAsync(DeleteTagCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command);
        return Task.CompletedTask;
    }
}
