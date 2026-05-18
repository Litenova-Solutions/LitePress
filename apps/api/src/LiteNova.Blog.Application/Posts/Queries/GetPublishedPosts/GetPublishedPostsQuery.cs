using LiteBus.CQRS;
namespace LiteNova.Blog.Application.Posts.Queries.GetPublishedPosts;
public sealed record GetPublishedPostsQuery(int Page = 1, int PageSize = 10) : IQuery<GetPublishedPostsQueryResult>;
