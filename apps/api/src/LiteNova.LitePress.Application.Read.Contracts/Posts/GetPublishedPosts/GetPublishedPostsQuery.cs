using LiteNova.LitePress.Application.Read.Contracts.Posts.GetAllPosts;
using LiteNova.LitePress.Application.Read.Contracts.Shared;

namespace LiteNova.LitePress.Application.Read.Contracts.Posts.GetPublishedPosts;

public sealed record GetPublishedPostsQuery : IQuery<PagedResult<PostSummaryResult>>
{
    public required PaginationParameters Pagination { get; init; }
}
