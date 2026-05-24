using LitePress.Domain.Shared;
using LitePress.Domain.Tags;

namespace LitePress.Domain.Posts.Events;

public sealed record PostTagAdded(PostId PostId, TagId TagId) : IDomainEvent;
