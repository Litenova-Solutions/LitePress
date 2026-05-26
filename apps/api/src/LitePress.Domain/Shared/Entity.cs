namespace LitePress.Domain.Shared;

/// <summary>Base type for domain entities identified by a strongly-typed id.</summary>
/// <typeparam name="TId">The entity identifier type.</typeparam>
public abstract class Entity<TId>
    where TId : struct
{
    /// <summary>The unique identifier of this entity.</summary>
    public TId Id { get; protected set; }
}
