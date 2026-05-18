namespace LiteNova.Blog.Api.Models.Responses;

/// <summary>Representation of a tag.</summary>
public sealed record TagResponse(Guid Id, string Name, string Slug);
