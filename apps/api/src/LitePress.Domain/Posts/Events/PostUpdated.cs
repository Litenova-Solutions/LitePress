using LitePress.Domain.Shared;

namespace LitePress.Domain.Posts.Events;

public sealed record PostUpdated(
    PostId PostId,
    PostTitle Title,
    PostSlug Slug,
    PostContent Content,
    PostExcerpt? Excerpt,
    PostCoverImageUrl? CoverImageUrl
) : IDomainEvent;
