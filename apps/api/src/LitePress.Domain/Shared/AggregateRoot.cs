namespace LitePress.Domain.Shared;

/// <summary>Base class for aggregate roots. Collects domain events and owns identity.</summary>
/// <typeparam name="TId">The aggregate identifier type.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    where TId : struct
{
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <inheritdoc />
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>Records a domain event to dispatch after the transaction commits.</summary>
    /// <param name="domainEvent">The event raised by this mutation.</param>
    protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);

    /// <inheritdoc />
    public void ClearDomainEvents() =>
        _domainEvents.Clear();
}
