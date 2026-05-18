namespace LiteNova.Blog.Api.Models.Responses;

/// <summary>
/// Response payload representing a tag.
/// </summary>
public sealed record TagResponse
{
    /// <summary>
    /// Tag identifier.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Tag display name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// URL slug.
    /// </summary>
    public required string Slug { get; init; }
}
