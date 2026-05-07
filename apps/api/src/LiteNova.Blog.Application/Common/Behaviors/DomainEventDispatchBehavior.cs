using LiteBus.CQRS;
using LiteNova.Blog.Application.Common.Interfaces;
using LiteNova.Blog.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Application.Common.Behaviors;

public sealed class DomainEventDispatchBehavior(IBlogDbContext dbContext, IMessageBus messageBus)
{
    public async Task DispatchAsync(CancellationToken cancellationToken)
    {
        if (dbContext is not DbContext efDbContext) return;
        var aggregates = efDbContext.ChangeTracker.Entries<AggregateRoot>().Select(e => e.Entity).ToArray();
        var events = aggregates.SelectMany(a => a.GetDomainEvents()).ToArray();
        foreach (var aggregate in aggregates) aggregate.ClearDomainEvents();
        foreach (var domainEvent in events) await messageBus.PublishAsync(domainEvent, cancellationToken);
    }
}
