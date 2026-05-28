using LitePress.Application.Read.Contracts.Shared;
using Marten;

namespace LitePress.Infrastructure.Marten;

/// <summary>
/// Infrastructure implementation of <see cref="IReadDatabase"/>. Wraps a scoped <see cref="IQuerySession"/>
/// and exposes <see cref="MartenReadDatabaseContext"/> to query handlers without referencing Marten in Application.Read.
/// </summary>
internal sealed class MartenReadDatabase(IQuerySession session) : IReadDatabase
{
    /// <inheritdoc />
    public Task<TResult> QueryAsync<TResult>(
        Func<IReadDatabaseContext, CancellationToken, Task<TResult>> query,
        CancellationToken cancellationToken = default) =>
        query(new MartenReadDatabaseContext(session), cancellationToken);
}
