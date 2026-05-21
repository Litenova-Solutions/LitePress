namespace LiteNova.Blog.Domain.Shared;

public abstract class AggregateRoot<TId> : IAggregateRoot
    where TId : struct
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public TId Id { get; protected set; }

    protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);

    public IReadOnlyList<IDomainEvent> GetDomainEvents() =>
        _domainEvents.AsReadOnly();

    public void ClearDomainEvents() =>
        _domainEvents.Clear();
}
