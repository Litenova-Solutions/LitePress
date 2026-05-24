using LitePress.Application.Read.Contracts.Tags.GetAllTags;

namespace LitePress.Application.Read.Contracts.Tags.GetTagBySlug;

public sealed record GetTagBySlugQuery : IQuery<TagResult>
{
    public required string Slug { get; init; }
}
