using LitePress.Application.Read.Contracts.Shared;

namespace LitePress.Application.Read.Contracts.Posts.GetPostById;

public sealed record PostDetailResult(
    Guid PostId,
    string Title,
    string Slug,
    string Content,
    string? Excerpt,
    string? CoverImageUrl,
    string AuthorDisplayName,
    string PostState,
    DateTimeOffset CreatedAt,
    DateTimeOffset? PublishedAt,
    IReadOnlyList<TagSummaryResult> Tags
);
