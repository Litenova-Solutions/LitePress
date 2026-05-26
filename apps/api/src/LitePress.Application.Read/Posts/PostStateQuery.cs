using LitePress.Application.Read.Contracts.Posts;
using Microsoft.EntityFrameworkCore;

namespace LitePress.Application.Read.Posts;

internal static class PostStateQuery
{
    internal static IQueryable<Post> WherePublished(IQueryable<Post> query) =>
        query.Where(p => EF.Property<string>(p, PostStateColumns.StateType) == PostStateColumns.Published);

    internal static IOrderedQueryable<Post> OrderByPublishedAtDescending(IQueryable<Post> query) =>
        query.OrderByDescending(p => EF.Property<DateTimeOffset>(p, PostStateColumns.PublishedAt));

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
}
