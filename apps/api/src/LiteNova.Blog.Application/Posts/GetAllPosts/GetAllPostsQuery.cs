using LiteBus.Queries.Abstractions;
namespace LiteNova.Blog.Application.Posts.GetAllPosts;
public sealed record GetAllPostsQuery(int Page = 1, int PageSize = 20) : IQuery<GetAllPostsQueryResult>;
