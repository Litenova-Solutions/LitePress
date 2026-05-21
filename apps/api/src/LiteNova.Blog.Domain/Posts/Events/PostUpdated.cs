using LiteNova.Blog.Domain.Shared;

namespace LiteNova.Blog.Domain.Posts.Events;

public sealed record PostUpdated(
    PostId PostId,
    PostTitle Title,
    PostSlug Slug,
    PostContent Content,
    PostExcerpt? Excerpt,
    PostCoverImageUrl? CoverImageUrl
) : IDomainEvent;
