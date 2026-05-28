using LitePress.Domain.Authors;
using LitePress.Domain.Posts;
using LitePress.Domain.Tags;

namespace LitePress.Application.Read.Contracts.Shared;

/// <summary>
/// Read-side database access for query handlers. Implementations live in Infrastructure.
/// </summary>
public interface IReadDatabase
{
    public Task<TResult> QueryAsync<TResult>(
        Func<IReadDatabaseContext, CancellationToken, Task<TResult>> query,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Marten-backed read context exposed to query handlers without leaking Infrastructure types.
/// </summary>
public interface IReadDatabaseContext
{
    public IQueryable<Post> Posts { get; }

    public IQueryable<Author> Authors { get; }

    public IQueryable<Tag> Tags { get; }

    public Task<IReadOnlyList<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
        where T : class;

    public Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
        where T : class;

    public Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
        where T : class;

    public Task<bool> AnyAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
        where T : class;

    public Task<T?> LoadAsync<T>(object id, CancellationToken cancellationToken = default)
        where T : class;
}
