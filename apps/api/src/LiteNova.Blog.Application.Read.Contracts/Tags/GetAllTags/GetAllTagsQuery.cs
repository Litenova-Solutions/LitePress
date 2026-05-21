using LiteNova.Blog.Application.Read.Contracts.Tags.GetAllTags;

namespace LiteNova.Blog.Application.Read.Contracts.Tags.GetAllTags;

public sealed record GetAllTagsQuery : IQuery<IReadOnlyList<TagResult>>;
