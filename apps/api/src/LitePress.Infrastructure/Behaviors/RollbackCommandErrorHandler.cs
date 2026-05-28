using LiteBus.Commands.Abstractions;
using LitePress.Infrastructure.Marten;

namespace LitePress.Infrastructure.Behaviors;

internal sealed class RollbackCommandErrorHandler(IMartenUnitOfWork unitOfWork) : ICommandErrorHandler<ICommand>
{
    public Task HandleErrorAsync(
        ICommand command,
        object? commandResult,
        Exception exception,
        CancellationToken cancellationToken)
    {
        unitOfWork.DiscardPending();
        throw exception;
    }
}
