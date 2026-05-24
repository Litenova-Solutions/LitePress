using LitePress.Application.Read.Contracts.Tags.GetAllTags;

namespace LitePress.Application.Read.Contracts.Tags.GetAllTags;

public sealed record GetAllTagsQuery : IQuery<IReadOnlyList<TagResult>>;
