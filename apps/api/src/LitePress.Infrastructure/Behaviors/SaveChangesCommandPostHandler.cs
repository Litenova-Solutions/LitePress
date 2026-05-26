using LiteBus.Commands.Abstractions;
using LiteBus.Events.Abstractions;
using LitePress.Domain.Shared;
using LitePress.Infrastructure.Persistence;

namespace LitePress.Infrastructure.Behaviors;

internal sealed class SaveChangesCommandPostHandler : ICommandPostHandler<ICommand>
{
    private readonly LitePressDbContext _context;
    private readonly IEventMediator _eventMediator;

    public SaveChangesCommandPostHandler(
        LitePressDbContext context,
        IEventMediator eventMediator)
    {
        _context = context;
        _eventMediator = eventMediator;
    }

    public async Task PostHandleAsync(
        ICommand command,
        object? result,
        CancellationToken cancellationToken)
    {
        var aggregates = _context.ChangeTracker.Entries<IAggregateRoot>()
            .Select(entry => entry.Entity)
            .ToList();

        var domainEvents = aggregates
            .SelectMany(aggregate => aggregate.DomainEvents)
            .ToList();

        foreach (var aggregate in aggregates)
        {
            aggregate.ClearDomainEvents();
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _context.Database.CommitTransactionAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _eventMediator.PublishAsync(domainEvent, cancellationToken: cancellationToken);
        }
    }
}
