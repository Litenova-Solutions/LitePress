using LiteBus.Commands.Abstractions;
using LiteBus.Events.Abstractions;
using LitePress.Infrastructure.Marten;

namespace LitePress.Infrastructure.Behaviors;

internal sealed class SaveChangesCommandPostHandler(
    IMartenUnitOfWork unitOfWork,
    IEventMediator eventMediator) : ICommandPostHandler<ICommand>
{
    public async Task PostHandleAsync(
        ICommand command,
        object? result,
        CancellationToken cancellationToken)
    {
        if (!unitOfWork.HasPendingChanges)
        {
            return;
        }

        var domainEvents = DomainEventPublishing.CollectAndClear(unitOfWork.GetTrackedAggregates());

        await unitOfWork.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await eventMediator.PublishAsync(domainEvent, cancellationToken: cancellationToken);
        }
    }
}
