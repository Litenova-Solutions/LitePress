using LiteNova.Blog.Application.Read.Contracts.Posts.GetAllPosts;
using LiteNova.Blog.Application.Read.Contracts.Shared;

namespace LiteNova.Blog.Application.Read.Contracts.Posts.GetPostsByTag;

public sealed record GetPostsByTagQuery : IQuery<PagedResult<PostSummaryResult>>
{
    public required string TagSlug { get; init; }
    public required PaginationParameters Pagination { get; init; }
}
