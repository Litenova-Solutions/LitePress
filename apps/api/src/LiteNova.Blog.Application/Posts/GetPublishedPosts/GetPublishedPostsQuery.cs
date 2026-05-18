using LiteBus.Queries.Abstractions;

namespace LiteNova.Blog.Application.Posts.GetPublishedPosts;

/// <summary>Query to retrieve a paginated list of published blog posts.</summary>
public sealed record GetPublishedPostsQuery(int Page = 1, int PageSize = 10) : IQuery<GetPublishedPostsQueryResult>;
