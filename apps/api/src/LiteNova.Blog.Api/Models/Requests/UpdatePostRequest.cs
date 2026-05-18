namespace LiteNova.Blog.Api.Models.Requests;

/// <summary>Request body for updating an existing blog post.</summary>
public sealed record UpdatePostRequest(
    string Title,
    string Excerpt,
    string Body,
    string? CoverImageUrl,
    IReadOnlyCollection<Guid> TagIds);
