using LiteBus.Events.Abstractions;
using LiteNova.Blog.Application.Common.Interfaces;
using LiteNova.Blog.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Application.Common.Behaviors;

public sealed class DomainEventDispatchBehavior(IBlogDbContext dbContext, IEventMediator eventMediator)
{
    public async Task DispatchAsync(CancellationToken cancellationToken)
    {
        if (dbContext is not DbContext efDbContext)
        {
            return;
        }

        var aggregates = efDbContext.ChangeTracker.Entries<AggregateRoot>().Select(entry => entry.Entity).ToArray();
        var domainEvents = aggregates.SelectMany(aggregate => aggregate.GetDomainEvents()).ToArray();

        foreach (var aggregate in aggregates)
        {
            aggregate.ClearDomainEvents();
        }

        foreach (var domainEvent in domainEvents)
        {
            if (domainEvent is IEvent @event)
            {
                await eventMediator.PublishAsync(@event, cancellationToken);
            }
        }
    }
}
