namespace LiteNova.Blog.Api.Models.Responses;

/// <summary>Summary representation of a blog post.</summary>
public record PostSummaryResponse(
    Guid Id,
    string Title,
    string Slug,
    string Excerpt,
    string? CoverImageUrl,
    string Status,
    DateTimeOffset? PublishedAt,
    int ReadingTimeMinutes,
    IReadOnlyCollection<string> Tags);
