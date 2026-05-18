namespace LiteNova.Blog.Api.Models.Responses;

/// <summary>
/// Response payload representing a summarized post.
/// </summary>
public record PostSummaryResponse
{
    /// <summary>
    /// Post identifier.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Post title.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// URL slug.
    /// </summary>
    public required string Slug { get; init; }

    /// <summary>
    /// Short post excerpt.
    /// </summary>
    public required string Excerpt { get; init; }

    /// <summary>
    /// Optional cover image URL.
    /// </summary>
    public string? CoverImageUrl { get; init; }

    /// <summary>
    /// Current publication status.
    /// </summary>
    public required string Status { get; init; }

    /// <summary>
    /// Publication date when published.
    /// </summary>
    public DateTimeOffset? PublishedAt { get; init; }

    /// <summary>
    /// Estimated reading time in minutes.
    /// </summary>
    public required int ReadingTimeMinutes { get; init; }

    /// <summary>
    /// Associated tag names.
    /// </summary>
    public required IReadOnlyCollection<string> Tags { get; init; }
}
