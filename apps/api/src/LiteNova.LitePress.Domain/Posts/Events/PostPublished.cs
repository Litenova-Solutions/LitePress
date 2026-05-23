using LiteNova.LitePress.Domain.Authors;
using LiteNova.LitePress.Domain.Shared;

namespace LiteNova.LitePress.Domain.Posts.Events;

public sealed record PostPublished(
    PostId PostId,
    AuthorId AuthorId,
    DateTimeOffset PublishedAt
) : IDomainEvent;
