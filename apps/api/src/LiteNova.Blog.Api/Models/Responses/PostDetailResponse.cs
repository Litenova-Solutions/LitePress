namespace LiteNova.Blog.Api.Models.Responses;

/// <summary>Full detail representation of a blog post including the body content.</summary>
public sealed record PostDetailResponse(
    Guid Id,
    string Title,
    string Slug,
    string Excerpt,
    string? CoverImageUrl,
    string Status,
    DateTimeOffset? PublishedAt,
    int ReadingTimeMinutes,
    IReadOnlyCollection<string> Tags,
    string Body) : PostSummaryResponse(Id, Title, Slug, Excerpt, CoverImageUrl, Status, PublishedAt, ReadingTimeMinutes, Tags);
