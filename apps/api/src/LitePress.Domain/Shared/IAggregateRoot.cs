namespace LitePress.Domain.Shared;

/// <summary>
/// Non-generic marker implemented by aggregate roots so Infrastructure can inspect
/// domain events without knowing the concrete id type.
/// </summary>
public interface IAggregateRoot
{
    /// <summary>Domain events raised during the current unit of work.</summary>
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }

    /// <summary>Clears pending domain events after dispatch.</summary>
    public void ClearDomainEvents();
}
