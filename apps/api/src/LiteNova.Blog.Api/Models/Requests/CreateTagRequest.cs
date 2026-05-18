namespace LiteNova.Blog.Api.Models.Requests;

/// <summary>
/// Request payload used to create a tag.
/// </summary>
public sealed record CreateTagRequest
{
    /// <summary>
    /// Tag display name.
    /// </summary>
    public required string Name { get; init; }
}
