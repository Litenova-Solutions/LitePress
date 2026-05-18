namespace LiteNova.Blog.Api.Models.Requests;

/// <summary>
/// Request payload used to update a post.
/// </summary>
public sealed record UpdatePostRequest
{
    /// <summary>
    /// Post title.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Short post excerpt.
    /// </summary>
    public required string Excerpt { get; init; }

    /// <summary>
    /// TipTap JSON content.
    /// </summary>
    public required string Body { get; init; }

    /// <summary>
    /// Optional cover image URL.
    /// </summary>
    public string? CoverImageUrl { get; init; }

    /// <summary>
    /// Tag identifiers to associate with the post.
    /// </summary>
    public required IReadOnlyCollection<Guid> TagIds { get; init; }
}
