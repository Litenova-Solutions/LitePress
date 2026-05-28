using LitePress.Domain.Shared;
using Marten;

namespace LitePress.Infrastructure.Marten;

/// <summary>
/// Scoped write-side session boundary for Marten. Repositories stage changes on
/// <see cref="Session"/>; <see cref="SaveChangesCommandPostHandler"/> commits and dispatches domain events.
/// </summary>
internal interface IMartenUnitOfWork
{
    /// <summary>Marten document session used by repositories for store, update, and delete.</summary>
    public IDocumentSession Session { get; }

    /// <summary>True when at least one aggregate was tracked for domain event collection on save.</summary>
    public bool HasPendingChanges { get; }

    /// <summary>Registers an aggregate so its domain events are published after a successful save.</summary>
    public void Track(IAggregateRoot aggregate);

    /// <summary>Aggregates tracked during the current command, in undefined order.</summary>
    public IReadOnlyList<IAggregateRoot> GetTrackedAggregates();

    /// <summary>Persists pending session changes to PostgreSQL.</summary>
    public Task SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>Discards pending session changes and clears tracked aggregates (command failure path).</summary>
    public void DiscardPending();
}
