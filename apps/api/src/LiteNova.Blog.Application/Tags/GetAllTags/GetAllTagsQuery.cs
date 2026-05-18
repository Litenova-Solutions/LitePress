using LiteBus.Queries.Abstractions;
namespace LiteNova.Blog.Application.Tags.GetAllTags;
public sealed record GetAllTagsQuery : IQuery<IReadOnlyCollection<GetAllTagsQueryResult>>;
