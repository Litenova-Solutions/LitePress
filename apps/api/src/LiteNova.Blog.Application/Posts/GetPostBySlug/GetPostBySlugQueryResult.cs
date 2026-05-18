namespace LiteNova.Blog.Application.Posts.GetPostBySlug;

/// <summary>Result of the <see cref="GetPostBySlugQuery"/>.</summary>
public sealed record GetPostBySlugQueryResult(Guid Id, string Title, string Slug, string Excerpt, string? CoverImageUrl, string Status, DateTimeOffset? PublishedAt, int ReadingTimeMinutes, IReadOnlyCollection<string> Tags, string Body = "");
