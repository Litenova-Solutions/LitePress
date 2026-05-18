using LiteBus.Queries.Abstractions;

namespace LiteNova.Blog.Application.Tags.GetAllTags;

/// <summary>Query to retrieve all tags.</summary>
public sealed record GetAllTagsQuery : IQuery<IReadOnlyCollection<GetAllTagsQueryResult>>;
