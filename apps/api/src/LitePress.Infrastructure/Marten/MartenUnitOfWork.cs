using LitePress.Domain.Shared;
using Marten;

namespace LitePress.Infrastructure.Marten;

/// <summary>
/// Default <see cref="IMartenUnitOfWork"/> implementation. One instance per HTTP request / command scope.
/// </summary>
internal sealed class MartenUnitOfWork(IDocumentSession session) : IMartenUnitOfWork
{
    private readonly HashSet<IAggregateRoot> _trackedAggregates = [];

    /// <inheritdoc />
    public IDocumentSession Session => session;

    /// <inheritdoc />
    public bool HasPendingChanges => _trackedAggregates.Count > 0;

    /// <inheritdoc />
    public void Track(IAggregateRoot aggregate) => _trackedAggregates.Add(aggregate);

    /// <inheritdoc />
    public IReadOnlyList<IAggregateRoot> GetTrackedAggregates() => [.. _trackedAggregates];

    /// <inheritdoc />
    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        session.SaveChangesAsync(cancellationToken);

    /// <inheritdoc />
    public void DiscardPending()
    {
        session.EjectAllPendingChanges();
        _trackedAggregates.Clear();
    }
}
