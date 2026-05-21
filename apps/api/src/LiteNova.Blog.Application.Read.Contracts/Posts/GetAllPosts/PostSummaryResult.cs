using LiteNova.Blog.Application.Read.Contracts.Shared;

namespace LiteNova.Blog.Application.Read.Contracts.Posts.GetAllPosts;

public sealed record PostSummaryResult(
    Guid PostId,
    string Title,
    string Slug,
    string? Excerpt,
    string? CoverImageUrl,
    string AuthorDisplayName,
    string PostState,
    DateTimeOffset CreatedAt,
    DateTimeOffset? PublishedAt,
    IReadOnlyList<TagSummaryResult> Tags
);
