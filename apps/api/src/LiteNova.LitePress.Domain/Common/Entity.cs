namespace LiteNova.LitePress.Domain.Common;

/// <summary>Base class for all domain entities. Provides a unique identifier.</summary>
public abstract class Entity
{
    /// <summary>The unique identifier of this entity.</summary>
    public Guid Id { get; protected set; } = Guid.NewGuid();
}
