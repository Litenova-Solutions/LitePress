using System.Reflection;
using LitePress.Application.Read.Contracts.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LitePress.Infrastructure.Persistence;

internal static class PostStatePersistence
{
    private static readonly PropertyInfo StateProperty =
        typeof(Post).GetProperty(nameof(Post.State), BindingFlags.Instance | BindingFlags.Public)!;

    internal static PostState ReadState(Post post) =>
        (PostState)StateProperty.GetValue(post)!;

    internal static void WriteState(Post post, PostState state) =>
        StateProperty.SetValue(post, state);

    internal static PostState FromColumns(
        string stateType,
        DateTimeOffset? publishedAt,
        DateTimeOffset? archivedAt) =>
        stateType switch
        {
            PostStateColumns.Draft => new DraftPostState(),
            PostStateColumns.Published when publishedAt.HasValue => new PublishedPostState(publishedAt.Value),
            PostStateColumns.Archived when archivedAt.HasValue => new ArchivedPostState(archivedAt.Value),
            _ => throw new InvalidOperationException($"Unknown post state discriminator '{stateType}'.")
        };

    internal static void ApplyColumnsFromState(EntityEntry<Post> entry)
    {
        var state = ReadState(entry.Entity);

        switch (state)
        {
            case DraftPostState:
                entry.Property(PostStateColumns.StateType).CurrentValue = PostStateColumns.Draft;
                entry.Property(PostStateColumns.PublishedAt).CurrentValue = null;
                entry.Property(PostStateColumns.ArchivedAt).CurrentValue = null;
                break;
            case PublishedPostState published:
                entry.Property(PostStateColumns.StateType).CurrentValue = PostStateColumns.Published;
                entry.Property(PostStateColumns.PublishedAt).CurrentValue = published.PublishedAt;
                entry.Property(PostStateColumns.ArchivedAt).CurrentValue = null;
                break;
            case ArchivedPostState archived:
                entry.Property(PostStateColumns.StateType).CurrentValue = PostStateColumns.Archived;
                entry.Property(PostStateColumns.PublishedAt).CurrentValue = null;
                entry.Property(PostStateColumns.ArchivedAt).CurrentValue = archived.ArchivedAt;
                break;
            default:
                throw new InvalidOperationException($"Unknown post state type '{state.GetType().Name}'.");
        }
    }

    internal static void ApplyStateFromColumns(EntityEntry<Post> entry)
    {
        var stateType = entry.Property(PostStateColumns.StateType).CurrentValue as string
            ?? PostStateColumns.Draft;
        var publishedAt = entry.Property(PostStateColumns.PublishedAt).CurrentValue as DateTimeOffset?;
        var archivedAt = entry.Property(PostStateColumns.ArchivedAt).CurrentValue as DateTimeOffset?;

        WriteState(entry.Entity, FromColumns(stateType, publishedAt, archivedAt));
    }
}

internal sealed class PostStatePersistenceInterceptor : IMaterializationInterceptor, ISaveChangesInterceptor
{
    public object InitializedInstance(MaterializationInterceptionData materializationData, object instance)
    {
        if (instance is Post post && materializationData.Context is not null)
        {
            PostStatePersistence.ApplyStateFromColumns(materializationData.Context.Entry(post));
        }

        return instance;
    }

    public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SyncTrackedPosts(eventData.Context);
        return result;
    }

    public ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        SyncTrackedPosts(eventData.Context);
        return ValueTask.FromResult(result);
    }

    private static void SyncTrackedPosts(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries<Post>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            PostStatePersistence.ApplyColumnsFromState(entry);
        }
    }
}
