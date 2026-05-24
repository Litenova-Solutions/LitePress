using LitePress.Domain.Authors;
using LitePress.Domain.Shared;
using LitePress.Domain.Tags;

namespace LitePress.Domain.Posts.Events;

public sealed record PostCreated(
    PostId PostId,
    AuthorId AuthorId,
    PostTitle Title,
    PostSlug Slug,
    PostContent Content,
    PostExcerpt? Excerpt,
    PostCoverImageUrl? CoverImageUrl,
    IReadOnlyList<TagId> TagIds
) : IDomainEvent;
