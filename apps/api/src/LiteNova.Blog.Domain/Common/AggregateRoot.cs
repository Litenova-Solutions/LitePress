namespace LiteNova.Blog.Domain.Common;

public abstract class AggregateRoot : Entity
{
    private readonly List<object> _domainEvents = [];

    protected void RaiseDomainEvent(object domainEvent) => _domainEvents.Add(domainEvent);

    public IReadOnlyCollection<object> GetDomainEvents() => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();
}
