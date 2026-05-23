using LiteNova.LitePress.Application.Read.Contracts.Tags.GetAllTags;

namespace LiteNova.LitePress.Application.Read.Contracts.Tags.GetAllTags;

public sealed record GetAllTagsQuery : IQuery<IReadOnlyList<TagResult>>;
