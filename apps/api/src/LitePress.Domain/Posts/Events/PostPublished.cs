using LitePress.Domain.Authors;
using LitePress.Domain.Shared;

namespace LitePress.Domain.Posts.Events;

public sealed record PostPublished(
    PostId PostId,
    AuthorId AuthorId,
    DateTimeOffset PublishedAt
) : IDomainEvent;
