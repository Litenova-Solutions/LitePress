namespace LiteNova.Blog.Application.Posts.GetAllPosts;

/// <summary>Result of the <see cref="GetAllPostsQuery"/>.</summary>
public sealed record GetAllPostsQueryResult(IReadOnlyCollection<PostSummaryItem> Items, int TotalCount);

/// <summary>Summary of a single blog post.</summary>
public sealed record PostSummaryItem(Guid Id, string Title, string Slug, string Excerpt, string? CoverImageUrl, string Status, DateTimeOffset? PublishedAt, int ReadingTimeMinutes, IReadOnlyCollection<string> Tags);
