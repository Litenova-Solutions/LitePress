using LitePress.Domain.Shared;

namespace LitePress.Infrastructure.Behaviors;

/// <summary>
/// Collects domain events from tracked aggregates and clears in-memory event lists before persistence.
/// </summary>
internal static class DomainEventPublishing
{
    internal static List<IDomainEvent> CollectAndClear(IEnumerable<IAggregateRoot> aggregates)
    {
        var events = new List<IDomainEvent>();

        foreach (var aggregate in aggregates)
        {
            events.AddRange(aggregate.DomainEvents);
            aggregate.ClearDomainEvents();
        }

        return events;
    }
}
