using LiteNova.LitePress.Application.Read.Contracts.Tags.GetAllTags;

namespace LiteNova.LitePress.Application.Read.Contracts.Tags.GetTagBySlug;

public sealed record GetTagBySlugQuery : IQuery<TagResult>
{
    public required string Slug { get; init; }
}
