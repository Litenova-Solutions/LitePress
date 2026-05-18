namespace LiteNova.Blog.Application.Posts.GetPublishedPosts;

/// <summary>Result of the <see cref="GetPublishedPostsQuery"/>.</summary>
public sealed record GetPublishedPostsQueryResult(IReadOnlyCollection<PostSummaryItem> Items, int TotalCount);

/// <summary>Summary of a single published blog post.</summary>
public sealed record PostSummaryItem(Guid Id, string Title, string Slug, string Excerpt, string? CoverImageUrl, string Status, DateTimeOffset? PublishedAt, int ReadingTimeMinutes, IReadOnlyCollection<string> Tags);
