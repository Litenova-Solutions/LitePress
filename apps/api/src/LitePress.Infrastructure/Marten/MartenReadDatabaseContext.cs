using LitePress.Application.Read.Contracts.Shared;
using LitePress.Domain.Authors;
using LitePress.Domain.Posts;
using LitePress.Domain.Tags;
using Marten;
using Marten.Linq;

namespace LitePress.Infrastructure.Marten;

/// <summary>
/// Marten-backed <see cref="IReadDatabaseContext"/>. Provides LINQ over stored aggregate documents
/// and async helpers that execute queries against PostgreSQL.
/// </summary>
internal sealed class MartenReadDatabaseContext(IQuerySession session) : IReadDatabaseContext
{
    /// <inheritdoc />
    public IQueryable<Post> Posts => session.Query<Post>();

    /// <inheritdoc />
    public IQueryable<Author> Authors => session.Query<Author>();

    /// <inheritdoc />
    public IQueryable<Tag> Tags => session.Query<Tag>();

    /// <inheritdoc />
    public async Task<IReadOnlyList<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
        where T : class =>
        await query.ToListAsync(cancellationToken);

    /// <inheritdoc />
    public Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
        where T : class =>
        query.CountAsync(cancellationToken);

    /// <inheritdoc />
    public Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
        where T : class =>
        query.FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public Task<bool> AnyAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
        where T : class =>
        query.AnyAsync(cancellationToken);

    /// <inheritdoc />
    public Task<T?> LoadAsync<T>(object id, CancellationToken cancellationToken = default)
        where T : class =>
        session.LoadAsync<T>(id, cancellationToken);
}
