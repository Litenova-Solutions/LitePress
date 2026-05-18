namespace LiteNova.Blog.Application.Tags.GetAllTags;

/// <summary>Result representing a single tag.</summary>
public sealed record GetAllTagsQueryResult(Guid Id, string Name, string Slug);
