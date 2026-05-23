using LiteNova.LitePress.Domain.Shared;
using LiteNova.LitePress.Domain.Tags;

namespace LiteNova.LitePress.Domain.Posts.Events;

public sealed record PostTagAdded(PostId PostId, TagId TagId) : IDomainEvent;
