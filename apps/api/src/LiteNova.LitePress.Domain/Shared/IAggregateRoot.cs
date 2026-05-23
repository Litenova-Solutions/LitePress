namespace LiteNova.LitePress.Domain.Shared;

public interface IAggregateRoot
{
    public IReadOnlyList<IDomainEvent> GetDomainEvents();
    public void ClearDomainEvents();
}
