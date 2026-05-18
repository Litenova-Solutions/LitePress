namespace LiteNova.Blog.Api.Models.Requests;
public sealed class UpdatePostRequest { public required string Title { get; init; } public required string Excerpt { get; init; } public required string Body { get; init; } public string? CoverImageUrl { get; init; } public required IReadOnlyCollection<Guid> TagIds { get; init; } }
