using LitePress.Application.Read.Contracts.Shared;

namespace LitePress.Application.Read.Posts;

internal static class PostReadSupport
{
    internal static async Task<Dictionary<Guid, string>> LoadAuthorNamesAsync(
        IReadDatabaseContext context,
        IReadOnlyList<Guid> authorIds,
        CancellationToken cancellationToken)
    {
        if (authorIds.Count == 0)
        {
            return [];
        }

        var authors = new Dictionary<Guid, string>();
        foreach (var authorId in authorIds.Distinct())
        {
            var author = await context.LoadAsync<Author>(new AuthorId(authorId), cancellationToken);
            if (author is not null)
            {
                authors[authorId] = author.DisplayName;
            }
        }

        return authors;
    }

    internal static async Task<Dictionary<Guid, TagSummaryResult>> LoadTagSummariesAsync(
        IReadDatabaseContext context,
        IReadOnlyList<Guid> tagIds,
        CancellationToken cancellationToken)
    {
        if (tagIds.Count == 0)
        {
            return [];
        }

        var tags = new Dictionary<Guid, TagSummaryResult>();
        foreach (var tagId in tagIds.Distinct())
        {
            var tag = await context.LoadAsync<Tag>(new TagId(tagId), cancellationToken);
            if (tag is not null)
            {
                tags[tagId] = new TagSummaryResult(tagId, tag.Name.Value, tag.Slug.Value);
            }
        }

        return tags;
    }
}
