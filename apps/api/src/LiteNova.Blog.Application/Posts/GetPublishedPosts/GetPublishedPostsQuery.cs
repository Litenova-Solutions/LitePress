using LiteBus.Queries.Abstractions;
namespace LiteNova.Blog.Application.Posts.GetPublishedPosts;
public sealed record GetPublishedPostsQuery(int Page = 1, int PageSize = 10) : IQuery<GetPublishedPostsQueryResult>;
