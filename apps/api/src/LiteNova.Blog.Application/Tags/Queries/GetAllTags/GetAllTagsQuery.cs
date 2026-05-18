using LiteBus.CQRS;
namespace LiteNova.Blog.Application.Tags.Queries.GetAllTags;
public sealed record GetAllTagsQuery : IQuery<IReadOnlyCollection<GetAllTagsQueryResult>>;
