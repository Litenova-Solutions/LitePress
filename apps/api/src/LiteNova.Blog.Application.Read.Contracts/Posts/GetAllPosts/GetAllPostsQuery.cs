using LiteNova.Blog.Application.Read.Contracts.Shared;

namespace LiteNova.Blog.Application.Read.Contracts.Posts.GetAllPosts;

public sealed record GetAllPostsQuery : IQuery<PagedResult<PostSummaryResult>>
{
    public required PaginationParameters Pagination { get; init; }
}
