namespace LiteNova.Blog.Api.Models.Requests;

/// <summary>Request body for creating a new blog post.</summary>
public sealed record CreatePostRequest(
    string Title,
    string Excerpt,
    string Body,
    string? CoverImageUrl,
    IReadOnlyCollection<Guid> TagIds);
