using LiteNova.LitePress.Domain.Shared;

namespace LiteNova.LitePress.Domain.Tags.Events;

public sealed record TagDeleted(TagId TagId) : IDomainEvent;
