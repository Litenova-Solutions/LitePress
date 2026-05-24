using LitePress.Domain.Shared;

namespace LitePress.Domain.Tags.Events;

public sealed record TagDeleted(TagId TagId) : IDomainEvent;
