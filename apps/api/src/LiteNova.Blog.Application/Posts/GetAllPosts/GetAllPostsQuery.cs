using LiteBus.Queries.Abstractions;

namespace LiteNova.Blog.Application.Posts.GetAllPosts;

/// <summary>Query to retrieve a paginated list of all blog posts.</summary>
public sealed record GetAllPostsQuery(int Page = 1, int PageSize = 20) : IQuery<GetAllPostsQueryResult>;
