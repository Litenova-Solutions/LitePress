using LiteNova.Blog.Application.Read.Contracts.Posts.GetAllPosts;
using LiteNova.Blog.Application.Read.Contracts.Shared;

namespace LiteNova.Blog.Application.Read.Contracts.Posts.GetPublishedPosts;

public sealed record GetPublishedPostsQuery : IQuery<PagedResult<PostSummaryResult>>
{
    public required PaginationParameters Pagination { get; init; }
}
