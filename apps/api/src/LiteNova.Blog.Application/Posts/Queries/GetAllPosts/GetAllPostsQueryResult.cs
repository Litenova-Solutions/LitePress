namespace LiteNova.Blog.Application.Posts.Queries.GetAllPosts;
public sealed record GetAllPostsQueryResult(IReadOnlyCollection<PostSummaryItem> Items, int TotalCount);
public sealed record PostSummaryItem(Guid Id, string Title, string Slug, string Excerpt, string? CoverImageUrl, string Status, DateTimeOffset? PublishedAt, int ReadingTimeMinutes, IReadOnlyCollection<string> Tags);
