namespace LitePress.Domain.Common;

/// <summary>
/// Base class for aggregate roots in the domain model.
/// An aggregate root is the entry point to a cluster of domain objects and ensures consistency boundaries.
/// </summary>
public abstract class AggregateRoot : Entity
{
    private readonly List<object> _domainEvents = [];

    /// <summary>Raises a domain event to be dispatched after the current operation completes.</summary>
    /// <param name="domainEvent">The domain event to raise.</param>
    protected void RaiseDomainEvent(object domainEvent) => _domainEvents.Add(domainEvent);

    /// <summary>Returns all domain events raised by this aggregate root since last cleared.</summary>
    public IReadOnlyCollection<object> GetDomainEvents() => _domainEvents.AsReadOnly();

    /// <summary>Clears all pending domain events after they have been dispatched.</summary>
    public void ClearDomainEvents() => _domainEvents.Clear();
}
