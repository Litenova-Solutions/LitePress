using LiteBus.CQRS;
namespace LiteNova.Blog.Application.Posts.Queries.GetAllPosts;
public sealed record GetAllPostsQuery(int Page = 1, int PageSize = 20) : IQuery<GetAllPostsQueryResult>;
