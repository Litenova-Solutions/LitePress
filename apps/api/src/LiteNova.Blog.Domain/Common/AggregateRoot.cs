namespace LiteNova.Blog.Domain.Common;

/// <summary>
/// Base class for aggregate roots that can collect and publish domain events.
/// </summary>
public abstract class AggregateRoot : Entity
{
    private readonly List<object> _domainEvents = [];

    /// <summary>
    /// Adds a domain event to the aggregate pending events collection.
    /// </summary>
    /// <param name="domainEvent">Domain event instance.</param>
    protected void RaiseDomainEvent(object domainEvent) => _domainEvents.Add(domainEvent);

    /// <summary>
    /// Gets the pending domain events raised by the aggregate.
    /// </summary>
    /// <returns>Read-only collection of pending domain events.</returns>
    public IReadOnlyCollection<object> GetDomainEvents() => _domainEvents.AsReadOnly();

    /// <summary>
    /// Clears all pending domain events.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();
}
